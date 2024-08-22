using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace procurement_system
{
    public partial class approval_purchase_order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Value = Session["nik"].ToString();
            lblEmail.Text = Session["email_karyawan"].ToString();

            if (!IsPostBack)
            {
                GetDataTablePONeedApproveITSectionHead();
                GetDataTablePONeedApproveGASectionHead();
                GetDataTablePONeedApproveAdminGM();
                GetDataTablePONeedApproveDirector();
            }

            if (Session["ActiveTab"] != null)
            {
                string activeTab = Session["ActiveTab"].ToString();

                // Aktifkan tab sesuai dengan nilai yang disimpan di sesi
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#" + activeTab + "\"]').tab('show');", true);

                // Hapus nilai sesi setelah menggunakannya
                Session.Remove("ActiveTab");
            }

            if (Session["Section"].ToString().ToUpper() == "9AF484E4-9DA8-4CB7-9537-8DEE9B935182" && (Session["Position"].ToString().ToUpper() == "35F3B9DB-254A-461B-800C-4497D12EBB10" || Session["Position"].ToString().ToUpper() == "135898D3-5B3F-4D71-8A4B-29C6262AC96F"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#it_section_head\"]').tab('show');", true);
                GetDataTablePONeedApproveITSectionHead();
            }
            else if (Session["Section"].ToString().ToUpper() == "95ED03F4-2420-4FCB-9D22-443787E5BF40" && (Session["Position"].ToString().ToUpper() == "35F3B9DB-254A-461B-800C-4497D12EBB10" || Session["Position"].ToString().ToUpper() == "135898D3-5B3F-4D71-8A4B-29C6262AC96F"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#ga_section_head\"]').tab('show');", true);
                GetDataTablePONeedApproveGASectionHead();
            }
            else if (Session["Section"].ToString().ToUpper() == "3BEAD7B1-A9D4-4557-976F-DA2C6B489910" && (Session["Position"].ToString().ToUpper() == "ACB2C17C-B3C6-468B-BD6B-835E56B2344D" || Session["Position"].ToString().ToUpper() == "527B62E2-A5B4-43FA-8AA4-9E55FC7341A4"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#gm_adm\"]').tab('show');", true);
                GetDataTablePONeedApproveAdminGM();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#director\"]').tab('show');", true);
                GetDataTablePONeedApproveDirector();
            }
        }

        #region it_section_head
        protected void GetDataTablePONeedApproveITSectionHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveITSectionHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by_it", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableITSectionHeadApproval.DataSource = dtb;
            TableITSectionHeadApproval.DataBind();
            TableITSectionHeadApproval.Columns[1].Visible = false;
            TableITSectionHeadApproval.Columns[16].Visible = false;
            TableITSectionHeadApproval.UseAccessibleHeader = true;
            TableITSectionHeadApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewITSectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTablePONeedApproveITSectionHead();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryITSectionHead_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region ga_section_head
        protected void GetDataTablePONeedApproveGASectionHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveGASectionHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableGASectionHeadApproval.DataSource = dtb;
            TableGASectionHeadApproval.DataBind();
            TableGASectionHeadApproval.Columns[1].Visible = false;
            TableGASectionHeadApproval.Columns[16].Visible = false;

            TableGASectionHeadApproval.UseAccessibleHeader = true;
            TableGASectionHeadApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewGASectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTablePONeedApproveGASectionHead();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryGASectionHead_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region gm_adm
        protected void GetDataTablePONeedApproveAdminGM()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveAdminGM");
            sqlcomm.Parameters.AddWithValue("@po_approved_by", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableGMAdminApproval.DataSource = dtb;
            TableGMAdminApproval.DataBind();
            TableGMAdminApproval.Columns[1].Visible = false;
            TableGMAdminApproval.Columns[16].Visible = false;
            TableGMAdminApproval.UseAccessibleHeader = true;
            TableGMAdminApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewGMAdminApproval_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTablePONeedApproveAdminGM();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryGMAdmin_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region director
        protected void GetDataTablePONeedApproveDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveDirector");
            sqlcomm.Parameters.AddWithValue("@authorized_by", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDirectorApproval.DataSource = dtb;
            TableDirectorApproval.DataBind();
            TableDirectorApproval.Columns[1].Visible = false;
            TableDirectorApproval.Columns[16].Visible = false;
            TableDirectorApproval.UseAccessibleHeader = true;
            TableDirectorApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewDirector_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTablePONeedApproveDirector();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_purchase_order_view.aspx?po_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryDirector_Click(object sender, EventArgs e)
        {

        }
        #endregion

        protected void btnCloseModalView_Click(object sender, EventArgs e)
        {

        }

        protected void btnDownloadRF_Click(object sender, EventArgs e)
        {

        }

    }
}