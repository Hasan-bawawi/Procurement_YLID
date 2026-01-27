using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class vendor_category : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataCategory();
            }
        }

        protected void GetDataCategory()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableVendorCategory.DataSource = dtb;
            TableVendorCategory.DataBind();

            TableVendorCategory.Columns[1].Visible = false;

            TableVendorCategory.UseAccessibleHeader = true;
            TableVendorCategory.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewCategory').modal();", true);
            GetDataCategory();
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            txtCreateByNew.Value = Session["Fullname"].ToString();
            txtCreateDateNew.Value = DateTime.Now.ToString();
            txtModifiedByNew.Value = Session["Fullname"].ToString();
            txtModifiedDateNew.Value = DateTime.Now.ToString();
        }

        protected void TableVendorCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableVendorCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableVendorCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewCategory').modal();", true);
            GetDataCategory();

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            btnSubmit.Visible = false;
            btnUpdate.Visible = true;

            hlbID.Value = row.Cells[1].Text.ToString();
            txtCategory.Value = row.Cells[2].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[3].Text.ToString());
            txtCreateByNew.Value = row.Cells[4].Text.ToString();
            txtCreateDateNew.Value = row.Cells[5].Text.ToString();
            txtModifiedByNew.Value = row.Cells[6].Text.ToString();
            txtModifiedDateNew.Value = row.Cells[7].Text.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isCategory_name_duplicate = CheckCategoryNameDuplicate(txtCategory.Value);

            if (txtCategory.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "EmptyFieldCategoryName();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewCategory').modal();", true);
                GetDataCategory();
            }
            else if (isCategory_name_duplicate)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateDataCategoryName();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlNewCategory').modal();", true);
                GetDataCategory();
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories_Vendor";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@category_vendor", txtCategory.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
                sqlcomm.Parameters.AddWithValue("@createby", txtCreateByNew.Value.Trim());
                //sqlcomm.Parameters.AddWithValue("@create_date", txtCreateDateNew.Value.Trim());
                sqlcomm.Parameters.AddWithValue("@modifiedby", txtModifiedByNew.Value.Trim());
                //sqlcomm.Parameters.AddWithValue("@modified_date", txtModifiedDateNew.Value.Trim());

                sqlcomm.ExecuteNonQuery();
                GetDataCategory();
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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Categories_Vendor";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@id", hlbID.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@category_vendor", txtCategory.Value.Trim());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@modifiedby", Session["Fullname"].ToString());
            //sqlcomm.Parameters.AddWithValue("@modified_date", DateTime.Now.ToString());

            sqlcomm.ExecuteNonQuery();
            GetDataCategory();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();
        }

        public bool CheckCategoryNameDuplicate(string category_name)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string sqlquery = "SELECT category_vendor FROM CategoryVendor WHERE category_vendor=@category_vendor";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, Con);
            sqlcomm.Parameters.AddWithValue("@category_vendor", category_name);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {

                Session.Add("category_vendor", (string)dr["category_vendor"]);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("vendor_category.aspx");
        }
    }
}