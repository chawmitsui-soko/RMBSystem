using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class CrtnMonthlyBals
    {
        [Key]
        public int Id { get; set; }
        public string CartonType { get; set; }        
        [DisplayName("Carton Type")]
        [Required]
        public int ClosingBalance { get; set; }
        [DisplayName("Closing Balance")]
        [Required]
        public DateTime AsatDate { get; set; }

		public string Remarks { get; set; }
	}
}
