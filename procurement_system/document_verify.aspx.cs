using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace procurement_system
{
    public partial class document_verify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string po = Request.QueryString["po"];

                if (!string.IsNullOrEmpty(po))
                {
                    LoadVerification(po);
                }
            }
        }

        private void LoadVerification(string po)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_ApprovalPurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "Viewverify");
                cmd.Parameters.AddWithValue("@po_no", po);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lbPO.Text = dt.Rows[0]["po_no"].ToString();
                    lbRF.Text = dt.Rows[0]["rf_no"].ToString();
                    lbName.Text = dt.Rows[0]["director_name"].ToString();
                    lbDate.Text = Convert.ToDateTime(dt.Rows[0]["approve_date"]).ToString("dd MMM yyyy HH:mm:ss");
                    lblstatus.Text = "Presdir/Director";

                    iconStatus.InnerText = "✓";
                    iconStatus.Style["color"] = "#28a745";

                    titleStatus.InnerText = "Document Verified";
                    titleStatus.Style["color"] = "#28a745";

                    statusDiv.InnerText = "STATUS : VALID";
                    statusDiv.Style["background"] = "#e9f7ef";
                    statusDiv.Style["color"] = "#28a745";
                }
                else
                {
                    iconStatus.InnerText = "✗";
                    iconStatus.Style["color"] = "#dc3545";

                    titleStatus.InnerText = "Document Not Valid";
                    titleStatus.Style["color"] = "#dc3545";

                    statusDiv.InnerText = "STATUS : INVALID";
                    statusDiv.Style["background"] = "#fdecea";
                    statusDiv.Style["color"] = "#dc3545";

                }
            }
        }
    }
}