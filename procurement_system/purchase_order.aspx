<%@ Page Title="Purchase Order" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="purchase_order.aspx.cs" Inherits="procurement_system.purchase_order" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <script>
        $(document).ready(function () {
            $(".datetimepicker").bootstrapMaterialDatePicker({ format: 'DD-MMM-YYYY HH:mm', clearButton: true, autoclose: true, nowButton: true, nowText: 'Today', okText: 'Submit' })
        });
    </script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").bootstrapMaterialDatePicker({ format: 'MM/DD/YYYY', time: false, clearButton: true, autoclose: true, nowButton: true, nowText: 'Today', okText: 'Submit' })
        });
    </script>
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
        function SelectItem() {
            swal('Submit Failed!', 'Please select item first', 'error');
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Purchase Order (PO)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Purchase Order (PO)</li>
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

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' id="divDashoardPO" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Dashboard of Purchase Order</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div runat="server" class="row">
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">PO Issued</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalPOIssued" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">PO Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-file-edit"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailTotalPOIssued_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">PO Not Yet Approved</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalPONotYetApproved" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">PO Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-file-signature"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailTotalPONotYetApproved_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">PO Approaching Delivery Date</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalPOApproachingDeliveryDate" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">PO Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-clock-rotate-left"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailTotalPOApproachingDeliveryDate_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">Late PO Delivery Date</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalLatePODeliveryDate" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">PO Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-calendar-minus"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailTotalLatePODeliveryDate_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divFilter" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">PO Create & Filter</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="btn-group" role="group">
                                                    <button type="button" class="btn mb-1 buttonColor dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">PO Create</button>
                                                    <div class="dropdown-menu">
                                                        <asp:LinkButton class="dropdown-item" runat="server" OnClick="btnNewPOStandart_Click">PO Standart</asp:LinkButton>
                                                        <asp:LinkButton class="dropdown-item" runat="server" OnClick="btnNewPOManual_Click">PO Manual</asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                <br />
                                            </div>
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
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>PO No.</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtPONo" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Enter PO No.">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Status</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <asp:DropDownList ID="ddlStatus" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Complete" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Not Complete" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="On Process" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="CANCEL" Value="4"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                <button type="button" style="float: right;" class="btn buttonColor" onclick="<%=btnSubmit.ClientID %>.click()">
                                                    Search <span class="btn-icon-right"><i class="fa fa-search" aria-hidden="true"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Purchase Order (PO)</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TablePurchaseOrder" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TablePurchaseOrder_RowCommand" OnRowDataBound="TablePurchaseOrder_RowDataBound" OnSelectedIndexChanged="TablePurchaseOrder_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="View">
                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnView" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!--**********************************
	Modal Select RF Number
    ***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlSelectRFNumber" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel1">Find RF Number</h4>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                &nbsp;
						<div class="input-group">
                            <input id="txtRFNumber" runat="server" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Enter RF Number">
                            <div class="input-group-append">
                                <button type="button" class="btn buttonColor" onclick="<%=btnSearchRFNumber.ClientID %>.click()">
                                    <i class="fa fa-search"></i>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnSearchRFNumber" OnClick="btnSearchRFNumber_Click"></asp:Button>
                            </div>
                        </div>
                            </div>
                            <div class='col-sm-12'>
                                <br />
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Select RF Number</legend>
                                    <div style="margin-left: 10px; height: 150px; overflow: auto;">
                                        <div class="table-responsive">
                                            <asp:GridView ID="TableRFNumber" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap" AutoGenerateColumns="False" Style="width: 100%"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                <Columns>
                                                    <asp:HyperLinkField DataTextField="rf_no" DataNavigateUrlFields="rf_no" DataNavigateUrlFormatString="~/input_vendor.aspx?rf_no={0}"
                                                        HeaderText="RF Number" ItemStyle-Width="150" />
                                                </Columns>
                                            </asp:GridView>
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
                </div>
            </div>
        </div>
    </div>

    <!--**********************************
Modal View Detail
***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlViewDetail" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalViewDetailPODashboard.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalViewDetailPODashboard" OnClick="btnCloseModalViewDetailPODashboard_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
	                </button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border"><i class="fa fa-search"></i><span class="nav-text">&nbsp;List of Purchase Order</span></legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12' id="divTablePODetail" runat="server">
                                            <br />
                                            <div class="table-responsive">
                                                <asp:GridView ID="TablePODetail" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="View">
                                                            <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                            <ItemStyle CssClass="fixed-column" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnViewDetailPO" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewDetailPO_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
