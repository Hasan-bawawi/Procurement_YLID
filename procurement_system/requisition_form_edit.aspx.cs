using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
//using Microsoft.Reporting.Map.WebForms.VirtualEarth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class requisition_form_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["rf_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailRF");
                sqlcomm.Parameters.AddWithValue("@rf_no", id);
                con.Open();
                using (SqlDataReader rdr = sqlcomm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Session.Add("rf_no", (string)rdr["rf_no"]);
                        Session.Add("id", (string)rdr["id"].ToString());
                        Session.Add("Requester", (string)rdr["Requester"]);
                        //Session.Add("ManagerApprove", (string)rdr["ManagerApprove"]);
                        //Session.Add("GMApprove", (string)rdr["GMApprove"]);
                        Session.Add("stok_code", (string)rdr["stok_code"]);
                        Session.Add("item_code", (string)rdr["item_code"]);
                        Session.Add("item_name", (string)rdr["item_name"]);
                        Session.Add("merk_name", (string)rdr["merk_name"]);
                        Session.Add("tipe", (string)rdr["tipe"]);
                        Session.Add("quantity", (int)rdr["quantity"]);
                        Session.Add("unit_name", (string)rdr["unit_name"]);
                        Session.Add("request_date", (DateTime)rdr["request_date"]);
                        Session.Add("remaks", (string)rdr["remaks"]);
                        Session.Add("status", (string)rdr["status"]);
                        Session.Add("type_request", (string)rdr["type_request"]);
                        Session.Add("status_approve", (string)rdr["status_approve"]);
                        Session.Add("description", (string)rdr["description"]);
                        //Session.Add("nik_approver", (string)rdr["nik_approver"]);
                        //Session.Add("nik_requester", (string)rdr["nik_requester"]);
                        //Session.Add("nik_gm_approver", (string)rdr["nik_gm_approver"]);
                        Session.Add("id_vendor", (string)rdr["id_vendor"].ToString());
                        //Session.Add("id_unit", (string)rdr["id_unit"].ToString());
                        Session.Add("nama_branch", (string)rdr["nama_branch"]);
                        Session.Add("DivisionRequester", (string)rdr["DivisionRequester"]);
                        Session.Add("SectionRequester", (string)rdr["SectionRequester"]);
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();
            }
            lbRFNumberBreadcrumb.Text = Session["rf_no"].ToString();
            lbRFNumberHeader.Text = Session["rf_no"].ToString();
            string ReqDateFromDatabase = Session["request_date"].ToString();
            DateTime ParseDatetime = DateTime.Parse(ReqDateFromDatabase);
            string ReqDate = ParseDatetime.ToString("dd MMMM yyyy");
            lbRequestDate.Text = ReqDate;
            lbDivision.Text = Session["DivisionRequester"].ToString();
            lbSection.Text = Session["SectionRequester"].ToString();
            lbRequester.Text = Session["Requester"].ToString();
            //lbApprovedBy.Text = Session["ManagerApprove"].ToString();
            //lbAcknowledgeBy.Text = Session["GMApprove"].ToString();
            lbLocation.Text = Session["nama_branch"].ToString();

            if (Session["status_approve"].ToString() == "NOT YET")
            {
                divRFCreated.Visible = true;
                divManagerApprove.Visible = false;
                divGMApprove.Visible = false;
                divPOIssued.Visible = false;
                divComplete.Visible = false;
            }
            else if (Session["status_approve"].ToString() == "Approved (MANAGER)")
            {
                divRFCreated.Visible = false;
                divManagerApprove.Visible = true;
                divGMApprove.Visible = false;
                divPOIssued.Visible = false;
                divComplete.Visible = false;
            }
            else
            {
                divRFCreated.Visible = false;
                divManagerApprove.Visible = false;
                divGMApprove.Visible = false;
                divPOIssued.Visible = false;
                divComplete.Visible = false;
            }

            if (!IsPostBack)
            {
                BindDataTableItemRF();
            }
        }

        protected void BindDataTableItemRF()
        {
            string id = Request.QueryString["rf_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailRF");
            sqlcomm.Parameters.AddWithValue("@rf_no", id);
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

        //protected void GridTemporary()
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPurchase");
        //    sqlcomm.Parameters.AddWithValue("@nota", lbRFNumber.Text);
        //    sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value);
        //    DataTable dtb = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

        //    sda.Fill(dtb);
        //    ViewState["myViewState"] = dtb;
        //    TableItemPurchase.DataSource = dtb;
        //    TableItemPurchase.DataBind();

        //    Con.Close();
        //}

        //protected void GetTableRF()
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPurchase");
        //    sqlcomm.Parameters.AddWithValue("@nota", lbRFNumber.Text);
        //    sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value);
        //    DataTable dtb = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

        //    sda.Fill(dtb);
        //    ViewState["myViewState"] = dtb;
        //    TableItemPurchase.DataSource = dtb;
        //    TableItemPurchase.DataBind();

        //    Con.Close();
        //}

        //protected void GetDetailItems()
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Items";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;

        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "DetailItem");
        //    sqlcomm.Parameters.AddWithValue("@kode_barang", ddlItem.SelectedValue);

        //    SqlDataReader dr = null;
        //    dr = sqlcomm.ExecuteReader();

        //    if (dr.Read())
        //    {
        //        Session.Add("id_barang", (int)dr["id_barang"]);
        //        Session.Add("kode_barang", (string)dr["kode_barang"]);
        //        Session.Add("merk", (string)dr["merk"]);
        //        Session.Add("tipe", (string)dr["tipe"]);
        //        Session.Add("kategori", (string)dr["kategori"]);
        //        Session.Add("nama_barang", (string)dr["nama_barang"]);
        //        Session.Add("nama_branch", (string)dr["nama_branch"]);
        //        Session.Add("stock_criteria", (string)dr["stock_criteria"]);
        //    }
        //    else
        //    {

        //    }
        //}

        //protected void GetItems()
        //{
        //    ddlItem.Items.Clear();
        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);

        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Items";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "AddDetailItem");
        //    sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value);

        //    SqlDataReader dr;


        //    try
        //    {
        //        ListItem newItem = new ListItem();
        //        newItem.Text = "<Item Name-Merk-Type>";
        //        newItem.Value = "0";
        //        ddlItem.Items.Add(newItem);

        //        Con.Open();
        //        dr = sqlcomm.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            newItem = new ListItem();
        //            newItem.Text = "(" + dr["nama_barang"].ToString() + ")" + " - (" + dr["merk"].ToString() + ")" + " - (" + dr["tipe"].ToString() + ")";
        //            newItem.Value = dr["kode_barang"].ToString();
        //            ddlItem.Items.Add(newItem);
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception err)
        //    {
        //        string _ErrorMsg = err.Message;
        //    }
        //    finally
        //    {
        //        Con.Close();
        //    }
        //}

        protected void TableItemPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableItemPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableItemPurchase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void submitrow(string remarks, string description, int qty, string kodebarang, string rfnumber)
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "UpdatePurchase");
        //    sqlcomm.Parameters.AddWithValue("@nota", rfnumber);
        //    sqlcomm.Parameters.AddWithValue("@jumlah_beli", qty);
        //    sqlcomm.Parameters.AddWithValue("@remaks", remarks);
        //    sqlcomm.Parameters.AddWithValue("@description", description);
        //    sqlcomm.Parameters.AddWithValue("@kode_barang", kodebarang);

        //    sqlcomm.ExecuteNonQuery();
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
        //    sqlcomm.Dispose();
        //    Con.Close();
        //    Con.Dispose();
        //}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in TableItemPurchase.Rows)
            //{
            //    HtmlInputGenericControl qty = (HtmlInputGenericControl)row.FindControl("txtQuantity");
            //    //string getQty = qty.Value;
            //    int getQty = Convert.ToInt32(qty.Value);

            //    HtmlInputText description = (HtmlInputText)row.FindControl("txtDescription");
            //    string getDescription = description.Value;

            //    HtmlInputText remarks = (HtmlInputText)row.FindControl("txtRemarks");
            //    string getremarks = remarks.Value;

            //    string kodebarang = row.Cells[1].Text;

            //    string nota = lbRFNumber.Text;

            //    submitrow(getremarks, getDescription, getQty, kodebarang, nota);
            //}
        }

        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;

        //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_IT_STOCK_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "Delete");
        //    sqlcomm.Parameters.AddWithValue("@kode_barang", row.Cells[1].Text);
        //    sqlcomm.Parameters.AddWithValue("@nota", lbRFNumber.Text);
            
        //    sqlcomm.ExecuteNonQuery();
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncDelete();", true);
        //    sqlcomm.Dispose();
        //    Con.Close();
        //    Con.Dispose();
        //    GetTableRF();
        //}

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            //GetItems();
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            //GetTableRF();
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlItem.SelectedItem.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            //}
            //else
            //{
            //    //GetDetailItems();
            //    string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_IT_STOCK_Purchase";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Connection = Con;
            //    sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchase_BarangMasuk");
            //    sqlcomm.Parameters.AddWithValue("@nota", lbRFNumber.Text.Trim());
            //    sqlcomm.Parameters.AddWithValue("@status_approve", hlbStatusApprove.Value);
            //    sqlcomm.Parameters.AddWithValue("@status", hlbStatus.Value);
            //    sqlcomm.Parameters.AddWithValue("@nik_approver", hlbNIKApprover.Value);
            //    sqlcomm.Parameters.AddWithValue("@nik_gm_approver", "890556");
            //    sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIKRequester.Value.Trim());
            //    sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
            //    sqlcomm.Parameters.AddWithValue("@kode_barang", ddlItem.SelectedValue);
            //    sqlcomm.Parameters.AddWithValue("@jumlah_beli", "");
            //    sqlcomm.Parameters.AddWithValue("@tanggal_beli", lbReqDate.Text);
            //    sqlcomm.Parameters.AddWithValue("@remaks", "");
            //    sqlcomm.Parameters.AddWithValue("@type_request", lbReqType.Text.ToString());
            //    sqlcomm.Parameters.AddWithValue("@description", "");
            //    sqlcomm.Parameters.AddWithValue("@tanggal_input", lbReqDate.Text);
            //    sqlcomm.Parameters.AddWithValue("@quantity_input", "0");

            //    sqlcomm.ExecuteNonQuery();
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncAdd();", true);
            //    sqlcomm.Dispose();
            //    Con.Close();
            //    Con.Dispose();
            //    GetTableRF();
            //}
        }

        protected void btnDownloadRF_Click(object sender, EventArgs e)
        {

        }
    }
}