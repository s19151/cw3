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
            return Ok(_dbService.GetStudent());
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