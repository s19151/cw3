using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Services
{
    public interface IStudentsDbService
    {
        public Enrollment EnrollStudent(EnrollStudentRequest request);
        public Enrollment PromoteStudents(PromoteStudentsRequest reuqest);
        public Student GetStudent(String index);
        public bool CheckIfStudentExists(String index);
        public bool CheckLogin(LoginRequest request);
    }
}
