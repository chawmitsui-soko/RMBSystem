using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class RecordDetail
    {
        [Key]
        public int Id { get; set; }        
        [DisplayName("Customer Code")]
        [Required]
        public string CustomerCode { get; set; }
        [DisplayName("Department Code")]
        [Required]
        public string DepartmentCode { get; set; }
        [DisplayName("Record No")]
        [Required]
        public string RecordNo { get; set; }
        [DisplayName("File No")]
        public string FileNo { get; set; }
        [DisplayName("Reference 1")]
        public string? Reference1 { get; set; }
        [DisplayName("Reference 2")]
        public string? Reference2 { get; set; }
        [DisplayName("Reference 3")]
        public string? Reference3 { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
