using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIN_INS.Models;
using FIN_INS.Data;

namespace FIN_INS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public InvoicesController(HospitalDbContext context)
        {
            _context = context;
        }

        [HttpPost("add-service")]
        public async Task<IActionResult> AddServiceToInvoice([FromBody] InvoiceServiceInsertionDto dto)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID);

            if (invoice == null)
                return NotFound(new { message = "Invoice not found." });

            if (invoice.IsClosed)
                return BadRequest(new { message = "Cannot add services. This invoice is already closed." });

            var service = await _context.MedicalServices
                .FirstOrDefaultAsync(s => s.ServiceID == dto.ServiceID);

            if (service == null)
                return NotFound(new { message = "Medical service not found." });

            decimal totalServicePrice = service.ServicePrice * dto.Quantity;

            var newItem = new InvoiceItem
            {
                InvoiceID = dto.InvoiceID,
                ServiceID = dto.ServiceID,
                Quantity = dto.Quantity,
                TotalPrice = totalServicePrice,
                IsVerified = true,
                CoverageStatus = "Pending"
            };

            _context.InvoiceItems.Add(newItem);
            invoice.TotalAmount += totalServicePrice;

            await RecalculateInsuranceAndPatientShares(invoice);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service added successfully and invoice updated.", invoice });
        }

        [HttpGet("verify-insurance/{patientId}")]
        public async Task<IActionResult> VerifyInsuranceStatus(string patientId, [FromQuery] decimal estimatedCost)
        {
            var insurance = await _context.PatientInsurances
                .FirstOrDefaultAsync(pi => pi.PatientID == patientId);

            if (insurance == null)
            {
                return Ok(new { payment_status = "Not Covered", message = "Patient has no insurance record. Self-pay required." });
            }

            if (!insurance.IsActive)
            {
                return Ok(new { payment_status = "Inactive", message = "Insurance card is inactive." });
            }

            decimal remainingCoverage = insurance.CoverageLimit - insurance.CurrentBalanceUsed;
            if (estimatedCost > remainingCoverage)
            {
                return Ok(new { payment_status = "Limit Exceeded", message = "Service cost exceeds the remaining insurance coverage limit." });
            }

            return Ok(new { payment_status = "Approved", message = "Insurance is valid and covers the estimated cost." });
        }

        [HttpPut("close/{invoiceId}")]
        public async Task<IActionResult> CloseInvoice(int invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.InvoiceID == invoiceId);

            if (invoice == null)
                return NotFound(new { message = "Invoice not found." });

            if (invoice.IsClosed)
                return BadRequest(new { message = "Invoice is already closed." });

            invoice.IsClosed = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Invoice has been securely reviewed and closed by the accountant.", invoice });
        }

        private async Task RecalculateInsuranceAndPatientShares(Invoice invoice)
        {
            var insurance = await _context.PatientInsurances
                .FirstOrDefaultAsync(pi => pi.PatientID == invoice.PatientID);

            if (insurance != null && insurance.IsActive)
            {
                decimal remainingCoverage = insurance.CoverageLimit - insurance.CurrentBalanceUsed;

                if (invoice.TotalAmount > remainingCoverage)
                {
                    invoice.InsuranceCoveredAmount = remainingCoverage;
                }
                else
                {
                    invoice.InsuranceCoveredAmount = invoice.TotalAmount;
                }

                invoice.PatientPayableAmount = invoice.TotalAmount - invoice.InsuranceCoveredAmount;
            }
            else
            {
                invoice.InsuranceCoveredAmount = 0;
                invoice.PatientPayableAmount = invoice.TotalAmount;
            }
        }
    }

    public class InvoiceServiceInsertionDto
    {
        public int InvoiceID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }
}