// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using RecordManagementPortalDev.Data;
using System.Net;
using System.Net.Mail;

namespace RecordManagementPortalDev.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;    

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext db,
            IConfiguration config)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _config = config;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        public SelectList CustomerList { get; set; }
        public SelectList DepartmentList { get; set; }
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Name")]
            [Required]
            public string Name { get; set; }

            [Display(Name = "User Code")]
            [Required]
            public string UserCode { get; set; }

            [Display(Name = "Customer Code")]
            [Required]
            public string CustomerCode { get; set; }
            [Display(Name = "Department Code")]
            [Required]
            public string DepartmentCode { get; set; }
            [Display(Name = "Department Permission")]            
            public string? DepartmentPermission { get; set; }

            [Display(Name = "User Role")]
            [Required]
            public string UserRole { get; set; }
            [Display(Name = "User Level")]
            [Required]
            public string UserLevel { get; set; }
            [Display(Name = "Request Service")]            
            public bool RequestService { get; set; }
            [Display(Name = "Request Destruction")]            
            public bool RequestDestruct { get; set; }
            [Display(Name = "Approve Destruction Service")]            
            public bool AppDestructService { get; set; }
            [Display(Name = "View File Content")]            
            public bool ViewFContent { get; set; }
            [Display(Name = "Amend File Content")]            
            public bool AmendFContent { get; set; }
            [Display(Name = "Upload RIC")]            
            public bool UploadRIC { get; set; }
            [Display(Name = "Can Export")]            
            public bool CanExport { get; set; }
            //[Display(Name = "Need Approval")]
            //public string NeedApproval { get; set; }
            //[Display(Name = "Control Id")]
            //[Required]
            //public string ControlId { get; set; }
            public DateTime LastLogin { get; set; } = DateTime.Now;
            public string CreatedBy { get; set; }

            public string UpdatedBy { get; set; }

            public DateTime CreatedDate { get; set; }

            public DateTime UpdatedDate { get; set; } = DateTime.Now;
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var loginuser = await _userManager.GetUserAsync(User);
            if (loginuser.UserRole == "RMB Super Admin")
            {
                CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");                
            }
            else
            {
                DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }       

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //Input.CustomerCode = "M009";            
            ModelState.Remove("Input.CustomerCode");
            ModelState.Remove("Input.DepartmentCode");
            ModelState.Remove("Input.DepartmentPermission");
            ModelState.Remove("Input.CreatedBy");
            ModelState.Remove("Input.UpdatedBy");            
            var loginuser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                if (Input.CustomerCode == null)
                {
                    user.CustomerCode = Request.Form["CustomerCode"];
                }
                else
                {
                    user.CustomerCode = Input.CustomerCode;
                }
                if (Input.DepartmentCode == null)
                {
                    user.DepartmentCode = Request.Form["DepartmentCode"];
                }
                else
                {
                    user.DepartmentCode = Input.DepartmentCode;
                }                 
                user.DepartmentPermission = Request.Form["DepartmentPermission"];
                
                user.UserCode = Input.UserCode;
                user.Name = Input.Name;
                user.UserRole = Input.UserRole;
                user.UserLevel = Input.UserLevel;
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                user.CreatedBy = loginuser.UserName;
                user.UpdatedBy = loginuser.UserName;
                if (Input.RequestService == true || Input.RequestDestruct == true || Input.AppDestructService == true)
                {
                    user.RequestService = Input.RequestService;
                    user.RequestDestruct = Input.RequestDestruct;
                    user.AppDestructService = Input.AppDestructService;
                }
                else
                {
                    TempData["error"] = "At least one checkbox was checked in Services!";
                    //var existing = await _userManager.GetUserAsync(User);
                    if (loginuser.UserRole == "RMB Super Admin")
                    {
                        CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == Input.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                    else
                    {
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                    return Page();
                }
                if (Input.ViewFContent == true || Input.AmendFContent == true || Input.UploadRIC == true || Input.CanExport == true)
                {
                    user.ViewFContent = Input.ViewFContent;
                    user.AmendFContent = Input.AmendFContent;
                    user.UploadRIC = Input.UploadRIC;
                    user.CanExport = Input.CanExport;
                }
                else
                {
                    TempData["error"] = "At least one checkbox was checked in Controls!";
                    if (loginuser.UserRole == "RMB Super Admin")
                    {
                        CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == Input.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                    else
                    {
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                    return Page();
                }
                //user.NeedApproval = Input.NeedApproval;
                user.LastLogin = DateTime.Now;

                await _userStore.SetUserNameAsync(user, Input.UserCode, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        TempData["success"] = "User created successfully.";
                        string Subject = "Mitsui-Soko RMP Login";
                        string Body = "<div style='font-size: 13px'>Dear " + user.Name + ", <br>" 
                        + "Welcome to Mitsui-Soko Record Management! You can now login to your account at <a href='https://www.mitsui-soko.com.sg'>https://www.mitsui-soko.com.sg</a> using the details below:"
                        + "<br>" 
                        +"<br>" 
                        +"User Name: " + user.UserCode+ "<br>" 
                        +"Password: " + Input.Password + "<br>" 
                        +"<br>" 
                        +"<br>" 
                        +"Yours, <br>" 
                        + "Mitsui-Soko Records Management <br></div>"
                        + "<p style='padding: 0; font-size:11px; color:#375a7f;'>Remark: This email is auto generated by Mitsui-Soko Records Management System. " +
                        "This message including any attachments may contain confidential " +
                        "or legally privileged information. Any unauthorized use, retention, reproduction " +
                        "or disclosure is prohibited and may attract civil and criminal penalties. If this e-mail has been sent to you in error, please delete it and notify us immediately." 
                        +"</p>";
                        SendEmail(Input.Email, Subject, Body);
                        //await _signInManager.SignInAsync(user, isPersistent: false);                       
                        return LocalRedirect("/Customer/UserInfo/" + loginuser.Id);
                        //return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    TempData["error"] = error.Description;
                    ViewData["custcode"] = Input.CustomerCode;
                    if (loginuser.UserRole == "RMB Super Admin")
                    {
                        CustomerList = new SelectList(_db.Customers, "CustomerCode", "CustomerCode");
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == Input.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                    else
                    {
                        DepartmentList = new SelectList(_db.Departments.Where(y => y.CustomerCode == loginuser.CustomerCode).ToList(), "DepartmentCode", "DepartmentCode");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
            //return LocalRedirect("/Customer/UserInfo/" + loginuser.Id);
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

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
