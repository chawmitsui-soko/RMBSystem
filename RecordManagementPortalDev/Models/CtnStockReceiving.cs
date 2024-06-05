using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class CtnStockReceiving
    {
        [Key]
        public int Id { get; set; }
        public string CartonType { get; set; }        
        [DisplayName("Carton Type")]
        [Required]
        public DateTime ReceivingDate { get; set; }
        [DisplayName("ReceivingDate")]
        [Required]
        public int Qty { get; set; }
		[DisplayName("Qty")]
		[Required]

		public string Remarks { get; set; }
	}
}
