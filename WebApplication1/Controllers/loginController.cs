
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class loginController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @"
select *
from dbo.account";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        public bool Post(account log)
        {
            string query = @"SELECT email FROM dbo.account WHERE email = @Email AND password = @Password";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@Email", log.email);
                cmd.Parameters.AddWithValue("@Password", log.password);
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return table.Rows.Count > 0;
        }


        [HttpPost]
        [Route("api/login/forgotpassword")]
        public HttpResponseMessage ForgotPassword([FromBody] foPas l)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string resetToken = GeneratePasswordResetToken();

                string query = "INSERT INTO forgetPass (email, resetToken, ExpiryDate) VALUES (@email, @resetToken, @ExpiryDate)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", l.email);
                command.Parameters.AddWithValue("@resetToken", resetToken);

                DateTime expiryDate = DateTime.UtcNow.AddHours(1).AddMinutes(10);

                command.Parameters.AddWithValue("@ExpiryDate", expiryDate);

                command.ExecuteNonQuery();
                SendPasswordResetEmail(l.email, resetToken);

                return Request.CreateResponse(HttpStatusCode.OK, "Password reset instructions sent to your email.");
            }

        }






        // generete token
        private string GeneratePasswordResetToken()
        {
            // Generate a secure random token (you may need to customize this)
            var tokenLength = 32;
            var randomBytes = new byte[tokenLength];
            using (var rngCryptoServiceProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(randomBytes);
            }

            // Convert the random bytes to a base64-encoded string

            var token = Convert.ToBase64String(randomBytes);
            // Remove spaces from the token
            token = token.Replace(" ", string.Empty);
            return token;
        }


        [HttpPost]
        [Route("api/login/UpdateUserPassword")]
        public HttpResponseMessage ResetPassword([FromBody] ResetPassword resetPasswordModel)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DateTime DateNow = DateTime.UtcNow.AddHours(1);

                // Verify the reset token and check if it's still valid (not expired)
                string query = "SELECT email FROM forgetPass WHERE resetToken = @resetToken AND ExpiryDate > @DateNow";
                SqlCommand selectCommand = new SqlCommand(query, connection);
                selectCommand.Parameters.AddWithValue("@resetToken", resetPasswordModel.FoPasModel.resetToken);
                selectCommand.Parameters.AddWithValue("DateNow", DateNow);

                var email = selectCommand.ExecuteScalar() as string;

                if (email != null)
                {

                    // Token is valid, update the user's password
                    string updateQuery = "UPDATE account SET password = @NewPassword WHERE email = @Email";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NewPassword", resetPasswordModel.UsersModel.password);
                    updateCommand.Parameters.AddWithValue("@Email", email);

                    updateCommand.ExecuteNonQuery();

                    // Remove the used reset token from the database
                    string deleteQuery = "DELETE FROM forgetPass WHERE resetToken = @resetToken";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@resetToken", resetPasswordModel.FoPasModel.resetToken);
                    deleteCommand.ExecuteNonQuery();

                    return Request.CreateResponse(HttpStatusCode.OK, "Password reset successful.");
                }
                else
                {
                    string deleteQuery = "DELETE FROM forgetPass WHERE resetToken = @resetToken";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@resetToken", resetPasswordModel.FoPasModel.resetToken);
                    deleteCommand.ExecuteNonQuery();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid or expired token.");
                }
            }
        }






        private void SendPasswordResetEmail(string email, string token)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "habibiwassim37@gmail.com";
            string smtpPassword = "bgex udvq tadw fwto";
            string senderEmail = email;


            using (SmtpClient smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true; // Enable SSL if required

                using (MailMessage mailMessage = new MailMessage(senderEmail, email))
                {
                    mailMessage.Subject = "Password Reset Request";
                    mailMessage.Body = $"To reset your password, click the following link: http://localhost:4200/auth/cover-reset-password?token={token}";
                    mailMessage.IsBodyHtml = true;

                    smtpClient.Send(mailMessage);
                }
            }
        }

    }
}