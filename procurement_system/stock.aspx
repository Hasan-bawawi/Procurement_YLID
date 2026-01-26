<%@ Page Title="Goods Stock" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="stock.aspx.cs" Inherits="procurement_system.stock" %>

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
        function UploadSuccess() {
            swal({
                title: 'Upload Success',
                text: 'File Successfully Uploaded',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'stock.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Data Stock Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'stock.aspx';
                }
            );
        }
    </script>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Data Item Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'stock.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncDelete() {
            swal({
                title: 'Delete Success',
                text: 'Data Merk Successfully Deleted',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'stock.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FieldItemName() {
            swal('Save Failed!', 'Please enter item name', 'error');
        }
    </script>
    <script type="text/javascript">
        function CheckDuplicate() {
            swal({
                title: 'Save Failed!',
                text: 'Data Item already exist!',
                type: 'error',
                showConfirmButton: true,
                html: true
            },
                function redirect() {
                    window.location.href = 'stock.aspx';
                }
            );
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
            <h1 class="page-header text-overflow" style="color: white;">Goods Stock</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Goods Stock</li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblID" runat="server" Text=""></asp:Label>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Goods Stock</legend>
                                    <div class='col-sm-12'>
                                        <button type="button" onclick="<%=btnAddNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                            Add
                                                <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                        </button>
                                        <asp:Button runat="server" Style="display: none;" ID="btnAddNew" OnClick="btnAddNew_Click"></asp:Button>
                                    </div>
                                    <div class='col-sm-12'>
                                        <div class="table-responsive">
                                            <asp:GridView ID="TableGoodsStock" runat="server" CssClass="table table-striped row-border order-column table-bordered zero-configuration text-nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowDataBound="TableGoodsStock_RowDataBound">
                                                <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                                            <%--<asp:LinkButton runat="server" ID="btnDelete" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnDelete_Click" ToolTip="Delete"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="id" HeaderText="id" />
                                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                                    <asp:BoundField DataField="category_name" HeaderText="Category" />
                                                    <asp:BoundField DataField="stok_code" HeaderText="Stock Code" />
                                                    <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                    <asp:BoundField DataField="Item" HeaderText="Item Name" />
                                                    <asp:BoundField DataField="Merk" HeaderText="Merk" />
                                                    <asp:BoundField DataField="tipe" HeaderText="Type/Description" />
                                                    <asp:BoundField DataField="criteria_stock" HeaderText="Stock Criteria" />
                                                    <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                    <asp:BoundField DataField="minimal_stok" HeaderText="Minimum Stock" />
                                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                                    <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                    <asp:BoundField DataField="id_item" HeaderText="id_item" />
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
        <div class="modal-dialog modal-dialog-centered">
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
                                    <asp:HiddenField ID="hlbStockCode" runat="server" />
                                    <asp:HiddenField ID="hlbIdItem" runat="server" />
                                    <div class='col-sm-12' id="ddlItem" runat="server">
                                        Item Name
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlItemName" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlItemName_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12' id="TxtItem" runat="server">
                                        Item Name
										<div class="form-group">
                                            <div class='input-group'>
                                                <textarea class="form-control h-150px" rows="2" id="txtItemName" runat="server" disabled></textarea>
                                                <%--<input runat="server" id="txtItemName" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Item Name" disabled>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        Item Code
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtCode" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Code" disabled>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        Minimum Stock
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtMinimumStock" data-validate-length-range="5,15" type="number" class="form-control input-rounded" placeholder="Minimum Stock" onkeypress="return isNumberKey(event)">
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        Location
										<div class="form-group">
                                            <div class='input-group'>
                                                <asp:DropDownList ID="ddlLocation" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                    runat="server" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
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
