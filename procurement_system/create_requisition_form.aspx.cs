using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using DocumentFormat.OpenXml.Vml;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using ZXing;
using System.Drawing;
using DocumentFormat.OpenXml.Vml.Office;

namespace procurement_system
{
    public partial class create_requisition_form : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];

        protected void Page_Load(object sender, EventArgs e)
        {
            txtReqDate.Value = DateTime.Now.ToString();
            lblNamaBranch.Value = Session["Location"].ToString();
            hblNIK.Value = Session["nik"].ToString();
            txtRequester.Value = Session["fullname"].ToString();
            txtPosition.Value = Session["Position"].ToString();
            txtSection.Value = Session["Section"].ToString();
            hlbEmailRequester.Value = Session["email_karyawan"].ToString();

            

            if (!IsPostBack)
            {
                GridTemporary();

                if (Session["Section"].ToString().ToUpper() == "95ED03F4-2420-4FCB-9D22-443787E5BF40" || Session["Section"].ToString().ToUpper() == "52591B16-4E97-4F3B-A48F-4807936E1052"
                    || Session["Section"].ToString().ToUpper() == "0AEE271E-A132-4A2F-BC46-AAD99BBE7519" || Session["Section"].ToString().ToUpper() == "DCDA04FD-4920-4F6C-BAF8-77B377DE2FFF"
                    || Session["Section"].ToString().ToUpper() == "E6A8EF10-5025-44C0-9CF1-7AB81CA4F523" || Session["Section"].ToString().ToUpper() == "3BEAD7B1-A9D4-4557-976F-DA2C6B489910"
                    || Session["Section"].ToString().ToUpper() == "0BF510DE-9348-418C-9E26-735243E8C05F" || Session["Section"].ToString().ToUpper() == "09F99302-B5CA-468A-9508-F2F032DC090D")
                {
                    ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("OPS"));
                    ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("IT"));
                    GetApprover();
                    //txtCatalogType.Value = "GA";
                    
                }
                else if (Session["Section"].ToString().ToUpper() == "9AF484E4-9DA8-4CB7-9537-8DEE9B935182")
                {
                    ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("GA"));
                    ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("OPS"));
                    GetApprover();
                    //txtCatalogType.Value = "IT";
                    
                }
                else
                {
                    if (Session["Division"].ToString().ToUpper() == "C999F3FD-F604-40A3-A300-A3B63FCF8B68" || Session["Division"].ToString().ToUpper() == "B094D3EB-0DEC-4066-8CB9-AB118F2B81D0"
                        || Session["Division"].ToString().ToUpper() == "63900462-5119-42D4-98DB-B2728A34868A")
                    {
                        GetApproverSUBSRG();
                        //txtCatalogType.Value = "OPS";
                    }
                    else
                    {
                        GetApprover();
                        //txtCatalogType.Value = "OPS";
                        ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("GA"));
                        ddlCatalogType.Items.Remove(ddlCatalogType.Items.FindByText("IT"));
                        
                    }
                    
                }
            }

        }

        

        protected void GetItems()
        {
            ddlItem.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddItemName");
            sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value);
            sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalogType.SelectedItem.Text);

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Code-Category-Item-Merk-Type>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlItem.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["item_code"].ToString() + "-" + dr["Category"].ToString() + "-" + dr["Item"].ToString() + "-" + dr["ItemMerk"].ToString() + "-" + dr["tipe"].ToString();
                    newItem.Value = dr["stok_code"].ToString();
                    ddlItem.Items.Add(newItem);
                }
                dr.Close();
            }
            catch (Exception err)
            {
                //TODO
            }
            finally
            {
                Con.Close();
            }
        }

        protected void GetItemsSUBSRG()
        {
            ddlItem.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddItemNameSUBSRG");
            sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value);

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Code-Category-Item-Merk-Type>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlItem.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["item_code"].ToString() + "-" + dr["Category"].ToString() + "-" + dr["Item"].ToString() + "-" + dr["ItemMerk"].ToString() + "-" + dr["tipe"].ToString();
                    newItem.Value = dr["stok_code"].ToString();
                    ddlItem.Items.Add(newItem);
                }
                dr.Close();
            }
            catch (Exception err)
            {
                //TODO
            }
            finally
            {
                Con.Close();
            }
        }

        protected void GetDetailApprover()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_IT_STOCK_Employees";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailPurchaseApprover");
            sqlcomm.Parameters.AddWithValue("@nik", ddlApprover.SelectedValue);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("FulnameApprover", (string)dr["FulnameApprover"]);
                Session.Add("NikApprover", (string)dr["NikApprover"]);
                Session.Add("SectionApprover", (string)dr["SectionApprover"]);
                Session.Add("BranchApprover", (string)dr["BranchApprover"]);
                Session.Add("EmailApprover", (string)dr["EmailApprover"]);
            }
            else
            {

            }
        }

        protected void GetApprover()
        {
            ddlApprover.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPurchaseApprover");
            sqlcomm.Parameters.AddWithValue("@id_section", Session["Section"].ToString().ToUpper());

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "Select Approver";
                newItem.Value = "0";
                ddlApprover.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["FulnameApprover"].ToString();
                    newItem.Value = dr["NikApprover"].ToString();
                    ddlApprover.Items.Add(newItem);
                }
                dr.Close();
            }
            catch (Exception err)
            {
                string _ErrorMsg = err.Message;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void GetApproverSUBSRG()
        {
            ddlApprover.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPurchaseApproverSUBSRG");
            sqlcomm.Parameters.AddWithValue("@id_division", Session["Division"].ToString().ToUpper());

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "Select Approver";
                newItem.Value = "0";
                ddlApprover.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["FulnameApprover"].ToString();
                    newItem.Value = dr["NikApprover"].ToString();
                    ddlApprover.Items.Add(newItem);
                }
                dr.Close();
                newItem = new ListItem();
                newItem.Text = "SURI ARBAD";
                newItem.Value = "891048";
                ddlApprover.Items.Add(newItem);
            }
            catch (Exception err)
            {
                string _ErrorMsg = err.Message;
            }
            finally
            {
                Con.Close();
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedItem.Text=="")
            {
                Response.Redirect("create_requisition_form.aspx");
            }
            else
            {
                GetDetailItems();
                hlbCodeItem.Value = Session["item_code"].ToString();
                hlbItem.Value = Session["Item"].ToString();
                hlbMerk.Value = Session["ItemMerk"].ToString();
                hlbType.Value = Session["tipe"].ToString();
            }
        }

        protected void ddlReqType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlApprover_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprover.SelectedItem.Text == "")
            {
                Response.Redirect("create_requisition_form.aspx");
            }
            else
            {
                GetDetailApprover();
                hlbNameApprover.Value = Session["FulnameApprover"].ToString();
                hlbNIKApprover.Value = Session["NikApprover"].ToString();
                hlbSectionApprover.Value = Session["SectionApprover"].ToString();
                hlbBranchApprover.Value = Session["BranchApprover"].ToString();
                hlbEmailApprover.Value = Session["EmailApprover"].ToString();
            }
        }

       

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            if (ddlItem.SelectedItem.Text == "<Code-Category-Item-Merk-Type>")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectItem();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed, Please select item!');", true);
            }
            else if (ddlReqType.SelectedItem.Text == "Select Request Type")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectReqType();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed, Please select request type!');", true);
            }
            else if (ddlApprover.SelectedItem.Text == "Select Approver")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectApprover();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed, Please select approver!');", true);
            }
            else
            {
                string code_stok = ddlItem.SelectedValue;
                string code_item = hlbCodeItem.Value;
                string item = hlbItem.Value;
                string merk = hlbMerk.Value;
                string description = txtDescription.Value;
                string qty = txtJumlahBeli.Value;
                string reqdate = txtReqDate.Value;
                string remarks = txtRemaks.Value;

                DataTable dt;

                if (ViewState["TempData"] != null)
                {
                    // Retrieve existing data from ViewState
                    dt = (DataTable)ViewState["TempData"];
                }
                else
                {
                    // Create a new DataTable if no data exists
                    dt = new DataTable();
                    dt.Columns.Add("stok_code", typeof(string));
                    dt.Columns.Add("kode_barang", typeof(string));
                    dt.Columns.Add("nama_barang", typeof(string));
                    dt.Columns.Add("merk", typeof(string));
                    dt.Columns.Add("description", typeof(string));
                    dt.Columns.Add("jumlah_beli", typeof(string));
                    dt.Columns.Add("tanggal_beli", typeof(string));
                    dt.Columns.Add("remaks", typeof(string));
                }


                // Create a new DataRow and populate it with data
                DataRow newRow = dt.NewRow(); 
                newRow["stok_code"] = code_stok;
                newRow["kode_barang"] = code_item;
                newRow["nama_barang"] = item;
                newRow["merk"] = merk;
                newRow["description"] = description;
                newRow["jumlah_beli"] = qty;
                newRow["tanggal_beli"] = reqdate;
                newRow["remaks"] = remarks;

                // Add the new DataRow to the DataTable
                dt.Rows.Add(newRow);

                // Store the updated DataTable in ViewState
                ViewState["TempData"] = dt;

                // Bind the DataTable to the GridView
                TableItemPurchase.DataSource = dt;
                TableItemPurchase.DataBind();

                GetItems();

                txtJumlahBeli.Value = "0";
                txtDescription.Value = "-";
                txtRemaks.Value = "-";
                ddlApprover.Enabled = false;
                ddlReqType.Enabled = false;
                //ddlGMApprover.Enabled = false;
                divSubmit.Visible = true;
                //ddlCatalogType.Enabled = false;
            }

            
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            DataTable dt = (DataTable)ViewState["TempData"];
            dt.Rows.RemoveAt(row.RowIndex);

            TableItemPurchase.DataSource = dt;
            TableItemPurchase.DataBind();
        }

        protected void ddlCatalogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetItems();
        }

        #region Tables
        protected void GetDetailItems()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "DetailItemNameToRF");
            sqlcomm.Parameters.AddWithValue("@stok_code", ddlItem.SelectedValue);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("id", (string)dr["id"].ToString());
                Session.Add("id_category", (string)dr["id_category"].ToString());
                Session.Add("Category", (string)dr["Category"].ToString());
                Session.Add("item_code", (string)dr["item_code"].ToString());
                Session.Add("id_item", (string)dr["id_item"].ToString());
                Session.Add("Item", (string)dr["Item"].ToString());
                Session.Add("id_merk", (string)dr["id_merk"].ToString());
                Session.Add("ItemMerk", (string)dr["ItemMerk"].ToString());
                Session.Add("id_unit", (string)dr["id_unit"].ToString());
                Session.Add("Unit", (string)dr["Unit"].ToString());
                Session.Add("tipe", (string)dr["tipe"].ToString());
                Session.Add("criteria_stock", (string)dr["criteria_stock"].ToString());
                Session.Add("active", (string)dr["active"].ToString());
                Session.Add("MinStock", (string)dr["MinStock"].ToString());
                Session.Add("nama_branch_detail", (string)dr["nama_branch_detail"].ToString());
                Session.Add("stok_code", (string)dr["stok_code"].ToString());
            }
            else
            {

            }
        }

        protected void GridTemporary()
        {
            DataTable dt = (DataTable)ViewState["TempData"];
            TableItemPurchase.DataSource = dt;
            TableItemPurchase.DataBind();
            //TableItemPO.Columns[9].Visible = false;
        }

        protected void TableItemPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableItemPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableItemPurchase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region OLDEmail
        //private void SendHtmlFormattedEmailToPurchasingCheckEstimatePrice(string recepientEmail, string subject, string body)
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());

        //    sqlcomm.Connection = Con;
        //    DataTable dtb = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

        //    sda.Fill(dtb);
        //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
        //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
        //    ReportViewerPurchase.LocalReport.DataSources.Clear();
        //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
        //    ReportViewerPurchase.LocalReport.Refresh();

        //    string FileName = txtNota.Value + ".pdf";
        //    string extension;
        //    string encoding;
        //    string mimeType;
        //    string[] streams;
        //    Warning[] warnings;
        //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
        //                  out extension, out encoding,
        //                  out mimeType, out streams, out warnings);
        //    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
        //    {
        //        fs.Write(mybytes, 0, mybytes.Length);
        //    }
        //    using (MailMessage mm = new MailMessage(_SMTPHost, "widhi.kusuma@id.yusen-logistics.com" /*"sardi.evelina@id.yusen-logistics.com"*/))
        //    {
        //        //mm.CC.Add(new MailAddress(hlbEmailRequester.Value));
        //        //mm.CC.Add(new MailAddress("rizal.syahputra@id.yusen-logistics.com"));
        //        //mm.CC.Add(new MailAddress("ylid.ml.it@id.yusen-logistics.com"));
        //        mm.Subject = "REQUESITION FORM : " + txtNota.Value;
        //        mm.Attachments.Add(new Attachment(Server.MapPath("~/Prints/" + FileName)));
        //        mm.Body = body;
        //        mm.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = _SMTPServer;
        //        NetworkCredential credential = new NetworkCredential();
        //        credential.UserName = _SMTPCredential;
        //        credential.Password = _SMTPPassword;
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = credential;
        //        smtp.Port = _SMTPPort;
        //        smtp.EnableSsl = true;
        //        smtp.Send(mm);
        //    }
        //    File.Delete(Server.MapPath("~/Prints/" + FileName));
        //}
        #endregion

        #region SendEmail
        public class FileAttachment
        {
            [JsonProperty("@odata.type")]
            public string Type { get; set; }

            public string Name { get; set; }
            public string ContentBytes { get; set; }
        }

        private string PopulateBodyToPurchasingCheckEstimatePrice(string reqby, string reqno, string approver_status, string reqdate, string PurchaseTeam)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTempleteRequestPurchase.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ReqBy}", reqby);
            body = body.Replace("{ReqNo}", reqno);
            body = body.Replace("{ApprovalStatus}", approver_status);
            body = body.Replace("{PurchaseTeam}", PurchaseTeam);
            body = body.Replace("{ReqDate}", reqdate);
            return body;
        }

        private async Task SendEmailToPurchasingCheckEstimatePrice()
        {
            string body = this.PopulateBodyToPurchasingCheckEstimatePrice
            (txtRequester.Value, txtNota.Value, "NOT YET", txtReqDate.Value, "Purchasing Teams");

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

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = txtNota.Value.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
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
                            subject = "REQUESITION FORM : " + txtNota.Value,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = hlbEmailRequester.Value } } },
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
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_form.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region RFNumber
        protected void GetRFNumber()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetRFNumber");

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

        protected void GetRFNumberNew()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetRFNumberNew");

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

        protected void SaveNumbering()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "SaveNumbering");
            sqlcomm.Parameters.AddWithValue("@years", DateTime.Now.Year);
            sqlcomm.Parameters.AddWithValue("@id", Session["id"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected void UpdateNumbering()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
            sqlcomm.Parameters.AddWithValue("@id", Session["id"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected void UpdateNumberingNewYear()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
            sqlcomm.Parameters.AddWithValue("@id", Session["IDNew"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }
        #endregion

        #region Submit
        protected void SaveMasterPurchase()
        {
            if (ddlCatalogType.SelectedItem.Text == "GA" || ddlCatalogType.SelectedItem.Text == "OPS")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchase");
                sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@nik_approver", ddlApprover.SelectedValue);
                //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", ddlGMApprover.SelectedValue);
                sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");
                sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalogType.SelectedItem.Text);

                sqlcomm.ExecuteNonQuery();
                Con.Close();
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "SaveMasterPurchaseIT");
                sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@nik_approver", ddlApprover.SelectedValue);
                //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", ddlGMApprover.SelectedValue);
                sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");
                //sqlcomm.Parameters.AddWithValue("@nik_it_manager", "880123");
                sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalogType.SelectedItem.Text);

                sqlcomm.ExecuteNonQuery();
                Con.Close();
            }

        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            GetRFNumber();
            var CurentYear = DateTime.Now.Year;

            if (CurentYear != (int)Session["years"])
            {
                SaveNumbering();
                GetRFNumberNew();
                hlbYearsNew.Value = Session["yearsNew"].ToString();
                hlblast_numberNew.Value = Session["last_numberNew"].ToString();
                int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
                int _getNumberUrut = _LastNumber + 1;

                if (_getNumberUrut < 10)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "000" + _getNumberUrut;
                }
                else if (_getNumberUrut > 9 && _getNumberUrut < 99)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "00" + _getNumberUrut;
                }
                else if (_getNumberUrut > 99 && _getNumberUrut < 999)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "0" + _getNumberUrut;
                }
                else if (_getNumberUrut > 999)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + _getNumberUrut;
                }

                SaveMasterPurchase();
                foreach (GridViewRow row in TableItemPurchase.Rows)
                {
                    if (ddlCatalogType.SelectedItem.Text == "GA" || ddlCatalogType.SelectedItem.Text == "OPS")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchase");
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[2].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@quantity", row.Cells[6].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@remaks", row.Cells[8].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                        sqlcomm.Parameters.AddWithValue("@type_request", ddlReqType.SelectedItem.Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                        sqlcomm.Parameters.AddWithValue("@description", row.Cells[5].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", hlbNIKApprover.Value);
                        sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                        //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", "890556");
                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[1].Text.ToString());
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_form.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseIT");
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[2].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@quantity", row.Cells[6].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@remaks", row.Cells[8].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                        sqlcomm.Parameters.AddWithValue("@type_request", ddlReqType.SelectedItem.Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                        sqlcomm.Parameters.AddWithValue("@description", row.Cells[5].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", hlbNIKApprover.Value);
                        sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                        //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", "890556");
                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[1].Text.ToString());
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");
                        //sqlcomm.Parameters.AddWithValue("@nik_it_manager", "880123");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_form.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                        sqlcomm.Dispose();
                        Con.Close();
                        Con.Dispose();
                    }
                }
                await SendEmailToPurchasingCheckEstimatePrice();
                UpdateNumberingNewYear();
            }
            else
            {
                UpdateNumbering();
                GetRFNumberNew();
                hlblast_numberNew.Value = Session["last_numberNew"].ToString();
                int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
                if (_LastNumber < 10)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "000" + _LastNumber;
                }
                else if (_LastNumber > 9 && _LastNumber < 99)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "00" + _LastNumber;
                }
                else if (_LastNumber > 99 && _LastNumber < 999)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + "0" + _LastNumber;
                }
                else if (_LastNumber > 999)
                {
                    txtNota.Value = "YLID-RF-" + CurentYear + "-" + _LastNumber;
                }

                SaveMasterPurchase();
                foreach (GridViewRow row in TableItemPurchase.Rows)
                {
                    if (ddlCatalogType.SelectedItem.Text == "GA" || ddlCatalogType.SelectedItem.Text == "OPS")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchase");
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[2].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@quantity", row.Cells[6].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@remaks", row.Cells[8].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                        sqlcomm.Parameters.AddWithValue("@type_request", ddlReqType.SelectedItem.Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                        sqlcomm.Parameters.AddWithValue("@description", row.Cells[5].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", hlbNIKApprover.Value);
                        sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                        //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", "890556");
                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[1].Text.ToString());
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_form.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveDetailPurchaseIT");
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtNota.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_requester", hblNIK.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[2].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@quantity", row.Cells[6].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@request_date", txtReqDate.Value.Trim());
                        sqlcomm.Parameters.AddWithValue("@remaks", row.Cells[8].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status", "Not Complete");
                        sqlcomm.Parameters.AddWithValue("@type_request", ddlReqType.SelectedItem.Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@status_approve", "NOT YET");
                        sqlcomm.Parameters.AddWithValue("@description", row.Cells[5].Text.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", hlbNIKApprover.Value);
                        sqlcomm.Parameters.AddWithValue("@nama_branch", lblNamaBranch.Value.Trim());
                        //sqlcomm.Parameters.AddWithValue("@nik_gm_approver", "890556");
                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[1].Text.ToString());
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_manager", "891048");
                        //sqlcomm.Parameters.AddWithValue("@nik_adm_gm", "890556");
                        //sqlcomm.Parameters.AddWithValue("@nik_it_manager", "880123");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_form.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                        sqlcomm.Dispose();
                        Con.Close();
                        Con.Dispose();
                    }
                }
                await SendEmailToPurchasingCheckEstimatePrice();
            }

        }
        #endregion

        #region Barcode
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = txtNota.Value; // Nilai ID yang akan digunakan dalam URL

            // Membuat URL dengan parameter
            string data = $"{baseUrl}?rf_no={Uri.EscapeDataString(id)}";

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
            string folderPath = Server.MapPath("~/Barcode/RF");
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
            string fileName = "barcode_rf.png";  // You can customize the file name
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

    }
}