using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FIN_INS.Models
{
   

    public class InsuranceCompany
    {
        [Key]
        public int CompanyID { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DefaultCoverageRatio { get; set; }  
    }
}
