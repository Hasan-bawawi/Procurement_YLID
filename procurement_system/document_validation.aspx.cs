using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;

namespace procurement_system
{
    public partial class document_validation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DetailDocumentRF();
                DetailDocumentPO();

                string _url = Request.QueryString["id"];
                //int start = url.LastIndexOf('=');
                //int length = url.Length - url.LastIndexOf('=');
                string _pageName = _url.Substring(0, 7);

                if (_pageName=="YLID-PO")
                {
                    txtDocNo.Value = Session["po_no"].ToString();
                    txtCreateby.Value = Session["po_created_by"].ToString();
                    txtDocDate.Value = Session["po_date"].ToString();
                }
                else
                {
                    txtDocNo.Value = Session["rf_no"].ToString();
                    txtCreateby.Value = Session["Requester"].ToString();
                    txtDocDate.Value = Session["request_date"].ToString();
                }
                
            }
        }

        protected void DetailDocumentRF()
        {
            string id = Request.QueryString["id"];
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
                        Session.Add("id_vendor", (string)rdr["id_vendor"].ToString());
                        Session.Add("nama_branch", (string)rdr["nama_branch"]);
                        Session.Add("DivisionRequester", (string)rdr["DivisionRequester"].ToString());
                        Session.Add("SectionRequester", (string)rdr["SectionRequester"]);
                        Session.Add("nik_requester", (string)rdr["nik_requester"]);
                        Session.Add("ManagerApprove", (string)(rdr.IsDBNull(7) ? null : rdr["ManagerApprove"]));
                        Session.Add("GMApprove", (string)(rdr.IsDBNull(9) ? null : rdr["GMApprove"]));
                        Session.Add("DeputyDirectorApprove", (string)(rdr.IsDBNull(11) ? null : rdr["DeputyDirectorApprove"]));
                        //Session.Add("AdmManagerApprove", (string)(rdr.IsDBNull(13) ? null : rdr["AdmManagerApprove"]));
                        //Session.Add("AdmGMApprove", (string)(rdr.IsDBNull(15) ? null : rdr["AdmGMApprove"]));
                        //Session.Add("ITManagerApprove", (string)(rdr.IsDBNull(17) ? null : rdr["ITManagerApprove"]));
                        Session.Add("DirectorApprove", (string)(rdr.IsDBNull(18) ? null : rdr["DirectorApprove"]));
                        //Session.Add("AdmDirectorApprove", (string)(rdr.IsDBNull(19) ? null : rdr["AdmDirectorApprove"]));
                        Session.Add("EmailRequester", (string)rdr["EmailRequester"]);
                        Session.Add("EmailManagerApprove", (string)(rdr.IsDBNull(8) ? null : rdr["EmailManagerApprove"]));
                        //Session.Add("EmailGMApprove", (string)rdr["EmailGMApprove"]);
                        //Session.Add("EmailAdmManagerApprove", (string)(rdr.IsDBNull(14) ? null : rdr["EmailAdmManagerApprove"]));
                        //Session.Add("EmailAdmGMApprove", (string)(rdr.IsDBNull(16) ? null : rdr["EmailAdmGMApprove"]));
                        //Session.Add("nik_adm_manager", (string)rdr["nik_adm_manager"]);
                        //Session.Add("nik_adm_gm", (string)rdr["nik_adm_gm"]);
                        Session.Add("DivisionReq", (string)rdr["DivisionReq"].ToString());
                        Session.Add("IDSectionRequester", (string)rdr["IDSectionRequester"].ToString());
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();
            }
            
        }

        protected void DetailDocumentPO()
        {
            string id = Request.QueryString["id"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailPurchaseOrder");
                sqlcomm.Parameters.AddWithValue("@po_no", id);
                con.Open();
                using (SqlDataReader rdr = sqlcomm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Session.Add("po_no", (string)rdr["po_no"]);
                        //Session.Add("rf_no", (string)rdr["rf_no"]);
                        Session.Add("po_type", (string)rdr["po_type"]);
                        Session.Add("vendor_name", (string)rdr["vendor_name"]);
                        Session.Add("po_date", (DateTime)rdr["po_date"]);
                        Session.Add("delivery_date", (DateTime)rdr["delivery_date"]);
                        Session.Add("delivery_to", (string)rdr["delivery_to"]);
                        Session.Add("item_code", (string)rdr["item_code"]);
                        Session.Add("item_name", (string)rdr["item_name"]);
                        Session.Add("price", (int)rdr["price"]);
                        Session.Add("vat", (int)rdr["vat"]);
                        Session.Add("amount", (int)rdr["amount"]);
                        Session.Add("payment_term", (string)rdr["payment_term"]);
                        Session.Add("remarks", (string)rdr["remarks"]);
                        Session.Add("aset_status", (string)rdr["aset_status"]);
                        Session.Add("po_created_by", (string)(rdr.IsDBNull(15) ? null : rdr["po_created_by"]));
                        Session.Add("po_approved_by", (string)(rdr.IsDBNull(16) ? null : rdr["po_approved_by"]));
                        Session.Add("po_checked_by", (string)(rdr.IsDBNull(17) ? null : rdr["po_checked_by"]));
                        Session.Add("authorized_by", (string)(rdr.IsDBNull(18) ? null : rdr["authorized_by"]));
                        Session.Add("po_checked_by_it", (string)(rdr.IsDBNull(19) ? null : rdr["po_checked_by_it"]));
                        Session.Add("approve_status", (string)rdr["approve_status"]);
                        Session.Add("po_status", (string)rdr["po_status"]);
                        Session.Add("Dept", (string)rdr["Dept"]);
                        Session.Add("create_date", (DateTime)rdr["create_date"]);
                        Session.Add("modifiedby", (string)rdr["modifiedby"]);
                        Session.Add("modified_date", (DateTime)rdr["modified_date"]);
                        Session.Add("Requester", (string)rdr["Requester"]);
                        Session.Add("quantity", (int)rdr["quantity"]);
                        Session.Add("unit_name", (string)rdr["unit_name"]);
                        Session.Add("id_vendor", (string)rdr["id_vendor"].ToString());
                        Session.Add("other_condition", (string)(rdr.IsDBNull(31) ? null : rdr["other_condition"]));
                        Session.Add("email_po_created_by", (string)(rdr.IsDBNull(32) ? null : rdr["email_po_created_by"]));
                        Session.Add("email_po_approved_by", (string)(rdr.IsDBNull(33) ? null : rdr["email_po_approved_by"]));
                        Session.Add("email_po_checked_by", (string)(rdr.IsDBNull(34) ? null : rdr["email_po_checked_by"]));
                        Session.Add("email_authorized_by", (string)(rdr.IsDBNull(35) ? null : rdr["email_authorized_by"]));
                        Session.Add("email_po_checked_by_it", (string)(rdr.IsDBNull(36) ? null : rdr["email_po_checked_by_it"]));
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();

            }
            
        }

        #region Barcode_RF
        private void GenerateAndDisplayBarcodeRF()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = txtDocNo.Value; // Nilai ID yang akan digunakan dalam URL

            // Membuat URL dengan parameter
            string data = $"{baseUrl}?rf_no={Uri.EscapeDataString(id)}";

            // Membuat gambar QR code
            System.Drawing.Image barcodeImage = GenerateBarcodeImageRF(data);

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
            SaveBarcodeImageRF(combinedImage, folderPath);

            // Menampilkan gambar barcode di halaman
            DisplayImageOnPageRF(combinedImage);
        }

        private void SaveBarcodeImageRF(System.Drawing.Image barcodeImage, string folderPath)
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

        private System.Drawing.Image GenerateBarcodeImageRF(string data)
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

        private void DisplayImageOnPageRF(System.Drawing.Image barcodeImage)
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

        #region Barcode_PO
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = txtDocNo.Value; // Nilai ID yang akan digunakan dalam URL

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

        protected void btnCekDoc_Click(object sender, EventArgs e)
        {
            string _url = Request.QueryString["id"];
            //int start = url.LastIndexOf('=');
            //int length = url.Length - url.LastIndexOf('=');
            string _pageName = _url.Substring(0, 7);

            if (_pageName == "YLID-PO")
            {
                #region Code_PO
                if (Session["approve_status"].ToString() == "PO Created")
                {
                    // Price<= 1Jt GA Catalog
                    if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price<= 1Jt IT Catalog
                    else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price beetwen 1Jt-20JT GA Catalog
                    else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price beetwen 1Jt-20JT IT Catalog
                    else if (Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price >= 20Jt GA Catalog
                    else if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price >= 20Jt GA Catalog
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
                {
                    if (Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_ITHeadApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderITHeadApproved_SendToGAHead.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_ITHeadApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderITHeadApproved_SendToGAHead.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                {
                    if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved_ITHeadNull";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin_ITHeadNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                else if (Session["approve_status"].ToString() == "Approved (Admin GM)")
                {
                    if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved_ITHeadNull";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized_ITHeadNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                else if (Session["approve_status"].ToString() == "Approved (Full Approval)")
                {
                    // Price<= 1Jt GA Catalog
                    if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under1jt_GA";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price<= 1Jt IT Catalog
                    else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under1jt_IT";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToPurchaseTim.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price beetwen 1Jt-20JT GA Catalog
                    else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under20jt_GA";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price beetwen 1Jt-20JT IT Catalog
                    else if (Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under20jt_IT";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToPurchaseTim_AuthorizedNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price >= 20Jt GA Catalog
                    else if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Upper20jt_GA";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderPresdir_DirectorApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                    // Price >= 20Jt GA Catalog
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Upper20jt_IT";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@po_no", txtDocNo.Value);

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderPresdir_DirectorApproved_SendToPurchaseTim.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Purchase Order - " + txtDocNo.Value.Trim() + ".pdf";
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
                else if (Session["approve_status"].ToString() == "CANCEL")
                {
                    // Price<= 1Jt GA Catalog
                    if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {

                    }
                    // Price<= 1Jt IT Catalog
                    else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {

                    }
                    // Price beetwen 1Jt-20JT GA Catalog
                    else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                    {

                    }
                    // Price beetwen 1Jt-20JT IT Catalog
                    else if (Session["authorized_by"] is null)
                    {

                    }
                    // Price >= 20Jt GA Catalog
                    else if (Session["po_checked_by_it"] is null)
                    {

                    }
                    // Price >= 20Jt GA Catalog
                    else
                    {

                    }
                }
                #endregion
            }
            else
            {
                #region Code_RF
                if (Session["status_approve"].ToString() == "NOT YET")
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Price Checked")
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_PriceEstimate_Under1Juta_SUBSRG";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPriceEstimated_Under1Juta_SUB_SRG.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ManagerApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseManagerDivisionApproved.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Approved (Division GM)")
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GMDivisionApproved_SendToDeputyDirector";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGMDivisionApproved_SendToDeputyDirector.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_DepDirApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcodeRF();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseDepDirApproved.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
                {
                    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                        else
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_ManagerDiv";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_ManagerDiv.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                        else
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_GMDiv";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_GMDiv.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else if (Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                        else
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_DepDir";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_DepDir.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else if (Session["DeputyDirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                        else
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_DepDirNull";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_DepDirNull.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                    else
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                        else
                        {
                            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                            SqlConnection Con = new SqlConnection(path);
                            Con.Open();
                            SqlCommand sqlcomm = new SqlCommand();
                            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved";
                            sqlcomm.CommandType = CommandType.StoredProcedure;
                            sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                            sqlcomm.Connection = Con;
                            DataTable dtb = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                            sda.Fill(dtb);
                            GenerateAndDisplayBarcodeRF();
                            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved.rdlc");
                            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                            ReportViewerPurchase.LocalReport.DataSources.Clear();
                            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                            ReportViewerPurchase.LocalReport.Refresh();

                            string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                }
                else if (Session["status_approve"].ToString() == "Reject (Division Manager)" || Session["status_approve"].ToString() == "Cancel (Division Manager)")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcodeRF();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Reject (Division GM)" || Session["status_approve"].ToString() == "Cancel (Division GM)")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcodeRF();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Reject (Deputy Director)" || Session["status_approve"].ToString() == "Cancel (Deputy Director)")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcodeRF();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Reject (Division Director)" || Session["status_approve"].ToString() == "Cancel (Division Director)")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcodeRF();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                else if (Session["status_approve"].ToString() == "Reject (Fully Approved)" || Session["status_approve"].ToString() == "Cancel (Fully Approved)")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", txtDocNo.Value.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcodeRF();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + txtDocNo.Value + ".pdf";
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
                #endregion
            }



        }
    }
}