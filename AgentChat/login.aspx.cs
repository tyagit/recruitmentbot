using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RecruitmentQnA.AgentChat
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblUsername.Value))
            {
                Session["userid"] = lblUsername.Value;
                //Response.Redirect("AgentDashboard.aspx");
                Response.Redirect("agentchat.html?s=" + Settings.GetDirectLineSecret() + "&userid=" + lblUsername.Value);
            }
            else
            { }
        }
    }
}