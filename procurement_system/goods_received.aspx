<%@ Page Title="Good Received (GR)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="goods_received.aspx.cs" Inherits="procurement_system.goods_received" %>

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


<style>

    /* ===== MODAL SIZE ===== */
    .modal-super {
        max-width: 95% !important;
        width: 95% !important;
    }

    .modal-body {
        font-size: 14px;
    }

    /* ===== FIELDSET STYLE ===== */
    .scheduler-border {
        border: 1px solid #dee2e6;
        border-radius: 8px;
        padding: 20px;
        background-color: #ffffff;
        margin-bottom: 20px;
    }

    .scheduler-border legend {
        font-size: 16px;
        font-weight: 600;
        padding: 0 10px;
        width: auto;
        color: #ff6600;
    }

    /* ===== FORM STYLE ===== */
    .form-label-custom {
        font-weight: 600;
    }

    .form-control[disabled],
    textarea[disabled] {
        background-color: #f8f9fa;
    }


    .gr-table thead {
        background-color: #06183d !important;
    }

    .gr-table thead th {
        background-color: #06183d !important;
        color: #ffffff !important;
        text-align: center;
    }

    .table td {
        vertical-align: middle;
    }

</style>


    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".grid").DataTable(
                {
                    scrollY: "400px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: true
                    //fixedColumns: {
                    //	left: 2
                    //}

                });
        });
    </script>

    <style>
        /* CSS for fixing the column */
        .fixed-column {
            position: sticky;
            left: 0;
            background-color: white; /* Background color to cover the overlapping content */
            z-index: 1; /* Ensures the fixed column is above other content */
            width: 150px; /* Set a fixed width for the column */
        }

        .fixed-column-header {
            position: sticky;
            left: 0;
            background-color: #06183d; /* Match the header background color */
            color: white; /* Match the header text color */
            z-index: 2; /* Higher z-index for header to be above the content */
            width: 150px; /* Set a fixed width for the header */
        }
    </style>
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

    <style>
        .width-212 {
            width: 212px;
        }
    </style>

    <script type="text/javascript">
        <%--function checkDate() {
            //console.log('Function checkDate() called.');
            var selectedDate = new Date(document.getElementById('<%= GRDate.ClientID %>').value);
            var estDate = new Date(document.getElementById('<%= lbDeliveryDate.ClientID %>').value);
            var label = document.getElementById('<%= LabelPesanNotif.ClientID %>');

            //console.log('Selected Date:', selectedDate);
            //console.log('Estimated Date:', estDate);

            if (selectedDate > estDate) {
                label.innerText = 'Note : Goods Receipt Date is late than the Delivery Date.';
            } else {
                label.innerText = '';
            }
        }--%>
        function checkDate() {
            var selectedDate = new Date(document.getElementById('<%= GRDate.ClientID %>').value);
            var estDate = new Date(document.getElementById('<%= lbDeliveryDate.ClientID %>').value);
            var badge = document.getElementById('<%= badgePesanNotif.ClientID %>');


            // Hitung selisih hari
            var differenceInTime = selectedDate.getTime() - estDate.getTime();
            var differenceInDays = differenceInTime / (1000 * 3600 * 24);

            if (selectedDate > estDate) {
                badge.innerText = 'Note : Goods Receipt Date is ' + Math.abs(differenceInDays) + ' days late than the Delivery Date.';
                badge.className = 'label label-pill label-danger';
            } else if (selectedDate < estDate) {
                badge.innerText = 'Note : Goods Receipt Date is ' + Math.abs(differenceInDays) + ' days earlier than the Delivery Date.';
                badge.className = 'label label-pill label-success';
            } else {
                badge.innerText = 'Note : Goods Receipt Date is On Time from the Delivery date.';
                badge.className = 'label label-pill label-primary';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Good Received (GR)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Good Received (GR)</li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="hblNIK" runat="server" Text=""></asp:Label>
    </div>
    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
    </div>

    <asp:HiddenField ID="txtRequester" runat="server" />
    <asp:HiddenField ID="txtPosition" runat="server" />
    <asp:HiddenField ID="txtSection" runat="server" />
    <asp:HiddenField ID="hlbGrandTotal" runat="server" />
    <asp:HiddenField ID="hlbRFNumber" runat="server" />
    <asp:HiddenField ID="hlbYears" runat="server" />
    <asp:HiddenField ID="hlbYearsNew" runat="server" />
    <asp:HiddenField ID="hlblast_numberNew" runat="server" />
    <asp:HiddenField ID="hlbCatalog" runat="server" />
    <asp:HiddenField ID="hlbIDVendor" runat="server" />
    <asp:HiddenField ID="txtGRNumber" runat="server" />
    <asp:HiddenField ID="lbErrorUploadNotif" runat="server" />
    <asp:HiddenField ID="hlbOK" runat="server" />
    <asp:HiddenField ID="lblAttachment" runat="server" />

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Purchase Order (PO) On Process</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TablePurchaseOrder" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowDataBound="TablePurchaseOrder_RowDataBound">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Create">
                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnCreateGR" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnCreateGR_Click" ToolTip="Create GR"><i class="fa-solid fa-file-pen"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                            <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                            <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                            <asp:BoundField DataField="po_date" HeaderText="Issued Date" />
                                                            <asp:BoundField DataField="delivery_date" HeaderText="Delivery Date" />
                                                            <asp:BoundField DataField="aset_status" HeaderText="Asset Status" />
                                                            <asp:BoundField DataField="po_created_by" HeaderText="Created by" />
                                                            <asp:BoundField DataField="po_approved_by" HeaderText="Approved by" />
                                                            <asp:BoundField DataField="po_checked_by" HeaderText="Checked by" />
                                                            <asp:BoundField DataField="authorized_by" HeaderText="Authorized by" />
                                                            <asp:BoundField DataField="approve_status" HeaderText="Approval Status" />
                                                            <asp:BoundField DataField="po_status" HeaderText="PO Status" />
                                                            <asp:BoundField DataField="po_approved_by" HeaderText="po_approved_by" />
                                                            <asp:BoundField DataField="po_checked_by" HeaderText="po_checked_by" />
                                                            <asp:BoundField DataField="authorized_by" HeaderText="authorized_by" />
                                                            <asp:BoundField DataField="email_po_approved_by" HeaderText="email_po_approved_by" />
                                                            <asp:BoundField DataField="email_po_checked_by" HeaderText="email_po_checked_by" />
                                                            <asp:BoundField DataField="email_authorized_by" HeaderText="email_authorized_by" />
                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                            <asp:BoundField DataField="modifiedby" HeaderText="Modified by" />
                                                            <asp:BoundField DataField="modified_date" HeaderText="Modification Date" />
                                                            <asp:BoundField DataField="DayToDelivery" HeaderText="DayToDelivery" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>

                            <div class='col-sm-12' id="divFilter" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Search GR Created</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Create Date(From)</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtDate1" data-validate-length-range="5,15" runat="server" class="form-control form-control-user shadow-sm datepicker1" placeholder="Select Date">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Create Date(To)</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtDate2" data-validate-length-range="5,15" runat="server" class="form-control form-control-user shadow-sm datepicker1" placeholder="Select Date">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>GR No.</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtGRNo" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Enter GR No.">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>PO No.</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtPONo" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Enter PO No.">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>RF No.</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtRFNo" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Enter RF No.">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Vendor</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtVendor" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Enter Vendor Name">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                <button type="button" style="float: right;" class="btn buttonColor" onclick="<%=btnSearch.ClientID %>.click()">
                                                    Search <span class="btn-icon-right"><i class="fa fa-search" aria-hidden="true"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSearch" OnClick="btnSearch_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12 col-sm-12 " id="divViewGR" runat="server" visible="false">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Goods Received (GR)</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableGR" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        OnRowDataBound="TableGR_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="View">
                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />
                                                                <ItemTemplate>
                                                                    <asp:HiddenField 
                                                                        ID="hfAttachment" 
                                                                        runat="server" 
                                                                        Value='<%# ResolveUrl(Convert.ToString(Eval("AttachmentGR"))) %>' />
                                                                    <asp:LinkButton runat="server" ID="btnView" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                    <%--<button type="button"
                                                                            ID="btnSee"
                                                                            runat="server"
                                                                            class="btn btn-sm btn-primary"
                                                                            onclick='openPdfModal("<%# ResolveUrl(Convert.ToString(Eval("AttachmentGR"))) %>")'>
                                                                        See Attachment <i class="fa fa-eye"></i>
                                                                    </button>--%>
                                                                    <button type="button"
                                                                            ID="btnSee"
                                                                            runat="server"
                                                                            class="btn btn-sm btn-primary"
                                                                            data-path='<%# ResolveUrl(Convert.ToString(Eval("AttachmentGR"))) %>'
                                                                            onclick="openPdfFromButton(this)">
                                                                        See Attachment <i class="fa fa-eye"></i>
                                                                    </button>
                                                               </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="gr_no" HeaderText="GR Number" />
                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                            <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor" />
                                                            <asp:BoundField DataField="gr_date" HeaderText="GR Date" />
                                                            <asp:BoundField DataField="received_by" HeaderText="Received by" />
                                                            <asp:BoundField DataField="qty_received" HeaderText="QTY Received" />
                                                            <asp:BoundField DataField="note" HeaderText="Note" />
                                                            <asp:BoundField DataField="Created_by" HeaderText="Created by" />
                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                            <asp:BoundField DataField="Modified_by" HeaderText="Modified by" />
                                                            <asp:BoundField DataField="modified_date" HeaderText="Modification Date" />
                                                        </Columns>
                                                    </asp:GridView>
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

    <!--**********************************
    Modal Create GR
    ***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlCreateGR" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel1">Create Goods Received</h4>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">PO NO. :&nbsp;<asp:Label ID="lbPONumber" runat="server"></asp:Label></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Issued Date</strong></span>
                                                        </div>
                                                        <input type="text" id="lbIssuedDate" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Issued Date" disabled>
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
                                                            <span class="input-group-text width-212"><strong>Delivery Date</strong></span>
                                                        </div>
                                                        <input type="text" id="lbDeliveryDate" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Delivery Date" disabled>
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
                                                            <span class="input-group-text width-212"><strong>Asset Type</strong></span>
                                                        </div>
                                                        <input runat="server" id="lbAssetType" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Asset Type" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Vendor Name</strong></span>
                                                        </div>
                                                        <input runat="server" id="lbVendorName" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Vendor Name" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>PO Status</strong></span>
                                                        </div>
                                                        <input runat="server" id="lbPOStatus" data-validate-length-range="5,15" type="text" class="form-control" placeholder="PO Status" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Payment Terms</strong></span>
                                                        </div>
                                                        <input runat="server" id="lbPaymentTerms" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Payment Terms" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-file-lines"></i><span class="nav-text">&nbsp;Goods Receipt Detail</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Goods Received Date</strong></span>
                                                        </div>
                                                        <input type="text" id="GRDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker1" placeholder="GR Date" onchange="checkDate()">
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
                                                            <span class="input-group-text width-212"><strong>Received by</strong></span>
                                                        </div>
                                                        <input runat="server" id="txtReceivedBy" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Received by">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Note</strong></span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="txtNote" maxlength="250" runat="server" placeholder="Max. 250 Character"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text width-212"><strong>Doc. Upload</strong></span>
                                                        </div>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <asp:FileUpload ID="FileUploadEDocs" AllowMultiple="true" runat="server" /></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPO" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                            <asp:BoundField DataField="stok_code" HeaderText="stok_code" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="remarks" HeaderText="Remark" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("amount"))) %>' name="txtAmount" id="txtAmount" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty Received">
                                                                <ItemTemplate>
                                                                    <input type="number" class="input-group-text" runat="server" name="txtQtyReceived" id="txtQtyReceived" placeholder="Enter the Qty received" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class='col-sm-12' hidden>
                                                <div class="bootstrap-label">
                                                    <span id="badgePesanNotif" runat="server" class="label label-pill"></span>
                                                </div>
                                                <%--<asp:Label ID="LabelPesanNotif" runat="server" ForeColor="Red"></asp:Label>--%>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" onclick="<%=btnCloseModal.ClientID %>.click()" class="btn mb-1 buttonColor">
                        Close <span class="btn-icon-right">
                            <i class="fa fa-close"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModal" OnClick="btnCloseModal_Click"></asp:Button>
                    <button type="button" onclick="<%=btnSubmit.ClientID %>.click()" class="btn mb-1 buttonColor">
                        Submit <span class="btn-icon-right">
                            <i class="fa fa-save"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click"></asp:Button>
                </div>
            </div>
        </div>
    </div>

<%--<div class="modal fade bs-example-modal-xl" id="mdlViewGR" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">

            <!-- HEADER -->
            <div class="modal-header">
                <h4 class="modal-title">
                    View Detail Goods Received
                </h4>
                <button type="button" class="close" onclick="closeViewGR()">
                    <span>&times;</span>
                </button>
            </div>

            <!-- BODY -->
            <div class="modal-body" style="max-height:75vh; overflow-y:auto;">

                <div class="row">

                    <!-- ===================== PO INFORMATION ===================== -->

                    <div class='col-sm-12'>
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">
                                PO NO. :&nbsp;
                                <asp:Label ID="lbViewPONumber" runat="server"></asp:Label>
                            </legend>

                            <div class="row">

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Issued Date</strong></span>
                                        </div>
                                        <input type="text" id="lbViewIssuedDate" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Delivery Date</strong></span>
                                        </div>
                                        <input type="text" id="lbViewDeliveryDate" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Asset Type</strong></span>
                                        </div>
                                        <input type="text" id="lbViewAssetType" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Vendor Name</strong></span>
                                        </div>
                                        <input type="text" id="lbViewVendorName" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>PO Status</strong></span>
                                        </div>
                                        <input type="text" id="lbViewPOStatus" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Payment Terms</strong></span>
                                        </div>
                                        <input type="text" id="lbViewPaymentTerms" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                            </div>
                        </fieldset>
                    </div>

                    <!-- ===================== GR INFORMATION ===================== -->

                    <div class='col-sm-12 mt-3'>
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">
                                <i class="fa-solid fa-file-lines"></i>
                                &nbsp;Goods Receipt Detail
                            </legend>

                            <div class="row">

                                <!-- No GR -->
                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>No. GR</strong></span>
                                        </div>
                                        <input type="text" id="viewGRNumber" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <!-- GR Date -->
                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Goods Received Date</strong></span>
                                        </div>
                                        <input type="text" id="viewGRDate" runat="server"
                                            class="form-control" disabled />
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>

                                <!-- Received By -->
                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Received By</strong></span>
                                        </div>
                                        <input type="text" id="viewReceivedBy" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>

                                <!-- Note -->
                                <div class='col-sm-6'>
                                    <div class="input-group mb-3">
                                        <div class="input-group-append">
                                            <span class="input-group-text width-212"><strong>Note</strong></span>
                                        </div>
                                        <textarea id="viewNote" runat="server"
                                            class="form-control"
                                            rows="3"
                                            disabled></textarea>
                                    </div>
                                </div>

                            </div>
                        </fieldset>
                    </div>

                    <!-- ===================== TABLE DETAIL ===================== -->

                    <div class='col-sm-12 mt-3'>
                        <div class="table-responsive">
                            <asp:GridView ID="gvViewGRDetail"
                                runat="server"
                                CssClass="table table-bordered table-striped verticle-middle"
                                AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="true"
                                EmptyDataText="No Record Found">
                                <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                    <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                    <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                    <asp:BoundField DataField="stok_code" HeaderText="Stock Code" />
                                    <asp:BoundField DataField="item_code" HeaderText="Code" />
                                    <asp:BoundField DataField="item_name" HeaderText="Item" />
                                    <asp:BoundField DataField="quantity" HeaderText="Qty PO" />
                                    <asp:BoundField DataField="qty_received" HeaderText="Qty Received" />
                                    <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                    <asp:BoundField DataField="price" HeaderText="Price"
                                        DataFormatString="{0:N0}" />
                                    <asp:BoundField DataField="amount" HeaderText="Amount"
                                        DataFormatString="{0:N0}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
</div>--%>
<div class="modal fade" id="mdlViewGR" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-super modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">

            
            <div class="modal-header text-black">
                <h4 class="modal-title">
                    View Detail Goods Received
                </h4>
                <button type="button" class="close text-black" onclick="closeViewGR()">
                    <span>&times;</span>
                </button>
            </div>

            
            <div class="modal-body">
                <div class="container-fluid">

                    <!-- ===================== PO INFORMATION ===================== -->
                    <fieldset class="scheduler-border">
                        <legend>
                            PO NO :
                            <asp:Label ID="lbViewPONumber" runat="server"></asp:Label>
                        </legend>

                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Issued Date</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewIssuedDate" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Delivery Date</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewDeliveryDate" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Asset Type</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewAssetType" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Vendor Name</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewVendorName" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">PO Status</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewPOStatus" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Payment Terms</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="lbViewPaymentTerms" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </fieldset>


                    <!-- ===================== GR INFORMATION ===================== -->
                    <fieldset class="scheduler-border">
                        <legend>Goods Receipt Detail</legend>

                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">No. GR</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="viewGRNumber" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Goods Received Date</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="viewGRDate" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Received By</label>
                                    <div class="col-sm-8">
                                        <input type="text" id="viewReceivedBy" runat="server"
                                            class="form-control" disabled />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group row">
                                    <label class="col-sm-4 col-form-label form-label-custom">Note</label>
                                    <div class="col-sm-8">
                                        <textarea id="viewNote" runat="server"
                                            class="form-control"
                                            rows="2"
                                            disabled></textarea>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </fieldset>


                    <!-- ===================== TABLE DETAIL ===================== -->
                    <div class="table-responsive" style="max-height:400px; overflow:auto;">
                        <%--<asp:GridView ID="gvViewGRDetail"
                            runat="server"
                            CssClass="table table-bordered table-striped table-sm custom-header"
                            AutoGenerateColumns="False"
                            ShowHeaderWhenEmpty="true"
                            EmptyDataText="No Record Found">--%>
                        <asp:GridView ID="gvViewGRDetail"
                                        runat="server"
                                        CssClass="table table-bordered table-sm gr-table"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No Record Found"
                                        UseAccessibleHeader="true">    
                            <Columns>

                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                <asp:BoundField DataField="stok_code" HeaderText="Stock Code" />
                                <asp:BoundField DataField="item_code" HeaderText="Code" />
                                <asp:BoundField DataField="item_name" HeaderText="Item" />
                                <asp:BoundField DataField="quantity" HeaderText="Qty PO" />
                                <asp:BoundField DataField="qty_received" HeaderText="Qty Received" />
                                <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:N0}" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
    </div>
</div>



<div class="modal fade" id="pdfModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered"
         style="max-width:95%; width:95%;">
        <div class="modal-content">

            <!-- HEADER -->
            <div class="modal-header d-flex justify-content-between align-items-center">
                <h5 class="modal-title mb-0">Attachment Document</h5>


                <button type="button"
                        onclick="closePdfModal()"
                        style="border:none; background:transparent;
                               font-size:28px; line-height:1; cursor:pointer;">
                    &times;
                </button>
            </div>

            
            <div class="modal-body p-0" style="height:85vh;">
                <iframe id="pdfFrame"
                        style="width:100%; height:100%; border:none;">
                </iframe>
            </div>

        </div>
    </div>
</div>
    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
        <script>
            function openPdfFromButton(btn) {

                var path = btn.getAttribute("data-path");
                openPdfModal(path);

            }

            function openPdfModal(path) {
                debugger
                if (!path || path.trim() === "") {
                    alert("Attachment kosong.");
                    return;
                }

                document.getElementById("pdfFrame").src = path;

                $('#pdfModal').modal({
                    backdrop: 'static',
                    keyboard: false
                });

               
            }

            function closePdfModal() {
                $('#pdfFrame').attr('src', '');
                $('#pdfModal').modal('hide');
            }


            function openViewGR() {
                $('#mdlViewGR').modal('show');
            }

            function closeViewGR() {
                $('#mdlViewGR').modal('hide');
            }
        </script>
    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>
    <script>

        $(window).on('load', function () {

            var tableId = '#<%= TableGR.ClientID %>';

                if ($(tableId).length > 0) {

                    $(tableId).DataTable({
                        stateSave: true, 
                        destroy: true,   
                        order: [[1, 'desc']], 
                        columnDefs: [
                            { orderable: false, targets: 0 }
                        ]
                    });

                }

            });
    </script>
</asp:Content>
