<%@ Page Title="Category" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="procurement_system.category" %>

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
                text: 'Data Category Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'category.aspx';
                }
            );
        }
    </script>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Data Category Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'category.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncDelete() {
            swal({
                title: 'Delete Success',
                text: 'Data Category Successfully Deleted',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'category.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FieldCategoryName() {
            swal('Save Failed!', 'Please enter category name', 'error');
        }
    </script>
    <script type="text/javascript">
        function FieldPartCode() {
            swal('Save Failed!', 'Please enter part code', 'error');
        }
    </script>
    <script type="text/javascript">
        function DuplicateCategory() {
            swal('Save Failed!', 'Category name already exist!', 'error');
        }
    </script>
    <script type="text/javascript">
        function DuplicatePartCode() {
            swal('Save Failed!', 'Part code name already exist!', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Category</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Category</li>
        </ol>
    </div>


    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="hlbID" runat="server" Text=""></asp:Label>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Categories</legend>
                                    <div class='col-sm-12'>
                                        <button type="button" onclick="<%=btnAddNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                            Add
                                                <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                        </button>
                                        <asp:Button runat="server" Style="display: none;" ID="btnAddNew" OnClick="btnAddNew_Click"></asp:Button>
                                    </div>
                                    <%--<div class="btn-group" role="group" style="float: right;">
                                        
                                    </div>--%>
                                    <div class='col-sm-12'>
                                        <asp:GridView ID="TableCategory" runat="server" CssClass="table table-striped table-bordered zero-configuration nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                            <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                                        <%--<asp:LinkButton runat="server" ID="btnDelete" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnDelete_Click" ToolTip="Delete"><i class="fa-solid fa-trash-can"></i></asp:LinkButton>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="id" HeaderText="ID" />
                                                <asp:BoundField DataField="category_name" HeaderText="Category" />
                                                <asp:BoundField DataField="part_code" HeaderText="Part Code" />
                                                <asp:BoundField DataField="active" HeaderText="Active" />
                                            </Columns>
                                        </asp:GridView>
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
    <div class="modal fade bd-example-modal-lg" id="mdlAddCategory" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
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
                                <legend class="scheduler-border">Category Detail</legend>
                                <div class="row">
                                    <div class='col-sm-12' hidden>
                                        ID
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtID" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="ID" disabled>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        Category
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtCategory" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Category Name">
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        Part Code
										<div class="form-group">
                                            <div class='input-group'>
                                                <input runat="server" id="txtPartCode" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Part Code">
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-6'>
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
                                    <div class='col-sm-6'>
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
