using Cwiczenia3.DTOs.Requests;
using Cwiczenia3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        private String _dbConString = "Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True";

        public bool CheckIfStudentExists(string index)
        {
            bool exists = false;

            using (var con = new SqlConnection(_dbConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "Select 1 from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", index);

                var dr = com.ExecuteReader();
                if (dr.Read())
                    exists = true;
            }

            return exists;
        }

        public bool CheckLogin(LoginRequest request)
        {
            bool authenticated = false;

            using (var con = new SqlConnection(_dbConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "Select 1 from Student where IndexNumber = @index and Password = @password";
                com.Parameters.AddWithValue("index", request.Index);
                com.Parameters.AddWithValue("password", request.Password);

                var dr = com.ExecuteReader();
                if (dr.Read())
                    authenticated = true;
            }

            return authenticated;
        }

        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(_dbConString))
            using (var com = con.CreateCommand())
            {
                con.Open();
                var tran = con.BeginTransaction("EnrollStudentTrans");

                com.Transaction = tran;
                com.Connection = con;

                com.CommandText = "select IdStudy from studies where name = @name";
                com.Parameters.AddWithValue("name", request.Studies);

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    throw new Exception("Studia nie istnieją");
                }
                int idStudies = (int)dr["IdStudy"];
                dr.Close();


                com.CommandText = "select 1 from student where indexnumber = @index";
                com.Parameters.AddWithValue("index", request.IndexNumber);

                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    throw new Exception("Podano zły numer indeksu");
                }
                dr.Close();

                com.CommandText = "select * from Enrollment where idstudy = @idstudy and semester = 1";
                com.Parameters.AddWithValue("idstudy", idStudies);

                Enrollment en = new Enrollment();
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    en.IdEnrollment = dr["IdEnrollment"].ToString();
                    en.Semester = dr["Semester"].ToString();
                    en.IdStudy = dr["IdStudy"].ToString();
                    en.StartDate = dr["StartDate"].ToString();
                }
                else
                {
                    dr.Close();

                    com.CommandText = "Select Max(IdEnrollment) from enrollment";
                    dr = com.ExecuteReader();

                    en.IdEnrollment = ((int)dr["IdEnrollment"] + 1).ToString();
                    en.Semester = "1";
                    en.IdStudy = idStudies.ToString();
                    en.StartDate = DateTime.Now.Date.ToString();

                    com.CommandText = "insert into enrollment values(@idenroll, @semester, @idtsudy, convert(date, @startdate, 103))";
                    com.Parameters.AddWithValue("idenroll", en.IdEnrollment);
                    com.Parameters.AddWithValue("semester", en.Semester);
                    com.Parameters.AddWithValue("idtsudy", en.IdStudy);
                    com.Parameters.AddWithValue("startdate", en.StartDate);

                    com.ExecuteNonQuery();
                }
                dr.Close();

                com.CommandText = "insert into student values(@ind, @fname, @lname, convert(date, @bdate, 103), @idenroll)";
                com.Parameters.AddWithValue("ind", request.IndexNumber);
                com.Parameters.AddWithValue("fname", request.FirstName);
                com.Parameters.AddWithValue("lname", request.LastName);
                com.Parameters.AddWithValue("bdate", request.BirthDate);
                com.Parameters.AddWithValue("idenroll", en.IdEnrollment);

                com.ExecuteNonQuery();
                tran.Commit();

                return en;
            }
        }

        public Student GetStudent(string index)
        {
            Student st;

            using (var con = new SqlConnection(_dbConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "Select * from Student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", index);

                var dr = com.ExecuteReader();
                if (!dr.Read())
                    throw new Exception("Podany student nie istnieje");

                st = new Student();
                st.IndexNumber = dr["IndexNumber"].ToString();
                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.BirthDate = dr["BirthDate"].ToString();
                st.IdEnrollment = dr["IdEnrollment"].ToString();
            }

            return st;
        }

        public Enrollment PromoteStudents(PromoteStudentsRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19151;Integrated Security=True"))
            using (var com = con.CreateCommand())
            {
                con.Open();
                var tran = con.BeginTransaction("PromoteStudentTrans");

                com.Connection = con;
                com.Transaction = tran;

                com.CommandText = "select 1 from enrollment inner join studies on studies.idstudy = enrollment.idstudy where name = @stud and semester = @sem";
                com.Parameters.AddWithValue("stud", request.Studies);
                com.Parameters.AddWithValue("sem", request.Semester);

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    throw new Exception("Brak podanego wpisu");
                }
                dr.Close();

                com.CommandText = "promotestudents @std, @sm";

                var returnParam = new SqlParameter("returnVal", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.ReturnValue;
                com.Parameters.Add(returnParam);

                com.Parameters.AddWithValue("std", request.Studies);
                com.Parameters.AddWithValue("sm", request.Semester);
                com.ExecuteNonQuery();

                dr.Close();

                int idenroll = Convert.ToInt32(returnParam.Value);

                com.CommandText = "select * from enrollment where idenrollment = @idenroll";
                com.Parameters.AddWithValue("idenroll", idenroll);

                //com.CommandText = "select enrollment.* from enrollment, studies where studies.idstudy = enrollment.idstudy and studies.name = @studies and enrollment.semester = @semester";
                //com.CommandText = "select enrollment.* from enrollment inner join studies on studies.idstudy = enrollment.idstudy where name = @studies and semester = @semester";
                //com.Parameters.AddWithValue("studies", request.Studies);
                //com.Parameters.AddWithValue("semester", request.Semester+1);

                dr = com.ExecuteReader();
                Enrollment en = new Enrollment();

                en.IdEnrollment = dr["IdEnrollment"].ToString();
                en.Semester = dr["Semester"].ToString();
                en.IdStudy = dr["IdStudy"].ToString();
                en.StartDate = dr["StartDate"].ToString();
                tran.Commit();

                return en;
            }
        }

    }
}
