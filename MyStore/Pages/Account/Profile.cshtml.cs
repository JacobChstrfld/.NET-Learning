using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStore.Pages.Account
{
    public class ProfileModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnGetLogout()
        {
            HttpContext.Session.SetString("loggedIn", "false");
            Response.Redirect("/Index");

          
        }
    }
}
