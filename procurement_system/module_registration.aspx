<%@ Page Title="Module Registration" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="module_registration.aspx.cs" Inherits="procurement_system.module_registration" %>

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
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Module Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'module_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'New Module Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'module_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function ErorrEmptyModulename() {
            swal('Save Failed!', 'Module name can not empty', 'error');
        }
    </script>
    <script type="text/javascript">
        function ErorrEmptyModulID() {
            swal('Save Failed!', 'Module ID can not empty', 'error');
        }
    </script>
    <script type="text/javascript">
        function ErorrEmptyType() {
            swal('Save Failed!', 'Please select type', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectAccess() {
            swal('Save Failed!', 'please select user access', 'error');
        }
    </script>
    <script type="text/javascript">
        function passwordfield() {
            swal('Save Failed!', 'please field the password', 'error');
        }
    </script>
    <script type="text/javascript">
        function Confirmpasswordfield() {
            swal('Save Failed!', 'please field the confirmation password', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Module Registration</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-users-gear"></i>&nbsp;User Management</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Module Registration</li>
        </ol>
    </div>

    <asp:HiddenField ID="hlbID" runat="server" />
    <asp:HiddenField ID="lbNIK" runat="server" />
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Module</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    New
											<span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnNew" OnClick="btnNew_Click"></asp:Button>
                                            </div>

                                            <div class='col-sm-12'>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableModule" runat="server" CssClass="table table-striped table-bordered zero-configuration text-nowrap" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableModule_RowCommand" OnSelectedIndexChanged="TableModule_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <%--<asp:CommandField ShowSelectButton="True" buttontype="Image" SelectImageUrl="~/images/edit.png" SelectText="Edit" ControlStyle-ForeColor="Blue"/>--%>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnView" CommandName="Tampil" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnView_Click" ToolTip="Views"><i class="fa fa-search"></i> Views</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Oid" HeaderText="Oid" />
                                                            <asp:BoundField DataField="ModuleName" HeaderText="ModuleName" />
                                                            <asp:BoundField DataField="ModuleNameID" HeaderText="ModuleNameID" />
                                                            <asp:BoundField DataField="Type" HeaderText="Type" />
                                                            <asp:BoundField DataField="IsActive" HeaderText="IsActive" />
                                                            <asp:BoundField DataField="CreateBy" HeaderText="Created by" />
                                                            <asp:BoundField DataField="CreateDate" HeaderText="Creation Date" />
                                                            <asp:BoundField DataField="ModifiedBy" HeaderText="Modified by" />
                                                            <asp:BoundField DataField="ModifiedDate" HeaderText="Modification Date" />
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
			Modal New Module
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlAddNewModule" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel1">Detail Module</h4>
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <asp:HiddenField ID="hlbCode" runat="server" />
                            <asp:HiddenField ID="hlbOid" runat="server" />
                            <div class='col-sm-12'>
                                Name of Module
					<div class="form-group">
                        <div class='input-group'>
                            <input runat="server" id="txtModuleName" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Name of Module">
                        </div>
                    </div>
                            </div>
                            <div class='col-sm-12'>
                                Module ID
					<div class="form-group">
                        <div class='input-group'>
                            <input runat="server" id="txtModuleID" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Module ID">
                        </div>
                    </div>
                            </div>
                            <div class='col-sm-12'>
                                Type
					<div class="form-group">
                        <div class='input-group'>
                            <asp:DropDownList ID="ddlType" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                <asp:ListItem Enabled="true" Text="<Select Type>" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Menu" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Module" Value="2"></asp:ListItem>
                            </asp:DropDownList>
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
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnUpdate" CssClass="btn buttonColor" Text="Update" OnClick="btnUpdate_Click"></asp:Button>
                    <%--<button type="button" class="btn buttonColor" data-dismiss="modal">Close
			<span class="btn-icon-right"><i class="fa fa-remove"></i></span></button>	  --%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
