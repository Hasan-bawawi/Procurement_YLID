using ClosedXML.Excel;
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

namespace procurement_system
{
    public partial class item : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataTableItems();
            }
        }

        protected void BindDataTableItems()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");

            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableItems.DataSource = dtb;
            TableItems.DataBind();

            TableItems.Columns[1].Visible = false;

            TableItems.UseAccessibleHeader = true;
            TableItems.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindDataTableItems();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            txtItem.Value = row.Cells[2].Text.ToString();
            txtCode.Value = row.Cells[1].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[3].Text.ToString());

            BindDataTableItems();
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;

        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
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
            Response.Redirect("item.aspx");
        }

        public bool CheckItemNameDuplicate(string ItemName)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicateItemName");
            sqlcomm.Parameters.AddWithValue("@item_name", ItemName);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("ItemName", (string)dr["ItemName"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate_item_name = CheckItemNameDuplicate(txtItem.Value);

            if (txtItem.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FieldItemName();", true);
                BindDataTableItems();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (isexistsduplicate_item_name)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateItemName();", true);
                BindDataTableItems();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@item_name", txtItem.Value.Trim());
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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@id", txtCode.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@item_name", txtItem.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);

            sqlcomm.ExecuteNonQuery();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImportExcel').modal();", true);
            BindDataTableItems();
        }

        protected void DownloadTemplete_Click(object sender, EventArgs e)
        {
            string filePath = Path.GetFileName(Server.MapPath("~/TemplateImportExcel/Templete Item.xlsx"));
            Response.ContentType = "Application/x-msexcel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath("~/TemplateImportExcel/Templete Item.xlsx"));
            Response.End();
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            string path = string.Concat(Server.MapPath("~/UploadFile/" + FileUpload.FileName));
            FileUpload.SaveAs(path);

            #region DefineTable
            DataTable dt = new DataTable();
            dt.Columns.Add("item_name", typeof(System.String));
            #endregion

            XLWorkbook _vWBUpload = new XLWorkbook(path);
            foreach (IXLWorksheet _vWorksheet in _vWBUpload.Worksheets)
            {
                int _ExcelRow = 2;

                while (_vWorksheet.Cell(_ExcelRow, 1).Value.ToString().Length > 0)
                {

                    DataRow _dr = dt.NewRow();

                    #region DefineValue
                    _dr["item_name"] = _vWorksheet.Cell(_ExcelRow, 1).Value.ToString();
                    
                    #endregion

                    dt.Rows.Add(_dr);

                    _ExcelRow = _ExcelRow + 1;
                }

                string pathdb = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(pathdb);
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Items";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;

                Con.Open();

                foreach (DataRow _dr1 in dt.Rows)
                {
                    #region ParametersValue
                    sqlcomm.Parameters.AddWithValue("@StatementType", "SaveImportExisting");
                    sqlcomm.Parameters.AddWithValue("@item_name", _dr1["item_name"]);

                    #endregion

                    sqlcomm.ExecuteNonQuery();

                    sqlcomm.Parameters.Clear();

                    BindDataTableItems();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "UploadSuccess();", true);


                }
                Con.Close();
                File.Delete(Server.MapPath("~/UploadFile/" + FileUpload.PostedFile.FileName));
            }
        }

        protected void btnCloseModalImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("item.aspx");
        }
    }
}