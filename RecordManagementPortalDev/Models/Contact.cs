using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }        
        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Company Name")]
        [Required]
        public string CompanyName { get; set; }
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }
        [DisplayName("Phone No")]        
        public string? Phone { get; set; }
        [DisplayName("Messages")]
        [Required]
        public string Messages { get; set; } 
        public DateTime Date { get; set; }
    }
}
