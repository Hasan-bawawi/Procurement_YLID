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
using ListItem = System.Web.UI.WebControls.ListItem;
using TableCell = System.Web.UI.WebControls.TableCell;

namespace procurement_system
{
    public partial class stock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNamaBranch.Text = Session["Location"].ToString();
            if (!IsPostBack)
            {
                BindDataTableGoodsStock();
                GetItemNameData();
                GetLocationData();
            }
        }

        protected void BindDataTableGoodsStock()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableGoodsStock.DataSource = dtb;
            TableGoodsStock.DataBind();

            TableGoodsStock.Columns[1].Visible = false;
            TableGoodsStock.Columns[14].Visible = false;

            TableGoodsStock.UseAccessibleHeader = true;
            TableGoodsStock.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void GetItemNameData()
        {
            ddlItemName.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddItemName");

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<<Code-Category-Item-Merk-Type>>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlItemName.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["item_code"].ToString() + "-" + dr["Category"].ToString() + "-" + dr["Item"].ToString() + "-" + dr["ItemMerk"].ToString() + "-" + dr["tipe"].ToString();
                    newItem.Value = dr["item_code"].ToString();
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

        protected void GetLocationData()
        {
            ddlLocation.Items.Clear();
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);

            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "AddLocation");

            SqlDataReader dr;

            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select Location>";
                newItem.Value = "00000000-0000-0000-0000-000000000000";
                ddlLocation.Items.Add(newItem);

                Con.Open();
                dr = sqlcomm.ExecuteReader();

                while (dr.Read())
                {
                    newItem = new ListItem();
                    newItem.Text = dr["branch_name"].ToString();
                    newItem.Value = dr["code_branch"].ToString();
                    ddlLocation.Items.Add(newItem);
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
            BindDataTableGoodsStock();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            TxtItem.Visible = false;
            ddlItem.Visible = true;
            TxtStock.Visible = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            ddlItem.Visible = false;
            ddlLocation.Enabled = false;
            TxtItem.Visible = true;
            
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            BindDataTableGoodsStock();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);

            txtItemName.Value = row.Cells[5].Text.ToString() + "-" + row.Cells[3].Text.ToString() + "-" + row.Cells[6].Text.ToString() + "-" + row.Cells[7].Text.ToString() + "-" + row.Cells[8].Text.ToString();
            txtCode.Value = row.Cells[5].Text.ToString();
            txtMinimumStock.Value = row.Cells[11].Text.ToString();
            txtJmlhStock.Value = row.Cells[10].Text.ToString();
            ddlLocation.SelectedItem.Text = row.Cells[13].Text.ToString();
            lblID.Text = row.Cells[1].Text.ToString();

        }

        protected void GetDetailItems()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "DetailItemName");
            sqlcomm.Parameters.AddWithValue("@item_code", ddlItemName.SelectedValue);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("id", (string)dr["id"].ToString());
                Session.Add("id_category", (string)dr["id_category"].ToString());
                Session.Add("Category", (string)dr["Category"].ToString());
                Session.Add("ItemCodeDetail", (string)dr["ItemCodeDetail"].ToString());
                Session.Add("id_item", (string)dr["id_item"].ToString());
                Session.Add("Item", (string)dr["Item"].ToString());
            }
            else
            {

            }
        }

        protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataTableGoodsStock();
            GetDetailItems();
            txtCode.Value = Session["ItemCodeDetail"].ToString();
            hlbIdItem.Value = Session["id_item"].ToString();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GetStockCode()
        {
            int a;
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            string query = "select count(item_code) from GoodsStock";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string val = dr[0].ToString();
                var year = DateTime.Now.Year;
                if (val == "")
                {
                    hlbStockCode.Value = "MST" + "0000";
                }
                else
                {
                    a = Convert.ToInt32(dr[0].ToString());
                    a = a + 1;
                    if (a < 10)
                    {
                        hlbStockCode.Value = "MST" + "000" + a.ToString();
                    }
                    else if (a > 9)
                    {
                        hlbStockCode.Value = "MST" + "00" + a.ToString();
                    }
                    else if (a > 99)
                    {
                        hlbStockCode.Value = "MST" + "0" + a.ToString();
                    }
                    else if (a > 999)
                    {
                        hlbStockCode.Value = "MST" + a.ToString();
                    }
                }
            }
        }

        public bool CheckItemDuplicate(string item_code, string nama_branch)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDuplicateItem");
            sqlcomm.Parameters.AddWithValue("@item_code", item_code);
            sqlcomm.Parameters.AddWithValue("@nama_branch", nama_branch);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("item_code_cek_duplicate", (string)dr["item_code_cek_duplicate"].ToString());
                Session.Add("nama_branch_cek_duplicate", (string)dr["nama_branch_cek_duplicate"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsduplicate = CheckItemDuplicate(txtCode.Value, ddlLocation.SelectedValue);
            if (isexistsduplicate)
            {
                BindDataTableGoodsStock();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "CheckDuplicate();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddItem').modal();", true);
            }
            else
            {
                GetStockCode();
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@stok_code", hlbStockCode.Value);
                sqlcomm.Parameters.AddWithValue("@item_code", txtCode.Value);
                sqlcomm.Parameters.AddWithValue("@id_item", hlbIdItem.Value);
                sqlcomm.Parameters.AddWithValue("@quantity", "0");
                sqlcomm.Parameters.AddWithValue("@minimal_stok", txtMinimumStock.Value);
                sqlcomm.Parameters.AddWithValue("@nama_branch", ddlLocation.SelectedValue);

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
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_GoodsStock";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
            sqlcomm.Parameters.AddWithValue("@id", lblID.Text);
            sqlcomm.Parameters.AddWithValue("@minimal_stok", txtMinimumStock.Value);
            sqlcomm.Parameters.AddWithValue("@quantity", txtJmlhStock.Value);

            sqlcomm.ExecuteNonQuery();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);
            Con.Close();

        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("stock.aspx");
        }

        protected void TableGoodsStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int32 qty_stock;
                Int32 min_stock;
                qty_stock = Convert.ToInt32(e.Row.Cells[10].Text.ToString());
                min_stock = Convert.ToInt32(e.Row.Cells[11].Text.ToString());
                TableCell statusCell = e.Row.Cells[2];

                if (qty_stock == 0)
                {
                    statusCell.Text = "No Stock";
                    statusCell.ControlStyle.Font.Bold = true;
                    statusCell.ControlStyle.ForeColor = System.Drawing.Color.White;
                    statusCell.BackColor = System.Drawing.Color.Red;

                    e.Row.Cells[3].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[4].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[5].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[6].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[7].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[8].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[9].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[10].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[11].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[12].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[13].ControlStyle.ForeColor = System.Drawing.Color.Red;

                    e.Row.Cells[14].ControlStyle.ForeColor = System.Drawing.Color.Red;
                }
                else if (qty_stock >= min_stock)
                {
                    statusCell.Text = "Good";
                    statusCell.ControlStyle.Font.Bold = true;
                    statusCell.ControlStyle.ForeColor = System.Drawing.Color.White;
                    statusCell.BackColor = System.Drawing.Color.Green;

                    e.Row.Cells[3].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[4].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[5].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[6].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[7].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[8].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[9].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[10].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[11].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[12].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[13].ControlStyle.ForeColor = System.Drawing.Color.Green;

                    e.Row.Cells[14].ControlStyle.ForeColor = System.Drawing.Color.Green;
                }
                else if (qty_stock < min_stock)
                {
                    statusCell.Text = "Low Stock";
                    statusCell.ControlStyle.Font.Bold = true;
                    statusCell.ControlStyle.ForeColor = System.Drawing.Color.White;
                    statusCell.BackColor = System.Drawing.Color.Orange;

                    e.Row.Cells[3].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[4].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[5].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[6].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[7].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[8].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[9].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[10].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[11].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[12].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[13].ControlStyle.ForeColor = System.Drawing.Color.Orange;

                    e.Row.Cells[14].ControlStyle.ForeColor = System.Drawing.Color.Orange;
                }
            }
        }
    }
}