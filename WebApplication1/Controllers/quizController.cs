using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class quizController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @"
select *
from dbo.questionnaire";
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
        public string Post(quizz user)
        {
            try
            {
                string query = @"INSERT INTO dbo.questionnaire ( speciality,question, choix1, choix2, choix3,photo,emailUserRequest )
                         VALUES (@Speciality, @Question, @Choix1, @Choix2, @Choix3,@Photo,@EmailUserRequest )";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Speciality", user.speciality);
                    cmd.Parameters.AddWithValue("@Question", user.question);
                    cmd.Parameters.AddWithValue("@Choix1", user.choix1);
                    cmd.Parameters.AddWithValue("@Choix2", user.choix2);
                    cmd.Parameters.AddWithValue("@Choix3", user.choix3);
                    cmd.Parameters.AddWithValue("@Photo", user.photo);
                    cmd.Parameters.AddWithValue("@EmailUserRequest ", user.emailUserRequest);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "Added successfully!";
                    }
                    else
                    {
                        return "Failed to add!";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
