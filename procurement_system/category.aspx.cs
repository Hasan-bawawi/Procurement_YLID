using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class category : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataTableCategories();
            }
        }

        protected void BindDataTableCategories()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");

            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableCategory.DataSource = dtb;
            TableCategory.DataBind();

            TableCategory.Columns[1].Visible = false;

            TableCategory.UseAccessibleHeader = true;
            TableCategory.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindDataTableCategories();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            txtPartCode.Disabled = true;

            txtCategory.Value = row.Cells[2].Text.ToString();
            txtPartCode.Value = row.Cells[3].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[4].Text.ToString());
            txtID.Value = row.Cells[1].Text.ToString();

            BindDataTableCategories();
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;

        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
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
            Response.Redirect("category.aspx");
        }

        public bool CheckCategoryDuplicate(string category_name)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicateCategory");
            sqlcomm.Parameters.AddWithValue("@category_name", category_name);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("CategoryName", (string)dr["CategoryName"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckPartCodeDuplicate(string part_code)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicatePartCode");
            sqlcomm.Parameters.AddWithValue("@part_code", part_code);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("PartCode", (string)dr["PartCode"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate_category_name = CheckCategoryDuplicate(txtCategory.Value);
            bool isexistsduplicate_part_code = CheckPartCodeDuplicate(txtPartCode.Value);

            if (txtCategory.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FieldCategoryName();", true);
                BindDataTableCategories();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            }
            else if (txtPartCode.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FieldPartCode();", true);
                BindDataTableCategories();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            }
            else if (isexistsduplicate_category_name)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateCategory();", true);
                BindDataTableCategories();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            }
            else if (isexistsduplicate_part_code)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicatePartCode();", true);
                BindDataTableCategories();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddCategory').modal();", true);
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@category_name", txtCategory.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
                sqlcomm.Parameters.AddWithValue("@part_code", txtPartCode.Value);

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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@category_name", txtCategory.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@part_code", txtPartCode.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@id", txtID.Value);

            sqlcomm.ExecuteNonQuery();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();
        }
    }
}