using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.Identity.Client;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using ZXing;
using System.Threading.Tasks;
using System.Text;

namespace procurement_system
{
    public partial class approval_purchase_order_view : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];

        public class FileAttachment
        {
            [JsonProperty("@odata.type")]
            public string Type { get; set; }

            public string Name { get; set; }
            public string ContentBytes { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string po_no = Request.QueryString["po_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailPurchaseOrder");
                sqlcomm.Parameters.AddWithValue("@po_no", po_no);
                con.Open();
                using (SqlDataReader rdr = sqlcomm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Session.Add("po_no", (string)rdr["po_no"]);
                        Session.Add("rf_no", (string)rdr["rf_no"]);
                        Session.Add("po_type", (string)rdr["po_type"]);
                        Session.Add("vendor_name", (string)rdr["vendor_name"]);
                        Session.Add("po_date", (DateTime)rdr["po_date"]);
                        Session.Add("delivery_date", (DateTime)rdr["delivery_date"]);
                        Session.Add("delivery_to", (string)rdr["delivery_to"]);
                        Session.Add("item_code", (string)rdr["item_code"]);
                        Session.Add("item_name", (string)rdr["item_name"]);
                        Session.Add("price", (int)rdr["price"]);
                        Session.Add("vat", (int)rdr["vat"]);
                        Session.Add("amount", (int)rdr["amount"]);
                        Session.Add("payment_term", (string)rdr["payment_term"]);
                        Session.Add("remarks", (string)rdr["remarks"]);
                        Session.Add("aset_status", (string)rdr["aset_status"]);
                        Session.Add("po_created_by", (string)(rdr.IsDBNull(15) ? null : rdr["po_created_by"]));
                        Session.Add("po_approved_by", (string)(rdr.IsDBNull(16) ? null : rdr["po_approved_by"]));
                        Session.Add("po_checked_by", (string)(rdr.IsDBNull(17) ? null : rdr["po_checked_by"]));
                        Session.Add("authorized_by", (string)(rdr.IsDBNull(18) ? null : rdr["authorized_by"]));
                        Session.Add("po_checked_by_it", (string)(rdr.IsDBNull(19) ? null : rdr["po_checked_by_it"]));
                        Session.Add("approve_status", (string)rdr["approve_status"]);
                        Session.Add("po_status", (string)rdr["po_status"]);
                        Session.Add("Dept", (string)rdr["Dept"]);
                        Session.Add("create_date", (DateTime)rdr["create_date"]);
                        Session.Add("modifiedby", (string)rdr["modifiedby"]);
                        Session.Add("modified_date", (DateTime)rdr["modified_date"]);
                        Session.Add("Requester", (string)rdr["Requester"]);
                        Session.Add("quantity", (int)rdr["quantity"]);
                        Session.Add("unit_name", (string)rdr["unit_name"]);
                        Session.Add("id_vendor", (string)rdr["id_vendor"].ToString());
                        Session.Add("other_condition", (string)(rdr.IsDBNull(31) ? null : rdr["other_condition"]));
                        Session.Add("email_po_created_by", (string)(rdr.IsDBNull(32) ? null : rdr["email_po_created_by"]));
                        Session.Add("email_po_approved_by", (string)(rdr.IsDBNull(33) ? null : rdr["email_po_approved_by"]));
                        Session.Add("email_po_checked_by", (string)(rdr.IsDBNull(34) ? null : rdr["email_po_checked_by"]));
                        Session.Add("email_authorized_by", (string)(rdr.IsDBNull(35) ? null : rdr["email_authorized_by"]));
                        Session.Add("email_po_checked_by_it", (string)(rdr.IsDBNull(36) ? null : rdr["email_po_checked_by_it"]));
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();
            }

            lbPONumberBreadcrumb.Text = Session["po_no"].ToString();
            lbPONumberHeader.Text = Session["po_no"].ToString();
            string ReqDateFromDatabase = Session["po_date"].ToString();
            DateTime ParseDatetime = DateTime.Parse(ReqDateFromDatabase);
            string ReqDate = ParseDatetime.ToString("dd MMMM yyyy");
            lbIssuedDate.Text = ReqDate;
            lbRequester.Text = Session["Requester"].ToString() + " (" + Session["Dept"].ToString() + ")";
            lbDeliveryDate.Text = Session["delivery_date"].ToString();
            lbAssetType.Text = Session["aset_status"].ToString();
            lbVendorName.Text = Session["vendor_name"].ToString();
            lbPaymentTerms.Text = Session["payment_term"].ToString();
            txtVAT.Value = Session["vat"].ToString();
            hlbIDVendor.Value = Session["id_vendor"].ToString();

            if (!IsPostBack)
            {
                GetTableItemPO();
            }

            int total = 0;
            foreach (GridViewRow grow in TableItemPO.Rows)
            {
                HtmlInputText amount = (HtmlInputText)grow.FindControl("txtAmount");
                decimal parsedValue = decimal.Parse(amount.Value, NumberStyles.Currency);
                int getAmount = Convert.ToInt32(parsedValue);
                total += getAmount;
            }
            int vat = Convert.ToInt32(txtVAT.Value.ToString());
            decimal vatValue = vat / 100m;
            int getTotal = Convert.ToInt32(total);
            decimal vatAmount;

            vatAmount = vatValue * getTotal;

            txtVatAmount.Value = vatAmount.ToString("#,##0.00");

            decimal grandTotal = getTotal + vatAmount;
            txtGrandTotal.Value = grandTotal.ToString("#,##0.00");
            int getGrandTotal = Convert.ToInt32(grandTotal);
            hlbGrandTotal.Value = getGrandTotal.ToString();

            // Display the total value
            decimal value;
            value = Convert.ToDecimal(total);
            txtTotalAmount.Value = value.ToString("#,##0.00");

            #region BarStatus
            if (Session["approve_status"].ToString() == "PO Created")
            {
                // Price<= 1Jt GA Catalog
                if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price<= 1Jt IT Catalog
                else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT GA Catalog
                else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt GA Catalog
                else if (Session["po_checked_by_it"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt IT Catalog
                else
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
            {
                // Price<= 1Jt IT Catalog
                if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt GA Catalog
                else
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
            {
                // Price<= 1Jt GA Catalog
                if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price<= 1Jt IT Catalog
                else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT GA Catalog
                else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt GA Catalog
                else if (Session["po_checked_by_it"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt IT Catalog
                else
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Admin GM)")
            {
                if (Session["po_checked_by_it"] is null)
                {
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    GetDataGMAdminApproved();
                    admin_gm.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGMAdmDate = Session["tgl_approve_GMAdmin"].ToString();
                    DateTime ParseDatetimeAppGMAdmDate = DateTime.Parse(AppGMAdmDate);
                    string GetAppGMAdmDate = ParseDatetimeAppGMAdmDate.ToString("dd MMMM yyyy");
                    lbDateGMAdm.Text = GetAppGMAdmDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GMAdminApprover"].ToString();
                }
                else
                {
                    GetDataITHeadApproved();
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppDate = Session["tgl_approve_ithead"].ToString();
                    DateTime ParseDatetimeAppDate = DateTime.Parse(AppDate);
                    string GetAppDate = ParseDatetimeAppDate.ToString("dd MMMM yyyy");
                    lbDateITHead.Text = GetAppDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["ITHeadApprover"].ToString();
                    GetDataGAHeadApproved();
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGADate = Session["tgl_approve_GAhead"].ToString();
                    DateTime ParseDatetimeAppGADate = DateTime.Parse(AppGADate);
                    string GetAppGADate = ParseDatetimeAppGADate.ToString("dd MMMM yyyy");
                    lbDateGAHead.Text = GetAppGADate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GAHeadApprover"].ToString();
                    GetDataGMAdminApproved();
                    admin_gm.Attributes.Add("class", "StepProgress-item is-done");
                    string AppGMAdmDate = Session["tgl_approve_GMAdmin"].ToString();
                    DateTime ParseDatetimeAppGMAdmDate = DateTime.Parse(AppGMAdmDate);
                    string GetAppGMAdmDate = ParseDatetimeAppGMAdmDate.ToString("dd MMMM yyyy");
                    lbDateGMAdm.Text = GetAppGMAdmDate + "&nbsp;-&nbsp;" + "Checked by" + "&nbsp" + Session["GMAdminApprover"].ToString();
                }
            }
            else if (Session["approve_status"].ToString() == "CANCEL")
            {
                // Price<= 1Jt GA Catalog
                if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price<= 1Jt IT Catalog
                else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    admin_gm.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT GA Catalog
                else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    admin_director.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt GA Catalog
                else if (Session["po_checked_by_it"] is null)
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                    it_section_head.Attributes.Add("style", "display:none");
                }
                // Price >= 20Jt GA Catalog
                else
                {
                    po_created.Attributes.Add("class", "StepProgress-item is-reject");
                    it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
                    admin_director.Attributes.Add("class", "StepProgress-item is-reject");
                    po_onprocess.Attributes.Add("class", "StepProgress-item is-reject");
                    good_receipt.Attributes.Add("class", "StepProgress-item is-reject");
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    lbDateCreatePO.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["po_created_by"].ToString();
                }
            }

            #endregion
        }

        protected void GetTableItemPO()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailPurchaseOrder");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberBreadcrumb.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableItemPO.DataSource = dtb;
            TableItemPO.DataBind();

            //TableItemPO.Columns[8].Visible = false;

            TableItemPO.UseAccessibleHeader = true;
            TableItemPO.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        protected void btnDownloadPO_Click(object sender, EventArgs e)
        {
            if (Session["approve_status"].ToString() == "PO Created")
            {
                // Price<= 1Jt GA Catalog
                if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                // Price<= 1Jt IT Catalog
                else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                // Price beetwen 1Jt-20JT GA Catalog
                else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendITManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendITManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                // Price >= 20Jt GA Catalog
                else if (Session["po_checked_by_it"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                // Price >= 20Jt GA Catalog
                else
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseOrderCreated_SendGAManager";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderCreated_SendGAManager.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
            {
                if (Session["authorized_by"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_ITHeadApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderITHeadApproved_SendToGAHead.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                else
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_ITHeadApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderITHeadApproved_SendToGAHead.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
            {
                if (Session["po_checked_by_it"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved_ITHeadNull";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                else
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
            }
            else if (Session["approve_status"].ToString() == "Approved (Admin GM)")
            {
                if (Session["po_checked_by_it"] is null)
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved_ITHeadNull";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
                else
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Purchase Order - " + lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(mybytes); // create the file
                    Response.Flush();
                }
            }
            else if (Session["approve_status"].ToString() == "CANCEL")
            {
                // Price<= 1Jt GA Catalog
                if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    
                }
                // Price<= 1Jt IT Catalog
                else if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                {
                    
                }
                // Price beetwen 1Jt-20JT GA Catalog
                else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                {
                    
                }
                // Price beetwen 1Jt-20JT IT Catalog
                else if (Session["authorized_by"] is null)
                {
                    
                }
                // Price >= 20Jt GA Catalog
                else if (Session["po_checked_by_it"] is null)
                {
                   
                }
                // Price >= 20Jt GA Catalog
                else
                {
                    
                }
            }
        }

        protected void ddlApproval_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void UpdateStatusPO()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateStatusPO");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtApprovalDate.Value != "" && ddlApproval.SelectedValue != "0")
            {
                if (Session["approve_status"].ToString() == "PO Created")
                {
                    //GA Catalog
                    if (Session["po_checked_by_it"] is null && Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Checked by GA Head");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Under1jt_GA();
                        }
                    }
                    else if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Checked by GA Head");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Checked by GA Head)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();

                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToGMAdmin_ITHeadNull();
                        }
                    }
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Checked by IT Head");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Checked by IT Head)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();

                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToGAHead();
                        }
                    }
                }
                else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
                {
                    // Price<= 1Jt IT Catalog
                    if (Session["po_approved_by"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Checked by GA Head");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Under1jt_IT();
                        }
                    }
                    // Price beetwen 1Jt-20JT IT Catalog
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Checked by GA Head");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Checked by GA Head)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();

                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToGMAdmin();
                        }
                    }
                    
                }
                else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                {
                    if (Session["po_checked_by_it"] is null && Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Admin GM");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Under20jt_GA();
                        }
                    }
                    else if (Session["authorized_by"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Admin GM");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Under20jt_IT();
                        }
                    }
                    else if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Admin GM");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Admin GM)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();

                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToAuthorized_ITHeadNull();
                        }
                    }
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Admin GM");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Admin GM)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();

                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToAuthorized();
                        }
                    }
                }
                else if (Session["approve_status"].ToString() == "Approved (Admin GM)")
                {
                    if (Session["po_checked_by_it"] is null)
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Presdir/Director");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Upper20jt_GA();
                        }
                    }
                    else
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Connection = Con;
                        sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                        sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
                        sqlcomm.Parameters.AddWithValue("@tgl_approve", txtApprovalDate.Value.ToString());
                        sqlcomm.Parameters.AddWithValue("@nik_approver", Session["nik"].ToString());
                        sqlcomm.Parameters.AddWithValue("@level_approver", "Presdir/Director");
                        sqlcomm.Parameters.AddWithValue("@approval_status", ddlApproval.SelectedItem.Text.ToString() + " (Full Approval)");

                        sqlcomm.ExecuteNonQuery();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        Con.Close();
                        UpdateStatusPO();
                        foreach (GridViewRow grow in TableItemPO.Rows)
                        {
                            //Searching CheckBox("chkSelect") in an individual row of Grid  
                            CheckBox chkdel = (CheckBox)grow.FindControl("ckSelectRemove");
                            //If CheckBox is checked than delete the record with particular id  
                            if (chkdel.Checked)
                            {
                                string item_code = grow.Cells[4].Text;
                                DeleteSelectedItems(item_code);
                            }
                        }
                        if (ddlApproval.SelectedItem.Text.ToString() == "Reject" || ddlApproval.SelectedItem.Text.ToString() == "Cancel")
                        {
                            UpdateStatusRejectCancel();
                            UpdateDetail_RFCancelReject();
                            await SendEmailCancel();
                        }
                        else
                        {
                            await SendEmailToPurchaseTim_FullApproval_Upper20jt_IT();
                        }
                    }
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed, field approval & approval date cannot be empty!');", true);
            }
        }

        protected void DeleteSelectedItems(string item_code)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Delete");
            sqlcomm.Parameters.AddWithValue("@item_code", item_code);

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected void UpdateStatusRejectCancel()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateStatusRejectCancel");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());
            sqlcomm.Parameters.AddWithValue("@rf_no", Session["rf_no"].ToString());
            sqlcomm.Parameters.AddWithValue("@status", ddlApproval.SelectedItem.Text.ToString());

            sqlcomm.ExecuteNonQuery();
            Con.Close();
        }

        protected void UpdateDetail_RFCancelReject()
        {
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalPurchaseOrder";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "CancelDetailRF");
                sqlcomm.Parameters.AddWithValue("@item_code", row.Cells[4].Text.ToString());
                sqlcomm.Parameters.AddWithValue("@rf_no", row.Cells[2].Text.ToString());

                sqlcomm.ExecuteNonQuery();
                sqlcomm.Dispose();
                Con.Close();
                Con.Dispose();
            }

        }

        protected void GetDataITHeadApproved()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusITHeadApproved");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("ITHeadApprover", (string)dr["ITHeadApprover"]);
                Session.Add("tgl_approve_ithead", (DateTime)dr["tgl_approve_ithead"]);
                Session.Add("approval_status_ithead", (string)dr["approval_status_ithead"]);
            }

        }

        protected void GetDataGAHeadApproved()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGAHeadApproved");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GAHeadApprover", (string)dr["GAHeadApprover"]);
                Session.Add("tgl_approve_GAhead", (DateTime)dr["tgl_approve_GAhead"]);
                Session.Add("approval_status_GAhead", (string)dr["approval_status_GAhead"]);
            }

        }

        protected void GetDataGMAdminApproved()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGMAdminApproved");
            sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GMAdminApprover", (string)dr["GMAdminApprover"]);
                Session.Add("tgl_approve_GMAdmin", (DateTime)dr["tgl_approve_GMAdmin"]);
                Session.Add("approval_status_GMAdmin", (string)dr["approval_status_GMAdmin"]);
            }

        }

        #region Barcode
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-procurement/detail_purchase_order_standart.aspx"; // URL tujuan untuk QR code
            string id = lbPONumberHeader.Text; // Nilai ID yang akan digunakan dalam URL

            // Membuat URL dengan parameter
            string data = $"{baseUrl}?po_no={Uri.EscapeDataString(id)}";

            // Membuat gambar QR code
            System.Drawing.Image barcodeImage = GenerateBarcodeImage(data);

            // Memuat gambar logo
            Bitmap logo = new Bitmap(Server.MapPath("~/images/logo_sayap_barcode.png"));

            // Menyesuaikan ukuran logo sesuai keinginan
            int logoWidth = barcodeImage.Width / 5; // Menyesuaikan ukuran logo menjadi 1/5 lebar dari QR code
            int logoHeight = barcodeImage.Height / 5; // Menyesuaikan ukuran logo menjadi 1/5 tinggi dari QR code
            Bitmap resizedLogo = new Bitmap(logo, new System.Drawing.Size(logoWidth, logoHeight));

            // Membuat bitmap baru untuk menyimpan gambar yang digabung
            Bitmap combinedImage = new Bitmap(barcodeImage.Width, barcodeImage.Height); // Ukuran gambar yang digabung sama dengan ukuran barcode

            // Menggambar barcode ke gambar yang digabung
            using (Graphics graphic = Graphics.FromImage(combinedImage))
            {
                graphic.DrawImage(barcodeImage, 0, 0);
            }

            // Menambahkan logo ke gambar yang digabung di tengah-tengah barcode
            int x = (combinedImage.Width - resizedLogo.Width) / 2;
            int y = (combinedImage.Height - resizedLogo.Height) / 2;
            using (Graphics graphic = Graphics.FromImage(combinedImage))
            {
                graphic.DrawImage(resizedLogo, new Point(x, y));
            }

            // Menyimpan gambar barcode ke folder
            string folderPath = Server.MapPath("~/Barcode/PO");
            SaveBarcodeImage(combinedImage, folderPath);

            // Menampilkan gambar barcode di halaman
            DisplayImageOnPage(combinedImage);
        }

        private void SaveBarcodeImage(System.Drawing.Image barcodeImage, string folderPath)
        {
            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save the barcode image to the folder
            string fileName = "barcode_po.png";  // You can customize the file name
            string filePath = System.IO.Path.Combine(folderPath, fileName);
            barcodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        private System.Drawing.Image GenerateBarcodeImage(string data)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 600,
                Height = 600
            };

            Bitmap barcodeBitmap = barcodeWriter.Write(data);

            return barcodeBitmap;
        }

        private void DisplayImageOnPage(System.Drawing.Image barcodeImage)
        {
            // Save the barcode image to a MemoryStream
            using (MemoryStream stream = new MemoryStream())
            {
                barcodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                // Set the Image control's properties
                imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
            }
        }
        #endregion

        #region EmailCancel
        private string PopulateBodyCancel(string po_no, string issued_date, string reqby, string preparedby)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTempleteRequestPOCancel.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            return body;
        }

        private async Task SendEmailCancel()
        {
            string body = this.PopulateBodyCancel
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString());

            string searchTerm = "IT"; // Get the search term (case-insensitive)

            // Loop through DataGridView rows
            foreach (GridViewRow row in TableItemPO.Rows)
            {
                if (row.Cells[1].Text.ToString() != null)
                {
                    string cellValue = row.Cells[1].Text.ToString();

                    // Check if the cell value matches the search term (case-sensitive)
                    if (cellValue == searchTerm)
                    {
                        hlbCatalog.Value = "IT";
                    }
                    else
                    {
                        hlbCatalog.Value = "GA";
                    }
                }
            }

            Int32 grandtotal;
            grandtotal = Convert.ToInt32(hlbGrandTotal.Value);

            string toEmailIT = string.Empty;
            string toEmailGA = string.Empty;
            string toEmailGM = string.Empty;
            string toEmailDirector = string.Empty;
            string ccEmailPOCreate = string.Empty;

            if (grandtotal < 1000000)
            {
                if (hlbCatalog.Value == "IT")
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
                else
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
            }
            else if (grandtotal > 1000000 && grandtotal < 20000000)
            {
                if (hlbCatalog.Value == "IT")
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (GM Admin)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
                else
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (GM Admin)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
            }
            else if (grandtotal > 20000000)
            {
                if (hlbCatalog.Value == "IT")
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by IT Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (GM Admin)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        toEmailDirector = "widhi.kusuma@id.yusen-logistics.com";//Session["email_authorized_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Presdir / Director)")
                    {
                        toEmailIT = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by_it"].ToString();
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        toEmailDirector = "widhi.kusuma@id.yusen-logistics.com";//Session["email_authorized_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
                else
                {
                    if (Session["approve_status"].ToString() == "PO Created")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Checked by GA Head)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (GM Admin)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        toEmailDirector = "widhi.kusuma@id.yusen-logistics.com";//Session["email_authorized_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                    else if (Session["approve_status"].ToString() == "Approved (Presdir / Director)")
                    {
                        toEmailGA = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_checked_by"].ToString();
                        toEmailGM = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_approved_by"].ToString();
                        toEmailDirector = "widhi.kusuma@id.yusen-logistics.com";//Session["email_authorized_by"].ToString();
                        ccEmailPOCreate = "widhi.kusuma@id.yusen-logistics.com";//Session["email_po_created_by"].ToString();
                    }
                }
            }

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                var toRecipients = new List<object>();
                if (!string.IsNullOrEmpty(toEmailIT)) toRecipients.Add(new { emailAddress = new { address = toEmailIT } });
                if (!string.IsNullOrEmpty(toEmailGA)) toRecipients.Add(new { emailAddress = new { address = toEmailGA } });
                if (!string.IsNullOrEmpty(toEmailGM)) toRecipients.Add(new { emailAddress = new { address = toEmailGM } });
                if (!string.IsNullOrEmpty(toEmailDirector)) toRecipients.Add(new { emailAddress = new { address = toEmailDirector } });

                var ccRecipients = new List<object>();
                if (!string.IsNullOrEmpty(ccEmailPOCreate)) ccRecipients.Add(new { emailAddress = new { address = ccEmailPOCreate } });

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            //toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            toRecipients = toRecipients.ToArray(),
                            ccRecipients = ccRecipients.ToArray(),

                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Under1jt_GA
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Under1jt_GA(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGAHeadApproved_ITNull_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Under1jt_GA()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Under1jt_GA
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under1jt_GA";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Under1jt_IT
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Under1jt_IT(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGAHeadApproved_ITNull_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Under1jt_IT()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Under1jt_IT
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under1jt_IT";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToPurchaseTim.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Under20jt_GA
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Under20jt_GA(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGMAdminApproved_ITNull_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Under20jt_GA()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Under20jt_GA
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under20jt_GA";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Under20jt_IT
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Under20jt_IT(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGMAdminApproved_ITNull_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Under20jt_IT()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Under20jt_IT
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Under20jt_IT";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToPurchaseTim_AuthorizedNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Upper20jt_GA
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Upper20jt_GA(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderPresdir_DirectorApproved_ITNull_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Upper20jt_GA()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Upper20jt_GA
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Upper20jt_GA";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderPresdir_DirectorApproved_SendToPurchaseTim_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["authorized_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToPurchaseTim_FullApproval_Upper20jt_IT
        private string PopulateBodyEmailSendToPurchaseTim_FullApproval_Upper20jt_IT(string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderPresdir_DirectorApproved_SendToPurchaseTim.html")))
            {
                body = reader.ReadToEnd();
            }
            //body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToPurchaseTim_FullApproval_Upper20jt_IT()
        {
            string body = this.PopulateBodyEmailSendToPurchaseTim_FullApproval_Upper20jt_IT
            (lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Full Approval)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_FullApproval_Upper20jt_IT";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderPresdir_DirectorApproved_SendToPurchaseTim.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_authorized_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToGAHead
        private string PopulateBodyEmailSendToGAHead(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderITHeadApproved_SendToGAHead.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToGAHead()
        {
            string body = this.PopulateBodyEmailSendToGAHead
            (Session["po_checked_by"].ToString(), lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(),ddlApproval.SelectedItem.Text + " (Checked by IT Head)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_ITHeadApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderITHeadApproved_SendToGAHead.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToGMAdmin
        private string PopulateBodyEmailSendToGMAdmin(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGAHeadApproved_SendToGMAdmin.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToGMAdmin()
        {
            string body = this.PopulateBodyEmailSendToGMAdmin
            (Session["po_approved_by"].ToString(), lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Checked by GA Head)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }, 
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_checked_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToGMAdmin_ITHeadNull
        private string PopulateBodyEmailSendToGMAdmin_ITHeadNull(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGAHeadApproved_SendToGMAdmin.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToGMAdmin_ITHeadNull()
        {
            string body = this.PopulateBodyEmailSendToGMAdmin_ITHeadNull
            (Session["po_approved_by"].ToString(), lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Checked by GA Head)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GAHeadApproved_ITHeadNull";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGAHeadApproved_SendToGMAdmin_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "email_po_checked_by" } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToAuthorized_ITHeadNull
        private string PopulateBodyEmailSendToAuthorized_ITHeadNull(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGMAdminApproved_SendToAuthorized.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToAuthorized_ITHeadNull()
        {
            string body = this.PopulateBodyEmailSendToAuthorized_ITHeadNull
            (Session["authorized_by"].ToString(), lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Admin GM)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved_ITHeadNull";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized_ITHeadNull.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "email_po_checked_by" } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

        #region EmailToAuthorized
        private string PopulateBodyEmailSendToAuthorized(string approver, string po_no, string issued_date, string reqby, string preparedby, string po_status)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplatePurchaseOrderGMAdminApproved_SendToAuthorized.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{APPROVER}", approver);
            body = body.Replace("{PONo}", po_no);
            body = body.Replace("{IssuedDate}", issued_date);
            body = body.Replace("{RequestBy}", reqby);
            body = body.Replace("{PreparedBy}", preparedby);
            body = body.Replace("{POStatus}", po_status);
            return body;
        }

        private async Task SendEmailToAuthorized()
        {
            string body = this.PopulateBodyEmailSendToAuthorized
            (Session["authorized_by"].ToString(), lbPONumberHeader.Text, lbIssuedDate.Text, lbRequester.Text, Session["po_created_by"].ToString(), ddlApproval.SelectedItem.Text + " (Admin GM)");

            try
            {
                // Konfigurasi autentikasi
                var authority = $"https://login.microsoftonline.com/{_tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_clientId)
                    .WithClientSecret(_clientSecret)
                    .WithAuthority(authority)
                    .Build();

                // Mendapatkan token akses
                var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                var accessToken = authResult.AccessToken;

                // Create HttpClient with Authorization Header
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Read HTML content from file
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PO_GMAdminApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@po_no", lbPONumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseOrderGMAdminApproved_SendToAuthorized.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchaseOrderCreated", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = lbPONumberHeader.Text.Trim() + ".pdf";
                    string extension;
                    string encoding;
                    string mimeType;
                    string[] streams;
                    Warning[] warnings;
                    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
                                  out extension, out encoding,
                                  out mimeType, out streams, out warnings);
                    using (FileStream fs = File.Create(Server.MapPath("~/Prints/" + FileName)))
                    {
                        fs.Write(mybytes, 0, mybytes.Length);
                    }

                    var attachmentBytes = File.ReadAllBytes(Server.MapPath("~/Prints/" + FileName));
                    var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

                    var attachment = new FileAttachment
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = FileName,
                        ContentBytes = attachmentBase64
                    };

                    string _vPONo = lbPONumberHeader.Text.Trim();
                    string folderPath = Server.MapPath("~/eDocs_Files/PO/" + _vPONo + "/"); // Specify the path to your folder

                    List<FileAttachment> attachments1 = new List<FileAttachment>();

                    if (Directory.Exists(folderPath))
                    {
                        // Get all files in the folder
                        string[] fileNames = Directory.GetFiles(folderPath);

                        foreach (string filePath in fileNames)
                        {
                            // Extract the file name
                            string fileName = Path.GetFileName(filePath);

                            // Read the file into a byte array
                            byte[] attachBytes = File.ReadAllBytes(filePath);

                            // Convert the byte array to Base64
                            string attachBase64 = Convert.ToBase64String(attachBytes);

                            // Create the file attachment object
                            var attach = new FileAttachment
                            {
                                Type = "#microsoft.graph.fileAttachment",
                                Name = fileName,
                                ContentBytes = attachBase64
                            };

                            // Add the attachment to the list
                            attachments1.Add(attach);
                        }
                    }

                    // Create email content with HTML body
                    var emailBody = new
                    {
                        message = new
                        {
                            subject = "PURCHASE ORDER FORM : " + lbPONumberHeader.Text,
                            body = new
                            {
                                contentType = "HTML",
                                content = body
                            },
                            toRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            ccRecipients = new[] { new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } }, new { emailAddress = new { address = "widhi.kusuma@id.yusen-logistics.com" } } },
                            //toRecipients = new[] { new { emailAddress = new { address = Session["email_po_approved_by"].ToString() } } },
                            //ccRecipients = new[] { new { emailAddress = new { address = "email_po_checked_by" } },
                            //    new { emailAddress = new { address = Session["email_po_created_by"].ToString() } },
                            //    new { emailAddress = new { address = Session["email_po_checked_by_it"].ToString() } } },
                            attachments = Directory.Exists(folderPath) ? new[] { attachment }.Concat(attachments1).ToArray() : new[] { attachment }
                        },
                        saveToSentItems = true
                    };

                    var emailBodyJson = JsonConvert.SerializeObject(emailBody);
                    var content = new StringContent(emailBodyJson, Encoding.UTF8, "application/json");

                    File.Delete(Server.MapPath("~/Prints/" + FileName));

                    // Send HTTP request to send email
                    var response = await httpClient.PostAsync(_endpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Handle response
                    if (response.IsSuccessStatusCode)
                    {
                        // Assuming 'FuncSave()' and 'FailedSend()' are JavaScript functions on the client side
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
                        string script = $@"
                                        $(document).ready(function() {{
                                            // Show Toastr notification
                                            toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                            // Redirect after 2 seconds (2000 milliseconds)
                                            setTimeout(function() {{
                                                window.location.href = 'approval_purchase_order.aspx'; // replace with your target URL
                                            }}, 2000);
                                        }});
                                    ";

                        // Register the script for partial postbacks
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                    }
                    else
                    {
                        Response.Write(responseContent.ToString());
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit failed');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write(ex.ToString());
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FailedSend();", true);
            }
        }
        #endregion

    }
}