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
    public partial class requisition_price_check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hblNIK.Text = Session["nik"].ToString();
            lblNamaBranch.Text = Session["Location"].ToString();
            if (!Page.IsPostBack)
            {
                GetTableRFPriceEstimated();
                //GetTableRFPriceFixed();
            }
        }

        protected void GetTableRFPriceEstimated()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewRFCheckPriceEstimate");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableRequisitionFormPriceEstimated.DataSource = dtb;
            TableRequisitionFormPriceEstimated.DataBind();

            TableRequisitionFormPriceEstimated.Columns[1].Visible = false;
            TableRequisitionFormPriceEstimated.Columns[11].Visible = false;

            TableRequisitionFormPriceEstimated.UseAccessibleHeader = true;
            TableRequisitionFormPriceEstimated.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        //protected void GetTableRFPriceFixed()
        //{
        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "ViewRFCheckPriceFixed");
        //    DataTable dtb = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

        //    sda.Fill(dtb);
        //    ViewState["myViewState"] = dtb;
        //    TableRequisitionFormPriceFixed.DataSource = dtb;
        //    TableRequisitionFormPriceFixed.DataBind();

        //    TableRequisitionFormPriceFixed.Columns[1].Visible = false;
        //    TableRequisitionFormPriceFixed.Columns[16].Visible = false;

        //    TableRequisitionFormPriceFixed.UseAccessibleHeader = true;
        //    TableRequisitionFormPriceFixed.HeaderRow.TableSection = TableRowSection.TableHeader;

        //    Con.Close();
        //}

        protected void btnPriceEstimate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetTableRFPriceEstimated();
            //GetTableRFPriceFixed();

            if (row.Cells[2].Text == "")
            {
                Response.Redirect("input_price.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("input_price.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }

        protected void btnPriceFixed_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            GetTableRFPriceEstimated();
            //GetTableRFPriceFixed();

            if (row.Cells[2].Text == "")
            {
                Response.Redirect("input_price_fixed.aspx?rf_no=" + 0);
            }
            else
            {
                Response.Redirect("input_price_fixed.aspx?rf_no=" + (row.Cells[2].Text));
            }
        }
    }
}