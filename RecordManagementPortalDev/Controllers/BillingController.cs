using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace RecordManagementPortalDev.Controllers
{
    public class BillingController : Controller
    {
        private const int defaultRange = 0;
        //private readonly ILogger<BillingController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private bool loginflag;
        public class CustBill
        {
            public string CustomerCode { get; set; }
            public SelectList CustomerList { get; set; }
            public SelectList DepartmentList { get; set; }            
            public DateTime BillStartDate { get; set; } 
            public DateTime BillEndDate { get; set; }
        }

        public class MonthlyBill
        {
            public Bill MngBillList { get; set; }

            public List<Bill> ProBillList { get; set; }

            public List<Bill> PickBillList { get; set; }

            public List<Bill> RetNBillList { get; set; }

            public List<Bill> RetPBillList { get; set; }

            public List<Bill> RetEBillList { get; set; }

            public List<Bill> RetAfBillList { get; set; }

            public List<Bill> PerRetBillList { get; set; }

            public List<Bill> DesBillList { get; set; }

            public List<Bill> SupplyCarBillList { get; set; }

            public List<Bill> SupplyTamPBillList { get; set; }

            public List<Bill> SupplyCabTBillList { get; set; }

            public List<Bill> SupplyOtherBillList { get; set; }
            public Bill Bill { get; set; }

        }
        static BillingController()
        {
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnQI5gr9m5ON2r7lKHhsl6gvM4xqcYAjFgP7a8ETj4yiINp1Z" +
"Qp9nXQooit3JA/ot2op63AAdHDNFcikNJuV208RFo0QnXiIwcixyow9h/SYVvLsY82omi9RHRyugicj5Fi8RLuNLva" +
"7kIXqF5hio6FumgVkmUQRHGuvAxBJz4keXI5fPPJ5oA+ETrHxPS5cLnGe4kYVZcEWAC9WGU9bpGWyO8wFEXuMmSMYG" +
"PzpoYC4g8wmN+XmYUyji7gy3l3xZMwl8fTUp3yVqjzZsWVa/99QbM2E+sjGST0bjsYDoLC5Ohm+Web8rtBV+fjq7VI" +
"t5F21CnGR6S1vQwhRiDaAFHcUpQSFM2L7lVPx0bNgLWKLTVvHT7K3duNXNhX576hfRhQIh0jaVexkyqLhDuWpHaSB6" +
"VoOpBQ/ZtRKzUJH3MRwT27gArUPwOun6FxT7yUlZjPabfefLPhbJ3rLeEOvymgxgrJYsqPM1+kp3StC2pY7smFrnCt" +
"2o/7H2CvUWEKsbLPSRxq6IuAFYU8iTZCgGcB";
        }

        public BillingController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;            
        }
        //public BillingController(ILogger<BillingController> logger)
        //{
        //    _logger = logger;
        //}
        public IActionResult MonthBilling()
        {
            CustBill model = new CustBill();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.BillStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.BillEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult MonthBillingByDept()
        {
            CustBill model = new CustBill();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.BillStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.BillEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult BillingReport(CustBill model)
        {
            //var custcode = model.CustomerCode;
            return View(model);
        }

        public IActionResult GetReportBill()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;
            var custcode = "";
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "customerCode")
                    {
                        custcode = value;
                    }
                    if (key == "billStartDate")
                    {
                        //var startdate = value;
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //var splittedDateTime = startdate.Split('-');
                        //DateTime sdate = new DateTime(int.Parse(splittedDateTime[0]), int.Parse(splittedDateTime[1]), int.Parse(splittedDateTime[2]));
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "billEndDate")
                    {
                        //var enddate = value;
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //var splittedDateTime = enddate.Split('-');
                        //DateTime edate =  int.Parse(splittedDateTime[2]), int.Parse(splittedDateTime[1]), int.Parse(splittedDateTime[0]));
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport report = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            
            var customer = _db.Customers.Where(x => x.CustomerCode == custcode).FirstOrDefault();            

            var rate = (from x in _db.BillRateMaster
                        where x.CustomerCode == custcode
                        select x).FirstOrDefault();

            var mngcartons = (from x in _db.JobsDetLoc
                              where (x.Status != "PerDeliver" || x.Status != "Destruct")
                              && x.CustCode == custcode
                              && x.ScannerDate <= LastMonthEndDate                              
                              select new
                              {
                                  cartons = x.Cartons
                              }).Distinct().ToList();
            var totCartons = mngcartons.Count();
            var prodetbill = (from x in _db.Customers
                              join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                              where z.RequestDate >= stdate
                              && z.RequestDate <= eddate
                              && x.CustomerCode == custcode
                              select new
                              {
                                  CustomerCode = x.CustomerCode,
                                  JobNo = z.OldJobNo,
                                  JobType = z.JobType,
                                  RequestBy = z.Person,
                                  RequestDate = z.RequestDate,
                                  ServiceLevel = z.ServLevel,
                                  Description = z.Remark,
                                  NoOfDays = (z.RequestDate - stdate).Days,
                              }).ToList();
            var procartons = (from x in _db.Customers
                              join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                              join a in _db.JobsDetLoc.Where(a => a.Status == "Stored") on z.OldJobNo equals a.JobNo
                              group a by new { a.JobNo } into gp
                              select new
                              {
                                  JobNo = gp.First().JobNo,
                                  Cartons = gp.Count(),
                              }).ToList();

            var delicartons = (from x in _db.Customers
                               join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                               join a in _db.JobsDetLoc.Where(a => a.Status == "Pulled") on z.OldJobNo equals a.JobNo
                               group a by new { a.JobNo } into gp
                               select new
                               {
                                   JobNo = gp.First().JobNo,
                                   Cartons = gp.Count(),
                               }).ToList();

            var proresult = new XElement("ProMngFees",
            from x in procartons
            join y in prodetbill on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("ProMngFee",
                                 new XElement("Cartons", x.Cartons),
                                 new XElement("JobNo", y.JobNo),
                                 new XElement("RequestBy", y.RequestBy),
                                 new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                 new XElement("NoOfDays", y.NoOfDays),
                                 new XElement("ActualRate", z.BillRateMMFees),
                                 new XElement("Amount", (y.NoOfDays * (z.BillRateMMFees / 30) * x.Cartons).ToString("0.##"))
                             ));
            var pickresult = new XElement("PickCartons",
            from x in procartons
            join y in prodetbill.Where(y => y.JobType == "C1" || y.JobType == "C2") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PickCarton",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("MinRate", z.PickupMinNewCtns),
                                  new XElement("ActualRate", z.PickupNewCtns),
                                  new XElement("Amount", (z.PickupNewCtns * x.Cartons).ToString("0.##"))
                              ));

            var retrinresult = new XElement("NormalRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("NormalRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvNextWDay),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvNextWDay * x.Cartons).ToString("0.##"))
                              ));

            var retripresult = new XElement("PriorityRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "1") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PriorityRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvSameDay),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvSameDay * x.Cartons).ToString("0.##"))
                              ));

            var retrieresult = new XElement("ExpressRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "2") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("ExpressRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvUrgent),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvUrgent * x.Cartons).ToString("0.##"))
                              ));

            var retrioresult = new XElement("AfterOfficeRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "4") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("AfterOfficeRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvAfterOffH),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvAfterOffH * x.Cartons).ToString("0.##"))
                              ));

            var retrihresult = new XElement("HolidayRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "5") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("HolidayRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrisresult = new XElement("SelfSevRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "6") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("SelfSevRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retriperresult = new XElement("PermanentRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "7") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PermanentRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retridesresult = new XElement("DestructionRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "8") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("DestructionRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrisspresult = new XElement("SelfSevPerRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("SelfSevPerRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrifileresult = new XElement("FileRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "9") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("FilePerRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var general = new XElement("General",
                               new XElement("CustomerName", customer?.CustomerName),
                               new XElement("InvoiceCode", customer?.FMSJMSInvoiceCode),
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            var monmngresult = new XElement("MonMngtFees",
            from mmf in mngcartons
            select new XElement("MonMngtFee",
                               new XElement("Sno", 1),
                               new XElement("Cartons", totCartons),
                               new XElement("ActualRate", rate.BillRateMMFees),
                               new XElement("Amount", (totCartons * rate.BillRateMMFees).ToString("0.##"))
                           ));

            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/BillingRpt.mrt"));
            report.RegData(new XElement("Root",
                new XElement("MMF", monmngresult),
                new XElement("PMF", proresult),
                new XElement("PCartons", pickresult),
                new XElement("NReterival", retrinresult),
                new XElement("PReterival", retripresult),
                new XElement("EReterival", retrieresult),
                new XElement("AOReterival", retrioresult),
                new XElement("HReterival", retrihresult),
                new XElement("SSReterival", retrisresult),
                new XElement("PERReterival", retriperresult),
                new XElement("DESReterival", retridesresult),
                new XElement("SSPerReterival", retrisspresult),
                new XElement("FileReterival", retrifileresult),
                new XElement("GEN", general)));
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MonBill(CustBill model)
        {
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);

            MonthlyBill monBill = new MonthlyBill();
            Bill bill = new Bill();
            var customer = _db.Customers.Where(x => x.CustomerCode == model.CustomerCode).FirstOrDefault();
            bill.CustomerName = customer?.CustomerName;
            bill.InvoiceCode = customer?.FMSJMSInvoiceCode;
            bill.BillStartDate = model.BillStartDate;
            bill.BillEndDate = model.BillEndDate;
            bill.PrintedDate = DateTime.Now;
            monBill.Bill = bill;

            var rate = (from x in _db.BillRateMaster
                        where x.CustomerCode == model.CustomerCode
                        select x).FirstOrDefault();

            var mngcartons = _db.JobsDetLoc.Where(x => x.Status == "Stored" && x.CustCode == model.CustomerCode).ToList();

            Bill mngBill = new Bill();
            var totCartons = mngcartons.Count();
            mngBill.Cartons = totCartons;
            mngBill.ActualRate = rate.BillRateMMFees;
            mngBill.Amount = totCartons * rate.BillRateMMFees;
            monBill.MngBillList = mngBill;

            var prodetbill = (from x in _db.Customers                           
                           join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                           where z.RequestDate >= ThisMonthStartDate
                           && z.RequestDate <= ThisMonthEndDate
                           && x.CustomerCode == model.CustomerCode
                           select new
                           {
                               CustomerCode = x.CustomerCode,
                               JobNo = z.OldJobNo,
                               JobType = z.JobType,
                               RequestBy = z.Person,
                               RequestDate = z.RequestDate,
                               NoOfDays = (z.RequestDate - ThisMonthStartDate).Days,
                           }).ToList();
            var procartons = (from x in _db.Customers                           
                           join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                           join a in _db.JobsDetLoc.Where(a => a.Status == "Stored") on z.OldJobNo equals a.JobNo
                           group a by new { a.JobNo } into gp
                           select new
                           {
                               JobNo = gp.First().JobNo,
                               Cartons = gp.Count(),                               
                           }).ToList();      
            
            var proresult = (from x in procartons
                          join y in prodetbill on x.JobNo equals y.JobNo
                          join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                          select new Bill()
                          {
                            Cartons = x.Cartons,
                            JobNo = y.JobNo,
                            RequestBy = y.RequestBy,
                            RequestDate = y.RequestDate,    
                            NoOfDays = y.NoOfDays,
                            ActualRate = z.BillRateMMFees,
                            Amount = y.NoOfDays * (z.BillRateMMFees/30) * x.Cartons
                          }).ToList();

            var pickresult = (from x in procartons
                             join y in prodetbill.Where(y => y.JobType =="C1") on x.JobNo equals y.JobNo
                             join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                             select new Bill()
                             {
                                 Cartons = x.Cartons,
                                 JobNo = y.JobNo,
                                 RequestBy = y.RequestBy,
                                 RequestDate = y.RequestDate,
                                 MinRate = z.PickupMinNewCtns,
                                 ActualRate = z.PickupNewCtns,
                                 Amount = z.PickupNewCtns * x.Cartons,                                 
                             }).ToList();

            var retrinresult = (from x in procartons
                              join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                              join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                              select new Bill()
                              {
                                  Cartons = x.Cartons,
                                  JobNo = y.JobNo,
                                  RequestBy = y.RequestBy,
                                  RequestDate = y.RequestDate,
                                  MinRate = z.PickupMinNewCtns,
                                  ActualRate = z.PickupNewCtns,
                                  Amount = z.PickupNewCtns * x.Cartons,
                              }).ToList();

            var retripresult = (from x in procartons
                                join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                                join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                                select new Bill()
                                {
                                    Cartons = x.Cartons,
                                    JobNo = y.JobNo,
                                    RequestBy = y.RequestBy,
                                    RequestDate = y.RequestDate,
                                    MinRate = z.PickupMinNewCtns,
                                    ActualRate = z.PickupNewCtns,
                                    Amount = z.PickupNewCtns * x.Cartons,
                                }).ToList();

            var retrieresult = (from x in procartons
                                join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                                join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                                select new Bill()
                                {
                                    Cartons = x.Cartons,
                                    JobNo = y.JobNo,
                                    RequestBy = y.RequestBy,
                                    RequestDate = y.RequestDate,
                                    MinRate = z.PickupMinNewCtns,
                                    ActualRate = z.PickupNewCtns,
                                    Amount = z.PickupNewCtns * x.Cartons,
                                }).ToList();

            var retriafresult = (from x in procartons
                                join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                                join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                                select new Bill()
                                {
                                    Cartons = x.Cartons,
                                    JobNo = y.JobNo,
                                    RequestBy = y.RequestBy,
                                    RequestDate = y.RequestDate,
                                    MinRate = z.PickupMinNewCtns,
                                    ActualRate = z.PickupNewCtns,
                                    Amount = z.PickupNewCtns * x.Cartons,
                                }).ToList();

            var perretresult = (from x in procartons
                                join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                                join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                                select new Bill()
                                {
                                    Cartons = x.Cartons,
                                    JobNo = y.JobNo,
                                    RequestBy = y.RequestBy,
                                    RequestDate = y.RequestDate,
                                    MinRate = z.PickupMinNewCtns,
                                    ActualRate = z.PickupNewCtns,
                                    Amount = z.PickupNewCtns * x.Cartons,
                                }).ToList();

            var desresult = (from x in procartons
                                join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                                join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                                select new Bill()
                                {
                                    Cartons = x.Cartons,
                                    JobNo = y.JobNo,
                                    RequestBy = y.RequestBy,
                                    RequestDate = y.RequestDate,
                                    MinRate = z.PickupMinNewCtns,
                                    ActualRate = z.PickupNewCtns,
                                    Amount = z.PickupNewCtns * x.Cartons,
                                }).ToList();

            var supcabtresult = (from x in procartons
                             join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                             join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                             select new Bill()
                             {
                                 Cartons = x.Cartons,
                                 JobNo = y.JobNo,
                                 RequestBy = y.RequestBy,
                                 RequestDate = y.RequestDate,
                                 MinRate = z.PickupMinNewCtns,
                                 ActualRate = z.PickupNewCtns,
                                 Amount = z.PickupNewCtns * x.Cartons,
                             }).ToList();

            var supcarresult = (from x in procartons
                             join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                             join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                             select new Bill()
                             {
                                 Cartons = x.Cartons,
                                 JobNo = y.JobNo,
                                 RequestBy = y.RequestBy,
                                 RequestDate = y.RequestDate,
                                 MinRate = z.PickupMinNewCtns,
                                 ActualRate = z.PickupNewCtns,
                                 Amount = z.PickupNewCtns * x.Cartons,
                             }).ToList();

            var supotherresult = (from x in procartons
                             join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                             join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                             select new Bill()
                             {
                                 Cartons = x.Cartons,
                                 JobNo = y.JobNo,
                                 RequestBy = y.RequestBy,
                                 RequestDate = y.RequestDate,
                                 MinRate = z.PickupMinNewCtns,
                                 ActualRate = z.PickupNewCtns,
                                 Amount = z.PickupNewCtns * x.Cartons,
                             }).ToList();

            var suptampresult = (from x in procartons
                             join y in prodetbill.Where(y => y.JobType == "D1") on x.JobNo equals y.JobNo
                             join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
                             select new Bill()
                             {
                                 Cartons = x.Cartons,
                                 JobNo = y.JobNo,
                                 RequestBy = y.RequestBy,
                                 RequestDate = y.RequestDate,
                                 MinRate = z.PickupMinNewCtns,
                                 ActualRate = z.PickupNewCtns,
                                 Amount = z.PickupNewCtns * x.Cartons,
                             }).ToList();

            monBill.ProBillList = proresult;
            monBill.PickBillList = pickresult;
            monBill.RetNBillList = retrinresult;
            monBill.RetPBillList = retripresult;
            monBill.RetEBillList = retrieresult;
            monBill.RetAfBillList = retriafresult;
            monBill.PerRetBillList = perretresult;
            monBill.DesBillList = desresult;
            monBill.SupplyCabTBillList = supcabtresult;
            monBill.SupplyCarBillList = supcarresult;
            monBill.SupplyOtherBillList = supotherresult;
            monBill.SupplyTamPBillList = suptampresult;

            return View(monBill);
        }

        public IActionResult BillCreate(CustBill model)
        {
            return View();
        }        

        public IActionResult CreateReportBill()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            StiReport report = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
                      
            var customer = _db.Customers.Where(x => x.CustomerCode == "A027").FirstOrDefault();            

            var rate = (from x in _db.BillRateMaster
                        where x.CustomerCode == "A027"
                        select x).FirstOrDefault();

            var mngcartons = (from x in _db.JobsDetLoc
                              where (x.Status != "PerDeliver" || x.Status != "Destruct")
                              && x.CustCode == "A027"
                              && x.ScannerDate <= LastMonthEndDate
                              select new
                              {
                                  cartons = x.Cartons
                              }).Distinct().ToList(); 
            var totCartons = mngcartons.Count();
            var prodetbill = (from x in _db.Customers
                              join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                              where z.RequestDate >= ThisMonthStartDate
                              && z.RequestDate <= ThisMonthEndDate
                              && x.CustomerCode == "A027"
                              select new
                              {
                                  CustomerCode = x.CustomerCode,
                                  JobNo = z.OldJobNo,
                                  JobType = z.JobType,
                                  RequestBy = z.Person,
                                  RequestDate = z.RequestDate,
                                  ServiceLevel = z.ServLevel,
                                  Description = z.Remark,
                                  NoOfDays = (z.RequestDate - ThisMonthStartDate).Days,
                              }).ToList();
            var procartons = (from x in _db.Customers
                              join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                              join a in _db.JobsDetLoc.Where(a => a.Status == "Stored") on z.OldJobNo equals a.JobNo
                              group a by new { a.JobNo } into gp
                              select new
                              {
                                  JobNo = gp.First().JobNo,
                                  Cartons = gp.Count(),
                              }).ToList();

            var delicartons = (from x in _db.Customers
                              join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                              join a in _db.JobsDetLoc.Where(a => a.Status == "Pulled") on z.OldJobNo equals a.JobNo
                              group a by new { a.JobNo } into gp
                              select new
                              {
                                  JobNo = gp.First().JobNo,
                                  Cartons = gp.Count(),
                              }).ToList();
            var proresult = new XElement("ProMngFees",
            from x in procartons
            join y in prodetbill on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("ProMngFee",
                                 new XElement("Cartons", x.Cartons),
                                 new XElement("JobNo", y.JobNo),
                                 new XElement("RequestBy", y.RequestBy),
                                 new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                 new XElement("NoOfDays", y.NoOfDays),
                                 new XElement("ActualRate", z.BillRateMMFees),
                                 new XElement("Amount", (y.NoOfDays * (z.BillRateMMFees / 30) * x.Cartons).ToString("0.##"))
                             ));
            var pickresult = new XElement("PickCartons",
            from x in procartons
            join y in prodetbill.Where(y => y.JobType == "C1" || y.JobType == "C2") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PickCarton",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("MinRate", z.PickupMinNewCtns),
                                  new XElement("ActualRate", z.PickupNewCtns),
                                  new XElement("Amount", (z.PickupNewCtns * x.Cartons).ToString("0.##"))
                              ));

            var retrinresult = new XElement("NormalRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("NormalRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvNextWDay),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvNextWDay * x.Cartons).ToString("0.##"))
                              ));

            var retripresult = new XElement("PriorityRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PriorityRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvSameDay),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvSameDay * x.Cartons).ToString("0.##"))
                              ));

            var retrieresult = new XElement("ExpressRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("ExpressRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvUrgent),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvUrgent * x.Cartons).ToString("0.##"))
                              ));

            var retrioresult = new XElement("AfterOfficeRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("AfterOfficeRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvAfterOffH),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvAfterOffH * x.Cartons).ToString("0.##"))
                              ));

            var retrihresult = new XElement("HolidayRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("HolidayRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrisresult = new XElement("SelfSevRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("SelfSevRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retriperresult = new XElement("PermanentRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("PermanentRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retridesresult = new XElement("DestructionRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("DestructionRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrisspresult = new XElement("SelfSevPerRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("SelfSevPerRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var retrifileresult = new XElement("FileRetrievals",
            from x in delicartons
            join y in prodetbill.Where(y => (y.JobType == "D1" || y.JobType == "D2") && y.ServiceLevel == "3") on x.JobNo equals y.JobNo
            join z in _db.BillRateMaster on y.CustomerCode equals z.CustomerCode
            select new XElement("FilePerRetrieval",
                                  new XElement("Cartons", x.Cartons),
                                  new XElement("JobNo", y.JobNo),
                                  new XElement("RequestBy", y.RequestBy),
                                  new XElement("RequestDate", y.RequestDate.ToString("dd/MM/yyyy")),
                                  new XElement("CartonDelRate", z.CtnSrvHolWEnd),
                                  new XElement("Description", y.Description),
                                  new XElement("Amount", (z.CtnSrvHolWEnd * x.Cartons).ToString("0.##"))
                              ));

            var general = new XElement("General",
                               new XElement("CustomerName", customer?.CustomerName),
                               new XElement("InvoiceCode", customer?.FMSJMSInvoiceCode),
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            var monmngresult = new XElement("MonMngtFees",
            from mmf in mngcartons
            select new XElement("MonMngtFee",
                               new XElement("Sno", 1),
                               new XElement("Cartons", totCartons),
                               new XElement("ActualRate", rate.BillRateMMFees),
                               new XElement("Amount", (totCartons * rate.BillRateMMFees).ToString("0.##"))                               
                           ));   
            
            report.RegData(new XElement("Root",
                new XElement("MMF", monmngresult),
                new XElement("PMF", proresult),
                new XElement("NReterival", retrinresult),
                new XElement("PReterival", retripresult),
                new XElement("EReterival", retrieresult),
                new XElement("AOReterival", retrioresult),
                new XElement("HReterival", retrihresult),
                new XElement("SSReterival", retrisresult),
                new XElement("PERReterival", retriperresult),
                new XElement("DESReterival", retridesresult),
                new XElement("SSPerReterival", retrisspresult),
                new XElement("FileReterival", retrifileresult),
                new XElement("PCartons", pickresult),
                new XElement("GEN", general)));
            report.Dictionary.Synchronize();            
            return StiNetCoreDesigner.GetReportResult(this, report);            
        }

        public IActionResult SaveReportAs()
        {
            StiReport report = StiNetCoreDesigner.GetReportObject(this);
            var requestParams = StiNetCoreDesigner.GetRequestParams(this);
            var reportName = requestParams.Designer.FileName;
            return StiNetCoreDesigner.SaveReportResult(this, "Your report has already saved.");
            //return Content("{"infoMessage":"Some info message after saving"}");
            //return Content("{\"warningMessage\":\"Some info message after saving\"}");

        }

        public IActionResult ViewerInteraction()
        {
            NameValueCollection formValues = StiNetCoreViewer.GetFormValues(this);
            return StiNetCoreViewer.InteractionResult(this);
        } 
        public IActionResult DesignerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

    }
}