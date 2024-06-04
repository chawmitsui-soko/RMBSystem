using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecordManagementPortalDev.Data;
using RecordManagementPortalDev.Models;
using System.Data.OleDb;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using IdentityModel;

namespace RecordManagementPortalDev.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private bool loginflag;

        public class ApprovalUsers
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string CustomerCode { get; set; }
            public string ApproverName { get; set; }
            public string SubApproverName { get; set; }
            public string Email { get; set; }
        }
        public class GroupedUserViewModel
        {
            public List<ApplicationUser> AppUser { get; set; }
            public ApplicationUser User { get; set; }
            public SelectList DepartmentList { get; set; }
            public SelectList CustomerList { get; set; }
        }

        public class ApvUserViewModel
        {
            //public List<ApprovalSetup> ListApvUser { get; set; }
            public List<ApprovalUsers> ListApprovalUser { get; set; }
            public SelectList CustomerList { get; set; }
            public string Email { get; set; }
            public ApprovalSetup ApvUser { get; set; }
            public SelectList UserList { get; set; }
            public ApplicationUser User { get; set; }
            public SelectList ApproverList { get; set; }
            public SelectList SubApproverList { get; set; }
            public Customer Customer { get; set; }
        }

        public class GroupedCustomerBillModel
        {
            public BillRateMaster BillRate { get; set; }
            public List<BillRateMaster> BillRateList { get; set; }
            public SelectList CustomerList { get; set; }

            public SelectList CartonTypeList { get; set; }

            public CrtnType CrtnType { get; set; }
            public Customer Customer { get; set; }
        }

        public class GroupedDepartmentModel
        {
            public Department Department { get; set; }
            public List<DepartmentInfo> DeptList { get; set; }
            public SelectList CustomerList { get; set; }
        }

        public class DepartmentInfo
        {
            public int Id { get; set; }            
            public string CustomerCode { get; set; }            
            public string DepartmentCode { get; set; }            
            public string Description { get; set; }
            public int CustDept { get; set; }
        }
        
        public CustomerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        private IWebHostEnvironment Environment;
        private bool checkUser()
        {
            loginflag = _signInManager.IsSignedIn(User);
            return loginflag;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Customer(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var fromDb = _db.AppUsers.Find(Id.ToString());
            if (fromDb == null)
            {
                return NotFound();
            }
            else
            {
                var objCustomerList = _db.Customers.FirstOrDefault(y => y.CustomerCode == fromDb.CustomerCode);
                if (objCustomerList == null)
                {
                    return NotFound();
                }
                return View(objCustomerList);
            }
        }

        public IActionResult RIC()
        {
            string filePath = "~/RIC.doc";
            Response.Headers.Add("Content-Disposition", "inline; filename=RIC.doc");
            return File(filePath, "application/doc");
        }

        public IActionResult UserGuide()
        {
            string filePath = "~/UserGuide.pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=UserGuide.pdf");
            return File(filePath, "application/pdf");
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public async Task<IActionResult> RejectAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/Home/Index");
        }

        public IActionResult CustomerInfo(int page, string keywords = "")
        {
            checkUser();
            if (loginflag == true)
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    page = 1;
                    ViewData["keywords"] = keywords;
                    int recsCount = _db.Customers.Where(x => x.CustomerCode.Contains(keywords) || x.CustomerName.Contains(keywords)).Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;

                    var objCustomerList = (from x in _db.Customers
                                           where x.CustomerCode.Contains(keywords)
                                           || x.CustomerName.Contains(keywords)
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(objCustomerList);
                }
                else
                {
                    int recsCount = _db.Customers.Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;

                    var objCustomerList = (from x in _db.Customers
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    //IEnumerable<Customer> objCustomerList = _db.Customers;
                    return View(objCustomerList);
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //GET
        public IActionResult CreateCustomer()
        {
            checkUser();
            if (loginflag == true)
            {
                return View();
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(Customer objCustomer)
        {
            checkUser();
            if (loginflag == true)
            {
                var existing = _db.Customers.Where(x => x.CustomerCode == objCustomer.CustomerCode).FirstOrDefault();
                if (existing == null)
                {
                    objCustomer.CreatedBy = "RMB Super Admin".ToString();
                    objCustomer.CreatedDate = DateTime.Now;
                    objCustomer.UpdatedBy = "RMB Super Admin".ToString();
                    objCustomer.UpdatedDate = DateTime.Now;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ModelState.Remove("CreatedBy");
                    ModelState.Remove("UpdatedBy");
                    ModelState.Remove("Address3");
                    ModelState.Remove("Fax");
                    ModelState.Remove("FMSJMSInvoiceCode");
                    if (ModelState.IsValid)
                    {
                        _db.Customers.Add(objCustomer);
                        _db.SaveChanges();
                        TempData["success"] = "Customer created successfully.";
                        return RedirectToAction("CustomerInfo");
                    }
                }
                else
                {
                    TempData["error"] = "Customer already existed.";
                    //return RedirectToAction("CustomerInfo");
                }
                return View(objCustomer);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public HttpRequest GetRequest()
        {
            return Request;
        }
        //GET
        public IActionResult DetailCustomer(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.Customers.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult EditCustomer(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.Customers.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(Customer objCustomer)
        {
            checkUser();
            if (loginflag == true)
            {
                objCustomer.CreatedBy = "RMB Super Admin".ToString();
                //objCustomer.CreatedDate = DateTime.Now;
                objCustomer.UpdatedBy = "RMB Super Admin".ToString();
                objCustomer.UpdatedDate = DateTime.Now;
                //objCustomer.CreatedDate = 
                ModelState.Remove("CreatedBy");
                ModelState.Remove("UpdatedBy");
                if (ModelState.IsValid)
                {
                    _db.Customers.Update(objCustomer);
                    _db.SaveChanges();
                    TempData["success"] = "Customer updated successfully.";
                    return RedirectToAction("CustomerInfo");
                }
                return View(objCustomer);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DeleteCustomer(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                var fromDb = _db.Customers.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostCustomer(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var obj = _db.Customers.Find(Id);
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                _db.Customers.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Customer deleted successfully.";
                return RedirectToAction("CustomerInfo");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public async Task<IActionResult> DepartmentInfoAsync(int page, string keywords = "")
        {
            checkUser();
            if (loginflag == true)
            {
                var loginuser = await _userManager.GetUserAsync(User);
                string userRole = loginuser.UserRole;
                GroupedDepartmentModel model = new GroupedDepartmentModel();
                if (userRole == "RMB Super Admin")
                {
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        page = 1;
                        ViewData["keywords"] = keywords;
                        int recsCount = _db.Departments.Where(x => x.DepartmentCode.Contains(keywords) || x.Description.Contains(keywords)).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objDeptList = (from x in _db.Departments
                                           join y in _db.JobsDetLoc.Where(a => a.Status == "Stored") on x.DepartmentCode equals y.DeptCode into a
                                           from z in a.DefaultIfEmpty()
                                           where x.DepartmentCode.Contains(keywords)
                                           || x.Description.Contains(keywords)
                                           orderby x.CustomerCode descending
                                           group new { x, z } by new
                                           {
                                               x.DepartmentCode,
                                               x.Description,
                                               x.CustomerCode,
                                               z.CustCode,
                                               z.DeptCode
                                           } into gp
                                           select new DepartmentInfo                                          
                                           {
                                               CustomerCode = gp.Key.CustomerCode,
                                               DepartmentCode = gp.Key.DepartmentCode,
                                               Description = gp.Key.Description,
                                               CustDept = (gp.Key.DeptCode == gp.Key.DepartmentCode) ? 1 : 0
                                           }).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        //IEnumerable<Department>? objDeptList = _db.Departments.ToList();
                        model.DeptList = (List<DepartmentInfo>)objDeptList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                    else
                    {
                        int recsCount = _db.Departments.Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objDeptList = (from x in _db.Departments
                                           join y in _db.JobsDetLoc.Where(a => a.Status == "Stored") on x.DepartmentCode equals y.DeptCode into a
                                           from z in a.DefaultIfEmpty()
                                           where x.CustomerCode == z.CustCode
                                           orderby x.CustomerCode descending
                                           group new { x, z } by new
                                           {
                                               x.DepartmentCode,
                                               x.Description,
                                               x.CustomerCode,
                                               z.CustCode,
                                               z.DeptCode
                                           } into gp
                                           select new DepartmentInfo
                                           {
                                               CustomerCode = gp.Key.CustomerCode,
                                               DepartmentCode = gp.Key.DepartmentCode,
                                               Description = gp.Key.Description,
                                               CustDept = (gp.Key.CustCode == gp.Key.CustomerCode && gp.Key.DeptCode == gp.Key.DepartmentCode) ? 1 : 0
                                           }).Skip(recSkip).Take(pager.PageSize).ToList();                        
                        this.ViewBag.Pager = pager;
                        //IEnumerable<Department>? objDeptList = _db.Departments.ToList();
                        model.DeptList = (List<DepartmentInfo>)objDeptList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        page = 1;
                        ViewData["keywords"] = keywords;
                        int recsCount = _db.Departments.Where(x => x.CustomerCode == loginuser.CustomerCode && (x.DepartmentCode.Contains(keywords) || x.Description.Contains(keywords))).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objDeptList = (from x in _db.Departments
                                           join y in _db.JobsDetLoc.Where(a => a.Status == "Stored") on x.DepartmentCode equals y.DeptCode into a
                                           from z in a.DefaultIfEmpty()
                                           where x.CustomerCode == loginuser.CustomerCode
                                           && (x.DepartmentCode.Contains(keywords)
                                           || x.Description.Contains(keywords))
                                           orderby x.CustomerCode descending
                                           group new { x, z } by new
                                           {
                                               x.DepartmentCode,
                                               x.Description,
                                               x.CustomerCode,
                                               z.CustCode,
                                               z.DeptCode
                                           } into gp
                                           select new DepartmentInfo
                                           {
                                               CustomerCode = gp.Key.CustomerCode,
                                               DepartmentCode = gp.Key.DepartmentCode,
                                               Description = gp.Key.Description,
                                               CustDept = (gp.Key.CustCode == gp.Key.CustomerCode && gp.Key.DeptCode == gp.Key.DepartmentCode) ? 1 : 0
                                           }).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        //IEnumerable<Department>? objDeptList = _db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList();
                        model.DeptList = (List<DepartmentInfo>)objDeptList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                    else
                    {
                        int recsCount = _db.Departments.Where(x => x.CustomerCode == loginuser.CustomerCode).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objDeptList = (from x in _db.Departments
                                           join y in _db.JobsDetLoc.Where(a => a.Status == "Stored") on x.DepartmentCode equals y.DeptCode into a
                                           from z in a.DefaultIfEmpty()
                                           where x.CustomerCode == loginuser.CustomerCode
                                           orderby x.CustomerCode descending
                                           group new { x, z } by new
                                           {
                                               x.DepartmentCode,
                                               x.Description,
                                               x.CustomerCode,
                                               z.CustCode,
                                               z.DeptCode
                                           } into gp
                                           select new DepartmentInfo
                                           {
                                               CustomerCode = gp.Key.CustomerCode,
                                               DepartmentCode = gp.Key.DepartmentCode,
                                               Description = gp.Key.Description,
                                               CustDept = (gp.Key.CustCode == gp.Key.CustomerCode && gp.Key.DeptCode == gp.Key.DepartmentCode) ? 1 : 0
                                           }).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        //IEnumerable<Department>? objDeptList = _db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList();
                        model.DeptList = (List<DepartmentInfo>)objDeptList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult CreateDepartment()
        {
            checkUser();
            if (loginflag == true)
            {
                GroupedDepartmentModel model = new GroupedDepartmentModel();
                model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartmentAsync(GroupedDepartmentModel objDepartment)
        {
            checkUser();
            if (loginflag == true)
            {

                var loginuser = await _userManager.GetUserAsync(User);
                objDepartment.Department.CreatedBy = loginuser.UserName;
                objDepartment.Department.UpdatedBy = loginuser.UserName;
                objDepartment.Department.UpdatedDate = DateTime.Now;
                objDepartment.Department.CreatedDate = DateTime.Now;
                ModelState.Remove("Department.CustomerCode");
                ModelState.Remove("CustomerList");
                ModelState.Remove("DeptList");
                ModelState.Remove("Department.CreatedBy");
                ModelState.Remove("Department.UpdatedBy");
                if (ModelState.IsValid)
                {
                    if (objDepartment.Department.CustomerCode == null)
                    {
                        objDepartment.Department.CustomerCode = Request.Form["CustomerCode"];
                    }
                    var existing = _db.Departments.Where(x => x.DepartmentCode == objDepartment.Department.DepartmentCode && x.CustomerCode == objDepartment.Department.CustomerCode).FirstOrDefault();
                    if (existing == null)
                    {
                        objDepartment.Department.DepartmentCode = objDepartment.Department.DepartmentCode.ToUpper();
                        _db.Departments.Add(objDepartment.Department);
                        _db.SaveChanges();
                        TempData["success"] = "Department created successfully.";
                        return RedirectToAction("DepartmentInfo");
                    }
                    else
                    {
                        TempData["error"] = "Department already existed.";
                        GroupedDepartmentModel model = new GroupedDepartmentModel();
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                        //return RedirectToAction("DepartmentInfo");
                    }
                }
                return View(objDepartment);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //GET
        public IActionResult EditDepartment(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.Departments.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartmentAsync(Department objDepartment)
        {
            checkUser();
            if (loginflag == true)
            {
                var loginuser = await _userManager.GetUserAsync(User);
                objDepartment.CreatedBy = loginuser.UserName;
                objDepartment.CreatedDate = DateTime.Now;
                objDepartment.UpdatedBy = loginuser.UserName;
                objDepartment.UpdatedDate = DateTime.Now;
                ModelState.Remove("CreatedBy");
                ModelState.Remove("UpdatedBy");
                if (ModelState.IsValid)
                {
                    objDepartment.DepartmentCode = objDepartment.DepartmentCode.ToUpper();
                    _db.Departments.Update(objDepartment);
                    _db.SaveChanges();
                    TempData["success"] = "Department updated successfully.";
                    return RedirectToAction("DepartmentInfo");
                }
                return View(objDepartment);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DeleteDepartment(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                var fromDb = _db.Departments.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                var NonDelete = _db.JobsDetLoc.Where(x => x.DeptCode == fromDb.DepartmentCode).FirstOrDefault();
                if (NonDelete != null)
				{
                    TempData["error"] = "Department is in used.";
                    return RedirectToAction("DepartmentInfo");
                }  
                else
                    return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteDepartment")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostDepartment(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var obj = _db.Departments.Find(Id);
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                _db.Departments.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Department deleted successfully.";
                return RedirectToAction("DepartmentInfo");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        [HttpPost]
        public JsonResult GetDept(string custcode)
        {
            SelectList DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == custcode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
            return Json(DepartmentList);
        }
        public JsonResult DeptByCust(string custcode)
        {

            if (custcode == "0")
            {
                IEnumerable<Department> objDeptList = _db.Departments.ToList();
                return Json(objDeptList);
            }
            else
            {
                IEnumerable<Department> objDeptList = _db.Departments.Where(y => y.CustomerCode == custcode).ToList();
                return Json(objDeptList);
            }
        }

        public JsonResult GenPassword()
        {
            PasswordOptions opts = new PasswordOptions()
            {
                RequiredLength = 12,                
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };
            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
                };
            CryptoRandom rand = new CryptoRandom();
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }
            return Json(chars.ToArray());
        }

        public JsonResult UserInfoByCustAndDept(string custcode, string deptcode, int page)
        {
            if (deptcode == null)
            {
                if (custcode == "0")
                {
                    int recsCount = _db.AppUsers.Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;
                    IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return Json(objUserList);
                }
                else
                {
                    int recsCount = _db.AppUsers.Where(y => y.CustomerCode == custcode).Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;
                    IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Where(y => y.CustomerCode == custcode).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return Json(objUserList);
                }

            }
            else
            {
                if (deptcode == "0")
                {
                    int recsCount = _db.AppUsers.Where(y => y.CustomerCode == custcode).Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;
                    IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Where(y => y.CustomerCode == custcode).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return Json(objUserList);
                }
                else
                {
                    int recsCount = _db.AppUsers.Where(y => y.CustomerCode == custcode && y.DepartmentCode == deptcode).Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;
                    IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Where(y => y.CustomerCode == custcode && y.DepartmentCode == deptcode).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return Json(objUserList);
                }
            }
        }

        public async Task<IActionResult> UserInfoAsync(int page, string keywords = "")
        {
            checkUser();
            if (loginflag == true)
            {
                var loginuser = await _userManager.GetUserAsync(User);
                GroupedUserViewModel model = new GroupedUserViewModel();
                string userRole = loginuser.UserRole;

                if (userRole == "RMB Super Admin")
                {
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        page = 1;
                        ViewData["keywords"] = keywords;
                        int recsCount = _db.Users.Where(x => x.UserCode.Contains(keywords) || x.UserName.Contains(keywords)).Count();
                        //int recsCount = _db.Customers.Where(x => x.CustomerCode.Contains(keywords) || x.CustomerName.Contains(keywords)).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;
                        var objUserList = (from x in _db.Users
                                           where x.UserCode.Contains(keywords)
                                               || x.UserName.Contains(keywords)
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        //IEnumerable<ApplicationUser> objUserList = _db.AppUsers.OrderBy(x => x.UserCode).ToList();
                        model.AppUser = (List<ApplicationUser>)objUserList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                    else
                    {
                        int recsCount = _db.Users.Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objUserList = (from x in _db.Users
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        //IEnumerable<ApplicationUser> objUserList = _db.AppUsers.OrderBy(x => x.UserCode).ToList();
                        model.AppUser = (List<ApplicationUser>)objUserList;
                        model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                        return View(model);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        page = 1;
                        ViewData["keywords"] = keywords;
                        int recsCount = _db.Users.Where(x => x.CustomerCode == loginuser.CustomerCode && (x.UserCode.Contains(keywords) || x.UserName.Contains(keywords))).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objUserList = (from x in _db.Users
                                           where x.CustomerCode == loginuser.CustomerCode
                                           && (x.UserCode.Contains(keywords)
                                               || x.UserName.Contains(keywords))
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        model.DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
                        //IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList();
                        model.AppUser = (List<ApplicationUser>)objUserList;
                        return View(model);
                    }
                    else
                    {
                        int recsCount = _db.Users.Where(x => x.CustomerCode == loginuser.CustomerCode).Count();
                        const int pageSize = 10;
                        if (page < 1)
                            page = 1;

                        var pager = new Pager(recsCount, page, pageSize);
                        int recSkip = (page - 1) * pageSize;

                        var objUserList = (from x in _db.Users
                                           where x.CustomerCode == loginuser.CustomerCode
                                           orderby x.CreatedDate descending
                                           select x).Skip(recSkip).Take(pager.PageSize).ToList();
                        this.ViewBag.Pager = pager;
                        model.DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
                        //IEnumerable<ApplicationUser> objUserList = _db.AppUsers.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList();
                        model.AppUser = (List<ApplicationUser>)objUserList;
                        return View(model);
                    }
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public IActionResult DetailUser(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.AppUsers.Find(Id.ToString());
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //GET
        public IActionResult EditUser(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.AppUsers.Find(Id.ToString());
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                GroupedUserViewModel model = new GroupedUserViewModel();
                model.User = fromDb;
                model.DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == fromDb.CustomerCode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserAsync(GroupedUserViewModel model, Guid Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var loginuser = await _userManager.GetUserAsync(User);
                ModelState.Remove("CreatedBy");
                ModelState.Remove("UpdatedBy");
                ApplicationUser? fromDb = _db.AppUsers.Find(Id.ToString());
                //if (ModelState.IsValid)
                //{
                if (fromDb != null)
                {
                    loginuser.CreatedBy = loginuser.UserName;
                    fromDb.CreatedDate = DateTime.Now;
                    fromDb.UpdatedBy = loginuser.UserName;
                    fromDb.UpdatedDate = DateTime.Now;
                    fromDb.UserCode = model.User.UserCode;
                    fromDb.Name = model.User.Name;
                    fromDb.UserRole = model.User.UserRole;
                    fromDb.UserLevel = model.User.UserLevel;
                    if (fromDb.DepartmentPermission == Request.Form["DepartmentPermission"].ToString())
                    {
                        fromDb.DepartmentPermission = Request.Form["DepartmentPermission"].ToString();
                    }
                    else
                    {
                        if (Request.Form["DepartmentPermission"].ToString() != "None")
                        {
                            fromDb.DepartmentPermission = "";
                            for (int i = 0; i < Request.Form["DepartmentPermission"].Count - 1; i++)
                            {
                                fromDb.DepartmentPermission += Request.Form["DepartmentPermission"][i] + ", ";
                            }
                            fromDb.DepartmentPermission = fromDb.DepartmentPermission.Remove(fromDb.DepartmentPermission.Length - 2);

                        }
                        else
                        {
                            fromDb.DepartmentPermission = "";
                        }
                    }
                    if (model.User.RequestService == true || model.User.RequestDestruct == true || model.User.AppDestructService == true)
                    {
                        fromDb.RequestService = model.User.RequestService;
                        fromDb.RequestDestruct = model.User.RequestDestruct;
                        fromDb.AppDestructService = model.User.AppDestructService;
                    }
                    else
                    {
                        TempData["error"] = "At least one checkbox was checked in Services!";
                        model.DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == fromDb.CustomerCode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
                        return View(model);
                    }
                    if (model.User.ViewFContent == true || model.User.AmendFContent == true || model.User.UploadRIC == true || model.User.CanExport == true)
                    {
                        fromDb.ViewFContent = model.User.ViewFContent;
                        fromDb.AmendFContent = model.User.AmendFContent;
                        fromDb.UploadRIC = model.User.UploadRIC;
                        fromDb.CanExport = model.User.CanExport;
                    }
                    else
                    {
                        TempData["error"] = "At least one checkbox was checked in Controls!";
                        model.DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == fromDb.CustomerCode).OrderBy(x => x.DepartmentCode).ToList(), "DepartmentCode", "DepartmentCode");
                        return View(model);
                    }
                    //fromDb.NeedApproval = model.User.NeedApproval;
                    fromDb.Email = model.User.Email;
                    _db.AppUsers.Update(fromDb);
                    _db.SaveChanges();
                }
                TempData["success"] = "User updated successfully.";
                string Subject = "Mitsui-Soko RMP User Update";
                string Body = "<div style='font-size: 13px'>Dear " + model.User.Name + ", <br>"
                        + "Welcome to Mitsui-Soko Record Management! You have already updated the user profile:"
                        + "<br>"
                        + "<br>"
                        + "User Name: " + model.User.UserCode + "<br>"
                        + "<br>"
                        + "Yours, <br>"
                        + "Mitsui-Soko Records Management <br></div>"
                        + "<p style='padding: 0; font-size:11px; color:#375a7f;'>Remark: This email is auto generated by Mitsui-Soko Records Management System. " +
                        "This message including any attachments may contain confidential " +
                        "or legally privileged information. Any unauthorized use, retention, reproduction " +
                        "or disclosure is prohibited and may attract civil and criminal penalties. If this e-mail has been sent to you in error, please delete it and notify us immediately."
                        + "</p>";
                SendEmail(model.User.Email, Subject, Body);
                return RedirectToAction("UserInfo");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DeleteUser(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.AppUsers.Find(Id.ToString());
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostUser(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var objUser = _db.AppUsers.Find(Id.ToString());
                if (Id == null)
                {
                    return NotFound();
                }
                if (objUser != null)
                {
                    _db.AppUsers.Remove(objUser);
                    _db.SaveChanges();
                    TempData["success"] = "User deleted successfully.";
                    return RedirectToAction("UserInfo");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //Get
        public IActionResult CreateMultiUser()
        {
            checkUser();
            if (loginflag == true)
            {
                return View();
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //POST
        [HttpPost]
        public IActionResult CreateMultiUser(IFormFile postedFile)
        {
            checkUser();
            if (loginflag == true)
            {
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine("Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    //Read the connection string for the Excel file.
                    string conString = _config.GetConnectionString("ExcelConString");
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();

                                //Read Data from First Sheet.
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }
                    //Insert the Data read from the Excel file to Database Table.
                    conString = _config.GetConnectionString("DefaultConnection");

                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'AppUsers'", con))
                        {
                            DataTable columnNames = new DataTable();
                            columnNames.Load(command.ExecuteReader());

                            int i = 0;
                            foreach (DataColumn column in dt.Columns)
                            {
                                string columnNameInDB = columnNames.Rows[i++]["COLUMN_NAME"].ToString();
                                Console.WriteLine("{0}\t{1}\t{2}", column.ColumnName, columnNameInDB, columnNameInDB == column.ColumnName);
                            }
                        }
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            DateTime today = DateTime.Now;
                            string date = today.ToString("yyyy-MM-dd H:mm:ss");
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.AppUsers";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("Id", Guid.NewGuid().ToString());
                            sqlBulkCopy.ColumnMappings.Add("CustomerCode", "CustomerCode");
                            sqlBulkCopy.ColumnMappings.Add("DepartmentCode", "DepartmentCode");
                            sqlBulkCopy.ColumnMappings.Add("DepartmentPermission", "DepartmentPermission");
                            sqlBulkCopy.ColumnMappings.Add("UserCode", "UserCode");
                            sqlBulkCopy.ColumnMappings.Add("UserName", "UserName");
                            sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                            sqlBulkCopy.ColumnMappings.Add("CreatedDate", date);
                            sqlBulkCopy.ColumnMappings.Add("UpdatedDate", date);
                            sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                            sqlBulkCopy.ColumnMappings.Add("EmailConfirmed", "EmailConfirmed");
                            sqlBulkCopy.ColumnMappings.Add("NormalizedUserName", "NormalizedUserName");
                            sqlBulkCopy.ColumnMappings.Add("NormalizedEmail", "NormalizedEmail");
                            sqlBulkCopy.ColumnMappings.Add("PasswordHash", "PasswordHash");
                            sqlBulkCopy.ColumnMappings.Add("SecurityStamp", Guid.NewGuid().ToString());
                            sqlBulkCopy.ColumnMappings.Add("ConcurrencyStamp", Guid.NewGuid().ToString());
                            sqlBulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
                            sqlBulkCopy.ColumnMappings.Add("PhoneNumberConfirmed", "PhoneNumberConfirmed");
                            sqlBulkCopy.ColumnMappings.Add("TwoFactorEnabled", "TwoFactorEnabled");
                            sqlBulkCopy.ColumnMappings.Add("LockoutEnd", "LockoutEnd");
                            sqlBulkCopy.ColumnMappings.Add("LockoutEnabled", "LockoutEnabled");
                            sqlBulkCopy.ColumnMappings.Add("AccessFailedCount", "AccessFailedCount");
                            sqlBulkCopy.ColumnMappings.Add("CreatedBy", "RMB Super Admin");
                            sqlBulkCopy.ColumnMappings.Add("UpdatedBy", "RMB Super Admin");
                            sqlBulkCopy.ColumnMappings.Add("UserRole", "UserRole");
                            sqlBulkCopy.ColumnMappings.Add("UserLevel", "UserLevel");
                            sqlBulkCopy.ColumnMappings.Add("RequestService", "RequestService");
                            sqlBulkCopy.ColumnMappings.Add("RequestDestruct", "RequestDestruct");
                            sqlBulkCopy.ColumnMappings.Add("AppDestructService", "AppDestructService");
                            sqlBulkCopy.ColumnMappings.Add("ViewFContent", "ViewFContent");
                            sqlBulkCopy.ColumnMappings.Add("AmendFContent", "AmendFContent");
                            sqlBulkCopy.ColumnMappings.Add("UploadRIC", "UploadRIC");
                            sqlBulkCopy.ColumnMappings.Add("CanExport", "CanExport");
                            sqlBulkCopy.ColumnMappings.Add("LastLogin", date);
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                }
                return View();
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public JsonResult BillRateInfoByCust(string custcode)
        {
            if (custcode == "0")
            {
                List<BillRateMaster> objBillList = _db.BillRateMaster.ToList();
                //IEnumerable<BillRateMaster> objBillList = _db.BillRateMaster.ToList();
                return Json(objBillList);
            }
            else
            {
                List<BillRateMaster> objBillList = _db.BillRateMaster.Where(y => y.CustomerCode == custcode).ToList();
                return Json(objBillList);
            }
        }
        public IActionResult BillRateInfo(int page, string keywords)
        {
            checkUser();
            if (loginflag == true)
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    page = 1;
                    ViewData["keywords"] = keywords;
                    int recsCount = _db.BillRateMaster.Where(x => x.CustomerCode.Contains(keywords)).Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;

                    var objBillList = (from x in _db.BillRateMaster
                                       where x.CustomerCode.Contains(keywords)
                                       orderby x.BillStartDate descending
                                       select x).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    GroupedCustomerBillModel model = new GroupedCustomerBillModel();
                    //IEnumerable<BillRateMaster> objBillList = _db.BillRateMaster.ToList();
                    model.BillRateList = (List<BillRateMaster>)objBillList;
                    model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                    return View(model);
                }
                else
                {
                    int recsCount = _db.BillRateMaster.Count();
                    const int pageSize = 10;
                    if (page < 1)
                        page = 1;

                    var pager = new Pager(recsCount, page, pageSize);
                    int recSkip = (page - 1) * pageSize;

                    var objBillList = (from x in _db.BillRateMaster
                                       orderby x.BillStartDate descending
                                       select x).Skip(recSkip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    GroupedCustomerBillModel model = new GroupedCustomerBillModel();
                    //IEnumerable<BillRateMaster> objBillList = _db.BillRateMaster.ToList();
                    model.BillRateList = (List<BillRateMaster>)objBillList;
                    model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                    return View(model);
                }
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        [HttpPost]
        public JsonResult GetStdMaterial(string crttype)
        {
            CrtnType? StdMaterials = _db.CartonType.Where(x => x.CtnType == crttype).FirstOrDefault();
            //_db.CartonType.Where(y => y.CtnType == crttype));
            return Json(StdMaterials);
        }
        //GET
        public IActionResult CreateCustBillRate()
        {
            checkUser();
            if (loginflag == true)
            {
                GroupedCustomerBillModel model = new GroupedCustomerBillModel();
                model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                model.CartonTypeList = new SelectList(_db.CartonType, "CtnType", "CtnType");
                model.CrtnType = _db.CartonType.Where(x => x.CtnType == "B").First();
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustBillRate(GroupedCustomerBillModel model)
        {
            checkUser();
            if (loginflag == true)
            {
                ModelState.Remove("BillRateList");
                ModelState.Remove("CustomerList");
                ModelState.Remove("CartonTypeList");
                ModelState.Remove("Customer");
                ModelState.Remove("CrtnType.CtnType");
                ModelState.Remove("CrtnType.Description");
                ModelState.Remove("CrtnType.Dimension");
                ModelState.Remove("CrtnType.StdRIC");
                ModelState.Remove("CrtnType.StdBarcode");
                ModelState.Remove("CrtnType.StdPlasticBag");
                ModelState.Remove("BillRate.CustomerCode");
                ModelState.Remove("BillRate.SmCrtnType");
                if (ModelState.IsValid)
                {
                    model.BillRate.CustomerCode = Request.Form["CustomerCode"];
                    model.BillRate.SmCrtnType = Request.Form["CrtnType"];
                    model.BillRate.StdBarcode = model.CrtnType.StdBarcode;
                    model.BillRate.StdPlasticBag = model.CrtnType.StdPlasticBag;
                    model.BillRate.StdCableTies = model.CrtnType.StdCableTies;
                    model.BillRate.StdRIC = model.CrtnType.StdRIC;
                    model.BillRate.StdZipBag = model.CrtnType.StdZipBag;
                    _db.BillRateMaster.Add(model.BillRate);
                    _db.SaveChanges();
                    TempData["success"] = "Customer Bill Rate created successfully.";
                    return RedirectToAction("BillRateInfo");
                }
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DetailCustBillRate(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.BillRateMaster.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult EditCustBillRate(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.BillRateMaster.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                GroupedCustomerBillModel model = new GroupedCustomerBillModel();
                //model.CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
                model.CartonTypeList = new SelectList(_db.CartonType, "CtnType", "CtnType");
                model.CrtnType = _db.CartonType.Where(x => x.CtnType == fromDb.SmCrtnType).First();
                model.BillRate = fromDb;
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustBillRate(GroupedCustomerBillModel model) //BillRateMaster objBillRate)
        {
            checkUser();
            if (loginflag == true)
            {
                ModelState.Remove("BillRateList");
                ModelState.Remove("CustomerList");
                ModelState.Remove("CartonTypeList");
                ModelState.Remove("Customer");
                ModelState.Remove("CrtnType");
                ModelState.Remove("BillRate.StdBarcode");
                ModelState.Remove("BillRate.StdPlasticBag");
                ModelState.Remove("BillRate.StdRIC");
                ModelState.Remove("BillRate.StdCableTies");
                ModelState.Remove("BillRate.StdZipBag");
                if (ModelState.IsValid)
                {
                    //model.BillRate.SmCrtnType = Request.Form["SmCrtnType"];
                    _db.BillRateMaster.Update(model.BillRate);
                    _db.SaveChanges();
                    TempData["success"] = "Customer Bill Rate updated successfully.";
                    return RedirectToAction("BillRateInfo");
                }
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DeleteCustBillRate(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.BillRateMaster.Find(Id);
                //var fromDbFirst = _db.Customers.FirstOrDefault(x => x.Id == Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteCustBillRate")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostCustBillRate(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var obj = _db.BillRateMaster.Find(Id);
                if (Id == null)
                {
                    return NotFound();
                }
                _db.BillRateMaster.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Customer Bill Rate deleted successfully.";
                return RedirectToAction("BillRateInfo");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult PrintCustBillRate(Guid? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                GroupedCustomerBillModel model = new GroupedCustomerBillModel();
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.BillRateMaster.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                var customerDb = _db.Customers.Where(x => x.CustomerCode == fromDb.CustomerCode).FirstOrDefault();
                if (customerDb == null)
                {
                    return NotFound();
                }
                model.Customer = customerDb;
                model.BillRate = fromDb;
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
            //StiReport report = LoadReport();
            //return StiNetCoreReportResponse.PrintAsPdf(report);            
        }

        public IActionResult CrtnsInfo(int page)
        {
            checkUser();
            if (loginflag == true)
            {
                int recsCount = _db.CartonType.Count();
                const int pageSize = 10;
                if (page < 1)
                    page = 1;

                var pager = new Pager(recsCount, page, pageSize);
                int recSkip = (page - 1) * pageSize;

                var objCrtnList = (from x in _db.CartonType
                                   orderby x.CtnType descending
                                   select x).Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                //IEnumerable<CrtnType> objCrtnList = _db.CartonType.OrderBy(x => x.CtnType).ToList();
                return View(objCrtnList);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //GET
        public IActionResult CreateCrtn()
        {
            checkUser();
            if (loginflag == true)
            {
                return View();
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCrtn(CrtnType objCrtnType)
        {
            checkUser();
            if (loginflag == true)
            {
                var existing = _db.CartonType.Where(x => x.CtnType == objCrtnType.CtnType).FirstOrDefault();
                if (existing == null)
                {
                    ModelState.Remove("StdBarcode");
                    ModelState.Remove("StdPlasticBag");
                    ModelState.Remove("StdRIC");
                    ModelState.Remove("StdZipBag");
                    ModelState.Remove("StdCableTies");
                    if (ModelState.IsValid)
                    {
                        _db.CartonType.Add(objCrtnType);
                        _db.SaveChanges();
                        TempData["success"] = "Carton Type created successfully.";
                        return RedirectToAction("CrtnsInfo");
                    }
                }
                else
                {
                    TempData["error"] = "Carton Type already existed.";
                }
                return View(objCrtnType);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DetailCrtn(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.CartonType.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult EditCrtn(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.CartonType.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCrtn(CrtnType objCrtnType, int Id)
        {
            checkUser();
            if (loginflag == true)
            {
                ModelState.Remove("StdBarcode");
                ModelState.Remove("StdPlasticBag");
                ModelState.Remove("StdRIC");
                ModelState.Remove("StdZipBag");
                ModelState.Remove("StdCableTies");
                CrtnType? fromDb = _db.CartonType.Find(Id);
                //if (ModelState.IsValid)
                //{
                if (fromDb != null)
                {
                    if (ModelState.IsValid)
                    {
                        fromDb.StdBarcode = objCrtnType.StdBarcode;
                        fromDb.StdPlasticBag = objCrtnType.StdPlasticBag;
                        fromDb.StdRIC = objCrtnType.StdRIC;
                        fromDb.StdCableTies = objCrtnType.StdCableTies;
                        fromDb.StdZipBag = objCrtnType.StdZipBag;
                        fromDb.Reorder = objCrtnType.Reorder;
                        //fromDb.Qty = objCrtnType.Qty;
                        fromDb.CloseBal = objCrtnType.CloseBal;
                        fromDb.Dimension = objCrtnType.Dimension;
                        fromDb.AsatDate = objCrtnType.AsatDate;
                        _db.CartonType.Update(fromDb);
                        _db.SaveChanges();
                        TempData["success"] = "Carton Type updated successfully.";
                        return RedirectToAction("CrtnsInfo");
                    }
                }
                return View(objCrtnType);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //GET
        public IActionResult DeleteCrtn(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                var fromDb = _db.CartonType.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                return View(fromDb);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteCrtn")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostCrtn(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var obj = _db.CartonType.Find(Id);
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                _db.CartonType.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Carton Type deleted successfully.";
                return RedirectToAction("CrtnsInfo");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
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

        public async Task<IActionResult> ApprovalSetAsync(int page, string CustomerCode)
        {
            checkUser();
            if (loginflag == true)
            {                
                var loginuser = await _userManager.GetUserAsync(User);
                ApvUserViewModel model = new ApvUserViewModel();
                List<ApprovalUsers> List = new List<ApprovalUsers>();

                if (loginuser.UserRole == "RMB Super Admin")
                {
                    var objCustomer = _db.Customers.Where(x => x.CustomerCode == ((CustomerCode == null || CustomerCode == "") ? x.CustomerCode : CustomerCode)).FirstOrDefault();
                    var userList = (from x in _db.ApprovalSetup
                                    where x.CustomerCode == ((CustomerCode == null || CustomerCode == "") ? x.CustomerCode : CustomerCode)
                                    select x).ToList();
                    model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                    model.Customer = objCustomer;
                    for (int i = 0; i < userList.Count; i++)
                    {
                        ApprovalUsers aUser = new ApprovalUsers();
                        aUser.Id = userList[i].Id;
                        aUser.CustomerCode = userList[i].CustomerCode;
                        aUser.UserName = (from a in _db.AppUsers
                                          where a.UserCode == userList[i].UserId
                                          select a.Name).FirstOrDefault();
                        aUser.ApproverName = (from a in _db.AppUsers
                                              where a.UserCode == userList[i].ApproverId
                                              select a.Name).FirstOrDefault();
                        aUser.SubApproverName = (from a in _db.AppUsers
                                                 where a.UserCode == userList[i].SubApproverId
                                                 select a.Name).FirstOrDefault();
                        aUser.Email = userList[i].Email;
                        List.Add(aUser);
                    }
                }
                else
                {
                    var objCustomer = _db.Customers.Where(x => x.CustomerCode == loginuser.CustomerCode).FirstOrDefault();
                    var userList = (from x in _db.ApprovalSetup
                                    where x.CustomerCode == loginuser.CustomerCode
                                    select x).ToList();
                    model.Customer = objCustomer;
                    for (int i = 0; i < userList.Count; i++)
                    {
                        ApprovalUsers aUser = new ApprovalUsers();
                        aUser.Id = userList[i].Id;
                        aUser.CustomerCode = userList[i].CustomerCode;
                        aUser.UserName = (from a in _db.AppUsers
                                          where a.UserCode == userList[i].UserId
                                          select a.Name).FirstOrDefault();
                        aUser.ApproverName = (from a in _db.AppUsers
                                              where a.UserCode == userList[i].ApproverId
                                              select a.Name).FirstOrDefault();
                        aUser.SubApproverName = (from a in _db.AppUsers
                                                 where a.UserCode == userList[i].SubApproverId
                                                 select a.Name).FirstOrDefault();
                        aUser.Email = userList[i].Email;
                        List.Add(aUser);
                    }
                }                
                model.ListApprovalUser = List;
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        public JsonResult ApprovalSetCust(string custcode)
        {
            ApvUserViewModel model = new ApvUserViewModel();
            List<ApprovalUsers> List = new List<ApprovalUsers>();

            var objCustomer = _db.Customers.Where(x => x.CustomerCode == ((custcode == null || custcode == "") ? x.CustomerCode : custcode)).FirstOrDefault();
            var userList = (from x in _db.ApprovalSetup
                            where x.CustomerCode == ((custcode == null || custcode == "") ? x.CustomerCode : custcode)
                            select x).ToList();
            model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
            model.Customer = objCustomer;
            for (int i = 0; i < userList.Count; i++)
            {
                ApprovalUsers aUser = new ApprovalUsers();
                aUser.Id = userList[i].Id;
                aUser.CustomerCode = userList[i].CustomerCode;
                aUser.UserName = (from a in _db.AppUsers
                                  where a.UserCode == userList[i].UserId
                                  select a.Name).FirstOrDefault();
                aUser.ApproverName = (from a in _db.AppUsers
                                      where a.UserCode == userList[i].ApproverId
                                      select a.Name).FirstOrDefault();
                aUser.SubApproverName = (from a in _db.AppUsers
                                         where a.UserCode == userList[i].SubApproverId
                                         select a.Name).FirstOrDefault();
                aUser.Email = userList[i].Email;
                List.Add(aUser);
            }
            //model.ListApprovalUser = List;
            return Json(List);
        }

        //GET
        public async Task<IActionResult> AddApprovalAsync()
        {
            checkUser();
            if (loginflag == true)
            {

                ApvUserViewModel model = new ApvUserViewModel();
                model.CustomerList = new SelectList(_db.Customers.OrderBy(x => x.CustomerCode), "CustomerCode", "CustomerCode");
                var loginuser = await _userManager.GetUserAsync(User);
                if (loginuser.UserRole != "RMB Super Admin")
                    model.UserList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == loginuser.CustomerCode), "UserCode", "Name");
                //else
                //    model.UserList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == model.ApvUser.CustomerCode), "UserCode", "Name");
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddApproval(ApvUserViewModel model)
        {
            checkUser();
            if (loginflag == true)
            {
                if (model.ApvUser.UserId == null)
                {
                    model.ApvUser.UserId = Request.Form["UserCode"];
                }
                if (model.ApvUser.ApproverId == null)
                {
                    model.ApvUser.ApproverId = Request.Form["ApproverCode"];
                }
                if (model.ApvUser.SubApproverId == null)
                {
                    model.ApvUser.SubApproverId = Request.Form["SubApproverCode"];
                }
                _db.ApprovalSetup.Add(model.ApvUser);
                _db.SaveChanges();
                TempData["success"] = "Approval Setup created successfully.";
                return RedirectToAction("ApprovalSet");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        [HttpPost]
        public JsonResult GetUsers(string custcode)
        {
            SelectList UserList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == custcode).ToList(), "UserCode", "Name");
            return Json(new { UserList });
        }
        [HttpPost]
        public JsonResult GetApprover(string usercode)
        {
            var user = _db.AppUsers.Where(x => x.UserCode == usercode).FirstOrDefault();

            if (user?.UserRole == "User")
            {
                SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" || y.UserRole == "Approver")).ToList(), "UserCode", "Name");
                return Json(new { ApproverList, user.Email });
            }
            else if (user?.UserRole == "Approver")
            {
                SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && y.UserRole == "Admin").ToList(), "UserCode", "Name");
                return Json(new { ApproverList, user.Email });
            }
            else if (user?.UserRole == "Admin")
            {
                SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" && y.UserCode != user.UserCode)).ToList(), "UserCode", "Name");
                return Json(new { ApproverList, user.Email });
            }
            else
            {
                SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode));
                return Json(new { ApproverList, user?.Email });
            }
        }

        [HttpPost]
        public JsonResult GetSubApprover(string usercode, string approvercode)
        {
            var user = _db.AppUsers.Where(x => x.UserCode == usercode).FirstOrDefault();

            if (user?.UserRole == "User")
            {
                SelectList SubApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" || y.UserRole == "Approver") && y.UserCode != approvercode).ToList(), "UserCode", "Name");
                return Json(SubApproverList);
            }
            else if (user?.UserRole == "Approver")
            {
                SelectList SubApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && y.UserRole == "Admin" && y.UserCode != approvercode).ToList(), "UserCode", "Name");
                return Json(SubApproverList);
            }
            else if (user?.UserRole == "Admin")
            {
                SelectList SubApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" && y.UserCode != user.UserCode) && y.UserCode != approvercode).ToList(), "UserCode", "Name");
                return Json(SubApproverList);
            }
            else
            {
                SelectList SubApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode));
                return Json(SubApproverList);
            }
        }

        //GET
        public IActionResult EditApproval(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null)
                {
                    return NotFound();
                }
                var fromDb = _db.ApprovalSetup.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                ApvUserViewModel model = new ApvUserViewModel();
                model.ApvUser = fromDb;
                var user = _db.AppUsers.Where(x => x.UserCode == fromDb.UserId).FirstOrDefault();
                if (user != null)
                {
                    model.ApvUser.UserId = user.Name;
                    if (user.UserRole == "User")
                    {
                        SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" || y.UserRole == "Approver")).ToList(), "UserCode", "Name");
                        model.ApproverList = ApproverList;
                    }
                    else if (user.UserRole == "Approver")
                    {
                        SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && y.UserRole == "Admin").ToList(), "UserCode", "Name");
                        return Json(ApproverList);
                    }
                    else if (user.UserRole == "Admin")
                    {
                        SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "Admin" && y.UserCode != user.UserCode)).ToList(), "UserCode", "Name");
                        model.ApproverList = ApproverList;
                    }
                    else if (user.UserRole == "RMB Super Admin")
                    {
                        SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "RMB Super Admin" && y.UserCode != user.UserCode)).ToList(), "UserCode", "Name");
                        model.ApproverList = ApproverList;
                    }
                    else
                    {
                        SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode));
                        model.ApproverList = ApproverList;
                    }
                }
                else
                {
                    SelectList ApproverList = new SelectList(_db.AppUsers.Where(y => y.CustomerCode == user.CustomerCode && (y.UserRole == "RMB Super Admin" && y.UserCode != user.UserCode)).ToList(), "UserCode", "Name");
                    model.ApproverList = ApproverList;
                }
                return View(model);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditApproval(ApvUserViewModel model, int Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (model.ApvUser.ApproverId == null)
                {
                    model.ApvUser.ApproverId = Request.Form["ApproverCode"];
                }
                if (model.ApvUser.SubApproverId == null)
                {
                    model.ApvUser.SubApproverId = Request.Form["SubApproverCode"];
                }
                if (model.ApvUser != null)
                {
                    var apvUser = _db.ApprovalSetup.Find(Id);
                    if (apvUser != null)
                    {
                        apvUser.ApproverId = model.ApvUser.ApproverId;
                        apvUser.SubApproverId = model.ApvUser.SubApproverId;
                        _db.ApprovalSetup.Update(apvUser);
                        _db.SaveChanges();
                        TempData["success"] = "Approval Setup updated successfully.";
                    }
                }

                return RedirectToAction("ApprovalSet");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

        //GET
        public IActionResult DeleteApproval(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                var fromDb = _db.ApprovalSetup.Find(Id);
                if (fromDb == null)
                {
                    return NotFound();
                }
                ApprovalUsers aUser = new ApprovalUsers();
                aUser.Id = fromDb.Id;
                aUser.CustomerCode = fromDb.CustomerCode;
                aUser.UserName = (from a in _db.AppUsers
                                  where a.UserCode == fromDb.UserId
                                  select a.Name).FirstOrDefault();
                aUser.ApproverName = (from a in _db.AppUsers
                                      where a.UserCode == fromDb.ApproverId
                                      select a.Name).FirstOrDefault();
                aUser.SubApproverName = (from a in _db.AppUsers
                                         where a.UserCode == fromDb.SubApproverId
                                         select a.Name).FirstOrDefault();
                aUser.Email = fromDb.Email;
                return View(aUser);
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }
        //POST
        [HttpPost, ActionName("DeleteApproval")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostApproval(int? Id)
        {
            checkUser();
            if (loginflag == true)
            {
                var obj = _db.ApprovalSetup.Find(Id);
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }
                _db.ApprovalSetup.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Approval setup deleted successfully.";
                return RedirectToAction("ApprovalSet");
            }
            else
            {
                return Redirect("~/Identity/Account/Login/");
            }
        }

    }
}
