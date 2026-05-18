using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FIN_INS.Models
{
   

    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // لأن الرقم الوطني ندخله يدوياً وليس ترقيم تلقائي
        public string NationalID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string DigitalID { get; set; }  

        [StringLength(10)]
        public string BloodType { get; set; }  

        public string Allergies { get; set; }  

         
        public ICollection<Invoice> Invoices { get; set; }
    }
}