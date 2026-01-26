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
    public partial class requisition_form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Text = Session["nik"].ToString();
            lblNamaBranch.Text = Session["Location"].ToString();

            if (string.IsNullOrEmpty(Convert.ToString(Session["nik"])))
            {
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }

            if (!Page.IsPostBack)
            {
                Branch();
                GetTableRF();

                if (Session["GroupName"].ToString() == "Admin Purchasing")
                {
                    divFilter.Visible = true;
                    divTableAdmin.Visible = true;
                    divDashoardRF.Visible = true;
                    divTableUser.Visible = false;


                    GetDataTotalRF();
                    lbTotalRF.Text = Session["TotalRF"].ToString();

                    GetDataTotalNotYetPOCreated();
                    lbTotalPONotYetCreated.Text = Session["TotalNotYetPOCreated"].ToString();

                    GetDataTotalItemRequest();
                    lbTotalItemReq.Text = Session["TotalItemRequest"].ToString();

                    GetDataTotalRF_Canceled();
                    lbTotalRFCanceled.Text = Session["TotalRF_Canceled"].ToString();
                }
                else
                {
                    divFilter.Visible = false;
                    divTableAdmin.Visible = false;
                    divDashoardRF.Visible = false;
                    divTableUser.Visible = true;


                }
            }
        }

        protected void GetDataTotalRF()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalRFCreated");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalRF", (Int32)dr["TotalRF"]);
            }
            else
            {

            }
        }

        protected void GetDataTotalNotYetPOCreated()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalRF_NotyetPOCreated");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalNotYetPOCreated", (Int32)dr["TotalNotYetPOCreated"]);
            }
            else
            {

            }
        }

        protected void GetDataTotalItemRequest()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalItemRequest");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalItemRequest", (Int32)dr["TotalItemRequest"]);
            }
            else
            {

            }
        }

        protected void GetDataTotalRF_Canceled()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalRF_Canceled");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalRF_Canceled", (Int32)dr["TotalRF_Canceled"]);
            }
            else
            {

            }
        }

        protected void Branch()
        {
            ddlBranch.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "FindBranch");
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select Branch>";
                newItem.Value = "0";
                ddlBranch.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();
                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["Branch"].ToString();
                    newItem.Value = dr["ID"].ToString();
                    ddlBranch.Items.Add(newItem);
                }

                dr.Close();
            }
            catch (Exception err)
            {
                //TODO
            }
            finally
            {
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }
        }

        protected void GetTableRF()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "c");
            sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequisitionForm.DataSource = dtb;
            TableRequisitionForm.DataBind();

            TableRequisitionForm.Columns[1].Visible = false;
            TableRequisitionForm.Columns[13].Visible = false;

            TableRequisitionForm.UseAccessibleHeader = true;
            TableRequisitionForm.HeaderRow.TableSection = TableRowSection.TableHeader;

        

                Con.Close();
        }

        protected void GetTableRFByFilter()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", /*"ViewFilter"*/"ViewFilterforRF");
            sqlcomm.Parameters.AddWithValue("@rf_no", txtRFNo.Value);
            sqlcomm.Parameters.AddWithValue("@date_from", txtDate1.Value);
            sqlcomm.Parameters.AddWithValue("@date_to", txtDate2.Value);
            sqlcomm.Parameters.AddWithValue("@requester", txtRequester.Value);
            sqlcomm.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
            sqlcomm.Parameters.AddWithValue("@nama_branch", ddlBranch.SelectedItem.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequisitionFormFilter.DataSource = dtb;
            TableRequisitionFormFilter.DataBind();

            TableRequisitionFormFilter.Columns[1].Visible = false;
            TableRequisitionFormFilter.Columns[13].Visible = false;

            TableRequisitionFormFilter.UseAccessibleHeader = true;
            TableRequisitionFormFilter.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void TableRequisitionForm_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        //protected void TableRequisitionFormFilter_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        LinkButton btnCreatePO = (LinkButton)e.Row.FindControl("btnCreatePO");

        //        if (btnCreatePO != null)
        //        {
        //            if (Session["GroupName"].ToString().Trim() != "Admin Purchasing")
        //            {

        //                if (e.Row.Cells[10].Text.ToString().Trim() == "Approved (Fully Approved)" && e.Row.Cells[11].Text.ToString().Trim() == "Not Complete" )
        //                {
        //                    btnCreatePO.Visible = true;

        //                }
        //                else
        //                {
        //                    btnCreatePO.Visible = false;

        //                }

        //            }
        //            else
        //            {
        //                if (e.Row.Cells[10].Text.ToString().Trim() == "Approved (Fully Approved)" && e.Row.Cells[11].Text.ToString().Trim() == "Not Complete")
        //                {
        //                    btnCreatePO.Visible = true;

        //                }
        //                else
        //                {
        //                    btnCreatePO.Visible = false;
        //                }   

        //            }
        //        }
        //    }

        //}
        //

        protected void TableRequisitionFormFilter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnCreatePO = (LinkButton)e.Row.FindControl("btnCreatePO");
                if (btnCreatePO != null)
                {
                    string groupName = Session["GroupName"]?.ToString().Trim() ?? "";
                    string statusApproval = e.Row.Cells[10].Text.ToString().Trim();
                    string statusPO = e.Row.Cells[11].Text.ToString().Trim();

                    int POallcreated = 0;
                    object keyVal = TableRequisitionFormFilter.DataKeys[e.Row.RowIndex].Value;
                    if (keyVal != null)
                    {
                        string val = keyVal.ToString().Trim();

                        if (val.Equals("True", StringComparison.OrdinalIgnoreCase))
                            POallcreated = 1;
                        else if (val.Equals("False", StringComparison.OrdinalIgnoreCase))
                            POallcreated = 0;
                        else
                            int.TryParse(val, out POallcreated);
                    }


                    bool isApproved = statusApproval == "Approved (Fully Approved)";
                    bool isStatusValid = statusPO == "Not Complete" || statusPO == "PO Created";
                    bool isPOAllCreated = POallcreated == 0;

                    if (isApproved && isStatusValid && isPOAllCreated)
                    {
                        btnCreatePO.Visible = true;
                    }
                    else
                    {
                        btnCreatePO.Visible = false;
                    }
                }
            }
        }


        protected void TableRequisitionForm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                LinkButton btnCreatePO = (LinkButton)e.Row.FindControl("btnCreatePO");

                btnCreatePO.Visible = false;


                //if (btnCreatePO != null)
                //{
                //    if (Session["GroupName"].ToString().Trim() != "Admin Purchasing")
                //    {

                //        if (e.Row.Cells[10].Text.ToString().Trim() == "Approved (Fully Approved)")
                //        {
                //            btnCreatePO.Visible = false;

                //        }
                //        else
                //        {
                //            btnCreatePO.Visible = false;
                //        }

                //    }
                //}
            }

        }

        protected void TableRequisitionForm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetTableRF();
            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[3].Text));
            }
            //if (row.Cells[15].Text == "&nbsp;" || row.Cells[15].Text == "")
            //{

            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "CannotEdit();", true);
            //}
        }



        protected void btnCreatePO_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string rf_no = btn.CommandArgument;

            Response.Redirect("~/create_purchase_order_standart.aspx?rf_no=" + rf_no);

        }

        protected void GenerateExcel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetTableRFByFilter();
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("create_requisition_form.aspx");
        }

        protected void ddlReqType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnNew_User_Click(object sender, EventArgs e)
        {
            Response.Redirect("create_requisition_form.aspx");
        }

        protected void btnGenerateExcel_Admin_Click(object sender, EventArgs e)
        {

        }

        protected void btnView_Admin_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void ViewDetailTotalRFCreated_Click(object sender, EventArgs e)
        {
            divTableRFDetail.Visible = false;
            divMasterRF.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_TotalRFCreated");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableMasterRF.DataSource = dtb;
            TableMasterRF.DataBind();

            TableMasterRF.Columns[1].Visible = false;

            TableMasterRF.UseAccessibleHeader = true;
            TableMasterRF.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailItemReq_Click(object sender, EventArgs e)
        {
            divTableRFDetail.Visible = true;
            divMasterRF.Visible = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewTotalItemRequest");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequesitionFormDetail.DataSource = dtb;
            TableRequesitionFormDetail.DataBind();

            TableRequesitionFormDetail.Columns[1].Visible = false;

            TableRequesitionFormDetail.UseAccessibleHeader = true;
            TableRequesitionFormDetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailPONotYetCreated_Click(object sender, EventArgs e)
        {
            divTableRFDetail.Visible = true;
            divMasterRF.Visible = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewRF_NotyetPOCreated");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequesitionFormDetail.DataSource = dtb;
            TableRequesitionFormDetail.DataBind();

            TableRequesitionFormDetail.Columns[1].Visible = false;

            TableRequesitionFormDetail.UseAccessibleHeader = true;
            TableRequesitionFormDetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailRFCanceled_Click(object sender, EventArgs e)
        {
            divTableRFDetail.Visible = false;
            divMasterRF.Visible = true;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewTotalRF_Canceled");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableMasterRF.DataSource = dtb;
            TableMasterRF.DataBind();

            TableMasterRF.Columns[1].Visible = false;

            TableMasterRF.UseAccessibleHeader = true;
            TableMasterRF.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void btnCloseModalViewDetailRF_Click(object sender, EventArgs e)
        {
            Response.Redirect("requisition_form.aspx");
        }

        protected void btnView_DetailRF_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

        protected void btnView_MasterRF_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_requisition_form.aspx?rf_no=" + (row.Cells[3].Text));
            }
        }

    }
}