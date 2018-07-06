using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Murlan.Models;

public partial class preview_dotnet_templates_registration_Form_Registration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string name = txtName.Text;
        string surname = txtSurname.Text;
        string pass = txtPass.Text;
        string confPass = txtConfPass.Text;
        if (Business.DbUpdate(email, name, surname, pass, confPass))
        {
            Response.Redirect("/Views/Login.aspx");
        }

    }
}