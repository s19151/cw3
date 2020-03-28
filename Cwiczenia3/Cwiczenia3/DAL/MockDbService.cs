using Cwiczenia3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DAL {
    public class MockDbService : IDbService {
        private static List<Student> _students;

        static MockDbService() {
            _students = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "Select * from Student";

                con.Open();

                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();

                    _students.Add(st);
                }
            }
        }
        public IEnumerable<Student> GetStudent() {
            return _students;
        }
    }
}
