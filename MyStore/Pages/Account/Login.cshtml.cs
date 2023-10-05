using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace MyStore.Pages.Account
{
    public class LoginModel : PageModel
    {

        public Account user = new Account();
        public String errorMessage = "";
        public String successMessage = "";

        public bool loggedIn = false;
        public void OnGet()
        {
            loggedIn = false;
            
        }

        private static readonly Encoding Encoding1252 = Encoding.GetEncoding(1252);

        public static byte[] SHA1HashValue(string s)
        {
            byte[] bytes = Encoding1252.GetBytes(s);

            var sha1 = SHA512.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return hashBytes;
        }

        static bool ByteArraysAreEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
                return false;

            if (ReferenceEquals(array1, array2))
                return true;

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }

        static byte[] ComputeSHA512Hash(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return sha512.ComputeHash(bytes);
            }
        }



        public void OnPost()
        {

            user.password = Request.Form["password"];
            user.email = Request.Form["email"];

            loggedIn = false;

            if (user.password.Length == 0 || user.email.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT [PasswordHash] FROM [mystore].[dbo].[User] WHERE [Email] = '" + user.email + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                
                                byte[] storedHashBytes = (byte[])reader["PasswordHash"];
                                byte[] hashedPasswordToCheckBytes = SHA1HashValue(user.password);

                                if (ByteArraysAreEqual(storedHashBytes, hashedPasswordToCheckBytes))
                                {
                                    
                                    loggedIn = true;    
                                }
                                else
                                {
                                    //Console.WriteLine("Password is incorrect!\n" + Encoding.UTF8.GetString(storedHashBytes) + "\n" +
                                    // Encoding.UTF8.GetString(hashedPasswordToCheckBytes));
                                    errorMessage = "Email and Password Do Not Match any User";
                                    
                                }


                            }
                        }
                    }
                    
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (loggedIn)
            {
                HttpContext.Session.SetString("loggedIn", "true");
                Response.Redirect("/Index");

            }
            


        }

     

   
    }

   
}
