using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Mvc;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static iTextSharp.text.pdf.PRTokeniser;
using Stimulsoft.Report.Dictionary;
using OfficeOpenXml.ConditionalFormatting;
using System.Reflection.PortableExecutable;

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
            public List<CrtnMonthlyBals> StockList { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime SelectedDate { get; set; }
            public string CartonType { get; set; }
            public int OpeningBalance { get; set; }
            public int ClosingBalance { get; set; }
            public DateTime OpeningDate { get; set; }

            public int CustAdditionalCtn { get; set; }
            public int CustDestruction { get; set; }
            public int CustPermRetrieval { get; set; }

            public string CustName { get; set; }


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
        private bool checkUser()
        {
            loginflag = _signInManager.IsSignedIn(User);
            return loginflag;
        }

        public IActionResult CustomerInvR()
        {
            ReportGen model = new ReportGen();
            var today = DateTime.Now;
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            model.StartDate = firstOfCurrentMonth.AddMonths(defaultRange);
            model.EndDate = firstOfCurrentMonth.AddMonths(1).AddDays(-1);
            var sortedCustomers = _db.Customers.OrderBy(c => c.CustomerCode).ToList();
            model.CustomerList = new SelectList(sortedCustomers, "CustomerCode", "CustomerCode");
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
        public DataSet GetCrtnMonthlyBalsDataSet(DateTime reportDate)
        {
            // Fetch the data using LINQ
            var stockList = new List<CrtnMonthlyBals>(from x in _db.CtnMonthlyClosingBal
                                                      where x.AsatDate == reportDate.AddMonths(1).AddDays(-1)
                                                      orderby x.CartonType
                                                      select x).ToList();

            // Create a DataTable and define the schema
            var dataTable = new DataTable("CtnMonthlyClosingBal");

            // Define the columns based on the properties of CrtnMonthlyBals
            dataTable.Columns.Add("AsatDate", typeof(DateTime));
            dataTable.Columns.Add("CartonType", typeof(string));
            dataTable.Columns.Add("ClosingBalance", typeof(int));
            dataTable.Columns.Add("Remarks", typeof(string));

            // Add other columns as needed, matching the properties of CrtnMonthlyBals

            // Fill the DataTable with data from tempStockList
            foreach (var item in stockList)
            {
                var row = dataTable.NewRow();
                Debug.WriteLine(item.CartonType);
                row["AsatDate"] = item.AsatDate;
                row["CartonType"] = item.CartonType;
                row["ClosingBalance"] = item.ClosingBalance;
                row["Remarks"] = item.Remarks;
                // Set other column values as needed

                dataTable.Rows.Add(row);
            }

            // Create a DataSet and add the DataTable to it
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;


        }

        public IActionResult CustInvSummary(string CustomerCode)
        {
            checkUser();
            if (loginflag == true)
            {
                ReportGen model = new ReportGen();
                //ViewBag.JobList
                model.CustomerCode = CustomerCode;

                var totalCtn = (from x in _db.CartonDetails
                                where x.CustCode == CustomerCode
                                select x).Count();

                var totalDest = (from x in _db.CartonDetails
                                 where x.CustCode == CustomerCode
                                 && x.Status == "DESTRUCT"
                                 && x.Permanentstored
                                 select x).Count();

                var totalPerm = (from x in _db.CartonDetails
                                 where x.CustCode == CustomerCode
                                 && x.Status == "PULLED"
                                 && x.Permanentstored
                                 select x).Count();


                model.CustAdditionalCtn = totalCtn;
                model.CustDestruction = totalDest;
                model.CustPermRetrieval = totalPerm;
                model.CustName = (from x in _db.Customers
                                  where x.CustomerCode == CustomerCode
                                  select x.CustomerName).FirstOrDefault();

                return PartialView("CustInvSummary", model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public IActionResult GenerateCustInventory(string CustomerCode)
        {
            checkUser();
            if (loginflag == true)
            {

                // Load the report template
                var report = new StiReport();
                report.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "CustomerInventory.mrt"));
                report.Dictionary.Variables["customerCode"].Value = CustomerCode;

                // Synchronize the report dictionary with the registered data
                report.Dictionary.Synchronize();

                // Render the report
                report.Render();

                //        // Set Excel export settings
                //StiExcelExportSettings settings = new StiExcelExportSettings
                //{
                //    ExcelType = StiExcelType.Excel2007, // Set the type of Excel file
                //    UseOnePageHeaderAndFooter = true, // Use one page header and footer
                //    DataExportMode = StiDataExportMode.DataAndHeaders, // Export data and headers
                //    ExportPageBreaks = true, // Include page breaks
                //    ExportObjectFormatting = true, // Include object formatting
                //    ExportEachPageToSheet = false, // Export each page to a separate sheet
                //    ImageQuality = 0.75f, // Set image quality
                //    ImageResolution = 100f, // Set image resolution
                //    CompanyString = "My Company", // Set company string
                //    LastModifiedString = "John Doe", // Set last modified string
                //    RestrictEditing = StiExcel2007RestrictEditing.No // No editing restrictions
                //};

                //// Export the report to Excel
                //var stream = new MemoryStream();
                //report.ExportDocument(StiExportFormat.Excel2007, stream, settings);
                //stream.Position = 0;

                //// Return the file result
                //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{CustomerCode}_Inventory_Report_{DateTime.Today:yyyyMMdd}.xlsx");


                // Set PDF export settings
                StiPdfExportSettings settings = new StiPdfExportSettings
                {
                    ImageQuality = 1.0F, // Maximum quality (1.0 = 100%, 0.0 = 0%)
                    ImageResolution = 300,
                    EmbeddedFonts = true,
                    Compressed = true,
                    UseUnicode = true
                };

                // Export the report to PDF
                var stream = new MemoryStream();
                report.ExportDocument(StiExportFormat.Pdf, stream, settings);
                stream.Position = 0;

                // Return the file result
                return File(stream, "application/pdf", $"{CustomerCode}_Inventory_Report_{DateTime.Today:yyyyMMdd}.pdf");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        public IActionResult GenerateStockReport(DateTime selectedDate)
        {
            checkUser();
            if (loginflag == true)
            {
                // Load the report template
                var report = new StiReport();
                report.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StockReport.mrt"));

                var closingDate = selectedDate.AddMonths(1).AddDays(-1);
                report.Dictionary.Variables["reportDate"].Value = closingDate.ToString("yyyy-MM-dd");
                // Register the filtered DataSet with the report

                // Synchronize the report dictionary with the registered data
                report.Dictionary.Synchronize();

                // Render the report
                report.Render();

                // Set PDF export settings
                StiPdfExportSettings settings = new StiPdfExportSettings
                {
                    ImageQuality = 1.0F, // Maximum quality (1.0 = 100%, 0.0 = 0%)
                    ImageResolution = 300,
                    EmbeddedFonts = true,
                    Compressed = true,
                    UseUnicode = true
                };

                // Export the report to PDF
                var stream = new MemoryStream();
                report.ExportDocument(StiExportFormat.Pdf, stream, settings);
                stream.Position = 0;

                var fileCloseDate = closingDate > DateTime.Today ? DateTime.Today : closingDate;

                // Return the file result
                return File(stream, "application/pdf", $"RMB_Stock_Report_{fileCloseDate:yyyyMMdd}.pdf");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }

        }
        public IActionResult GenerateTransReport(DateTime selectedDate)
        {
            checkUser();
            if (loginflag == true)
            {

                var stockList = new List<CrtnMonthlyBals>(from x in _db.CtnMonthlyClosingBal
                                                          where x.AsatDate == selectedDate.AddMonths(1).AddDays(-1)
                                                          orderby x.CartonType
                                                          select x).ToList();

                List<string> reportPaths = new List<string>();

                var closingDate = selectedDate.AddDays(-1);

                StiPdfExportSettings settings = new StiPdfExportSettings
                {
                    ImageQuality = 1.0F, // Maximum quality (1.0 = 100%, 0.0 = 0%)
                    ImageResolution = 300,
                    EmbeddedFonts = true,
                    Compressed = true,
                    UseUnicode = true
                };


                foreach (var stock in stockList)
                {
                    var monthlyTransSum = (from x in _db.Job
                                           where x.RequestDate.Month == selectedDate.Month
                                              && x.RequestDate.Year == selectedDate.Year
                                              && x.CtnType == stock.CartonType
                                              && x.JobType == "D1"
                                              && x.JobLevel == "Empty Cartons Only"
                                           select x.TotalCtn).Sum();

                    var monthlyReceiving = (from x in _db.CtnStockReceiving
                                            where x.ReceivingDate.Month == selectedDate.Month
                                               && x.ReceivingDate.Year == selectedDate.Year
                                               && x.CartonType == stock.CartonType
                                            select x.Qty).Sum();

                    if (monthlyReceiving > 0 || monthlyTransSum > 0)
                    {
                        // Load the report template
                        var report = new StiReport();
                        report.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StockTransReport.mrt"));

                        report.Dictionary.Variables["openingDate"].Value = selectedDate.ToString("yyyy-MM-dd");
                        report.Dictionary.Variables["currentClosingDate"].Value = selectedDate.AddMonths(1).AddDays(-1) > DateTime.Today ? DateTime.Today.ToString("yyyy-MM-dd") : selectedDate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        report.Dictionary.Variables["lastClosingDate"].Value = closingDate.ToString("yyyy-MM-dd");
                        report.Dictionary.Variables["stockType"].Value = stock.CartonType.Trim();

                        Debug.WriteLine(report.Dictionary.Variables["stockType"].Value);

                        // Register the filtered DataSet with the report

                        // Synchronize the report dictionary with the registered data
                        report.Dictionary.Synchronize();

                        // Render the report
                        report.Render();

                        // Export the report to PDF and save it to a file
                        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports","Reports", $"StockTransReport_{stock.CartonType.Trim()}.pdf");
                        report.ExportDocument(StiExportFormat.Pdf, reportPath, settings);

                        // Add the file path to the list
                        reportPaths.Add(reportPath);

                    }

                }

                // Load the report template
                var no_stock_report = new StiReport();
                no_stock_report.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "NoStockReport.mrt"));

                no_stock_report.Dictionary.Variables["openingDate"].Value = selectedDate.ToString("yyyy-MM-dd");
                no_stock_report.Dictionary.Variables["closingDate"].Value = selectedDate > DateTime.Today ? DateTime.Today.ToString("yyyy-MM-dd") : selectedDate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");


                // Register the filtered DataSet with the report

                // Synchronize the report dictionary with the registered data
                no_stock_report.Dictionary.Synchronize();

                // Render the report
                no_stock_report.Render();


                // Export the report to PDF and save it to a file
                var no_stock_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "Reports", $"No_Stock.pdf");
                no_stock_report.ExportDocument(StiExportFormat.Pdf, no_stock_path, settings);

                // Add the file path to the list
                reportPaths.Add(no_stock_path);


                // Combine all individual PDF files into one PDF
                var combinedPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "Reports", $"Stock_Transaction_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf");

                using (var fs = new FileStream(combinedPath, FileMode.Create))
                {
                    using (var document = new Document())
                    {
                        using (var writer = new PdfCopy(document, fs))
                        {
                            document.Open();

                            foreach (var reportPath in reportPaths)
                            {
                                var reader = new PdfReader(reportPath);
                                int n = reader.NumberOfPages;

                                for (int i = 1; i <= n; i++)
                                {
                                    document.NewPage();
                                    var page = writer.GetImportedPage(reader, i);
                                    writer.AddPage(page);
                                }

                                reader.Close();
                            }

                            document.Close();
                        }
                    }
                }

                // Return the combined PDF file result
                var fileBytes = System.IO.File.ReadAllBytes(combinedPath);
                return File(fileBytes, "application/pdf", Path.GetFileName(combinedPath));

            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }

        }

        public IActionResult PrintStkR(ReportGen model)
        {
            checkUser();
            if (loginflag == true)
            {
                if (model.SelectedDate < new DateTime(1900, 01, 01))
                {
                    model.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                }
                else
                {
                    if (model.SelectedDate.Month != DateTime.Now.Month || model.SelectedDate.Year != DateTime.Now.Year)
                    {
                        model.SelectedDate = new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        model.SelectedDate = DateTime.Now;
                    }
                }

                var last_day_of_selected = new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddMonths(1).AddDays(-1);

                var tempStockList = new List<CrtnMonthlyBals>(from x in _db.CtnMonthlyClosingBal
                                                              where x.AsatDate == last_day_of_selected
                                                              orderby x.CartonType
                                                              select x).ToList();
                Debug.WriteLine(tempStockList.Count);
                if (tempStockList.Count == 0)
                {
                    var last_stock_date = new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddDays(-1);


                    // Query the second table to get the items to add
                    var newStockList = (from y in _db.CtnMonthlyClosingBal
                                        where y.AsatDate == last_stock_date // Replace with your actual condition
                                        select y).ToList();

                    // Create a copy of newStockList and update AsatDate for each item
                    var copiedStockList = new List<CrtnMonthlyBals>();

                    foreach (var stock in newStockList)
                    {
                        var copiedStock = new CrtnMonthlyBals
                        {
                            CartonType = stock.CartonType,
                            ClosingBalance = stock.ClosingBalance,
                            // Copy other properties as needed
                            AsatDate = last_day_of_selected,
                            Remarks = stock.Remarks// Update AsatDate to the new value
                        };

                        copiedStockList.Add(copiedStock);
                    }

                    foreach (var stock in copiedStockList)
                    {
                        _db.CtnMonthlyClosingBal.Add(stock);
                    }

                    _db.SaveChanges();

                    tempStockList.AddRange(copiedStockList);
                }


                if (last_day_of_selected.Month != DateTime.Now.Month || last_day_of_selected.Year != DateTime.Now.Year)
                {
                    model.StockList = tempStockList;
                }
                else
                {
                    foreach (var stock in tempStockList)
                    {
                        var monthlyTransSum = (from x in _db.Job
                                               where x.RequestDate.Month == model.SelectedDate.Month
                                                   && x.RequestDate.Year == model.SelectedDate.Year
                                                   && x.CtnType == stock.CartonType
                                                   && x.JobType == "D1"
                                                   && x.JobLevel == "Empty Cartons Only"
                                               orderby x.RequestDate
                                               select x.TotalCtn
                                        ).Sum();


                        var monthlyReceiving = (from x in _db.CtnStockReceiving
                                                where x.ReceivingDate.Month == model.SelectedDate.Month
                                                    && x.ReceivingDate.Year == model.SelectedDate.Year
                                                    && x.CartonType == stock.CartonType
                                                select x.Qty).Sum();


                        var closingBalEntity = (from x in _db.CtnMonthlyClosingBal
                                                where x.AsatDate == new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddMonths(1).AddDays(-1)
                                                && x.CartonType == stock.CartonType
                                                select x).FirstOrDefault();

                        var openingBalance = (from x in _db.CtnMonthlyClosingBal
                                              where x.AsatDate == new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddDays(-1)
                                              && x.CartonType == stock.CartonType
                                              select x.ClosingBalance).FirstOrDefault();



                        closingBalEntity.ClosingBalance = openingBalance - monthlyTransSum + monthlyReceiving;
                        _db.SaveChanges();
                    }

                    model.StockList = new List<CrtnMonthlyBals>(from x in _db.CtnMonthlyClosingBal
                                                                where x.AsatDate == last_day_of_selected
                                                                orderby x.CartonType
                                                                select x).ToList();
                }

                //IEnumerable<CrtnType> objCrtnList = _db.CartonType.OrderBy(x => x.CtnType).ToList();
                return View(model);

            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }

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

        public IActionResult CtnTypeTrans(string ctntype, string date)
        {
            checkUser();
            if (loginflag == true)
            {
                ReportGen model = new ReportGen();
                model.CartonType = ctntype.Trim();
                if (int.Parse(date.Split("-")[0]) == DateTime.Now.Year && int.Parse(date.Split("-")[1]) == DateTime.Now.Month)
                {
                    model.SelectedDate = DateTime.Now;
                }
                else
                {
                    model.SelectedDate = new DateTime(int.Parse(date.Split("-")[0]), int.Parse(date.Split("-")[1]), 1).AddMonths(1).AddDays(-1);
                }
                // DO YOUR STUFF. 
                var last_closing_date = new DateTime(model.SelectedDate.Year, model.SelectedDate.Month, 1).AddDays(-1);
                model.OpeningDate = last_closing_date.AddDays(1);
                model.OpeningBalance = (from x in _db.CtnMonthlyClosingBal
                                        where x.AsatDate == last_closing_date && x.CartonType == model.CartonType
                                        select x.ClosingBalance).FirstOrDefault();

                var JobList = (from x in _db.Job
                               where x.RequestDate.Month == model.SelectedDate.Month
                                  && x.RequestDate.Year == model.SelectedDate.Year
                                  && x.CtnType == model.CartonType
                                  && x.JobType == "D1"
                                  && x.JobLevel == "Empty Cartons Only"
                               orderby x.RequestDate
                               select new
                               {
                                   x.RequestDate,
                                   Desc = "JOB",
                                   x.OldJobNo,
                                   x.CustCode,
                                   x.TotalCtn,
                                   Remarks = (string)null
                               }).ToList();

                var ReceivingList = (from x in _db.CtnStockReceiving
                                     where x.ReceivingDate.Month == model.SelectedDate.Month
                                        && x.ReceivingDate.Year == model.SelectedDate.Year
                                        && x.CartonType == model.CartonType
                                     orderby x.ReceivingDate
                                     select new
                                     {
                                         RequestDate = x.ReceivingDate,
                                         Desc = "RECEIVING",
                                         OldJobNo = (string)null, // Placeholder, since it doesn't exist in Receiving
                                         CustCode = (string)null, // Placeholder, since it doesn't exist in Receiving
                                         TotalCtn = x.Qty,
                                         x.Remarks,
                                     }).ToList();

                // Combine the two lists
                var CombinedList = JobList.Concat(ReceivingList).OrderBy(x => x.RequestDate).ToList();

                ViewBag.JobList = CombinedList;

                model.ClosingBalance = (from x in _db.CtnMonthlyClosingBal
                                        where x.AsatDate == model.OpeningDate.AddMonths(1).AddDays(-1) && x.CartonType == model.CartonType
                                        select x.ClosingBalance).FirstOrDefault();




                return PartialView("CtnTypeTrans", model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
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
                               select new
                               {
                                   x,
                                   z,
                                   a
                               }).ToList();
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
            Debug.WriteLine(formValues[0]);
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