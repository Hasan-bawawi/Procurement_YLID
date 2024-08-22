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
    public partial class approval_requisition_form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Value = Session["nik"].ToString();
            lblEmail.Text = Session["email_karyawan"].ToString();

            if (!IsPostBack)
            {
                GetDataTableRFNeedApproveManager();
                GetDataTableApprovalHistory();

                GetDataTableRFNeedApproveDivisionGM();
                GetDataTableApprovalHistoryDivisionGM();

                GetDataTableRFNeedApproveAdminGM();
                GetDataTableApprovalHistoryAdmGM();


                GetDataTableRFNeedApproveDeputyDirector();
                GetDataTableApprovalHistoryDeputyDirector();

                GetDataTableRFNeedApproveDirector();
                GetDataTableApprovalHistoryDirector();

                GetDataTableRFNeedApproveGASectionHead();
                GetDataTableApprovalHistoryGAHead();

                GetDataTableRFNeedApproveITSectionHead();
                GetDataTableApprovalHistoryITHead();

                GetDataTableRFNeedApproveAdminDirector();
                GetDataTableApprovalHistoryAdmDirector();
            }

            if (Session["ActiveTab"] != null)
            {
                string activeTab = Session["ActiveTab"].ToString();

                // Aktifkan tab sesuai dengan nilai yang disimpan di sesi
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#" + activeTab + "\"]').tab('show');", true);

                // Hapus nilai sesi setelah menggunakannya
                Session.Remove("ActiveTab");
            }

            if (Session["Position"].ToString().ToUpper() == "35F3B9DB-254A-461B-800C-4497D12EBB10" || Session["Position"].ToString().ToUpper() == "135898D3-5B3F-4D71-8A4B-29C6262AC96F" || Session["Position"].ToString().ToUpper() == "FFA62916-E17C-4A34-9351-0A8CF78224EA")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#division_manager\"]').tab('show');", true);
                GetDataTableRFNeedApproveManager();
                GetDataTableApprovalHistory();
            }
            else if (Session["Position"].ToString().ToUpper() == "ACB2C17C-B3C6-468B-BD6B-835E56B2344D" || Session["Position"].ToString().ToUpper() == "EE3A6202-001E-447B-9C29-594CF57FA7EC" || Session["Position"].ToString().ToUpper() == "527B62E2-A5B4-43FA-8AA4-9E55FC7341A4")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#division_gm\"]').tab('show');", true);
                GetDataTableRFNeedApproveDivisionGM();
                GetDataTableApprovalHistoryDivisionGM();
                GetDataTableRFNeedApproveAdminGM();
                GetDataTableApprovalHistoryAdmGM();
            }
            else if (Session["Position"].ToString().ToUpper() == "68DA0E68-522A-4AC1-81C7-F5BF26C19E30")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#deputy_director\"]').tab('show');", true);
                GetDataTableRFNeedApproveDeputyDirector();
                GetDataTableApprovalHistoryDeputyDirector();
            }
            else if (Session["Position"].ToString().ToUpper() == "6EAFBFBE-0BA1-4018-91D5-CC6B64F87326")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#director\"]').tab('show');", true);
                GetDataTableRFNeedApproveDirector();
                GetDataTableApprovalHistoryDirector();
            }
            else if (Session["Section"].ToString().ToUpper() == "95ED03F4-2420-4FCB-9D22-443787E5BF40" && (Session["Position"].ToString().ToUpper() == "35F3B9DB-254A-461B-800C-4497D12EBB10" || Session["Position"].ToString().ToUpper() == "135898D3-5B3F-4D71-8A4B-29C6262AC96F"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#ga_section_head\"]').tab('show');", true);
                GetDataTableRFNeedApproveGASectionHead();
                GetDataTableApprovalHistoryGAHead();
            }
            else if (Session["Section"].ToString().ToUpper() == "9AF484E4-9DA8-4CB7-9537-8DEE9B935182" && (Session["Position"].ToString().ToUpper() == "35F3B9DB-254A-461B-800C-4497D12EBB10" || Session["Position"].ToString().ToUpper() == "135898D3-5B3F-4D71-8A4B-29C6262AC96F"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#it_section_head\"]').tab('show');", true);
                GetDataTableRFNeedApproveITSectionHead();
                GetDataTableApprovalHistoryITHead();
            }
            else if (Session["nik"].ToString().ToUpper() == "880713")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetActiveTab", "$('.nav-link[href=\"#director_adm\"]').tab('show');", true);
                GetDataTableRFNeedApproveAdminDirector();
                GetDataTableApprovalHistoryAdmDirector();
            }

        }

        protected void BindDataTableItemRF()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailRF");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableItemPurchase.DataSource = dtb;
            TableItemPurchase.DataBind();

            TableItemPurchase.UseAccessibleHeader = true;
            TableItemPurchase.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnCloseModalView_Click(object sender, EventArgs e)
        {
            Response.Redirect("approval_requisition_form.aspx");
        }

        #region division_manager
        protected void GetDataTableRFNeedApproveManager()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveManager");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRF_NeedApprovalManager.DataSource = dtb;
            TableRF_NeedApprovalManager.DataBind();
            TableRF_NeedApprovalManager.Columns[1].Visible = false;
            TableRF_NeedApprovalManager.Columns[15].Visible = false;
            TableRF_NeedApprovalManager.UseAccessibleHeader = true;
            TableRF_NeedApprovalManager.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void GetDataTableApprovalHistory()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryMgrApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryApprovalManager.DataSource = dtb;
            TableHistoryApprovalManager.DataBind();
            TableHistoryApprovalManager.Columns[1].Visible = false;
            TableHistoryApprovalManager.UseAccessibleHeader = true;
            TableHistoryApprovalManager.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewManager_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveManager();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryManager_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableApprovalHistory();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0); 
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region division_gm
        protected void GetDataTableRFNeedApproveDivisionGM()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveDivGM");
            sqlcomm.Parameters.AddWithValue("@nik_gm_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDivisionGMApproval.DataSource = dtb;
            TableDivisionGMApproval.DataBind();
            TableDivisionGMApproval.Columns[1].Visible = false;
            TableDivisionGMApproval.Columns[16].Visible = false;
            TableDivisionGMApproval.UseAccessibleHeader = true;
            TableDivisionGMApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void GetDataTableApprovalHistoryDivisionGM()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryDivisionGMApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryDivisionGMApproval.DataSource = dtb;
            TableHistoryDivisionGMApproval.DataBind();
            TableHistoryDivisionGMApproval.Columns[1].Visible = false;
            TableHistoryDivisionGMApproval.UseAccessibleHeader = true;
            TableHistoryDivisionGMApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewDivisionGM_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDivisionGM();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryDivisionGM_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDivisionGM();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region deputy_director
        protected void GetDataTableRFNeedApproveDeputyDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveDeputyDirector");
            sqlcomm.Parameters.AddWithValue("@nik_deputy_director", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDeputyDirectorApproval.DataSource = dtb;
            TableDeputyDirectorApproval.DataBind();
            TableDeputyDirectorApproval.Columns[1].Visible = false;
            TableDeputyDirectorApproval.Columns[16].Visible = false;
            TableDeputyDirectorApproval.UseAccessibleHeader = true;
            TableDeputyDirectorApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void GetDataTableApprovalHistoryDeputyDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryDeputyDirectorApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryDeputyDirectorApproval.DataSource = dtb;
            TableHistoryDeputyDirectorApproval.DataBind();
            TableHistoryDeputyDirectorApproval.Columns[1].Visible = false;
            TableHistoryDeputyDirectorApproval.UseAccessibleHeader = true;
            TableHistoryDeputyDirectorApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewDeputyDirector_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDeputyDirector();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryDeputyDirector_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDeputyDirector();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region director
        protected void GetDataTableRFNeedApproveDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveDirector");
            sqlcomm.Parameters.AddWithValue("@nik_director", hblNIK.Value.Trim());
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

        protected void GetDataTableApprovalHistoryDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryDivDirectorApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryDirectorApproval.DataSource = dtb;
            TableHistoryDirectorApproval.DataBind();
            TableHistoryDirectorApproval.Columns[1].Visible = false;
            TableHistoryDirectorApproval.UseAccessibleHeader = true;
            TableHistoryDirectorApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewDirector_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDirector();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryDirector_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveDirector();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region ga_section_head
        protected void GetDataTableRFNeedApproveGASectionHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveGASectionHead");
            sqlcomm.Parameters.AddWithValue("@nik_adm_manager", hblNIK.Value.Trim());
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

        protected void GetDataTableApprovalHistoryGAHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryGAHeadApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryGASectionHeadApproval.DataSource = dtb;
            TableHistoryGASectionHeadApproval.DataBind();
            TableHistoryGASectionHeadApproval.Columns[1].Visible = false;
            TableHistoryGASectionHeadApproval.UseAccessibleHeader = true;
            TableHistoryGASectionHeadApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewGASectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveGASectionHead();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryGASectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveGASectionHead();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region it_section_head
        protected void GetDataTableRFNeedApproveITSectionHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveITSectionHead");
            sqlcomm.Parameters.AddWithValue("@nik_it_manager", hblNIK.Value.Trim());
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

        protected void GetDataTableApprovalHistoryITHead()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryITHeadApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryITSectionHeadApproval.DataSource = dtb;
            TableHistoryITSectionHeadApproval.DataBind();
            TableHistoryITSectionHeadApproval.Columns[1].Visible = false;
            TableHistoryITSectionHeadApproval.UseAccessibleHeader = true;
            TableHistoryITSectionHeadApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewITSectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveITSectionHead();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryITSectionHead_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveITSectionHead();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region gm_adm
        protected void GetDataTableRFNeedApproveAdminGM()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveAdminGM");
            sqlcomm.Parameters.AddWithValue("@nik_adm_gm", hblNIK.Value.Trim());
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

        protected void GetDataTableApprovalHistoryAdmGM()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryAdmGMApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryGMAdminApproval.DataSource = dtb;
            TableHistoryGMAdminApproval.DataBind();
            TableHistoryGMAdminApproval.Columns[1].Visible = false;
            TableHistoryGMAdminApproval.UseAccessibleHeader = true;
            TableHistoryGMAdminApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewGMAdminApproval_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveAdminGM();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryGMAdmin_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveAdminGM();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region director_adm
        protected void GetDataTableRFNeedApproveAdminDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNeedApproveAdminDirector");
            sqlcomm.Parameters.AddWithValue("@nik_adm_director", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDirectorAdminApproval.DataSource = dtb;
            TableDirectorAdminApproval.DataBind();
            TableDirectorAdminApproval.Columns[1].Visible = false;
            TableDirectorAdminApproval.Columns[16].Visible = false;
            TableDirectorAdminApproval.UseAccessibleHeader = true;
            TableDirectorAdminApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void GetDataTableApprovalHistoryAdmDirector()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewHistoryAdmDirectorApproval");
            sqlcomm.Parameters.AddWithValue("@nik_approver", hblNIK.Value.Trim());
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableHistoryDirectorAdminApproval.DataSource = dtb;
            TableHistoryDirectorAdminApproval.DataBind();
            TableHistoryDirectorAdminApproval.Columns[1].Visible = false;
            TableHistoryDirectorAdminApproval.UseAccessibleHeader = true;
            TableHistoryDirectorAdminApproval.HeaderRow.TableSection = TableRowSection.TableHeader;
            Con.Close();

        }

        protected void btnViewDirectorAdminApproval_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveAdminDirector();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("approval_requisition_form_view.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnViewHistoryDirectorAdmin_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataTableRFNeedApproveAdminDirector();
            if (row.Cells[2].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
        #endregion

        #region DownloadForm
        protected void btnDownloadRF_Click(object sender, EventArgs e)
        {
            if (Session["status_approve"].ToString() == "NOT YET")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
            else if (Session["status_approve"].ToString() == "Approved (MANAGER)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseManagerApproved";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseManagerApproved.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
            else if (Session["status_approve"].ToString() == "Reject (MANAGER)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
            else if (Session["status_approve"].ToString() == "Cancel (MANAGER)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
            else if (Session["status_approve"].ToString() == "Cancel (GM)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
            else if (Session["status_approve"].ToString() == "Reject (GM)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.BinaryWrite(mybytes); // create the file
                Response.Flush();
            }
        }
        #endregion

        
    }
}