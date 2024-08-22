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
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
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
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="View">
                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnView" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_Click" ToolTip="View Details"><i class="fa-solid fa-eye"></i></asp:LinkButton>
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
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#.00}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#.00}", Convert.ToDecimal(Eval("amount"))) %>' name="txtAmount" id="txtAmount" data-type="currency" disabled />
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
                                            <div class='col-sm-12'>
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

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>
</asp:Content>
