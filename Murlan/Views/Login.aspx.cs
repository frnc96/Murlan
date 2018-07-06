using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Murlan.Models;

public partial class preview_dotnet_templates_registration_Form_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnRegister.PostBackUrl = "~/Views/Registration.aspx";
        btnLogin.Click += new EventHandler(this.btnLogin_Click);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string email = emailTxt.Text;
        string pass = passTxt.Text;
        if (Business.ServerAuthentication(email, pass))
        {
            Response.Redirect("/Views/Lobby.aspx");
        }

    }

}