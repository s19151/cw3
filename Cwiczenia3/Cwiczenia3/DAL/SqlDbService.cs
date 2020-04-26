using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Cwiczenia3.DAL
{
    public class SqlDbService : IStudentsDbService
    {
        private String _conString = "Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True";

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetEnrollment(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudent()
        {
            throw new NotImplementedException();
        }

        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
