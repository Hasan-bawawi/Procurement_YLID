using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Ocsp;
using MimeKit.Cryptography;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Vml;
using System.Drawing;
using ZXing;
using ZXing.QrCode;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using ZXing.QrCode.Internal;

namespace procurement_system
{
    public partial class test_email_oauth : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];

        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Value = Session["nik"].ToString();
            //lblEmail.Text = Session["email_karyawan"].ToString();
            if (!IsPostBack)
            {
                GetDataTableRFNeedApproveDirector();
            }
        }

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

        protected async void btnCreateRF_Click(object sender, EventArgs e)
        {
            await SendEmail();
        }

        private string PopulateBodyToPurchasingCheckEstimatePrice(string reqby, string reqno, string approver, string reqdate)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTempleteRequestPurchase.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ReqBy}", reqby);
            body = body.Replace("{ReqNo}", reqno);
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{ReqDate}", reqdate);
            return body;
        }

        public class FileAttachment
        {
            [JsonProperty("@odata.type")]
            public string Type { get; set; }

            public string Name { get; set; }
            public string ContentBytes { get; set; }
        }

        private async Task SendEmail()
        {
            #region OLD
            //// Konfigurasi autentikasi
            //var clientId = "2c39d851-2ec9-4c9a-9899-d8b9c155a16a";
            //var clientSecret = "IB.8Q~6DT7gwA66BAS7CmsdRIUEyG0uGhpHJhbZ4";
            //var tenantId = "ca2aecff-bc6d-4032-9161-fe56057fd193";
            //var authority = $"https://login.microsoftonline.com/{tenantId}";
            //var scopes = new[] { "https://graph.microsoft.com/.default" };

            //var confidentialClientApplication = ConfidentialClientApplicationBuilder
            //    .Create(clientId)
            //    .WithClientSecret(clientSecret)
            //    .WithAuthority(authority)
            //    .Build();

            //// Mendapatkan token akses
            //var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
            //var accessToken = authResult.AccessToken;

            //// Membuat HttpClient dengan Authorization Header
            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //// Mengirim permintaan HTTP untuk mengirim email
            //var userPrincipalName = "noreply.ylid@nykgroup.com";
            //var endpoint = $"https://graph.microsoft.com/v1.0/users/{userPrincipalName}/sendMail";
            //var emailBody = new
            //{
            //    message = new
            //    {
            //        subject = "Subject Email",
            //        body = new
            //        {
            //            contentType = "Text",
            //            content = "Isi email."
            //        },
            //        toRecipients = new[]
            //        {
            //    new
            //    {
            //        emailAddress = new
            //        {
            //            address = "widhi.kusuma@id.yusen-logistics.com"
            //        }
            //    }
            //}
            //    },
            //    saveToSentItems = true
            //};
            //var emailBodyJson = JsonConvert.SerializeObject(emailBody);
            //var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

            //var response = await httpClient.PostAsync(endpoint, content);
            //var responseContent = await response.Content.ReadAsStringAsync();

            //// Tindakan setelah mengirim email
            //// Misalnya, menampilkan pesan sukses atau mengarahkan pengguna ke halaman lain
            //if (response.IsSuccessStatusCode)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
            //}
            //else
            //{
            //    Response.Write(responseContent.ToString());
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            //}
            #endregion

            #region FIX
            //try
            //{
            //    // Konfigurasi autentikasi
            //    var authority = $"https://login.microsoftonline.com/{_tenantId}";
            //    var scopes = new[] { "https://graph.microsoft.com/.default" };

            //    var confidentialClientApplication = ConfidentialClientApplicationBuilder
            //        .Create(_clientId)
            //        .WithClientSecret(_clientSecret)
            //        .WithAuthority(authority)
            //        .Build();

            //    // Mendapatkan token akses
            //    var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
            //    var accessToken = authResult.AccessToken;

            //    // Membuat HttpClient dengan Authorization Header
            //    var httpClient = new HttpClient();
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //    // Mengirim permintaan HTTP untuk mengirim email

            //    var emailBody = new
            //    {
            //        message = new
            //        {
            //            subject = "Subject Email",
            //            body = new
            //            {
            //                contentType = "Text",
            //                content = "TEST Modern authentication / OAUTH2."
            //            },
            //            toRecipients = new[]
            //            {
            //        new
            //        {
            //            emailAddress = new
            //            {
            //                address = "widhi.kusuma@id.yusen-logistics.com"
            //            }
            //        }
            //    }
            //        },
            //        saveToSentItems = true
            //    };

            //    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
            //    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

            //    var response = await httpClient.PostAsync(_endpoint, content);
            //    var responseContent = await response.Content.ReadAsStringAsync();

            //    // Tindakan setelah mengirim email
            //    // Misalnya, menampilkan pesan sukses atau mengarahkan pengguna ke halaman lain
            //    if (response.IsSuccessStatusCode)
            //    {
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
            //    }
            //    else
            //    {
            //        Response.Write(responseContent.ToString());
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Tangani kesalahan
            //    Response.Write(ex.ToString());
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            //}
            #endregion

            #region OAUTH2_HTML_BODYEMAIL
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

                    StreamReader reader = new StreamReader(Server.MapPath("~/EmailTempleteRequestPurchase.html"));
                    string htmlContent = reader.ReadToEnd();
                    htmlContent = htmlContent.Replace("{ReqBy}", "Widhi");
                    htmlContent = htmlContent.Replace("{ReqNo}", "YLID-RF-2023-0007");
                    htmlContent = htmlContent.Replace("{ApprovalStatus}", "TEST OAUTH2");
                    htmlContent = htmlContent.Replace("{GMDivision}", "-");
                    htmlContent = htmlContent.Replace("{ReqDate}", "TODAY");

                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", "YLID-RF-2023-0007");

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "YLID-RF-2023-0007" + ".pdf";
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
                            subject = "Subject Email",
                            body = new
                            {
                                contentType = "HTML",
                                content = htmlContent
                            },
                            toRecipients = new[]
                            {
                                new
                                {
                                    emailAddress = new
                                    {
                                        address = "widhi.kusuma@id.yusen-logistics.com"
                                    }
                                }
                            },
                            ccRecipients = new[]
                            {
                                new
                                {
                                    emailAddress = new
                                    {
                                        address = "widhi.kusuma@id.yusen-logistics.com"
                                    }
                                },
                                new
                                {
                                    emailAddress = new
                                    {
                                        address = "widhi.kusuma@id.yusen-logistics.com"
                                    }
                                }
                            },
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
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
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
            #endregion
        }

        protected void btnDownloadRF_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath_itadmin_test"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string sqlquery = "SELECT signature FROM Employee where nik=@nik";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, Con);
            sqlcomm.Parameters.AddWithValue("@nik", "891109");

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                // Retrieve the image data from the database
                byte[] imageData = (byte[])dr["signature"];

                // Generate the QR code
                string qrCodeBase64 = GenerateQRCode(imageData);

                // Set the QR code image to the ImageUrl property of a control (assuming QRCodeImage is an ASP.NET Image control)
                QRCodeImage.ImageUrl = "data:image/png;base64," + qrCodeBase64;
            }



        }

        private string GenerateQRCode(byte[] imageData)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new QrCodeEncodingOptions
            {
                QrVersion = 10,
                ErrorCorrection = ErrorCorrectionLevel.Q,
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 300, // Set the desired width of the QR code
                Height = 300, // Set the desired height of the QR code
            };

            // Generate the QR code from the image data
            Bitmap qrCodeBitmap = barcodeWriter.Write(Convert.ToBase64String(imageData));

            // Convert the QR code bitmap to a base64-encoded string
            string qrCodeBase64 = ImageToBase64(qrCodeBitmap);

            return qrCodeBase64;
        }

        // Function to convert an image to a base64-encoded string
        private string ImageToBase64(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        protected void btnViewDirector_Click(object sender, EventArgs e)
        {

        }
    }
}