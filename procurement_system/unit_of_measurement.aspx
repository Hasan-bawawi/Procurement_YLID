<%@ Page Title="Unit of Measurement" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="unit_of_measurement.aspx.cs" Inherits="procurement_system.unit_of_measurement" %>

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
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Unit Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'unit_of_measurement.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Unit Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'unit_of_measurement.aspx';
                }
            );
        }
    </script>

    <script type="text/javascript">
        function DuplicateDataCategoryName() {
            swal('Save Failed!', 'Category name already exist!', 'error');
        }
    </script>
    <script type="text/javascript">
        function EmptyFieldCategoryName() {
            swal('Save Failed!', 'Please enter category name!', 'error');
        }
    </script>

    <script type="text/javascript">
        function DuplicateDataUnitName() {
            swal('Save Failed!', 'Unit name already exist!', 'error');
        }
    </script>
    <script type="text/javascript">
        function EmptyFieldUnitName() {
            swal('Save Failed!', 'Please enter unit name!', 'error');
        }
    </script>
    <script type="text/javascript">
        function EmptyFieldUnitConversion() {
            swal('Save Failed!', 'Please enter unit conversion!', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Unit of Measurement</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Unit of Measurement</li>
        </ol>
    </div>


    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Units</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnAdd.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add
											    <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAdd" OnClick="btnAdd_Click"></asp:Button>
                                            </div>

                                            <div class='col-sm-12'>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableUnit" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableUnit_RowCommand" OnRowDataBound="TableUnit_RowDataBound" OnSelectedIndexChanged="TableUnit_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <%--<asp:CommandField ShowSelectButton="True" buttontype="Image" SelectImageUrl="~/images/edit.png" SelectText="Edit" ControlStyle-ForeColor="Blue"/>--%>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="Unit Name" />
                                                            <asp:BoundField DataField="active" HeaderText="Active" />
                                                            <asp:BoundField DataField="createby" HeaderText="Created by" />
                                                            <asp:BoundField DataField="create_date" HeaderText="Creation by" />
                                                            <asp:BoundField DataField="modifiedby" HeaderText="Modified by" />
                                                            <asp:BoundField DataField="modified_date" HeaderText="Modification by" />
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
			Modal New
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlNew" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Detail Units</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                Unit Name
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtUnitName" data-validate-length-range="5,15" type="text" class="form-control input-rounded">
                                                </div>
                                            </div>
                                            </div>
                                            <div class='col-sm-12'>
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
                                            <div class='col-sm-12'>
                                                Created by
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtCreateByNew" data-validate-length-range="5,15" type="text" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                Creation Date
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input type="text" id="txtCreateDateNew" data-validate-length-range="5,15" runat="server" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                Modified by
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtModifiedByNew" data-validate-length-range="5,15" type="text" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                            </div>
                                            <div class='col-sm-12'>
                                                Modification Date
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input type="text" id="txtModifiedDateNew" data-validate-length-range="5,15" runat="server" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                            </div>
                                            <asp:HiddenField ID="hlbID" runat="server" />
                                            <div class='col-sm-12'>
                                                &nbsp;
											<div class="form-group">
                                                <div class='input-group'>
                                                    <asp:Button runat="server" Visible="false" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                                    <asp:Button runat="server" Visible="false" ID="btnUpdate" CssClass="btn buttonColor" Text="Update" OnClick="btnUpdate_Click"></asp:Button>
                                                </div>
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
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
</asp:Content>
