<%@ Page Title="Create Purchase Order (PO)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="create_purchase_order_standart.aspx.cs" Inherits="procurement_system.create_purchase_order_standart" Async="true" %>

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
                        window.location.href = 'create_purchase_order_standart.aspx?rf_no=' + RFNumber;
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
            <li class="active">&nbsp;&nbsp;Create Purchase Order (PO)</li>
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


    <asp:HiddenField ID="hfFormData" runat="server" />
    <asp:Button ID="btnSubItemsatuan" runat="server" Text="HiddenSubmit"  OnClick="btnSubItemsatuan_Click" Style="display:none;" />
    

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' id="divListItemRF" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;Draft of Item RF -
                                        <asp:Label ID="lblRFNumber" runat="server"></asp:Label>
                                    </span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                        <div class='col-sm-12'>
                                         <button type="button" style="float: left;" class="btn mb-1 mt-2 buttonColor" onclick="handleSubmitAll()">
                                                   SUBMIT All PO
							                    <span class="btn-icon-right"><i class="fa fa-send-o"></i></span>
                                                </button>
                                         <asp:Button runat="server" Style="display: none;" ID="btnSubmitAll" OnClick="btnSubmitAll_Click"></asp:Button>                            
                                         </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableDetailsRF" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableDetailsRF_RowCommand" OnRowDataBound="TableDetailsRF_RowDataBound" OnSelectedIndexChanged="TableDetailsRF_SelectedIndexChanged" DataKeyNames="IDDivisionRequester">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" HorizontalAlign="Center" />
                                                        <Columns>
                                                        <%--<asp:TemplateField HeaderText="Create PO">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnCreate" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnCreate_Click" ToolTip="Create PO"><i class="fa-solid fa-edit"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                           <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:BoundField DataField="item_code" HeaderText="Item Code" />--%>
                                                            <%--<asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remark" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="UOM" />--%>
                                                            <%--<asp:BoundField DataField="price" HeaderText="Price" />--%>
                                                            <%-- <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" HtmlEncode="False" />--%>
                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor" />
                                                            <asp:BoundField DataField="DrafPO" HeaderText="DrafPO" />
                                                            <%--<asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />--%>
                                                            <%-- <asp:BoundField DataField="no_po" HeaderText="PO Number" />--%>
                                                            <%--<asp:BoundField DataField="amount" HeaderText="amount" />--%>
                                                            <%--<asp:BoundField DataField="rf_no" HeaderText="rf_no" />--%>
                                                            <asp:BoundField DataField="catalog_type" HeaderText="catalog_type" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="DivisionReq" HeaderText="DivisionRequester" />
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <div Class="action-cell">
                                                                       <button type="button" class="btn btn-sm btn-default" data-idvendor="<%# Eval("id_vendor") %>" data-vendor="<%# Eval("vendor_name") %>"onclick="toggleInlineForm(this)"><i class="fa fa-angle-down"></i>
                                                                    </button>
                                                                   <%-- <button type="button" class="btn btn-sm btn-default" onclick="toggleInlineForm(this)"><i class="fa fa fa-angle-down"></i> --%>
                                                                    </button>
                                                                     </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField DataField="DivisionRequester" HeaderText="OIDDivisionRequester" />--%>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divCreatePO" runat="server" visible="false">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;Create Purchase Order (Standart)</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6' hidden>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">PO Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtPONumber" data-validate-length-range="5,15" type="text" class="form-control" placeholder="PO Number" disabled>
                                                    </div>
                                                </div>
                                            </div>
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
                                                            <span class="input-group-text">Request by&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtReqBy" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Select Date" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
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
                                                            <span class="input-group-text">Asset Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
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
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Vendor Name&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtVendorName" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Vendor Name" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Payment Terms</span>
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
                                                            <span class="input-group-text">Delivery to&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <%--<textarea class="form-control h-150px" rows="3" id="txtDeliveryTo" runat="server"></textarea>--%>
                                                        <asp:DropDownList ID="ddlDeliveryTo" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlDeliveryTo_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Temas Building Lantai 3A, Jl. Yos Sudarso Kav.33, Sunter Jaya, Jakarta Utara 14350, Indonesia" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Soewarna Business Park Blok A, Lot 1-2 Soekarno-Hatta Airport, Pajang, Benda, Tengerang, Banten 15126, Indonesia" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Kawasan Industri MM2100 Blok EE-4, Desa Danau Indah, Cikarang Barat, Bekasi 17520, Indonesia" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Ruko Permata Juanda, West Wing Super B/8-8A, Jl. Raya Juanda, Sedatiagung - Sedati, Sidoarjo 61253,  Indonesia" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, HSBC Building 4th Floor Suite 408, Jl. Gajah Mada No. 135, Pekunden, Semarang 50134,  Indonesia" Value="5"></asp:ListItem>
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

                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divItemPO" runat="server" visible="false">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-file-text-o"></i><span class="nav-text">&nbsp;List of Item PO</span></legend>
                                    <div class="control-group">
                                        <div class="row">
