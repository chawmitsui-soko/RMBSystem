using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace RecordManagementPortalDev.Controllers
{
    public class ReportsController : Controller
    {
        private const int defaultRange = -1;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private bool loginflag;

        public class ReportGen
        {
            public string CustomerCode { get; set; }
            public string Location { get; set; }
            public SelectList CustomerList { get; set; }
            public SelectList DepartmentList { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        static ReportsController()
        {
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnQI5gr9m5ON2r7lKHhsl6gvM4xqcYAjFgP7a8ETj4yiINp1Z" +
"Qp9nXQooit3JA/ot2op63AAdHDNFcikNJuV208RFo0QnXiIwcixyow9h/SYVvLsY82omi9RHRyugicj5Fi8RLuNLva" +
"7kIXqF5hio6FumgVkmUQRHGuvAxBJz4keXI5fPPJ5oA+ETrHxPS5cLnGe4kYVZcEWAC9WGU9bpGWyO8wFEXuMmSMYG" +
"PzpoYC4g8wmN+XmYUyji7gy3l3xZMwl8fTUp3yVqjzZsWVa/99QbM2E+sjGST0bjsYDoLC5Ohm+Web8rtBV+fjq7VI" +
"t5F21CnGR6S1vQwhRiDaAFHcUpQSFM2L7lVPx0bNgLWKLTVvHT7K3duNXNhX576hfRhQIh0jaVexkyqLhDuWpHaSB6" +
"VoOpBQ/ZtRKzUJH3MRwT27gArUPwOun6FxT7yUlZjPabfefLPhbJ3rLeEOvymgxgrJYsqPM1+kp3StC2pY7smFrnCt" +
"2o/7H2CvUWEKsbLPSRxq6IuAFYU8iTZCgGcB";
        }
        public ReportsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
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


        public IActionResult CustomerInvR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult LocationR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult JobsR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult CustomerFR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult ScanR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            return View(model);
        }

        public IActionResult StockInvR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult PrintStkR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult MonBillSumR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult ScannerPLogR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
            return View(model);
        }

        public IActionResult LocationReport()
        {
            return View();
        }

        public IActionResult LocationCreate()
        {
            return View();
        }

        public IActionResult JobsReport()
        {
            return View();
        }

        public IActionResult JobsCreate()
        {
            return View();
        }

        public IActionResult CustomerInvReport()
        {
            return View();
        }

        public IActionResult CustomerInvCreate()
        {
            return View();
        }

        public IActionResult CustomerFReport()
        {
            return View();
        }

        public IActionResult CustomerFCreate()
        {
            return View();
        }

        public IActionResult ScanStatusReport()
        {
            return View();
        }

        public IActionResult ScanStatusCreate()
        {
            return View();
        }

        public IActionResult MonBillSumReport()
        {
            return View();
        }

        public IActionResult MonBillSumCreate()
        {
            return View();
        }

        public IActionResult PrintStkReport()
        {
            return View();
        }

        public IActionResult PrintStkCreate()
        {
            return View();
        }

        public IActionResult ScannerPLogReport()
        {
            return View();
        }

        public IActionResult ScannerPLogCreate()
        {
            return View();
        }

        public IActionResult StockInvReport()
        {
            return View();
        }

        public IActionResult StockInvCreate()
        {
            return View();
        }

        public IActionResult CreateReportSStatus()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;            
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {                    
                    if (key == "startDate")
                    {                        
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {                        
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport scanreport = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);

            var scancartons = (from x in _db.Customers
                               join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                               join a in _db.JobsDetLoc on z.OldJobNo equals a.JobNo
                               select new {
                                   x, z, a }).ToList();
            var groupscan = scancartons.GroupBy(a => a.a.JobNo)
                               .Select(gp => new                               
                               {
                                   JobNo = gp.ToList().FirstOrDefault().a.JobNo,
                                   ScannedCartons = gp.Where(gp => gp.a.Status == "Stored").Count(),
                                   RequestedCartons = gp.Where(gp => gp.a.Status == "Pulled" || gp.a.Status == "Collected").Count()
                               }).ToList();            

           var scan = (from x in groupscan
                        join y in _db.Job on x.JobNo equals y.OldJobNo
                        join z in _db.Customers on y.CustCode equals z.CustomerCode
                        select new
                        {
                           customercode = z.CustomerCode,
                           customername = z.CustomerName,
                           jobno = y.OldJobNo,
                           requestdate = y.RequestDate,
                           jobtype = y.JobType,
                           scancartons = x.ScannedCartons,
                           requestcartons = x.RequestedCartons
                        }).ToList();
            
            var scansresult = new XElement("ScanStatusReport",
            from x in scan
            select new XElement("ScanStatus",
                                 new XElement("CustomerCode", x.customercode),
                                 new XElement("CustomerName", x.customername),
                                 new XElement("JobNo", x.jobno),
                                 new XElement("RequestDate", x.requestdate.ToString("dd/MM/yyyy")),
                                 new XElement("JobType", x.jobtype),
                                 new XElement("TotalRequestCartons", x.requestcartons),
                                 new XElement("TotalScanCartons", x.scancartons)
                             ));
            var general = new XElement("General",                               
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            scanreport.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("SSReport", scansresult)
                )); 
            scanreport.Dictionary.Synchronize();
            return StiNetCoreDesigner.GetReportResult(this, scanreport);
        }

        public IActionResult GetReportSStatus()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;            
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {                    
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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

            var scancartons = (from x in _db.Customers
                               join z in _db.Job.Where(z => z.DeleteFlag == 0) on x.CustomerCode equals z.CustCode
                               join a in _db.JobsDetLoc on z.OldJobNo equals a.JobNo                               
                               group a by new { a.JobNo } into gp                               
                               select new
                               {                               
                                   JobNo = gp.ToList().FirstOrDefault().JobNo,
                                   ScannedCartons = gp.Where(gp => gp.Status == "Stored").Count(),
                                   RequestedCartons = gp.Where(gp => gp.Status == "Pulled" || gp.Status == "Collected").Count()
                               }).ToList();

            var scan = (from x in scancartons
                        join y in _db.Job on x.JobNo equals y.OldJobNo
                        join z in _db.Customers on y.CustCode equals z.CustomerCode
                        select new
                        {
                            customercode = z.CustomerCode,
                            customername = z.CustomerName,
                            jobno = y.OldJobNo,
                            requestdate = y.RequestDate,
                            jobtype = y.JobType,
                            scancartons = x.ScannedCartons,
                            requestcartons = x.RequestedCartons
                        }).ToList();
            var scansresult = new XElement("ScanStatusReport",
            from x in scan
            select new XElement("ScanStatus",
                                 new XElement("CustomerCode", x.customercode),
                                 new XElement("CustomerName", x.customername),
                                 new XElement("JobNo", x.jobno),
                                 new XElement("RequestDate", x.requestdate.ToString("dd/MM/yyyy")),
                                 new XElement("JobType", x.jobtype),
                                 new XElement("TotalRequestCartons", x.requestcartons),
                                 new XElement("TotalScanCartons", x.scancartons)
                             ));
            var general = new XElement("General",
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/ScanSRpt.mrt"));
            
            report.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("SSReport", scansresult)
                ));
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult CreateReportLocation()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;
            var location = "";
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "location")
                    {
                        location = value;
                    }
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport locreport = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            //var location = "D03B71L1";
            var loc = (from x in _db.JobsDetLoc
                       where x.Location == location
                       select new
                       {
                           CustomerCode = x.CustCode,
                           DepartmentCode = x.DeptCode,
                           JobNo = x.JobNo,
                           BoxNo = x.Cartons,
                           ReceivedDate = x.ScannerDate,
                           Location = x.Location,
                           Status = x.Status
                       }).ToList();
            var locationresult = new XElement("LocationReport",
            from x in loc
            select new XElement("Location",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("Department", x.DepartmentCode),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("ReceivedDate", x.ReceivedDate.ToString("dd/MM/yyyy")),
                                 new XElement("BoxNo", x.BoxNo),
                                 new XElement("Location", x.Location),
                                 new XElement("Status", x.Status),
                                 new XElement("Permanent", "False"),
                                 new XElement("DestructionDate", "")
                             ));
            var general = new XElement("General",
                               new XElement("Location", location),
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            locreport.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("LocReport", locationresult)
                ));
            locreport.Dictionary.Synchronize();
            return StiNetCoreDesigner.GetReportResult(this, locreport);
        }

        public IActionResult GetReportLocation()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;
            var location = "";
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "location")
                    {
                        location = value;
                    }
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
            //var location = "D03B71L1";
            var loc = (from x in _db.JobsDetLoc
                               where x.Location == location
                               select new
                               {
                                   CustomerCode = x.CustCode,
                                   DepartmentCode = x.DeptCode,
                                   JobNo = x.JobNo,
                                   BoxNo = x.Cartons,
                                   ReceivedDate = x.ScannerDate,
                                   Location = x.Location,
                                   Status = x.Status
                               }).ToList();            
            var locationresult = new XElement("LocationReport",
            from x in loc
            select new XElement("Location",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("Department", x.DepartmentCode),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("ReceivedDate", x.ReceivedDate.ToString("dd/MM/yyyy")),
                                 new XElement("BoxNo", x.BoxNo),
                                 new XElement("Location", x.Location),
                                 new XElement("Status", x.Status),
                                 new XElement("Permanent", "False"),
                                 new XElement("DestructionDate", "")
                             ));
            var general = new XElement("General",
                               new XElement("Location", location),
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/LocationRpt.mrt"));

            report.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("LocReport", locationresult)
                ));
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult CreateReportCustomerInv()
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
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport cireport = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);            
            var custname = (from a in _db.Customers
                            where a.CustomerCode == custcode
                            select a.CustomerName).FirstOrDefault();
            var custinv = (from x in _db.JobsDetLoc
                       join y in _db.Job on x.JobNo equals y.OldJobNo
                       join z in _db.OrderRequests on y.CustCode equals z.CustomerCode
                       where x.ScannerDate >= stdate
                       && x.ScannerDate <= eddate
                       && x.CustCode == custcode
                       select new
                       {
                           CustomerCode = x.CustCode,
                           Prefix = x.DeptCode,
                           JobNo = x.JobNo,
                           BoxNo = x.Cartons,
                           TransDate = z.TransactionDate,
                           RequestedBy = y.Person,
                           FirstRecDate = x.CreatedDate,
                           Location = x.Location,
                           Status = x.Status                           
                       }).GroupBy(x => x.BoxNo)
                   .Select(grp => grp.First()).ToList();
            var custinvresult = new XElement("CustomerInvReport",
            from x in custinv
            select new XElement("CustomerInv",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("Prefix", x.Prefix),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("TransDate", x.TransDate.ToString("dd/MM/yyyy")),
                                 new XElement("RequestedBy", x.RequestedBy),
                                 new XElement("BoxNo", x.BoxNo),
                                 new XElement("FirstRecDate", x.FirstRecDate.ToString("dd/MM/yyyy")),
                                 new XElement("Location", x.Location),
                                 new XElement("Status", x.Status),
                                 new XElement("Permanent", "False"),
                                 new XElement("DestructionDate", "")
                             ));
            var general = new XElement("General",
                               new XElement("CustCode", custcode),
                               new XElement("CustName", custname),
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            cireport.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("CstInvReport", custinvresult)
                ));
            cireport.Dictionary.Synchronize();
            return StiNetCoreDesigner.GetReportResult(this, cireport);
        }

        public IActionResult GetReportCustomerInv()
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
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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
            //var custcode = "A027";
            var custname = (from a in _db.Customers
                            where a.CustomerCode == custcode
                            select a.CustomerName).FirstOrDefault();
            var custinv = (from x in _db.JobsDetLoc
                           join y in _db.Job on x.JobNo equals y.OldJobNo
                           join z in _db.OrderRequests on y.CustCode equals z.CustomerCode
                           where x.ScannerDate >= stdate
                           && x.ScannerDate <= eddate
                           && x.CustCode == custcode
                           select new
                           {
                               CustomerCode = x.CustCode,
                               Prefix = x.DeptCode,
                               JobNo = x.JobNo,
                               BoxNo = x.Cartons,
                               TransDate = z.TransactionDate,
                               RequestedBy = y.Person,
                               FirstRecDate = x.CreatedDate,
                               Location = x.Location,
                               Status = x.Status
                           }).GroupBy(x => x.BoxNo)
                   .Select(grp => grp.First()).ToList();
            var custinvresult = new XElement("CustomerInvReport",
            from x in custinv
            select new XElement("CustomerInv",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("Prefix", x.Prefix),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("TransDate", x.TransDate.ToString("dd/MM/yyyy")),
                                 new XElement("RequestedBy", x.RequestedBy),
                                 new XElement("BoxNo", x.BoxNo),
                                 new XElement("FirstRecDate", x.FirstRecDate.ToString("dd/MM/yyyy")),
                                 new XElement("Location", x.Location),
                                 new XElement("Status", x.Status),
                                 new XElement("Permanent", "False"),
                                 new XElement("DestructionDate", "")
                             ));
            var general = new XElement("General",
                               new XElement("CustCode", custcode),
                               new XElement("CustName", custname),
                               new XElement("BillStartDate", stdate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", eddate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/CustomerInvRpt.mrt"));

            report.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("CstInvReport", custinvresult)
                ));
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult GetReportCustomerF()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;            
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {                    
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            var customers = (from x in _db.Customers
                             orderby x.CreatedDate descending
                             select x).ToList();
            var data = new XElement("Customers",
            from cus in customers
            select new XElement("Customer",
                               new XElement("CustomerName", cus.Designation),
                               new XElement("Address", cus.Address1 + "," + cus.Address2 + "," + cus.Address3),
                               new XElement("PostalCode", cus.Address4),
                               new XElement("PIC", cus.PIC)
                           ));
            StiReport report = new StiReport();
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/CustomerFRpt.mrt"));
            report.RegData("Customers", data);
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult CreateReportCustomerF()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport report = new StiReport();
            var customers = (from x in _db.Customers
                             orderby x.CreatedDate descending
                             select x).ToList();
            var data = new XElement("Customers",
            from cus in customers
            select new XElement("Customer",
                               new XElement("CustomerName", cus.Designation),
                               new XElement("Address", cus.Address1 + "," + cus.Address2 + "," + cus.Address3),
                               new XElement("PostalCode", cus.Address4),
                               new XElement("PIC", cus.PIC)
                           ));
            
            report.RegData(data);
            report.Dictionary.Synchronize();            
            return StiNetCoreDesigner.GetReportResult(this, report);            
        }

        public IActionResult GetReportJobs()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);
            string[] values = null;
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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

            var jobs = (from x in _db.JobsDetLoc
                        join y in _db.Job on x.JobNo equals y.OldJobNo
                        join z in _db.OrderRequests on y.CustCode equals z.CustomerCode
                        where x.ScannerDate >= stdate
                        && x.ScannerDate <= eddate
                        select new
                        {
                            CustomerCode = x.CustCode,
                            DeptCode = x.DeptCode,
                            JobNo = x.JobNo,
                            JobType = y.JobType,
                            MatType = y.CtnType,
                            RequestDate = z.OrderDate,
                            ServiceType = y.ServLevel,
                            TotalCarton = y.TotalCtn,
                            TotalMat = y.PlasticBagQty,
                            JobDate = x.CreatedDate,
                            Remark = z.Remark
                        }).ToList();
            var jobsresult = new XElement("JobReport",
            from x in jobs
            select new XElement("Job",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("DeptCode", x.DeptCode),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("RequestDate", x.RequestDate.ToString("dd/MM/yyyy")),
                                 new XElement("ServiceType", x.ServiceType),
                                 new XElement("JobType", x.JobType),
                                 new XElement("MatType", x.MatType),
                                 new XElement("TotalCarton", x.TotalCarton),
                                 new XElement("TotalMat", x.TotalMat),
                                 new XElement("JobDate", x.JobDate.ToString("dd/MM/yyyy")),
                                 new XElement("Remark", x.Remark)
                             ));
            var general = new XElement("General",
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/JobsRpt.mrt"));
            report.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("JobsReport", jobsresult)
                ));
            report.Dictionary.Synchronize();
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult CreateReportJobs()
        {
            NameValueCollection formValues = StiNetCoreDesigner.GetFormValues(this);            
            string[] values = null;
            DateTime stdate = new DateTime();
            DateTime eddate = new DateTime();
            foreach (string key in formValues.Keys)
            {
                values = formValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "startDate")
                    {
                        DateTime sdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        stdate = new DateTime(sdate.Year, sdate.Month, 1); //sdate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    if (key == "endDate")
                    {
                        DateTime edate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        eddate = new DateTime(edate.Year, edate.Month, 1); //edate.ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                }
            }
            StiReport cireport = new StiReport();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            var LastMonthStartDate = firstOfCurrentMonth.AddMonths(-1);
            var ThisMonthStartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            var LastMonthEndDate = firstOfCurrentMonth.AddMonths(defaultRange).AddDays(-1);
            var ThisMonthEndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            
            var jobs = (from x in _db.JobsDetLoc
                           join y in _db.Job on x.JobNo equals y.OldJobNo
                           join z in _db.OrderRequests on y.CustCode equals z.CustomerCode                           
                           where x.ScannerDate >= stdate
                           && x.ScannerDate <= eddate                           
                           select new
                           {
                               CustomerCode = x.CustCode,                               
                               DeptCode = x.DeptCode,
                               JobNo = x.JobNo,
                               JobType = y.JobType,
                               MatType = y.CtnType,                               
                               RequestDate = z.OrderDate,
                               ServiceType = y.ServLevel,
                               TotalCarton = y.TotalCtn,
                               TotalMat = y.PlasticBagQty,
                               JobDate = x.CreatedDate,
                               Remark = z.Remark
                           }).ToList();
            var jobsresult = new XElement("JobReport",
            from x in jobs
            select new XElement("Job",
                                 new XElement("CustomerCode", x.CustomerCode),
                                 new XElement("DeptCode", x.DeptCode),
                                 new XElement("JobNo", x.JobNo),
                                 new XElement("RequestDate", x.RequestDate.ToString("dd/MM/yyyy")),
                                 new XElement("ServiceType", x.ServiceType),
                                 new XElement("JobType", x.JobType),
                                 new XElement("MatType", x.MatType),
                                 new XElement("TotalCarton", x.TotalCarton),
                                 new XElement("TotalMat", x.TotalMat),
                                 new XElement("JobDate", x.JobDate.ToString("dd/MM/yyyy")),
                                 new XElement("Remark", x.Remark)
                             )); 
            var general = new XElement("General",                               
                               new XElement("BillStartDate", ThisMonthStartDate.ToString("dd/MM/yyyy")),
                               new XElement("BillEndDate", ThisMonthEndDate.ToString("dd/MM/yyyy")),
                               new XElement("PrintedDate", DateTime.Now.ToString("dd/MM/yyyy"))
                           );
            cireport.RegData(new XElement("Root",
                new XElement("GEN", general),
                new XElement("JobsReport", jobsresult)
                ));
            cireport.Dictionary.Synchronize();
            return StiNetCoreDesigner.GetReportResult(this, cireport);
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
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