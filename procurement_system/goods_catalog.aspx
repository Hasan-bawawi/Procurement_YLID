<%@ Page Title="Goods Catalog" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="goods_catalog.aspx.cs" Inherits="procurement_system.goods_catalog" %>

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
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Data Item Successfully Submited to Catalog',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'goods_catalog.aspx';
                }
            );
        }
    </script>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Data Item Successfully Updated from Catalog',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'goods_catalog.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncDelete() {
            swal({
                title: 'Delete Success',
                text: 'Data Item Successfully Deleted from Catalog',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'goods_catalog.aspx';
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
        function SelectItem() {
            swal('Save Failed!', 'Please select item name!', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectMerk() {
            swal('Save Failed!', 'Please select merk name!', 'error');
        }
    </script>
    <script type="text/javascript">
        function FieldType() {
            swal('Save Failed!', 'Please enter type item!', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectStockCriteria() {
            swal('Save Failed!', 'Please select stock criteria!', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectCatalogType() {
            swal('Save Failed!', 'Please enter catalog type!', 'error');
        }
    </script>
    <script type="text/javascript">
        function DuplicateCode() {
            swal('Save Failed!', 'Item Code is already in Catalog, please submit to Good Stock!', 'error');
        }
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
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
            <h1 class="page-header text-overflow" style="color: white;">Goods Catalog</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Goods Catalog</li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblStockCode" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Goods Catalog</legend>
                                    <div class='col-sm-12'>
                                        <button type="button" onclick="<%=btnAddNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                            Add
                                                <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                        </button>
                                        <asp:Button runat="server" Style="display: none;" ID="btnAddNew" OnClick="btnAddNew_Click"></asp:Button>
                                    </div>
                                    <div class='col-sm-12'>
                                        <div class="table-responsive">
                                            <asp:GridView ID="TableCatalog" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                                            <%--<asp:LinkButton runat="server" ID="btnDelete" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnDelete_Click" ToolTip="Delete"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="id" HeaderText="id" />
                                                    <asp:BoundField DataField="catalog_type" HeaderText="Catalog Type" />
                                                    <asp:BoundField DataField="Category" HeaderText="Category" />
                                                    <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item Name" />
                                                    <asp:BoundField DataField="ItemMerk" HeaderText="Merk" />
                                                    <asp:BoundField DataField="tipe" HeaderText="Type/Description" />
                                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                    <asp:BoundField DataField="criteria_stock" HeaderText="Stock Criteria" />
                                                    <asp:BoundField DataField="active" HeaderText="Active" />
                                                    <asp:BoundField DataField="id_category" HeaderText="id_category" />
                                                    <asp:BoundField DataField="id_item" HeaderText="id_item" />
                                                    <asp:BoundField DataField="id_merk" HeaderText="id_merk" />
                                                    <asp:BoundField DataField="id_unit" HeaderText="id_unit" />
                                                    <asp:BoundField DataField="MinStock" HeaderText="MinStock" />
                                                    <asp:BoundField DataField="status_asset" HeaderText="Asset Status" />
                                                    <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                </Columns>
                                            </asp:GridView>
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
			Modal Add
		***********************************-->
    <div class="modal fade bd-example-modal-lg" id="mdlAddItem" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">Item Details</legend>
                                <div class="row">
                                    <asp:HiddenField ID="hlbID" runat="server" />
                                    <asp:HiddenField ID="hblIDKategori" runat="server" />
                                    <div class='col-sm-6'>
                                        Catalog Type
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlCatalogType" class="form-control custom-select mr-sm-2" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlCatalogType_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                    <asp:ListItem Enabled="true" Text="Select Type" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="GA" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="IT" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="OPS" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Item Code
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtCode" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Code" disabled>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Categories
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlKategori" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlKategori_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <%--<div class='col-sm-12'></div>--%>
                                    <div class='col-sm-6'>
                                        Type/Description
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtType" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type/Description">
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Item Name
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlItemName" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Stock Criteria
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlCriteria" class="form-control custom-select mr-sm-2" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlCriteria_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                    <asp:ListItem Enabled="true" Text="Select Criteria" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="STOCK" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="NON STOCK" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Merk
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlMerk" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlMerk_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Asset Status
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlStatusAsset" class="form-control custom-select mr-sm-2" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlCriteria_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                    <asp:ListItem Enabled="true" Text="Select Status" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Fixed Asset" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Non Fixed Asset" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
                                        Unit
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlUnit" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-3'>
                                        Status
										<div class="input-group mb-3">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox runat="server" ID="ckActive" />
                                                </div>
                                            </div>
                                            <input runat="server" id="txtActive" type="text" class="form-control" value="ACTIVE" disabled>
                                        </div>
                                    </div>
                                    <div class='col-sm-3'>
                                        &nbsp;
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:Button runat="server" Visible="false" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                                <asp:Button runat="server" Visible="false" ID="btnUpdate" CssClass="btn buttonColor" Text="Update" OnClick="btnUpdate_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".grid").DataTable(
                {
                    scrollY: "500px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: true
                    //fixedColumns: {
                    //	left: 2
                    //}

                });
        });
    </script>
    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', timeFormat: "hh:mm:ss", autoclose: true, todayBtn: 'linked' })
        });
    </script>
</asp:Content>
