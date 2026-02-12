using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Identity.Client;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZXing;

namespace procurement_system
{
    public partial class create_purchase_order_standart : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];
        string _emailGAMgr = WebConfigurationManager.AppSettings["emailGAMgr"];
        string _emailITMgr = WebConfigurationManager.AppSettings["emailITMgr"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["nik"])))
            {
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }

            if (Session["GroupName"].ToString() == "Admin Purchasing")
            {
                string id = Request.QueryString["rf_no"];
                lblRFNumber.Text = id;
                if (!IsPostBack)
                {
                    //GridTemporary();
                    //GetSection();
                    BindDetailitemRF();
                    BindDataTableItemRF();
                    

                }
            }
            else
            {
                Response.Write("<script>alert('Access Denied!!, Purchasing Team Only!'),window.location.href = 'login.aspx';</script>");
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
            sqlcomm.Parameters.AddWithValue("@StatementType", /*"ViewDetailRF"*/ "ViewDetailRFNew");
            sqlcomm.Parameters.AddWithValue("@rf_no", id);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDetailsRF.DataSource = dtb;
            TableDetailsRF.DataBind();

            //TableDetailsRF.Columns[8].Visible = false;
            //TableDetailsRF.Columns[10].Visible = false;
            //TableDetailsRF.Columns[11].Visible = false;
            //TableDetailsRF.Columns[12].Visible = false;
            //TableDetailsRF.Columns[13].Visible = false;
            //TableDetailsRF.Columns[14].Visible = false;
            //TableDetailsRF.Columns[15].Visible = false;
            hlbCatalog.Value = TableDetailsRF.Rows[0].Cells[3].Text;
            txtOIDReqDept.Value = TableDetailsRF.DataKeys[0]["IDDivisionRequester"].ToString();
            txtReqBy.Value = TableDetailsRF.Rows[0].Cells[4].Text.ToString();
            TableDetailsRF.UseAccessibleHeader = true;
            TableDetailsRF.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void BindDetailitemRF()
        {
            string id = Request.QueryString["rf_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_Purchase", Con);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StatementType", "DetailitemRF");
                sqlcomm.Parameters.AddWithValue("@rf_no", id);
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
                DataTable dtb = new DataTable();
                sda.Fill(dtb);

                //ViewState["myViewState"] = dtb;
                //TableDetailsRF.DataSource = dtb;
                //TableDetailsRF.DataBind();

                // Convert ke JSON
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(dtb);
                hfDetailRF.Value = json;
            }
        }

        protected async void btnSubItemsatuan_Click(object sender, EventArgs e)
        {
            string jsonData = hfFormData.Value;
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var formData = serializer.Deserialize<Dictionary<string, string>>(jsonData);
            
            
            int rowIndex = Convert.ToInt32(formData["RowIndex"]);
            string Catalog = TableDetailsRF.Rows[rowIndex].Cells[3].Text.ToString();
            //string remark = /*TableDetailsRF.DataKeys[rowIndex]["Remark"].ToString();*/ formData["remarks"];
            string iddivision = TableDetailsRF.DataKeys[rowIndex]["IDDivisionRequester"].ToString();
            string deliveryTo = formData["DeliveryTo"];
            string deliveryDate = formData["DeliveryDate"];
            string vat = formData["Vat"];
            string assetType = formData["AssetType"];
            string paymentTerm = formData["PaymentTerm"];
            string otherCondition = formData["OtherCondition"];
            string id_vendor = formData["IDVendor"];
            string deliveryselect = formData["Deliveryselect"].Trim();
            hfAttachmentPath.Value = formData["FilePath"];
            string attachmentpathPO = "~/" + formData["FilePath"];

            //return;

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();
                SqlTransaction transaction = Con.BeginTransaction();

                try
                {
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Connection = Con;
                    sqlcomm.Transaction = transaction;

                    sqlcomm.Parameters.AddWithValue("@StatementType", "SavesatuanPO");
                    sqlcomm.Parameters.AddWithValue("@delivery_to", deliveryTo);
                    sqlcomm.Parameters.AddWithValue("@delivery_date", deliveryDate);
                    sqlcomm.Parameters.AddWithValue("@vat", vat);
                    sqlcomm.Parameters.AddWithValue("@aset_status", assetType);
                    sqlcomm.Parameters.AddWithValue("@payment_term", paymentTerm);
                    //sqlcomm.Parameters.AddWithValue("@other_condition", otherCondition);
                    sqlcomm.Parameters.AddWithValue("@other_condition",string.IsNullOrWhiteSpace(otherCondition) ? (object)DBNull.Value : otherCondition);

                    //sqlcomm.Parameters.AddWithValue("@remarks", remark);
                    sqlcomm.Parameters.AddWithValue("@rf_no", lblRFNumber.Text);
                    sqlcomm.Parameters.AddWithValue("@id_vendor", id_vendor);
                    sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                    sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                    sqlcomm.Parameters.AddWithValue("@catalog_type", Catalog);
                    sqlcomm.Parameters.AddWithValue("@status", "PO Created");
                    sqlcomm.Parameters.AddWithValue("@deliveryselect", deliveryselect);
                    sqlcomm.Parameters.AddWithValue("@attachment_path", attachmentpathPO);
                    sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                    sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                    sqlcomm.Parameters.AddWithValue("@requesting_dept", iddivision);
                    //sqlcomm.ExecuteNonQuery();


                    //object result = sqlcomm.ExecuteScalar();
                    //transaction.Commit();                   
                    //string newPONumber = result != null ? result.ToString() : "";

                    //txtPONumber.Value = newPONumber;

                    string newPONumber = "";
                    string Headapprover = "";
                    bool poAllCreated = false;

                    using (SqlDataReader dr = sqlcomm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            newPONumber = dr["NoPO"].ToString();
                            poAllCreated = Convert.ToBoolean(dr["POallcreated"]);
                            Headapprover = dr["Headapprover"].ToString();

                        }
                    }

                    transaction.Commit();
                    txtPONumber.Value = newPONumber;
                    txtReqBy.Value = TableDetailsRF.Rows[rowIndex].Cells[4].Text.ToString();
                    txtReqDept.Value = TableDetailsRF.Rows[rowIndex].Cells[5].Text.ToString(); ;
                    DateTime currentDateTime = DateTime.Now;
                    txtIssuedDate.Value = currentDateTime.ToString();
 
                    string oldFilePath = Server.MapPath("~/"+hfAttachmentPath.Value);

                    if (File.Exists(oldFilePath))
                    {
                        //string folderPath = Path.GetDirectoryName(oldFilePath);
                        //string ext = Path.GetExtension(oldFilePath);
                        //string newFilePath = Path.Combine(folderPath, "AttachmentPO-"+ newPONumber + ext);
                        string newFileName = /*"AttachmentPO-"+ newPONumber + ext;*/ Path.GetFileName(oldFilePath);

                        //if (File.Exists(newFilePath))
                        //    File.Delete(newFilePath);

                        //File.Move(oldFilePath, newFilePath);
                        hfAttachmentPath.Value = newFileName;
                    }

                    if (Catalog == "IT")
                    {

                        await SendEmailToManagerIT("satuan", newPONumber, hfAttachmentPath.Value,poAllCreated,Headapprover);

                    }
                    else
                    {
                        await SendEmailToManagerGA("satuan", newPONumber, hfAttachmentPath.Value,poAllCreated,Headapprover);


                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }

            }

        }



        protected async void btnSubmitAll_Click(object sender, EventArgs e)
        {

            string jsonData = hfFormData.Value;
            if (string.IsNullOrEmpty(jsonData)) return;

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var dataList = serializer.Deserialize<List<FormPOData>>(jsonData);


            DataTable tvp = new DataTable();
            tvp.Columns.Add("IdVendor", typeof(Guid));
            tvp.Columns.Add("Vendor", typeof(string));
            tvp.Columns.Add("RFnum", typeof(string));
            tvp.Columns.Add("DeliverySelect", typeof(string));
            tvp.Columns.Add("DeliverToArea", typeof(string));
            tvp.Columns.Add("DeliveryDate", typeof(DateTime));
            tvp.Columns.Add("Vat", typeof(decimal));
            tvp.Columns.Add("PaymentTerm", typeof(string));
            tvp.Columns.Add("AssetType", typeof(string));
            tvp.Columns.Add("OtherCondition", typeof(string));
            //tvp.Columns.Add("FileName", typeof(string));
            //tvp.Columns.Add("OldNameFile", typeof(string));
            tvp.Columns.Add("AttachmentPath", typeof(string));
            tvp.Columns.Add("Catalog", typeof(string));


            //return;

            foreach (var item in dataList)
            {
                tvp.Rows.Add(
                    Guid.Parse(item.idVendor),
                    item.vendor ?? "",                    
                    lblRFNumber.Text,
                    item.deliveryoptn.Trim() ?? "",
                    item.deliverToarea ?? "",
                    string.IsNullOrEmpty(item.deliveryDate) ? (object)DBNull.Value : DateTime.Parse(item.deliveryDate),
                    string.IsNullOrEmpty(item.vat) ? 0 : Convert.ToDecimal(item.vat),
                    item.paymentTerm ?? "",
                    item.assetType ?? "",
                    item.otherCondition ?? "",
                    //item.filename ?? "",
                    //item.oldnamefile ?? "",
                    item.filename ?? "",
                    hlbCatalog.Value.ToString()
                );
            }

            //return;

            try
            {
                
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                   
                    using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_GenerateALLPO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var tvpParam = cmd.Parameters.AddWithValue("@POData", tvp);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.FormPODataType"; //nama type tvp di database


                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"] ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);


                        await con.OpenAsync();
         
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            List<(string PONumber, string Catalog, string AttachmentPath, string headapprover)> poResults = new List<(string, string, string,string)>();

                            while (await reader.ReadAsync())
                            {
                                string poNumber = reader["PONumber"].ToString();
                                string catalog = reader["Catalog"].ToString();
                                string attachment = reader["AttachmentPath"].ToString();
                                string headapprover = reader["Headapprover"].ToString();
                                poResults.Add((poNumber, catalog, attachment,headapprover));

                            }

                            if (poResults.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "swal",
                                    "swal('Warning', 'No records returned from database.', 'warning');", true);
                                return;
                            }


                            int totalPO = poResults.Count;
                            int successCount = 0;

                            // === Kirim email berdasarkan Catalog ===
                            foreach (var po in poResults)
                            {

                                try
                                {

                                if (po.Catalog.Equals("IT", StringComparison.OrdinalIgnoreCase))
                                {   
                                    DateTime CurrunteDate = DateTime.Now;
                                    txtIssuedDate.Value = CurrunteDate.ToString();

                                        string oldFilePath = Server.MapPath("~/" + po.AttachmentPath);

                                    if (File.Exists(oldFilePath))
                                    {
                                        
                                        string newFileName = Path.GetFileName(oldFilePath);
                                        hfAttachmentPath.Value = newFileName;
                                    }

                                    await SendEmailToManagerIT("all",po.PONumber,hfAttachmentPath.Value,true,po.headapprover);
                                        successCount++;
                                    }
                                else if  (po.Catalog.Equals("GA", StringComparison.OrdinalIgnoreCase))
                                {
                                    DateTime CurrunteDate = DateTime.Now;
                                    txtIssuedDate.Value = CurrunteDate.ToString();

                                    string oldFilePath = Server.MapPath("~/" + po.AttachmentPath);

                                    if (File.Exists(oldFilePath))
                                    {
                                        string newFileName = Path.GetFileName(oldFilePath);
                                        hfAttachmentPath.Value = newFileName;
                                    }
                                    await SendEmailToManagerGA("all", po.PONumber, hfAttachmentPath.Value,true,po.headapprover);
                                    successCount++;
                                }

                                }
                                catch
                                {
                                    continue;
                                }

                            }

                            //string swalMessage = $"swal('Success', 'Data successfully created po = {totalPO} submitted and emails sent = {successCount}', 'success');";
                            //ScriptManager.RegisterStartupScript(this, GetType(), "swal", swalMessage, true);
                            string swalMessage = $@"
                                swal('Success', 'Data successfully created po = {totalPO} submitted and emails sent = {successCount}', 'success');
                                setTimeout(function() {{
                                    window.location.href = 'purchase_order.aspx';
                                }}, 2000);";

                            ScriptManager.RegisterStartupScript(this, GetType(), "swalSuccessAll", swalMessage, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "\\'");
                //ScriptManager.RegisterStartupScript(this, GetType(), "swal", $"swal('Error', 'Submit failed: {message}', 'error');", true);
                string rf = lblRFNumber.Text;

                string script = $@"
                swal({{
                    title: 'Error',
                    text: 'Submit failed: {message}',
                    type: 'error'
                }}, function() {{
                    window.location.href = 'create_purchase_order_standart.aspx?rf_no={rf}';
                }});";

                ScriptManager.RegisterStartupScript(this, GetType(), "swal", script, true);


            }

        }

        public class FormPOData
        {
            public string idVendor { get; set; }
            public string vendor { get; set; }
            public string deliveryTo { get; set; }
            public string deliveryoptn { get; set; }
            public string deliverToarea { get; set; }
            public string deliveryDate { get; set; }
            public string vat { get; set; }
            public string paymentTerm { get; set; }
            public string assetType { get; set; }
            public string otherCondition { get; set; }
            public string filename { get; set; }
            public string oldnamefile { get; set; }
            public string attachmentPath { get; set; }

        }



        //protected void GetSection()
        //{
        //    ddlRequester.Items.Clear();
        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);

        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "AddSectionName");

        //    SqlDataReader dr;


        //    try
        //    {
        //        ListItem newItem = new ListItem();
        //        newItem.Text = "";
        //        newItem.Value = "00000000-0000-0000-0000-000000000000";
        //        ddlRequester.Items.Add(newItem);

        //        Con.Open();
        //        dr = sqlcomm.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            newItem = new ListItem();
        //            newItem.Text = dr["section"].ToString();
        //            newItem.Value = dr["id"].ToString();
        //            ddlRequester.Items.Add(newItem);
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

        protected void GridTemporary()
        {
            DataTable dt = (DataTable)ViewState["GetRecords"];
            TableItemPO.DataSource = dt;
            TableItemPO.DataBind();
        }

        private void GetSelectedRows()
        {
            DataTable dt;
            if (ViewState["GetRecords"] != null)
                dt = (DataTable)ViewState["GetRecords"];
            else
                dt = CreateTable();
            for (int i = 0; i < TableRequesitionItem.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)TableRequesitionItem.Rows[i].Cells[0].FindControl("chkSelect");
                if (chk.Checked)
                {
                    dt = AddGridRow(TableRequesitionItem.Rows[i], dt);
                }
                else
                {
                    dt = RemoveRow(TableRequesitionItem.Rows[i], dt);
                }
            }
            ViewState["GetRecords"] = dt;
        }

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RF No.");
            dt.Columns.Add("catalog_type");
            dt.Columns.Add("item_code");
            dt.Columns.Add("Item");
            dt.Columns.Add("Qty");
            dt.Columns.Add("UOM");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Price");
            dt.Columns.Add("Amount");

            dt.AcceptChanges();
            return dt;
        }

        private DataTable AddGridRow(GridViewRow gvRow, DataTable dt)
        {
            DataRow[] dr = dt.Select("item_code = '" + gvRow.Cells[6].Text + "' AND [RF No.] = '" + gvRow.Cells[1].Text + "'");
            if (dr.Length <= 0)
            {
                dt.Rows.Add();
                int rowscount = dt.Rows.Count - 1;
                dt.Rows[rowscount]["item_code"] = gvRow.Cells[6].Text;
                dt.Rows[rowscount]["RF No."] = gvRow.Cells[1].Text;
                dt.Rows[rowscount]["Item"] = gvRow.Cells[7].Text;
                dt.Rows[rowscount]["Qty"] = gvRow.Cells[10].Text;
                dt.Rows[rowscount]["UOM"] = gvRow.Cells[11].Text;
                dt.Rows[rowscount]["Remark"] = gvRow.Cells[13].Text;
                dt.Rows[rowscount]["Price"] = gvRow.Cells[18].Text;
                dt.Rows[rowscount]["Amount"] = gvRow.Cells[22].Text;
                dt.Rows[rowscount]["catalog_type"] = gvRow.Cells[23].Text;
                dt.AcceptChanges();
            }
            return dt;
        }

        private DataTable RemoveRow(GridViewRow gvRow, DataTable dt)
        {
            DataRow[] dr = dt.Select("item_code = '" + gvRow.Cells[6].Text + "'");
            if (dr.Length > 0)
            {
                dt.Rows.Remove(dr[0]);
                dt.AcceptChanges();
            }
            return dt;
        }

        protected void ddlRequester_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlAssetStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GetDataItemRF()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewItemPurchaseNotYetPO");
            sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequesitionItem.DataSource = dtb;
            TableRequesitionItem.DataBind();

            TableRequesitionItem.Columns[2].Visible = false;
            TableRequesitionItem.Columns[20].Visible = false;
            TableRequesitionItem.Columns[22].Visible = false;
            TableRequesitionItem.Columns[23].Visible = false;
            TableRequesitionItem.Columns[18].Visible = false;
            TableRequesitionItem.Columns[9].Visible = false;

            TableRequesitionItem.UseAccessibleHeader = true;
            TableRequesitionItem.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            if (txtVendorName.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectVendor();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
                GetDataItemRF();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
                GetDataItemRF();
            }
        }

        protected void TableItemPO_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableItemPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableItemPO_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            DataTable dt = (DataTable)ViewState["GetRecords"];
            dt.Rows.RemoveAt(row.RowIndex);

            TableItemPO.DataSource = dt;
            TableItemPO.DataBind();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);

            int total = 0;
            foreach (GridViewRow grow in TableItemPO.Rows)
            {
                HtmlInputText amount = (HtmlInputText)grow.FindControl("txtAmount");
                decimal parsedValue = decimal.Parse(amount.Value, NumberStyles.Currency);
                int getAmount = Convert.ToInt32(parsedValue);
                total += getAmount;
            }

            int vat = Convert.ToInt32(txtVAT.Text.ToString());
            decimal vatValue = vat / 100m;
            int getTotal = Convert.ToInt32(total);
            decimal vatAmount;

            vatAmount = vatValue * getTotal;

            txtVatAmount.Value = vatAmount.ToString("#,##0.00");

            decimal grandTotal = getTotal + vatAmount;
            txtGrandTotal.Value = grandTotal.ToString("#,##0.00");
            int getGrandTotal = Convert.ToInt32(grandTotal);
            hlbGrandTotal.Value = getGrandTotal.ToString();

            // Display the total value
            decimal value;
            value = Convert.ToDecimal(total);
            txtTotalAmount.Value = value.ToString("#,##0.00");
        }

        protected async void btnSubmitPO_Click(object sender, EventArgs e)
        {
            if (txtIssuedDate.Value == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectIssuedDate();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Please select issued date!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            else if (txtDeliveryDate.Value == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectDeliveryDate();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Please select Delivery date!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            else if (ddlDeliveryTo.SelectedItem.Text == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectDeliveryTo();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Please enter Delivery Location!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            //else if (ddlRequester.SelectedItem.Text.ToString() == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectRequester();", true);
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            //}
            else if (ddlAssetStatus.SelectedItem.Text.ToString() == "Select Asset")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectAssetType();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Please select Asset type!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            else if (txtPaymentTerms.Value == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectPaymentTerm();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Enter Payment Terms!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            else if (txtTotalAmount.Value == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectItem();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed, Please select Item!');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewPO').modal();", true);
            }
            else
            {


                string path_db = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path_db))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {

                        if (hlbCatalog.Value  == "IT")
                        {
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Connection = con;
                            sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT");
                            //sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                            //sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                            //sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                            //sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                            //sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                            //sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                            //sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                            //HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                            //decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                            //int getprice = Convert.ToInt32(parsedValue);
                            //sqlcomm.Parameters.AddWithValue("@price", getprice);
                            //sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                            //HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                            //decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                            //int getAmount = Convert.ToInt32(parsedAmount);
                            //sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                            //sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                            //sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                            //sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                            //sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                            //sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                            //sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                            //sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                            //sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                            //sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                            //sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                            //sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                            //sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                            sqlcomm.ExecuteNonQuery();
                            //CheckUploadDocument();
                            sqlcomm.Dispose();
                            con.Close();
                            con.Dispose();

                            //await SendEmailToManagerIT();

                        }
                        else
                        {

                            //SqlCommand sqlcomm = new SqlCommand();
                            //sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                            //sqlcomm.CommandType = CommandType.StoredProcedure;
                            //sqlcomm.Connection = con;
                            //sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT");
                            //sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                            //sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                            //sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                            //sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                            //sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                            //sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                            //sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                            //HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                            //decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                            //int getprice = Convert.ToInt32(parsedValue);
                            //sqlcomm.Parameters.AddWithValue("@price", getprice);
                            //sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                            //HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                            //decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                            //int getAmount = Convert.ToInt32(parsedAmount);
                            //sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                            //sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                            //sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                            //sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                            //sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                            //sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                            //sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                            //sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                            //sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                            //sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                            //sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                            //sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                            //sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);



                            //sqlcomm.Dispose();
                            //con.Close();
                            //con.Dispose();
                            //await SendEmailToManagerGA();
                        }

                        #region


                        //GetPONumber();
                        //var CurentYear = DateTime.Now.Year;

                        //if (CurentYear != (int)Session["years"])
                        //{
                        //    SaveNumbering(con, transaction);
                        //    GetPONumberNew();

                        //    hlbYearsNew.Value = DateTime.Now.Year.ToString();
                        //    hlblast_numberNew.Value = Session["last_numberNew"].ToString();
                        //    int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
                        //    int _getNumberUrut = _LastNumber + 1;

                        //    if (_getNumberUrut < 10)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "000" + _getNumberUrut;
                        //    }
                        //    else if (_getNumberUrut > 9 && _getNumberUrut < 99)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "00" + _getNumberUrut;
                        //    }
                        //    else if (_getNumberUrut > 99 && _getNumberUrut < 999)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "0" + _getNumberUrut;
                        //    }
                        //    else if (_getNumberUrut > 999)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + _getNumberUrut;
                        //    }

                        //    Int32 grandtotal;
                        //    grandtotal = Convert.ToInt32(hlbGrandTotal.Value);

                        //    if (grandtotal < 1000000)
                        //    {
                        //        SaveMasterPO_under_1JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }

                        //    }
                        //    else if (grandtotal > 1000000 && grandtotal < 20000000)
                        //    {
                        //        SaveMasterPO_beetwen_1JT_20JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT_Up1JT_Under20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA_Up1JT_Under20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }
                        //    }
                        //    else if (grandtotal > 20000000)
                        //    {
                        //        SaveMasterPO_Up20JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT_Up20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA_Up20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    UpdateNumbering(con, transaction);
                        //    //GetPONumberNew();
                        //    //hlblast_numberNew.Value = Session["last_number"].ToString();
                        //    int _LastNumber = (Convert.ToInt32(hlblast_numberNew.Value) + 1);
                        //    if (_LastNumber < 10)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "000" + _LastNumber;
                        //    }
                        //    else if (_LastNumber > 9 && _LastNumber < 99)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "00" + _LastNumber;
                        //    }
                        //    else if (_LastNumber > 99 && _LastNumber < 999)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + "0" + _LastNumber;
                        //    }
                        //    else if (_LastNumber > 999)
                        //    {
                        //        txtPONumber.Value = "YLID-PO-" + CurentYear + "-" + _LastNumber;
                        //    }

                        //    Int32 grandtotal;
                        //    grandtotal = Convert.ToInt32(hlbGrandTotal.Value);

                        //    //LOGIC BARU 

                        //    if (grandtotal < 1000000)
                        //    {
                        //        SaveMasterPO_under_1JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }

                        //    }
                        //    else if (grandtotal > 1000000 && grandtotal < 20000000)
                        //    {
                        //        SaveMasterPO_beetwen_1JT_20JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT_Up1JT_Under20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA_Up1JT_Under20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }
                        //    }
                        //    else if (grandtotal > 20000000)
                        //    {
                        //        SaveMasterPO_Up20JT();
                        //        UpdateNoPO_RF();

                        //        if (hlbCatalog.Value == "IT")
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderIT_Up20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerIT();
                        //        }
                        //        else
                        //        {
                        //            foreach (GridViewRow row in TableItemPO.Rows)
                        //            {
                        //                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        //                SqlConnection Con = new SqlConnection(path);
                        //                Con.Open();
                        //                SqlCommand sqlcomm = new SqlCommand();
                        //                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                        //                sqlcomm.CommandType = CommandType.StoredProcedure;
                        //                sqlcomm.Connection = Con;
                        //                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseOrderGA_Up20JT");
                        //                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                        //                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                        //                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                        //                sqlcomm.Parameters.AddWithValue("@delivery_to", ddlDeliveryTo.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                        //                HtmlInputText price = (HtmlInputText)row.FindControl("txtPrice");
                        //                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                        //                int getprice = Convert.ToInt32(parsedValue);
                        //                sqlcomm.Parameters.AddWithValue("@price", getprice);
                        //                sqlcomm.Parameters.AddWithValue("@vat", Convert.ToInt32(txtVAT.Text.ToString()));
                        //                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                        //                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                        //                int getAmount = Convert.ToInt32(parsedAmount);
                        //                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                        //                sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                        //                sqlcomm.Parameters.AddWithValue("@remarks", row.Cells[5].Text.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                        //                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                        //                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                        //                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                        //                sqlcomm.Parameters.AddWithValue("@requesting_dept", txtOIDReqDept.Value);
                        //                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                        //                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());
                        //                sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                        //                sqlcomm.ExecuteNonQuery();
                        //                CheckUploadDocument();
                        //                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        //                sqlcomm.Dispose();
                        //                Con.Close();
                        //                Con.Dispose();
                        //            }
                        //            await SendEmailToManagerGA();
                        //        }
                        //    }
                        //}
                        #endregion

                        transaction.Commit();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        // Jika terjadi kesalahan, rollback transaksi
                        transaction.Rollback();

                        // Tangkap dan tangani kesalahan di sini
                        Response.Write($"Error: {ex.Message}");
                    }
                }
            }
        }

        protected void SaveMasterPO_under_1JT()
        {
            string searchTerm = "IT"; // Get the search term (case-insensitive)

            // Loop through DataGridView rows
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                if (row.Cells[2].Text.ToString() != null)
                {
                    string cellValue = row.Cells[2].Text.ToString();

                    // Check if the cell value matches the search term (case-sensitive)
                    if (cellValue == searchTerm)
                    {
                        hlbCatalog.Value = "IT";
                    }
                    else
                    {
                        hlbCatalog.Value = "GA";
                    }
                }
            }
            if (hlbCatalog.Value == "IT")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderIT");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderGA");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }
            
        }

        protected void SaveMasterPO_beetwen_1JT_20JT()
        {
            string searchTerm = "IT"; // Get the search term (case-insensitive)

            // Loop through DataGridView rows
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                if (row.Cells[2].Text.ToString() != null)
                {
                    string cellValue = row.Cells[2].Text.ToString();

                    // Check if the cell value matches the search term (case-sensitive)
                    if (cellValue == searchTerm)
                    {
                        hlbCatalog.Value = "IT";
                    }
                    else
                    {
                        hlbCatalog.Value = "GA";
                    }
                }
            }
            if (hlbCatalog.Value == "IT")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderIT_Up1JT_Under20JT");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderGA_Up1JT_Under20JT");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }

        }

        protected void SaveMasterPO_Up20JT()
        {
            string searchTerm = "IT"; // Get the search term (case-insensitive)

            // Loop through DataGridView rows
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                if (row.Cells[2].Text.ToString() != null)
                {
                    string cellValue = row.Cells[2].Text.ToString();

                    // Check if the cell value matches the search term (case-sensitive)
                    if (cellValue == searchTerm)
                    {
                        hlbCatalog.Value = "IT";
                    }
                    else
                    {
                        hlbCatalog.Value = "GA";
                    }
                }
            }
            if (hlbCatalog.Value == "IT")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderIT_Up20JT");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseOrderGA_Up20JT");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                sqlcomm.Parameters.AddWithValue("@id_vendor", hlbIDVendor.Value);
                sqlcomm.Parameters.AddWithValue("@po_date", txtIssuedDate.Value);
                sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@create_date", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@po_type", "PO Standart");
                sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }

        }

        protected void UpdateNoPO_RF()
        {
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNoPO_RF");
                sqlcomm.Parameters.AddWithValue("@po_no", txtPONumber.Value);
                HtmlInputText amount = (HtmlInputText)row.FindControl("txtAmount");
                decimal parsedAmount = decimal.Parse(amount.Value, NumberStyles.Currency);
                int getAmount = Convert.ToInt32(parsedAmount);
                sqlcomm.Parameters.AddWithValue("@amount", getAmount);
                sqlcomm.Parameters.AddWithValue("@status", "PO Created");
                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[3].Text.ToString());
                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[1].Text.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }

        }

        protected void btnCloseModalAddItem_Click(object sender, EventArgs e)
        {

        }

        protected void TableRequesitionItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableRequesitionItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 qty;
                Int32 amount;
                Int32 price;

                price = Convert.ToInt32(e.Row.Cells[18].Text.ToString());
                qty = Convert.ToInt32(e.Row.Cells[10].Text.ToString());
                amount = price * qty;
                TableCell CellAmount = e.Row.Cells[22];
                CellAmount.Text = amount.ToString();
            }
        }

        protected void TableRequesitionItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnSubmitItem_Click(object sender, EventArgs e)
        {
            divUploadFile.Visible = true;
            divSubmit.Visible = true;
            GetSelectedRows();
            GridTemporary();
            decimal total = 0;
            for (int i = 0; i < TableRequesitionItem.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)TableRequesitionItem.Rows[i].Cells[0].FindControl("chkSelect");
                if (chk.Checked)
                {
                    decimal fieldValue = Convert.ToDecimal(TableRequesitionItem.Rows[i].Cells[22].Text);
                    total += fieldValue;
                }
            }
            int vat = Convert.ToInt32(txtVAT.Text.ToString());
            decimal vatValue = vat / 100m;
            int getTotal = Convert.ToInt32(total);
            decimal vatAmount;
            //string vatDecimal= vatValue.ToString("0.00");

            vatAmount = vatValue * getTotal;

            txtVatAmount.Value = vatAmount.ToString("#,##0");

            decimal grandTotal = getTotal + vatAmount;
            txtGrandTotal.Value = grandTotal.ToString("#,##0");
            int getGrandTotal = Convert.ToInt32(grandTotal);
            hlbGrandTotal.Value = getGrandTotal.ToString();

            // Display the total value
            decimal value;
            Decimal.TryParse(total.ToString(), out value);
            txtTotalAmount.Value = value.ToString("#,##0");
        }

        protected void txtVAT_TextChanged(object sender, EventArgs e)
        {
            int total = 0;
            foreach (GridViewRow grow in TableItemPO.Rows)
            {
                HtmlInputText amount = (HtmlInputText)grow.FindControl("txtAmount");
                decimal parsedValue = decimal.Parse(amount.Value, NumberStyles.Currency);
                int getAmount = Convert.ToInt32(parsedValue);
                total += getAmount;
            }

            int vat = Convert.ToInt32(txtVAT.Text.ToString());
            decimal vatValue = vat / 100m;
            int getTotal = Convert.ToInt32(total);
            decimal vatAmount;
            //string vatDecimal= vatValue.ToString("0.00");

            vatAmount = vatValue * getTotal;

            txtVatAmount.Value = vatAmount.ToString("#,##0");

            decimal grandTotal = getTotal + vatAmount;
            txtGrandTotal.Value = grandTotal.ToString("#,##0");
            int getGrandTotal = Convert.ToInt32(grandTotal);
            hlbGrandTotal.Value = getGrandTotal.ToString();

            // Display the total value
            decimal value;
            Decimal.TryParse(total.ToString(), out value);
            txtTotalAmount.Value = value.ToString("#,##0");
        }

        #region PONumber
        protected void GetPONumber()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetPONumber");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("id", (string)dr["id"].ToString());
                Session.Add("module", (string)dr["module"].ToString());
                Session.Add("years", (int)dr["years"]);
                Session.Add("last_number", (int)dr["last_number"]);
            }
            else
            {

            }
        }

        protected void GetPONumberNew()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetPONumberNew");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("IDNew", (string)dr["IDNew"].ToString());
                Session.Add("moduleNew", (string)dr["moduleNew"].ToString());
                Session.Add("yearsNew", (int)dr["yearsNew"]);
                Session.Add("last_numberNew", (int)dr["last_numberNew"]);
            }
            else
            {

            }
        }

        protected void SaveNumbering(SqlConnection connection, SqlTransaction transaction)
        {
            string SP = "sp_PROCUREMENT_DB_PurchaseOrder";
            using (SqlCommand sqlcomm = new SqlCommand(SP, connection, transaction))
            {
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveNumbering");
                sqlcomm.Parameters.AddWithValue("@years", DateTime.Now.Year);
                sqlcomm.Parameters.AddWithValue("@id", Session["id"].ToString());
                sqlcomm.ExecuteNonQuery();
            }
        }

        protected void UpdateNumbering(SqlConnection connection, SqlTransaction transaction)
        {
            GetPONumber();
            hlblast_numberNew.Value = Session["last_number"].ToString();
            int _LastNumber = (Convert.ToInt32(hlblast_numberNew.Value) + 1);
            string SP = "sp_PROCUREMENT_DB_PurchaseOrder";
            using (SqlCommand sqlcomm = new SqlCommand(SP, connection, transaction))
            {
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
                sqlcomm.Parameters.AddWithValue("@id", Session["id"].ToString());
                sqlcomm.Parameters.AddWithValue("@last_number", _LastNumber);
                sqlcomm.ExecuteNonQuery();
            }
        }

        protected void UpdateNumberingNewYear()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
            sqlcomm.Parameters.AddWithValue("@id", Session["IDNew"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }
        #endregion

        #region SendEmail

        public class FileAttachment
        {
            [JsonProperty("@odata.type")]
            public string Type { get; set; }

            public string Name { get; set; }
            public string ContentBytes { get; set; }
        }

        #region SendToManagerIT
        private string PopulateBodySendToManagerIT(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderCreated_SendToITManager.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);

            return body;
        }

        private async Task SendEmailToManagerIT(string btn,string pono,string attcment,bool ispo,string headapprover)
        {
            string body = this.PopulateBodySendToManagerIT
            (headapprover/*"DUDY SETIADI"*/, /*txtPONumber.Value*/ pono, txtIssuedDate.Value, txtReqBy.Value, Session["fullname"].ToString(), "PO Created");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    if (/*hfAttachmentPath.Value*/attcment != "" || /*hfAttachmentPath.Value*/ attcment != null)
                    {
                        // Read HTML content from file
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", /*txtPONumber.Value*/pono);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        PurchaseOrederCreated.ProcessingMode = ProcessingMode.Local;
                        PurchaseOrederCreated.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                        PurchaseOrederCreated.LocalReport.EnableExternalImages = true;
                        PurchaseOrederCreated.LocalReport.DataSources.Clear();
                        PurchaseOrederCreated.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        PurchaseOrederCreated.LocalReport.Refresh();

                        string FileName = /*txtPONumber.Value.Trim() */ pono.Trim() + ".pdf";
                        string extension;
                        string encoding;
                        string mimeType;
                        string[] streams;
                        Warning[] warnings;
                        
                        Byte[] mybytes = PurchaseOrederCreated.LocalReport.Render("PDF", null,
                                      out extension, out encoding,
                                      out mimeType, out streams, out warnings);

                        using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                        {
                            fs.Write(mybytes, 0, mybytes.Length);
                        }

                        var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                        
                        var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                        var attachment = new FileAttachment
                        {
                            Type = "#microsoft.graph.fileAttachment",
                            Name = FileName,
                            ContentBytes = attachmentBase64
                        };

                     
                        List<FileAttachment> attachments1 = new List<FileAttachment>();

                        string FiletpathNew = Server.MapPath("~/eDocs_Files/PO/" + /*hfAttachmentPath.Value*/ attcment);

                        if (File.Exists(FiletpathNew))
                        {

                            byte[] attachBytes = File.ReadAllBytes(FiletpathNew);

                            string attachBase64 = Convert.ToBase64String(attachBytes);


                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = Path.GetFileName(FiletpathNew),
                                ContentBytes = attachBase64
                            };

                            attachments1.Add(attach);

                        }

                       
                        //foreach (string filePath in fileNames)
                        //{
                        //    // Extract the file name
                        //    string fileName = Path.GetFileName(filePath);

                        //    // Read the file into a byte array
                        //    byte[] attachBytes = File.ReadAllBytes(filePath);

                        //    // Convert the byte array to Base64
                        //    string attachBase64 = Convert.ToBase64String(attachBytes);

                        //    // Create the file attachment object
                        //    var attach = new FileAttachment
                        //    {
                        //        Type = "#microsoft.graph.fileAttachment",
                        //        Name = fileName,
                        //        ContentBytes = attachBase64
                        //    };

                        //    // Add the attachment to the list
                        //    attachments1.Add(attach);
                        //}
                        
                        // Create email content with HTML body
                        var emailBody = new
                        {
                            message = new
                            {
                                subject = "PURCHASE ORDER FORM : " + /*txtPONumber.Value*/ pono,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = body
                                },
                                //toRecipients = new[] { new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } }, new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } } },
                                toRecipients = new[] { new { emailAddress = new { address = _emailITMgr } } },
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } },
                                    new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } },
                                    new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } } },
                                //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                attachments = new[] { attachment }.Concat(attachments1).ToArray()
                            },
                            saveToSentItems = true
                        };

                        var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                        var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                        File.Delete(Server.MapPath("~/Prints/" + FileName));

                        // Send HTTP request to send email
                        var response = await httpClient.PostAsync(_endpoint, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle response
                        if (response.IsSuccessStatusCode)
                        {
                            // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);                                                  
                            if (btn == "satuan" && ispo == false)
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(lblRFNumber.Text)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                                return;

                            }
                            else if (btn == "satuan" && ispo == true)
                            {
                                string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                                // Register the script for partial postbacks
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            }


                        }
                        else
                        {
                            if (btn == "satuan")
                            {
                                Response.Write(responseContent.ToString());
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                                return;
                            }              

                        }
                    }
                    else
                    {
                        // Read HTML content from file
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", /*txtPONumber.Value*/pono);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        PurchaseOrederCreated.ProcessingMode = ProcessingMode.Local;
                        PurchaseOrederCreated.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                        PurchaseOrederCreated.LocalReport.EnableExternalImages = true;
                        PurchaseOrederCreated.LocalReport.DataSources.Clear();
                        PurchaseOrederCreated.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        PurchaseOrederCreated.LocalReport.Refresh();

                        string FileName = /*txtPONumber.Value.Trim()*/ attcment.Trim() + ".pdf";
                        string extension;
                        string encoding;
                        string mimeType;
                        string[] streams;
                        Warning[] warnings;
                        Byte[] mybytes = PurchaseOrederCreated.LocalReport.Render("PDF", null,
                                      out extension, out encoding,
                                      out mimeType, out streams, out warnings);
                        using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                        {
                            fs.Write(mybytes, 0, mybytes.Length);
                        }

                        var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                        var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                        var attachment = new FileAttachment
                        {
                            Type = "#microsoft.graph.fileAttachment",
                            Name = FileName,
                            ContentBytes = attachmentBase64
                        };


                        // Create email content with HTML body
                        var emailBody = new
                        {
                            message = new
                            {
                                subject = "PURCHASE ORDER FORM : " + /*txtPONumber.Value*/pono,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = body
                                },
                                //toRecipients = new[] { new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } }, new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } } },
                                toRecipients = new[] { new { emailAddress = new { address = _emailITMgr } } },
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } },
                                new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } } },
                                //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                attachments = new[] { attachment }
                            },
                            saveToSentItems = true
                        };

                        var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                        var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                        File.Delete(Server.MapPath("~/Prints/" + FileName));

                        // Send HTTP request to send email
                        var response = await httpClient.PostAsync(_endpoint, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle response
                        if (response.IsSuccessStatusCode)
                        {
                            // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

                            if (btn == "satuan" && ispo == false)
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(lblRFNumber.Text)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                                return;

                            }
                            else if (btn == "satuan" && ispo == true)
                            {
                                string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                                // Register the script for partial postbacks
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            }

                        }
                        else
                        {
                            if (btn == "satuan")
                            {

                                Response.Write(responseContent.ToString());
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                                return;

                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (btn == "satuan")
                {
                    Response.Write(ex.ToString());
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                    return; 
                }
                // Handle exceptions
               
            }
        }
        #endregion

        #region SendToManagerGA
        private string PopulateBodySendToManagerGA(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderCreated_SendToGAManager.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);

            return body;
        }

        private async Task SendEmailToManagerGA(string btn, string pono, string attcment, bool ispo, string headapprover)
        {
            string body = this.PopulateBodySendToManagerGA
            (/*"SURI ARBAD"*/headapprover, /*txtPONumber.Value*/ pono, txtIssuedDate.Value, txtReqBy.Value, Session["fullname"].ToString(), "PO Created");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    if (/*hfAttachmentPath.Value*/ attcment != "" || /*hfAttachmentPath.Value*/ attcment != null)
                    {
                        // Read HTML content from file
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", /*txtPONumber.Value*/ pono);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        PurchaseOrederCreated.ProcessingMode = ProcessingMode.Local;
                        PurchaseOrederCreated.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        PurchaseOrederCreated.LocalReport.EnableExternalImages = true;
                        PurchaseOrederCreated.LocalReport.DataSources.Clear();
                        PurchaseOrederCreated.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        PurchaseOrederCreated.LocalReport.Refresh();

                        string FileName = /*txtPONumber.Value.Trim()*/pono.Trim() + ".pdf";
                        string extension;
                        string encoding;
                        string mimeType;
                        string[] streams;
                        Warning[] warnings;
                        Byte[] mybytes = PurchaseOrederCreated.LocalReport.Render("PDF", null,
                                      out extension, out encoding,
                                      out mimeType, out streams, out warnings);
                        using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                        {
                            fs.Write(mybytes, 0, mybytes.Length);
                        }

                        var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                        var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                        var attachment = new FileAttachment
                        {
                            Type = "#microsoft.graph.fileAttachment",
                            Name = FileName,
                            ContentBytes = attachmentBase64
                        };

                        List<FileAttachment> attachments1 = new List<FileAttachment>();

                        string FiletpathNew = Server.MapPath("~/eDocs_Files/PO/" + /*hfAttachmentPath.Value*/ attcment);

                        if (File.Exists(FiletpathNew))
                        {

                            byte[] attachBytes = File.ReadAllBytes(FiletpathNew);

                            string attachBase64 = Convert.ToBase64String(attachBytes);


                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = Path.GetFileName(FiletpathNew),
                                ContentBytes = attachBase64
                            };

                            attachments1.Add(attach);

                        }
                        // Create email content with HTML body
                        var emailBody = new
                        {
                            message = new
                            {
                                subject = "PURCHASE ORDER FORM : " + /*txtPONumber.Value*/pono,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = body
                                },
                                toRecipients = new[] { new { emailAddress = new { address = _emailGAMgr } } },
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } },
                                new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } } },
                                //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                attachments = new[] { attachment }.Concat(attachments1).ToArray()

                            },
                            saveToSentItems = true
                        };

                        var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                        var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                        File.Delete(Server.MapPath("~/Prints/" + FileName));

                        // Send HTTP request to send email
                        var response = await httpClient.PostAsync(_endpoint, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle response
                        if (response.IsSuccessStatusCode)
                        {
                            // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);


                            if (btn == "satuan" && ispo == false)
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(lblRFNumber.Text)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                                return;
                            }
                            else if (btn == "satuan" && ispo == true)
                            {
                                string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                                // Register the script for partial postbacks
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            }

                        }
                        else
                        {
                            if (btn == "satuan")
                            {
                                Response.Write(responseContent.ToString());
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                                return;
                            }
                            
                        }
                    }
                    else
                    {
                        // Read HTML content from file
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", /*txtPONumber.Value*/pono);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        PurchaseOrederCreated.ProcessingMode = ProcessingMode.Local;
                        PurchaseOrederCreated.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        PurchaseOrederCreated.LocalReport.EnableExternalImages = true;
                        PurchaseOrederCreated.LocalReport.DataSources.Clear();
                        PurchaseOrederCreated.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        PurchaseOrederCreated.LocalReport.Refresh();

                        string FileName = /*txtPONumber.Value.Trim()*/ pono.Trim() + ".pdf";
                        string extension;
                        string encoding;
                        string mimeType;
                        string[] streams;
                        Warning[] warnings;
                        Byte[] mybytes = PurchaseOrederCreated.LocalReport.Render("PDF", null,
                                      out extension, out encoding,
                                      out mimeType, out streams, out warnings);
                        using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                        {
                            fs.Write(mybytes, 0, mybytes.Length);
                        }

                        var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                        var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                        var attachment = new FileAttachment
                        {
                            Type = "#microsoft.graph.fileAttachment",
                            Name = FileName,
                            ContentBytes = attachmentBase64
                        };

                        // Create email content with HTML body
                        var emailBody = new
                        {
                            message = new
                            {
                                subject = "PURCHASE ORDER FORM : " + /*txtPONumber.Value*/ pono,
                                body = new
                                {
                                    contentType = "HTML",
                                    content = body
                                },
                                toRecipients = new[] { new { emailAddress = new { address = _emailGAMgr } } },
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } ,
                                    new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } } },
                                //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                //ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                                attachments = new[] { attachment }
                            },
                            saveToSentItems = true
                        };

                        var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                        var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                        File.Delete(Server.MapPath("~/Prints/" + FileName));

                        // Send HTTP request to send email
                        var response = await httpClient.PostAsync(_endpoint, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Handle response
                        if (response.IsSuccessStatusCode)
                        {
                            // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

                            if (btn == "satuan" && ispo == false)
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(lblRFNumber.Text)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                                return;

                            }
                            else if (btn == "satuan" && ispo == true)
                            {
                                string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                                // Register the script for partial postbacks
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            }
                        }
                        else
                        {   
                            
                            if (btn == "satuan")
                            {
                                Response.Write(responseContent.ToString());
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                                return;
                            }
                           
                        }
                    }
                        
                }
            }
            catch (Exception ex)
            {
                if (btn == "satuan")
                {
                    // Handle exceptions
                    Response.Write(ex.ToString());
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                    return;
                }
            }
        }
        #endregion

        #endregion

        #region Barcode
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = txtPONumber.Value; // Nilai ID yang akan digunakan dalam URL

            // Membuat URL dengan parameter
            string data = $"{baseUrl}?po_no={Uri.EscapeDataString(id)}";

            // Membuat gambar QR code
            System.Drawing.Image barcodeImage = GenerateBarcodeImage(data);

            // Memuat gambar logo
            Bitmap logo = new Bitmap(Server.MapPath("~/images/logo_sayap_barcode.png"));

            // Menyesuaikan ukuran logo sesuai keinginan
            int logoWidth = barcodeImage.Width / 5; // Menyesuaikan ukuran logo menjadi 1/5 lebar dari QR code
            int logoHeight = barcodeImage.Height / 5; // Menyesuaikan ukuran logo menjadi 1/5 tinggi dari QR code
            Bitmap resizedLogo = new Bitmap(logo, new System.Drawing.Size(logoWidth, logoHeight));

            // Membuat bitmap baru untuk menyimpan gambar yang digabung
            Bitmap combinedImage = new Bitmap(barcodeImage.Width, barcodeImage.Height); // Ukuran gambar yang digabung sama dengan ukuran barcode

            // Menggambar barcode ke gambar yang digabung
            using (Graphics graphic = Graphics.FromImage(combinedImage))
            {
                graphic.DrawImage(barcodeImage, 0, 0);
            }

            // Menambahkan logo ke gambar yang digabung di tengah-tengah barcode
            int x = (combinedImage.Width - resizedLogo.Width) / 2;
            int y = (combinedImage.Height - resizedLogo.Height) / 2;
            using (Graphics graphic = Graphics.FromImage(combinedImage))
            {
                graphic.DrawImage(resizedLogo, new Point(x, y));
            }

            // Menyimpan gambar barcode ke folder
            string folderPath = Server.MapPath("~/Barcode/PO");
            SaveBarcodeImage(combinedImage, folderPath);

            // Menampilkan gambar barcode di halaman
            DisplayImageOnPage(combinedImage);
        }

        private void SaveBarcodeImage(System.Drawing.Image barcodeImage, string folderPath)
        {
            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save the barcode image to the folder
            string fileName = "barcode_po.png";  // You can customize the file name
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            barcodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        private System.Drawing.Image GenerateBarcodeImage(string data)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 600,
                Height = 600
            };

            Bitmap barcodeBitmap = barcodeWriter.Write(data);

            return barcodeBitmap;
        }

        private void DisplayImageOnPage(System.Drawing.Image barcodeImage)
        {
            // Save the barcode image to a MemoryStream
            using (MemoryStream stream = new MemoryStream())
            {
                barcodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                // Set the Image control's properties
                imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
            }
        }
        #endregion

        protected void TableDetailsRF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void TableDetailsRF_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string vendorName = DataBinder.Eval(e.Row.DataItem, "vendor_name")?.ToString();
                string idVendor = DataBinder.Eval(e.Row.DataItem, "id_vendor")?.ToString();

                e.Row.Attributes["data-vendor"] = vendorName;
                e.Row.Attributes["data-idvendor"] = idVendor;

            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Int32 qty;
            //    Int32 amount;
            //    Int32 price;

            //    price = Convert.ToInt32(e.Row.Cells[6].Text.ToString());
            //    qty = Convert.ToInt32(e.Row.Cells[4].Text.ToString());
            //    amount = price * qty;
            //    TableCell CellAmount = e.Row.Cells[10];
            //    CellAmount.Text = amount.ToString();
            //}
        }

        protected void TableDetailsRF_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            if (row.Cells[9].Text == null || row.Cells[9].Text == "" || row.Cells[9].Text == "&nbsp;")
            {
                divCreatePO.Visible = true;
                divItemPO.Visible = true;
                divListItemRF.Visible = false;
                hlbIDVendor.Value = row.Cells[8].Text;
                txtVendorName.Value = row.Cells[7].Text;
                txtReqBy.Value = row.Cells[13].Text; //+" "+ "(" + row.Cells[14].Text + ")";
                txtReqDept.Value = row.Cells[14].Text;
                txtOIDReqDept.Value = row.Cells[15].Text;
                DateTime currentDateTime = DateTime.Now;
                txtIssuedDate.Value = currentDateTime.ToString();

                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
                sqlcomm.CommandType = CommandType.StoredProcedure;

                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailVendors");
                sqlcomm.Parameters.AddWithValue("@id", row.Cells[8].Text);

                SqlDataReader dr = null;
                dr = sqlcomm.ExecuteReader();

                if (dr.Read())
                {
                    Session.Add("id", (string)dr["id"].ToString());
                    Session.Add("id_category", (string)dr["id_category"].ToString());
                    Session.Add("Category", (string)dr["Category"]);
                    Session.Add("vendor_name", (string)dr["vendor_name"]);
                    Session.Add("sales_pic_name", (string)dr["sales_pic_name"]);
                    Session.Add("sales_phone_number", (string)dr["sales_phone_number"]);
                    Session.Add("sales_email", (string)dr["sales_email"]);
                    Session.Add("invoice_pic_name", (string)dr["invoice_pic_name"].ToString());
                    Session.Add("invoice_phone_number", (string)dr["invoice_phone_number"].ToString());
                    Session.Add("invoice_email", (string)dr["invoice_email"]);
                    Session.Add("address", (string)dr["address"]);
                    Session.Add("t_o_p", (string)dr["t_o_p"]);
                    Session.Add("pkp_nonpkp", (string)dr["pkp_nonpkp"]);
                }
                else
                {

                }

                if (Session["pkp_nonpkp"].ToString() == "PKP")
                {
                    txtVAT.Text = "11";
                }
                else
                {
                    txtVAT.Text = "0";
                }
                txtAddress.Value = Session["address"].ToString();
                txtPaymentTerms.Value = Session["t_o_p"].ToString() + " " + "days after invoice received";
                //txtDeliveryTo.Value = "PT. Yusen Logistics Indonesia\r\nTemas Building Lantai 3A\r\nJl. Yos Sudarso Kav.33,Sunter Jaya\r\nJakarta Utara 14350, Indonesia";
                
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "AlreadyPO();", true);
            }

        }




        protected void CheckUploadDocument()
        {
            if (FileUploadEDocs.HasFiles)
            {
                int filecount = 0;
                int fileuploadcount = 0;
                lbErrorUploadNotif.InnerText = ""; // Clear previous messages

                // Check the number of selected files
                filecount = FileUploadEDocs.PostedFiles.Count();
                string[] allowedExtensions = { ".pdf" };

                if (filecount <= 5)
                {
                    foreach (HttpPostedFile postfiles in FileUploadEDocs.PostedFiles)
                    {
                        // Get the File Extension
                        string filetype = Path.GetExtension(postfiles.FileName);

                        if (IsValidFileType(filetype, allowedExtensions))
                        {
                            // Get the File Size In Byte
                            double filesize = postfiles.ContentLength;

                            if (filesize < (5242880))
                            {
                                fileuploadcount++;
                                string serverfolder = string.Empty;
                                string serverpath = string.Empty;

                                // Adding File Into Specific Folder Depend On his Extension
                                switch (filetype.ToLower())
                                {
                                    case ".pdf":
                                        string _vPONumber = txtPONumber.Value;
                                        serverfolder = Server.MapPath("~/eDocs_Files/PO" + "/" + _vPONumber + "/");

                                        // Ensure the folder is available
                                        if (!Directory.Exists(serverfolder))
                                        {
                                            // Create the folder
                                            Directory.CreateDirectory(serverfolder);
                                        }

                                        // Append a unique identifier to avoid overwriting
                                        string uniqueFileName = Path.GetFileNameWithoutExtension(postfiles.FileName)
                                            + Path.GetExtension(postfiles.FileName);

                                        serverpath = Path.Combine(serverfolder, uniqueFileName);
                                        postfiles.SaveAs(serverpath);
                                        lbErrorUploadNotif.InnerText += "[" + uniqueFileName + "]- " + filetype + " file uploaded successfully";
                                        hlbOK.Value = "OK";
                                        break;
                                }
                            }
                            else
                            {
                                lbErrorUploadNotif.InnerText += "[" + postfiles.FileName + "]- File not uploaded; size is greater than 5MB. Your File Size is " + (filesize / (1024 * 1034)) + " MB";
                               //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMaxSize();", true);
                            }
                        }
                        else
                        {
                            lbErrorUploadNotif.InnerText += "[" + postfiles.FileName + "]- Invalid file type. Allowed types are .pdf";
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMustBe();", true);
                        }
                    }
                }
                else
                {
                    lbErrorUploadNotif.InnerText = "You have selected " + filecount + " files. Please select a maximum of 5 files." ;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMaximumUpload();", true);
                }
            }
            else
            {
                lbErrorUploadNotif.InnerText = "No Files Upload.";
            }
        }

        // Function to check if the file type is valid

        private bool IsValidFileType(string fileType, string[] allowedExtensions)
        {
            return Array.Exists(allowedExtensions, ext => ext.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }

        protected void ddlDeliveryTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}