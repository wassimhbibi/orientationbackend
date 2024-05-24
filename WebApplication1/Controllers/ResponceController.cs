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
    public class ResponceController : ApiController
    { 
        public HttpResponseMessage Get()
    {
        string query = @"
WITH CTE AS (
    SELECT
        *,
        ROW_NUMBER() OVER (PARTITION BY question ORDER BY id) AS rn -- Adjust the ORDER BY clause as needed
    FROM dbo.questionnaire
)
SELECT *
FROM CTE
WHERE rn = 1;
";
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
            string query = @"INSERT INTO dbo.questionnaire (question, choix1, choix2, choix3, inputChoix, CheckedChoix, emailUserResponse, emailUserRequest, speciality)
                         VALUES (@Question, @Choix1, @Choix2, @Choix3, @InputChoix, @CheckedChoix, @EmailUserResponse, @EmailUserRequest, @Speciality)";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Question", (object)user.question ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Choix1", (object)user.choix1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Choix2", (object)user.choix2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Choix3", (object)user.choix3 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@InputChoix", (object)user.inputChoix ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CheckedChoix", (object)user.CheckedChoix ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailUserResponse", (object)user.emailUserResponse ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailUserRequest", (object)user.emailUserRequest ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Speciality", (object)user.speciality ?? DBNull.Value);


                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    string deleteQuery = "DELETE FROM dbo.questionnaire WHERE question = @question AND emailUserResponse IS NULL";
                    using (var deleteCmd = new SqlCommand(deleteQuery, con))
                    {
                        deleteCmd.Parameters.AddWithValue("@question", user.question);
                        deleteCmd.ExecuteNonQuery();
                    }
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
