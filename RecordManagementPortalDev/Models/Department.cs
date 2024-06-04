using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Customer Code")]
        [Required]        
        public string CustomerCode { get; set; }
        [DisplayName("Department Code")]
        [Required]
        public string DepartmentCode { get; set; }
        [Required]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
