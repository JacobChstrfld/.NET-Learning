using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace MyStore.Pages.Account
{
    public class RegisterModel : PageModel
    {

        public Account user = new Account();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            Console.WriteLine("Loaded reg");
        }

        public void OnPost()
        {

            user.password = Request.Form["password"];
            user.passwordConfirm = Request.Form["passwordConfirm"];
            user.email = Request.Form["email"];

            if (user.password != user.passwordConfirm)
            {
                errorMessage = "Passwords do not match";
                return;
            }

            if (!IsValid(user.email))
            {
                errorMessage = "Please enter a valid email address";
                return;
            }

            if (user.password.Length == 0 || user.email.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            //Console.WriteLine(user.password + " " + validPassword(user.password));
            if (!validPassword(user.password))
            {
                errorMessage = "Password must be at least 8 characters long and contain one digit or special character.";
                return;
            }


            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "INSERT INTO dbo.[User] (Email, PasswordHash) " +
                        "VALUES(@Email, HASHBYTES('SHA2_512', '" + user.password + "'))";

                    //Console.WriteLine(query);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Email", user.email);
                        command.Parameters.AddWithValue("@PasswordHash", user.password);
                        

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorMessage = ex.Message;
                if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint"))
                {
                    errorMessage = "This email already exists, please enter another email.";
                }

                return;
            }

            
            user.email = ""; user.password = ""; user.passwordConfirm = ""; 
            successMessage = "New Account Created Successfuly";
            
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

        private static bool validPassword(string password)
        {
            var valid = true;
            
            if (password.Length < 8 || (!password.Any(char.IsDigit) && !password.Any(ch => !char.IsLetterOrDigit(ch))))
            {
                valid = false;

            }
            return valid;
        }
    }

    public class Account
    {
        public String id;
        public String password;
        public String passwordConfirm;
        public String email;
        public String createdAt;
    }
}
