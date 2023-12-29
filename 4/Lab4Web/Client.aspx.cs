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
        private ServerRealization client;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.client = new ServerRealization();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            int x, y;

            if (int.TryParse(first.Text.ToString(), out x) && int.TryParse(second.Text.ToString(), out y))
            {
                result.Text = client.Add(x, y).ToString();
            }
            else
            {
                result.Text = "Error!";
            }
        }

        protected void Concat_Click(object sender, EventArgs e)
        {
            double d;

            if (double.TryParse(second1.Text.ToString(), out d))
            {
                result1.Text = client.Concat(first1.Text.ToString(), d);
            }
            else
            {
                result.Text = "Error!";
            }
        }
    }
}