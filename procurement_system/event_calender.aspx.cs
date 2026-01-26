using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;




namespace procurement_system
{
    public partial class event_calender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void GetCheck()
        {
            if (txtMonth.Value == "")
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_MasterEventCalender";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StatementType", "ViewAll");

                sqlcomm.Connection = Con;

                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ViewState["myViewState"] = dtb;
                TableEventCalender.DataSource = dtb;
                TableEventCalender.DataBind();

                TableEventCalender.Columns[1].Visible = false;

                TableEventCalender.UseAccessibleHeader = true;
                TableEventCalender.HeaderRow.TableSection = TableRowSection.TableHeader;

                Con.Close();
            }
            else
            {
                string _Submonth = txtMonth.Value;
                string _getMonth = _Submonth.Substring(0, _Submonth.Length - 5);

                string _Subyears = txtMonth.Value;
                string _getYears = _Subyears.Substring(3, 4);



                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_MasterEventCalender";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StatementType", "CheckByMonthYear");
                sqlcomm.Parameters.AddWithValue("@month", _getMonth);
                sqlcomm.Parameters.AddWithValue("@years", _getYears);

                sqlcomm.Connection = Con;

                DataTable dtb = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);

                sda.Fill(dtb);
                ViewState["myViewState"] = dtb;
                TableEventCalender.DataSource = dtb;
                TableEventCalender.DataBind();

                TableEventCalender.Columns[1].Visible = false;

                TableEventCalender.UseAccessibleHeader = true;
                TableEventCalender.HeaderRow.TableSection = TableRowSection.TableHeader;

                Con.Close();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(
                    Page, Page.GetType(),
                    "modal",
                    "$('#mdlAddEvent').modal(); initDatePicker();",
                    true
            );
        

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddEvent').modal();", true);
            //GetCheck();
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        private async Task syncHoliday ()
        {
            try
            {
                string apiUrl = "https://api-harilibur.vercel.app/api";

                using (var client = new HttpClient())
                {
                    var json = await client.GetStringAsync(apiUrl).ConfigureAwait(false);
                    JArray data = JArray.Parse(json);

                    string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(path))
                    {
                        con.Open();

                        foreach (var item in data)
                        {
                            bool isHoliday = item["is_national_holiday"].Value<bool>();
                            if (!isHoliday) continue;

                            DateTime holidayDate = DateTime.Parse(item["holiday_date"].ToString());
                            string holidayName = item["holiday_name"].ToString();

                            using (SqlCommand sqlcomm = new SqlCommand("sp_PROCUREMENT_DB_MasterEventCalender", con))
                            {
                                sqlcomm.CommandType = CommandType.StoredProcedure;

                                sqlcomm.Parameters.AddWithValue("@StatementType", "Savesync");
                                sqlcomm.Parameters.AddWithValue("@EventDate", holidayDate); // ✅ holiday_date
                                sqlcomm.Parameters.AddWithValue("@EventRemark", holidayName); // ✅ holiday_name
                                sqlcomm.Parameters.AddWithValue("@User", Session["nik"].ToString());
                                sqlcomm.Parameters.AddWithValue("@Active", true);

                                sqlcomm.ExecuteNonQuery();
                            }
                        }
                    }
                }

                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sync data libur nasional selesai!');", true);
                string script = $@"
                                $(document).ready(function() {{
                                // Show Toastr notification
                                toastr.success('Sync data national holiday sucess', 'Success');

                                // Redirect after 2 seconds (2000 milliseconds)
                                setTimeout(function() {{
                                    window.location.href = 'event_calender.aspx'; // replace with your target URL
                                }}, 2000);
                            }});
                        ";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
            }
            catch (Exception ex)
            {
                string script = $@"
                            $(document).ready(function() {{
                                // Show Toastr notification
                                toastr.error({ex.Message}, 'Error');

                                // Redirect after 2 seconds (2000 milliseconds)
                                setTimeout(function() {{
                                    window.location.href = 'event_calender.aspx'; // replace with your target URL
                                }}, 2000);
                            }});
                        ";

                // Register the script for partial postbacks
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);

                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }


        protected void btnsync_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('btnsync_Click terpanggil!');", true);
            syncHoliday().GetAwaiter().GetResult();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddEvent').modal();", true);
            GetCheck();
            btnSubmit.Visible = false;
            btnUpdate.Visible = true;
            txtEventDate.Disabled = true;

            txtOid.Value = row.Cells[1].Text;
            txtEventDate.Value = row.Cells[2].Text;
            txtEventRemark.Value = row.Cells[3].Text;
            ckActive.Checked = Convert.ToBoolean(row.Cells[4].Text.ToString());
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            GetCheck();
        }

        public bool CheckDataInput(string check_date)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_MasterEventCalender";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckDataInput");
            sqlcomm.Parameters.AddWithValue("@check_date", check_date);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();



            if (dr.Read())
            {

                Session.Add("Oid", (Guid)dr["Oid"]);
                Session.Add("EventDate", (DateTime)dr["EventDate"]);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isexistsData = CheckDataInput(txtEventDate.Value.ToString());

            if (isexistsData)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed. Event date is already exist.');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddEvent').modal();", true);
                GetCheck();
                btnSubmit.Visible = true;
                btnUpdate.Visible = false;
            }
            else if (txtEventDate.Value=="" || txtEventRemark.Value=="" || ckActive.Checked==false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Submit Failed. The field cannot be empty.');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddEvent').modal();", true);
                GetCheck();
                btnSubmit.Visible = true;
                btnUpdate.Visible = false;
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_MasterEventCalender";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Save");
                sqlcomm.Parameters.AddWithValue("@EventDate", txtEventDate.Value);
                sqlcomm.Parameters.AddWithValue("@EventRemark", txtEventRemark.Value);
                sqlcomm.Parameters.AddWithValue("@User", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@Active", ckActive.Checked);

                sqlcomm.ExecuteNonQuery();
                string script = $@"
                            $(document).ready(function() {{
                                // Show Toastr notification
                                toastr.success('Your operation was successful, Please wait to redirect the page!', 'Submit Success');

                                // Redirect after 2 seconds (2000 milliseconds)
                                setTimeout(function() {{
                                    window.location.href = 'event_calender.aspx'; // replace with your target URL
                                }}, 2000);
                            }});
                        ";

                // Register the script for partial postbacks
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                sqlcomm.Dispose();
                sqlcomm.Parameters.Clear();
                Con.Close();
                Con.Dispose();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtEventDate.Value == "" || txtEventRemark.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Update Failed. The field cannot be empty.');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddEvent').modal();", true);
                GetCheck();
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                txtEventDate.Disabled = true;
            }
            else
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                SqlConnection Con = new SqlConnection(path);
                Con.Open();
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "sp_PROCUREMENT_DB_MasterEventCalender";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Connection = Con;
                sqlcomm.Parameters.AddWithValue("@StatementType", "Update");
                sqlcomm.Parameters.AddWithValue("@EventRemark", txtEventRemark.Value);
                sqlcomm.Parameters.AddWithValue("@User", Session["nik"].ToString());
                sqlcomm.Parameters.AddWithValue("@Active", ckActive.Checked);
                sqlcomm.Parameters.AddWithValue("@Oid", txtOid.Value);

                sqlcomm.ExecuteNonQuery();
                string script = $@"
                            $(document).ready(function() {{
                                // Show Toastr notification
                                toastr.success('Your operation was successful, Please wait to redirect the page!', 'Update Success');

                                // Redirect after 2 seconds (2000 milliseconds)
                                setTimeout(function() {{
                                    window.location.href = 'event_calender.aspx'; // replace with your target URL
                                }}, 2000);
                            }});
                        ";

                // Register the script for partial postbacks
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                sqlcomm.Dispose();
                sqlcomm.Parameters.Clear();
                Con.Close();
                Con.Dispose();
            }
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("event_calender.aspx");
        }
    }
}