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
    public partial class purchase_order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Text = Session["nik"].ToString();
            lblNamaBranch.Text = Session["Location"].ToString();

            if (string.IsNullOrEmpty(Convert.ToString(Session["nik"])))
            {
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }

            if (Session["GroupName"].ToString() == "Admin Purchasing")
            {
                if (!Page.IsPostBack)
                {
                    GetDataTotalPOIssued();
                    lbTotalPOIssued.Text = Session["TotalPO"].ToString();

                    GetDataTotalPONotYetApproved();
                    lbTotalPONotYetApproved.Text = Session["TotalPONotYetApproved"].ToString();

                    GetDataPOApproachingDeliveryDate();
                    lbTotalPOApproachingDeliveryDate.Text = Session["POApproachingDeliveryDate"].ToString();

                    GetDataPOLateDeliveryDate();
                    lbTotalLatePODeliveryDate.Text = Session["POLateDeliveryDate"].ToString();
                }
            }
            else
            {
                Response.Write("<script>alert('Access Denied!!, Purchasing Team Only!'),window.location.href = 'login.aspx';</script>");
            }
        }

        protected void GetDataTotalPOIssued()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalPOIssued");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalPO", (Int32)dr["TotalPO"]);
            }
            else
            {

            }
        }

        protected void GetDataTotalPONotYetApproved()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "TotalPONotYetApproved");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("TotalPONotYetApproved", (Int32)dr["TotalPONotYetApproved"]);
            }
            else
            {

            }
        }

        protected void GetDataPOApproachingDeliveryDate()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "POApproachingDeliveryDate");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("POApproachingDeliveryDate", (Int32)dr["POApproachingDeliveryDate"]);
            }
            else
            {

            }
        }

        protected void GetDataPOLateDeliveryDate()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "POLateDeliveryDate");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("POLateDeliveryDate", (Int32)dr["POLateDeliveryDate"]);
            }
            else
            {

            }
        }

        protected void GetDataPurchaseOrder()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewFilterPurchaseOrder");
            sqlcomm.Parameters.AddWithValue("@po_no", txtPONo.Value);
            sqlcomm.Parameters.AddWithValue("@date_from", txtDate1.Value);
            sqlcomm.Parameters.AddWithValue("@date_to", txtDate2.Value);
            sqlcomm.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TablePurchaseOrder.DataSource = dtb;
            TablePurchaseOrder.DataBind();

            TablePurchaseOrder.Columns[1].Visible = false;
            TablePurchaseOrder.Columns[4].Visible = false;
            TablePurchaseOrder.Columns[15].Visible = false;
            TablePurchaseOrder.Columns[16].Visible = false;
            TablePurchaseOrder.Columns[17].Visible = false;
            TablePurchaseOrder.Columns[18].Visible = false;
            TablePurchaseOrder.Columns[19].Visible = false;
            TablePurchaseOrder.Columns[20].Visible = false;
            //TablePurchaseOrder.Columns[24].Visible = false;

            TablePurchaseOrder.UseAccessibleHeader = true;
            TablePurchaseOrder.HeaderRow.TableSection = TableRowSection.TableHeader;

            dtb.Dispose();
            Con.Close();
            sqlcomm.Dispose();
            Con.Dispose();

            Con.Close();
        }

        protected void btnNewPOStandart_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlSelectRFNumber').modal();", true);
        }

        protected void btnNewPOManual_Click(object sender, EventArgs e)
        {
            Response.Redirect("create_purchase_order_manual.aspx");
        }

        protected void TablePurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TablePurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TablePurchaseOrder_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataPurchaseOrder();

            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_purchase_order_standart.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_purchase_order_standart.aspx?po_no=" + (row.Cells[3].Text));
            }
        }

        protected void GetRFNumber()
        {
            string _vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection _vSQLCon = new SqlConnection(_vPath);
            SqlCommand _vSQLComm = new SqlCommand();

            _vSQLCon.Open();
            _vSQLComm.Connection = _vSQLCon;
            _vSQLComm.CommandType = CommandType.StoredProcedure;
            _vSQLComm.CommandText = "sp_PROCUREMENT_DB_Purchase";

            _vSQLComm.Parameters.AddWithValue("@StatementType", "FindRFNumber");
            _vSQLComm.Parameters.AddWithValue("@rf_no", txtRFNumber.Value);

            DataTable _dt = new DataTable();
            SqlDataReader _dr = _vSQLComm.ExecuteReader();
            _dt.Load(_dr);

            TableRFNumber.DataSource = _dt;
            TableRFNumber.DataBind();

            _dt.Dispose();
            _vSQLCon.Close();
            _vSQLComm.Dispose();
            _vSQLCon.Dispose();

            _vSQLCon.Close();
        }

        protected void btnSearchRFNumber_Click(object sender, EventArgs e)
        {
            GetRFNumber();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlSelectRFNumber').modal();", true);
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            Response.Redirect("purchase_order.aspx");
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetDataPurchaseOrder();
        }

        protected void ViewDetailTotalPOIssued_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewTotalPOIssued");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TablePODetail.DataSource = dtb;
            TablePODetail.DataBind();

            TablePODetail.Columns[1].Visible = false;
            TablePODetail.Columns[4].Visible = false;
            TablePODetail.Columns[15].Visible = false;
            TablePODetail.Columns[16].Visible = false;
            TablePODetail.Columns[17].Visible = false;
            TablePODetail.Columns[18].Visible = false;
            TablePODetail.Columns[19].Visible = false;
            TablePODetail.Columns[20].Visible = false;

            TablePODetail.UseAccessibleHeader = true;
            TablePODetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailTotalPONotYetApproved_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewTotalPONotYetApproved");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TablePODetail.DataSource = dtb;
            TablePODetail.DataBind();

            TablePODetail.Columns[1].Visible = false;
            TablePODetail.Columns[4].Visible = false;
            TablePODetail.Columns[15].Visible = false;
            TablePODetail.Columns[16].Visible = false;
            TablePODetail.Columns[17].Visible = false;
            TablePODetail.Columns[18].Visible = false;
            TablePODetail.Columns[19].Visible = false;
            TablePODetail.Columns[20].Visible = false;

            TablePODetail.UseAccessibleHeader = true;
            TablePODetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailTotalPOApproachingDeliveryDate_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPOApproachingDeliveryDate");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TablePODetail.DataSource = dtb;
            TablePODetail.DataBind();

            TablePODetail.Columns[1].Visible = false;
            TablePODetail.Columns[4].Visible = false;
            TablePODetail.Columns[15].Visible = false;
            TablePODetail.Columns[16].Visible = false;
            TablePODetail.Columns[17].Visible = false;
            TablePODetail.Columns[18].Visible = false;
            TablePODetail.Columns[19].Visible = false;
            TablePODetail.Columns[20].Visible = false;

            TablePODetail.UseAccessibleHeader = true;
            TablePODetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void ViewDetailTotalLatePODeliveryDate_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPOLateDeliveryDate");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TablePODetail.DataSource = dtb;
            TablePODetail.DataBind();

            TablePODetail.Columns[1].Visible = false;
            TablePODetail.Columns[4].Visible = false;
            TablePODetail.Columns[15].Visible = false;
            TablePODetail.Columns[16].Visible = false;
            TablePODetail.Columns[17].Visible = false;
            TablePODetail.Columns[18].Visible = false;
            TablePODetail.Columns[19].Visible = false;
            TablePODetail.Columns[20].Visible = false;

            TablePODetail.UseAccessibleHeader = true;
            TablePODetail.HeaderRow.TableSection = TableRowSection.TableHeader;

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void btnCloseModalViewDetailPODashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("purchase_order.aspx");
        }

        protected void btnViewDetailPO_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetDataPurchaseOrder();

            if (row.Cells[3].Text == "")
            {
                Response.Redirect("detail_purchase_order_standart.aspx?po_no=" + 0);
            }
            else
            {
                Response.Redirect("detail_purchase_order_standart.aspx?po_no=" + (row.Cells[3].Text));
            }
        }
    }
}