using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Old Job Registration No")]
        [Required]
        public string OldJobNo { get; set; }
        [DisplayName("Job Registration No")]
        [Required]
        public Guid JobNo { get; set; }
        [DisplayName("Job Date")]
        [Required]
        public DateTime JobDate { get; set; } = DateTime.Now;
        [DisplayName("Customer Code")]
        [Required]
        public string CustCode { get; set; }
        [DisplayName("Department Code")]        
        public string? DeptCode { get; set; }
        [DisplayName("Bill Date")]
        [Required]
        public DateTime BillDate { get; set; }
        [DisplayName("Delivery Address1")]
        [Required]
        public string Address1 { get; set; }
        [DisplayName("Delivery Address2")]        
        public string? Address2 { get; set; }
        [DisplayName("Delivery Address3")]        
        public string? Address3 { get; set; }
        [DisplayName("Delivery Address4")]        
        public string? Address4 { get; set; }
        [DisplayName("Job Level")]
        [Required]
        public string JobLevel { get; set; }
        [DisplayName("Job Type")]
        [Required]
        public string JobType { get; set; }
        [DisplayName("Service Level")]
        [Required]
        public string ServLevel { get; set; }
        [DisplayName("Carton Type")]
        [Required]
        public string CtnType { get; set; }
        [DisplayName("Request Date")]
        [Required]
        public DateTime RequestDate { get; set; }
        [DisplayName("Contact Person")]
        [Required]
        public string Person { get; set; }
        [DisplayName("Contact No")]
        public string? Contact { get; set; }
        [DisplayName("Fax No")]
        public string? Fax { get; set; }
        [DisplayName("Remark")]
        public string? Remark { get; set; }        
        [DisplayName("Index Card")]        
        public string? IndexCard { get; set; }        
        [DisplayName("Job Order No")]
        public string? JobOrderNo { get; set; }       
        [DisplayName("Total Carton")]
        [Required]
        public int TotalCtn { get; set; }
        [DisplayName("Tamper Seal Quntity")]        
        public int? TamperSealQty { get; set; }
        [DisplayName("Plastic Bag Quntity")]
        public int? PlasticBagQty { get; set; }
        [DisplayName("RIC Quntity")]        
        public int? RICQty { get; set; }
        [DisplayName("Tie Quntity")]        
        public int? TieQty { get; set; }
        [DisplayName("Staff")]        
        public string? Staff { get; set; }

        public string? MssInvoice { get; set; }

        public int? DeleteFlag { get; set; }

        public string? RMBRemark { get; set; }
        
        public string? RMBRemark1 { get; set; }
        
        public string? RMBRemark2 { get; set; }
        
        public string? RMBRemark3 { get; set; }


    }
}
