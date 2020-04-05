using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DAL;
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
        public IActionResult EnrollStudent(Student st) {
            

            return BadRequest("nie wszystkie pola wypełnione");
        }
    }
}