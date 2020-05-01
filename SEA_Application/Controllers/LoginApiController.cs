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
using SEA_Application.Models;
using System.Web.Http;

namespace SEA_Application.Controllers
{
    public class LoginApiController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public async Task<String> Login(string UserName, string Password)
        {
            SEA_DatabaseEntities db = new SEA_DatabaseEntities();

            LoginViewModel model = new LoginViewModel();
            model.UserName = UserName;
            model.Password = Password;
            model.RememberMe = false;

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            LoginUser login = new LoginUser();

            switch (result)
            {
                case SignInStatus.Success:

                    var userID = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();

                    var USER = db.AspNetUsers.Where(x => x.Id == userID).Select(x => x).FirstOrDefault();
                    login.Id = userID;
                    login.Name = USER.Name;
                    
                    if (UserManager.IsInRole(userID, "Teacher"))
                    {
                        login.Role = "Teacher";
                    }
                    else if (UserManager.IsInRole(userID, "Student"))
                    {
                        login.Role = "Student";
                    }
                    else if (UserManager.IsInRole(userID, "Admin"))
                    {
                        login.Role = "Admin";
                    }
                    else if (UserManager.IsInRole(userID, "Principal"))
                    {
                        login.Role = "Principal";
                    }
                    else if (UserManager.IsInRole(userID, "Accountant"))
                    {
                        login.Role = "Accountant";
                    }
                    else if (UserManager.IsInRole(userID, "Parent"))
                    {
                        login.Role = "Parent";
                    }

                    if(USER.Status == "False")
                    {
                        login.Message = "Admin Has disabled your account";
                    }
                    else
                    {
                        login.Message = "Login Success";
                    }

                    

                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(login);

                    return jsonString;

                case SignInStatus.Failure:
                    login.Message = "Invalid login attempt.";

                    var javaScriptSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString1 = javaScriptSerializer1.Serialize(login);

                    return jsonString1;

                default:
                    login.Message = "Invalid login attempt.";
                    var javaScriptSerializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString2 = javaScriptSerializer2.Serialize(login);

                    return jsonString2;
            }
        }

        public async Task<string> ChangePassword(string New, string old, string confirm, string Id)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.NewPassword = New;
            model.OldPassword = old;
            model.ConfirmPassword = confirm;

            var result = await UserManager.ChangePasswordAsync(Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(Id);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                
                return "Password has been changed";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(result);

            return jsonString;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public class LoginUser
        {
            public string Id { set; get; }
            public string Name { set; get; }
            public string Role { set; get; }
            public string Message { set; get; }
            
        }
  }
}
