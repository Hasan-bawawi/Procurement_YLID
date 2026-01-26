<%@ Page Title="Unit of Measurement" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup ="true" CodeBehind="ReportPO.aspx.cs" Inherits="procurement_system.ReportPO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        fieldset.scheduler-border {
            border: 1px groove #ff6d10 !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            box-shadow: none;
        }
        legend.scheduler-border {
            width: auto;
            padding: 0 10px;
            border-bottom: none;
            color: #06183d;
        }
        .buttonColor {
            background-color: #06183d;
            color: white;
        }
        .buttonColor:hover {
            background-color: #ff6d10;
            color: white;
        }
        .page-head {
            background-color: #06183d;
            color: white;
            padding: 1px 2.938rem;
            height: 182px;
            position: relative;
            margin-top: 0;
            margin-left: 0;
        }
        .custom-card-body {
            position: relative;
            top: -99px;
        }
        .form-label {
            font-weight: 600;
            color: #06183d;
        }

        .form-control {
            border-radius: 8px;
            border: 1px solid #ccc;
            padding: 6px 10px;
        }

        .dtp-actual-num,
        .dtp-picker-days,
        .dtp-calendar {
            display: none !important;
        }

        .dtp-header {
            background-color: #ff6d10 !important;
        }


        .dtp-btn-ok, .dtp-btn-cancel, .dtp-btn-clear{
            background-color: #06183d !important;
            color: #ff6d10 !important;
        }

        .dtp-btn-ok,
        .dtp-btn-cancel,
        .dtp-btn-clear {
            border: 1px solid #ff6d10 !important;
            outline: none !important;
            box-shadow: none !important;
            margin-left: 8px;
        }

        .dtp-date{
            background-color: #ff6d10 !important;
            color: black !important;

        }


        .dtp-content {
                background-color: #06183d !important;
            }

        .dtp-select-month-before,
        .dtp-select-month-after,
        .dtp-select-year-before,
        .dtp-select-year-after {
            color: black !important;
        }

        .dtp-header .dtp-actual-day {
            display: none !important;
        }

        .dtp-header .dtp-close {
            top: -1px;
            right: 10px;
            position: absolute;
        }

        .dtp-header {
            height: 25px !important; 
            position: relative;
            background-color: #ff6d10 !important; 
        }


        .wide-input {
        width: 300px;
        }

        #chartLeadTime {
            display: block;
        }

        .row > [class*='col-'] {
            padding-bottom: 0 !important;
        }
    </style>

    <!-- Tambahkan Chart.js -->
     <link href="vendors/toastr/toastr.min.css" rel="stylesheet" />
     <script src="vendors/toastr/jquery.min.js"></script>
     <script src="vendors/toastr/toastr.min.js"></script>
     <link href="vendors/sweetalert.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
     <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Report PO</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa-solid fa-server"></i>&nbsp;Report</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Report PO</li>
        </ol>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <!-- 🔽 Filter Tahun dan Bulan -->
                        <div class="row mb-3" style="margin-bottom: 20px;">
                            <div class="col-md-3">
                                <label for="ddlYear" class="form-label">Tahun</label>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" 
                                                  AutoPostBack="true" OnSelectedIndexChanged="FilterChanged" />
                            </div>
                            <div class="col-md-3">
                                <label for="ddlMonth" class="form-label">Bulan</label>
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"
                                                  AutoPostBack="true" OnSelectedIndexChanged="FilterChanged" />
                            </div>
                        </div>
                        <!-- 🔽 Baris Pertama Grafik -->

                        <div class="row align-items-start">
                            <!-- Chart kiri -->
                            <div class="col-md-4">
                                <canvas id="chartOTD"></canvas>
                            </div>

                            <!-- Chart kanan -->
                            <div class="col-md-8">
                                <div style="width: 100%; height: 250px;">
                                    <canvas id="chartLeadTime"></canvas>
                                </div>
                            </div>
                        </div>
