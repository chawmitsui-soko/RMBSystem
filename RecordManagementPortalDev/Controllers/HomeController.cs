using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;

namespace RecordManagementPortalDev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public HomeController(ApplicationDbContext db, IConfiguration config, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }       

        public IActionResult DocMgnt()
        {
            return View();
        }

        public IActionResult AFMgnt()
        {
            return View();
        }

        public IActionResult MedMgnt()
        {
            return View();
        }

        public IActionResult SDMgnt()
        {
            return View();
        }

        public IActionResult BPOSource()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult VisionMission()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Location()
        {
            return View();
        }

        public IActionResult IntStandard()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //GET
        public IActionResult Contact()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact objContact)
        {
            objContact.Date = DateTime.Now; 
            var errors = ModelState.Values.SelectMany(v => v.Errors);                    
            ModelState.Remove("Phone");
            if (ModelState.IsValid)
            {
                _db.Contact.Add(objContact);
                _db.SaveChanges();
                ModelState.Clear();
                string Subject = "Inquiry from " + objContact.CompanyName;
                string Body = objContact.Messages + "<br><br>" + objContact.Name + "<br>" + objContact.Email;
                SendEmail("chaw@mitsui-soko.com.sg", Subject, Body);
                ViewData["success"] = "Your inquiry sent successfully.";
                objContact = new Contact();
                return View(objContact);
            }
            return View(); 
        }

        private void SendEmail(string email, string subject, string body)
        {
            string from = _config.GetValue<string>("smtp:Sender");
            string password = _config.GetValue<string>("smtp:Password");
            try
            {
                using (MailMessage mm = new MailMessage(from, email))
                {
                    mm.Subject = subject;
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    mm.Bcc.Add(_config.GetValue<string>("smtp:BCC"));
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = _config.GetValue<string>("smtp:Host");
                        smtp.EnableSsl = _config.GetValue<bool>("smtp:EnableSsl");
                        smtp.Port = _config.GetValue<int>("smtp:Port");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(from, password);
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
    }
}