using System;
using System.IO;
using System.Web;



namespace procurement_system
{
    public class DeleteAttachmentHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                string path = context.Request["path"];
                if (string.IsNullOrEmpty(path))
                {
                    context.Response.Write("{\"success\": false, \"message\": \"Path not find.\"}");
                    return;
                }

                string fullPath = context.Server.MapPath("~/" + path);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    context.Response.Write("{\"success\": true}");
                }
                else
                {
                    context.Response.Write("{\"success\": false, \"message\": \"File not find in server.\"}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false, \"message\": \"" + ex.Message.Replace("\"", "'") + "\"}");
            }
        }

        public bool IsReusable => false;
    }
}
