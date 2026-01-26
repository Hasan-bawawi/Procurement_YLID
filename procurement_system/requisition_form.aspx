<%@ Page Title="Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="requisition_form.aspx.cs" Inherits="procurement_system.requisition_form" %>

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

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>

    <script src="plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <script src="plugins/bootstrap-daterangepicker/daterangepicker.js"></script>
    <script src="plugins/moment/moment.js"></script>
    <script src="plugins/bootstrap-material-datetimepicker/js/bootstrap-material-datetimepicker.js"></script>

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
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncDelete() {
            swal({
                title: 'Remove Success',
                text: 'Item Successfully Removed',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Form Successfully Update',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">  

        function DeleteConfirm() {
            var Ans = confirm("Do you want to remove selected record?");
            if (Ans) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function CannotEdit() {
            swal('Can not edit!', 'PO has been issued', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Requisition Form (RF)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Requisition Form (RF)</li>
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
                            <div class='col-sm-12' id="divDashoardRF" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Dashboard of Requisition Form</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div runat="server" class="row">
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">RF Created</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalRF" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">RF Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-file-edit"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailTotalRFCreated_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">Item Request</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalItemReq" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">Item</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-cart-arrow-down"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailItemReq_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">PO Not Yet Created</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalPONotYetCreated" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">Item</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-file-text"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailPONotYetCreated_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-3 col-sm-6">
                                                        <div class="card gradient-3">
                                                            <div class="card-body">
                                                                <h3 class="card-title text-white">RF Canceled</h3>
                                                                <div class="d-inline-block">
                                                                    <h2 class="text-white">
                                                                        <asp:Label ID="lbTotalRFCanceled" runat="server"></asp:Label></h2>
                                                                    <p class="text-white mb-0">RF Number</p>
                                                                </div>
                                                                <span class="float-right display-5 opacity-5"><i class="fa fa-file-excel"></i></span>
                                                            </div>
                                                            <div class="card-footer">
                                                                <a href="#">
                                                                    <asp:LinkButton runat="server" OnClick="ViewDetailRFCanceled_Click">View Details <i class="fa fa-arrow-circle-right"></i></asp:LinkButton></a>
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
                                    <legend class="scheduler-border">New Create & Filter</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnNew.ClientID %>.click()" class="btn buttonColor">
                                                    New
                                            <span class="btn-icon-right"><i class="fa fa-plus"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnNew" OnClick="btnNew_Click"></asp:Button>
                                            </div>
                                            <div class='col-sm-12'>
                                                <br />
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Request Date(From)</strong></div>
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
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Request Date(To)</strong></div>
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
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>RF No.</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtRFNo" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="RF No.">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Requester</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <input type="text" id="txtRequester" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Requester">
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
                                                            <asp:ListItem Text="PO Created" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Canceled" Value="4"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1"><strong>Branch</strong></div>
                                                <div class="form-group">
                                                    <div class='input-group'>
                                                        <asp:DropDownList ID="ddlBranch" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                    &nbsp;
                                                </div>
                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                    <button type="button" class="btn buttonColor" onclick="<%=btnSubmit.ClientID %>.click()">
                                                        Search <span class="btn-icon-right"><i class="fa fa-search" aria-hidden="true"></i></span>
                                                    </button>
                                                    <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divTableUser" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Requisition Form</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnNew_User.ClientID %>.click()" class="btn buttonColor">
                                                    New
                                                    <span class="btn-icon-right"><i class="fa fa-plus"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnNew_User" OnClick="btnNew_User_Click"></asp:Button>

                                                <%--<button type="button" onclick="<%=GenerateExcel.ClientID %>.click()" class="btn buttonColor">
                                                    Generate .xlxs
                                                    <span class="btn-icon-right"><i class="fa fa-file-excel-o"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="GenerateExcel" OnClick="GenerateExcel_Click"></asp:Button>--%>
                                            </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableRequisitionForm" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableRequisitionForm_RowCommand" DataKeyNames="POallcreated" OnRowDataBound="TableRequisitionForm_RowDataBound" OnSelectedIndexChanged="TableRequisitionForm_SelectedIndexChanged" >
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="View">
<%--                                                            <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />--%>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnView" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm buttonColorGridview" OnClick="btnView_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnCreatePO" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm btn-success" OnClick="btnCreatePO_Click" ToolTip="Create PO"> Create PO <i class="fa fa-file-circle-plus"></i></asp:LinkButton>
                                                                    <%--<asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog Type" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                            <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                                            <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                                            <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                                            <asp:BoundField DataField="DirectorApprove" HeaderText="Director Approve" />
                                                            <%--<asp:BoundField DataField="AdmGMApprove" HeaderText="Adm. GM Approve" />
                                                            <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Manager Approve" />--%>
                                                            <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                            <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                            <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                            <asp:BoundField DataField="no_po" HeaderText="no_po" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divTableAdmin" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Requisition Form</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <%--<button type="button" onclick="<%=GenerateExcel.ClientID %>.click()" class="btn buttonColor">
                                                    Generate .xlxs
                                                <span class="btn-icon-right"><i class="fa fa-file-excel-o"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnGenerateExcel_Admin" OnClick="btnGenerateExcel_Admin_Click"></asp:Button>--%>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableRequisitionFormFilter" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" OnRowDataBound="TableRequisitionFormFilter_RowDataBound"  DataKeyNames="POallcreated" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <%--<HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />--%>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnView_Admin" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm buttonColorGridview" OnClick="btnView_Admin_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnCreatePO" CommandName="Buat" CommandArgument='<%# Eval("rf_no") %>'  CssClass="btn btn-sm btn-success" OnClick="btnCreatePO_Click" ToolTip="Create PO"> Create PO <i class="fa fa-file-circle-plus"></i></asp:LinkButton>
                                                                    <%--<asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>--%>
                                                                    <%-- CommandArgument="<%# Container.DataItemIndex %>"--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog Type" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                            <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                                            <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                                            <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                                            <asp:BoundField DataField="DirectorApprove" HeaderText="Director Approve" />
                                                            <%--<asp:BoundField DataField="AdmManagerApprove" HeaderText="Adm. Manager Approve" />
                                                            <asp:BoundField DataField="AdmGMApprove" HeaderText="Adm. GM Approve" />
                                                            <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Manager Approve" />--%>
                                                            <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                            <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                            <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                            <asp:BoundField DataField="no_po" HeaderText="no_po" />
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
    Modal View Detail
	***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlViewDetail" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalViewDetailRF.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalViewDetailRF" OnClick="btnCloseModalViewDetailRF_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
		                </button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border"><i class="fa fa-search"></i><span class="nav-text">&nbsp;List of Requesition Form</span></legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12' id="divTableRFDetail" runat="server">
                                            <br />
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableRequesitionFormDetail" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                            <ItemStyle CssClass="fixed-column" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnView_DetailRF" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_DetailRF_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                <%--<asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="id" HeaderText="id" />
                                                        <asp:BoundField DataField="catalog_type" HeaderText="Calatog" />
                                                        <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                        <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                        <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                        <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                        <asp:BoundField DataField="stok_code" HeaderText="Stock Code" />
                                                        <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                        <asp:BoundField DataField="item_name" HeaderText="Item Name" />
                                                        <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                        <asp:BoundField DataField="tipe" HeaderText="Tipe" />
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                        <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                        <asp:BoundField DataField="remaks" HeaderText="Remarks" />
                                                        <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                        <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                        <asp:BoundField DataField="no_po" HeaderText="PO Number" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class='col-sm-12' id="divMasterRF" runat="server">
                                            <br />
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableMasterRF" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                            <ItemStyle CssClass="fixed-column" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnView_MasterRF" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_MasterRF_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                <%--<asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="id" HeaderText="id" />
                                                        <asp:BoundField DataField="catalog_type" HeaderText="Catalog Type" />
                                                        <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                        <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                        <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                        <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                                        <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                                        <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                                        <asp:BoundField DataField="AdmManagerApprove" HeaderText="Adm. Manager Approve" />
                                                        <asp:BoundField DataField="AdmGMApprove" HeaderText="Adm. GM Approve" />
                                                        <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Manager Approve" />
                                                        <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                        <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                        <asp:BoundField DataField="nama_branch" HeaderText="Location" />
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
