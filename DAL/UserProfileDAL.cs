using RecruitmentQnA.Models;
using System;
using System.Data.SqlClient;

namespace RecruitmentQnA.DAL
{
    public class UserProfileDAL
    {
        public static int SaveUserProfile(UserProfile user)
        {
            SqlConnection con = new SqlConnection(Common.AnaBotConnection);
            try
            {                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "uspInsUserProfile";
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@FirstName", Value = user.FirstName });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@LastName", Value = user.LastName });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Email", Value = user.Email });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Phone", Value = user.Phone });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@FromCard", Value = user.FromCard });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@InterviewDate", Value = user.InterviewDate });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@AvailableStartDate", Value = user.AvailableStartDate });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@JoinUsSourceInfo", Value = user.JoinUsSourceInfo });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@DataJson", Value = user.DataJSon });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ResignationDate", Value = user.ResignationDate });
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@Account", Value = user.Account });
                con.Open();
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally { if (con.State != System.Data.ConnectionState.Closed) con.Close(); }
        }
    }
}