namespace RecordManagementPortalDev.Models
{
    public class Bill
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string InvoiceCode { get; set; }
        public string JobNo { get; set; }
        public string RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public int NoOfDays { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualRate { get; set; }
        public decimal MinRate { get; set; }

        public DateTime PrintedDate = DateTime.Today;
        public int Cartons { get; set; }
        public string Description { get; set; }
        public DateTime BillStartDate { get; set; }
        public DateTime BillEndDate { get; set; }
    }
}
