using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DAL;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public StudentsController(IDbService dbService) {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id) {
            if (id == 1)
                return Ok("Kowalski");
            else if (id == 2)
                return Ok("Malewski");

            return NotFound("Nie znaleziono studenta");
        }

        [HttpGet]
        public IActionResult GetStudent(String orderBy) {
            var stList = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select * from Student";

                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();

                    stList.Add(st);
                }
            }

            if (stList.Count > 0)
                return Ok(stList);

            return NotFound();

            //return Ok(_dbService.GetStudent());
        }

        [HttpGet("enrollment/{id}")]
        public IActionResult GetEnrollment(String id) {
            var enrList = new List<Enrollment>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select Enrollment.* from Enrollment inner join Student on Enrollment.IdEnrollment = Student.IdEnrollment where IndexNumber = @id;";
                com.Parameters.AddWithValue("id", id);

                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var enr = new Enrollment();
                    enr.IdEnrollment = dr["IdEnrollment"].ToString();
                    enr.Semester = dr["Semester"].ToString();
                    enr.IdStudy = dr["IdStudy"].ToString();
                    enr.StartDate = dr["StartDate"].ToString();

                    enrList.Add(enr);
                }
            }

            if (enrList.Count > 0)
                return Ok(enrList);

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student) {
            student.IndexNumber = $"s{new Random().Next(1, 20_000)}";
            return Ok(student);
        }

        [HttpPut("put/{id:int}")]
        public IActionResult PutStudent(int id) {
            return Ok("Aktualizacja ukończona");
        }

        [HttpDelete("delete/{id:int}")]
        public IActionResult DeleteStudent(int id) {
            return Ok("Usuwanie ukończone");
        }
    }
}