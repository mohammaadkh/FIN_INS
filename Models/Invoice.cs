using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIN_INS.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        public string PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; } = null!;

        [Required]
        public int EncounterID { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal InsuranceCoveredAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PatientPayableAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; } = 0;  

        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash";  

        public bool IsClosed { get; set; } = false;

        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}