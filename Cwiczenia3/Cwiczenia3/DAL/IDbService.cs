using Cwiczenia3.Models;
using System.Collections.Generic;

namespace Cwiczenia3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudent();
    }
}