using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
//using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
            string nik = Convert.ToString(Session["nik"]);
            if (string.IsNullOrEmpty(nik))
            {
                string currentUrl = Server.UrlEncode(Request.Url.AbsoluteUri);
                Response.Redirect("login.aspx?url=" + currentUrl);
                return;
            }
            hblNIK.Text = nik;
            lblNamaBranch.Text = Convert.ToString(Session["Location"]);

            if (GetUserAccess())
            {
                Session.Add("AllowPO", true);
                if (!IsPostBack)
                {
                    LoadDashboardData();
                }
            }
            else
            {
                // Use JavaScript to alert and redirect
                string script = "alert('Access Denied! Purchasing Team Only!'); window.location.href='login.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "AccessDenied", script, true);
            }

            #region old code
            //hblNIK.Text = Session["nik"].ToString();
            //lblNamaBranch.Text = Session["Location"].ToString();
            //if (string.IsNullOrEmpty(Convert.ToString(Session["nik"])))
            //{
            //    Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            //}

            ////if (Session["GroupName"].ToString() == "Admin Purchasing" || Session["GroupName"].ToString() == "Super Admin")
            //if (GetUserAccess() == true)
            //{
            //    if (!Page.IsPostBack)
            //    {
            //        GetDataTotalPOIssued();
            //        lbTotalPOIssued.Text = Session["TotalPO"].ToString();

            //        GetDataTotalPONotYetApproved();
            //        lbTotalPONotYetApproved.Text = Session["TotalPONotYetApproved"].ToString();

            //        GetDataPOApproachingDeliveryDate();
            //        lbTotalPOApproachingDeliveryDate.Text = Session["POApproachingDeliveryDate"].ToString();

            //        GetDataPOLateDeliveryDate();
            //        lbTotalLatePODeliveryDate.Text = Session["POLateDeliveryDate"].ToString();
            //    }
            //}
            //else
            //{
            //    Response.Write("<script>alert('Access Denied!!, Purchasing Team Only!'),window.location.href = 'login.aspx';</script>");
            //}
            #endregion
        }
        private void LoadDashboardData()
        {
            GetDataTotalPOIssued();
            lbTotalPOIssued.Text = Convert.ToString(Session["TotalPO"]);

            GetDataTotalPONotYetApproved();
            lbTotalPONotYetApproved.Text = Convert.ToString(Session["TotalPONotYetApproved"]);

            GetDataPOApproachingDeliveryDate();
            lbTotalPOApproachingDeliveryDate.Text = Convert.ToString(Session["POApproachingDeliveryDate"]);

            GetDataPOLateDeliveryDate();
            lbTotalLatePODeliveryDate.Text = Convert.ToString(Session["POLateDeliveryDate"]);
        }

        private bool GetUserAccess()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();

                using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_UserControl", Con))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@Code", Session["Oid"].ToString());
                    sqlcomm.Parameters.AddWithValue("@Flag", "ModuleNav");

                    using (SqlDataReader dr = sqlcomm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["ModuleName"].ToString() == "purchase_order")
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
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

            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();

                using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_PurchaseOrder", Con))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@StatementType", "TotalPONotYetApproved");

                    using (SqlDataReader dr = sqlcomm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Session["TotalPONotYetApproved"] = Convert.ToInt32(dr["TotalPONotYetApproved"]);
                        }
                    }
                }
            }
            #region OldCode
            //string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //SqlConnection Con = new SqlConnection(path);
            //Con.Open();
            //SqlCommand sqlcomm = new SqlCommand();
            //sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            //sqlcomm.CommandType = CommandType.StoredProcedure;

            //sqlcomm.Connection = Con;
            //sqlcomm.Parameters.AddWithValue("@StatementType", "TotalPONotYetApproved");

            //SqlDataReader dr = null;
            //dr = sqlcomm.ExecuteReader();

            //if (dr.Read())
            //{
            //    Session.Add("TotalPONotYetApproved", (Int32)dr["TotalPONotYetApproved"]);
            //}
            //else
            //{

            //}
            #endregion
        }

        protected void GetDataPOApproachingDeliveryDate()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();
                using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_PurchaseOrder",Con))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@StatementType", "POApproachingDeliveryDate");

                    using (SqlDataReader dr = sqlcomm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Session.Add("POApproachingDeliveryDate", (Int32)dr["POApproachingDeliveryDate"]);
                        }
                    }
                }
            }
        }

        protected void GetDataPOLateDeliveryDate()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();
                using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_PurchaseOrder",Con))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@StatementType", "POLateDeliveryDate");

                    using (SqlDataReader dr = sqlcomm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Session.Add("POLateDeliveryDate", (Int32)dr["POLateDeliveryDate"]);
                        }
                    }
                }
            }
        }

        protected void GetDataPurchaseOrder()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection Con = new SqlConnection(path))
            {
                using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_PurchaseOrder", Con))
                {
                    sqlcomm.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewFilterPurchaseOrder");
                    sqlcomm.Parameters.AddWithValue("@po_no", txtPONo.Value);
                    sqlcomm.Parameters.AddWithValue("@date_from", txtDate1.Value);
                    sqlcomm.Parameters.AddWithValue("@date_to", txtDate2.Value);
                    sqlcomm.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);

                    using (SqlDataAdapter sda = new SqlDataAdapter(sqlcomm))
                    {
                        DataTable dtb = new DataTable();
                        sda.Fill(dtb);

                        ViewState["myViewState"] = dtb;

                        TablePurchaseOrder.DataSource = dtb;
                        TablePurchaseOrder.DataBind();

                        // Hide unnecessary columns safely (check bounds first)
                        int[] hiddenColumns = { 1, 4, 15, 16, 17, 18, 19, 20 };
                        foreach (int index in hiddenColumns)
                        {
                            if (index < TablePurchaseOrder.Columns.Count)
                            {
                                TablePurchaseOrder.Columns[index].Visible = false;
                            }
                        }

                        // Optional: hide column 24 if necessary and safe
                        // if (TablePurchaseOrder.Columns.Count > 24)
                        //     TablePurchaseOrder.Columns[24].Visible = false;

                        TablePurchaseOrder.UseAccessibleHeader = true;
                        if (TablePurchaseOrder.Rows.Count > 0)
                        {
                            TablePurchaseOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                }
            }
            #region OldCode
            //string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //using (SqlConnection Con = new SqlConnection(path))
            //{
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Connection = Con;
            //    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewFilterPurchaseOrder");
            //    sqlcomm.Parameters.AddWithValue("@po_no", txtPONo.Value);
            //    sqlcomm.Parameters.AddWithValue("@date_from", txtDate1.Value);
            //    sqlcomm.Parameters.AddWithValue("@date_to", txtDate2.Value);
            //    sqlcomm.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    ViewState["myViewState"] = dtb;
            //    TablePurchaseOrder.DataSource = dtb;
            //    TablePurchaseOrder.DataBind();

            //    TablePurchaseOrder.Columns[1].Visible = false;
            //    TablePurchaseOrder.Columns[4].Visible = false;
            //    TablePurchaseOrder.Columns[15].Visible = false;
            //    TablePurchaseOrder.Columns[16].Visible = false;
            //    TablePurchaseOrder.Columns[17].Visible = false;
            //    TablePurchaseOrder.Columns[18].Visible = false;
            //    TablePurchaseOrder.Columns[19].Visible = false;
            //    TablePurchaseOrder.Columns[20].Visible = false;
            //    //TablePurchaseOrder.Columns[24].Visible = false;

            //    TablePurchaseOrder.UseAccessibleHeader = true;
            //    TablePurchaseOrder.HeaderRow.TableSection = TableRowSection.TableHeader;

            //    dtb.Dispose();
            //    Con.Close();
            //    sqlcomm.Dispose();
            //    Con.Dispose();

            //    Con.Close();
            //}
            #endregion
        }


        protected void GetRFNumberNew()
        {
            string _vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection _vSQLCon = new SqlConnection(_vPath);
            SqlCommand _vSQLComm = new SqlCommand();

            _vSQLCon.Open();
            _vSQLComm.Connection = _vSQLCon;
            _vSQLComm.CommandType = CommandType.StoredProcedure;
            _vSQLComm.CommandText = "sp_PROCUREMENT_DB_Purchase";

            _vSQLComm.Parameters.AddWithValue("@StatementType", "FindRFNumberNew");

            DataTable _dt = new DataTable();
            SqlDataReader _dr = _vSQLComm.ExecuteReader();
            _dt.Load(_dr);

            TableRFNumber.DataSource = _dt;
            TableRFNumber.DataBind();
            TableRFNumber.UseAccessibleHeader = true;
            if (TableRFNumber.Rows.Count > 0)
            {
                TableRFNumber.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            _dt.Dispose();
            _vSQLCon.Close();
            _vSQLComm.Dispose();
            _vSQLCon.Dispose();

            _vSQLCon.Close();
        }


        protected void btnNewPOStandart_Click(object sender, EventArgs e)
        {

            GetRFNumberNew();
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

        //protected void GetRFNumber()
        //{
        //    string _vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection _vSQLCon = new SqlConnection(_vPath);
        //    SqlCommand _vSQLComm = new SqlCommand();

        //    _vSQLCon.Open();
        //    _vSQLComm.Connection = _vSQLCon;
        //    _vSQLComm.CommandType = CommandType.StoredProcedure;
        //    _vSQLComm.CommandText = "sp_PROCUREMENT_DB_Purchase";

        //    _vSQLComm.Parameters.AddWithValue("@StatementType", "FindRFNumber");
        //    _vSQLComm.Parameters.AddWithValue("@rf_no", txtRFNumber.Value);

        //    DataTable _dt = new DataTable();
        //    SqlDataReader _dr = _vSQLComm.ExecuteReader();
        //    _dt.Load(_dr);

        //    TableRFNumber.DataSource = _dt;
        //    TableRFNumber.DataBind();

        //    _dt.Dispose();
        //    _vSQLCon.Close();
        //    _vSQLComm.Dispose();
        //    _vSQLCon.Dispose();

        //    _vSQLCon.Close();
        //}


        //protected void btnSearchRFNumber_Click(object sender, EventArgs e)
        //{
        //    GetRFNumber();
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlSelectRFNumber').modal();", true);
        //}

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            Response.Redirect("purchase_order.aspx");
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //divBtnGenerateExcell.Visible = true;

            GetDataPurchaseOrder();

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "reinitDT",
                "initMainDT();",
                true
            );
        }


        protected void ViewDetailTotalPOIssued_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);



            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal","$('#mdlViewDetail').modal(); setTimeout(function(){ initModalDataTable(); }, 300);",true);

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

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "openModal",
                "$('#mdlViewDetail').modal('show');",
                true
            );

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "reinitModalDT",
                "initModalDT();",
                true
            );


        }

        protected void ViewDetailTotalPONotYetApproved_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);


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



            ScriptManager.RegisterStartupScript(
               this,
               GetType(),
               "openModal",
               "$('#mdlViewDetail').modal('show');",
               true
           );

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "reinitModalDT",
                "initModalDT();",
                true
            );
        }

        protected void ViewDetailTotalPOApproachingDeliveryDate_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
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

            ScriptManager.RegisterStartupScript(
               this,
               GetType(),
               "openModal",
               "$('#mdlViewDetail').modal('show');",
               true
           );

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "reinitModalDT",
                "initModalDT();",
                true
            );
        }

        protected void ViewDetailTotalLatePODeliveryDate_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewDetail').modal();", true);
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

            ScriptManager.RegisterStartupScript(
               this,
               GetType(),
               "openModal",
               "$('#mdlViewDetail').modal('show');",
               true
           );

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "reinitModalDT",
                "initModalDT();",
                true
            );
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

        //protected void btnGenerateExcell_Click(object sender, EventArgs e)
        //{
        //    string _vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection _vSQLCon = new SqlConnection(_vPath);
        //    SqlCommand _vSQLComm = new SqlCommand();

        //    _vSQLCon.Open();
        //    _vSQLComm.Connection = _vSQLCon;
        //    _vSQLComm.CommandType = CommandType.StoredProcedure;
        //    _vSQLComm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";

        //    _vSQLComm.Parameters.AddWithValue("@StatementType", "GenerateExcell");
        //    _vSQLComm.Parameters.AddWithValue("@po_no", txtPONo.Value);
        //    _vSQLComm.Parameters.AddWithValue("@date_from", txtDate1.Value);
        //    _vSQLComm.Parameters.AddWithValue("@date_to", txtDate2.Value);
        //    _vSQLComm.Parameters.AddWithValue("@status", ddlStatus.SelectedItem.Text);

        //    SqlDataAdapter sda = new SqlDataAdapter(_vSQLComm);
        //    using (DataTable dt = new DataTable())
        //    {
        //        sda.Fill(dt);
        //        string _vDate1 = txtDate1.Value.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
        //        string _vDate2 = txtDate2.Value.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt, "Report PO");
        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            Response.ClearContent();
        //            Response.AppendHeader("content-disposition", "attachment; filename=" + "Report PO " + "(" + _vDate1 + "-" + _vDate2 + ")" + ".xlsx");
        //            Response.ContentType = "application/excel";
        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                wb.SaveAs(MyMemoryStream);
        //                MyMemoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }
        //    }
        //}
    }
}