using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace procurement_system
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDataUser();
            lbFullname.InnerText = Session["Fullname"].ToString();
            lblLocation.Text = Session["Location"].ToString();
            lbNIK.Text = Session["nik"].ToString();
            lblOid.Text = Session["Oid"].ToString();
            lblOidEmployees.Text = Session["Employees"].ToString();
            lblDivision.Text = Session["Division"].ToString();
            lblSection.Text = Session["Section"].ToString();
            lblPosition.Text = Session["Position"].ToString();

            if (!IsPostBack)
            {
                CheckUserNav(Session["Oid"].ToString());
            }
            //--RF--//
            GetDataEstimatePriceRFNotif();
            lblnotifEstimatePriceRF.Text = Session["NeedEstimatePriceRFNotif"].ToString();

            GetDataNeedCreatePONotif();
            lblnotifNeedCreatePO.Text = Session["NeedCreatePONotif"].ToString();

            GetDataNeedApproveManagerDivisionRFNotif();
            lblnotifNeedApproveManagerDivisionRF.Text = Session["NeedApproveManagerDivisionRFNotif"].ToString();

            GetDataNeedApproveGMDivisionRFNotif();
            lblnotifNeedApproveGMDivisionRF.Text = Session["NeedApproveGMDivisionRFNotif"].ToString();

            GetDataNeedApproveDeputyDirectorRFNotif();
            lblnotifNeedApproveDeputyDirectorRF.Text = Session["NeedApproveDeputyDirectorRFNotif"].ToString();

            GetDataNeedApproveDivisionDirectorRFNotif();
            lblnotifNeedApproveDivisionDirectorRF.Text = Session["NeedApproveDivisionDirectorRFNotif"].ToString();

            GetDataNeedApproveITHeadRFNotif();
            lblnotifNeedApproveITHeadRF.Text = Session["NeedApproveITHeadNotifRF"].ToString();

            GetDataNeedApproveGAHeadRFNotif();
            lblnotifNeedApproveGAHeadRF.Text = Session["NeedApproveGAHeadNotifRF"].ToString();

            GetDataNeedApproveGMAdminRFNotif();
            lblnotifNeedApproveGMAdminRF.Text = Session["NeedApproveGMAdminNotifRF"].ToString();

            GetDataNeedApproveDirectorAdminRFNotif();
            lblnotifNeedApproveDirectorAdminRF.Text = Session["NeedApproveDirectorAdminNotifRF"].ToString();

            //--------------------PO------------------//
            GetDataNeedApproveGAHeadPONotif();
            lblnotifNeedApproveGAHeadPO.Text = Session["NeedApproveGAHeadNotifPO"].ToString();

            GetDataNeedApproveITHeadPONotif();
            lblnotifNeedApproveITHeadPO.Text = Session["NeedApproveITHeadNotifPO"].ToString();

            GetDataNeedApproveGMAdminPONotif();
            lblnotifNeedApproveGMAdminPO.Text = Session["NeedApproveGMAdminNotifPO"].ToString();

            GetDataNeedApproveDirectorPONotif();
            lblnotifNeedApproveDirectorPO.Text = Session["NeedApproveDirectorNotifPO"].ToString();

            //View Details---RF----------------------//:
            GetDataViewDetailEstimatePriceRFNotif();
            GetDataViewDetailPOCreateNotif();
            GetDataViewDetailNeedApproveManagerDivisionRFNotif();
            GetDataViewDetailNeedApproveGMDivisionRFNotif();
            GetDataViewDetailNeedApproveDeputyDirectorRFNotif();
            GetDataViewDetailNeedApproveDivisionDirectorRFNotif();
            GetDataViewDetailNeedITHeadApprovedRFNotif();
            GetDataViewDetailNeedGAHeadApprovedRFNotif();
            GetDataViewDetailNeedGMAdminApprovedRFNotif();
            GetDataViewDetailNeedDirectorAdminApprovedRFNotif();

            //View Details---PO----------------------//:
            GetDataViewDetailNeedGAHeadApprovedPONotif();
            GetDataViewDetailNeedITHeadApprovedPONotif();
            GetDataViewDetailNeedGMAdminApprovedPONotif();
            GetDataViewDetailNeedDirectorApprovedPONotif();


            //View Total Notif:
            if (lbNIK.Text== "891011" || lbNIK.Text == "891163")
            {
                int notif = Convert.ToInt32(lblnotifEstimatePriceRF.Text) + Convert.ToInt32(lblnotifNeedCreatePO.Text) + Convert.ToInt32(lblnotifNeedApproveManagerDivisionRF.Text);
                lblnotif.Text = notif.ToString();
                lblShowTotalNotif.Text = notif.ToString();

                if (notif == 0)
                {
                    //-----RF-----//
                    numberNotif.Visible = false;
                    ViewEstimatePriceRFNotif.Visible = false;
                    ViewPOCreateNotif.Visible = false;
                    ViewNeedApproveManagerDivisionRFNotif.Visible = false;
                    ViewNeedApproveITHeadRFNotif.Visible=false;
                    ViewNeedApproveGAHeadRFNotif.Visible = false;
                    ViewNeedApproveGMDivisionRFNotif.Visible = false;
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = false;
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = false;
                    ViewNeedApproveGMAdminRFNotif.Visible = false;
                    ViewNeedApproveDirectorAdminRFNotif.Visible = false;
                    //-----PO-----//
                    ViewNeedApproveGAHeadPONotif.Visible = false;
                    ViewNeedApproveITHeadPONotif.Visible = false;
                    ViewNeedApproveGMAdminPONotif.Visible = false;
                    ViewNeedApproveDirectorPONotif.Visible = false;
                }
                else
                {
                    //-----RF-----//
                    numberNotif.Visible = true;
                    ViewEstimatePriceRFNotif.Visible = true;
                    ViewPOCreateNotif.Visible = true;
                    ViewNeedApproveManagerDivisionRFNotif.Visible = true;
                    ViewNeedApproveITHeadRFNotif.Visible = false;
                    ViewNeedApproveGAHeadRFNotif.Visible = false;
                    ViewNeedApproveGMDivisionRFNotif.Visible = false;
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = false;
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = false;
                    ViewNeedApproveGMAdminRFNotif.Visible = false;
                    ViewNeedApproveDirectorAdminRFNotif.Visible = false;
                    //-----PO-----//
                    ViewNeedApproveGAHeadPONotif.Visible = false;
                    ViewNeedApproveITHeadPONotif.Visible = false;
                    ViewNeedApproveGMAdminPONotif.Visible = false;
                    ViewNeedApproveDirectorPONotif.Visible = false;
                    txtNotif.Text = lblnotif.Text.ToString();

                }

                //-----RF-----//
                if (Convert.ToInt32(lblnotifEstimatePriceRF.Text) == 0)
                {
                    ViewEstimatePriceRFNotif.Visible = false;
                }
                else
                {
                    ViewEstimatePriceRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedCreatePO.Text) == 0)
                {
                    ViewPOCreateNotif.Visible = false;
                }
                else
                {
                    ViewPOCreateNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveManagerDivisionRF.Text) == 0)
                {
                    ViewNeedApproveManagerDivisionRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveManagerDivisionRFNotif.Visible = true;
                }

            }
            else
            {
                int notif = Convert.ToInt32(lblnotifNeedApproveManagerDivisionRF.Text) + Convert.ToInt32(lblnotifNeedApproveGMDivisionRF.Text) + Convert.ToInt32(lblnotifNeedApproveITHeadRF.Text)
                            + Convert.ToInt32(lblnotifNeedApproveGAHeadRF.Text) + Convert.ToInt32(lblnotifNeedApproveDeputyDirectorRF.Text) + Convert.ToInt32(lblnotifNeedApproveDivisionDirectorRF.Text)
                            + Convert.ToInt32(lblnotifNeedApproveGMAdminRF.Text) + Convert.ToInt32(lblnotifNeedApproveDirectorAdminRF.Text) + Convert.ToInt32(lblnotifNeedApproveGAHeadPO.Text)
                            + Convert.ToInt32(lblnotifNeedApproveITHeadPO.Text) + Convert.ToInt32(lblnotifNeedApproveGMAdminPO.Text) + Convert.ToInt32(lblnotifNeedApproveDirectorPO.Text);
                lblnotif.Text = notif.ToString();
                lblShowTotalNotif.Text = notif.ToString();

                if (notif == 0)
                {
                    //-----------------------RF---------------------------------//
                    numberNotif.Visible = false;
                    ViewEstimatePriceRFNotif.Visible = false;
                    ViewPOCreateNotif.Visible = false;
                    ViewNeedApproveManagerDivisionRFNotif.Visible = false;
                    ViewNeedApproveGMDivisionRFNotif.Visible = false;
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = false;
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = false;
                    ViewNeedApproveITHeadRFNotif.Visible = false;
                    ViewNeedApproveGAHeadRFNotif.Visible = false;
                    ViewNeedApproveGMAdminRFNotif.Visible = false;
                    ViewNeedApproveDirectorAdminRFNotif.Visible = false;
                    //-----------------------PO---------------------------------//
                    ViewNeedApproveGAHeadPONotif.Visible = false;
                    ViewNeedApproveITHeadPONotif.Visible = false;
                    ViewNeedApproveGMAdminPONotif.Visible = false;
                    ViewNeedApproveDirectorPONotif.Visible = false;
                }
                else
                {
                    //-----------------------RF---------------------------------//
                    numberNotif.Visible = true;
                    ViewEstimatePriceRFNotif.Visible = false;
                    ViewPOCreateNotif.Visible = false;
                    ViewNeedApproveGMDivisionRFNotif.Visible = true;
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = true;
                    ViewNeedApproveManagerDivisionRFNotif.Visible = true;
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = true;
                    ViewNeedApproveITHeadRFNotif.Visible = true;
                    ViewNeedApproveGAHeadRFNotif.Visible = true;
                    ViewNeedApproveGMAdminRFNotif.Visible = true;
                    ViewNeedApproveDirectorAdminRFNotif.Visible = true;
                    //-----------------------PO---------------------------------//
                    ViewNeedApproveGAHeadPONotif.Visible = false;
                    ViewNeedApproveITHeadPONotif.Visible = false;
                    ViewNeedApproveGMAdminPONotif.Visible = false;
                    ViewNeedApproveDirectorPONotif.Visible = false;
                    txtNotif.Text = lblnotif.Text.ToString();
                }

                //-----------------------RF---------------------------------//
                if (Convert.ToInt32(lblnotifNeedApproveManagerDivisionRF.Text) == 0)
                {
                    ViewNeedApproveManagerDivisionRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveManagerDivisionRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveGMDivisionRF.Text) == 0)
                {
                    ViewNeedApproveGMDivisionRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveGMDivisionRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveDeputyDirectorRF.Text) == 0)
                {
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveDeputyDirectorRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveDivisionDirectorRF.Text) == 0)
                {
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveDivisionDirectorRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveITHeadRF.Text) == 0)
                {
                    ViewNeedApproveITHeadRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveITHeadRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveGAHeadRF.Text) == 0)
                {
                    ViewNeedApproveGAHeadRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveGAHeadRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveGMAdminRF.Text) == 0)
                {
                    ViewNeedApproveGMAdminRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveGMAdminRFNotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveDirectorAdminRF.Text) == 0)
                {
                    ViewNeedApproveDirectorAdminRFNotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveDirectorAdminRFNotif.Visible = true;
                }

                //-----------------------PO---------------------------------//
                if (Convert.ToInt32(lblnotifNeedApproveGAHeadPO.Text) == 0)
                {
                    ViewNeedApproveGAHeadPONotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveGAHeadPONotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveITHeadPO.Text) == 0)
                {
                    ViewNeedApproveITHeadPONotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveITHeadPONotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveGMAdminPO.Text) == 0)
                {
                    ViewNeedApproveGMAdminPONotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveGMAdminPONotif.Visible = true;
                }

                if (Convert.ToInt32(lblnotifNeedApproveDirectorPO.Text) == 0)
                {
                    ViewNeedApproveDirectorPONotif.Visible = false;
                }
                else
                {
                    ViewNeedApproveDirectorPONotif.Visible = true;
                }
            }
        }

        protected void GetDataUser()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_UserManagement";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckUser");
            sqlcomm.Parameters.AddWithValue("@id", Session["Employees"].ToString());
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("Fullname", (string)dr["Fullname"]);
                Session.Add("GroupName", (string)dr["GroupName"]);
            }
            else
            {

            }
        }

        private void CheckUserNav(string vNIK)
        {
            user_management.Visible = false;
            user_registration.Visible = false;
            module_registration.Visible = false;
            group_registration.Visible = false;
            menu_master_data.Visible = false;
            category.Visible = false;
            item.Visible = false;
            merk.Visible = false;
            goods_catalog.Visible = false;
            stock.Visible = false;
            unit_of_measurement.Visible = false;
            vendor_menu.Visible = false;
            vendor_category.Visible = false;
            vendor_detail.Visible = false;
            menu_orders.Visible = false;
            requisition_form.Visible = false;
            requisition_check.Visible = false;
            purchase_order.Visible = false;
            goods_received.Visible = false;
            menu_approval.Visible = false;
            app_rf.Visible = false;
            app_po.Visible = false;
            vendor_detail.Visible = false;

            string vUserCode = vNIK;

            string vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection vCon = new SqlConnection(vPath);
            SqlCommand vSQLComm = new SqlCommand();
            vSQLComm.Connection = vCon;

            vCon.Open();

            vSQLComm.CommandText = "sp_PROCUREMENT_DB_UserControl";
            vSQLComm.CommandType = CommandType.StoredProcedure;
            vSQLComm.Parameters.AddWithValue("@Code", vUserCode);
            vSQLComm.Parameters.AddWithValue("@Flag", "ModuleNav");

            SqlDataReader vDR = vSQLComm.ExecuteReader();
            if (vDR.HasRows)
            {
                while (vDR.Read())
                {
                    if (vDR["ModuleName"].ToString().Trim() == "user_management") { user_management.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "user_registration") { user_registration.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "module_registration") { module_registration.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "group_registration") { group_registration.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "menu_master_data") { menu_master_data.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "category") { category.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "item") { item.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "merk") { merk.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "goods_catalog") { goods_catalog.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "stock") { stock.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "unit_of_measurement") { unit_of_measurement.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "vendor_menu") { vendor_menu.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "vendor_category") { vendor_category.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "vendor_detail") { vendor_detail.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "menu_orders") { menu_orders.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "requisition_form") { requisition_form.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "requisition_check") { requisition_check.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "purchase_order") { purchase_order.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "goods_received") { goods_received.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "menu_approval") { menu_approval.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "app_rf") { app_rf.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "app_po") { app_po.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                    if (vDR["ModuleName"].ToString().Trim() == "event_calender") { event_calender.Visible = Convert.ToBoolean(vDR["RoleNavigation"]); }
                }
            }
        }

        protected void Logout(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("login.aspx");
        }

        #region NotifCodeRF
        protected void GetDataEstimatePriceRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedEstimatePrice");
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedEstimatePriceRFNotif", (Int32)dr["NeedEstimatePriceRFNotif"]);
            }
            Con.Close();
        }

        protected void GetDataNeedCreatePONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RF_NeedCreatePO");
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedCreatePONotif", (Int32)dr["NeedCreatePONotif"]);
            }
            Con.Close();
        }

        protected void GetDataNeedApproveManagerDivisionRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveManagerDivision");
            sqlcomm.Parameters.AddWithValue("@nik_approver", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveManagerDivisionRFNotif", (Int32)dr["NeedApproveManagerDivisionRFNotif"]);
            }
            Con.Close();
        }

        protected void GetDataNeedApproveGMDivisionRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveGMDivision");
            sqlcomm.Parameters.AddWithValue("@nik_gm_approver", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveGMDivisionRFNotif", (Int32)dr["NeedApproveGMDivisionRFNotif"]);
            }
            Con.Close();
        }

        protected void GetDataNeedApproveDeputyDirectorRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveDeputyDirector");
            sqlcomm.Parameters.AddWithValue("@nik_deputy_director", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveDeputyDirectorRFNotif", (Int32)dr["NeedApproveDeputyDirectorRFNotif"]);
            }
            Con.Close();
        }

        protected void GetDataNeedApproveDivisionDirectorRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveDivisionDirector");
            sqlcomm.Parameters.AddWithValue("@nik_director", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveDivisionDirectorRFNotif", (Int32)dr["NeedApproveDivisionDirectorRFNotif"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailEstimatePriceRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_Notification_RequestPurchase_NeedEstimatePrice");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewEstimatePriceRFNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataViewDetailPOCreateNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotifRFNumberNeedCreatePO");
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewPOCreateNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            string _VText = sb.ToString();
            if (!string.IsNullOrEmpty(_VText) && _VText.Length >= 44 + 17)
            {
                lbRFNumber.Text = _VText.Substring(44, 17);
            }
            
            Con.Close();
        }

        protected void GetDataViewDetailNeedApproveManagerDivisionRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_Notification_RequestPurchase_NeedApproveManagerDivision");
            sqlcomm.Parameters.AddWithValue("@nik_approver", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveManagerDivisionNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataViewDetailNeedApproveGMDivisionRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_Notification_RequestPurchase_NeedApproveGMDivision");
            sqlcomm.Parameters.AddWithValue("@nik_gm_approver", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveGMDivisionNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataViewDetailNeedApproveDeputyDirectorRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_Notification_RequestPurchase_NeedApproveDeputyDirector");
            sqlcomm.Parameters.AddWithValue("@nik_deputy_director", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveDeputyDirectorNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataViewDetailNeedApproveDivisionDirectorRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "View_Notification_RequestPurchase_NeedApproveDivisionDirector");
            sqlcomm.Parameters.AddWithValue("@nik_director", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveDivisionDirectorNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveITHeadRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveITHead");
            sqlcomm.Parameters.AddWithValue("@nik_it_manager", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveITHeadNotifRF", (Int32)dr["NeedApproveITHeadNotifRF"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedITHeadApprovedRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_RequestPurchase_NeedApproveITHead");
            sqlcomm.Parameters.AddWithValue("@nik_it_manager", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveITHeadNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveGAHeadRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveGAHead");
            sqlcomm.Parameters.AddWithValue("@nik_adm_manager", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveGAHeadNotifRF", (Int32)dr["NeedApproveGAHeadNotifRF"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedGAHeadApprovedRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_RequestPurchase_NeedApproveGAHead");
            sqlcomm.Parameters.AddWithValue("@nik_adm_manager", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveGAHeadNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveGMAdminRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveGMAdmin");
            sqlcomm.Parameters.AddWithValue("@nik_adm_gm", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveGMAdminNotifRF", (Int32)dr["NeedApproveGMAdminNotifRF"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedGMAdminApprovedRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_RequestPurchase_NeedApproveGMAdmin");
            sqlcomm.Parameters.AddWithValue("@nik_adm_gm", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveGMAdminNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveDirectorAdminRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_RequestPurchase_NeedApproveDirectorAdmin");
            sqlcomm.Parameters.AddWithValue("@nik_adm_director", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveDirectorAdminNotifRF", (Int32)dr["NeedApproveDirectorAdminNotifRF"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedDirectorAdminApprovedRFNotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Purchase";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_RequestPurchase_NeedApproveDirectorAdmin");
            sqlcomm.Parameters.AddWithValue("@nik_adm_director", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewRFNeedApproveDirectorAdminNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }
        #endregion

        #region NotifCodePO
        protected void GetDataNeedApproveGAHeadPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_PO_NeedApproveGAHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveGAHeadNotifPO", (Int32)dr["NeedApproveGAHeadNotifPO"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedGAHeadApprovedPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_PO_NeedApproveGAHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewPONeedApproveGAHeadNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveITHeadPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_PO_NeedApproveITHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by_it", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveITHeadNotifPO", (Int32)dr["NeedApproveITHeadNotifPO"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedITHeadApprovedPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_PO_NeedApproveITHead");
            sqlcomm.Parameters.AddWithValue("@po_checked_by_it", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewPONeedApproveITHeadNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveGMAdminPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_PO_NeedApproveGMAdmin");
            sqlcomm.Parameters.AddWithValue("@po_approved_by", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveGMAdminNotifPO", (Int32)dr["NeedApproveGMAdminNotifPO"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedGMAdminApprovedPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_PO_NeedApproveGMAdmin");
            sqlcomm.Parameters.AddWithValue("@po_approved_by", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewPONeedApproveGMAdminNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }

        protected void GetDataNeedApproveDirectorPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "Notification_PO_NeedApproveDirector");
            sqlcomm.Parameters.AddWithValue("@authorized_by", lbNIK.Text);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("NeedApproveDirectorNotifPO", (Int32)dr["NeedApproveDirectorNotifPO"]);
            }
            Con.Close();
        }

        protected void GetDataViewDetailNeedDirectorApprovedPONotif()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_PurchaseOrder";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "ViewNotification_PO_NeedApproveDirector");
            sqlcomm.Parameters.AddWithValue("@authorized_by", lbNIK.Text);
            DataTable dtb = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

            sda.Fill(dtb);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table table-striped table-bordered>");

            foreach (DataRow dr in dtb.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dtb.Columns)
                {
                    sb.Append("<th>");
                    sb.Append(dr[dc.ColumnName].ToString());
                    sb.Append("</th>");
                }
            }
            sb.Append("</table>");
            PanelViewPONeedApproveDirectorNotif.Controls.Add(new LiteralControl { Text = sb.ToString() });
            Con.Close();
        }
        #endregion

        protected void TabEstimatePrice_Click(object sender, EventArgs e)
        {
            Response.Redirect("requisition_price_check.aspx");
        }

        protected void TabManagerDivision_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "division_manager";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabGMDivision_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "division_gm";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabDeputyDirector_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "deputy_director";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabDivisionDirector_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "director";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabITHead_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "it_section_head";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabGAHead_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "ga_section_head";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabGMAdmin_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "gm_adm";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void TabDirectorAdmin_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "director_adm";
            Response.Redirect("approval_requisition_form.aspx"); // Pindah ke halaman lain
        }

        protected void GetRFNumberNeedCreatePO()
        {
            string _vPath = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection _vSQLCon = new SqlConnection(_vPath);
            SqlCommand _vSQLComm = new SqlCommand();

            _vSQLCon.Open();
            _vSQLComm.Connection = _vSQLCon;
            _vSQLComm.CommandType = CommandType.StoredProcedure;
            _vSQLComm.CommandText = "sp_PROCUREMENT_DB_Purchase";

            _vSQLComm.Parameters.AddWithValue("@StatementType", "ViewNotifRFNumberNeedCreatePO");

            DataTable _dt = new DataTable();
            SqlDataReader _dr = _vSQLComm.ExecuteReader();
            _dt.Load(_dr);

            TableRFNumber.DataSource = _dt;
            TableRFNumber.DataBind();

            _dt.Dispose();
            _vSQLCon.Close();
            _vSQLComm.Dispose();
            _vSQLCon.Dispose();

            _vSQLCon.Close();
        }

        protected void TabFixedPrice_Click(object sender, EventArgs e)
        {
            //Response.Redirect("input_vendor.aspx?rf_no=" + lbRFNumber.Text);
            GetRFNumberNeedCreatePO();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlSelectRFNumber').modal();", true);
        }

        protected void TabGAHeadPO_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "ga_section_head";
            Response.Redirect("approval_purchase_order.aspx"); // Pindah ke halaman lain
        }

        protected void TabITHeadPO_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "it_section_head";
            Response.Redirect("approval_purchase_order.aspx"); // Pindah ke halaman lain
        }

        protected void TabGMAdminPO_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "gm_adm";
            Response.Redirect("approval_purchase_order.aspx"); // Pindah ke halaman lain
        }

        protected void TabDirectorPO_Click(object sender, EventArgs e)
        {
            // Simpan nilai tab aktif ke dalam sesi
            Session["ActiveTab"] = "director";
            Response.Redirect("approval_purchase_order.aspx"); // Pindah ke halaman lain
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {

        }
    }
}