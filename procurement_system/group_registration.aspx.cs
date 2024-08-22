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
    public partial class group_registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataGroup();
                btnRemove_navigation.Attributes.Add("onclick", "javascript:return DeleteConfirm()");
            }
        }

        protected void GetDataGroup()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Group";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableGroup.DataSource = dtb;
            TableGroup.DataBind();

            TableGroup.Columns[2].Visible = false;
            TableGroup.Columns[3].Visible = false;

            TableGroup.UseAccessibleHeader = true;
            TableGroup.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewGroup').modal();", true);
            GetDataGroup();
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            int a;
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string query = "select COUNT(Code) from UserManagement_Group";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string val = dr[0].ToString();
                if (val == "")
                {
                    hlbCode.Value = "Group01";
                }
                else
                {
                    a = Convert.ToInt32(dr[0].ToString());
                    a = a + 1;
                    hlbCode.Value = "Group0" + a.ToString();
                }
            }
        }

        protected void TableGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewGroup').modal();", true);
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            GetDataGroup();
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            hlbCode.Value = row.Cells[3].Text.ToString();
            txtGroupName.Value = row.Cells[4].Text.ToString();
            hlbOid.Value = row.Cells[2].Text.ToString();

            ckActive.Checked = Convert.ToBoolean(row.Cells[5].Text.ToString());
            GetDataGroup();
        }

        protected void btnShowNavigation_Click(object sender, EventArgs e)
        {
            btnSubmit_Navigation.Visible = true;
            btnUpdate_Navigation.Visible = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewNavigationRole').modal();", true);
            GetDataGroup();
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            txtGroupName_Navigation.Value = row.Cells[4].Text.ToString();
            hlbOid.Value = row.Cells[2].Text.ToString();
            GetModuleName();
            GetDataNavigationRole();
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("group_registration.aspx");
        }

        public bool CheckGroupNameDuplicate(string group_name)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string sqlquery = "SELECT GroupName FROM UserManagement_Group WHERE GroupName=@group_name";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, Con);
            sqlcomm.Parameters.AddWithValue("@group_name", group_name);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {

                Session.Add("GroupName", (string)dr["GroupName"]);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate = CheckGroupNameDuplicate(txtGroupName.Value);

            if (txtGroupName.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "ErorrEmptyGroupname();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewGroup').modal();", true);
                GetDataGroup();
            }
            else if (isexistsduplicate)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateGroupName();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewGroup').modal();", true);
                GetDataGroup();
            }
            else
            {

                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Group";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@CreateBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@Code", hlbCode.Value);
                sqlcomm.Parameters.AddWithValue("@GroupName", txtGroupName.Value);
                sqlcomm.Parameters.AddWithValue("@IsActive", ckActive.Checked);

                sqlcomm.ExecuteNonQuery();
                Con.Close();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                GetDataGroup();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_Group";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@Oid", hlbOid.Value.ToString());
            sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
            sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
            sqlcomm.Parameters.AddWithValue("@GroupName", txtGroupName.Value);
            sqlcomm.Parameters.AddWithValue("@IsActive", ckActive.Checked);

            sqlcomm.ExecuteNonQuery();
            Con.Close();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            GetDataGroup();
        }

        protected void GetDataNavigationRole()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_AccessGroup_DetailNavigation";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewRoleNavigation");
            sqlcomm.Parameters.AddWithValue("@Oid_UserManagement_Group", hlbOid.Value);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRoleNavigation.DataSource = dtb;
            TableRoleNavigation.DataBind();

            TableRoleNavigation.Columns[1].Visible = false;

            TableRoleNavigation.UseAccessibleHeader = true;
            TableRoleNavigation.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void GetModuleName()
        {
            ddlModuleName.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_AccessGroup_DetailNavigation";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewModuleName");

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlModuleName.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["ModuleName"].ToString() + " (" + dr["Type"].ToString() + ")";
                    newItem.Value = dr["Oid"].ToString();
                    ddlModuleName.Items.Add(newItem);
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

        public bool CheckNavigationModuleDuplicate(string module, string group)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string sqlquery = "select * from UserManagement_AccessGroup_DetailNavigation where ModuleName=@ModuleName and UserManagement_Group=@UserManagement_Group";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, Con);
            sqlcomm.Parameters.AddWithValue("@ModuleName", module);
            sqlcomm.Parameters.AddWithValue("@UserManagement_Group", group);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {

                Session.Add("ModuleName", (Guid)dr["ModuleName"]);
                Session.Add("UserManagement_Group", (Guid)dr["UserManagement_Group"]);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnCloseModalNew_Navigation_Click(object sender, EventArgs e)
        {
            Response.Redirect("group_registration.aspx");
        }

        protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Navigation_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate = CheckNavigationModuleDuplicate(ddlModuleName.SelectedValue, hlbOid.Value);
            if (isexistsduplicate)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Savedfailed();", true);
            }
            else if (ddlModuleName.SelectedValue == "00000000-0000-0000-0000-000000000000")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectModule();", true);
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_AccessGroup_DetailNavigation";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@CreateBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
                sqlcomm.Parameters.AddWithValue("@Oid_ModuleName", ddlModuleName.SelectedValue);
                sqlcomm.Parameters.AddWithValue("@Oid_UserManagement_Group", hlbOid.Value);
                sqlcomm.Parameters.AddWithValue("@IsActive", ckActive_Navigation.Checked);
                sqlcomm.Parameters.AddWithValue("@RoleNavigation", ckActiveNavigationRole.Checked);

                sqlcomm.ExecuteNonQuery();
                Con.Close();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSaveNavigation();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewNavigationRole').modal();", true);
                GetModuleName();
                GetDataNavigationRole();
                GetDataGroup();
            }
        }

        protected void btnUpdate_Navigation_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_AccessGroup_DetailNavigation";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@ModifiedBy", Session["Fullname"].ToString());
            sqlcomm.Parameters.AddWithValue("@ModifiedDate", DateTime.Now.ToString());
            sqlcomm.Parameters.AddWithValue("@Oid", hlbOidModule.Value);
            sqlcomm.Parameters.AddWithValue("@IsActive", ckActive_Navigation.Checked);
            sqlcomm.Parameters.AddWithValue("@RoleNavigation", ckActiveNavigationRole.Checked);

            sqlcomm.ExecuteNonQuery();
            Con.Close();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdateNavigation();", true);
        }

        protected void DeleteRecord(string oid)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement_AccessGroup_DetailNavigation";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Delete");
            sqlcomm.Parameters.AddWithValue("@Oid", oid);

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected void btnRemove_navigation_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (GridViewRow grow in TableRoleNavigation.Rows)
            {
                CheckBox chkdel = (CheckBox)grow.FindControl("chkSelect");
                if (chkdel.Checked)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                foreach (GridViewRow grow in TableRoleNavigation.Rows)
                {
                    CheckBox chkdel = (CheckBox)grow.FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        string oid = grow.Cells[1].Text;
                        DeleteRecord(oid);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncDeleteNavigation();", true);
                    }
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Deletefailed();", true);
            }
        }

        protected void TableRoleNavigation_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableRoleNavigation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnEditModule_Click(object sender, EventArgs e)
        {
            btnSubmit_Navigation.Visible = false;
            btnUpdate_Navigation.Visible = true;
            ddlModuleName.Enabled = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewNavigationRole').modal();", true);
            GetDataGroup();
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            hlbOidModule.Value = row.Cells[1].Text.ToString();
            ddlModuleName.SelectedItem.Text = row.Cells[3].Text.ToString() + " (" + row.Cells[4].Text.ToString() + ")";
            ckActive_Navigation.Checked = Convert.ToBoolean(row.Cells[6].Text.ToString());
            ckActiveNavigationRole.Checked = Convert.ToBoolean(row.Cells[5].Text.ToString());
            //hlbOid.Value = row.Cells[2].Text.ToString();
            GetDataNavigationRole();
        }
    }
}