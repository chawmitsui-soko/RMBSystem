using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class ApprovalSetup
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("User Id")]
        [Required]
        public string UserId { get; set; }
        [DisplayName("Customer Code")]
        [Required]
        public string CustomerCode { get; set; }
        [DisplayName("Approver Id")]        
        public string ApproverId { get; set; }
        [DisplayName("Substitute Approver Id")]
        public string SubApproverId { get; set; }
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }
    }
}
