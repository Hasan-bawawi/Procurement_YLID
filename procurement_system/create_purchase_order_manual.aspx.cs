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
using System.Globalization;

namespace procurement_system
{
    public partial class create_purchase_order_manual : System.Web.UI.Page
    {

        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];
        string _emailGAMgr = WebConfigurationManager.AppSettings["emailGAMgr"];
        string _emailITMgr = WebConfigurationManager.AppSettings["emailITMgr"];

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                DateTime currentDateTime = DateTime.Now;
                txtIssuedDate.Value = currentDateTime.ToString();
                txtPaymentTerms.Value = "30";
                LoadVendor();
                GetRequester();
                GridTemporary();
            }

        }


        protected void LoadVendor()
        {
            ddlVendor.Items.Clear();
            ddlVendor.Items.Add(new ListItem("-- Select Vendor --", ""));

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_Vendor", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "AddVendor");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlVendor.Items.Add(new ListItem(
                        dr["vendor_name"].ToString(),
                        dr["id"].ToString()
                    ));
                }
            }
        }

        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vendorId = ddlVendor.SelectedValue;

            if (vendorId == "")
            {
                txtAddress.InnerText = "";
                txtAddress.Value = "";
                return;
            }

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_Vendor", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "GetVendorById");
                cmd.Parameters.AddWithValue("@Id", vendorId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtAddress.Value = dr["address"].ToString();
                    txtAddress.InnerText = dr["address"].ToString();
                }
            }
        }



        //protected void GetRequester()
        //{
        //    ddlRequester.Items.Clear();
        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);


        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewRequester");
        //    SqlDataReader dr;

        //    try
        //    {
        //        ListItem newItem = new ListItem();
        //        newItem.Text = "-- Select Requester --";
        //        newItem.Value = "0";
        //        ddlRequester.Items.Add(newItem);

        //        Con.Open();
        //        dr = sqlcomm.ExecuteReader();

        //        //while (dr.Read())
        //        //{
        //        //    newItem = new ListItem();
        //        //    newItem.Text = dr["FulnameApprover"].ToString();
        //        //    newItem.Value = dr["NikApprover"].ToString();
        //        //    //hlblocation.Value = dr["Location"].ToString();
        //        //    ddlRequester.Items.Add(newItem);

        //        //}
        //        while (dr.Read())
        //        {
        //            ListItem item = new ListItem();

        //            // TAMPILAN (boleh gabung)
        //            item.Text = dr["FulnameApprover"].ToString(); // fullname + (division)

        //            // VALUE tetap NIK
        //            item.Value = dr["NikApprover"].ToString();

        //            // SIMPAN DATA TAMBAHAN
        //            item.Attributes["data-fullname"] = dr["Fullname"].ToString();
        //            item.Attributes["data-divisionid"] = dr["id"].ToString(); // uniqueidentifier

        //            ddlRequester.Items.Add(item);
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


        protected void GetRequester()
        {
            ddlRequester.Items.Clear();

            // Default item
            ddlRequester.Items.Add(new ListItem("-- Select Requester --", ""));

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_Purchase", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "ViewRequester");

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string nik = dr["NikApprover"].ToString();
                        string fullname = dr["Fullname"].ToString();
                        string divisionId = dr["id"].ToString();

                        // 🔐 VALUE = DATA ASLI (AMAN POSTBACK)
                        string value = $"{nik}|{fullname}|{divisionId}";

                        // 👀 TEXT = TAMPILAN
                        string text = dr["FulnameApprover"].ToString();
                        // contoh: "Budi Santoso (Finance)"

                        ddlRequester.Items.Add(new ListItem(text, value));
                    }
                }
            }

            // Wajib kalau pakai selectpicker
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "refreshSelect",
                "$('.selectpicker').selectpicker('refresh');",
                true
            );
        }


        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ddlItem.SelectedItem.Text == "")
            {
                Response.Redirect("create_purchase_order_manual.aspx");
            }
            else
            {
                GetDetailItems();
                hlbCodeItem.Value = Session["item_code"].ToString();
                hlbItem.Value = Session["Item"].ToString();
                hlbUOM.Value = Session["Unit"].ToString();
            }

        }

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

            TableItemPurchase.UseAccessibleHeader = true;
            //TableItemPO.Columns[9].Visible = false;
        }


        protected void ddlRequester_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(ddlRequester.SelectedValue))
            {
                hfFullnameApprover.Value = "";
                txtdivid.Value = "";
                return;
            }

            // 🔓 Pecah VALUE
            string[] parts = ddlRequester.SelectedValue.Split('|');

            string nikrequest = parts[0];
            string fullname = parts[1];
            string divisionId = parts[2];

            // Simpan ke HiddenField
            hfFullnameApprover.Value = fullname;
            txtdivid.Value = divisionId;
            txtRequester.Value = nikrequest;

            //string nikrequest = ddlRequester.SelectedValue;

            //if (nikrequest == "" || nikrequest == "0")
            //{
            //    hlblocation.Value = "";
            //    hfFullnameApprover.Value = "";

            //    return;
            //}
            //ListItem selectedItem = ddlRequester.SelectedItem;
            //hfFullnameApprover.Value = selectedItem.Attributes["data-fullname"];
            //txtdivid.Value = selectedItem.Attributes["data-divisionid"];

            //hfFullnameApprover.Value = ddlRequester.SelectedItem.Text;
            //txtdivid.Value = ;

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_Purchase", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "Applyloc");
                cmd.Parameters.AddWithValue("@nik_requester", nikrequest);
               
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hlblocation.Value = dr["Location"].ToString();
                }
            }

            GetItems();

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            DataTable dt = (DataTable)ViewState["TempData"];
            dt.Rows.RemoveAt(row.RowIndex);

            txtGrandTotal.Value = "0";
            txtvat.Value = "0";

            TableItemPurchase.DataSource = dt;
            TableItemPurchase.DataBind();
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

        protected void ddlAssetStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlRequesttype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetItems();
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
            sqlcomm.Parameters.AddWithValue("@nama_branch", hlblocation.Value);
            sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalog.SelectedItem.Text);

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






        protected void ddlDeliveryTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btnAddItem_Click(object sender, EventArgs e)
        {
                string code_stok = ddlItem.SelectedValue;
                string code_item = hlbCodeItem.Value;
                string item = hlbItem.Value;
                //string description = txtDescription.Value;
                string qty = txtJumlahBeli.Value;
                string remarks = txtRemaks.Value;
                string UOM = hlbUOM.Value;


                DataTable dt;

                if (ViewState["TempData"] != null)
                {
                    dt = (DataTable)ViewState["TempData"];
                }
                else
                {
                    // Create a new DataTable if no data exists
                    dt = new DataTable();
                    dt.Columns.Add("kode_barang", typeof(string));
                    dt.Columns.Add("nama_barang", typeof(string));
                    dt.Columns.Add("stok_kode", typeof(string));
                    dt.Columns.Add("jumlah_beli", typeof(string));
                    dt.Columns.Add("remaks", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("price", typeof(decimal));

            }


                // Create a new DataRow and populate it with data
                DataRow newRow = dt.NewRow();
                newRow["kode_barang"] = code_item;
                newRow["nama_barang"] = item;
                newRow["stok_kode"] = code_stok;
                newRow["jumlah_beli"] = qty;
                newRow["remaks"] = remarks;
                newRow["UOM"] = UOM;


                //foreach (GridViewRow row in TableItemPurchase.Rows)
                //{
                //    HtmlInputText txtPrice = (HtmlInputText)row.FindControl("txtprice");

                //    if (txtPrice != null)
                //    {
                //        int rowIndex = row.RowIndex;
                //        string rawValue = txtPrice.Value;

                //        int price = 0;
                //        int.TryParse(
                //            rawValue,
                //            NumberStyles.AllowThousands,
                //            CultureInfo.InvariantCulture,
                //            out price
                //        );

                //    dt.Rows[rowIndex]["price"] = price;
                //}
                //}

                // Add the new DataRow to the DataTable
                dt.Rows.Add(newRow);

                // Store the updated DataTable in ViewState
                ViewState["TempData"] = dt;

                // Bind the DataTable to the GridView
                TableItemPurchase.DataSource = dt;
                TableItemPurchase.DataBind();

                GetItems();

                txtJumlahBeli.Value = "0";
                txtRemaks.Value = "-";
                //ddlReqType.Enabled = false;                
                divSubmit.Visible = true;
                divUploadFile.Visible = true;

                divgrantot.Visible = true;

                txtGrandTotal.Value = "0";
                txtvat.Value = "0";
            //}

        }


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

        private async Task SendEmailToManagerIT(string approver,string btn, string pono, string attcment)
        {
            string body = this.PopulateBodySendToManagerIT
            (/*"DUDY SETIADI"*/ approver, /*txtPONumber.Value*/ pono, txtIssuedDate.Value, hfFullnameApprover.Value, Session["fullname"].ToString(), "PO Created");

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

                        string FiletpathNew = Server.MapPath("~/eDocs_Files/PO/" + attcment);

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
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } },
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

                            if (btn == "satuan")
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(txtPONumber.Value)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                                return;

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
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } },
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

                            if (btn == "satuan")
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(txtPONumber.Value)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                                return;

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

        private async Task SendEmailToManagerGA(string approver,string btn, string pono, string attcment)
        {
            string body = this.PopulateBodySendToManagerGA
            (/*"SURI ARBAD"*/approver, /*txtPONumber.Value*/ pono, txtIssuedDate.Value, hfFullnameApprover.Value, Session["fullname"].ToString(), "PO Created");

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
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } },
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


                            if (btn == "satuan")
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(txtPONumber.Value)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                                return;
                            }
                            //else
                            //{
                            //    string script = $@"
                            //            $(document).ready(function() {{
                            //                // Show Toastr notification
                            //                toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                            //                // Redirect after 2 seconds (2000 milliseconds)
                            //                setTimeout(function() {{
                            //                    window.location.href = 'purchase_order.aspx'; // replace with your target URL
                            //                }}, 2000);
                            //            }});
                            //        ";

                            //    // Register the script for partial postbacks
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            //}

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
                                ccRecipients = new[] { new { emailAddress = new { address = "sardi.evelina@id.yusen-logistics.com" } }, new { emailAddress = new { address = "rizal.syahputra@id.yusen-logistics.com" } } },
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

                            if (btn == "satuan")
                            {

                                string Message = $@"
                                setTimeout(function() {{
                                SelectSucsess('{HttpUtility.JavaScriptStringEncode(txtPONumber.Value)}');}});";

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                                return;

                            }
                            //else
                            //{
                            //    string script = $@"
                            //            $(document).ready(function() {{
                            //                // Show Toastr notification
                            //                toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                            //                // Redirect after 2 seconds (2000 milliseconds)
                            //                setTimeout(function() {{
                            //                    window.location.href = 'purchase_order.aspx'; // replace with your target URL
                            //                }}, 2000);
                            //            }});
                            //        ";

                            //    // Register the script for partial postbacks
                            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                            //}
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






        protected async void btnSubmitPO_Click(object sender, EventArgs e)
        {

            CheckUploadDocument();



            DataTable dtDetail = (DataTable)ViewState["TempData"];

            for (int i = 0; i < TableItemPurchase.Rows.Count; i++)
            {
                GridViewRow row = TableItemPurchase.Rows[i];

                HtmlInputText txtPrice = (HtmlInputText)row.FindControl("txtprice");

                decimal price = 0;
                if (txtPrice != null)
                {
                    decimal.TryParse(txtPrice.Value.Replace(",", ""), out price);
                }

                dtDetail.Rows[i]["price"] = price;
            }


            ViewState["TempData"] = dtDetail;



            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection Con = new SqlConnection(path))
            {
                Con.Open();
                SqlTransaction transaction = Con.BeginTransaction();

                try
                {

                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrderManual";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Connection = Con;
                    sqlcomm.Transaction = transaction;

                    sqlcomm.Parameters.AddWithValue("@StatementType", "SavesatuanPOManual");
                    sqlcomm.Parameters.AddWithValue("@delivery_to", Textareadelivery.Value);
                    sqlcomm.Parameters.AddWithValue("@delivery_date", txtDeliveryDate.Value);
                    sqlcomm.Parameters.AddWithValue("@vat", txtvat.Value);
                    sqlcomm.Parameters.AddWithValue("@aset_status", ddlAssetStatus.SelectedItem.Text);
                    sqlcomm.Parameters.AddWithValue("@payment_term", txtPaymentTerms.Value);
                    sqlcomm.Parameters.AddWithValue("@other_condition", txtOtherCondition.Value);

                    sqlcomm.Parameters.AddWithValue("@id_vendor", ddlVendor.SelectedValue);
                    sqlcomm.Parameters.AddWithValue("@po_created_by", Session["nik"].ToString());
                    sqlcomm.Parameters.AddWithValue("@modifiedby", Session["nik"].ToString());
                    sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalog.SelectedItem.Text);
                    sqlcomm.Parameters.AddWithValue("@status", "PO Created");
                    sqlcomm.Parameters.AddWithValue("@deliveryselect", ddlDeliveryTo.SelectedItem.Text);
                    sqlcomm.Parameters.AddWithValue("@approve_status", "PO Created");
                    sqlcomm.Parameters.AddWithValue("@po_status", "Not Complete");
                    sqlcomm.Parameters.AddWithValue("@requesting_dept", txtdivid.Value);
                    sqlcomm.Parameters.AddWithValue("@po_requestermanual", txtRequester.Value);
                    //sqlcomm.ExecuteNonQuery();


                    SqlParameter tvpParam = sqlcomm.Parameters.AddWithValue("@DetailItems", dtDetail);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.DetailPurchaseType";



                    //object result = sqlcomm.ExecuteScalar();


                    //transaction.Commit();
                    //string newPONumber = result != null ? result.ToString() : "";

                    //txtPONumber.Value = newPONumber;

                    string newPONumber = "";
                    string approverName = "";

                    using (SqlDataReader reader = sqlcomm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            newPONumber = reader["NoPO"].ToString();
                            approverName = reader["Approver"].ToString();
                        }
                    }

                    transaction.Commit();

                    txtPONumber.Value = newPONumber;
                    hfApprover.Value = approverName;



                    DateTime currentDateTime = DateTime.Now;
                    txtIssuedDate.Value = currentDateTime.ToString();

                    string oldFilePath = Server.MapPath("~/" + hfAttachmentPath.Value);

                    if (File.Exists(oldFilePath))
                    {

                        string newFileName = Path.GetFileName(oldFilePath);
                        hfAttachmentPath.Value = newFileName;
                    }

                    if (ddlCatalog.SelectedItem.Text == "IT")
                    {

                        await SendEmailToManagerIT(hfApprover.Value,"satuan", newPONumber, hfAttachmentPath.Value);

                    }
                    else
                    {
                        await SendEmailToManagerGA(hfApprover.Value,"satuan", newPONumber, hfAttachmentPath.Value);


                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }

            }



        }




        //protected void txtVAT_TextChanged(object sender, EventArgs e)
        //{
        //    int total = 0;
        //    foreach (GridViewRow grow in TableItemPO.Rows)
        //    {
        //        HtmlInputText amount = (HtmlInputText)grow.FindControl("txtAmount");
        //        decimal parsedValue = decimal.Parse(amount.Value, NumberStyles.Currency);
        //        int getAmount = Convert.ToInt32(parsedValue);
        //        total += getAmount;
        //    }

        //    int vat = Convert.ToInt32(txtVAT.Text.ToString());
        //    decimal vatValue = vat / 100m;
        //    int getTotal = Convert.ToInt32(total);
        //    decimal vatAmount;
        //    //string vatDecimal= vatValue.ToString("0.00");

        //    vatAmount = vatValue * getTotal;

        //    txtVatAmount.Value = vatAmount.ToString("#,##0");

        //    decimal grandTotal = getTotal + vatAmount;
        //    txtGrandTotal.Value = grandTotal.ToString("#,##0");
        //    int getGrandTotal = Convert.ToInt32(grandTotal);
        //    hlbGrandTotal.Value = getGrandTotal.ToString();

        //    // Display the total value
        //    decimal value;
        //    Decimal.TryParse(total.ToString(), out value);
        //    txtTotalAmount.Value = value.ToString("#,##0");
        //}

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
                                        //string fileName = Path.GetFileName(postfiles.FileName);
                                        //string _vPONumber =/* txtPONumber.Value*/"POMANUAL_" + postfiles + "_" + Guid.NewGuid().ToString("N").Substring(0, 8); 
                                        //serverfolder = Server.MapPath("~/eDocs_Files/PO" + "/" + _vPONumber + "/");

                                        serverfolder = Server.MapPath("~/eDocs_Files/PO/");


                                        // Ensure the folder is available
                                        if (!Directory.Exists(serverfolder))
                                        {
                                            // Create the folder
                                            Directory.CreateDirectory(serverfolder);
                                        }

                                        // Append a unique identifier to avoid overwriting
                                        string uniqueFileName = "Purchase Order (manual)_" + Path.GetFileNameWithoutExtension(postfiles.FileName) +"_" + Guid.NewGuid().ToString("N").Substring(0, 8) + Path.GetExtension(postfiles.FileName);

                                        serverpath = Path.Combine(serverfolder, uniqueFileName);

                                        postfiles.SaveAs(serverpath);
                                        lbErrorUploadNotif.InnerText += "[" + uniqueFileName + "]- " + filetype + " file uploaded successfully";
                                        hlbOK.Value = "OK";
                                        hfAttachmentPath.Value = "eDocs_Files/PO/" + uniqueFileName;

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
                    lbErrorUploadNotif.InnerText = "You have selected " + filecount + " files. Please select a maximum of 5 files.";
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

    }
}