using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecordManagementPortalDev.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
	[DisplayName("User Code")]
	[Required]
	public string UserCode { get; set; }
	[DisplayName("Name")]
	[Required]
	public string Name { get; set; }

	[DisplayName("Customer Code")]
	[Required]
	public string CustomerCode { get; set; }
	[DisplayName("Department Code")]
	[Required]
	public string DepartmentCode { get; set; }
	[DisplayName("Department Permission")]	
	public string? DepartmentPermission { get; set; }

	[DisplayName("User Role")]
	[Required]
	public string UserRole { get; set; }
	[DisplayName("User Level")]
	[Required]
	public string UserLevel { get; set; }
	[DisplayName("Request Service")]	
	public bool RequestService { get; set; }
	[DisplayName("Request Destruction")]	
	public bool RequestDestruct { get; set; }
	[DisplayName("Approve Destruction Service")]	
	public bool AppDestructService { get; set; }
	//[DisplayName("Service Id")]
	//[Required]
	//public string ServiceId { get; set; }
	[DisplayName("View File Content")]	
	public bool ViewFContent { get; set; }
	[DisplayName("Amend File Content")]	
	public bool AmendFContent { get; set; }
	[DisplayName("Upload RIC")]	
	public bool UploadRIC { get; set; }
	[DisplayName("Can Export")]	
	public bool CanExport { get; set; }

	//[DisplayName("Need Approval")]
	//public string NeedApproval { get; set; }
	//[DisplayName("Control Id")]
	//[Required]
	//public string ControlId { get; set; }
	public DateTime LastLogin { get; set; } = DateTime.Now;
	public string CreatedBy { get; set; }

	public string UpdatedBy { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime UpdatedDate { get; set; } = DateTime.Now;
}

