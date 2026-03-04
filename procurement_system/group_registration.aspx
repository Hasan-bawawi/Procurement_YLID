<%@ Page Title="Group Registration" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="group_registration.aspx.cs" Inherits="procurement_system.group_registration" %>

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
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Group Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncUpdateNavigation() {
            swal({
                title: 'Update Success',
                text: 'Navigation Module/Menu Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'New Group Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSaveNavigation() {
            swal({
                title: 'Save Success',
                text: 'Navigation role Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncDeleteNavigation() {
            swal({
                title: 'Remove Success',
                text: 'Navigation role Successfully Removed',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function Deletefailed() {
            swal({
                title: 'Remove Failed!',
                text: 'Please select navigation module or menu to be removed!',
                type: 'error',
                showConfirmButton: true,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function Savedfailed() {
            swal({
                title: 'Save Failed!',
                text: 'Module/Menu already exist',
                type: 'error',
                showConfirmButton: true,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function SelectModule() {
            swal({
                title: 'Save Failed!',
                text: 'please select module/menu',
                type: 'error',
                showConfirmButton: true,
                html: true,
            },
                function redirect() {
                    window.location.href = 'group_registration.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">  

        function DeleteConfirm() {
            var Ans = confirm("Do you want to Remove Selected Navigation Module/Menu?");
            if (Ans) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function ErorrEmptyGroupname() {
            swal('Save Failed!', 'Group name can not empty', 'error');
        }
    </script>
    <script type="text/javascript">
        function DuplicateGroupName() {
            swal('Save Failed!', 'Group name input already exist', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectAccess() {
            swal('Save Failed!', 'please select user access', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Group Registration</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-users-gear"></i>&nbsp;User Management</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Group Registration</li>
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
                                    <legend class="scheduler-border">List of Groups</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnNew.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    New Group
											<span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnNew" OnClick="btnNew_Click"></asp:Button>
                                            </div>

                                            <div class='col-sm-12'>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableGroup" runat="server" CssClass="table table-striped table-bordered zero-configuration text-nowrap" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableGroup_RowCommand" OnSelectedIndexChanged="TableGroup_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <%--<asp:CommandField ShowSelectButton="True" buttontype="Image" SelectImageUrl="~/images/edit.png" SelectText="Edit" ControlStyle-ForeColor="Blue"/>--%>
                                                            <asp:TemplateField HeaderText="Edit">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa fa-edit"></i> Edit</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Navigation">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnShowNavigation" CommandName="Tampil" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnShowNavigation_Click" ToolTip="Show"><i class="fa fa-search"></i> Show Navigation</asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Oid" HeaderText="Oid" />
                                                            <asp:BoundField DataField="Code" HeaderText="Code" />
                                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
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
			Modal New Group
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlAddNewGroup" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel1">Detail Group</h4>
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <asp:HiddenField ID="hlbCode" runat="server" />
                            <asp:HiddenField ID="hlbOid" runat="server" />
                            <asp:HiddenField ID="hlbOidNavigation" runat="server" />
                            <asp:HiddenField ID="hlbOidModule" runat="server" />
                            <div class='col-sm-12'>
                                Name of Group
					<div class="form-group">
                        <div class='input-group'>
                            <input runat="server" id="txtGroupName" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Name of Group">
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

    <!--**********************************
			Modal New Navigation Role
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlAddNewNavigationRole" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabela1">Navigation Role</h4>
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew_Navigation" OnClick="btnCloseModalNew_Navigation_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                Name of Group
					<div class="form-group">
                        <div class='input-group'>
                            <input runat="server" id="txtGroupName_Navigation" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type Name of Group" disabled>
                        </div>
                    </div>
                            </div>
                            <div class='col-sm-9'></div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Add/Edit Role Navigation</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-3'>
                                                Module Name
									<div class="form-group">
                                        <div class='input-group'>
                                            <asp:DropDownList ID="ddlModuleName" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                runat="server" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                Navigation Role
								<div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:CheckBox runat="server" ID="ckActiveNavigationRole" />
                                        </div>
                                    </div>
                                    <input runat="server" id="Text1" type="text" class="form-control" value="ACTIVE" disabled>
                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                Status
								<div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:CheckBox runat="server" ID="ckActive_Navigation" />
                                        </div>
                                    </div>
                                    <input runat="server" id="Text2" type="text" class="form-control" value="ACTIVE" disabled>
                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                &nbsp;
									<div class="form-group">
                                        <div class='input-group'>
                                            <asp:Button runat="server" ID="btnSubmit_Navigation" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Navigation_Click"></asp:Button>
                                            <asp:Button runat="server" ID="btnUpdate_Navigation" CssClass="btn buttonColor" Text="Update" OnClick="btnUpdate_Navigation_Click"></asp:Button>
                                        </div>
                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <button type="button" onclick="<%=btnRemove_navigation.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Remove
					<span class="btn-icon-right"><i class="fa fa-remove"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnRemove_navigation" OnClick="btnRemove_navigation_Click"></asp:Button>
                                <div class="table-responsive">
                                    <asp:GridView ID="TableRoleNavigation" runat="server" CssClass="table table-striped table-bordered zero-configuration text-nowrap" AutoGenerateColumns="False" Style="width: 100%"
                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableRoleNavigation_RowCommand" OnSelectedIndexChanged="TableRoleNavigation_SelectedIndexChanged">
                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Oid" HeaderText="Oid" />
                                            <asp:BoundField DataField="GroupName" HeaderText="GroupName" />
                                            <asp:BoundField DataField="ModuleName" HeaderText="ModuleName" />
                                            <asp:BoundField DataField="Type" HeaderText="Type" />
                                            <asp:BoundField DataField="RoleNavigation" HeaderText="Role Navigation" />
                                            <asp:BoundField DataField="IsActive" HeaderText="IsActive" />
                                            <asp:TemplateField HeaderText="Tools">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="btnEditModule" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEditModule_Click" ToolTip="Edit"><i class="fa fa-edit"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn buttonColor" data-dismiss="modal">Close
			<span class="btn-icon-right"><i class="fa fa-remove"></i></span></button>	  --%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
