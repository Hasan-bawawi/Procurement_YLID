<%@ Page Title="RF Price Check" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="requisition_price_check.aspx.cs" Inherits="procurement_system.requisition_price_check" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="vendors/toastr/jquery.min.js"></script>
    <link href="vendors/toastr/toastr.min.css" rel="stylesheet" />
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
                    window.location.href = 'requisition_price_check.aspx';
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
                    window.location.href = 'requisition_price_check.aspx';
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
            <h1 class="page-header text-overflow" style="color: white;">RF Price Check</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;RF Price Check</li>
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
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need Input Price</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableRequisitionFormPriceEstimated" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <HeaderStyle CssClass="fixed-column fixed-column-header" />
                                                                <ItemStyle CssClass="fixed-column" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnPriceEstimate" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnPriceEstimate_Click" ToolTip="Input Price"><i class="fa-solid fa-pen-to-square"></i>&nbsp;&nbsp;Price Input</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                            <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                                            <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                                            <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                                           <%-- <asp:BoundField DataField="DirectorApprove" HeaderText="Director Approve" />
                                                            <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Head Approve" />
                                                            <asp:BoundField DataField="AdmManagerApprove" HeaderText="GA Head Approve" />
                                                            <asp:BoundField DataField="AdmGMApprove" HeaderText="Admin GM Approve" />
                                                            <asp:BoundField DataField="AdmDirectorApprove" HeaderText="Admin Director Approve" />--%>
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
                            <%--<div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Requisition Form (RF) - Need Price Fixed & Vendor</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableRequisitionFormPriceFixed" runat="server" CssClass="table table-striped row-border order-column table-bordered nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnPriceFixed" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnPriceFixed_Click" ToolTip="Input Price Fixed"><i class="fa-solid fa-pen-to-square"></i>&nbsp;&nbsp;Price Fixed</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                                            <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                                            <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                                            <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                                            <asp:BoundField DataField="DirectorApprove" HeaderText="Director Approve" />
                                                            <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Head Approve" />
                                                            <asp:BoundField DataField="AdmManagerApprove" HeaderText="GA Head Approve" />
                                                            <asp:BoundField DataField="AdmGMApprove" HeaderText="Admin GM Approve" />
                                                            <asp:BoundField DataField="AdmDirectorApprove" HeaderText="Admin Director Approve" />
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
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
