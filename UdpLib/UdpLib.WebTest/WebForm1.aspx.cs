using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UdpLib.WebTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static UdpLib.UdpClient mClient = new UdpClient(null, 0);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            mClient.Send(txtUrls.Text, txtIP.Text, int.Parse(txtPort.Text));
        }
    }
}