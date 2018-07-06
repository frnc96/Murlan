using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class preview_dotnet_templates_registration_Form_Lobby : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        TableRow row = new TableRow();
        TableCell Id0 = new TableCell();
        TableCell Name1 = new TableCell();
        TableCell Players2 = new TableCell();
        TableCell Link3 = new TableCell();

        HyperLink link = new HyperLink();
        link.NavigateUrl = "Default.aspx";
        link.Text = "Go to Game...";
        Link3.Controls.Add(link);

        Id0.Text = "8080";
        Name1.Text = "Room Name #1";
        Players2.Text = "3";
        
        row.Cells.Add(Id0);
        row.Cells.Add(Name1);
        row.Cells.Add(Players2);
        row.Cells.Add(Link3);
        lobbyTbl.Rows.Add(row);
    }

}