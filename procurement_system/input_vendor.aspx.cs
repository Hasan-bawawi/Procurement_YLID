using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ZXing;

namespace procurement_system
{
    public partial class input_vendor : System.Web.UI.Page
    {
        string _clientId = WebConfigurationManager.AppSettings["clientId"];
        string _clientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        string _tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string _endpoint = WebConfigurationManager.AppSettings["endpoint"];

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["rf_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(path))
            {
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailRFInPutVendor");
                sqlcomm.Parameters.AddWithValue("@rf_no", id);
                con.Open();
                using (SqlDataReader rdr = sqlcomm.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Session.Add("catalog_type", (string)rdr["catalog_type"]);
                        Session.Add("rf_no", (string)rdr["rf_no"]);
                        Session.Add("id", (string)rdr["id"].ToString());
                        Session.Add("Requester", (string)rdr["Requester"]);
                        Session.Add("stok_code", (string)rdr["stok_code"]);
                        Session.Add("item_code", (string)rdr["item_code"]);
                        Session.Add("item_name", (string)rdr["item_name"]);
                        Session.Add("merk_name", (string)rdr["merk_name"]);
                        Session.Add("tipe", (string)rdr["tipe"]);
                        Session.Add("quantity", (int)rdr["quantity"]);
                        Session.Add("price", (int)rdr["price"]);
                        Session.Add("unit_name", (string)rdr["unit_name"]);
                        Session.Add("request_date", (DateTime)rdr["request_date"]);
                        Session.Add("remaks", (string)rdr["remaks"]);
                        Session.Add("status", (string)rdr["status"]);
                        Session.Add("type_request", (string)rdr["type_request"]);
                        Session.Add("status_approve", (string)rdr["status_approve"]);
                        //Session.Add("description", (string)rdr["description"]);
                        Session.Add("id_vendor", (string)rdr["id_vendor"].ToString());
                        Session.Add("nama_branch", (string)rdr["nama_branch"]);
                        Session.Add("DivisionRequester", (string)rdr["DivisionRequester"].ToString());
                        Session.Add("SectionRequester", (string)rdr["SectionRequester"]);
                        Session.Add("nik_requester", (string)rdr["nik_requester"]);
                        Session.Add("ManagerApprove", (string)(rdr.IsDBNull(7) ? null : rdr["ManagerApprove"]));
                        Session.Add("GMApprove", (string)(rdr.IsDBNull(9) ? null : rdr["GMApprove"]));
                        Session.Add("DeputyDirectorApprove", (string)(rdr.IsDBNull(11) ? null : rdr["DeputyDirectorApprove"]));
                        //Session.Add("AdmManagerApprove", (string)(rdr.IsDBNull(13) ? null : rdr["AdmManagerApprove"]));
                        //Session.Add("AdmGMApprove", (string)(rdr.IsDBNull(15) ? null : rdr["AdmGMApprove"]));
                        //Session.Add("ITManagerApprove", (string)(rdr.IsDBNull(17) ? null : rdr["ITManagerApprove"]));
                        Session.Add("DirectorApprove", (string)(rdr.IsDBNull(18) ? null : rdr["DirectorApprove"]));
                        //Session.Add("AdmDirectorApprove", (string)(rdr.IsDBNull(19) ? null : rdr["AdmDirectorApprove"]));
                        Session.Add("EmailRequester", (string)rdr["EmailRequester"]);
                        Session.Add("EmailManagerApprove", (string)rdr["EmailManagerApprove"]);
                        //Session.Add("EmailGMApprove", (string)rdr["EmailGMApprove"]);
                        //Session.Add("EmailAdmManagerApprove", (string)rdr["EmailAdmManagerApprove"]);
                        //Session.Add("EmailAdmGMApprove", (string)rdr["EmailAdmGMApprove"]);
                        //Session.Add("nik_adm_manager", (string)rdr["nik_adm_manager"]);
                        //Session.Add("nik_adm_gm", (string)rdr["nik_adm_gm"]);
                        Session.Add("DivisionReq", (string)rdr["DivisionReq"].ToString());
                    }
                }
                sqlcomm.Dispose();
                con.Close();
                con.Dispose();
            }
            lbRFNumberBreadcrumb.Text = Session["rf_no"].ToString();
            lbRFNumberHeader.Text = Session["rf_no"].ToString();
            string ReqDateFromDatabase = Session["request_date"].ToString();
            DateTime ParseDatetime = DateTime.Parse(ReqDateFromDatabase);
            string ReqDate = ParseDatetime.ToString("dd MMMM yyyy");
            lbRequestDate.Text = ReqDate;
            lbDivision.Text = Session["DivisionRequester"].ToString();
            lbDivisionReq.Text = Session["DivisionReq"].ToString();
            lbSection.Text = Session["SectionRequester"].ToString();
            lbRequester.Text = Session["Requester"].ToString();
            lbApprovedBy.Text = Session["ManagerApprove"].ToString();
            lbLocation.Text = Session["nama_branch"].ToString();
            hblEmailRequester.Value = Session["EmailRequester"].ToString();
            hblEmailManager.Value = Session["EmailManagerApprove"].ToString();
            //hlbNIKManagerAdm.Value = Session["nik_adm_manager"].ToString();
            //hlbNIKGMAdm.Value = Session["nik_adm_gm"].ToString();
            //hlbManagerAdm.Value = Session["AdmManagerApprove"].ToString();
            //hlbGMAdm.Value = Session["AdmGMApprove"].ToString();
            lbCatalogType.Text = Session["catalog_type"].ToString();

