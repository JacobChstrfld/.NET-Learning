using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {

            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            Console.WriteLine(clientInfo.name);

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || 
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }


            if (!IsValid(clientInfo.email))
            {
                errorMessage = "Please enter a valid email address";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "INSERT INTO clients " + 
                        "(name, email, phone, address) VALUES " + 
                        "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorMessage = ex.Message;
                if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint 'UQ__clients__AB6E616425611A84'. " +
                    "Cannot insert duplicate key in object 'dbo.clients'. The duplicate key value is")){
                    errorMessage = "This email already exists, please enter another email.";
                }
               
                return;
            }

            //save data to database
            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New Client Added Succesfully";
            Response.Redirect("/Clients/Index");
        }

        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                return false;
            }
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|edu|mil|int|arpa)$";

            valid = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            return valid;
        }
    }
}
