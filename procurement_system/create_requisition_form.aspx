<%@ Page Title="Create Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="create_requisition_form.aspx.cs" Inherits="procurement_system.create_requisition_form" Async="true" %>

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
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Requisition Form Successfully Submited',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Requisition Form Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function SelectCategory() {
            swal('Save Failed!', 'Please select category name!', 'error');
        }
    </script>
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
    </script>
    <script type="text/javascript">
        function SelectItem() {
            swal('Save Failed!', 'Please select Item first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectReqType() {
            swal('Save Failed!', 'Please select Request Type first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectApprover() {
            swal('Save Failed!', 'Please select Manager Approver first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectApproverGM() {
            swal('Save Failed!', 'Please select GM Approver first', 'error');
        }
    </script>
    <script type="text/javascript">
        function FailedSend() {
            swal('Send Failed!', 'Check Email!', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Create Requisition Form (RF)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="requisition_form.aspx">&nbsp;Requisition Form (RF)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Create Requisition Form (RF)</li>
        </ol>
    </div>

    <asp:HiddenField ID="lblNamaBranch" runat="server" />
    <asp:HiddenField ID="hblNIK" runat="server" />
    <asp:HiddenField ID="txtRequester" runat="server" />
    <asp:HiddenField ID="txtPosition" runat="server" />
    <asp:HiddenField ID="txtSection" runat="server" />
    <asp:HiddenField ID="hlbCodeItem" runat="server" />
    <asp:HiddenField ID="hlbItem" runat="server" />
    <asp:HiddenField ID="hlbMerk" runat="server" />
    <asp:HiddenField ID="hlbType" runat="server" />
    <asp:HiddenField ID="hlbNameApprover" runat="server" />
    <asp:HiddenField ID="hlbNIKApprover" runat="server" />
    <asp:HiddenField ID="hlbBranchApprover" runat="server" />
    <asp:HiddenField ID="hlbEmailApprover" runat="server" />
    <asp:HiddenField ID="hlbEmailRequester" runat="server" />
    <asp:HiddenField ID="hlbSectionApprover" runat="server" />
    <asp:HiddenField ID="hlbRFNumber" runat="server" />
    <asp:HiddenField ID="hlbYears" runat="server" />
    <asp:HiddenField ID="hlbYearsNew" runat="server" />
    <asp:HiddenField ID="hlblast_numberNew" runat="server" />
    <asp:HiddenField ID="hlbNameApproverGM" runat="server" />
    <asp:HiddenField ID="hlbNIKApproverGM" runat="server" />
    <asp:HiddenField ID="hlbSectionApproverGM" runat="server" />
    <asp:HiddenField ID="hlbBranchApproverGM" runat="server" />
    <asp:HiddenField ID="hlbEmailApproverGM" runat="server" />

    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <asp:Image ID="imgQRCode" runat="server" />
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-file-pen"></i><span class="nav-text">&nbsp;Create Requisition Form (RF)</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6' hidden>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">RF Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtNota" data-validate-length-range="5,15" type="text" class="form-control" placeholder="RF Number" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Request Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtReqDate" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Request Date" disabled>
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
                                                            <span class="input-group-text">Request Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlReqType" class="selectpicker form-control" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlReqType_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Request Type" Value="-1"></asp:ListItem>
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
                                                            <span class="input-group-text">Catalog Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlCatalogType" class="selectpicker form-control" AppendDataBoundItems="true" AutoPostBack="true"
                                                            runat="server" OnSelectedIndexChanged="ddlCatalogType_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Catalog Type" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="GA" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="IT" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="OPS" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Manager Approver&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlApprover" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="true"
                                                            runat="server" OnSelectedIndexChanged="ddlApprover_SelectedIndexChanged">
                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--<div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">GM Approver&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlGMApprover" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="true"
                                                            runat="server" OnSelectedIndexChanged="ddlGMApprover_SelectedIndexChanged">
                                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <%--<div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Admin Manager&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtAdminManagerApprover" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Admin Manager Approver" disabled>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <%--<div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Admin GM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtAdminGMApprover" data-validate-length-range="5,15" runat="server" class="form-control" placeholder="Admin GM Approver" disabled>
                                                    </div>
                                                </div>
                                            </div>--%>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-file-lines"></i><span class="nav-text">&nbsp;Detail of Items Requested</span></legend>
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
                                                        <input runat="server" id="txtJumlahBeli" data-validate-length-range="5,15" type="number" class="form-control" placeholder="Quantity" onkeypress="return isNumberKey(event)" required="required">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Description&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="2" id="txtDescription" runat="server" required="required"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Remarks&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="2" id="txtRemaks" runat="server" required="required"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hlbNIKAdminManager" runat="server" />
                                            <asp:HiddenField ID="hlbNIKAdminGM" runat="server" />
                                            <div class='col-sm-12' id="divBtnAddCart" runat="server">
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
									                <span class="btn-icon-right"><i class="fa-solid fa-cart-plus"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>
                                            <div class='col-sm-12'>
                                                <br />
                                            </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPurchase_RowCommand" OnRowDataBound="TableItemPurchase_RowDataBound" OnSelectedIndexChanged="TableItemPurchase_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="stok_code" HeaderText="Code Stock" />
                                                            <asp:BoundField DataField="kode_barang" HeaderText="Code Item" />
                                                            <asp:BoundField DataField="nama_barang" HeaderText="Item" />
                                                            <asp:BoundField DataField="merk" HeaderText="Merk" />
                                                            <asp:BoundField DataField="description" HeaderText="Description" />
                                                            <asp:BoundField DataField="jumlah_beli" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="tanggal_beli" HeaderText="Request Date" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnRemove" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnRemove_Click" ToolTip="Remove"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class='col-sm-12' id="divSubmit" runat="server" visible="false">
                                                <button type="button" style="float: right;" onclick="<%=btnSubmit.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Submit
							                    <span class="btn-icon-right"><i class="fa fa-check-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click" OnClientClick="ShowLoading()"></asp:Button>
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

    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>


    <script type="text/javascript">
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
    <script type="text/javascript">
        $('#date').bootstrapMaterialDatePicker({ weekStart: 0, time: false });

    </script>
</asp:Content>
