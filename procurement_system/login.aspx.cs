using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace procurement_system
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "InfoBox();", true);
            }
        }

        private string Decrypt(string clearText)
        {
            string EncryptionKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public bool IsAuthenticated(string username, string password)
        {
            string domain = "ylid.local";
            //string username = txtUserName.Text;
            //string password = txtPassword.Text;
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry("LDAP://DC=ylid,DC=local", domainAndUsername, password);



            try
            {
                // Bind to the native AdsObject to force authentication.
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(sAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                //search.PropertiesToLoad.Add("mail");
                //search.PropertiesToLoad.Add("givenname");
                //search.PropertiesToLoad.Add("sn");
                //search.PropertiesToLoad.Add("title");
                //search.PropertiesToLoad.Add("telephoneNumber");
                SearchResult result = search.FindOne();
                if (null == result)
                {

                    return false;
                }
                else { return true; }
                // Update the new path to the user in the directory
                //_path = result.Path;
                //_filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {

                //throw new Exception("Error authenticating user. " + ex.Message);
                return false;
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Func1()", true);
            }

        }

        public bool CheckUser(string username, string password, int _vFlag)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_PROCUREMENT_DB_Login";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@Username", username);
            sqlcomm.Parameters.AddWithValue("@Password", /*password*/Decrypt(password));
            sqlcomm.Parameters.AddWithValue("@Flag", _vFlag);
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();



            if (dr.Read())
            {
                Session.Add("Oid", (Guid)dr["Oid"]);
                Session.Add("nik", (string)dr["nik"]);
                Session.Add("username_AD", (string)dr["username_AD"]);
                Session.Add("UserName", (string)dr["UserName"]);
                Session.Add("StoredPassword", (string)dr["StoredPassword"]);
                Session.Add("CreateDate", (DateTime)dr["CreateDate"]);
                Session.Add("fullname", (string)dr["fullname"]);
                Session.Add("Position", (string)dr["Position"].ToString());
                Session.Add("Section", (string)dr["Section"].ToString());
                Session.Add("Division", (string)dr["Division"].ToString());
                Session.Add("Location", (string)dr["Location"]);
                Session.Add("GroupName", (string)dr["GroupName"].ToString());
                Session.Add("Employees", (Guid)dr["Employees"]);
                Session.Add("email_karyawan", (string)dr["email_karyawan"]);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

                //bool isexists = CheckUser(txtusername.Value, txtpass.Value);
                bool isexistsAD = IsAuthenticated(txtusername.Value, txtpassword.Value);
                //if (isexistsAD && Session["level_user"].ToString() == "ADMIN")
                if (isexistsAD)
                {

                    bool isexists = CheckUser(txtusername.Value, txtpassword.Value, 1);
                    if (isexists)
                    {
                        string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'index.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                            Response.Redirect(ReturnUrl);
                        }
                        else
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'index.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                        }
                            

                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "FailedLogin();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Login failed, Username or Password incorrect, please try again!');", true);
                    }

                }
                else
                {

                    bool isexists = CheckUser(txtusername.Value, txtpassword.Value, 0);
                    if (isexists)
                    {
                        string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'index.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                            Response.Redirect(ReturnUrl);
                        }
                        else
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'index.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                        }
                            
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "FailedLogin();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Login failed, Username or Password incorrect, please try again!');", true);
                    }
                }


            }
        }
    }
}