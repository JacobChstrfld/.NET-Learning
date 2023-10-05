using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";


        public void OnGet()
        {

            String id = Request.Query["id"];


            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT * from clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                             
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }

            }
            catch(Exception e)
            {
                errorMessage = e.Message;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Query["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.email = Request.Form["email"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 || 
                clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || 
                clientInfo.address.Length == 0)
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
                    String query = "UPDATE clients " + "SET name=@name, email=@email, phone=@phone, address=@address " +
                        "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }
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
