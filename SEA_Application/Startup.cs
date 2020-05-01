using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SEA_Application.Models;
using System;
using System.Web.Optimization;

[assembly: OwinStartupAttribute(typeof(SEA_Application.Startup))]
namespace SEA_Application
{
    public partial class Startup
    {


        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

         //   GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
           // app.UseHangfireDashboard();
             //app.UseHangfireServer();

            BundleTable.EnableOptimizations = true;
            createRolesandUsers();

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
             validateInterval: TimeSpan.FromMinutes(15),
            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                },
                SlidingExpiration = false,
                ExpireTimeSpan = TimeSpan.FromHours(6)
            });
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "SeaAdmin";
                user.Name = "SeaAdmin";
                user.Email = "asad.ishaq@theriskadvisors.com";

                string userPWD = "Admin@1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
              }
           
            if (!roleManager.RoleExists("Principal"))
            {

                // first we create Principal role  
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Principal";
                roleManager.Create(role);

                //Here we create a Principal user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Azeem112";
                user.Name = "Azeem";
                user.Email = "azeemazeem187@gmail.com";

                string userPWD = "Azeem@1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Principal   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Principal");

                }
                
            }
            if (!roleManager.RoleExists("Supervisor"))
            {

                // first we create Supervisor role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Supervisor";
                roleManager.Create(role);

                //Here we create a Supervisor user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Kashif";
                user.Name = "Kashif";
                user.Email = "Kashif@gmail.com";

                string userPWD = "Kashif@1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Supervisor   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Supervisor");

                }

            }
            if (!roleManager.RoleExists("SuperAdmin"))
            {

                // first we create Supervisor role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);

                //Here we create a Supervisor user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "SuperAdmin";
                user.Name = "SuperAdmin";
                user.Email = "asad.ishaq@theriskadvisors.com";

                string userPWD = "SuperAdmin@1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Supervisor   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");

                }

            }
            // creating Creating Manager role    
            if (!roleManager.RoleExists("Teacher"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Teacher";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Student"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);

            }
            if (!roleManager.RoleExists("Accountant"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Accountant";
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("Parent"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Parent";
                roleManager.Create(role);

            }
            
        }
    }
}
