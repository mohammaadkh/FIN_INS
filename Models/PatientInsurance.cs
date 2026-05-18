using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIN_INS.Models
{
   

    public class PatientInsurance
    {
        [Key]
        public int InsuranceID { get; set; }

        [Required]
        public string PatientID { get; set; }
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; }

        [Required]
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public InsuranceCompany InsuranceCompany { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CoverageLimit { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalanceUsed { get; set; }  

        public bool IsActive { get; set; } = true;  
    }
}
