using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class error_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                lblErrorMessage.Text = "Error Message: " + exception.Message;
                //lblErrorDescription.Text = "Error Description: " + exception.StackTrace; // Jika ingin menampilkan lebih banyak detail
                Server.ClearError(); // untuk menghapus error setelah ditampilkan
            }
            
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
        }
    }
}