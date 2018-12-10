using System;
using System.Configuration;

namespace RecruitmentQnA.AgentChat
{
    public partial class AgentDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                hdnDirectLineKey.Value = Settings.GetDirectLineSecret();
                hdnUserId.Value = Convert.ToString(Session["userid"]);
            }
        }
    }
}