using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

namespace procurement_system
{
    public partial class vendor_detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCategoryName();
                GetDataVendor();
            }
        }

        protected void GetCategoryName()
        {
            ddlCategory.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddCategory");

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlCategory.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["category_vendor"].ToString();
                    newItem.Value = dr["id"].ToString();
                    ddlCategory.Items.Add(newItem);
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

        protected void GetDataVendor()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableVendor.DataSource = dtb;
            TableVendor.DataBind();

            TableVendor.Columns[1].Visible = false;
            TableVendor.Columns[2].Visible = false;

            TableVendor.UseAccessibleHeader = true;
            TableVendor.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewVendor').modal();", true);
            GetDataVendor();
            GetCategoryName();
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            txtCreateByNew.Value = Session["Fullname"].ToString();
            txtCreateDateNew.Value = DateTime.Now.ToString();
            txtModifiedByNew.Value = Session["Fullname"].ToString();
            txtModifiedDateNew.Value = DateTime.Now.ToString();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImportExcel').modal();", true);
            GetDataVendor();
            GetCategoryName();
        }

        protected void TableVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewVendor').modal();", true);
            GetDataVendor();
            GetCategoryName();

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            hlbID.Value = row.Cells[1].Text.ToString();
            ddlCategory.SelectedValue = row.Cells[2].Text;
            txtCode.Value = row.Cells[3].Text.ToString();
            ddlCategory.SelectedItem.Text = row.Cells[4].Text;
            txtVendorName.Value = row.Cells[5].Text.ToString();
            txtPICNameSales.Value = row.Cells[6].Text.ToString();
            txtPhoneNumberSales.Value = row.Cells[7].Text.ToString();
            txtEmailSales.Value = row.Cells[8].Text.ToString();
            txtPICNameInvoice.Value = row.Cells[9].Text.ToString();
            txtPhoneNumberInvoice.Value = row.Cells[10].Text.ToString();
            txtEmailInvoice.Value = row.Cells[11].Text.ToString();
            txtAddress.Value = row.Cells[12].Text.ToString();
            ddlTOP.SelectedItem.Text = row.Cells[13].Text.ToString();
            ddlPKP_NonPKP.SelectedItem.Text = row.Cells[14].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[15].Text.ToString());
            txtCreateByNew.Value = row.Cells[16].Text.ToString();
            txtCreateDateNew.Value = row.Cells[17].Text.ToString();
            txtModifiedByNew.Value = row.Cells[18].Text.ToString();
            txtModifiedDateNew.Value = row.Cells[19].Text.ToString();
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("vendor_detail.aspx");
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTOP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPKP_NonPKP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region VendorNumber
        protected void GetVendorNumber()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetVendorNumber");

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

        protected void GetVendorNumberNew()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetVendorNumberNew");

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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateNumbering");
            sqlcomm.Parameters.AddWithValue("@id", Session["IDNew"].ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }
        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetVendorNumber();
            var CurentYear = DateTime.Now.Year;

            UpdateNumbering();
            GetVendorNumberNew();
            hlblast_numberNew.Value = Session["last_numberNew"].ToString();
            int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
            if (_LastNumber < 10)
            {
                txtCode.Value = "V" + "00000" + _LastNumber;
            }
            else if (_LastNumber > 9 && _LastNumber < 99)
            {
                txtCode.Value = "V" + "0000" + _LastNumber;
            }
            else if (_LastNumber > 99 && _LastNumber < 999)
            {
                txtCode.Value = "V" + "000" + _LastNumber;
            }
            else if (_LastNumber > 999 && _LastNumber < 9999)
            {
                txtCode.Value = "V" + "00" + _LastNumber;
            }
            else if (_LastNumber > 9999 && _LastNumber < 99999)
            {
                txtCode.Value = "V" + "0" + _LastNumber;
            }
            else if (_LastNumber > 99999)
            {
                txtCode.Value = "V" + _LastNumber;
            }

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
            sqlcomm.Parameters.AddWithValue("@id_category", ddlCategory.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@vendor_name", txtVendorName.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_pic_name", txtPICNameSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_phone_number", txtPhoneNumberSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_email", txtEmailSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_pic_name", txtPICNameInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_phone_number", txtPhoneNumberInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_email", txtEmailInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@address", txtAddress.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@t_o_p", ddlTOP.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@pkp_nonpkp", ddlPKP_NonPKP.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@createby", txtCreateByNew.Value.Trim());
            //sqlcomm.Parameters.AddWithValue("@create_date", txtCreateDateNew.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@modifiedby", txtModifiedByNew.Value.Trim());
            //sqlcomm.Parameters.AddWithValue("@modified_date", txtModifiedDateNew.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@code", txtCode.Value.Trim());

            sqlcomm.ExecuteNonQuery();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
            Con.Close();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@id", hlbID.Value);
            sqlcomm.Parameters.AddWithValue("@id_category", ddlCategory.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@vendor_name", txtVendorName.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_pic_name", txtPICNameSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_phone_number", txtPhoneNumberSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@sales_email", txtEmailSales.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_pic_name", txtPICNameInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_phone_number", txtPhoneNumberInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@invoice_email", txtEmailInvoice.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@address", txtAddress.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@t_o_p", ddlTOP.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@pkp_nonpkp", ddlPKP_NonPKP.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@modifiedby", Session["Fullname"].ToString());
            //sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

            sqlcomm.ExecuteNonQuery();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            string path = string.Concat(Server.MapPath("~/UploadFile/" + FileUpload.FileName));
            FileUpload.SaveAs(path);

            #region DefineTable
            DataTable dt = new DataTable();
            dt.Columns.Add("id_category", typeof(System.String));
            dt.Columns.Add("vendor_name", typeof(System.String));
            dt.Columns.Add("sales_pic_name", typeof(System.String));
            dt.Columns.Add("sales_phone_number", typeof(System.String));
            dt.Columns.Add("sales_email", typeof(System.String));
            dt.Columns.Add("invoice_pic_name", typeof(System.String));
            dt.Columns.Add("invoice_phone_number", typeof(System.String));
            dt.Columns.Add("invoice_email", typeof(System.String));
            dt.Columns.Add("address", typeof(System.String));
            dt.Columns.Add("t_o_p", typeof(System.String));
            dt.Columns.Add("pkp_nonpkp", typeof(System.String));

            //dt.Columns.Add("createby", typeof(System.String));
            //dt.Columns.Add("create_date", typeof(System.String));
            //dt.Columns.Add("modifiedby", typeof(System.String));
            //dt.Columns.Add("modified_date", typeof(System.String));
            #endregion

            XLWorkbook _vWBUpload = new XLWorkbook(path);
            foreach (IXLWorksheet _vWorksheet in _vWBUpload.Worksheets)
            {
                int _ExcelRow = 2;

                while (_vWorksheet.Cell(_ExcelRow, 1).Value.ToString().Length > 0)
                {

                    DataRow _dr = dt.NewRow();

                    #region DefineValue
                    _dr["id_category"] = _vWorksheet.Cell(_ExcelRow, 1).Value.ToString();
                    _dr["vendor_name"] = _vWorksheet.Cell(_ExcelRow, 2).Value.ToString();
                    _dr["sales_pic_name"] = _vWorksheet.Cell(_ExcelRow, 3).Value.ToString();
                    _dr["sales_phone_number"] = _vWorksheet.Cell(_ExcelRow, 4).Value.ToString();
                    _dr["sales_email"] = _vWorksheet.Cell(_ExcelRow, 5).Value.ToString();
                    _dr["invoice_pic_name"] = _vWorksheet.Cell(_ExcelRow, 6).Value.ToString();
                    _dr["invoice_phone_number"] = _vWorksheet.Cell(_ExcelRow, 7).Value.ToString();
                    _dr["invoice_email"] = _vWorksheet.Cell(_ExcelRow, 8).Value.ToString();
                    _dr["address"] = _vWorksheet.Cell(_ExcelRow, 9).Value.ToString();
                    _dr["t_o_p"] = _vWorksheet.Cell(_ExcelRow, 10).Value.ToString();
                    _dr["pkp_nonpkp"] = _vWorksheet.Cell(_ExcelRow, 11).Value.ToString();

                    #endregion

                    dt.Rows.Add(_dr);

                    _ExcelRow = _ExcelRow + 1;
                }

                string pathdb = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(pathdb);
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;

                Con.Open();

                foreach (DataRow _dr1 in dt.Rows)
                {
                    GetVendorNumber();
                    var CurentYear = DateTime.Now.Year;

                    UpdateNumbering();
                    GetVendorNumberNew();
                    hlblast_numberNew.Value = Session["last_numberNew"].ToString();
                    int _LastNumber = Convert.ToInt32(hlblast_numberNew.Value);
                    if (_LastNumber < 10)
                    {
                        txtCode.Value = "V" + "00000" + _LastNumber;
                    }
                    else if (_LastNumber > 9 && _LastNumber < 99)
                    {
                        txtCode.Value = "V" + "0000" + _LastNumber;
                    }
                    else if (_LastNumber > 99 && _LastNumber < 999)
                    {
                        txtCode.Value = "V" + "000" + _LastNumber;
                    }
                    else if (_LastNumber > 999 && _LastNumber < 9999)
                    {
                        txtCode.Value = "V" + "00" + _LastNumber;
                    }
                    else if (_LastNumber > 9999 && _LastNumber < 99999)
                    {
                        txtCode.Value = "V" + "0" + _LastNumber;
                    }
                    else if (_LastNumber > 99999)
                    {
                        txtCode.Value = "V" + _LastNumber;
                    }

                    #region ParametersValue
                    sqlcomm.Parameters.AddWithValue("@StatementType", "SaveImportExisting");
                    sqlcomm.Parameters.AddWithValue("@id_category", _dr1["id_category"]);
                    sqlcomm.Parameters.AddWithValue("@vendor_name", _dr1["vendor_name"]);
                    sqlcomm.Parameters.AddWithValue("@sales_pic_name", _dr1["sales_pic_name"]);
                    sqlcomm.Parameters.AddWithValue("@sales_phone_number", _dr1["sales_phone_number"]);
                    sqlcomm.Parameters.AddWithValue("@sales_email", _dr1["sales_email"]);
                    sqlcomm.Parameters.AddWithValue("@invoice_pic_name", _dr1["invoice_pic_name"]);
                    sqlcomm.Parameters.AddWithValue("@invoice_phone_number", _dr1["invoice_phone_number"]);
                    sqlcomm.Parameters.AddWithValue("@invoice_email", _dr1["invoice_email"]);
                    sqlcomm.Parameters.AddWithValue("@address", _dr1["address"]);
                    sqlcomm.Parameters.AddWithValue("@t_o_p", _dr1["t_o_p"]);
                    sqlcomm.Parameters.AddWithValue("@pkp_nonpkp", _dr1["pkp_nonpkp"]);
                    sqlcomm.Parameters.AddWithValue("@createby", Session["Fullname"].ToString());
                    sqlcomm.Parameters.AddWithValue("@modifiedby", Session["Fullname"].ToString());
                    sqlcomm.Parameters.AddWithValue("@code", txtCode.Value.ToString());

                    #endregion

                    sqlcomm.ExecuteNonQuery();

                    sqlcomm.Parameters.Clear();

                    GetDataVendor();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "UploadSuccess();", true);


                }
                Con.Close();
                File.Delete(Server.MapPath("~/UploadFile/" + FileUpload.PostedFile.FileName));
            }
        }

        protected void btnCloseModalImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("vendor_detail.aspx");
        }

        protected void DownloadTemplete_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetFileName(Server.MapPath("~/TemplateImportExcel/Template Import Vendor.xlsx"));
            Response.ContentType = "Application/x-msexcel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath("~/TemplateImportExcel/Template Import Vendor.xlsx"));
            Response.End();
        }
    }
}