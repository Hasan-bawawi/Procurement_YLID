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
    public partial class module_registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataModule();
            }
        }

        protected void GetDataModule()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Module";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableModule.DataSource = dtb;
            TableModule.DataBind();

            TableModule.Columns[1].Visible = false;

            TableModule.UseAccessibleHeader = true;
            TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewModule').modal();", true);
            GetDataModule();
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        protected void TableModule_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableModule_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewModule').modal();", true);
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            GetDataModule();
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            hlbOid.Value = row.Cells[1].Text.ToString();
            txtModuleName.Value = row.Cells[2].Text.ToString();
            txtModuleID.Value = row.Cells[3].Text.ToString();
            ddlType.SelectedItem.Text = row.Cells[4].Text.ToString();
            ckActive.Checked = Convert.ToBoolean(row.Cells[5].Text.ToString());
            GetDataModule();
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("module_registration.aspx");
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtModuleName.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "ErorrEmptyModulename();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewModule').modal();", true);
                GetDataModule();
            }
            else if (txtModuleID.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "ErorrEmptyModulID();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewModule').modal();", true);
                GetDataModule();
            }
            else if (ddlType.SelectedItem.Text == "<Select Type>")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "ErorrEmptyType();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewModule').modal();", true);
                GetDataModule();
            }
            else
            {

                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Module";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@CreateBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@ModuleName", txtModuleName.Value);
                sqlcomm.Parameters.AddWithValue("@ModuleNameID", txtModuleID.Value);
                sqlcomm.Parameters.AddWithValue("@IsActive", ckActive.Checked);
                sqlcomm.Parameters.AddWithValue("@Type", ddlType.SelectedItem.Text.ToString());

                sqlcomm.ExecuteNonQuery();
                Con.Close();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                GetDataModule();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Module";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@Oid", hlbOid.Value.ToString());
            sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
            sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
            sqlcomm.Parameters.AddWithValue("@ModuleName", txtModuleName.Value);
            sqlcomm.Parameters.AddWithValue("@ModuleNameID", txtModuleID.Value);
            sqlcomm.Parameters.AddWithValue("@IsActive", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@Type", ddlType.SelectedItem.Text.ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            GetDataModule();
        }
    }
}