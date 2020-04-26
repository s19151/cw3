using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using Cwiczenia3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cwiczenia3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase {
        private IStudentsDbService _dbService;
        private IConfiguration _configuration;

        public EnrollmentsController(IStudentsDbService db, IConfiguration configuration) {
            _dbService = db;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
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
        [Authorize(Roles = "employee")]
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

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (!_dbService.CheckLogin(request))
                return Unauthorized("Podano złe dane logowania");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "jan123"),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "student")
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}