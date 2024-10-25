<%@ Page Title="Approval Purchase Order (PO)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="approval_purchase_order.aspx.cs" Inherits="procurement_system.approval_purchase_order" %>

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

    <%--<script type="text/javascript">
        $(document).ready(function () {
            $(".grid").DataTable(
                {
                    scrollY: "400px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: true,
                    fixedColumns: {
                        leftColumns: 1 // Number of columns to fix on the left
                    }

                });
        });
    </script>--%>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Approval Purchase Order (PO)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-file-signature"></i>&nbsp;Approval</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Purchase Order (PO)</li>
        </ol>
    </div>

    <asp:HiddenField ID="hlbID" runat="server" />
    <asp:HiddenField ID="hlbiduser" runat="server" />
    <asp:HiddenField ID="hblNIK" runat="server" />
    <asp:HiddenField ID="hlbRequester" runat="server" />
    <asp:HiddenField ID="hblEmail" runat="server" />
    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        <asp:Label ID="lbttd" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblnik" runat="server" Text=""></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ApprovePurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
    </div>
    <asp:HiddenField ID="hblEmailRequester" runat="server" />
    <asp:HiddenField ID="hblEmailManager" runat="server" />
    <asp:HiddenField ID="hblEmailGM" runat="server" />
    <asp:HiddenField ID="hlbEmailManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbEmailGMAdm" runat="server" />
    <asp:HiddenField ID="hlbNIKManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbNIKGMAdm" runat="server" />
    <asp:HiddenField ID="hlbManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbGMAdm" runat="server" />
    <asp:HiddenField ID="hlbPosition" runat="server" />

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <h4 class="card-title">Approval Level</h4>
                        <!-- Nav tabs -->
                        <div class="default-tab">
                            <ul class="nav nav-tabs mb-3" role="tablist">
                                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#it_section_head">IT Head</a>
                                </li>
                                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#ga_section_head">GA Head</a>
                                </li>
                                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#gm_adm">Administration GM</a>
                                </li>
                                <li class="nav-item"><a class="nav-link" data-toggle="tab" href="#director">Director</a>
                                </li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade show active" id="it_section_head" role="tabpanel">
                                    <div class="p-t-15">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need IT Head Approved</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableITSectionHeadApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewITSectionHead" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewITSectionHead_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                                            <asp:BoundField DataField="code" HeaderText="Vendor Code" />
                                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" />
                                                                            <asp:BoundField DataField="po_date" HeaderText="PO Date" />
                                                                            <asp:BoundField DataField="delivery_date" HeaderText="Delivery Date" />
                                                                            <asp:BoundField DataField="aset_status" HeaderText="Asset Status" />
                                                                            <asp:BoundField DataField="CreateBy" HeaderText="Created by" />
                                                                            <asp:BoundField DataField="ApprovedByGM" HeaderText="GM Approved" />
                                                                            <asp:BoundField DataField="CheckedByGA" HeaderText="GA Checked" />
                                                                            <asp:BoundField DataField="CheckedByIT" HeaderText="IT Checked" />
                                                                            <asp:BoundField DataField="AuthorizedByDirector" HeaderText="Director Approved" />
                                                                            <asp:BoundField DataField="approve_status" HeaderText="Approval Status" />
                                                                            <asp:BoundField DataField="po_status" HeaderText="PO Status" />
                                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                                        </Columns>
                                                                    </asp:GridView>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">Approval History</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableHistoryITSectionHeadApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewHistoryITSectionHead" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewHistoryITSectionHead_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                                            <asp:BoundField DataField="tgl_approve" HeaderText="Approval Date" />
                                                                            <asp:BoundField DataField="nik_approver" HeaderText="NIK" />
                                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Approver" />
                                                                            <asp:BoundField DataField="level_approver" HeaderText="Approval Level" />
                                                                            <asp:BoundField DataField="approval_status" HeaderText="Approval Status" />
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

                                <div class="tab-pane fade" id="ga_section_head">
                                    <div class="p-t-15">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need GA Head Approved</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableGASectionHeadApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewGASectionHead" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewGASectionHead_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                                            <asp:BoundField DataField="code" HeaderText="Vendor Code" />
                                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" />
                                                                            <asp:BoundField DataField="po_date" HeaderText="PO Date" />
                                                                            <asp:BoundField DataField="delivery_date" HeaderText="Delivery Date" />
                                                                            <asp:BoundField DataField="aset_status" HeaderText="Asset Status" />
                                                                            <asp:BoundField DataField="CreateBy" HeaderText="Created by" />
                                                                            <asp:BoundField DataField="ApprovedByGM" HeaderText="GM Approved" />
                                                                            <asp:BoundField DataField="CheckedByGA" HeaderText="GA Checked" />
                                                                            <asp:BoundField DataField="CheckedByIT" HeaderText="IT Checked" />
                                                                            <asp:BoundField DataField="AuthorizedByDirector" HeaderText="Director Approved" />
                                                                            <asp:BoundField DataField="approve_status" HeaderText="Approval Status" />
                                                                            <asp:BoundField DataField="po_status" HeaderText="PO Status" />
                                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">Approval History</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableHistoryGASectionHeadApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewHistoryGASectionHead" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewHistoryGASectionHead_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                                            <asp:BoundField DataField="tgl_approve" HeaderText="Approval Date" />
                                                                            <asp:BoundField DataField="nik_approver" HeaderText="NIK" />
                                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Approver" />
                                                                            <asp:BoundField DataField="level_approver" HeaderText="Approval Level" />
                                                                            <asp:BoundField DataField="approval_status" HeaderText="Approval Status" />
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

                                <div class="tab-pane fade" id="gm_adm">
                                    <div class="p-t-15">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need Administration GM Approved</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableGMAdminApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewGMAdminApproval" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewGMAdminApproval_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                                            <asp:BoundField DataField="code" HeaderText="Vendor Code" />
                                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" />
                                                                            <asp:BoundField DataField="po_date" HeaderText="PO Date" />
                                                                            <asp:BoundField DataField="delivery_date" HeaderText="Delivery Date" />
                                                                            <asp:BoundField DataField="aset_status" HeaderText="Asset Status" />
                                                                            <asp:BoundField DataField="CreateBy" HeaderText="Created by" />
                                                                            <asp:BoundField DataField="ApprovedByGM" HeaderText="GM Approved" />
                                                                            <asp:BoundField DataField="CheckedByGA" HeaderText="GA Checked" />
                                                                            <asp:BoundField DataField="CheckedByIT" HeaderText="IT Checked" />
                                                                            <asp:BoundField DataField="AuthorizedByDirector" HeaderText="Director Approved" />
                                                                            <asp:BoundField DataField="approve_status" HeaderText="Approval Status" />
                                                                            <asp:BoundField DataField="po_status" HeaderText="PO Status" />
                                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">Approval History</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableHistoryGMAdminApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewHistoryGMAdmin" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewHistoryGMAdmin_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                                            <asp:BoundField DataField="tgl_approve" HeaderText="Approval Date" />
                                                                            <asp:BoundField DataField="nik_approver" HeaderText="NIK" />
                                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Approver" />
                                                                            <asp:BoundField DataField="level_approver" HeaderText="Approval Level" />
                                                                            <asp:BoundField DataField="approval_status" HeaderText="Approval Status" />
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

                                <div class="tab-pane fade" id="director">
                                    <div class="p-t-15">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need Director Approved</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableDirectorApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewDirector" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewDirector_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                                            <asp:BoundField DataField="po_no" HeaderText="PO Number" />
                                                                            <asp:BoundField DataField="code" HeaderText="Vendor Code" />
                                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" />
                                                                            <asp:BoundField DataField="po_date" HeaderText="PO Date" />
                                                                            <asp:BoundField DataField="delivery_date" HeaderText="Delivery Date" />
                                                                            <asp:BoundField DataField="aset_status" HeaderText="Asset Status" />
                                                                            <asp:BoundField DataField="CreateBy" HeaderText="Created by" />
                                                                            <asp:BoundField DataField="ApprovedByGM" HeaderText="GM Approved" />
                                                                            <asp:BoundField DataField="CheckedByGA" HeaderText="GA Checked" />
                                                                            <asp:BoundField DataField="CheckedByIT" HeaderText="IT Checked" />
                                                                            <asp:BoundField DataField="AuthorizedByDirector" HeaderText="Director Approved" />
                                                                            <asp:BoundField DataField="approve_status" HeaderText="Approval Status" />
                                                                            <asp:BoundField DataField="po_status" HeaderText="PO Status" />
                                                                            <asp:BoundField DataField="create_date" HeaderText="Creation Date" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div class='col-sm-12'>
                                                <fieldset class="scheduler-border">
                                                    <legend class="scheduler-border">Approval History</legend>
                                                    <div class="control-group">
                                                        <div class="row">
                                                            <div class='col-sm-12'>
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="TableHistoryDirectorApproval" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Action">
                                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                                <ItemStyle CssClass="fixed-column" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton runat="server" ID="btnViewHistoryDirector" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewHistoryDirector_Click" ToolTip="View Details"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                                            <asp:BoundField DataField="tgl_approve" HeaderText="Approval Date" />
                                                                            <asp:BoundField DataField="nik_approver" HeaderText="NIK" />
                                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Approver" />
                                                                            <asp:BoundField DataField="level_approver" HeaderText="Approval Level" />
                                                                            <asp:BoundField DataField="approval_status" HeaderText="Approval Status" />
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
                </div>
            </div>
        </div>
    </div>


    <!--**********************************
		Modal View ManagerApproval
	***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlViewManagerApproval" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalView.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalView" OnClick="btnCloseModalView_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
				</button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">
                                    <i class="fa fa-file-text-o"></i>
                                    <span class="nav-text">&nbsp;Requisition Form (RF) -
                                    <asp:Label runat="server" ID="lbRFNumberHeader"></asp:Label>
                                        <asp:LinkButton ID="btnDownloadRF" runat="server" class="badge badge-pill badge-light" OnClick="btnDownloadRF_Click" Text="">Download Form</asp:LinkButton>
                                    </span>

                                </legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-2'>
                                            <div class="form-group">
                                                <label>Request Date</label>
                                            </div>
                                        </div>
                                        <div class='col-sm-4'>
                                            <div class="form-group">
                                                :&nbsp;<asp:Label runat="server" ID="lbRequestDate"></asp:Label>
                                            </div>
                                        </div>
                                        <div hidden>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Approved by</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbApprovedBy"></asp:Label>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Acknowledge by</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbAcknowledgeBy"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class='col-sm-2'>
                                            <div class="form-group">
                                                <label>Request by</label>
                                            </div>
                                        </div>
                                        <div class='col-sm-4'>
                                            <div class="form-group">
                                                :&nbsp;<asp:Label runat="server" ID="lbRequester"></asp:Label>
                                            </div>
                                        </div>

                                        <div class='col-sm-2'>
                                            <div class="form-group">
                                                <label>Division/Section</label>
                                            </div>
                                        </div>
                                        <div class='col-sm-4'>
                                            <div class="form-group">
                                                :&nbsp;<asp:Label runat="server" ID="lbDivision"></asp:Label>
                                                <asp:Label runat="server" ID="Label1">/</asp:Label>
                                                <asp:Label runat="server" ID="lbSection"></asp:Label>
                                            </div>
                                        </div>
                                        <div class='col-sm-2'>
                                            <div class="form-group">
                                                <label>Location</label>
                                            </div>
                                        </div>
                                        <div class='col-sm-4'>
                                            <div class="form-group">
                                                :&nbsp;<asp:Label runat="server" ID="lbLocation"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border"><i class="fa-solid fa-file-lines"></i><span class="nav-text">&nbsp;Item Request Details</span></legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12'>
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="No.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="stok_code" HeaderText="Code Stock" />
                                                        <asp:BoundField DataField="item_code" HeaderText="Code Item" />
                                                        <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                        <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                        <asp:BoundField DataField="description" HeaderText="Description" />
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                        <asp:BoundField DataField="remaks" HeaderText="Remarks" />
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
    <asp:HiddenField ID="ActiveTabHiddenField" runat="server" />
    <!-- Datatables -->

    <link href="./plugins/tables/css/datatable/dataTables.bootstrap4.min.css" rel="stylesheet">
    <script src="./plugins/tables/js/jquery.dataTables.min.js"></script>
    <script src="./plugins/tables/js/datatable/dataTables.bootstrap4.min.js"></script>
    <script src="./plugins/tables/js/datatable-init/datatable-basic.min.js"></script>

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script src="vendors/sweetalert.js"></script>
    <script src="vendors/sweetalert.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>
</asp:Content>
