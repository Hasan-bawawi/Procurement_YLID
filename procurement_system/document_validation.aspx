<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="document_validation.aspx.cs" Inherits="procurement_system.document_validation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Purchasing System - YLID | Document Validation</title>
    <!-- Favicon icon -->
    <link rel="icon" href="images/favicon.ico" type="image/x-icon">
    <!-- Custom Stylesheet -->
    <link href="css/style.css" rel="stylesheet">
    <link href="./plugins/bootstrap-material-datetimepicker/css/bootstrap-material-datetimepicker.css" rel="stylesheet">
    <!-- Pignose Calender -->
    <link href="./plugins/pg-calendar/css/pignose.calendar.min.css" rel="stylesheet">
    <!-- Chartist -->
    <link rel="stylesheet" href="./plugins/chartist/css/chartist.min.css">
    <link rel="stylesheet" href="./plugins/chartist-plugin-tooltips/css/chartist-plugin-tooltip.css">
    <link href="./plugins/tables/css/datatable/dataTables.bootstrap4.min.css" rel="stylesheet">
    <!-- Page plugins css -->
    <link href="./plugins/clockpicker/dist/jquery-clockpicker.min.css" rel="stylesheet">
    <!-- Color picker plugins css -->
    <link href="./plugins/jquery-asColorPicker-master/css/asColorPicker.css" rel="stylesheet">
    <!-- Date picker plugins css -->
    <link href="./plugins/bootstrap-datepicker/bootstrap-datepicker.min.css" rel="stylesheet">
    <!-- Daterange picker plugins css -->
    <link href="./plugins/timepicker/bootstrap-timepicker.min.css" rel="stylesheet">
    <link href="./plugins/bootstrap-daterangepicker/daterangepicker.css" rel="stylesheet">
    <!-- Dropzone Css -->
    <script src="plugins/chart.js/Chart.bundle.min.js"></script>

    <!-- Date range Plugin JavaScript -->
    <script src="./plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <script src="./plugins/bootstrap-daterangepicker/daterangepicker.js"></script>
    <script src="./plugins/moment/moment.js"></script>
    <script src="./plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>

    <link href="plugins/dropzone/dropzone.css" rel="stylesheet">

    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script src="vendors/sweetalert.js"></script>
    <script src="vendors/sweetalert.min.js"></script>
    <link href="plugins/fullcalendar/css/fullcalendar.min.css" rel="stylesheet" />

    <style>
        fieldset.scheduler-border {
            border: 1px groove #ff6d10 !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            width: inherit; /* Or auto */
            padding: 0 10px; /* To give a bit of padding on the left and right */
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

        .buttonColorGridview {
            background-color: #ff6d10;
            color: white;
        }

            .buttonColorGridview:hover {
                background-color: #06183d;
                color: white;
            }
    </style>


    <style>
        .page-head {
            background-color: #06183d; /* Change this to your desired background color */
            color: white; /* Text color for the header */
            padding: 75px 2.938rem; /* Optional padding for the header */
            height: 254px;
            position: relative;
            margin-top: 71px; /* Adjust top margin to move the header down */
            margin-left: 0px; /* Adjust left margin to move the header right */
        }

        .custom-card-body {
            position: relative;
            top: -99px; /* Adjust the top position to move the card body down */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div hidden="hidden">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
            </rsweb:ReportViewer>
            <asp:Image ID="imgQRCode" runat="server" />
        </div>
        <!--*******************
	        Preloader start
        ********************-->

        <div id="preloader">
            <div class="loader">
                <svg class="circular" viewBox="25 25 50 50">
                    <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="3" stroke-miterlimit="10" />
                </svg>
            </div>
        </div>

        <div id="main-wrapper">
            <div class="nav-header">
                <div class="brand-logo text-center">
                    <a href="#">
                        <b class="logo-abbr text-center">
                            <img src="images/logo.png" alt="" />
                        </b>
                        <span class="logo-compact">
                            <img src="images/logo-compact.png" alt="" /></span>
                        <span class="brand-title">
                            <img src="images/logo-text.png" alt="" />
                        </span>
                    </a>
                </div>
            </div>
            <div class="header">
                <div class="header-content clearfix">
                    <div class="header-left">
                        <div class="input-group icons">
                            <div class="input-group-prepend">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="page-head">
                <div id="page-title">
                    <div class="pad-all text-center">
                        <h2 class="page-header text-center" style="color: white;">Document Validation</h2>
                        <h5 class="page-header text-center" style="color: white;">Purchasing System - YLID</h5>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-3 col-sm-3"></div>
                    <div class="col-md-6 col-sm-6">
                        <div class="card custom-card-body">
                            <div class="card-body">
                                <div class="row">
                                    <div class='col-sm-12'>
                                        <fieldset class="scheduler-border">
                                            <legend class="scheduler-border">Document Detail</legend>
                                            <div class="control-group">
                                                <div class="row">
                                                    <div class='col-sm-4'>
                                                        <strong>Doc Number</strong>
                                                        <div class="form-group">
                                                            <div class='input-group'>
                                                                <input runat="server" id="txtDocNo" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Doc Number" required="required" disabled>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class='col-sm-4'>
                                                        <strong>Create by</strong>
                                                        <div class="form-group">
                                                            <div class='input-group'>
                                                                <input runat="server" id="txtCreateby" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Create by" required="required" disabled>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class='col-sm-4'>
                                                        <strong>Document Date</strong>
                                                        <div class="form-group">
                                                            <div class='input-group'>
                                                                <input runat="server" id="txtDocDate" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Document Date" required="required" disabled>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class='col-sm-4'>
                                                        &nbsp;
				                                        <div class="form-group">
                                                            <div class='input-group'>
                                                                <asp:Button runat="server" ID="btnCekDoc" CssClass="btn buttonColor" Text="Doc Check" OnClick="btnCekDoc_Click"></asp:Button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--**********************************
	Scripts
***********************************-->
        <script src="plugins/common/common.min.js"></script>
        <script src="js/custom.min.js"></script>
        <script src="js/settings.js"></script>
        <script src="js/gleek.js"></script>
        <script src="js/styleSwitcher.js"></script>

        <!-- PNotify -->
        <link href="vendors/pnotify/dist/pnotify.css" rel="stylesheet" />
        <link href="vendors/pnotify/dist/pnotify.buttons.css" rel="stylesheet" />
        <link href="vendors/pnotify/dist/pnotify.nonblock.css" rel="stylesheet" />
        <script src="vendors/pnotify/dist/pnotify.js"></script>
        <script src="vendors/pnotify/dist/pnotify.buttons.js"></script>
        <script src="vendors/pnotify/dist/pnotify.nonblock.js"></script>

        <!-- Chartjs -->
        <script src="./plugins/chart.js/Chart.bundle.min.js"></script>
        <!-- Circle progress -->
        <script src="./plugins/circle-progress/circle-progress.min.js"></script>
        <!-- Datamap -->
        <script src="./plugins/d3v3/index.js"></script>
        <script src="./plugins/topojson/topojson.min.js"></script>
        <script src="./plugins/datamaps/datamaps.world.min.js"></script>
        <!-- Morrisjs -->
        <script src="./plugins/raphael/raphael.min.js"></script>
        <script src="./plugins/morris/morris.min.js"></script>
        <!-- Pignose Calender -->
        <script src="./plugins/moment/moment.min.js"></script>
        <script src="./plugins/pg-calendar/js/pignose.calendar.min.js"></script>
        <!-- ChartistJS -->
        <script src="./plugins/chartist/js/chartist.min.js"></script>
        <script src="./plugins/chartist-plugin-tooltips/js/chartist-plugin-tooltip.min.js"></script>

        <script src="./plugins/tables/js/jquery.dataTables.min.js"></script>
        <script src="./plugins/tables/js/datatable/dataTables.bootstrap4.min.js"></script>
        <script src="./plugins/tables/js/datatable-init/datatable-basic.min.js"></script>

        <!-- Toastr -->
        <%--<script src="./plugins/toastr/js/toastr.min.js"></script>
    <script src="./plugins/toastr/js/toastr.init.js"></script>
        --%>
        <%--<script src="./js/dashboard/dashboard-1.js"></script>--%>

        <script src="./plugins/moment/moment.js"></script>
        <script src="./plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>
        <!-- Clock Plugin JavaScript -->
        <script src="./plugins/clockpicker/dist/jquery-clockpicker.min.js"></script>
        <!-- Color Picker Plugin JavaScript -->
        <script src="./plugins/jquery-asColorPicker-master/libs/jquery-asColor.js"></script>
        <script src="./plugins/jquery-asColorPicker-master/libs/jquery-asGradient.js"></script>
        <script src="./plugins/jquery-asColorPicker-master/dist/jquery-asColorPicker.min.js"></script>
        <!-- Date Picker Plugin JavaScript -->
        <script src="./plugins/bootstrap-datepicker/bootstrap-datepicker.min.js"></script>
        <!-- Date range Plugin JavaScript -->
        <script src="./plugins/timepicker/bootstrap-timepicker.min.js"></script>
        <script src="./plugins/bootstrap-daterangepicker/daterangepicker.js"></script>
        <script src="./plugins/moment/moment.js"></script>
        <script src="./plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>

        <script src="./js/plugins-init/form-pickers-init.js"></script>
        <!-- Dropzone Plugin Js -->
        <script src="../../plugins/dropzone/dropzone.js"></script>

        <!-- Datatables -->

        <link href="plugins/tables/css/datatable/dataTables.bootstrap4.min.css" rel="stylesheet" />
        <script src="plugins/tables/js/jquery.dataTables.min.js"></script>
        <script src="plugins/tables/js/datatable/dataTables.bootstrap4.min.js"></script>
        <script src="plugins/tables/js/datatable-init/datatable-basic.min.js"></script>

        <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
        <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>
        <script src="plugins/fullcalendar/js/fullcalendar.min.js"></script>
        <script src="js/plugins-init/fullcalendar-init.js"></script>


        <script>
            (function ($) {
                "use strict"

                new quixSettings({
                    version: "light", //2 options "light" and "dark"
                    layout: "vertical", //2 options, "vertical" and "horizontal"
                    navheaderBg: "color_1", //have 10 options, "color_1" to "color_10"
                    headerBg: "color_1", //have 10 options, "color_1" to "color_10"
                    sidebarStyle: "vertical", //defines how sidebar should look like, options are: "full", "compact", "mini" and "overlay". If layout is "horizontal", sidebarStyle won't take "overlay" argument anymore, this will turn into "full" automatically!
                    sidebarBg: "color_1", //have 10 options, "color_1" to "color_10"
                    sidebarPosition: "fixed", //have two options, "static" and "fixed"
                    headerPosition: "fixed", //have two options, "static" and "fixed"
                    containerLayout: "wide",  //"boxed" and  "wide". If layout "vertical" and containerLayout "boxed", sidebarStyle will automatically turn into "overlay".
                    direction: "ltr" //"ltr" = Left to Right; "rtl" = Right to Left
                });


            })(jQuery);
        </script>

        <script>
            $(document).ready(function () {
                $('#date').bootstrapMaterialDatePicker
                    ({
                        time: false,
                        clearButton: true
                    });
                $('#time').bootstrapMaterialDatePicker
                    ({
                        date: false,
                        shortTime: false,
                        format: 'HH:mm'
                    });
                $('#date-format').bootstrapMaterialDatePicker
                    ({
                        format: 'dddd DD MMMM YYYY - HH:mm'
                    });
                $('#date-fr').bootstrapMaterialDatePicker
                    ({
                        format: 'DD/MM/YYYY HH:mm',
                        lang: 'fr',
                        weekStart: 1,
                        cancelText: 'ANNULER',
                        nowButton: true,
                        switchOnClick: true
                    });

                $('#date-end').bootstrapMaterialDatePicker
                    ({
                        weekStart: 0, format: 'DD/MM/YYYY HH:mm'
                    });
                $('#date-start').bootstrapMaterialDatePicker
                    ({
                        weekStart: 0, format: 'DD/MM/YYYY HH:mm', shortTime: true
                    }).on('change', function (e, date) {
                        $('#date-end').bootstrapMaterialDatePicker('setMinDate', date);
                    });
                $.material.init()
            });
        </script>

        <script src="vendors/sweetalert.js"></script>
        <script src="vendors/sweetalert.min.js"></script>
        <link href="OtherThemes/fontawesome-free-6.4.0-web/css/fontawesome.css" rel="stylesheet" />
        <link href="OtherThemes/fontawesome-free-6.4.0-web/css/brands.css" rel="stylesheet" />
        <link href="OtherThemes/fontawesome-free-6.4.0-web/css/solid.css" rel="stylesheet" />
    </form>
</body>
</html>
