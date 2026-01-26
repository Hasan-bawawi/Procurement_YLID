using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using DataTable = System.Data.DataTable;
using System.Drawing;



namespace procurement_system
{
    public partial class ReportPO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        
        {
            if (!IsPostBack)
            {
                LoadFilter();
                LoadDashboardData();
            }
        }

        protected void FilterChanged(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadFilter()
        {
            // Tahun dropdown
            for (int y = DateTime.Now.Year - 10; y <= DateTime.Now.Year + 1; y++)
                ddlYear.Items.Add(y.ToString());
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            // Bulan dropdown
            ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            for (int m = 1; m <= 12; m++)
                ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(
                    new DateTime(2000, m, 1).ToString("MMM"), m.ToString()));
            ddlMonth.SelectedValue = "0";
        }

        private void LoadDashboardData()
        {
            int year = int.Parse(ddlYear.SelectedValue);
            int month = int.Parse(ddlMonth.SelectedValue);

            string cs = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDashboardData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    dynamic chartData = null;
                    if (dr.Read())
                    {
                         chartData = new
                        {
                            OTD_Hit = dr["OTD_Hit"],
                            OTD_Miss = dr["OTD_Miss"],
                            Months = dr["Months"].ToString().Split(','),
                             //SupplierNames = dr["SupplierNames"].ToString().Split(','),
                             //SupplierCount = Array.ConvertAll(dr["SupplierCount"].ToString().Split(','), int.Parse)
                             SupplierNames = dr["SupplierNames"].ToString()
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToArray(),

                            SupplierCount = dr["SupplierCount"].ToString()
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s =>
                            {
                                int val;
                                return int.TryParse(s.Trim(), out val) ? val : 0;
                            })
                            .ToArray()

                    }
                    ;

                    }

                    List<object> rawData = new List<object>();
                    if (dr.NextResult())
                    {
                        while (dr.Read())
                        {
                            rawData.Add(new
                            {
                                po = dr["PO"].ToString(),
                                leadTime = Convert.ToInt32(dr["LeadTime"])
                            });
                        }
                    }

