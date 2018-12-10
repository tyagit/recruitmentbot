using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitmentQnA.Models
{
    [Serializable]
    public class UserProfile
    {
        /* Changes might be required in AssignValueToClass & UserProfileDAL & table & SP*/
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FromCard { get; set; }
        public string InterviewDate { get; set; }
        public string AvailableStartDate { get; set; }
        public string JoinUsSourceInfo { get; set; }
        public string ResignationDate { get; set; }
        public string Account { get; set; }
        public string DataJSon { get; set; }
    }
}