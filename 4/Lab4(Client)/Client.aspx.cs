using Lab4_Client_.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lab4_ASMX
{
    public partial class Client : System.Web.UI.Page
    {
        private ServerRealization simplexClass;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.simplexClass = new ServerRealization();
        }

        protected void Sum_Click(object sender, EventArgs e)
        {
            int x, y;
            if (int.TryParse(first.Text.ToString(), out x) && int.TryParse(second.Text.ToString(), out y))
            {
                result.Text = simplexClass.Add(x, y).ToString();
            }
            else
            {
                result.Text = "Error!";
            }
        }
    }
}