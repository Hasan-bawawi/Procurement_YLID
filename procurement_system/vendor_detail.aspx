<%@ Page Title="Vendor Details" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="vendor_detail.aspx.cs" Inherits="procurement_system.vendor_detail" %>

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
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Vendor Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'vendor_detail.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Vendor Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'vendor_detail.aspx';
                }
            );
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
                    window.location.href = 'vendor_detail.aspx';
                }
            );
        }
    </script>

    <script type="text/javascript">
        function DuplicateDataVendorName() {
            swal('Save Failed!', 'Vendor name already exist!', 'error');
        }
    </script>
    <script type="text/javascript">
        function EmptyFieldCategoryName() {
            swal('Save Failed!', 'Please enter category name!', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Vendor Details</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Vendor</li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Vendor Details</li>
        </ol>
    </div>
    <asp:HiddenField ID="hlblast_numberNew" runat="server" />

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Vendor</legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" onclick="<%=btnAdd.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add
											    <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAdd" OnClick="btnAdd_Click"></asp:Button>

                                                <button type="button" onclick="<%=btnImport.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Import Excel
											    <span class="btn-icon-right"><i class="fa fa-upload"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnImport" OnClick="btnImport_Click"></asp:Button>
                                                Excel File --><asp:LinkButton runat="server" OnClick="DownloadTemplete_Click"> Download Template Here</asp:LinkButton>
                                            </div>

                                            <div class='col-sm-12'>
                                                <br />
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableVendor" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableVendor_RowCommand" OnRowDataBound="TableVendor_RowDataBound" OnSelectedIndexChanged="TableVendor_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <%--<asp:CommandField ShowSelectButton="True" buttontype="Image" SelectImageUrl="~/images/edit.png" SelectText="Edit" ControlStyle-ForeColor="Blue"/>--%>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa fa-edit"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="id_category" HeaderText="id_category" />
                                                            <asp:BoundField DataField="Code" HeaderText="Code" />
                                                            <asp:BoundField DataField="Category" HeaderText="Category" />
                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor Name" />
                                                            <asp:BoundField DataField="sales_pic_name" HeaderText="PIC Name(Sales)" />
                                                            <asp:BoundField DataField="sales_phone_number" HeaderText="Phone Number(Sales)" />
                                                            <asp:BoundField DataField="sales_email" HeaderText="Email(Sales)" />
                                                            <asp:BoundField DataField="invoice_pic_name" HeaderText="PIC Name(Invoice)" />
                                                            <asp:BoundField DataField="invoice_phone_number" HeaderText="Phone Number(Invoice)" />
                                                            <asp:BoundField DataField="invoice_email" HeaderText="Email(Invoice)" />
                                                            <asp:BoundField DataField="address" HeaderText="Address" />
                                                            <asp:BoundField DataField="t_o_p" HeaderText="TOP" />
                                                            <asp:BoundField DataField="pkp_nonpkp" HeaderText="PKP/Non PKP" />
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
    <div class="modal fade bs-example-modal-lg" id="mdlNewVendor" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
					</button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">Vendor Details</legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-3' hidden>
                                            Code
				                            <div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtCode" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Code" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-9' hidden></div>
                                        <div class='col-sm-3'>
                                            Categories
											<div class="form-group">
                                                <div class='input-group'>
                                                    <asp:DropDownList ID="ddlCategory" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                        runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Vendor Name
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtVendorName" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type vendor name...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-6'></div>
                                        <div class='col-sm-3'>
                                            PIC Name (Sales)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtPICNameSales" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type PIC name...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Phone Number (Sales)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtPhoneNumberSales" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type phone number...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Email (Sales)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtEmailSales" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type email...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'></div>
                                        <div class='col-sm-3'>
                                            PIC Name (Invoice)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtPICNameInvoice" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type PIC name...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Phone Number (Invoice)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtPhoneNumberInvoice" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type phone number...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Email (Invoice)
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtEmailInvoice" data-validate-length-range="5,15" type="text" class="form-control input-rounded" placeholder="Type email...">
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'></div>
                                        <div class='col-sm-3'>
                                            Address
											<div class="form-group">
                                                <div class='input-group'>
                                                    <textarea class="form-control h-150px" rows="2" id="txtAddress" runat="server"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            TOP
											<div class="form-group">
                                                <div class='input-group'>
                                                    <asp:DropDownList ID="ddlTOP" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                        runat="server" OnSelectedIndexChanged="ddlTOP_SelectedIndexChanged">
                                                        <asp:ListItem Enabled="true" Text="<Select TOP>" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="0" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="14" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="15" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="30" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="COD" Value="6"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            PKP/Non PKP
											<div class="form-group">
                                                <div class='input-group'>
                                                    <asp:DropDownList ID="ddlPKP_NonPKP" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true"
                                                        runat="server" OnSelectedIndexChanged="ddlPKP_NonPKP_SelectedIndexChanged">
                                                        <asp:ListItem Enabled="true" Text="<Select PKP or Non PKP>" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="PKP" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Non PKP" Value="2"></asp:ListItem>
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
                                        <asp:HiddenField ID="hlbID" runat="server" />
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
                                </div>
                            </fieldset>
                        </div>
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">Reff Records</legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-3'>
                                            Created by
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtCreateByNew" data-validate-length-range="5,15" type="text" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Creation Date
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input type="text" id="txtCreateDateNew" data-validate-length-range="5,15" runat="server" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Modified by
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input runat="server" id="txtModifiedByNew" data-validate-length-range="5,15" type="text" class="form-control input-rounded" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-3'>
                                            Modification Date
											<div class="form-group">
                                                <div class='input-group'>
                                                    <input type="text" id="txtModifiedDateNew" data-validate-length-range="5,15" runat="server" class="form-control input-rounded" disabled>
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
        </div>
    </div>

    <!--**********************************
			Modal Import Excel
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlImportExcel" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel112a">Import File Excel</h4>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
			</button>--%>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                Upload File
					<div class="form-group">
                        <div class='input-group'>
                            <asp:FileUpload ID="FileUpload" AllowMultiple="false" runat="server" />
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                    </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" onclick="<%=btnUploadFile.ClientID %>.click()" class="btn mb-1 buttonColor">
                        Upload
			<span class="btn-icon-right"><i class="fa fa-upload"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnUploadFile" OnClick="btnUploadFile_Click" OnClientClick="ShowLoading()"></asp:Button>
                    <button type="button" onclick="<%=btnCloseModalImport.ClientID %>.click()" class="btn mb-1 buttonColor">
                        Close
			<span class="btn-icon-right"><i class="fa fa-remove"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalImport" OnClick="btnCloseModalImport_Click"></asp:Button>
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
</asp:Content>
