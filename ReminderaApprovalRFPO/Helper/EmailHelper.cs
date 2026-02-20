using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;


namespace ReminderaApprovalRFPO.Helper
{
    public static class EmailHelper
    {
        public static async Task SendReminderGraph(
            string toEmail,
            List<string> documentList,
            string type,
            string approvename)
        {
            try
            {
                string clientId = ConfigurationManager.AppSettings["clientId"];
                string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                string tenantId = ConfigurationManager.AppSettings["tenantId"];
                string endpoint = ConfigurationManager.AppSettings["endpoint"];

                string authority = $"https://login.microsoftonline.com/{tenantId}";
                var scopes = new[] { "https://graph.microsoft.com/.default" };

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(authority)
                    .Build();

                var token = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                string body = BuildBody(documentList, type, approvename);

                var emailObject = new
                {
                    message = new
                    {
                        subject = $"Reminder {type} Need Approve!!",
                        body = new
                        {
                            contentType = "HTML",
                            content = body
                        },
                        toRecipients = new[]
                        {
                        new { emailAddress = new { address = toEmail } }
                        //new { emailAddress = new { address = "hasan.bawawi@id.yusen-logistics.com" } }
                        },
                        CcRecipients = new[]
                        {
                            new { emailAddress = new { address = "YLID.ML.IT@id.yusen-logistics.com" } }
                        }
                    },
                    saveToSentItems = true
                };

                var json = JsonConvert.SerializeObject(emailObject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine($"Email sent to {toEmail}");
                    Logger.Log($"SUCCESS - {type} reminder sent to {toEmail}");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    Logger.Log($"FAILED - {type} reminder to {toEmail}");
                    Logger.Log(error);

                    throw new Exception($"Graph API Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"ERROR sending {type} reminder to {toEmail}");
                Logger.Log(ex);

                throw; 
            }
        }

        private static string BuildBody(List<string> list, string type, string approverName)
        {

            string htmlemail = type == "PO"  ? "EmailReminderPO.html" : "EmailReminderRF.html";

            //string templatePath = Path.Combine(@"D:\procurement_system\ReminderaApprovalRFPO\Templates",htmlemail);

            string templatePath = Path.Combine( AppContext.BaseDirectory,"Templates",htmlemail);


            //string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates",htmlemail);
            //Console.WriteLine(File.Exists(templatePath));

            if (!File.Exists(templatePath))
            {
                Logger.Log("Template file not found: " + templatePath);
                throw new FileNotFoundException("Email template not found.");
            }

            string body = File.ReadAllText(templatePath);

            string noListHtml = string.Join("<br/>", list);

            //body = body.Replace("{Nolist}", noListHtml);

            body = body.Replace("{nama}", approverName ?? "");
            body = body.Replace("{Nolist}", noListHtml ?? "");


            return body;
        }
    }

}
