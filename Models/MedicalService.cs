using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FIN_INS.Models
{
    

    public class MedicalService
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(150)]
        public string ServiceName { get; set; }  

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ServicePrice { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }  
    }
}
