using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using Cwiczenia3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase {
        private IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService db) {
            _dbService = db;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request) {
            Enrollment en;
            try
            {
                en = _dbService.EnrollStudent(request);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }

            return Created("", en);
        }

        [HttpPost]
        [Route("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request) {
            Enrollment en;

            try
            {
                en = _dbService.PromoteStudents(request);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

            return Created("", en);
        }
    }
}