            #region BarStatus_OLD
            //if (lbLocation.Text == "YLID-SUB" || lbLocation.Text == "YLID-SRG")
            //{
            //    if (Session["status_approve"].ToString() == "Price Checked")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT & GA Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT & GA Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division GM)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division Director)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (IT Head)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (GA Head)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataGAHeadApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataGAHeadApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DirectorAdminApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DirectorAdminApproval"].ToString();
            //            }
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Admin GM)")
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //        }
            //        else
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division Manager)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division GM)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDivGMApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDivGMApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDivGMApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDivGMApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Deputy Director)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDeputyDirectorApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDeputyDirectorApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDeputyDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDeputyDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division Director)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataDivisionDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector_reject"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivisionDirectorApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataDivisionDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector_reject"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivisionDirectorApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (IT Head)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataITHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateITHead = Session["tgl_approve_ITHead_reject"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["ITHeadApprovalreject"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataITHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateITHead = Session["tgl_approve_ITHead_reject"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["ITHeadApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataITHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateITHead = Session["tgl_approve_ITHead_reject"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["ITHeadApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (GA Head)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGAHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGAHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Admin GM)")
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            GetDataGMAdminApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin_reject"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GMAdminApprovalreject"].ToString();
            //        }
            //        else
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            GetDataGMAdminApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin_reject"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GMAdminApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Fully Approved)")
            //    {
            //        //EST.PRICE <= 1Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null
            //         && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            gm.Attributes.Add("style", "display:none");
            //            deputy_director.Attributes.Add("style", "display:none");
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt IT Catalog
            //        else if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE >= 1Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                GetDataDirectorAdminApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin_reject"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DirectorAdminApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                GetDataDirectorAdminApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin_reject"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DirectorAdminApprovalreject"].ToString();
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (Session["status_approve"].ToString() == "Price Checked")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division GM)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Division Director)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (IT Head)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (GA Head)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            //GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            //GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DirectorAdminApproval"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DirectorAdminApproval"].ToString();
            //            }
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Approved (Admin GM)")
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //        }
            //        else
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division Manager)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if (Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division GM)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDivGMApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDivGMApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDivGMApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDivGMApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Deputy Director)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDeputyDirectorApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDeputyDirectorApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDeputyDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDeputyDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Division Director)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataDivisionDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector_reject"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivisionDirectorApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataDivisionDirectorApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector_reject"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivisionDirectorApprovalreject"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (IT Head)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataITHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateITHead = Session["tgl_approve_ITHead_reject"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["ITHeadApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataITHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateITHead = Session["tgl_approve_ITHead_reject"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["ITHeadApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (GA Head)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            //GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGAHeadApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE > 5Jt IT Catalog
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGAHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGAHeadApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead_reject"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GAHeadApprovalreject"].ToString();
            //            }

            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Admin GM)")
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            GetDataGMAdminApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin_reject"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GMAdminApprovalreject"].ToString();
            //        }
            //        else
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminApproval();
            //            GetDataGMAdminApprovalReject();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-reject");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdmin_reject"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["GMAdminApprovalreject"].ToString();
            //        }
            //    }
            //    else if (Session["status_approve"].ToString() == "Reject (Fully Approved)")
            //    {
            //        //EST.PRICE <= 5Jt IT Catalog
            //        if (Session["catalog_type"].ToString() == "IT" && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataITHeadApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //            DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //            string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //            lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        //EST.PRICE <= 5Jt GA Catalog
            //        else if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS") && Session["DirectorApprove"] is null && Session["AdmDirectorApprove"] is null && Session["ITManagerApprove"] is null)
            //        {
            //            GetDataPriceEstimatedApproval();
            //            GetDataMgrApproval();
            //            GetDataDivGMApproval();
            //            GetDataDeputyDirectorApproval();
            //            //GetDataDivisionDirectorApproval();
            //            GetDataGAHeadApproval();
            //            GetDataGMAdminFullApproval();
            //            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //            manager.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //            gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //            ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //            DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //            string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //            lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //            admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //            string ReqDateGMAdmin = Session["tgl_approve_GMAdminFull"].ToString();
            //            DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //            string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //            lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApprovalFull"].ToString();
            //            it_section_head.Attributes.Add("style", "display:none");
            //            director.Attributes.Add("style", "display:none");
            //            admin_director.Attributes.Add("style", "display:none");
            //        }
            //        else
            //        {
            //            if (Session["catalog_type"].ToString() == "IT")
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataITHeadApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                GetDataDirectorAdminApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateITHead = Session["tgl_approve_ITHead"].ToString();
            //                DateTime ParseDatetimeITHead = DateTime.Parse(ReqDateITHead);
            //                string GetReqDateITHead = ParseDatetimeITHead.ToString("dd MMMM yyyy");
            //                lbDateITHead.Text = GetReqDateITHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["ITHeadApproval"].ToString();
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin_reject"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DirectorAdminApprovalreject"].ToString();
            //            }
            //            else
            //            {
            //                GetDataPriceEstimatedApproval();
            //                GetDataMgrApproval();
            //                GetDataDivGMApproval();
            //                GetDataDeputyDirectorApproval();
            //                GetDataDivisionDirectorApproval();
            //                GetDataGAHeadApproval();
            //                GetDataGMAdminApproval();
            //                GetDataDirectorAdminApproval();
            //                GetDataDirectorAdminApprovalReject();
            //                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
            //                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
            //                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
            //                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
            //                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
            //                manager.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
            //                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
            //                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
            //                gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivGM = Session["tgl_approve_mgr"].ToString();
            //                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
            //                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
            //                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
            //                deputy_director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
            //                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
            //                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
            //                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
            //                director.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector"].ToString();
            //                DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
            //                string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
            //                lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApproval"].ToString();
            //                it_section_head.Attributes.Add("style", "display:none");
            //                ga_section_head.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGAHead = Session["tgl_approve_GAHead"].ToString();
            //                DateTime ParseDatetimeGAHead = DateTime.Parse(ReqDateGAHead);
            //                string GetReqDateGAHead = ParseDatetimeGAHead.ToString("dd MMMM yyyy");
            //                lbDateGAHead.Text = GetReqDateGAHead + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GAHeadApproval"].ToString();
            //                admin_gm.Attributes.Add("class", "StepProgress-item is-done");
            //                string ReqDateGMAdmin = Session["tgl_approve_GMAdmin"].ToString();
            //                DateTime ParseDatetimeGMAdmin = DateTime.Parse(ReqDateGMAdmin);
            //                string GetReqDateGMAdmin = ParseDatetimeGMAdmin.ToString("dd MMMM yyyy");
            //                lbDateGMAdm.Text = GetReqDateGMAdmin + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["GMAdminApproval"].ToString();
            //                admin_director.Attributes.Add("class", "StepProgress-item is-reject");
            //                string ReqDateDirectorAdmin = Session["tgl_approve_DirectorAdmin_reject"].ToString();
            //                DateTime ParseDatetimeDirectorAdmin = DateTime.Parse(ReqDateDirectorAdmin);
            //                string GetReqDateDirectorAdmin = ParseDatetimeDirectorAdmin.ToString("dd MMMM yyyy");
            //                lbDateDirAdm.Text = GetReqDateDirectorAdmin + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DirectorAdminApprovalreject"].ToString();
            //            }
            //        }
            //    }
            //}
            #endregion

            #region BarStatus_New
            if (Session["status_approve"].ToString() == "Price Checked")
            {
                if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        gm.Attributes.Add("style", "display:none");
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        gm.Attributes.Add("style", "display:none");
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    }
                }
            }
            else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
            {
                if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("style", "display:none");
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("style", "display:none");
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    }
                }
            }
            else if (Session["status_approve"].ToString() == "Approved (Division GM)")
            {
                if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGMFull"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApprovalFull"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                    }
                }
                else if (Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                        director.Attributes.Add("style", "display:none");
                    }
                }
                else
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataCancelRFl();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                        string DateCancel = Session["tgl_approve_Cancel"].ToString();
                        DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                        string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                        lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                    }
                }
            }
            else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApproval();
                    GetDataDeputyDirectorApproval();
                    GetDataCancelRFl();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                    deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
                    DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                    string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                    lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
                    status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                    string DateCancel = Session["tgl_approve_Cancel"].ToString();
                    DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                    string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                    lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                }
                else
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApproval();
                    GetDataDeputyDirectorApproval();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                    deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
                    DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                    string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                    lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
                }
            }
            else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
            {
                if (Session["status"].ToString() == "Complete")
                {
                    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr_full"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval_full"].ToString();
                        gm.Attributes.Add("style", "display:none");
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-done");
                    }
                    else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGMFull"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApprovalFull"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-done");
                    }
                    else if (Session["DeputyDirectorApprove"] is null)
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        GetDataDivisionDirectorApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                        director.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirectorFull"].ToString();
                        DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
                        string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
                        lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApprovalFull"].ToString();
                        deputy_director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-done");
                    }
                    else if (Session["DirectorApprove"] is null)
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        GetDataDeputyDirectorApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                        deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirectorFull"].ToString();
                        DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                        string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                        lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApprovalFull"].ToString();
                        director.Attributes.Add("style", "display:none");
                        status_completed.Attributes.Add("class", "StepProgress-item is-done");
                    }
                    else
                    {
                        GetDataPriceEstimatedApproval();
                        GetDataMgrApproval();
                        GetDataDivGMApproval();
                        GetDataDeputyDirectorApproval();
                        GetDataDivisionDirectorApprovalFull();
                        lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                        price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                        DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                        string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                        lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                        manager.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                        DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                        string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                        lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                        gm.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                        DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                        string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                        lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                        deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
                        DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                        string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                        lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
                        director.Attributes.Add("class", "StepProgress-item is-done");
                        string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirectorFull"].ToString();
                        DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
                        string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
                        lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApprovalFull"].ToString();
                        status_completed.Attributes.Add("class", "StepProgress-item is-done");
                    }
                }
                else
                {
                    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApprovalFull();
                            GetDataCancelRFl();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr_full"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["tgl_approve_mgr_full"].ToString();
                            gm.Attributes.Add("style", "display:none");
                            deputy_director.Attributes.Add("style", "display:none");
                            director.Attributes.Add("style", "display:none");
                            status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                            string DateCancel = Session["tgl_approve_Cancel"].ToString();
                            DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                            string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                            lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                        }
                        else
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApprovalFull();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr_full"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval_full"].ToString();
                            gm.Attributes.Add("style", "display:none");
                            deputy_director.Attributes.Add("style", "display:none");
                            director.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApprovalFull();
                            GetDataCancelRFl();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGMFull"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApprovalFull"].ToString();
                            deputy_director.Attributes.Add("style", "display:none");
                            director.Attributes.Add("style", "display:none");
                            status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                            string DateCancel = Session["tgl_approve_Cancel"].ToString();
                            DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                            string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                            lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                        }
                        else
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApprovalFull();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGMFull"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApprovalFull"].ToString();
                            deputy_director.Attributes.Add("style", "display:none");
                            director.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (Session["DeputyDirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataCancelRFl();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            deputy_director.Attributes.Add("style", "display:none");
                            status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                            string DateCancel = Session["tgl_approve_Cancel"].ToString();
                            DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                            string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                            lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                        }
                        else
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataDivisionDirectorApprovalFull();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            director.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirectorFull"].ToString();
                            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
                            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
                            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApprovalFull"].ToString();
                            deputy_director.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (Session["DirectorApprove"] is null)
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataCancelRFl();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            director.Attributes.Add("style", "display:none");
                            status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                            string DateCancel = Session["tgl_approve_Cancel"].ToString();
                            DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                            string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                            lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                        }
                        else
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataDeputyDirectorApprovalFull();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirectorFull"].ToString();
                            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApprovalFull"].ToString();
                            director.Attributes.Add("style", "display:none");
                        }
                    }
                    else
                    {
                        if (Session["status"].ToString() == "Canceled")
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataCancelRFl();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            status_completed.Attributes.Add("class", "StepProgress-item is-reject");
                            string DateCancel = Session["tgl_approve_Cancel"].ToString();
                            DateTime ParseDateCancel = DateTime.Parse(DateCancel);
                            string GetDateCancel = ParseDateCancel.ToString("dd MMMM yyyy");
                            lbDateComplete.Text = GetDateCancel + "&nbsp;-&nbsp;" + "Canceled by" + "&nbsp" + Session["CanceledBy"].ToString();
                        }
                        else
                        {
                            GetDataPriceEstimatedApproval();
                            GetDataMgrApproval();
                            GetDataDivGMApproval();
                            GetDataDeputyDirectorApproval();
                            GetDataDivisionDirectorApprovalFull();
                            lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                            price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                            DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                            string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                            lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                            manager.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                            DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                            string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                            lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                            gm.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                            DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                            string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                            lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                            deputy_director.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector"].ToString();
                            DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                            string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                            lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DeputyDirectorApproval"].ToString();
                            director.Attributes.Add("class", "StepProgress-item is-done");
                            string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirectorFull"].ToString();
                            DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
                            string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
                            lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivisionDirectorApprovalFull"].ToString();
                        }
                    }
                }
            }
            else if (Session["status_approve"].ToString() == "Reject (Division Manager)" || Session["status_approve"].ToString() == "Cancel (Division Manager)")
            {
                if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
                    gm.Attributes.Add("style", "display:none");
                    deputy_director.Attributes.Add("style", "display:none");
                    director.Attributes.Add("style", "display:none");
                }
                else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
                    deputy_director.Attributes.Add("style", "display:none");
                    director.Attributes.Add("style", "display:none");
                }
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
                    deputy_director.Attributes.Add("style", "display:none");
                }
                else if (Session["DirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
                    director.Attributes.Add("style", "display:none");
                }
                else
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateMgr = Session["tgl_approve_mgr_reject"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["MgrApprovalreject"].ToString();
                }
            }
            else if (Session["status_approve"].ToString() == "Reject (Division GM)" || Session["status_approve"].ToString() == "Cancel (Division GM)")
            {
                if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
                    deputy_director.Attributes.Add("style", "display:none");
                    director.Attributes.Add("style", "display:none");
                }
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
                    deputy_director.Attributes.Add("style", "display:none");
                }
                else if (Session["DirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
                    director.Attributes.Add("style", "display:none");
                }
                else
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDivGM = Session["tgl_approve_DivGM_reject"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivGMApprovalreject"].ToString();
                }
            }
            else if (Session["status_approve"].ToString() == "Reject (Deputy Director)" || Session["status_approve"].ToString() == "Cancel (Deputy Director)")
            {
                GetDataPriceEstimatedApproval();
                GetDataMgrApproval();
                GetDataDivGMApproval();
                GetDataDeputyDirectorApprovalReject();
                lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                manager.Attributes.Add("class", "StepProgress-item is-done");
                string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                gm.Attributes.Add("class", "StepProgress-item is-done");
                string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
                string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
                DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
            }
            else if (Session["status_approve"].ToString() == "Reject (Division Director)" || Session["status_approve"].ToString() == "Cancel (Division Director)")
            {
                if (Session["DeputyDirectorApprove"] is null)
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApproval();
                    GetDataDivisionDirectorApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                    director.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDivisionDirector = Session["tgl_approve_DivisionDirector_reject"].ToString();
                    DateTime ParseDatetimeDivisionDirector = DateTime.Parse(ReqDateDivisionDirector);
                    string GetReqDateDivisionDirector = ParseDatetimeDivisionDirector.ToString("dd MMMM yyyy");
                    lbDateDir.Text = GetReqDateDivisionDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DivisionDirectorApprovalreject"].ToString();
                    deputy_director.Attributes.Add("style", "display:none");
                }
                else
                {
                    GetDataPriceEstimatedApproval();
                    GetDataMgrApproval();
                    GetDataDivGMApproval();
                    GetDataDeputyDirectorApprovalReject();
                    lbDateCreateRF.Text = ReqDate + "&nbsp;-&nbsp;" + "Created by" + "&nbsp" + Session["Requester"].ToString();
                    price_estimated.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDatePE = Session["tgl_approve_price_est"].ToString();
                    DateTime ParseDatetimePE = DateTime.Parse(ReqDatePE);
                    string GetReqDatePE = ParseDatetimePE.ToString("dd MMMM yyyy");
                    lbDatePriceEstimate.Text = GetReqDatePE + "&nbsp;-&nbsp;" + "Price Checked by" + "&nbsp" + Session["PriceEstInput"].ToString();
                    manager.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateMgr = Session["tgl_approve_mgr"].ToString();
                    DateTime ParseDatetimeMgr = DateTime.Parse(ReqDateMgr);
                    string GetReqDateMgr = ParseDatetimeMgr.ToString("dd MMMM yyyy");
                    lbDateMgr.Text = GetReqDateMgr + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["MgrApproval"].ToString();
                    gm.Attributes.Add("class", "StepProgress-item is-done");
                    string ReqDateDivGM = Session["tgl_approve_DivGM"].ToString();
                    DateTime ParseDatetimeDivGM = DateTime.Parse(ReqDateDivGM);
                    string GetReqDateDivGM = ParseDatetimeDivGM.ToString("dd MMMM yyyy");
                    lbDateGM.Text = GetReqDateDivGM + "&nbsp;-&nbsp;" + "Approved by" + "&nbsp" + Session["DivGMApproval"].ToString();
                    deputy_director.Attributes.Add("class", "StepProgress-item is-reject");
                    string ReqDateDeputyDirector = Session["tgl_approve_DeputyDirector_reject"].ToString();
                    DateTime ParseDatetimeDeputyDirector = DateTime.Parse(ReqDateDeputyDirector);
                    string GetReqDateDeputyDirector = ParseDatetimeDeputyDirector.ToString("dd MMMM yyyy");
                    lbDateDepDir.Text = GetReqDateDeputyDirector + "&nbsp;-&nbsp;" + "Rejected by" + "&nbsp" + Session["DeputyDirectorApprovalreject"].ToString();
                    director.Attributes.Add("style", "display:none");
                }
            }

            #endregion

            if (!IsPostBack)
            {
                BindDataTableItemRF();
                GetVendorData();

                foreach (GridViewRow grow in TableDetailsRF.Rows)
                {
                    DropDownList ddlList = (DropDownList)grow.FindControl("ddlVendor");
                    if (grow.Cells[7].Text != "&nbsp;")
                    {
                        ddlList.SelectedItem.Text = grow.Cells[7].Text;
                    }
                    else
                    {
                        ddlList.SelectedItem.Text = "";
                    }
                    if (grow.Cells[8].Text != "&nbsp;")
                    {
                        ddlList.SelectedValue = grow.Cells[8].Text;
                    }
                    else
                    {
                        ddlList.SelectedValue = "00000000-0000-0000-0000-000000000000";
                    }
                }
            }
        }

        protected void GetVendorData()
        {
            foreach (GridViewRow grow in TableDetailsRF.Rows)
            {
                DropDownList ddlList = (DropDownList)grow.FindControl("ddlVendor");
                ddlList.Items.Clear();
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);

                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Vendor";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "AddVendor");

                SqlDataReader dr;

                try
                {
                    System.Web.UI.WebControls.ListItem newItem = new System.Web.UI.WebControls.ListItem();
                    newItem.Text = "";
                    newItem.Value = "00000000-0000-0000-0000-000000000000";
                    ddlList.Items.Add(newItem);

                    Con.Open();
                    dr = sqlcomm.ExecuteReader();

                    while (dr.Read())
                    {
                        newItem = new System.Web.UI.WebControls.ListItem();
                        newItem.Text = dr["vendor_name"].ToString();
                        newItem.Value = dr["id"].ToString();
                        ddlList.Items.Add(newItem);
                    }
                    dr.Close();
                }
                catch (Exception err)
                {
                    //TODO
                }
                finally
                {
                    sqlcomm.Dispose();
                    Con.Close();
                    Con.Dispose();
                }
            }

        }

        protected void BindDataTableItemRF()
        {
            string id = Request.QueryString["rf_no"];
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewDetailRFInPutVendor");
            sqlcomm.Parameters.AddWithValue("@rf_no", id);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            ViewState["myViewState"] = dtb;
            TableDetailsRF.DataSource = dtb;
            TableDetailsRF.DataBind();

            TableDetailsRF.Columns[7].Visible = false;
            TableDetailsRF.Columns[8].Visible = false;
            TableDetailsRF.Columns[9].Visible = false;

            TableDetailsRF.UseAccessibleHeader = true;
            TableDetailsRF.HeaderRow.TableSection = TableRowSection.TableHeader;

            Con.Close();
        }

        #region StatusBarDetail
        protected void GetDataPriceEstimatedApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusPriceEstimated");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("PriceEstInput", (string)dr["PriceEstInput"]);
                Session.Add("tgl_approve_price_est", (DateTime)dr["tgl_approve_price_est"]);
                Session.Add("approval_status_price_est", (string)dr["approval_status_price_est"]);
            }
        }

        protected void GetDataMgrApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusMgrApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("MgrApproval", (string)dr["MgrApproval"]);
                Session.Add("tgl_approve_mgr", (DateTime)dr["tgl_approve_mgr"]);
                Session.Add("approval_status_mgr", (string)dr["approval_status_mgr"]);
            }

        }

        protected void GetDataMgrApprovalFull()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusMgrApprovalFull");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("MgrApproval_full", (string)dr["MgrApproval_full"]);
                Session.Add("tgl_approve_mgr_full", (DateTime)dr["tgl_approve_mgr_full"]);
                Session.Add("approval_status_mgr_full", (string)dr["approval_status_mgr_full"]);
            }

        }

        protected void GetDataMgrApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusMgrApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("MgrApprovalreject", (string)dr["MgrApprovalreject"]);
                Session.Add("tgl_approve_mgr_reject", (DateTime)dr["tgl_approve_mgr_reject"]);
                Session.Add("approval_status_mgr_reject", (string)dr["approval_status_mgr_reject"]);
            }

        }

        protected void GetDataDivGMApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivGMApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivGMApproval", (string)dr["DivGMApproval"]);
                Session.Add("tgl_approve_DivGM", (DateTime)dr["tgl_approve_DivGM"]);
                Session.Add("approval_status_DivGM", (string)dr["approval_status_DivGM"]);
            }

        }

        protected void GetDataDivGMApprovalFull()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivGMApprovalFull");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivGMApprovalFull", (string)dr["DivGMApprovalFull"]);
                Session.Add("tgl_approve_DivGMFull", (DateTime)dr["tgl_approve_DivGMFull"]);
                Session.Add("approval_status_DivGMFull", (string)dr["approval_status_DivGMFull"]);
            }

        }

        protected void GetDataDivGMApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivGMApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivGMApprovalreject", (string)dr["DivGMApprovalreject"]);
                Session.Add("tgl_approve_DivGM_reject", (DateTime)dr["tgl_approve_DivGM_reject"]);
                Session.Add("approval_status_DivGM_reject", (string)dr["approval_status_DivGM_reject"]);
            }

        }

        protected void GetDataDeputyDirectorApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDeputyDirectorApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DeputyDirectorApproval", (string)dr["DeputyDirectorApproval"]);
                Session.Add("tgl_approve_DeputyDirector", (DateTime)dr["tgl_approve_DeputyDirector"]);
                Session.Add("approval_status_DeputyDirector", (string)dr["approval_status_DeputyDirector"]);
            }

        }

        protected void GetDataDeputyDirectorApprovalFull()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDeputyDirectorApprovalFull");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DeputyDirectorApprovalFull", (string)dr["DeputyDirectorApprovalFull"]);
                Session.Add("tgl_approve_DeputyDirectorFull", (DateTime)dr["tgl_approve_DeputyDirectorFull"]);
                Session.Add("approval_status_DeputyDirectorFull", (string)dr["approval_status_DeputyDirectorFull"]);
            }

        }

        protected void GetDataDeputyDirectorApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDeputyDirectorApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DeputyDirectorApprovalreject", (string)dr["DeputyDirectorApprovalreject"]);
                Session.Add("tgl_approve_DeputyDirector_reject", (DateTime)dr["tgl_approve_DeputyDirector_reject"]);
                Session.Add("approval_status_DeputyDirector_reject", (string)dr["approval_status_DeputyDirector_reject"]);
            }

        }

        protected void GetDataDivisionDirectorApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivisionDirectorApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivisionDirectorApproval", (string)dr["DivisionDirectorApproval"]);
                Session.Add("tgl_approve_DivisionDirector", (DateTime)dr["tgl_approve_DivisionDirector"]);
                Session.Add("approval_status_DivisionDirector", (string)dr["approval_status_DivisionDirector"]);
            }

        }

        protected void GetDataDivisionDirectorApprovalFull()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivisionDirectorApprovalFull");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivisionDirectorApprovalFull", (string)dr["DivisionDirectorApprovalFull"]);
                Session.Add("tgl_approve_DivisionDirectorFull", (DateTime)dr["tgl_approve_DivisionDirectorFull"]);
                Session.Add("approval_status_DivisionDirectorFull", (string)dr["approval_status_DivisionDirectorFull"]);
            }

        }

        protected void GetDataDivisionDirectorApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDivisionDirectorApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DivisionDirectorApprovalreject", (string)dr["DivisionDirectorApprovalreject"]);
                Session.Add("tgl_approve_DivisionDirector_reject", (DateTime)dr["tgl_approve_DivisionDirector_reject"]);
                Session.Add("approval_status_DivisionDirector_reject", (string)dr["approval_status_DivisionDirector_reject"]);
            }

        }

        protected void GetDataITHeadApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusITHeadApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("ITHeadApproval", (string)dr["ITHeadApproval"]);
                Session.Add("tgl_approve_ITHead", (DateTime)dr["tgl_approve_ITHead"]);
                Session.Add("approval_status_ITHead", (string)dr["approval_status_ITHead"]);
            }

        }

        protected void GetDataITHeadApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusITHeadApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("ITHeadApprovalreject", (string)dr["ITHeadApprovalreject"]);
                Session.Add("tgl_approve_ITHead_reject", (DateTime)dr["tgl_approve_ITHead_reject"]);
                Session.Add("approval_status_ITHead_reject", (string)dr["approval_status_ITHead_reject"]);
            }

        }

        protected void GetDataGAHeadApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGAHeadApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GAHeadApproval", (string)dr["GAHeadApproval"]);
                Session.Add("tgl_approve_GAHead", (DateTime)dr["tgl_approve_GAHead"]);
                Session.Add("approval_status_GAHead", (string)dr["approval_status_GAHead"]);
            }

        }

        protected void GetDataGAHeadApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGAHeadApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GAHeadApprovalreject", (string)dr["GAHeadApprovalreject"]);
                Session.Add("tgl_approve_GAHead_reject", (DateTime)dr["tgl_approve_GAHead_reject"]);
                Session.Add("approval_status_GAHead_reject", (string)dr["approval_status_GAHead_reject"]);
            }

        }

        protected void GetDataGMAdminFullApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGMAdminFullApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GMAdminApprovalFull", (string)dr["GMAdminApprovalFull"]);
                Session.Add("tgl_approve_GMAdminFull", (DateTime)dr["tgl_approve_GMAdminFull"]);
                Session.Add("approval_status_GMAdminFull", (string)dr["approval_status_GMAdminFull"]);
            }

        }

        protected void GetDataGMAdminApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGMAdminApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GMAdminApproval", (string)dr["GMAdminApproval"]);
                Session.Add("tgl_approve_GMAdmin", (DateTime)dr["tgl_approve_GMAdmin"]);
                Session.Add("approval_status_GMAdmin", (string)dr["approval_status_GMAdmin"]);
            }

        }

        protected void GetDataGMAdminApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusGMAdminApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("GMAdminApprovalreject", (string)dr["GMAdminApprovalreject"]);
                Session.Add("tgl_approve_GMAdmin_reject", (DateTime)dr["tgl_approve_GMAdmin_reject"]);
                Session.Add("approval_status_GMAdmin_reject", (string)dr["approval_status_GMAdmin_reject"]);
            }

        }

        protected void GetDataDirectorAdminApproval()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDirectorAdminApproval");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DirectorAdminApproval", (string)dr["DirectorAdminApproval"]);
                Session.Add("tgl_approve_DirectorAdmin", (DateTime)dr["tgl_approve_DirectorAdmin"]);
                Session.Add("approval_status_DirectorAdmin", (string)dr["approval_status_DirectorAdmin"]);
            }

        }

        protected void GetDataDirectorAdminApprovalReject()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusDirectorAdminApprovalReject");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("DirectorAdminApprovalreject", (string)dr["DirectorAdminApprovalreject"]);
                Session.Add("tgl_approve_DirectorAdmin_reject", (DateTime)dr["tgl_approve_DirectorAdmin_reject"]);
                Session.Add("approval_status_DirectorAdmin_reject", (string)dr["approval_status_DirectorAdmin_reject"]);
            }

        }

        protected void GetDataCancelRFl()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_ApprovalRequisitionForm";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewStatusCancelRF");
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);

            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("CanceledBy", (string)dr["CanceledBy"]);
                Session.Add("tgl_approve_Cancel", (DateTime)dr["tgl_approve_Cancel"]);
                Session.Add("ApprovalStatus", (string)dr["ApprovalStatus"]);
            }

        }
        #endregion

        #region Barcode
        private void GenerateAndDisplayBarcode()
        {
            // Generate barcode
            string baseUrl = "https://172.19.160.3:8585/ylid-purchasing/document_validation.aspx"; // URL tujuan untuk QR code
            string id = lbRFNumberBreadcrumb.Text; // Nilai ID yang akan digunakan dalam URL

            // Membuat URL dengan parameter
            string data = $"{baseUrl}?rf_no={Uri.EscapeDataString(id)}";

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
            string folderPath = Server.MapPath("~/Barcode/RF");
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
            string fileName = "barcode_rf.png";  // You can customize the file name
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

        protected void btnDownloadRF_Click(object sender, EventArgs e)
        {
            #region Code_Old
            //if (Session["status_approve"].ToString() == "NOT YET")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Price Checked")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_PriceEstimate_Under1Juta_SUBSRG";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPriceEstimated_Under1Juta_SUB_SRG.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ManagerApproved";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseManagerDivisionApproved.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Division GM)")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GMDivisionApproved";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGMDivisionApproved.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_DeputyDirectorApproved";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseDeputyDirectorApproved.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Division Director)")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_DivisionDirectorApproved";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseDivisionDirectorApproved.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            //else if (Session["status_approve"].ToString() == "Approved (IT Head)")
            //{
            //    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ITHeadApproved_SendGAHead";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseITHeadApproved_SendToGAHead.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //    else if (Session["DirectorApprove"] is null)
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ITHeadApproved_SendGAHead_DirNull";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseITHeadApproved_SendToGAHead_DirNull.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //    else
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ITHeadApproved_SendGAHead_DirNotNull";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseITHeadApproved_SendToGAHead_DirNotNull.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //}
            //else if (Session["status_approve"].ToString() == "Approved (GA Head)")
            //{
            //    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
            //    {
            //        if ((Session["catalog_type"].ToString() == "GA" || Session["catalog_type"].ToString() == "OPS"))
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GAHeadApproved_SendGMAdmin";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGAHeadApproved_SendToGMAdmin.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //        else
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GAHeadApproved_SendGMAdmin_IT";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGAHeadApproved_SendToGMAdmin_IT.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //    }
            //    else if (Session["DirectorApprove"] is null)
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GAHeadApproved_SendGMAdmin_DirNull";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGAHeadApproved_SendToGMAdmin_DirNull.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //    else
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GAHeadApproved_SendGMAdmin_DirNotNull";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGAHeadApproved_SendToGMAdmin_DirNotNull.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Admin GM)")
            //{
            //    if (Session["catalog_type"].ToString() == "IT")
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GMAdminApproved_SendToDirectorAdm_IT";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGMAdminApproved_SendToAdminDirector_IT.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //    else
            //    {
            //        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //        SqlConnection Con = new SqlConnection(path);
            //        Con.Open();
            //        SqlCommand sqlcomm = new SqlCommand();
            //        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GMAdminApproved_SendToDirectorAdm_GA";
            //        sqlcomm.CommandType = CommandType.StoredProcedure;
            //        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //        sqlcomm.Connection = Con;
            //        DataTable dtb = new DataTable();
            //        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //        sda.Fill(dtb);
            //        GenerateAndDisplayBarcode();
            //        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGMAdminApproved_SendToAdminDirector_GA.rdlc");
            //        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //        ReportViewerPurchase.LocalReport.DataSources.Clear();
            //        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //        ReportViewerPurchase.LocalReport.Refresh();

            //        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //        string extension;
            //        string encoding;
            //        string mimeType;
            //        string[] streams;
            //        Warning[] warnings;
            //        Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                        out extension, out encoding,
            //                        out mimeType, out streams, out warnings);
            //        Response.Buffer = true;
            //        Response.Clear();
            //        Response.ContentType = mimeType;
            //        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //        Response.BinaryWrite(mybytes); // create the file
            //        Response.Flush();
            //    }
            //}
            //else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
            //{
            //    if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_IT_GMNull_DepDirNull_DirNull";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_IT_GMNull_DepDirNull_DirNull.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //        else
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_GA_GMNull_DepDirNull_DirNull";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_GA_GMNull_DepDirNull_DirNull.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //    }
            //    else if (Session["DirectorApprove"] is null)
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_IT_DirNull";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_IT_DirNull.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //        else
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_GA_DirNull";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_GA_DirNull.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //    }
            //    else
            //    {
            //        if (Session["catalog_type"].ToString() == "IT")
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_IT";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_IT.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //        else
            //        {
            //            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //            SqlConnection Con = new SqlConnection(path);
            //            Con.Open();
            //            SqlCommand sqlcomm = new SqlCommand();
            //            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_GA";
            //            sqlcomm.CommandType = CommandType.StoredProcedure;
            //            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //            sqlcomm.Connection = Con;
            //            DataTable dtb = new DataTable();
            //            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //            sda.Fill(dtb);
            //            GenerateAndDisplayBarcode();
            //            ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //            ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_GA.rdlc");
            //            ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //            ReportViewerPurchase.LocalReport.DataSources.Clear();
            //            ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //            ReportViewerPurchase.LocalReport.Refresh();

            //            string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //            string extension;
            //            string encoding;
            //            string mimeType;
            //            string[] streams;
            //            Warning[] warnings;
            //            Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                            out extension, out encoding,
            //                            out mimeType, out streams, out warnings);
            //            Response.Buffer = true;
            //            Response.Clear();
            //            Response.ContentType = mimeType;
            //            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //            Response.BinaryWrite(mybytes); // create the file
            //            Response.Flush();
            //        }
            //    }

            //}
            //else if (Session["status_approve"].ToString() == "Reject (Division Manager)")
            //{
            //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            //    SqlConnection Con = new SqlConnection(path);
            //    Con.Open();
            //    SqlCommand sqlcomm = new SqlCommand();
            //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

            //    sqlcomm.Connection = Con;
            //    DataTable dtb = new DataTable();
            //    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            //    sda.Fill(dtb);
            //    GenerateAndDisplayBarcode();
            //    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
            //    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
            //    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
            //    ReportViewerPurchase.LocalReport.DataSources.Clear();
            //    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
            //    ReportViewerPurchase.LocalReport.Refresh();

            //    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
            //    string extension;
            //    string encoding;
            //    string mimeType;
            //    string[] streams;
            //    Warning[] warnings;
            //    Byte[] mybytes = ReportViewerPurchase.LocalReport.Render("PDF", null,
            //                    out extension, out encoding,
            //                    out mimeType, out streams, out warnings);
            //    Response.Buffer = true;
            //    Response.Clear();
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            //    Response.BinaryWrite(mybytes); // create the file
            //    Response.Flush();
            //}
            #endregion

            #region Code_New
            if (Session["status_approve"].ToString() == "NOT YET")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_Purchase";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchase.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Price Checked")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_PriceEstimate_Under1Juta_SUBSRG";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPriceEstimated_Under1Juta_SUB_SRG.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Approved (Division Manager)")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_ManagerApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseManagerDivisionApproved.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Approved (Division GM)")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_GMDivisionApproved_SendToDeputyDirector";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseGMDivisionApproved_SendToDeputyDirector.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Approved (Deputy Director)")
            {
                if (Session["status"].ToString() == "Canceled")
                {
                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    SqlConnection Con = new SqlConnection(path);
                    Con.Open();
                    SqlCommand sqlcomm = new SqlCommand();
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                    sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_DepDirApproved";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                    sqlcomm.Connection = Con;
                    DataTable dtb = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                    sda.Fill(dtb);
                    GenerateAndDisplayBarcode();
                    ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                    ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseDepDirApproved.rdlc");
                    ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                    ReportViewerPurchase.LocalReport.DataSources.Clear();
                    ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                    ReportViewerPurchase.LocalReport.Refresh();

                    string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Approved (Fully Approved)")
            {
                if (Session["GMApprove"] is null && Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_ManagerDiv";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_ManagerDiv.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                else if (Session["DeputyDirectorApprove"] is null && Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_GMDiv";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_GMDiv.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                else if (Session["DirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_DepDir";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_DepDir.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                else if (Session["DeputyDirectorApprove"] is null)
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved_DepDirNull";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved_DepDirNull.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                else
                {
                    if (Session["status"].ToString() == "Canceled")
                    {
                        string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                        SqlConnection Con = new SqlConnection(path);
                        Con.Open();
                        SqlCommand sqlcomm = new SqlCommand();
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
                        sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_RF_FullApproved";
                        sqlcomm.CommandType = CommandType.StoredProcedure;
                        sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                        sqlcomm.Connection = Con;
                        DataTable dtb = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                        sda.Fill(dtb);
                        GenerateAndDisplayBarcode();
                        ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                        ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseFullApproved.rdlc");
                        ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                        ReportViewerPurchase.LocalReport.DataSources.Clear();
                        ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                        ReportViewerPurchase.LocalReport.Refresh();

                        string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            }
            else if (Session["status_approve"].ToString() == "Reject (Division Manager)" || Session["status_approve"].ToString() == "Cancel (Division Manager)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                GenerateAndDisplayBarcode();
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Reject (Division GM)" || Session["status_approve"].ToString() == "Cancel (Division GM)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                GenerateAndDisplayBarcode();
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Reject (Deputy Director)" || Session["status_approve"].ToString() == "Cancel (Deputy Director)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                GenerateAndDisplayBarcode();
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Reject (Division Director)" || Session["status_approve"].ToString() == "Cancel (Division Director)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                GenerateAndDisplayBarcode();
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            else if (Session["status_approve"].ToString() == "Reject (Fully Approved)" || Session["status_approve"].ToString() == "Cancel (Fully Approved)")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_Attachment_PurchaseRejectCancel";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text.Trim());

                sqlcomm.Connection = Con;
                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                GenerateAndDisplayBarcode();
                ReportViewerPurchase.ProcessingMode = ProcessingMode.Local;
                ReportViewerPurchase.LocalReport.ReportPath = Server.MapPath("~/Prints/PrintFormPurchaseRejectCancel.rdlc");
                ReportViewerPurchase.LocalReport.EnableExternalImages = true;
                ReportViewerPurchase.LocalReport.DataSources.Clear();
                ReportViewerPurchase.LocalReport.DataSources.Add(new ReportDataSource("DataSetFormPurchase", dtb));
                ReportViewerPurchase.LocalReport.Refresh();

                string FileName = "Requisition Form - " + lbRFNumberHeader.Text + ".pdf";
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
            #endregion
        }

        protected void submitrow(string vendor,  int getprice, string item_code)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "UpdateRF");
            sqlcomm.Parameters.AddWithValue("@id_vendor", vendor);
            sqlcomm.Parameters.AddWithValue("@price", getprice);
            sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);
            sqlcomm.Parameters.AddWithValue("@item_code", item_code);

            sqlcomm.ExecuteNonQuery();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);
            sqlcomm.Dispose();
            Con.Close();
            Con.Dispose();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow grow in TableDetailsRF.Rows)
            {
                DropDownList dlList = (DropDownList)grow.FindControl("ddlVendor");
                string selectedvalue = dlList.SelectedItem.Value;
                HtmlInputText price = (HtmlInputText)grow.FindControl("txtPrice");
                decimal parsedValue = decimal.Parse(price.Value, NumberStyles.Currency);
                int getprice = Convert.ToInt32(parsedValue);
                string item_code = grow.Cells[0].Text;
                submitrow(selectedvalue, getprice, item_code);
            }
            string id = Request.QueryString["rf_no"];
            Response.Redirect("create_purchase_order_standart.aspx?rf_no=" + id);

        }


        //baru 
        //protected void btnGenerate_Click(object sender, EventArgs e)
        //{

        //    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
        //    SqlConnection Con = new SqlConnection(path);
        //    Con.Open();
        //    SqlCommand sqlcomm = new SqlCommand();
        //    sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
        //    sqlcomm.CommandType = CommandType.StoredProcedure;
        //    sqlcomm.Connection = Con;
        //    sqlcomm.Parameters.AddWithValue("@StatementType", "GenerateRFtoPO");
        //    sqlcomm.Parameters.AddWithValue("@rf_no", lbRFNumberHeader.Text);
           
        //    sqlcomm.ExecuteNonQuery();

        //    sqlcomm.Dispose();
        //    Con.Close();
        //    Con.Dispose();

        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

        //}



        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TableDetailsRF_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableDetailsRF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.Cells[9].Text.ToString() == "&nbsp;")
            //    {
            //        HtmlInputText price = (HtmlInputText)e.Row.FindControl("txtPrice");
            //        DropDownList vendor = (DropDownList)e.Row.FindControl("ddlVendor");
            //        price.Disabled = false;
            //        vendor.Enabled = true;
            //    }
            //    else
            //    {
            //        HtmlInputText price = (HtmlInputText)e.Row.FindControl("txtPrice");
            //        DropDownList vendor = (DropDownList)e.Row.FindControl("ddlVendor");
            //        price.Disabled = true;
            //        vendor.Enabled = false;
            //    }
            //}
        }

        protected void TableDetailsRF_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}