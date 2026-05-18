using Microsoft.EntityFrameworkCore;
using FIN_INS.Models;  

namespace FIN_INS.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

         public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<PatientInsurance> PatientInsurances { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<PatientInsurance>()
                .Property(pi => pi.CoverageLimit).HasPrecision(18, 2);

            modelBuilder.Entity<PatientInsurance>()
                .Property(pi => pi.CurrentBalanceUsed).HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount).HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.InsuranceCoveredAmount).HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.PatientPayableAmount).HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.TotalPrice).HasPrecision(18, 2);
        }
    }
}