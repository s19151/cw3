﻿using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DAL {
    public class MockDbService : IDbService {
        private static IEnumerable<Student> _students;

        static MockDbService() {
            _students = new List<Student>();
            /*{
                new Student{ IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
                new Student{ IdStudent = 2, FirstName = "Anna", LastName = "Malewska"},
                new Student{ IdStudent = 3, FirstName = "Andrzej", LastName = "Andrzejewicz"}
            };*/
        }

        public void EnrollStudent(EnrollStudentRequest request)
        {
            
        }

        public IEnumerable<Student> GetStudent() {
            return _students;
        }
    }
}