<%--                        <div class="row">
                            <div class="col-md-4">
                                <canvas id="chartOTD"></canvas>
                            </div>
                            
                            <div class="col-md-4">
                                <canvas id="chartSupplier"></canvas>
                            </div>

                             <div class="col-md-4">
                                <canvas id="chartLeadTime"></canvas>
                            </div>     
                            <div style="width: 900px; height: 250px;">
                                <canvas id="chartLeadTime"></canvas>
                            </div>
                        </div>--%>

                        <!-- 🔽 Baris Kedua Grafik -->
                   <%--  <div class="row mt-4">
                          <div class="col-md-6">
                             <canvas id="chartLeadTime"></canvas>
                         </div>                           
                      </div>--%>

                        <div class="col-xl-12 mt-5">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body"> <!-- card 1  -->
                                    <div class="row no-gutters align-items-center">
                                        <div class="col-12">
                                            <div class="form-group">
                                            <%-- <div style="display: inline-block; background-color: #06183d; transform: skew(-10deg); padding: 5px 10px;">--%>
<%--                                                        <label for="txtDate4" 
                                                               class="font-weight-bold" 
                                                               style="font-size: 30px; font-style: italic; color: white; display: inline-block; transform: skew(10deg);">
                                                            Report PO
                                                        </label>--%>
                                                    <%--</div>--%>
                                                <div class="row mt-4">
                                                    <!-- Periode -->
                                                    <div class="col-md-3">
                                                        <label for="txtDate2" class="font-weight-bold" style="font-size: 20px;">Start Periode</label>
                                                        <div class="input-group">
                                                            <input type="text"
                                                                   id="txtDate2"
                                                                   class="form-control shadow-sm datepicker1 start"
                                                                   runat="server"
                                                                   placeholder="Select Month & Year"
                                                                   style="height: 40px;" />
                                                            <div class="input-group-append">
                                                                <span class="input-group-text" style="height: 40px;"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>

                                                   <div class="col-md-3">
                                                        <label for="txtDate3" class="font-weight-bold" style="font-size: 20px;">End Periode</label>
                                                        <div class="input-group">
                                                            <input type="text"
                                                                   id="txtDate3"
                                                                   class="form-control shadow-sm datepicker1 end"
                                                                   runat="server"
                                                                   placeholder="Select Month & Year"
                                                                   style="height: 40px;" />
                                                            <div class="input-group-append">
                                                                <span class="input-group-text" style="height: 40px;"><i class="fa fa-calendar"></i></span>
                                                            </div>
                                                        </div>
                                                    </div>      
                                                    
                                                    <div class="col-md-2" style="margin-top: 2.3rem;">
                                                           <button type="button"
                                                                class="btn buttonColor align-middle"
                                                                style="height: 40px; line-height: 1.2;"
                                                                onclick="handleClientClick()">
                                                            <i class="fa fa-check" aria-hidden="true"></i>
                                                                Export Report
                                                          </button>

                                                        <!-- Tombol ASP.NET tersembunyi yang akan diklik melalui JS -->
                                                        <asp:Button runat="server"
                                                                    Style="display: none;"
                                                                    ID="btnSubmit"
                                                                    OnClick="btnSubmit_Click" 
                                                                    CommandArgument="PL"/>

                                                    </div>

                                               </div>
                                             </div>
