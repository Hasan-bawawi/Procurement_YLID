using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class expired_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _url = Request.QueryString["url"];

            //string _pageName = _url.Substring(57, _url.Length - 57); //test

            //string _pageName = _url.Substring(42, _url.Length - 42); //live

            string _pageName = _url.Substring(24, _url.Length - 24); //dev
            lblUrl.Value = _pageName;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("login.aspx?url=" + lblUrl.Value);
        }
    }
}