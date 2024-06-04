using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecordManagementPortalDev.Models
{
    public class OrderRequests
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Order Id")]
        [Required]
        public int OrderId { get; set; }
        [DisplayName("Customer Code")]
        [Required]
        public string CustomerCode { get; set; }
        [DisplayName("Customer Name")]
        [Required]
        public string CustomerName { get; set; }
        [DisplayName("Department Code")]
        [Required]
        public string DepartmentCode { get; set; }         
        [DisplayName("Contact Person")]
        [Required]
        public string ContactPerson { get; set; }

        public string? Telephone { get; set; }

        public string? Fax { get; set; }
        [Required]
        public string Email { get; set; }
        [DisplayName("User ID")]
        [Required]
        public string UserId { get; set; }
        [DisplayName("Delivery Address1")]
        [Required]
        public string Address1 { get; set; }
        [DisplayName("Delivery Address2")]
        [Required]
        public string Address2 { get; set; }
        [DisplayName("Delivery Address3")]
        public string? Address3 { get; set; }
        [DisplayName("Delivery Address4")]
        public string? Address4 { get; set; }
        //public string Address4 { get; set; }
        [DisplayName("Order Type")]
        [Required]
        public string OrderType { get; set; }
        [DisplayName("Carton Quntity")]
        [Required]
        public int CartonQty { get; set; }
        [DisplayName("Tamper Seal Quntity")]        
        public int? TamperSealQty { get; set; }
        [DisplayName("Plastic Bag Quntity")]
        public int? PlasticBagQty { get; set; }
        [DisplayName("RIC Quntity")]        
        public int? RICQty { get; set; }
        [DisplayName("Tie Quntity")]        
        public int? TieQty { get; set; }
        [DisplayName("Order Date")]
        [Required]
        public DateTime OrderDate { get; set; }
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;        
        [Required]
        public string OrderStatus { get; set; }
        [DisplayName("Remark")]        
        public string? Remark { get; set; }

    }
}
