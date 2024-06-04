using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
	public class Customer
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Customer Code")]
		[Required]
		public string CustomerCode { get; set; }
		[DisplayName("Customer Name")]
		[Required]
		public string CustomerName { get; set; }
		[Required]
		public string Address1 { get; set; }
		[Required]
		public string Address2 { get; set; }		
		public string? Address3 { get; set; }
		[Required]
		public string Address4 { get; set; }
		[DisplayName("Person In Charge")]
		[Required]
		public string PIC { get; set; }	
		[Required]
		public string Designation { get; set; }
		[Required]
		public string Telephone { get; set; }
		
		public string?  Fax { get; set; }
		[Required]
		public string Email { get; set; }
		[DisplayName("Need Approval")]
		public string NeedApproval { get; set; }
		[DisplayName("FMS/JMS/Invoice Code")]
		public string? FMSJMSInvoiceCode { get; set; }
		[Required]
		public string Billing { get; set; }
		
		public string CreatedBy { get; set; }
		
		public string UpdatedBy { get; set; }

		public DateTime CreatedDate { get; set; } 
		
		public DateTime UpdatedDate { get; set; } = DateTime.Now;
	}
}
