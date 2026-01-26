using System;
using System.Web;
using System.IO;

namespace procurement_system
{
    public class UploadAttachmentHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpPostedFile file = context.Request.Files["fileAttachment"];
                var vendorId = context.Request["vendorId"];
                string namavendor = context.Request["namavendor"];
                string rf = context.Request["RF"];

                if (file == null || file.ContentLength == 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("No file uploaded.");
                    return;
                }

                //string poNumber = context.Request.Form["PONumber"];
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(fileName);

                string folder = context.Server.MapPath("~/eDocs_Files/PO/");

                if (!Directory.Exists(folder))Directory.CreateDirectory(folder);

                //string uniqueName = /*Path.GetFileNameWithoutExtension(fileName) + "-" +*/ rf + "-" +  namavendor + fileExt;

                string uniqueName = $"{rf}-{namavendor}{fileExt}";
                string fullPath = Path.Combine(folder, uniqueName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath); // Hapus file lama
                }

                //string fullPath = Path.Combine(folder, uniqueName);
                file.SaveAs(fullPath);

                string relativePath = $"eDocs_Files/PO/{uniqueName}";
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"success\": true,\"fileName\": \"" + fileName + "\", \"path\": \"" + relativePath.Replace("\\", "/") + "\"}");



            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("Error: " + ex.Message);
            }
        }

        public bool IsReusable => false;
    }
}
