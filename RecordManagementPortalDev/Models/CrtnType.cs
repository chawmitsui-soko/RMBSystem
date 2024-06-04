using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecordManagementPortalDev.Models
{
    public class CrtnType
    {
        [Key]
        public int Id { get; set; }        
        [DisplayName("Carton Type")]
        [Required]
        public string CtnType { get; set; }
        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Dimension")]        
        public string? Dimension { get; set; }
        [DisplayName("Barcode Label")]
        public bool StdBarcode { get; set; }
        [DisplayName("Plastic Bag")]
        public bool StdPlasticBag { get; set; }
        [DisplayName("Cable Ties")]
        public bool StdCableTies { get; set; }
        [DisplayName("Zip Lock Bag")]
        public bool StdZipBag { get; set; }
        [DisplayName("Record Index Card")]
        public bool StdRIC { get; set; }
        [DisplayName("Reorder")]
        [Required]
        public float Reorder { get; set; }
        [DisplayName("QTY")]
        [Required]
        public float Qty { get; set; }
        [DisplayName("CLoseBal")]
        [Required]
        public int CloseBal { get; set; }
        [DisplayName("AsatDate")]
        [Required]
        public DateTime AsatDate { get; set; }
    }
}