<%--                                            <div class='col-sm-12'>
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
							                    <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>--%>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPO" runat="server" CssClass="table table-bordered table-striped text-nowrap verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPO_RowCommand" OnRowDataBound="TableItemPO_RowDataBound" OnSelectedIndexChanged="TableItemPO_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RF No." HeaderText="RF No." />
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code" />
                                                            <asp:BoundField DataField="Item" HeaderText="Item" />
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                            <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("Price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("Amount"))) %>' name="txtAmount" id="txtAmount" data-type="currency" disabled />
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
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Total Amount (IDR)</span>
                                                        </div>
                                                        <input runat="server" id="txtTotalAmount" data-validate-length-range="5,15" type="text" class="form-control" placeholder="total amount..." disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-8'></div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">VAT</span>
                                                        </div>
                                                        <asp:TextBox runat="server" ID="txtVAT" data-validate-length-range="5,15" type="text" class="form-control" placeholder="vat..." onkeydown="if (event.keyCode == 13) { __doPostBack('<%=txtVAT.UniqueID %>', ''); return false; }" OnTextChanged="txtVAT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">%</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <input runat="server" id="txtVatAmount" data-validate-length-range="5,15" type="text" class="form-control" placeholder="vat amount..." disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-8'></div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="grand total..." disabled>
                                                    </div>
                                                </div>
                                            </div>
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
                                                <button type="button" style="float: right;" onclick="<%=btnSubmitPO.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Submit
							                    <span class="btn-icon-right"><i class="fa fa-check-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSubmitPO" OnClick="btnSubmitPO_Click" OnClientClick="ShowLoading()"></asp:Button>
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

 <!--**********************************
Modal Detail Item
***********************************-->

    <!-- Modal Detail Draft Item -->
<div class="modal fade" id="modalSeeDetail" tabindex="-1" aria-labelledby="modalSeeDetailLabel" aria-hidden="true">
  <div class="modal-dialog modal-mdplus">
    <div class="modal-content">
      <div class="modal-header text-white">
        <h5 class="modal-title" id="modalSeeDetailLabel">Detail Draft Item</h5>
      </div>
      <div class="modal-body">
        <table id="tblDetailItemRF" class="table table-bordered table-striped table-sm mb-0">
          <thead class="<%--table-dark--%> table-custome">
            <tr>
              <th>Code Item</th>
              <th>Nama Item</th>
              <th>Vendor</th>
              <th>Qty</th>
              <th>Price</th>
              <th>Amount</th>
            </tr>
          </thead>
          <tbody></tbody>
          <tfoot class="<%--table-light--%> table-custome">
            <tr>
              <td colspan="5" class="text-end fw-bold">VAT(%)</td>
              <td id="tdVat" class="fw-bold text-end"></td>
            </tr>
            <tr>
              <td colspan="5" class="text-end fw-bold">Grand Total</td>
              <td id="tdGrandTotal" class="fw-bold text-end "></td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  </div>
</div>








    <!--**********************************
			Modal Add Item
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlAddItem" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalAddItem.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalAddItem" OnClick="btnCloseModalAddItem_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
					</button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border"><i class="fa fa-search"></i><span class="nav-text">List of requesition item don't yet have a PO number</span></legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12'>sardi.evelina
                                            <br />
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableRequesitionItem" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableRequesitionItem_RowCommand" OnRowDataBound="TableRequesitionItem_RowDataBound" OnSelectedIndexChanged="TableRequesitionItem_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                                        <asp:BoundField DataField="nik_requester" HeaderText="nik_requester" />
                                                        <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                        <asp:BoundField DataField="DivisionRequester" HeaderText="Division" />
                                                        <asp:BoundField DataField="SectionRequester" HeaderText="Section" />
                                                        <asp:BoundField DataField="item_code" HeaderText="item_code" />
                                                        <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                        <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                        <asp:BoundField DataField="description" HeaderText="Description" />
                                                        <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                        <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                        <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                        <asp:BoundField DataField="remaks" HeaderText="Remark" />
                                                        <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                        <asp:BoundField DataField="type_request" HeaderText="Request Type" />
                                                        <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                        <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                        <asp:BoundField DataField="price" HeaderText="Price" />
                                                        <%--<asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" HtmlEncode="False" />--%>
                                                        <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                        <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                        <asp:BoundField DataField="no_po" HeaderText="no_po" />
                                                        <asp:BoundField DataField="amount" HeaderText="amount" />
                                                        <%--<asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:N0}" HtmlEncode="False" />--%>
                                                        <asp:BoundField DataField="catalog_type" HeaderText="catalog_type" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class='col-sm-12'>
                            <button type="button" style="float: right;" onclick="<%=btnSubmitItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                Submit
                                <span class="btn-icon-right"><i class="fa fa-check-circle"></i></span>
                            </button>
                            <asp:Button runat="server" Style="display: none;" ID="btnSubmitItem" OnClick="btnSubmitItem_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
    <script>
