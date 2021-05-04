using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using HUD.Data.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using HUD.Data;

namespace HUD.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterOrganizationModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        protected ApplicationDbContext _context;

        public RegisterOrganizationModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Organization Name")]
            public string OrganizationName { get; set; }

            [Required]
            [Display(Name = "Organization URL Prefix")]
            public string UrlPrefix{ get; set; }

            [Required]
            [Display(Name = "Organization API Key")]
            public string ApiKey { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The PIN must be at least 4 characters long")]
            [DataType(DataType.Password)]
            [Display(Name = "Organization PIN")]
            public string Pin { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Organization PIN")]
            [Compare("Pin", ErrorMessage = "The PIN and confirmation PIN do not match.")]
            public string ConfirmPin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (User.Identity.IsAuthenticated)
            {
                //Organization existingOrg = (Organization)_context.Organizations.Where(m => m.UrlPrefix == Input.UrlPrefix);
                //if (existingOrg is not null)
                //{
                //    if (existingOrg.Members.ToList().Contains(User.Identity))
                //    {

                //    }
                //}
                if (ModelState.IsValid)
                {
                    var org = new Organization { Name = Input.OrganizationName, UrlPrefix = Input.UrlPrefix, ApiKey = Input.ApiKey, Pin = Input.Pin };
                    _context.Organizations.Add(org);
                    return RedirectToPage("~/index");
                }
            }
            else
            {
                return RedirectToPage("Login");
            }
            

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
