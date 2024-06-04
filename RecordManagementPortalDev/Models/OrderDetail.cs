using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Order Id")]
        [Required]
        public int OrderId { get; set; }
        [DisplayName("Department Code")]
        [Required]
        public string DepartmentCode { get; set; }
        [DisplayName("Record No")]
        public string? RecordNo { get; set; }
        [DisplayName("Destruction Date")]      
        public DateTime? DestructionDate { get; set; }
    }
}
