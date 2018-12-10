using RecruitmentQnA.Models;
using System;
using System.Data.SqlClient;

namespace RecruitmentQnA.DAL
{
    public class CommonDAL
    {
        public static string getInterviewDate()
        {
            SqlConnection con = new SqlConnection(Common.AnaBotConnection);
            string result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "getInterviewDate";
                cmd.Connection = con;
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = Convert.ToString(reader.GetValue(0));
                }
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally { if (con.State != System.Data.ConnectionState.Closed) con.Close(); }
        }
    }
}