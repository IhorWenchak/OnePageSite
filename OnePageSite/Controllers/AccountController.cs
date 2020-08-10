using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnePageSite.Models;

namespace OnePageSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
			ViewBag.Message = "";

			if (!ModelState.IsValid)
            {
                return View(model);
            }
			
			ServiceReference1.ICUTechClient iCUTechClient = new ServiceReference1.ICUTechClient();
			iCUTechClient.Open();
			var result = iCUTechClient.Login(model.Email, model.Password, "");
			iCUTechClient.Close();
				
			if(result.IndexOf("ResultCode") > 0)
				{
					result = result.Substring(result.LastIndexOf(":") + 1).Replace("{", "").Replace("}", "").Replace("\"", "");
					ModelState.AddModelError("", result);
				}
			if (result.IndexOf("EntityId") > 0)
				{
					result = result.Replace("{", "").Replace("}", "");
					var resArr = result.Split(',');
					string str = String.Empty; 

					foreach(var res in resArr)
					{
						if((res.IndexOf("FirstName") > 0) || (res.IndexOf("LastName") > 0) || (res.IndexOf("Email") > 0) && (res.IndexOf("EmailConfirm") < 0) || (res.IndexOf("Mobile") > 0) && (res.IndexOf("MobileConfirm") < 0))
						{
							str += res.Replace("\"", "") + "			";							
						}
					}

					ViewBag.Message = str;
					return View("ConfirmEmail");
				}

					return View(model);
		}

 
    }
}