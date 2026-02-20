using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace procurement_system
{
    public partial class goods_catalog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNamaBranch.Text = Session["Location"].ToString();

            if (!IsPostBack)
            {
                GetKategoriData();
                GetItemNameData();
                GetMerkData();
                GetUnitData();
                BindDataTableCatalog();
            }
        }

        protected void BindDataTableCatalog()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableCatalog.DataSource = dtb;
            TableCatalog.DataBind();

            TableCatalog.Columns[1].Visible = false;
            TableCatalog.Columns[11].Visible = false;
            TableCatalog.Columns[12].Visible = false;
            TableCatalog.Columns[13].Visible = false;
            TableCatalog.Columns[14].Visible = false;
            TableCatalog.Columns[15].Visible = false;

            TableCatalog.UseAccessibleHeader = true;
            TableCatalog.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void GetKategoriData()
        {
            ddlKategori.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddCategories");

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select Categories>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlKategori.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["category_name"].ToString();
                    newItem.Value = dr["id"].ToString();
                    ddlKategori.Items.Add(newItem);
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

        protected void GetItemNameData()
        {
            ddlItemName.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddItemName");

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select Item>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlItemName.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["item_name"].ToString();
                    newItem.Value = dr["id"].ToString();
                    ddlItemName.Items.Add(newItem);
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

        protected void GetMerkData()
        {
            ddlMerk.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddMerk");

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select merk>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlMerk.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["merk_name"].ToString();
                    newItem.Value = dr["id"].ToString();
                    ddlMerk.Items.Add(newItem);
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

        protected void GetUnitData()
        {
            ddlUnit.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddUnit");

            SqlDataReader dr;


            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlUnit.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["unit_name"].ToString();
                    newItem.Value = dr["id"].ToString();
                    ddlUnit.Items.Add(newItem);
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

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            BindDataTableCatalog();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            txtType.Disabled = false;

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            GetUnitData();
            GetKategoriData();
            GetItemNameData();
            GetMerkData();
            ddlItemName.SelectedItem.Text = row.Cells[5].Text.ToString();
            ddlItemName.SelectedValue = row.Cells[12].Text.ToString();
            txtCode.Value = row.Cells[4].Text.ToString();
            ddlMerk.SelectedItem.Text = row.Cells[6].Text.ToString();
            ddlMerk.SelectedValue = row.Cells[13].Text.ToString();
            txtType.Value = row.Cells[7].Text.ToString();
            ddlKategori.SelectedItem.Text = row.Cells[3].Text.ToString();
            ddlKategori.SelectedValue = row.Cells[11].Text.ToString();
            ddlCriteria.SelectedItem.Text = row.Cells[9].Text.ToString();
            ddlUnit.SelectedItem.Text = row.Cells[8].Text;
            ddlUnit.SelectedValue = row.Cells[14].Text;
            ckActive.Checked = Convert.ToBoolean(row.Cells[10].Text.ToString());
            ddlStatusAsset.SelectedItem.Text = row.Cells[16].Text;
            ddlCatalogType.SelectedItem.Text = row.Cells[2].Text;

            ddlKategori.Enabled = false;
            ddlItemName.Enabled = false;
            ddlCatalogType.Enabled = false;

            BindDataTableCatalog();
            
        }

        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.NamingContainer;

        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "Delete");
        //    sqlcomm.Parameters.AddWithValue("@kode_barang", row.Cells[2].Text);

        //    sqlcomm.ExecuteNonQuery();
        //    Con.Close();
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncDelete();", true);
        //}

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("goods_catalog.aspx");
        }

        protected void GetPartCode()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "GetPartCode");
            sqlcomm.Parameters.AddWithValue("@id", ddlKategori.SelectedValue);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("part_code", (string)dr["part_code"]);
            }
            else
            {

            }
        }

        protected void ddlKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            GetPartCode();
            int _vPart;

            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CountPartCode");
            sqlcomm.Parameters.AddWithValue("@item_code", Session["part_code"].ToString());

            sqlcomm.ExecuteNonQuery();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                string val = dr[0].ToString();
                if (val == "")
                {
                    txtCode.Value = Session["part_code"].ToString() + "0000";
                }
                else
                {
                    int _vGetCount = Convert.ToInt32(dr[0].ToString());
                    _vPart = _vGetCount + 1;
                    if (_vPart < 10)
                    {
                        txtCode.Value = Session["part_code"].ToString() + "000" + _vPart.ToString();
                    }
                    else if (_vPart > 9 && _vPart < 99)
                    {
                        txtCode.Value = Session["part_code"].ToString() + "00" + _vPart.ToString();
                    }
                    else if (_vPart > 99 && _vPart < 999)
                    {
                        txtCode.Value = Session["part_code"].ToString() + "0" + _vPart.ToString();
                    }
                    else if (_vPart > 999)
                    {
                        txtCode.Value = Session["part_code"].ToString() + _vPart.ToString();
                    }
                }
            }
        }

        protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMerk_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void SaveDataBarang()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
            sqlcomm.Parameters.AddWithValue("@id_category", ddlKategori.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@item_code", txtCode.Value);
            sqlcomm.Parameters.AddWithValue("@id_item", ddlItemName.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@id_merk", ddlMerk.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@id_unit", ddlUnit.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@tipe", txtType.Value);
            sqlcomm.Parameters.AddWithValue("@criteria_stock", ddlCriteria.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@status_asset", ddlStatusAsset.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@catalog_type", ddlCatalogType.SelectedItem.Text.ToString());

            sqlcomm.ExecuteNonQuery();

            Con.Close();
        }

        public bool CheckCodeItemDuplicate(string ItemCode)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicateItemCode");
            sqlcomm.Parameters.AddWithValue("@item_code", ItemCode);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("ItemCode", (string)dr["ItemCode"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlKategori.SelectedItem.Text == "<Select Categories>")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectCategory();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (ddlItemName.SelectedItem.Text == "<Select Item>")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectItem();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (ddlMerk.SelectedItem.Text == "<Select merk>")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Selectmerk();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (txtType.Value == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FieldType();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (ddlCriteria.SelectedItem.Text == "Select Criteria")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectStockCriteria();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else if (ddlCatalogType.SelectedItem.Text == "Select Type")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "SelectCatalogType();", true);
                BindDataTableCatalog();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }            
            else
            {
                bool isexistsduplicate_item_code = CheckCodeItemDuplicate(txtCode.Value);
                if (isexistsduplicate_item_code)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "DuplicateCode();", true);
                    BindDataTableCatalog();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
                }
                else
                {
                    SaveDataBarang();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsCatalog";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@item_code", txtCode.Value);
            sqlcomm.Parameters.AddWithValue("@id_item", ddlItemName.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@id_merk", ddlMerk.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@id_unit", ddlUnit.SelectedValue);
            sqlcomm.Parameters.AddWithValue("@tipe", txtType.Value);
            sqlcomm.Parameters.AddWithValue("@criteria_stock", ddlCriteria.SelectedItem.Text.ToString());
            sqlcomm.Parameters.AddWithValue("@active", ckActive.Checked);
            sqlcomm.Parameters.AddWithValue("@status_asset", ddlStatusAsset.SelectedItem.Text.ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
        }

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCatalogType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}