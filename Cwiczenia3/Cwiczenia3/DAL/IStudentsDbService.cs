using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DAL
{
    interface IStudentsDbService {
        public IEnumerable<Student> GetStudent();
        public IActionResult GetEnrollment(String id);
        public IActionResult EnrollStudent(EnrollStudentRequest request);
        public IActionResult PromoteStudent(PromoteStudentRequest request);
    }
}
