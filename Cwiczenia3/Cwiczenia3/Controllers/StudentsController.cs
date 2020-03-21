using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        public String GetStudent() {
            return "Kowalski, Malewski, Andrzejewski";
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
        public String GetStudent(String orderBy) {
            return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
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