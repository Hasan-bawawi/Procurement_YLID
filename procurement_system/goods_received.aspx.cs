using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Asn1.Cmp;

namespace procurement_system
{
    public partial class goods_received : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataPurchaseOrder();
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
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewPurchaseOrderNeedGR");
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

            Con.Close();
        }

        protected void btnCreateGR_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            GetDataPurchaseOrder();

            lbPONumber.Text = row.Cells[3].Text;
            GetTableItemPO();

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailPurchaseOrder");
                sqlcomm.Parameters.AddWithValue("@po_no", row.Cells[3].Text);
                con.Open();
                using (SqlDataReader rdr = sqlcomm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Session.Add("po_no", (string)rdr["po_no"]);
                        Session.Add("rf_no", (string)rdr["rf_no"]);
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
                        Session.Add("stok_code", (string)(rdr.IsDBNull(38) ? null : rdr["stok_code"]));
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();
            }

            string ReqDateFromDatabase = Session["po_date"].ToString();
            DateTime ParseDatetime = DateTime.Parse(ReqDateFromDatabase);
            string ReqDate = ParseDatetime.ToString("dd MMMM yyyy");
            lbIssuedDate.Value = ReqDate;
            //lbRequester.Value = Session["Requester"].ToString() + " (" + Session["Dept"].ToString() + ")";
            string DateDelivery = Session["delivery_date"].ToString();
            DateTime ParseDateDelivery = DateTime.Parse(DateDelivery);
            string GetDateDelivery = ParseDateDelivery.ToString("dd MMMM yyyy");
            lbDeliveryDate.Value = GetDateDelivery;
            lbAssetType.Value = Session["aset_status"].ToString();
            lbVendorName.Value = Session["vendor_name"].ToString();
            lbPaymentTerms.Value = Session["payment_term"].ToString();
            hlbIDVendor.Value = Session["id_vendor"].ToString();
            lbPOStatus.Value = Session["po_status"].ToString();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlCreateGR').modal();", true);
        }

        protected void GetTableItemPO()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailItemPOOnProcess");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumber.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableItemPO.DataSource = dtb;
            TableItemPO.DataBind();

            //TableItemPO.Columns[8].Visible = false;

            TableItemPO.UseAccessibleHeader = true;
            TableItemPO.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            Response.Redirect("goods_received.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string path_db = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(path_db))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    GetGRNumber();
                    var CurentYear = DateTime.Now.Year;
                    if (CurentYear != (int)Session["years"])
                    {
                        SaveNumbering(con, transaction);
                        GetGRNumberNew();

                        hlbYearsNew.Value = DateTime.Now.Year.ToString();
                        hlblast_numberNew.Value = Session["last_numberNew"].ToString();
                        int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
                        int _getNumberUrut = _LastNumber + 1;

                        if (_getNumberUrut < 10)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "000" + _getNumberUrut;
                        }
                        else if (_getNumberUrut > 9 && _getNumberUrut < 99)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "00" + _getNumberUrut;
                        }
                        else if (_getNumberUrut > 99 && _getNumberUrut < 999)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "0" + _getNumberUrut;
                        }
                        else if (_getNumberUrut > 999)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + _getNumberUrut;
                        }

                        foreach (GridViewRow row in TableItemPO.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                HtmlInputGenericControl QtyReceived = (HtmlInputGenericControl)row.FindControl("txtQtyReceived");
                                if (QtyReceived != null && !string.IsNullOrEmpty(QtyReceived.Value))
                                {
                                    string txtQtyReceived = QtyReceived.Value;
                                    string QtyOrder = row.Cells[8].Text.ToString();
                                    int GetQtyReceived = Convert.ToInt32(txtQtyReceived);
                                    int GetQtyOrder = Convert.ToInt32(QtyOrder);
                                    if (GetQtyReceived != GetQtyOrder)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Save Failed. The quantity received must be the same as the quantity ordered.');", true);
                                    }
                                    else
                                    {
                                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                                        SqlConnection Con = new SqlConnection(path);
                                        Con.Open();
                                        SqlCommand sqlcomm = new SqlCommand();
                                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
                                        sqlcomm.CommandType = CommandType.StoredProcedure;
                                        sqlcomm.Connection = Con;
                                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveGR");
                                        sqlcomm.Parameters.AddWithValue("@gr_no", txtGRNumber.Value);
                                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumber.Text);
                                        sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[2].Text.ToString());
                                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[5].Text.ToString());
                                        sqlcomm.Parameters.AddWithValue("@qty_received", GetQtyReceived);
                                        sqlcomm.Parameters.AddWithValue("@gr_date", GRDate.Value);
                                        sqlcomm.Parameters.AddWithValue("@received_by", txtReceivedBy.Value);
                                        sqlcomm.Parameters.AddWithValue("@note", txtNote.Value);
                                        sqlcomm.Parameters.AddWithValue("@User", Session["nik"].ToString());
                                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[4].Text.ToString());

                                        sqlcomm.ExecuteNonQuery();
                                        CheckUploadDocument();
                                        UpdateCompleteStatusPO(con, transaction);
                                        UpdateCompleteStatusRF(con, transaction);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UpdateNumbering(con, transaction);

                        int _LastNumber = (Convert.ToInt32(hlblast_numberNew.Value) + 1);
                        if (_LastNumber < 10)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "000" + _LastNumber;
                        }
                        else if (_LastNumber > 9 && _LastNumber < 99)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "00" + _LastNumber;
                        }
                        else if (_LastNumber > 99 && _LastNumber < 999)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + "0" + _LastNumber;
                        }
                        else if (_LastNumber > 999)
                        {
                            txtGRNumber.Value = "YLID-GR-" + CurentYear + "-" + _LastNumber;
                        }

                        foreach (GridViewRow row in TableItemPO.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                HtmlInputGenericControl QtyReceived = (HtmlInputGenericControl)row.FindControl("txtQtyReceived");
                                if (QtyReceived != null && !string.IsNullOrEmpty(QtyReceived.Value))
                                {
                                    string txtQtyReceived = QtyReceived.Value;
                                    string QtyOrder = row.Cells[8].Text.ToString();
                                    int GetQtyReceived = Convert.ToInt32(txtQtyReceived);
                                    int GetQtyOrder = Convert.ToInt32(QtyOrder);
                                    if (GetQtyReceived != GetQtyOrder)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Save Failed. The quantity received must be the same as the quantity ordered.');", true);
                                    }
                                    else
                                    {
                                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                                        SqlConnection Con = new SqlConnection(path);
                                        Con.Open();
                                        SqlCommand sqlcomm = new SqlCommand();
                                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
                                        sqlcomm.CommandType = CommandType.StoredProcedure;
                                        sqlcomm.Connection = Con;
                                        sqlcomm.Parameters.AddWithValue("@StatementType", "SaveGR");
                                        sqlcomm.Parameters.AddWithValue("@gr_no", txtGRNumber.Value);
                                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumber.Text);
                                        sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[2].Text.ToString());
                                        sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[5].Text.ToString());
                                        sqlcomm.Parameters.AddWithValue("@qty_received", GetQtyReceived);
                                        sqlcomm.Parameters.AddWithValue("@gr_date", GRDate.Value);
                                        sqlcomm.Parameters.AddWithValue("@received_by", txtReceivedBy.Value);
                                        sqlcomm.Parameters.AddWithValue("@note", txtNote.Value);
                                        sqlcomm.Parameters.AddWithValue("@User", Session["nik"].ToString());
                                        sqlcomm.Parameters.AddWithValue("@stok_code", row.Cells[4].Text.ToString());

                                        sqlcomm.ExecuteNonQuery();
                                        CheckUploadDocument();
                                        UpdateCompleteStatusPO(con, transaction);
                                        UpdateCompleteStatusRF(con, transaction);
                                    }
                                }
                            }
                        }
                    }
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

        protected void UpdateCompleteStatusPO(SqlConnection con, SqlTransaction transaction)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CompletedStatusPO");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumber.Text);

            sqlcomm.ExecuteNonQuery();
            string _vNotif = lbErrorUploadNotif.Value;
            string script = $@"
                    $(document).ready(function() {{
                        // Show Toastr notification
                        toastr.success('Submit Successfully' + ' ' + 'File Upload:' + '{_vNotif}');

                        // Redirect after 2 seconds (2000 milliseconds)
                        setTimeout(function() {{
                            window.location.href = 'goods_received.aspx'; // replace with your target URL
                        }}, 2000);
                    }});
                ";

            // Register the script for partial postbacks
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void UpdateCompleteStatusRF(SqlConnection con, SqlTransaction transaction)
        {
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "CompletedStatusRF");
                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[2].Text.ToString());

                sqlcomm.ExecuteNonQuery();
                string _vNotif = lbErrorUploadNotif.Value;
                string script = $@"
                    $(document).ready(function() {{
                        // Show Toastr notification
                        toastr.success('Submit Successfully' + ' ' + 'File Upload:' + '{_vNotif}');

                        // Redirect after 2 seconds (2000 milliseconds)
                        setTimeout(function() {{
                            window.location.href = 'goods_received.aspx'; // replace with your target URL
                        }}, 2000);
                    }});
                ";

                // Register the script for partial postbacks
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                sqlcomm.Dispose();
                sqlcomm.Parameters.Clear();
                Con.Close();
                Con.Dispose();
            }
        }

        protected void CheckUploadDocument()
        {
            if (FileUploadEDocs.HasFiles)
            {
                int filecount = 0;
                int fileuploadcount = 0;
                lbErrorUploadNotif.Value = ""; // Clear previous messages

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

                            if (filesize < (2048576))
                            {
                                fileuploadcount++;
                                string serverfolder = string.Empty;
                                string serverpath = string.Empty;

                                // Adding File Into Specific Folder Depend On his Extension
                                switch (filetype.ToLower())
                                {
                                    case ".pdf":
                                        string _vGRNumber = txtGRNumber.Value;
                                        serverfolder = Server.MapPath("~/eDocs_Files/GR" + "/" + _vGRNumber + "/");

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
                                        lbErrorUploadNotif.Value += "[" + uniqueFileName + "]- " + filetype + " file uploaded successfully";
                                        hlbOK.Value = "OK";
                                        break;
                                }
                            }
                            else
                            {
                                lbErrorUploadNotif.Value += "[" + postfiles.FileName + "]- File not uploaded; size is greater than 2MB. Your File Size is " + (filesize / (1024 * 1034)) + " MB";
                                //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMaxSize();", true);
                            }
                        }
                        else
                        {
                            lbErrorUploadNotif.Value += "[" + postfiles.FileName + "]- Invalid file type. Allowed types are .pdf";
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMustBe();", true);
                        }
                    }
                }
                else
                {
                    lbErrorUploadNotif.Value = "You have selected " + filecount + " files. Please select a maximum of 5 files.";
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedMaximumUpload();", true);
                }
            }
            else
            {
                lbErrorUploadNotif.Value = "No Files Upload.";
            }
        }

        private bool IsValidFileType(string fileType, string[] allowedExtensions)
        {
            return Array.Exists(allowedExtensions, ext => ext.Equals(fileType, StringComparison.OrdinalIgnoreCase));
        }

        #region GRNumber
        protected void GetGRNumber()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetGRNumber");

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

        protected void GetGRNumberNew()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetGRNumberNew");

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
            //string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //SqlConnection Con = new SqlConnection(path);
            //Con.Open();
            //SqlCommand sqlcomm = new SqlCommand();
            //sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            //sqlcomm.CommandType = CommandType.StoredProcedure;
            //sqlcomm.Connection = Con;
            //sqlcomm.Parameters.AddWithValue("@StatementType", "SaveNumbering");
            //sqlcomm.Parameters.AddWithValue("@years", DateTime.Now.Year);
            //sqlcomm.Parameters.AddWithValue("@id", Session["id"].ToString());

            //sqlcomm.ExecuteNonQuery();
            //Con.Close();
            string SP = "sp_PROCUREMENT_DB_GoodsReceived";
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
            GetGRNumber();
            hlblast_numberNew.Value = Session["last_number"].ToString();
            int _LastNumber = (Convert.ToInt32(hlblast_numberNew.Value) + 1);
            string SP = "sp_PROCUREMENT_DB_GoodsReceived";
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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
            sqlcomm.Parameters.AddWithValue("@id", Session["IDNew"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }
        #endregion

        #region SearchGR
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divViewGR.Visible = true;
            GetDataPurchaseOrder();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsReceived";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewGR");
            sqlcomm.Parameters.AddWithValue("@gr_no", txtGRNo.Value);
            sqlcomm.Parameters.AddWithValue("@po_no", txtPONo.Value);
            sqlcomm.Parameters.AddWithValue("@rf_no", txtRFNo.Value);
            sqlcomm.Parameters.AddWithValue("@vendor_name", txtVendor.Value);
            sqlcomm.Parameters.AddWithValue("@date_from", txtDate1.Value);
            sqlcomm.Parameters.AddWithValue("@date_to", txtDate2.Value);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableGR.DataSource = dtb;
            TableGR.DataBind();

            TableGR.Columns[1].Visible = false;

            TableGR.UseAccessibleHeader = true;
            TableGR.HeaderRow.TableSection = TableRowSection.TableHeader;

            dtb.Dispose();
            sqlcomm.Dispose();
            Con.Dispose();

            Con.Close();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {

        }
        #endregion

        protected void TablePurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 _VDaytoDelivery;
                _VDaytoDelivery = Convert.ToInt32(e.Row.Cells[24].Text.ToString());
                if (_VDaytoDelivery < 0)
                {
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[12].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[13].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[14].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[15].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[16].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[17].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[18].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[19].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[20].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[21].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[22].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[23].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}