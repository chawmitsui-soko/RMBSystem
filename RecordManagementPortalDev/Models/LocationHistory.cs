using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
	public class LocationHistory
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Customer Code")]
		[Required]
		public string CustCode { get; set; }
		[DisplayName("Department Code")]
		[Required]
		public string DeptCode { get; set; }
		[DisplayName("Cartons")]
		[Required]
		public string Cartons { get; set; }
		[DisplayName("Location")]
		[Required]
		public string Location { get; set; }
		[DisplayName("Status")]
		public string Status { get; set; }
		[DisplayName("File No")]
		[Required]
		public string FileNo { get; set; }
		[DisplayName("Scanner Date")]
		[Required]
		public DateTime ScannerDate { get; set; }
		[DisplayName("Staff")]
		[Required]
		public string Staff { get; set; }
		[DisplayName("Creted Date")]
		[Required]
		public DateTime CreatedDate { get; set; }
		[DisplayName("Updated Date")]
		[Required]
		public DateTime UpdatedDate { get; set; }
	}
}
