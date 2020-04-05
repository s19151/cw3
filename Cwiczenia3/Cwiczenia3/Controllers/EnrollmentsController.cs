using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DAL;
using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase {
        private IDbService _dbService;

        public EnrollmentsController(IDbService db) {
            _dbService = db;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request) {
            /*_dbService.EnrollStudent(request);

            return Ok();*/

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = con.CreateCommand()) {
                con.Open();
                var tran = con.BeginTransaction("EnrollStudentTrans");

                com.Transaction = tran;
                com.Connection = con;

                try
                {
                    com.CommandText = "select 1 from student where indexnumber = @index";
                    com.Parameters.AddWithValue("index", request.IndexNumber);

                    var dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("Podano zły numer indeksu");
                    }
                    dr.Close();

                    com.CommandText = "select IdStudy from studies where name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest("Studia nie istnieją");
                    }
                    int idStudies = (int)dr["IdStudy"];
                    dr.Close();

                    com.CommandText = "select * from Enrollment where idstudy = @idstudy and semester = 1";
                    com.Parameters.AddWithValue("idstudy", idStudies);

                    Enrollment en = new Enrollment();
                    dr = com.ExecuteReader();
                    if (dr.Read()) {
                        en.IdEnrollment = dr["IdEnrollment"].ToString();
                        en.Semester = dr["Semester"].ToString();
                        en.IdStudy = dr["IdStudy"].ToString();
                        en.StartDate = dr["StartDate"].ToString();
                    }
                    else {
                        com.CommandText = "Select Max(IdEnrollment) from enrollment";
                        dr = com.ExecuteReader();

                        en.IdEnrollment = ((int)dr["IdEnrollment"] + 1).ToString();
                        en.Semester = "1";
                        en.IdStudy = idStudies.ToString();
                        en.StartDate = DateTime.Now.Date.ToString();

                        com.CommandText = "insert into enrollment values(@idenroll, @semester, @idtsudy, convert(date, @startdate))";
                        com.Parameters.AddWithValue("idenroll", en.IdEnrollment);
                        com.Parameters.AddWithValue("semester", en.Semester);
                        com.Parameters.AddWithValue("idtsudy", en.IdStudy);
                        com.Parameters.AddWithValue("startdate", en.StartDate);

                        com.ExecuteNonQuery();
                    }
                    dr.Close();

                    com.CommandText = "insert into student values(@index, @fname, @lname, @bdate, @idenroll)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    com.Parameters.AddWithValue("fname", request.FirstName);
                    com.Parameters.AddWithValue("lname", request.LastName);
                    com.Parameters.AddWithValue("bdate", request.BirthDate);
                    com.Parameters.AddWithValue("idenroll", en.IdEnrollment);

                    com.ExecuteNonQuery();
                    tran.Commit();

                    return Ok(en);
                }
                catch (SqlException e) {
                    tran.Rollback();
                    return BadRequest();
                }
            }
        }

        public IActionResult PromoteStudents(PromoteStudentRequest request) {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = con.CreateCommand())
            {
                con.Open();
                com.Connection = con;

                com.CommandText = "select idenrollment from enrollment inner join studies on studies.idstudy = enrollment.idstudy where name = @studies and semester = @semester";
                com.Parameters.AddWithValue("studies", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);

                var dr = com.ExecuteReader();
                if (!dr.Read()) {
                    return NotFound("Brak podanego wpisu");
                }
                dr.Close();

                com.CommandText = "execute promotestudents(@studies, @semester)";
                com.Parameters.AddWithValue("studies", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);

                dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return BadRequest("Brak studiów");
                }

                Enrollment en = new Enrollment();
                en.IdEnrollment = dr["IdEnrollment"].ToString();
                en.Semester = dr["Semester"].ToString();
                en.IdStudy = dr["IdStudy"].ToString();
                en.StartDate = dr["StartDate"].ToString();

                return Ok(en);
            }
        }
    }
}