<%--        $(document).ready(function () {
            debugger
            const tableId = '#<%= TableDetailsRF.ClientID %>';
            //console.log("Table ID Rendered:", tableId);
            //console.log($(tableId));

            const table = $(tableId);
            //if (table.length === 0) {
            //    console.error("Tabel GridView tidak ditemukan di DOM!");
            //    return;
            //}

            table.DataTable({
                paging: true,
                searching: true,
                ordering: true,
                info: true
            });

            table.on('preDraw.dt', function () {
                $('.inline-form-row').each(function () {

                    saveFormData(this);

                });
            });

            table.on('draw.dt', function () {

                restoreInlineFormData();


                $('#<%= TableDetailsRF.ClientID %> tbody tr').each(function () {
                    const idVendor = $(this).data("idvendor");
                    const vendorName = $(this).data("vendor");

                    // Tambahkan hanya jika belum ada di array
                    if (idVendor && !allFormData.some(x => x.idVendor === idVendor)) {
                        allFormData.push({
                            idVendor,
                            vendor,
                            deliveryTo ="",
                            deliverToarea="",
                            deliveryDate="",
                            vat="",
                            paymentTerm="",
                            assetType="",
                            otherCondition="",
                            filename="",
                            oldnamefile=""
                        });
                    }
                });

            });
        });--%>


        $(document).ready(function () {
            debugger;
            const tableId = '#<%= TableDetailsRF.ClientID %>';
            const table = $(tableId).DataTable({
                paging: true,
                searching: true,
                ordering: true,
                info: true
            });

            initializeVendorData();

            // Simpan data sebelum redraw (misal saat search, paging, sort)
            table.on('preDraw.dt', function () {
                $('.inline-form-row').each(function () {
                    saveFormData(this);
                });
            });

            // Setelah redraw (misal setelah paging / search)
            table.on('draw.dt', function () {
                restoreInlineFormData();
                initializeVendorData();

            });


            function initializeVendorData() {
                $(`${tableId} tbody tr`).each(function () {
                    const row = $(this);

                    // Lewatkan baris inline form
                    if (row.hasClass("inline-form-row")) return;

                    const idVendor = row.data("idvendor");
                    const vendorName = row.data("vendor");

                    // Tambahkan jika belum ada
                    if (idVendor && !allFormData.some(x => x.idVendor === idVendor)) {
                        allFormData.push({
                            idVendor: idVendor,
                            vendor: vendorName,
                            deliveryTo: "",
                            deliverToarea: "",
                            deliveryDate: "",
                            vat: "",
                            paymentTerm: "30 days after invoice received",
                            assetType: "",
                            otherCondition: "",
                            filename: "",
                            oldnamefile: "",
                            attachmentPath: ""
                        });
                    }
                });
            }


        });


        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });

        $(document).on("change", ".delivery-select", function () {

            const selectedValue = $(this).val(); 
            const parentRow = $(this).closest(".inline-form-row");

            const deliveryTextarea = parentRow.find(".lbl-area-divto");
            const areaisi = parentRow.find(".delivery-to");


            if (selectedValue) {
               
                deliveryTextarea.slideDown(200);
                areaisi.val(selectedValue);
                const formRow = $(this).closest(".inline-form-row");
                saveFormData(formRow);

            } else {
                
                deliveryTextarea.slideUp(150).val("");
            }
        });

        function toggleInlineForm(btn) {
            var row = $(btn).closest("tr");
            var vendorName = row.data("vendor");
            var idVendor = row.data("idvendor");
            var next = row.next(".inline-form-row");

            if (next.length) {
                next.toggle("fast");
                return;
            }
            var formRow = $(`
                    <tr class="inline-form-row">
                      <td colspan="${row.children("td").length}">
                       
                        <div class="p-3 rounded border mt-2 bg-detail-dark">

                          <div class="row g-2">
                            <!-- Baris 1 -->
                           <div class="col-md-2">
                              <label class="fw-bold">Delivery To *</label>
                              <select class="form-select form-select-sm delivery-select">
                                <option value="">-- Select Warehouse --</option>
                                <option value="PT Yusen Logistics Indonesia, Temas Building Lantai 3A, Jl. Yos Sudarso Kav.33, Sunter Jaya, Jakarta Utara 14350, Indonesia">
                                  Temas Building - Jakarta Utara
                                </option>
                                <option value="PT Yusen Logistics Indonesia, Soewarna Business Park Blok A, Lot 1-2 Soekarno-Hatta Airport, Pajang, Benda, Tengerang, Banten 15126, Indonesia">
                                  Soewarna Business Park - Tangerang
                                </option>
                                <option value="PT Yusen Logistics Indonesia, Kawasan Industri MM2100 Blok EE-4, Desa Danau Indah, Cikarang Barat, Bekasi 17520, Indonesia">
                                  MM2100 - Bekasi
                                </option>
                                <option value="PT Yusen Logistics Indonesia, Ruko Permata Juanda, West Wing Super B/8-8A, Jl. Raya Juanda, Sedatiagung - Sedati, Sidoarjo 61253, Indonesia">
                                  Ruko Permata Juanda - Sidoarjo
                                </option>
                                <option value="PT Yusen Logistics Indonesia, HSBC Building 4th Floor Suite 408, Jl. Gajah Mada No. 135, Pekunden, Semarang 50134, Indonesia">
                                  HSBC Building - Semarang
                                </option>
                              </select>
                            </div>
                          <div class="col-md-2">
                          <label class="fw-bold">Delivery Date *</label>
                          <div class="d-flex align-items-center">
                            <input type="date" class="form-control form-control-sm me-2" id="txtDeliveryDate" />
                            <label class="fw-bold" style="margin-left:10px;">VAT(%)</label>
                            <input type="number" placeholder="VAT" class="form-control form-control-sm me-2" 
                            id="txtVatPercent" min="0" max="100" style="width: 200px;margin-left:5px;" value="0" />
                            <button type="button" class="btn btn-secondary btn-sm ml-3 btnSeeDetail"  
                                    data-bs-toggle="modal"
                                    data-vendor="${vendorName}"
                                    data-idvendor="${idVendor}"
                                    data-bs-target="#modalSeeDetail" style="margin-top:2px;">
                              <i class="fa fa-eye"></i> See Detail Draf Item
                            </button>

                          </div>
                          </div>
                         </div>
                          <div class="row g-2 mt-2">
                            <!-- Baris 2 -->
                            <div class="col-md-2">
                              <label class="fw-bold">Payment Term</label>
                             <input type="text" class="form-control form-control-sm" 
                              value="30 days after invoice received" />
                            </div>
                            <div class="col-md-2">
                              <label class="fw-bold">Asset Type *</label>
                              <select class="form-select form-select-sm">
                                <option value="">-- Select Asset --</option>
                                <option value="Fixed Asset">Fixed Asset </option>
                                <option value="Non Fixed Asset">Non Fixed Asset </option>
                              </select>
                            </div>
                            <div class="col-md-8 lbl-area-divto" style="display:none;">
                              <label class="fw-bold" style="margin-top:8px;" >Delivery To</label>
                              <textarea id="txtdeliveryto" class="form-control form-control-sm delivery-to" rows="5" ></textarea>
                            </div>
                            <div class="col-md-8">
                              <label class="fw-bold"  style="margin-top:8px;" >Other Condition</label>
                              <textarea id="txtothercondition" class="form-control form-control-sm" rows="5" ></textarea>
                            </div>
                          </div>
                          <div class="row g-2 mt-2">
                                <!-- Baris 3 -->
                                <div class="col-md-2">
                                  <label class="fw-bold" style="margin-top:8px;">Attachment</label>
                                  <div class="d-flex align-items-center">
                                  <input type="file" id="fileAttachment" style="width: 1000px" class="form-control form-control-sm file-input" accept=".pdf" />
                                  <input type="hidden" class="hfAttachmentPath" id="hfAttachmentPath"/> 
                                  <input type="hidden" class="oldnamefile" id="oldnamefile"/>  
                                  <button type="button" id="btncleare" class="btn btn-danger btn-sm ml-3 btncleare" style="margin-top:2px;">
                                      Clear <i class="fa fa-refresh"></i> 
                                  </button>
                                  </div>
                                   <small class="file-status"></small>
                                   <small class="text-danger ms-2 note-attachment">*Must not be more than 5m and file must be .PDF</small>
                                </div>
                          </div>
                          <div class="text-end mt-3">
                              <button type="button" class="btn btn-primary btn-sm btnSubmitForm"
                                data-index="${row.index()}"
                                data-idvendor="${idVendor}"
                                data-vendor="${vendorName}">
                                <i class="fa fa-paper-plane"></i> Submit 
                              </button>
                            </div>
                        </div>
                      </td>
                    </tr>`);
            row.after(formRow);

            formRow.hide().slideDown("fast");

            const saved = allFormData.find(x => x.idVendor === idVendor);
            if (saved) {       

                debugger
                formRow.find('#txtDeliveryDate').val(saved.deliveryDate);
                formRow.find('#txtVatPercent').val(saved.vat);
                formRow.find('input[type="text"]').val(saved.paymentTerm);
                formRow.find('select.form-select-sm').eq(1).val(saved.assetType);
                formRow.find('#txtothercondition').val(saved.otherCondition);
                formRow.find('#hfAttachmentPath').val(saved.filename);
                formRow.find('#oldnamefile').val(saved.oldnamefile);

                if (saved.filename != "" || saved.filname != null) {
                    const fileStatus = formRow.find(".file-status");
                    fileStatus.text("✅ " + saved.oldnamefile + " berhasil diunggah");

                }

                const deliverySelect = formRow.find('.delivery-select');         
                const selectedValue = saved.deliveryTo;

                deliverySelect.val(selectedValue);

                const deliveryTextarea = formRow.find(".lbl-area-divto");
                const areaisi = formRow.find(".delivery-to");

                if (selectedValue) {
                    deliveryTextarea.slideDown(200);
                    areaisi.val(saved.deliverToarea);
                } else {
                    deliveryTextarea.slideUp(150).val("");
                }
            }

        }

        $(document).on("change", ".file-input", function () {
            debugger
            const rfNumber = $("#<%= lblRFNumber.ClientID %>").text().trim();
            const fileInput = $(this)[0];
            const file = fileInput.files[0];
            const parentRow = $(this).closest(".inline-form-row");
            const fileStatus = parentRow.find(".file-status");
            const idVendor = parentRow.prev("tr").data("idvendor");
            const namavendor = parentRow.prev("tr").data("vendor")

            if (!file) return;

            if (file.size > 5 * 1024 * 1024) {
                fileStatus.text("❌ File to large (max 5MB)").removeClass("text-success text-warning").addClass("text-danger");;
                fileInput.value = "";
                return;
            }

            if (!file.name.toLowerCase().endsWith(".pdf")) {
                fileStatus.text("❌ Only PDF files are allowed").removeClass("text-success text-warning").addClass("text-danger");;
                fileInput.value = "";
                return;
            }

            // Tampilkan status upload
            fileStatus.text("⏳ Mengunggah...");

            const formData = new FormData();
            formData.append("fileAttachment", file);
            formData.append("vendorId", idVendor);
            formData.append("namavendor", namavendor);
            formData.append("RF", rfNumber)

            fetch(
                "UploadAttachmentHandler.ashx", {
                method: "POST",
                body: formData
            })
                .then(res => res.json()).then(result => {
                    if (result.success) {

                        parentRow.find("#hfAttachmentPath").val(result.path);
                        parentRow.find("#oldnamefile").val(result.fileName);

                        const formRow = $(this).closest(".inline-form-row");
                        saveFormData(formRow);

                        fileStatus.text("✅ " + result.fileName + " berhasil diunggah").removeClass("text-danger text-warning").addClass("text-success");

                    } else {
                        fileStatus.text("❌ Gagal upload: " + result.message).removeClass("text-success text-warning").addClass("text-danger");
                    }
                })
                .catch(err => {
                    //console.error(err);
                    fileStatus.text("❌ Terjadi kesalahan saat upload").removeClass("text-success text-warning").addClass("text-danger");
                });
        });


        let allFormData = [];

        function saveFormData(rowElement) {

            debugger

            const formRow = $(rowElement);
            const idVendor = formRow.find(".btnSubmitForm").data("idvendor");
            const vendor = formRow.find(".btnSubmitForm").data("vendor");
            const deliveryoptn = formRow.find(".delivery-select option:selected").text();
            const deliveryTo = formRow.find(".delivery-select").val();
            const deliverToarea = formRow.find("#txtdeliveryto").val();
            const deliveryDate = formRow.find("#txtDeliveryDate").val();
            const vat = formRow.find("#txtVatPercent").val();
            const paymentTerm = formRow.find('input[type="text"]').val();
            const assetType = formRow.find('select.form-select-sm').eq(1).val();
            const otherCondition = formRow.find("#txtothercondition").val();
            const filename = formRow.find("#hfAttachmentPath").val();
            const oldnamefile = formRow.find('#oldnamefile').val();

          
            allFormData = allFormData.filter(d => d.idVendor !== idVendor);

            
            allFormData.push({

                idVendor,
                vendor,
                deliveryoptn,
                deliveryTo,
                deliverToarea,
                deliveryDate,
                vat,
                paymentTerm,
                assetType,
                otherCondition,
                filename,
                oldnamefile

            });
        }

        $(document).on("change keyup", ".inline-form-row :input", function () {
            const formRow = $(this).closest(".inline-form-row");
            saveFormData(formRow);
        });

       
      
        // untuk lihat detail
         $(document).ready(function () {

         const detailRFData = JSON.parse($("#<%= hfDetailRF.ClientID %>").val() || "[]");

            $(document).on("click", ".btnSeeDetail", function () {
                const vendorName = $(this).data("vendor");
                const tbody = $("#tblDetailItemRF tbody");
                const tdVat = $("#tdVat");
                const tdGrand = $("#tdGrandTotal");
                tbody.empty();
                tdVat.text("");
                tdGrand.text("");


                const parentRow = $(this).closest(".inline-form-row");
                const vatPercent = parseFloat(parentRow.find("#txtVatPercent").val()) || 0;


                if (!vendorName) {
                    tbody.append(`<tr><td colspan="6" class="text-center text-danger">Vendor name not found</td></tr>`);
                    $("#modalSeeDetail").modal("show");
                    return;
                }

                const filtered = detailRFData.filter(item =>
                    item["vendor"] &&
                    item["vendor"].toUpperCase().trim() === vendorName.toUpperCase().trim()
                );

                let totalAmount = 0;

                if (filtered.length === 0) {
                    tbody.append(`<tr><td colspan="6" class="text-center">No data found for vendor: <b>${vendorName}</b></td></tr>`);
                } else {

                    filtered.forEach(item => {
                        const amount = Number(item["amount"]) || 0;
                        totalAmount += amount;

                        const row = `
                            <tr>
                                <td>${item["code_item"] ?? ""}</td>
                                <td>${item["item_name"] ?? ""}</td>
                                <td>${item["vendor"] ?? ""}</td>
                                <td class="text-center">${item["qty"] ?? ""}</td>
                                <td class="text-end">Rp. ${Number(item["price"] || 0).toLocaleString('id-ID')}</td>
                                <td class="text-end">Rp. ${amount.toLocaleString('id-ID')}</td>
                            </tr>`;
                        tbody.append(row);
                    });
                }


                const vat = totalAmount * (vatPercent / 100);
                const grandTotal = totalAmount + vat;

                tdVat.text(`Rp. ${vat.toLocaleString('id-ID')}`);
                tdGrand.text(`Rp. ${grandTotal.toLocaleString('id-ID')}`);


                $("#modalSeeDetail").modal("show");
            });
         });


 

        $(document).on("click", ".btncleare", function () {
            debugger
            const $parentRow = $(this).closest(".inline-form-row");
            const $fileInput = $parentRow.find(".file-input");
            const $fileStatus = $parentRow.find(".file-status");
            const $hiddenPath = $parentRow.find("#hfAttachmentPath");
            const $oldnamefile = $parentRow.find("#oldnamefile");

            const existingPath = $hiddenPath.val();

            if (!existingPath) {
                $fileStatus.text("⚠️ No file to delete").removeClass("text-success text-danger").addClass("text-warning");
                return;
            }

            fetch("DeleteAttachmentHandler.ashx", {
                method: "POST",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: "path=" + encodeURIComponent(existingPath)
            })
                .then(res => res.json())
                .then(result => {
                    if (result.success) {
             
                        $fileInput.val("");
                        $hiddenPath.val("");
                        $oldnamefile.val("");
                        $fileStatus.text("🗑️ File success deleted").removeClass("text-danger text-warning").addClass("text-success");
                    } else {
                        $fileStatus.text("❌ Filed to delete file: " + (result.message || "Tidak diketahui")).removeClass("text-success text-warning").addClass("text-danger");
                    }
                })
                .catch(err => {
                    console.error("Delete error:", err);
                    $fileStatus.text("❌ An error occurred while deleting the file").removeClass("text-success text-warning").addClass("text-danger");
                });


        });


        $(document).on("click", ".btnSubmitForm", function () {

            debugger;

            const rowIndex = $(this).data("index");
            const id_vendor = $(this).data("idvendor");
            const formContainer = $(this).closest(".inline-form-row");
            const filePath = formContainer.find("#hfAttachmentPath").val();
            const deliveryselect = formContainer.find(".delivery-select option:selected").val();
            const deliveryselect2 = formContainer.find(".delivery-select option:selected").text();
            const deliveryTo = formContainer.find("#txtdeliveryto").val();
            const deliveryDate = formContainer.find("#txtDeliveryDate").val();
            const vat = formContainer.find("#txtVatPercent").val();
            const assetType = formContainer.find("select:contains('Asset')").val();
            const paymentTerm = formContainer.find("input[type=text]").val();
            const otherCondition = formContainer.find("#txtothercondition").val();

            /*const remarks = remarks*/

            // Validasi
            if (!deliveryDate || !assetType || !deliveryselect) {

                swal("Required!", "Delivery Date, Asset Type, & Delivery To are mandatory!", "error");
                return;
            }

            ShowLoading();
            lanjutPostback();

            // Fungsi postback
            function lanjutPostback() {

                const formData = {
                    RowIndex: rowIndex,
                    DeliveryTo: deliveryTo,
                    Deliveryselect: deliveryselect2,
                    DeliveryDate: deliveryDate,
                    Vat: vat,
                    AssetType: assetType,
                    PaymentTerm: paymentTerm,
                    OtherCondition: otherCondition,
                    IDVendor: id_vendor,
                    FilePath: filePath
                };

                $("#<%=hfFormData.ClientID %>").val(JSON.stringify(formData));
              __doPostBack('<%= btnSubItemsatuan.UniqueID %>', '');
                }
          });


        async function handleSubmitAll() {

            debugger

            if (allFormData.length === 0) {
                swal("No Data!", "No found data to submit.", "error");
                return;
            }

            // Validasi semua form
            const invalidVendors = allFormData
                .filter(item => !item.deliveryDate || !item.assetType || !item.deliveryTo)
                .map(v => v.vendor);

            if (invalidVendors.length > 0) {

                const vendorList = invalidVendors
                    .map(v => `<span style="color:red;">${v}</span>`)
                    .join("<br>");

                swal({
                    title: "Required!",
                    text: "Please check mandatory* field in this vendor:<br><br>" + vendorList,
                    type: "error",
                    html: true
                });

                return;
            }

            // Kirim semua data ke server (aspx)
            const jsonData = JSON.stringify(allFormData);
            $("#<%=hfFormData.ClientID %>").val(JSON.stringify(allFormData));

            //$("#hfFormData").val(jsonData);

            /*return;*/

            ShowLoading();
            // Trigger tombol server ASP.NET
            $("#<%= btnSubmitAll.ClientID %>").click();

       }



    </script>
</asp:Content>