                    var dashboardData = new
                    {
                        chartData,
                        rawData
                    };

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    hfChartData.Value = js.Serialize(dashboardData);

                }
            }
        }

        #region Code Lama
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                string periode = "";
                string endperiode = "";

                if (txtDate2.Value != null && txtDate2.Value != "")
                {
                    string a = txtDate2.Value.Replace(" ", "");
                    string[] parts = a.Split('-');

                    periode = parts[0] + parts[1];

                }
                if (txtDate3.Value != null && txtDate3.Value != "")
                {
                    string a = txtDate3.Value.Replace(" ", "");
                    string[] parts = a.Split('-');

                    endperiode = parts[0] + parts[1];

                }

                DataTable dt = GetReportData(periode, endperiode);

                if (dt.Rows.Count == 0)
                {


                    string errorMessage = "Choose periode correctly for get data to export.";
                    Response.Cookies["fileDownload"].Value = "failed";
                    Response.Cookies["errorMessage"].Value = errorMessage;
                    Response.Cookies["fileDownload"].Path = "/";
                    Response.Cookies["errorMessage"].Path = "/";
                    Response.Flush();
                    return;



                }

                // 3️⃣ Buat file Excel pakai ClosedXML
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Report PO PDR");

                    DateTime startDate = DateTime.ParseExact(periode, "MMyyyy", null);
                    DateTime endDate = DateTime.ParseExact(endperiode, "MMyyyy", null);

                    // Format bulan menjadi singkatan English otomatis
                    string bulanStart = startDate.ToString("MMM");   // Sept
                    string bulanEnd = endDate.ToString("MMM");     // Dec

                    string tahunStart = startDate.Year.ToString();   // 2025
                    string tahunEnd = endDate.Year.ToString();

                    string title;

                    // Jika tahun sama
                    if (tahunStart == tahunEnd)
                    {
                        if (bulanStart == bulanEnd)
                        {

                            title = $"Report PO PDR ({bulanStart} - {tahunStart})";
                        }
                        else
                        {
                            title = $"Report PO PDR ({bulanStart} – {bulanEnd} {tahunStart})";
                        }
                    }
                    else
                    {
                        title = $"Report PO PDR ({bulanStart} {tahunStart} – {bulanEnd} {tahunEnd})";
                    }

                    // Masukkan ke Excel
                    ws.Cell(1, 1).Value = title;


                    ws.Range("A1:E2").Merge().Style.Font.SetBold().Font.FontSize = 16;
                    ws.Range("A1:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A1:E2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    ws.Range("A1:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(4, 1).InsertTable(dt);

                    ws.Columns().AdjustToContents();

                    ws.Column("C").Width = 18;
                    ws.Column("D").Width = 18;


                    int firstDataRow = 5;
                    int lastDataRow = firstDataRow + dt.Rows.Count - 1;

                    var rangeAA = ws.Range($"Z{firstDataRow}:Z{lastDataRow}");


                    ws.Range($"M{firstDataRow}:S{lastDataRow}").Style.NumberFormat.Format = "#,##0";

                    rangeAA.AddConditionalFormat()
                        .WhenEquals("HIT")
                        .Fill.SetBackgroundColor(XLColor.Green)
                        .Font.SetFontColor(XLColor.White);

                    // Jika MISS → merah dengan teks putih
                    rangeAA.AddConditionalFormat()
                        .WhenEquals("MISS")
                        .Fill.SetBackgroundColor(XLColor.Red)
                        .Font.SetFontColor(XLColor.White);

                    ws.SheetView.FreezeColumns(7);




                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=Report PO PDR.xlsx");

                        // tulis stream
                        memoryStream.WriteTo(Response.OutputStream);

                        // tambahkan cookie penanda sukses download
                        Response.Cookies.Add(new HttpCookie("fileDownload", "success")
                        {
                            Path = "/",
                            Expires = DateTime.Now.AddMinutes(5)
                        });

                        Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                }
            }
            catch (Exception ex)
            {

                string errorMessage = ex.Message;
                Response.Cookies["fileDownload"].Value = "failed";
                Response.Cookies["errorMessage"].Value = errorMessage;
                Response.Cookies["fileDownload"].Path = "/";
                Response.Cookies["errorMessage"].Path = "/";
                Response.Flush();
                return;

            }

        }
        #endregion

        #region Code Coba"
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string periode = "", endperiode = "";

        //        if (!string.IsNullOrEmpty(txtDate2.Value))
        //        {
        //            var p = txtDate2.Value.Replace(" ", "").Split('-');
        //            periode = p[0] + p[1];
        //        }

        //        if (!string.IsNullOrEmpty(txtDate3.Value))
        //        {
        //            var p = txtDate3.Value.Replace(" ", "").Split('-');
        //            endperiode = p[0] + p[1];
        //        }

        //        DataTable dt = GetReportData(periode, endperiode);
        //        if (dt.Rows.Count == 0)
        //            throw new Exception("Data Not Found.");

        //        DateTime startDate = DateTime.ParseExact(periode, "MMyyyy", null);
        //        DateTime endDate = DateTime.ParseExact(endperiode, "MMyyyy", null);

        //        string title = startDate.Year == endDate.Year
        //            ? (startDate.Month == endDate.Month
        //                ? $"Report PO PDR ({startDate:MMM yyyy})"
        //                : $"Report PO PDR ({startDate:MMM} – {endDate:MMM} {startDate:yyyy})")
        //            : $"Report PO PDR ({startDate:MMM yyyy} – {endDate:MMM yyyy})";

        //        using (var wb = new XLWorkbook())
        //        {
        //            var ws = wb.Worksheets.Add("Report PO PDR");

        //            // ======================
        //            // TITLE
        //            // ======================
        //            var header = ws.Range("A1:E2");
        //            header.Merge();
        //            header.Value = title;
        //            header.Style.Font.Bold = true;
        //            header.Style.Font.FontSize = 16;
        //            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        //            header.Style.Fill.BackgroundColor = XLColor.LightBlue;
        //            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        //            // ======================
        //            // HEADER MANUAL
        //            // ======================
        //            int headerRow = 4;
        //            for (int i = 0; i < dt.Columns.Count; i++)
        //            {
        //                var cell = ws.Cell(headerRow, i + 1);
        //                cell.Value = dt.Columns[i].ColumnName;
        //                cell.Style.Font.Bold = true;
        //                cell.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
        //                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //            }

        //            // ======================
        //            // DATA (AMAN)
        //            // ======================
        //            int dataStartRow = headerRow + 1;
        //            ws.Cell(dataStartRow, 1).InsertData(dt);

        //            int lastRow = dataStartRow + dt.Rows.Count - 1;

        //            // ======================
        //            // FORMAT NUMBER
        //            // ======================
        //            ws.Range($"M{dataStartRow}:S{lastRow}").Style.NumberFormat.Format = "#,##0";

        //            // ======================
        //            // CONDITIONAL FORMAT (TEXT)
        //            // ======================
        //            var zRange = ws.Range($"Z{dataStartRow}:Z{lastRow}");
        //            zRange.Style.NumberFormat.Format = "@";

        //            zRange.AddConditionalFormat()
        //                  .WhenEquals("HIT")
        //                  .Fill.SetBackgroundColor(XLColor.Green)
        //                  .Font.SetFontColor(XLColor.White);

        //            zRange.AddConditionalFormat()
        //                  .WhenEquals("MISS")
        //                  .Fill.SetBackgroundColor(XLColor.Red)
        //                  .Font.SetFontColor(XLColor.White);

        //            // ======================
        //            // FREEZE (SAFE)
        //            // ======================
        //            if (dt.Columns.Count >= 7)
        //                ws.SheetView.FreezeColumns(7);

        //            ws.Columns().AdjustToContents();

        //            // ======================
        //            // DOWNLOAD
        //            // ======================
        //            //using (var ms = new MemoryStream())
        //            //{
        //            //    wb.SaveAs(ms);
        //            //    ms.Position = 0;

        //            //    Response.Clear();
        //            //    Response.Buffer = true;
        //            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            //    Response.AddHeader("content-disposition", "attachment;filename=Report PO PDR.xlsx");

        //            //    // tulis stream
        //            //    ms.WriteTo(Response.OutputStream);

        //            //    // tambahkan cookie penanda sukses download
        //            //    Response.Cookies.Add(new HttpCookie("fileDownload", "success")
        //            //    {
        //            //        Path = "/",
        //            //        Expires = DateTime.Now.AddMinutes(5)
        //            //    });

        //            //    Response.Flush();
        //            //    HttpContext.Current.ApplicationInstance.CompleteRequest();

        //            //}

        //            using (var ms = new MemoryStream())
        //            {
        //                wb.SaveAs(ms);

        //                byte[] fileBytes = ms.ToArray();

        //                Response.Clear();
        //                Response.ClearContent();
        //                Response.ClearHeaders();

        //                Response.Buffer = true;
        //                Response.ContentType =
        //                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //                Response.AddHeader(
        //                    "Content-Disposition",
        //                    "attachment; filename=\"Report PO PDR.xlsx\""
        //                );

        //                Response.BinaryWrite(fileBytes);

        //                Response.Cookies.Add(new HttpCookie("fileDownload", "success")
        //                {
        //                    Path = "/",
        //                    Expires = DateTime.Now.AddMinutes(5)
        //                });

        //                Response.Flush();

        //                // HENTIKAN REQUEST TANPA ThreadAbortException
        //                HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Cookies["fileDownload"].Value = "failed";
        //        Response.Cookies["errorMessage"].Value = ex.Message;
        //        Response.Cookies["fileDownload"].Path = "/";
        //        Response.Cookies["errorMessage"].Path = "/";
        //        Response.Flush();
        //    }
        //}
        #endregion


        private DataTable GetReportData(string startDate, string endDate)
        {
            DataTable dt = new DataTable();
            string connStr = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("sp_PROCUREMENT_DB_ReportPO", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(startDate) ? (object)DBNull.Value : startDate);
                    cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(endDate) ? (object)DBNull.Value : endDate);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }



    }
}
