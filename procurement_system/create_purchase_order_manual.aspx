<%@ Page Title="Create Purchase Order (PO) Manual" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="create_purchase_order_manual.aspx.cs" Inherits="procurement_system.create_purchase_order_manual" Async="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="vendors/toastr/toastr.min.css" rel="stylesheet" />
    <script src="vendors/toastr/jquery.min.js"></script>
    <script src="vendors/toastr/toastr.min.js"></script>
    <script type="text/javascript">
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "2000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    </script>
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

        .container {
            display: block;
            position: relative;
            padding-left: 35px;
            margin-bottom: 20px;
            cursor: pointer;
            font-size: 25px;
        }

            /* Hide the default checkbox */
            .container input {
                visibility: hidden;
                cursor: pointer;
            }

        /* Create a custom checkbox */
        .mark {
            position: absolute;
            top: 0;
            left: 0;
            height: 25px;
            width: 25px;
            background-color: lightgray;
        }

        .container:hover input ~ .mark {
            background-color: gray;
        }

        .container input:checked ~ .mark {
            background-color: blue;
        }

        /* Create the mark/indicator (hidden when not checked) */
        .mark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the mark when checked */
        .container input:checked ~ .mark:after {
            display: block;
        }

        /* Style the mark/indicator */
        .container .mark:after {
            left: 9px;
            top: 5px;
            width: 5px;
            height: 10px;
            border: solid white;
            border-width: 0 3px 3px 0;
            transform: rotate(45deg);
        }

        .action-cell {
            display: flex;
            justify-content: center;   
            align-items: center;       
            height: 100%;
        }

        .inline-form-row .form-control,
        .inline-form-row .form-select {
            height: 32px !important;
            line-height: 1.2;
            font-size: 13px;
        }

        .inline-form-row label {
            font-size: 13px;
            margin-bottom: 2px;
            display: block;
        }


        .bg-detail-dark {
          background-color: #06183d !important;
          color: white;
        }

        .table-custome
         {
            background-color: #06183d !important;
            color: white;
        }
        .table-custome2
         {
            background-color: #5676b8 !important;
            color: white;
         }

       .modal-dialog.modal-mdplus {
          max-width: 1000px; 
        }

        .modal-header .btn-close {
          filter: invert(1); 
          opacity: 0.9;
        }

        .modal-header {
            padding: 0.85rem 1rem;
         }

        /* Background detail form */
        .bg-detail-dark {
            background-color: #0b2144; /* warna biru gelap elegan */
            border-radius: 12px;
            color: #fff;
        }

        /* Semua input dan select */
        .form-control-sm {
            border-radius: 8px !important;
            height: 38px !important;
            line-height: 1.4 !important;
            font-size: 0.9rem !important;
        }

        /* Textarea seragam */
        textarea.form-control-sm {
            border-radius: 8px !important;
            font-size: 0.9rem !important;
        }

        /* Label tebal dan jarak */
        label.fw-bold {
            font-size: 0.9rem;
            color: #eaeaea;
            margin-bottom: 4px;
        }

        /* Tombol detail item */
        .btnSeeDetail {
            border-radius: 8px !important;
            font-size: 0.85rem !important;
            height: 25px !important;
        }

        .btncleare {
            border-radius: 8px !important;
            font-size: 0.85rem !important;
            height: 25px !important;
        }


        /* Spasi antar baris */
        .row.g-2 {
            margin-bottom: 6px;
        }

        /* Input file agar lebih seragam */
        input[type="file"].form-control-sm {
            padding: 5px !important;
            border-radius: 8px !important;
        }

        /* Hover efek halus */
        .form-control-sm:focus,
        .form-select-sm:focus,
        textarea.form-control-sm:focus {
            box-shadow: 0 0 0 0.15rem rgba(13, 110, 253, 0.25);
            border-color: #0879d4;
        }

        /* Divider antar bagian */
        .inline-form-row {
            background-color: transparent;
        }

        /* 🔹 Tambahan jarak antar kolom Delivery To & Delivery Date */
        .col-md-2:first-child {
            margin-right: 20px; /* kasih jarak kanan di kolom pertama */
        }

        .form-select-sm {
            border-radius: 8px !important;
            padding: 6px 10px !important;
        }
               .dataTables_filter input {
            border: 3px solid darkblue !important;
            background-color: #f0f8ff !important;
            padding: 6px 10px !important;
            font-weight: bold !important;
            height:20px;
        }
            
        /* Tambahan efek saat fokus */
        .dataTables_filter input:focus {
            outline: none;
            border-color: midnightblue !important; /* biru terang */
            box-shadow: 0 0 5px rgba(0,123,255,0.5);
        }

        .wrap-remarks {
            white-space: normal !important;
            word-wrap: break-word;
            display: block;
            max-width: 400px;   /* bebas, atur sesuai lebar kolom */
        }

    </style>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>
    <script type="text/javascript">
        function ShowLoading() {
            swal({
                title: 'Loading...',
                text: 'Please wait!',
                imageUrl: "icons/loader.gif",
                showConfirmButton: false,
                allowOutsideClick: false
            });
        }

        function HideLoading() {
            swal.close();
        }
    </script>
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Category Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'purchase_order.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            // Split the inner text into an array of items based on a newline character
            var itemsArray = '<%= lbErrorUploadNotif.InnerText.Replace(Environment.NewLine, "|") %>'.split('|');

            var listItems = ""; // Initialize an empty string for list items

            // Build the list items dynamically
            for (var i = 0; i < itemsArray.length; i++) {
                listItems += "<li>" + itemsArray[i] + "</li>";
            }

            // Construct the HTML content with the list
            var contentHTML = '<span style="color: green; display: none;"><label id="lbErrorUploadNotif" runat="server"></label></span>' +
                '<ul>' + listItems + '</ul>';
            swal({
                title: 'Save Success',
                text: 'PO Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'purchase_order.aspx';
                }
            );
        }
    </script>
        <script type="text/javascript">
            function SelectSucsess(RFNumber) {
                swal({
                    title: 'Success',
                    text: 'PO Successfully created',
                    timer: '2000',
                    type: 'success',
                    showConfirmButton: false,
                    html: true
                },
                    function redirect() {                        
                        window.location.href = 'create_purchase_order_manual.aspx';
                    }
                );

            }

        </script>
    <script type="text/javascript">
        function SelectVendor() {
            swal('Can not add Item!', 'Please select Vendor first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectVendorFirst() {
            swal('Submit Failed!', 'Please select Vendor first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectIssuedDate() {
            swal('Submit Failed!', 'Please select Issued Date first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectDeliveryDate() {
            swal('Submit Failed!', 'Please select Delivery Date first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectDeliveryTo() {
            swal('Submit Failed!', 'Please enter delivery to first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectRequester() {
            swal('Submit Failed!', 'Please select requester first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectAssetType() {
            swal('Submit Failed!', 'Please select asset type first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectPaymentTerm() {
            swal('Submit Failed!', 'Please enter payment term first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectItem() {
            swal('Submit Failed!', 'Please select item first', 'error');
        }
    </script>
    <script type="text/javascript">
        function AlreadyPO() {
            swal('Select Failed!', 'This item already create PO', 'error');
        }
    </script>
    <style>
        .page-head {
            background-color: #06183d; /* Change this to your desired background color */
            color: white; /* Text color for the header */
            padding: 1px 2.938rem; /* Optional padding for the header */
            height: 182px;
            position: relative;
            margin-top: 0px; /* Adjust top margin to move the header down */
            margin-left: 0px; /* Adjust left margin to move the header right */
        }

        .custom-card-body {
            position: relative;
            top: -99px; /* Adjust the top position to move the card body down */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="page-head">
       <div id="page-title">
           <h1 class="page-header text-overflow" style="color: white;">Purchase Order (PO)</h1>
       </div>
       <ol class="breadcrumb">
           <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
           <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
           <li><a href="purchase_order.aspx">&nbsp;Purchase Order (PO)</a></li>
           <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
           <li class="active">&nbsp;&nbsp;Create Purchase Order (Manual)</li>
       </ol>
   </div>
   <div hidden="hidden">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
       <rsweb:ReportViewer ID="PurchaseOrederCreated" runat="server" Width="100%" Height="100%">
       </rsweb:ReportViewer>
       <asp:Image ID="imgQRCode" runat="server" />
   </div>
   <asp:HiddenField ID="lblNamaBranch" runat="server" />
   <asp:HiddenField ID="hblNIK" runat="server" />
   <asp:HiddenField ID="txtRequester" runat="server" />
   <asp:HiddenField ID="txtReqDept" runat="server" />
   <asp:HiddenField ID="txtOIDReqDept" runat="server" />
   <asp:HiddenField ID="txtPosition" runat="server" />
   <asp:HiddenField ID="txtSection" runat="server" />
   <asp:HiddenField ID="hlbGrandTotal" runat="server" />
   <asp:HiddenField ID="hlbRFNumber" runat="server" />
   <asp:HiddenField ID="hlbYears" runat="server" />
   <asp:HiddenField ID="hlbYearsNew" runat="server" />
   <asp:HiddenField ID="hlblast_numberNew" runat="server" />
   <asp:HiddenField ID="hlbCatalog" runat="server" />
   <asp:HiddenField ID="hlbIDVendor" runat="server" />
   <asp:HiddenField ID="hlbOK" runat="server" />
   <asp:HiddenField ID="hfDetailRF" runat="server" />
   <asp:HiddenField ID="hfAttachmentPath" runat="server" />
   <asp:HiddenField ID="hlblocation" runat="server" />
   <asp:HiddenField ID="hlbCodeItem" runat="server" />
   <asp:HiddenField ID="hlbItem" runat="server" />
   <asp:HiddenField ID="hlbUOM" runat="server" />
   <asp:HiddenField ID ="hfFullnameApprover" runat="server" />
   <asp:HiddenField ID ="hfApprover" runat="server" />
   <asp:HiddenField ID ="txtPONumber" runat="server" />
   <asp:HiddenField ID ="txtdivid" runat="server" />
    
    


     <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' id="divCreatePO" runat="server" visible="true">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;Create Purchase Order (Manual)</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Issued Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtIssuedDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker1" placeholder="Issued Date" disabled>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                         
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Requester*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
<%--                                                        <asp:DropDownList ID="ddlRequester"
                                                            class="selectpicker form-control" 
                                                            data-show-subtext="true" 
                                                            data-live-search="true" 
                                                            AppendDataBoundItems="true" 
                                                            AutoPostBack="true"
                                                            runat="server" 
                                                            OnSelectedIndexChanged="ddlRequester_SelectedIndexChanged">
                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        </asp:DropDownList>--%>

                                                        <asp:DropDownList
                                                            ID="ddlRequester"
                                                            runat="server"
                                                            CssClass="selectpicker form-control"
                                                            data-show-subtext="true"
                                                            data-live-search="true"
                                                            AutoPostBack="true"
                                                            EnableViewState="true"
                                                            OnSelectedIndexChanged="ddlRequester_SelectedIndexChanged">
                                                        </asp:DropDownList>



                                                    </div>
                                                </div>
                                            </div>


                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery Date*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtDeliveryDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker1" placeholder="Delivery Date">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Asset Type*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlAssetStatus" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlAssetStatus_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Asset" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Fixed Asset" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Non Fixed Asset" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                           <div class='col-sm-6'>
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <div class="input-group-prepend">
                                                                <span class="input-group-text">Vendor Name*&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </div>

                                                            <asp:DropDownList 
                                                                ID="ddlVendor"
                                                                runat="server"
                                                                class="custom-select mr-sm-2"
                                                                CssClass ="form-control selectpicker"
                                                                data-live-search="true"
                                                                AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                                <asp:ListItem Text="-- Select Vendor --" Value=""></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </div>
                                                    </div>
                                                </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Payment Terms*</span>
                                                        </div>
                                                        <input runat="server" id="txtPaymentTerms" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Payment of terms...">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Vendor Address</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="txtAddress" runat="server" disabled></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery to*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlDeliveryTo" class="custom-select mr-sm-2" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlDeliveryTo_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Text="-- Select Delivery --" Value=""></asp:ListItem>
                                                            <asp:ListItem Text="Jakarta Utara - Temas Building" Value="PT Yusen Logistics Indonesia, Temas Building Lantai 3A, Jl. Yos Sudarso Kav.33, Sunter Jaya, Jakarta Utara 14350, Indonesia"></asp:ListItem>
                                                            <asp:ListItem Text="Banten(Cengkareng) - Soewarna Business Park" Value="PT Yusen Logistics Indonesia, Soewarna Business Park Blok A, Lot 1-2 Soekarno-Hatta Airport, Pajang, Benda, Tengerang, Banten 15126, Indonesia"></asp:ListItem>
                                                            <asp:ListItem Text="Cikarang - Kawasan industri" Value="PT Yusen Logistics Indonesia, Kawasan Industri MM2100 Blok EE-4, Desa Danau Indah, Cikarang Barat, Bekasi 17520, Indonesia"></asp:ListItem>
                                                            <asp:ListItem Text="Sidoarjo - Ruko Permata juanda" Value="PT Yusen Logistics Indonesia, Ruko Permata Juanda, West Wing Super B/8-8A, Jl. Raya Juanda, Sedatiagung - Sedati, Sidoarjo 61253,  Indonesia"></asp:ListItem>
                                                            <asp:ListItem Text="Semarang - HSBC Building" Value="PT Yusen Logistics Indonesia, HSBC Building 4th Floor Suite 408, Jl. Gajah Mada No. 135, Pekunden, Semarang 50134,  Indonesia"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Other Condition</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="txtOtherCondition" runat="server" maxlength="100" placeholder="Max. 100 character"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery To Edited</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="Textareadelivery" runat="server" maxlength="100000" ></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Request Type*&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="DropDownList1" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlRequesttype_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Req Type" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Purchase" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Maintenance" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Catalog*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlCatalog" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                           AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCatalog_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Catalog" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="GA" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="IT" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divItemPO" runat="server" visible="true">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-file-text-o"></i><span class="nav-text">&nbsp;List of Item PO</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                             <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlItem" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="true"
                                                            runat="server" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                    </div>
                                                </div>
                                            </div>

                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Quantity&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtJumlahBeli" data-validate-length-range="5,15" type="number" class="form-control" placeholder="Quantity" onkeypress="return isNumberKey(event)">
                                                    </div>
                                                </div>
                                            </div>

                                                <div class='col-sm-6'>
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <div class="input-group-append">
                                                                <span class="input-group-text">Remarks&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </div>
                                                            <textarea class="form-control h-150px" rows="2" id="txtRemaks" runat="server"></textarea>
                                                        </div>
                                                    </div>
                                                </div>
                                             <div class='col-sm-6'>
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <button type="button" style="float: right;" onclick="validateAndSubmit()"  class="btn mb-1 buttonColor">
                                                                Add Item
                                                                <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                             </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                       ShowHeader="True" ShowHeaderWhenEmpty="true"  EmptyDataText="No Record Found" OnRowCommand="TableItemPurchase_RowCommand" OnRowDataBound="TableItemPurchase_RowDataBound" OnSelectedIndexChanged="TableItemPurchase_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="kode_barang" HeaderText="Code Item" />
                                                            <asp:BoundField DataField="nama_barang" HeaderText="Item" />                                                          
                                                            <asp:BoundField DataField="jumlah_beli" HeaderText="Quantity" />
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemaks" runat="server" Text='<%# Eval("remaks") %>' CssClass="wrap-remarks"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" />
                                                            </asp:TemplateField>
<%--                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks"/>--%>
                                                            <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="@ Price Input">
                                                                <ItemTemplate>
                                                                    <input type="text" 
                                                                           name="txtprice" 
                                                                           id="txtprice"
                                                                           class="input-price form-control"
                                                                           runat="server"
                                                                           value=""
                                                                           data-quantity='<%# Eval("jumlah_beli") %>' 
                                                                           placeholder="enter price per @" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnRemove" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnRemove_Click" ToolTip="Remove"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                             <div class='col-sm-8'></div>
                                             <div class='col-sm-4' id="divgrantot" runat="server" visible="false">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">VAT(%)&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtvat" data-validate-length-range="5,15" type="text" class="form-control" placeholder="0" value="0">
                                                    </div>
                                                    <div class="input-group mt-2">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="0" value="0" readonly>
                                                        <button type="button" class="btn btn-sm btn-danger ml-2" onclick="clearEstimasi();"> Clear <i class="fa-solid  fa-refresh"></i></button>
                                                    </div>
                                                </div>
                                            </div>
<%--                                                 <div class='col-sm-4'>
                                                     <div class="form-group">
                                                         
                                                     </div>
                                                 </div>--%>
                                            <div class='col-sm-8' id="divUploadFile" runat="server" visible="false">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Doc. Upload&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <asp:FileUpload ID="FileUploadEDocs" AllowMultiple="true" runat="server" /></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hlbKodeBarang" runat="server" />
                                            <asp:HiddenField ID="hlbID" runat="server" />
                                            <div class='col-sm-4' id="divSubmit" runat="server" visible="false">
       <%--                                         <button type="button" style="float: right;" onclick="<%=btnSubmitPO.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Submit
							                    <span class="btn-icon-right"><i class="fa fa-save"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSubmitPO" OnClick="btnSubmitPO_Click" OnClientClick="ShowLoading()"></asp:Button>--%>


                                                <button type="button" style="float: right;" onclick="validateBeforeSubmit()" class="btn mb-1 buttonColor">
                                                        Submit
                                                <span class="btn-icon-right"><i class="fa fa-save"></i></span>
                                                </button>
                                                  <asp:Button runat="server" Style="display: none;" 
                                                                ID="btnSubmitPO" 
                                                                OnClick="btnSubmitPO_Click" 
                                                                OnClientClick="ShowLoading()">
                                                 </asp:Button>
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


 <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
<script>

    $(document).ready(function () {
        $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
    });

    $(function () {
        $("#<%= ddlDeliveryTo.ClientID %>").change(function () {
            $("#<%= Textareadelivery.ClientID %>").val($(this).val());
        });
    });


    $(document).ready(function () {
        debugger
        console.log("JS loaded...");

        // Event untuk input harga
        $(document).on("keyup blur", ".input-price", function () {
            formatCurrency($(this));
            calculateGrandTotal();
        });

        // Pastikan event txtvat terpasang
        var vatInput = $("#<%= txtvat.ClientID %>");  

        vatInput.on("keyup blur", function () {
           /* console.log("VAT changed:", $(this).val());*/
            calculateGrandTotal();
        });

        function calculateGrandTotal() {
          /*  console.log("calculateGrandTotal() running...");*/

            var grandTotal = 0;

            // Hitung grand total tanpa VAT
            $(".input-price").each(function () {
                var priceText = $(this).val().replace(/,/g, "");
                var price = parseFloat(priceText) || 0;
                var qty = parseFloat($(this).data("quantity")) || 0;

                grandTotal += (price * qty);
            });

            /*console.log("Grand Total Asli =", grandTotal);*/

            var vat = parseFloat(vatInput.val()) || 0;
           /* console.log("VAT =", vat);*/

            var grandTotalWithVat = grandTotal + ((vat / 100) * grandTotal);

           /* console.log("Grand Total + VAT =", grandTotalWithVat);*/

              $("#<%= txtGrandTotal.ClientID %>").val(
                  grandTotalWithVat.toLocaleString('en-US')
              );
          }

  });


     function clearEstimasi() {
         $(".input-price").each(function () {
             $(this).val("");
         });

         $("#<%= txtGrandTotal.ClientID %>").val("0");
         $("#<%= txtvat.ClientID %>").val("0");

        }


        function formatNumber(n) {
            return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        }


        function formatCurrency(input, blur) {

            var input_val = input.val();

            if (input_val === "") { return; }

            var original_len = input_val.length;

            var caret_pos = input.prop("selectionStart");

            if (input_val.indexOf(".") >= 0) {


                var decimal_pos = input_val.indexOf(".");

                // split number by decimal point
                var left_side = input_val.substring(0, decimal_pos);
                var right_side = input_val.substring(decimal_pos);

                // add commas to left side of number
                left_side = formatNumber(left_side);

                // validate right side
                right_side = formatNumber(right_side);

                // On blur make sure 2 numbers after decimal
                if (blur === "blur") {
                    right_side += "00";
                }

                // Limit decimal to only 2 digits
                right_side = right_side.substring(0, 2);

                // join number by .
                input_val = "" + left_side + "." + right_side;

            } else {

                input_val = formatNumber(input_val);
                input_val = "" + input_val;

                // final formatting
                if (blur === "blur") {
                    input_val += "";
                }
            }

            // send updated string to input
            input.val(input_val);

            // put caret back in the right position
            var updated_len = input_val.length;
            caret_pos = updated_len - original_len + caret_pos;
            input[0].setSelectionRange(caret_pos, caret_pos);
    }


    function validateAndSubmit() {
        debugger
        var isValid = true;

        // === 1. VALIDASI DDL ITEM (BOOTSTRAP SELECT) ===
        var ddl = document.getElementById("<%= ddlItem.ClientID %>");
        var ddlValue = ddl.value;
        var ddlWrapper = $(ddl).closest(".bootstrap-select");

        if (ddlValue === "" || ddlValue === "<Code-Category-Item-Merk-Type>" || ddlValue === "00000000-0000-0000-0000-000000000000") {
            ddlWrapper.css("border", "2px solid red").css("border-radius", "5px");
            isValid = false;
        } else {
            ddlWrapper.css("border", "").css("border-radius", "");
        }

        // === 2. VALIDASI QUANTITY ===
        var qty = document.getElementById("<%= txtJumlahBeli.ClientID %>");
            if (qty.value === "" || parseFloat(qty.value) <= 0) {
                qty.style.border = "2px solid red";
                isValid = false;
            } else {
                qty.style.border = "";
            }

    // === 3. VALIDASI REMARKS ===
    var remarks = document.getElementById("<%= txtRemaks.ClientID %>");
            if (remarks.value.trim() === "") {
                remarks.style.border = "2px solid red";
                isValid = false;
            } else {
                remarks.style.border = "";
            }

    // Jika tidak valid → stop
    if (!isValid) return;

    // Jika valid → klik server button
        document.getElementById("<%= btnAddItem.ClientID %>").click();


    }

    function validateBeforeSubmit() {
        debugger

        let isValid = true;

        // ===== Helper function: border merah =====
        function setError(input) {
            input.style.border = "2px solid red";
            isValid = false;
        }

        function clearError(input) {
            input.style.border = "";
        }

        // ===== Helper untuk Bootstrap Select =====
        function setErrorSelectPicker(selectElement) {
            $(selectElement).closest(".bootstrap-select")
                .css("border", "2px solid red")
                .css("border-radius", "5px");
            isValid = false;
        }

        function clearErrorSelectPicker(selectElement) {
            $(selectElement).closest(".bootstrap-select")
                .css("border", "")
                .css("border-radius", "");
        }

        // ==== VALIDASI FIELD WAJIB ====

        // 1. Requester (selectpicker)
        let requester = document.getElementById("<%= ddlRequester.ClientID %>");
        if (requester.value === "" || requester.value === "0" || requester.value === "00000000-0000-0000-0000-000000000000") setErrorSelectPicker(requester);
        else clearErrorSelectPicker(requester);

        // 2. Delivery Date
        let deliveryDate = document.getElementById("<%= txtDeliveryDate.ClientID %>");
        if (deliveryDate.value.trim() === "") setError(deliveryDate);
        else clearError(deliveryDate);

        // 3. Asset Type
        let assetType = document.getElementById("<%= ddlAssetStatus.ClientID %>");
        if (assetType.value === "0") setError(assetType);
        else clearError(assetType);

        // 4. Vendor (selectpicker)
        let vendor = document.getElementById("<%= ddlVendor.ClientID %>");
        if (vendor.value === "" || vendor.value === "0" || vendor.value === "00000000-0000-0000-0000-000000000000") setErrorSelectPicker(vendor);
        else clearErrorSelectPicker(vendor);

        // 5. Payment Terms
        let paymentTerms = document.getElementById("<%= txtPaymentTerms.ClientID %>");
        if (paymentTerms.value.trim() === "") setError(paymentTerms);
        else clearError(paymentTerms);

        // 6. Delivery To
        let deliveryTo = document.getElementById("<%= ddlDeliveryTo.ClientID %>");
        if (deliveryTo.value === "") setError(deliveryTo);
        else clearError(deliveryTo);

        // 7. Request Type
        let reqType = document.getElementById("<%= DropDownList1.ClientID %>");
        if (reqType.value === "0") setError(reqType);
        else clearError(reqType);

        // 8. Catalog
        let catalog = document.getElementById("<%= ddlCatalog.ClientID %>");
        if (catalog.value === "0") setError(catalog);
        else clearError(catalog);

        // === Jika ada error → jangan submit ===
        if (!isValid) return;

        // === Semua valid → Klik tombol submit server ===
            document.getElementById("<%= btnSubmitPO.ClientID %>").click();
    }




</script>

</asp:Content>
