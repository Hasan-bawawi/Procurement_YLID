using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using System.Drawing;
using ZXing;

namespace procurement_system
{
    public partial class input_price : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["nik"])))
            {
                Response.Redirect("login.aspx?url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }

            if (Session["GroupName"].ToString() == "Admin Purchasing")
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
                            Session.Add("catalog_type", (string)rdr["catalog_type"]);
                            Session.Add("rf_no", (string)rdr["rf_no"]);
                            Session.Add("id", (string)rdr["id"].ToString());
                            Session.Add("Requester", (string)rdr["Requester"]);
                            Session.Add("ManagerApprove", (string)rdr["ManagerApprove"]);
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
                            Session.Add("DivisionRequester", (string)rdr["DivisionRequester"].ToString());
                            Session.Add("SectionRequester", (string)rdr["SectionRequester"]);
                            Session.Add("nik_requester", (string)rdr["nik_requester"]);
                            Session.Add("EmailRequester", (string)rdr["EmailRequester"]);
                            //Session.Add("AdmManagerApprove", (string)rdr["AdmManagerApprove"]);
                            //Session.Add("EmailAdmManagerApprove", (string)rdr["EmailAdmManagerApprove"]);
                            //Session.Add("AdmGMApprove", (string)rdr["AdmGMApprove"]);
                            //Session.Add("EmailAdmGMApprove", (string)rdr["EmailAdmGMApprove"]);
                            Session.Add("EmailManagerApprove", (string)rdr["EmailManagerApprove"]);
                            Session.Add("DivisionReq", (string)rdr["DivisionReq"].ToString());
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
                lbDivisionReq.Text = Session["DivisionReq"].ToString();
                lbSection.Text = Session["SectionRequester"].ToString();
                lbRequester.Text = Session["Requester"].ToString();
                lbApprovedBy.Value = Session["ManagerApprove"].ToString();
                //lbAcknowledgeBy.Text = Session["GMApprove"].ToString();
                lbLocation.Text = Session["nama_branch"].ToString();
                hlbEmailRequester.Value = Session["EmailRequester"].ToString();
                //hlbEmailAdmGM.Value = Session["EmailAdmGMApprove"].ToString();
                //hlbAdmManager.Value = Session["AdmManagerApprove"].ToString();
                //hlbEmailAdmManager.Value = Session["EmailAdmManagerApprove"].ToString();
                hlbEmailMgrApprover.Value = Session["EmailManagerApprove"].ToString();

                if (!IsPostBack)
                {
                    BindDataTableItemRF();
                }
            }
            else
            {
                Response.Write("<script>alert('Access Denied!!, Purchasing Team Only!'),window.location.href = 'login.aspx';</script>");
            }
            
            
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (GridViewRow grow in TableItemPurchase.Rows)
            {
                Int32 qty;
                Int32 amount;
                HtmlInputText price = (HtmlInputText)grow.FindControl("txtPrice");
                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                int getprice = Convert.ToInt32(parsedValue);

                qty = Convert.ToInt32(grow.Cells[7].Text.ToString());
                amount = getprice * qty;

                int getAmount = Convert.ToInt32(amount);
                total += getAmount;
                price.Disabled = true;
            }
            decimal value;
            value = Convert.ToDecimal(total);
            txtGrandTotal.Value = value.ToString("#,##0");
            
            divSubmit.Visible = true;
            divCheck.Visible = false;
            divClearPrice.Visible = true;
        }

        #region Tables
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

            //TableItemPurchase.Columns[9].Visible = false;
            TableItemPurchase.Columns[1].Visible = false;

            TableItemPurchase.UseAccessibleHeader = true;
            TableItemPurchase.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();

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

        #region SendEmail
        public class FileAttachment
        {
            [JsonProperty("@odata.type")]
            public string Type { get; set; }

            public string Name { get; set; }
            public string ContentBytes { get; set; }
        }

        private string PopulateBodyEmailSendToManagerDivision(string reqby, string reqno, string approver_status, string reqdate, string ManagerDiv)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTempleteRFSendToManagerDivision.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ReqBy}", reqby);
            body = body.Replace("{ReqNo}", reqno);
            body = body.Replace("{ApprovalStatus}", approver_status);
            body = body.Replace("{ManagerDivision}", ManagerDiv);
            body = body.Replace("{ReqDate}", reqdate);
            return body;
        }

        private async Task SendEmailSendToManagerDivision()
        {
            string body = this.PopulateBodyEmailSendToManagerDivision
            (lbRequester.Text, lbRFNumberBreadcrumb.Text, "Price Checked", lbRequestDate.Text, lbApprovedBy.Value);

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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_PriceEstimate_Under1Juta_SUBSRG";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPriceEstimated_Under1Juta_SUB_SRG.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbRFNumberBreadcrumb.Text + ".pdf";
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
                            subject = "REQUESITION FORM : " + lbRFNumberBreadcrumb.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = hlbEmailMgrApprover.Value } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } },
                                new { emailAddress = new { address = hlbEmailRequester.Value } } },
                            //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
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
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
            }
        }
        #endregion

        #region OLDEmail
        //private void SendHtmlFormattedEmailSendToManagerDivision(string recepientEmail, string subject, string body)
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_PriceEstimate_Under1Juta_SUBSRG";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());

        //    sqlcomm.Connection = Con;
        //    DataTable dtb = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

        //    sda.Fill(dtb);
        //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
        //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPriceEstimated_Under1Juta_SUB_SRG.rdlc");
        //    ReportViewerPurchase.LocalReport.DataSources.Clear();
        //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
        //    ReportViewerPurchase.LocalReport.Refresh();

        //    string FileName = lbRFNumberBreadcrumb.Text + ".pdf";
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
        //    using (MailMessage mm = new MailMessage(_SMTPHost, "widhi.kusuma@id.yusen-logistics.com" /*hlbEmailMgrApprover.Value*/))
        //    {
        //        //mm.CC.Add(new MailAddress(hlbEmailRequester.Value));
        //        //mm.CC.Add(new MailAddress("sardi.evelina@id.yusen-logistics.com"));
        //        //mm.CC.Add(new MailAddress("rizal.syahputra@id.yusen-logistics.com"));
        //        //mm.CC.Add(new MailAddress("ylid.ml.it@id.yusen-logistics.com"));
        //        mm.Subject = "REQUESITION FORM : " + lbRFNumberBreadcrumb.Text;
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

        //protected void SendEmailSendToManagerDivision()
        //{
        //    string body = this.PopulateBodyEmailSendToManagerDivision
        //    (lbRequester.Text, lbRFNumberBreadcrumb.Text, "Price Checked", lbRequestDate.Text, lbApprovedBy.Value);
        //    this.SendHtmlFormattedEmailSendToManagerDivision("widhitech@gmail.com", "New article published!", body);
        //}
        #endregion

        #region Submit
        protected void submitrow(int getprice, string item_code)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdatePriceRF");
            sqlcomm.Parameters.AddWithValue("@price", getprice);
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);
            sqlcomm.Parameters.AddWithValue("@item_code", item_code);

            sqlcomm.ExecuteNonQuery();

            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void UpdatePriceRF()
        {
            foreach (GridViewRow grow in TableItemPurchase.Rows)
            {
                HtmlInputText price = (HtmlInputText)grow.FindControl("txtPrice");
                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                int getprice = Convert.ToInt32(parsedValue);
                string item_code = grow.Cells[3].Text;
                submitrow(getprice, item_code);
            }
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            divSubmit.Visible = false;
            divCheck.Visible = true;

            if (lbLocation.Text.ToUpper() == "YLID-SUB" || lbLocation.Text.ToUpper() == "YLID-SRG")
            {
                decimal parsedValue = decimal.Parse(txtGrandTotal.Value, NumberStyles.Currency);
                int getGrandTotal = Convert.ToInt32(parsedValue);
                if (getGrandTotal <= 1000000)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Connection = Con;
                    sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under1Juta_SUB_SRG");
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                    sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                    sqlcomm.ExecuteNonQuery();
                    Con.Close();
                    UpdatePriceRF();
                    await SendEmailSendToManagerDivision();
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                    string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                    // Register the script for partial postbacks
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                }
                else if (getGrandTotal >= 1000000 && getGrandTotal <= 5000000)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Connection = Con;
                    sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper1Juta_SUB_SRG");
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                    sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                    sqlcomm.ExecuteNonQuery();
                    Con.Close();
                    UpdatePriceRF();
                    await SendEmailSendToManagerDivision();
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                    string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                    // Register the script for partial postbacks
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
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
                    sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_SUB_SRG");
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                    sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                    sqlcomm.ExecuteNonQuery();
                    Con.Close();
                    UpdatePriceRF();
                    await SendEmailSendToManagerDivision();
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                    string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                    // Register the script for partial postbacks
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                }
            }
            else
            {
                decimal parsedValue = decimal.Parse(txtGrandTotal.Value, NumberStyles.Currency);
                int getGrandTotal = Convert.ToInt32(parsedValue);
                if (getGrandTotal <= 5000000)
                {
                    if (lbDivision.Text.ToUpper() == "5D2F0CA6-961D-4C09-9EEE-978EEE6309B8")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under5Juta_ADMINISTRATION");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "6471E2F5-8BBF-4EEC-991F-4B2F7A56A8E5" || lbDivision.Text.ToUpper() == "C42F75D0-F7AD-42B2-ACAD-5200199B813D" || lbDivision.Text.ToUpper() == "825B4273-6E9A-456A-81D5-8B710F50598E" || lbDivision.Text.ToUpper() == "238217DC-2872-4A00-8EBB-1F41A951D363")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under5Juta_OFF");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "2B4B4E32-3ED9-4C8E-B74C-5BDB8D0C7C9E")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under5Juta_BD");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "555C16F8-EDD9-493E-9E1E-82C94CB87C90")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under5Juta_FINACC");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "6C1974F7-08CB-4BE0-804B-63278DB111C5")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Under5Juta_AFF");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                }
                else if (getGrandTotal >= 5000000)
                {
                    if (lbDivision.Text.ToUpper() == "5D2F0CA6-961D-4C09-9EEE-978EEE6309B8")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_ADMINISTRATION");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "6471E2F5-8BBF-4EEC-991F-4B2F7A56A8E5" || lbDivision.Text.ToUpper() == "C42F75D0-F7AD-42B2-ACAD-5200199B813D" || lbDivision.Text.ToUpper() == "825B4273-6E9A-456A-81D5-8B710F50598E" || lbDivision.Text.ToUpper() == "238217DC-2872-4A00-8EBB-1F41A951D363")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_OFF");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "2B4B4E32-3ED9-4C8E-B74C-5BDB8D0C7C9E")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_BD");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "555C16F8-EDD9-493E-9E1E-82C94CB87C90")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_FINACC");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else if (lbDivision.Text.ToUpper() == "6C1974F7-08CB-4BE0-804B-63278DB111C5")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRFPriceEstimated_Upper5Juta_AFF");
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberBreadcrumb.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());

                        sqlcomm.ExecuteNonQuery();
                        Con.Close();
                        UpdatePriceRF();
                        await SendEmailSendToManagerDivision();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'requisition_price_check.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";
                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                }
            }

        }
        #endregion

        #region Barcode
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = lbRFNumberHeader.Text; // Nilai ID yang akan digunakan dalam URL

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

        protected void btnClearPrice_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow grow in TableItemPurchase.Rows)
            {
                HtmlInputText price = (HtmlInputText)grow.FindControl("txtPrice");
                txtGrandTotal.Value = "0";
                price.Value = "0";
                price.Disabled = false;
            }

            divSubmit.Visible = false;
            divCheck.Visible = true;
            divClearPrice.Visible = false;
        }
    }
}