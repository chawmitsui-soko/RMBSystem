using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class CartonDetails
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Customer Code")]
        [Required]
        public string CustCode { get; set; }

        [DisplayName("Department Code")]
        public string? DeptCode { get; set; }

        [DisplayName("Cartons")]
        public string? Cartons { get; set; }

        [DisplayName("Carton Type")]        
        public string? CtnType { get; set; }

        [DisplayName("Transaction Date")]        
        public DateTime? TransDate { get; set; }

        [DisplayName("Received Date")]        
        public DateTime? ReceivedDate { get; set; }

        [DisplayName("Out Date")]
        public DateTime? OutDate { get; set; }

        [DisplayName("Job No")]        
        public string? JobNo { get; set; }

        [DisplayName("Last Job No")]
        public string? LastJobNo { get; set; }

        [DisplayName("Location")]
        public string? Location { get; set; }

        [DisplayName("Control")]
        public string? Control { get; set; }

        [DisplayName("Permanentstored")]
        [Required]
        public bool Permanentstored { get; set; }

        [DisplayName("Status")]
        public string? Status { get; set; }

        [DisplayName("Destruct Date")]
        public DateTime? DestructDate { get; set; }

        [DisplayName("SealNo")]
        public string? SealNo { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("PalletBay")]
        public string? PalletBay { get; set; }

        [DisplayName("msrepl_synctran_ts")]        
        public long? msrepl_synctran_ts { get; set; }
    }
}
