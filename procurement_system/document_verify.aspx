<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="document_verify.aspx.cs" Inherits="procurement_system.document_verify" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>





<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Purchasing System - YLID | Document Verify</title>
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
<body style="background:#f4f6f9;">
    <form id="form1" runat="server">

      <div style="display:flex;justify-content:center;align-items:center;height:100vh;">

    <div style="width:520px;background:white;border-radius:10px;
                box-shadow:0 10px 30px rgba(0,0,0,0.1);padding:40px;text-align:center;">

        <h2 style="color:#06183d;margin-bottom:5px;">
            Purchasing System - YLID
        </h2>

        <p style="color:#777;margin-bottom:25px;">
            Document Verification
        </p>

        <!-- ICON -->
        <div id="iconStatus" runat="server" 
             style="font-size:60px;color:#28a745;margin-bottom:10px;">
            ✓
        </div>

        <!-- TITLE -->
        <h3 id="titleStatus" runat="server" 
            style="color:#28a745;font-weight:bold;margin-bottom:25px;">
            Document Verified
        </h3>

        <hr />

        <table style="width:100%;margin-top:20px;text-align:left;font-size:15px;">
            <tr>
                <td><b>PO Number</b></td>
                <td>:</td>
                <td><asp:Label ID="lbPO" runat="server"></asp:Label></td>
            </tr>

            <tr>
                <td><b>RF Number</b></td>
                <td>:</td>
                <td><asp:Label ID="lbRF" runat="server"></asp:Label></td>
            </tr>

            <tr>
                <td><b>Approved</b></td>
                <td>:</td>
                <td><asp:Label ID="lblstatus" runat="server"></asp:Label></td>
            </tr>

            <tr>
                <td><b>Name</b></td>
                <td>:</td>
                <td><asp:Label ID="lbName" runat="server"></asp:Label></td>
            </tr>

            <tr>
                <td><b>Date</b></td>
                <td>:</td>
                <td><asp:Label ID="lbDate" runat="server"></asp:Label></td>
            </tr>
        </table>

        <!-- STATUS -->
        <div id="statusDiv" runat="server"
             style="margin-top:30px;background:#e9f7ef;color:#28a745;
                    padding:12px;border-radius:6px;font-weight:bold;">
            STATUS : VALID
        </div>

    </div>

</div>

    </form>
</body>
</html>
