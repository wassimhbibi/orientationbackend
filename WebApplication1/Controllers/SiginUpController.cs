using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SiginUpController : ApiController
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
        public string Post(account user)
        {
            try
            {
                string query = @"INSERT INTO dbo.account (name, lastname, datebirth, adress, email, password, photo, phone, gender, year1, year2, year3, etablisement1, etablisement2, etablisement3, parcour1, parcour2, parcour3,speciality, status, encours)
                         VALUES (@Name, @LastName, @DateBirth, @Adress, @Email, @Password, @Photo, @Phone, @Gender, @Year1, @Year2, @Year3, @Etablisment1, @Etablisment2, @Etablisment3, @Parcour1, @Parcour2, @Parcour3,@Speciality, @Status, @Encours)";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", user.name);
                    cmd.Parameters.AddWithValue("@LastName", user.lastname);
                    cmd.Parameters.AddWithValue("@DateBirth", user.datebirth);
                    cmd.Parameters.AddWithValue("@Adress", user.adress);
                    cmd.Parameters.AddWithValue("@Email", user.email);
                    cmd.Parameters.AddWithValue("@Password", user.password);
                    cmd.Parameters.AddWithValue("@Photo", user.photo);
                    cmd.Parameters.AddWithValue("@Phone", user.phone);
                    cmd.Parameters.AddWithValue("@Gender", user.gender);
                    cmd.Parameters.AddWithValue("@Year1", user.year1);
                    cmd.Parameters.AddWithValue("@Year2", user.year2);
                    cmd.Parameters.AddWithValue("@Year3", user.year3);
                    cmd.Parameters.AddWithValue("@Etablisment1", user.etablisment1);
                    cmd.Parameters.AddWithValue("@Etablisment2", user.etablisment2);
                    cmd.Parameters.AddWithValue("@Etablisment3", user.etablisment3);
                    cmd.Parameters.AddWithValue("@Parcour1", user.parcour1);
                    cmd.Parameters.AddWithValue("@Parcour2", user.parcour2);
                    cmd.Parameters.AddWithValue("@Parcour3", user.parcour3);
                    cmd.Parameters.AddWithValue("@Speciality", user.speciality);
                    cmd.Parameters.AddWithValue("@Status", user.status);
                    cmd.Parameters.AddWithValue("@Encours", user.encours);

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


        public String put(account q)
        {
            try
            {
                string query = @"
            UPDATE dbo.account 
            SET 
                name = @name,
                lastname = @lastname,
                datebirth = @datebirth,
                adress = @adress,
                email = @email,
                photo = @photo,
                phone = @phone,
                gender = @gender,
                status = @status,
                stage1 = @stage1,
                stage2 = @stage2,
                stage3 = @stage3,
                stage4 = @stage4,
                stage5 = @stage5,
                stage6 = @stage6,
                parcourpro1 = @parcourpro1,
                parcourpro2 = @parcourpro2,
                parcourpro3 = @parcourpro3,
                parcourpro4 = @parcourpro4,
                parcourpro5 = @parcourpro5,
                parcourpro6 = @parcourpro6,
                datestage1 = @datestage1,
                datestage2 = @datestage2,
                datestage3 = @datestage3,
                datestage4 = @datestage4,
                datestage5 = @datestage5,
                datestage6 = @datestage6,
                dateparcourpro1 = @dateparcourpro1,
                dateparcourpro2 = @dateparcourpro2,
                dateparcourpro3 = @dateparcourpro3,
                dateparcourpro4 = @dateparcourpro4,
                dateparcourpro5 = @dateparcourpro5,
                dateparcourpro6 = @dateparcourpro6,
                titlestage1 = @titlestage1,
                titlestage2 = @titlestage2,
                titlestage3 = @titlestage3,
                titlestage4 = @titlestage4,
                titlestage5 = @titlestage5,
                titlestage6 = @titlestage6,
                titleparcourpro1 = @titleparcourpro1,
                titleparcourpro2 = @titleparcourpro2,
                titleparcourpro3 = @titleparcourpro3,
                titleparcourpro4 = @titleparcourpro4,
                titleparcourpro5 = @titleparcourpro5,
                titleparcourpro6 = @titleparcourpro6,
                photocov = @photocov
            WHERE 
                id = @id;
        ";

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", (object)q.name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lastname", (object)q.lastname ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datebirth", (object)q.datebirth ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@adress", (object)q.adress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object)q.email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@photo", (object)q.photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone", (object)q.phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@gender", (object)q.gender ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", (object)q.status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage1", (object)q.stage1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage2", (object)q.stage2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage3", (object)q.stage3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage4", (object)q.stage4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage5", (object)q.stage5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@stage6", (object)q.stage6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro1", (object)q.parcourpro1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro2", (object)q.parcourpro2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro3", (object)q.parcourpro3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro4", (object)q.parcourpro4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro5", (object)q.parcourpro5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@parcourpro6", (object)q.parcourpro6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage1", (object)q.datestage1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage2", (object)q.datestage2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage3", (object)q.datestage3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage4", (object)q.datestage4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage5", (object)q.datestage5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@datestage6", (object)q.datestage6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro1", (object)q.dateparcourpro1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro2", (object)q.dateparcourpro2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro3", (object)q.dateparcourpro3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro4", (object)q.dateparcourpro4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro5", (object)q.dateparcourpro5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateparcourpro6", (object)q.dateparcourpro6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage1", (object)q.titlestage1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage2", (object)q.titlestage2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage3", (object)q.titlestage3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage4", (object)q.titlestage4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage5", (object)q.titlestage5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titlestage6", (object)q.titlestage6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro1", (object)q.titleparcourpro1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro2", (object)q.titleparcourpro2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro3", (object)q.titleparcourpro3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro4", (object)q.titleparcourpro4 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro5", (object)q.titleparcourpro5 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@titleparcourpro6", (object)q.titleparcourpro6 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@photocov", (object)q.photocov ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", q.id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return "yessss update successful !!";
            }
            catch (Exception)
            {
                return "failed to update !!";
            }
        }



        public String Delete(int id)
        {
            try
            {
                string query = @"DELETE from dbo.account WHERE id =" + id + @" ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "delete successful !!";
            }
            catch (Exception)
            {
                return "failed to delete !!";
            }
        }






        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/SiginUp/VerifAccount")]

        public Boolean VerifAccount(account log)
        {

            string query = @"select email from dbo.account where email='" + log.email + "'";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Orientation"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);

            }

            if (table.Rows.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        [System.Web.Http.Route("api/SiginUp/SaveFile")]
        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                var originalFileName = Path.GetFileName(postedFile.FileName);
                var physicalPath = HttpContext.Current.Server.MapPath("~/photo" + originalFileName);

                postedFile.SaveAs(physicalPath);

                return originalFileName;
            }
            catch (Exception)
            {
                return "D:/projet PFA/backend/WebApi/WebApi/partieback/photo/ano.png";
            }
        }

        [System.Web.Http.Route("api/SiginUp/SaveFile2")]
        public string SaveFile2()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                var originalFileName = Path.GetFileName(postedFile.FileName);
                var physicalPath = HttpContext.Current.Server.MapPath("~/photo" + originalFileName);

                postedFile.SaveAs(physicalPath);

                return originalFileName;
            }
            catch (Exception)
            {
                return "D:/projet PFA/backend/WebApi/WebApi/partieback/photo/ano.png";
            }
        }
    }
}

    

