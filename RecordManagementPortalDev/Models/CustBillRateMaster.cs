using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordManagementPortalDev.Models
{
    public class BillRateMaster
    {
        [Required]
        public Guid Id { get; set; }
        [DisplayName("Customer Code")]
        [Required]
        public string CustomerCode { get; set; }    

        public DateTime BillStartDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal BillRateMMFees { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal BillRateSMFees { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal BillRateSupplyCtns { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal BillRateTamperPSeal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal PickupNewCtns { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal PickupOldCtns { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal PickupMinNewCtns { get; set; }

        public int PickupCtnsQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CollectNewOld { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CollectNew { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CollectOld { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvSameDay { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvSameDay { get; set; }

        public int MinQtySrvSameDay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvUrgent { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvUrgent { get; set; }

        public int MinQtySrvUrgent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvNextWDay { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvNextWDay { get; set; }

        public int MinQtySrvNextWDay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvAfterOffH { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvAfterOffH { get; set; }

        public int MinQtySrvAfterOffH { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvHolWEnd { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvHolWEnd { get; set; }

        public int MinQtySrvHolWEnd { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvSelf { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvSelf { get; set; }

        public int MinQtySrvSelf { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvPermanent { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvPermanent { get; set; }

        public int MinQtySrvPermanent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvDestruct { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvDestruct { get; set; }

        public int MinQtySrvDestruct { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvPerDestruct { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvPerDestruct { get; set; }

        public int MinQtySrvPerDestruct { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal CtnSrvDelPermanent { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        //public Decimal DelvSrvDelPermanent { get; set; }

        public int MinQtySrvDelPermanent { get; set; }

        public char BillReqDate { get; set; }        

        public DateTime BillExpDate { get; set; }

        public string SmCrtnType { get; set; }
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
        [Column(TypeName = "decimal(18,2)")]
        public Decimal SmRatePlastic { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal SmRateRIC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal SmRateCable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal SmRateSerCable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal SmRateTamper { get; set; }

        //public int SmQtyPlastic { get; set; }

        //public int SmQtyRIC { get; set; }

        //public int SmQtyCable { get; set; }

        //public int SmQtySerCable { get; set; }

        //public int SmQtyTamper { get; set; }

        public int SmMinQtyPlastic { get; set; }

        public int SmMinQtyRIC { get; set; }

        public int SmMinQtyCable { get; set; }

        public int SmMinQtySerCable { get; set; }

        public int SmMinQtyTamper { get; set; }

    }
}