<%--                                            <div class="h5 mb-0 font-weight-bold text-gray-800 mt-4">
                                                        <!-- Tombol utama yang diklik user -->
                                                        <button type="button"
                                                                class="btn buttonColor align-middle"
                                                                style="height: 40px; line-height: 1.2;"
                                                                onclick="handleClientClick()">
                                                            <i class="fa fa-check" aria-hidden="true"></i>
                                                            Export Report
                                                        </button>

                                                        <!-- Tombol ASP.NET tersembunyi yang akan diklik melalui JS -->
                                                        <asp:Button runat="server"
                                                                    Style="display: none;"
                                                                    ID="btnSubmit"
                                                                    OnClick="btnSubmit_Click" 
                                                                    CommandArgument="PL"/>
                                              </div>--%>
                                              
                                       </div>

                                 <iframe id="downloadFrame" name="downloadFrame" style="display: none;"></iframe>
                                 <div id="loadingOverlay"
                                     style="display:none;position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.6);z-index:9999;text-align:center;">
                                     
                   
                                     <div style="position: absolute; top: 40%; left: 50%; transform: translate(-50%, -50%);">
                                        <img src="images/loadingylid.gif" alt="Loading..." style="width: 80px;" />
                                        <p style="font-size: 18px; font-weight: bold; margin-top: 10px;">Processing, please wait...</p>
                                    </div>  
                                </div>
                                                            
                                       </div> <!-- /.col-12 -->
                                 </div> <!-- /.row no-gutters -->              
                            </div> <!-- /.card-body -->
                        </div> <!-- /.card -->

                        <asp:HiddenField ID="hfChartData" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>


        document.addEventListener("DOMContentLoaded", function () {

            const data = JSON.parse(document.getElementById("<%= hfChartData.ClientID %>").value || "{}");
            const rawData = data.rawData;

            const ranges = [
                { label: '0-5 day', min: 0, max: 5 },
                { label: '5-10 day', min: 5, max: 10 },
                { label: '10-15 day', min: 10, max: 15 },
                { label: '20-25 day', min: 20, max: 25 },
                { label: '25-35 day', min: 25, max: 35 },
                { label: '35+ day', min: 35, max: Infinity }
            ];

            const grouped = ranges.map(r => {
                const items = rawData.filter(d => d.leadTime >= r.min && d.leadTime < r.max);
                return { label: r.label, count: items.length, poList: items.map(i => i.po) };
            });

            new Chart(document.getElementById('chartLeadTime'), {
                type: 'bar',
                data: {
                    labels: grouped.map(g => g.label),
                    datasets: [{ label: 'PR to PO', data: grouped.map(g => g.count), backgroundColor: '#06183d' }]
                },
                options: {
                    plugins: {
                        tooltip: {
                            callbacks: {
                                title: ctx => ctx[0].label,
                                label: ctx => grouped[ctx.dataIndex].poList.join('\n') || 'Tidak ada PO'
                            }
                        },
                        legend: { display: true, position: 'top' }
                    },
                    scales: { y: { beginAtZero: true } },
                    maintainAspectRatio: false, // biar tinggi tetap
                    responsive: true,
                }
            });
        });
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const data = JSON.parse(document.getElementById("<%= hfChartData.ClientID %>").value || "{}");

         
            // Chart 1: OTD Hit vs Miss
            new Chart(document.getElementById("chartOTD"), {
                type: "doughnut",
                data: {
                    labels: ["Hit", "Miss"],
                    datasets: [{
                        data: [data.chartData.OTD_Hit || 0, data.chartData.OTD_Miss || 0],
                        backgroundColor: ["#06183d", "#ff0000"]
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            position: "top"
                        },
                        tooltip: {
                            enabled: true
                        },
                        // Plugin untuk menulis teks di tengah
                        centerText: {
                            display: true,
                            text: ["On-Time", "Delivery"] 
                        }
                    }
                },
                plugins: [{
                    id: 'centerText',
                    beforeDraw: function (chart) {
                        const opts = chart.config.options.plugins.centerText;
                        if (opts && opts.display) {
                            const ctx = chart.ctx;
                            const width = chart.width;
                            const height = chart.height;
                            ctx.save();
                            ctx.textAlign = 'center';
                            ctx.textBaseline = 'middle';
                            ctx.fillStyle = '#ff6d10';
                            const centerX = chart.chartArea.left + (chart.chartArea.right - chart.chartArea.left) / 2;
                            const centerY = chart.chartArea.top + (chart.chartArea.bottom - chart.chartArea.top) / 2;
                  

                            const fontSize = (height / 240).toFixed(2);
                            ctx.font = fontSize + 'em sans-serif';

                            const lines = opts.text;
                            const lineHeight = 20;

                            // Hitung posisi tengah dan render tiap baris
                            const totalHeight = lineHeight * (lines.length - 1);
                            const startY = centerY - totalHeight / 2;

                            lines.forEach((line, index) => {
                                ctx.fillText(line, centerX, startY + (index * lineHeight));
                            });

                            ctx.restore();
                        }
                    }
                }]
            });


            // Chart 3: Top Supplier by PO
            //new Chart(document.getElementById("chartSupplier"), {
            //    type: "bar",
            //    data: {
            //        labels: data.chartData.SupplierNames || [],
            //        datasets: [{
            //            label: "Total PO/Supplier",
            //            data: data.chartData.SupplierCount || [],
            //            backgroundColor: "#4e73df"
            //        }]
            //    }
            //});

           
            // Chart 5: Lead Time RF-PO
            //new Chart(document.getElementById("chartLeadTime"), {
            //    type: "bar",
            //    data: {
            //        labels: ["0-5", "5-10", "10-15", "20-25", "25-35", "35+"],
            //        datasets: [{
            //            label: "Jumlah PR-PO",
            //            data: data.LeadTime || [],
            //            backgroundColor: "#FF9F40"
            //        }]
            //    }
            //});

           

        });



        function handleClientClick() {

        debugger

            const startDate = document.getElementById("<%= txtDate2.ClientID %>").value.trim();
            const endDate = document.getElementById("<%= txtDate3.ClientID %>").value.trim();

            if (startDate === "" || endDate === "") {
                swal({
                    icon: 'warning',
                    title: 'Incomplete Input',
                    text: 'Please select both Start Periode and End Periode before exporting.'
                });
                return false; 
            }


            ShowLoading();

                  document.forms[0].target = "downloadFrame";
      
                  const btnSubmit = document.getElementById("<%= btnSubmit.ClientID %>");
                  if (btnSubmit) btnSubmit.click();
        }

        let checkInterval = null;

        function ShowLoading() {
            debugger

            console.log("ShowLoading called");

            const overlay = document.getElementById("loadingOverlay");
            overlay.style.display = "block";

            checkInterval = setInterval(function () {
                const cookies = document.cookie;

                if (cookies.includes("fileDownload=success") || cookies.includes("fileDownload=failed")) {

                    clearInterval(checkInterval);
                    checkInterval = null;

                    hideLoading();

                    // hapus cookie
                    document.cookie = "fileDownload=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";

                    if (cookies.includes("fileDownload=failed")) {
                        
                        const errorCookie = cookies.split('; ').find(row => row.startsWith('errorMessage='));
                        const message = errorCookie ? decodeURIComponent(errorCookie.split('=')[1]) : 'Export failed.';

                        swal({
                            icon: 'error',
                            title: 'Export Failed',
                            text: message
                        });

                    } else {
                        location.reload();
                    }
                }

            }, 500);

        }

        function hideLoading() {

            console.log("hideLoading dipanggil");

            const overlay = document.getElementById("loadingOverlay");

            if (!overlay) {
                //console.warn("❌ #loadingOverlay tidak ditemukan");
                return;
            }

            overlay.style.setProperty("display", "none", "important");
        }


        $(document).ready(function () {
            //var $startPicker = $('.datepicker1.start');
            //var $endPicker = $('.datepicker1.end');

            //function parseToMoment(value) {
            //    if (!value) return null;
            //    return moment("01-" + value, "DD-MM-YYYY"); /
            //}

            $('.datepicker1').bootstrapMaterialDatePicker({
                format: 'MM-YYYY',
                time: false,
                clearButton: true,
                okText: 'Pilih',
                cancelText: 'Batal',
                date: true,
                monthPicker: true,
                year: true
            }).on('beforeShow', function () {

                setTimeout(hideDateElements, 50);

            }).on('change', function () {

                setTimeout(hideDateElements, 50);


            }).on('open', function () {

                setTimeout(hideDateElements, 50);

            });

            function hideDateElements() {
                $('.dtp-actual-num').hide();
                $('.dtp-picker-days').hide();
                $('.dtp-calendar').hide();
            }
        });



    </script>
</asp:Content>
