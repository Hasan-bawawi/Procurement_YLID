using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace procurement_system
{
    public partial class merk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataTableMerks();
            }
        }

        protected void BindDataTableMerks()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");

            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableMerk.DataSource = dtb;
            TableMerk.DataBind();

            TableMerk.Columns[1].Visible = false;

            TableMerk.UseAccessibleHeader = true;
            TableMerk.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindDataTableMerks();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddMerk').modal();", true);
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImportExcel').modal();", true);
            BindDataTableMerks();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddMerk').modal();", true);
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtMerk.Value = row.Cells[2].Text.ToString();
            txtCode.Value = row.Cells[1].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[3].Text.ToString());

            BindDataTableMerks();
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;

        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "Delete");
        //    sqlcomm.Parameters.AddWithValue("@id", row.Cells[1].Text.ToString());

        //    sqlcomm.ExecuteNonQuery();
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncDelete();", true);

        //    Con.Close();
        //}

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("merk.aspx");
        }

        public bool CheckMerkNameDuplicate(string merk_name)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicateMerkName");
            sqlcomm.Parameters.AddWithValue("@merk_name", merk_name);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("MerkName", (string)dr["MerkName"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate_merk_name = CheckMerkNameDuplicate(txtMerk.Value);

            if (txtMerk.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FieldMerkName();", true);
                BindDataTableMerks();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddMerk').modal();", true);
            }
            else if (isexistsduplicate_merk_name)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateMerkName();", true);
                BindDataTableMerks();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddMerk').modal();", true);
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@merk_name", txtMerk.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);

                sqlcomm.ExecuteNonQuery();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                Con.Close();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@id", txtCode.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@merk_name", txtMerk.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);

            sqlcomm.ExecuteNonQuery();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();
        }

        protected void DownloadTemplete_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetFileName(Server.MapPath("~/TemplateImportExcel/Templete Merk.xlsx"));
            Response.ContentType = "Application/x-msexcel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath("~/TemplateImportExcel/Templete Merk.xlsx"));
            Response.End();
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            string path = string.Concat(Server.MapPath("~/UploadFile/" + FileUpload.FileName));
            FileUpload.SaveAs(path);

            #region DefineTable
            DataTable dt = new DataTable();
            dt.Columns.Add("merk_name", typeof(System.String));
            #endregion

            XLWorkbook _vWBUpload = new XLWorkbook(path);
            foreach (IXLWorksheet _vWorksheet in _vWBUpload.Worksheets)
            {
                int _ExcelRow = 2;

                while (_vWorksheet.Cell(_ExcelRow, 1).Value.ToString().Length > 0)
                {

                    DataRow _dr = dt.NewRow();

                    #region DefineValue
                    _dr["merk_name"] = _vWorksheet.Cell(_ExcelRow, 1).Value.ToString();

                    #endregion

                    dt.Rows.Add(_dr);

                    _ExcelRow = _ExcelRow + 1;
                }

                string pathdb = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(pathdb);
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Merk";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;

                Con.Open();

                foreach (DataRow _dr1 in dt.Rows)
                {
                    #region ParametersValue
                    sqlcomm.Parameters.AddWithValue("@StatementType", "SaveImportExisting");
                    sqlcomm.Parameters.AddWithValue("@merk_name", _dr1["merk_name"]);

                    #endregion

                    sqlcomm.ExecuteNonQuery();

                    sqlcomm.Parameters.Clear();

                    BindDataTableMerks();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "UploadSuccess();", true);


                }
                Con.Close();
                File.Delete(Server.MapPath("~/UploadFile/" + FileUpload.PostedFile.FileName));
            }
        }

        protected void btnCloseModalImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("merk.aspx");
        }
    }
}