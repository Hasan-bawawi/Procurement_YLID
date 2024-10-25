<%@ Page Title="Create Purchase Order (PO)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="create_purchase_order_standart.aspx.cs" Inherits="procurement_system.create_purchase_order_standart" Async="true" %>

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

        .container {
            display: block;
            position: relative;
            padding-left: 35px;
            margin-bottom: 20px;
            cursor: pointer;
            font-size: 25px;
        }

            /* Hide the default checkbox */
            .container input {
                visibility: hidden;
                cursor: pointer;
            }

        /* Create a custom checkbox */
        .mark {
            position: absolute;
            top: 0;
            left: 0;
            height: 25px;
            width: 25px;
            background-color: lightgray;
        }

        .container:hover input ~ .mark {
            background-color: gray;
        }

        .container input:checked ~ .mark {
            background-color: blue;
        }

        /* Create the mark/indicator (hidden when not checked) */
        .mark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the mark when checked */
        .container input:checked ~ .mark:after {
            display: block;
        }

        /* Style the mark/indicator */
        .container .mark:after {
            left: 9px;
            top: 5px;
            width: 5px;
            height: 10px;
            border: solid white;
            border-width: 0 3px 3px 0;
            transform: rotate(45deg);
        }
    </style>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>
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
                text: 'Category Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'purchase_order.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncSave() {
            // Split the inner text into an array of items based on a newline character
            var itemsArray = '<%= lbErrorUploadNotif.InnerText.Replace(Environment.NewLine, "|") %>'.split('|');

            var listItems = ""; // Initialize an empty string for list items

            // Build the list items dynamically
            for (var i = 0; i < itemsArray.length; i++) {
                listItems += "<li>" + itemsArray[i] + "</li>";
            }

            // Construct the HTML content with the list
            var contentHTML = '<span style="color: green; display: none;"><label id="lbErrorUploadNotif" runat="server"></label></span>' +
                '<ul>' + listItems + '</ul>';
            swal({
                title: 'Save Success',
                text: 'PO Successfully Submit',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'purchase_order.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function SelectVendor() {
            swal('Can not add Item!', 'Please select Vendor first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectVendorFirst() {
            swal('Submit Failed!', 'Please select Vendor first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectIssuedDate() {
            swal('Submit Failed!', 'Please select Issued Date first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectDeliveryDate() {
            swal('Submit Failed!', 'Please select Delivery Date first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectDeliveryTo() {
            swal('Submit Failed!', 'Please enter delivery to first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectRequester() {
            swal('Submit Failed!', 'Please select requester first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectAssetType() {
            swal('Submit Failed!', 'Please select asset type first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectPaymentTerm() {
            swal('Submit Failed!', 'Please enter payment term first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectItem() {
            swal('Submit Failed!', 'Please select item first', 'error');
        }
    </script>
    <script type="text/javascript">
        function AlreadyPO() {
            swal('Select Failed!', 'This item already create PO', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">Purchase Order (PO)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="purchase_order.aspx">&nbsp;Purchase Order (PO)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Create Purchase Order (PO)</li>
        </ol>
    </div>
    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="PurchaseOrederCreated" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <asp:Image ID="imgQRCode" runat="server" />
    </div>
    <asp:HiddenField ID="lblNamaBranch" runat="server" />
    <asp:HiddenField ID="hblNIK" runat="server" />
    <asp:HiddenField ID="txtRequester" runat="server" />
    <asp:HiddenField ID="txtReqDept" runat="server" />
    <asp:HiddenField ID="txtOIDReqDept" runat="server" />
    <asp:HiddenField ID="txtPosition" runat="server" />
    <asp:HiddenField ID="txtSection" runat="server" />
    <asp:HiddenField ID="hlbGrandTotal" runat="server" />
    <asp:HiddenField ID="hlbRFNumber" runat="server" />
    <asp:HiddenField ID="hlbYears" runat="server" />
    <asp:HiddenField ID="hlbYearsNew" runat="server" />
    <asp:HiddenField ID="hlblast_numberNew" runat="server" />
    <asp:HiddenField ID="hlbCatalog" runat="server" />
    <asp:HiddenField ID="hlbIDVendor" runat="server" />
    <asp:HiddenField ID="hlbOK" runat="server" />
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12' id="divListItemRF" runat="server">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;List of Item RF -
                                        <asp:Label ID="lblRFNumber" runat="server"></asp:Label>
                                    </span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableDetailsRF" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableDetailsRF_RowCommand" OnRowDataBound="TableDetailsRF_RowDataBound" OnSelectedIndexChanged="TableDetailsRF_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Create PO">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnCreate" CommandName="Buat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnCreate_Click" ToolTip="Create PO"><i class="fa-solid fa-edit"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remark" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                            <%--<asp:BoundField DataField="price" HeaderText="Price" />--%>
                                                            <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" HtmlEncode="False" />
                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor" />
                                                            <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                            <asp:BoundField DataField="no_po" HeaderText="PO Number" />
                                                            <asp:BoundField DataField="amount" HeaderText="amount" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="rf_no" />
                                                            <asp:BoundField DataField="catalog_type" HeaderText="catalog_type" />
                                                            <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                            <asp:BoundField DataField="DivisionReq" HeaderText="DivisionRequester" />
                                                            <asp:BoundField DataField="DivisionRequester" HeaderText="OIDDivisionRequester" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divCreatePO" runat="server" visible="false">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;Create Purchase Order (Standart)</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-6' hidden>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">PO Number&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtPONumber" data-validate-length-range="5,15" type="text" class="form-control" placeholder="PO Number" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Issued Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtIssuedDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker1" placeholder="Issued Date" disabled>
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
                                                            <span class="input-group-text">Request by&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtReqBy" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Select Date" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input type="text" id="txtDeliveryDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker1" placeholder="Delivery Date">
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
                                                            <span class="input-group-text">Asset Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <asp:DropDownList ID="ddlAssetStatus" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlAssetStatus_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Enabled="true" Text="Select Asset" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Fixed Asset" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Non Fixed Asset" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Vendor Name&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtVendorName" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Vendor Name" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Payment Terms</span>
                                                        </div>
                                                        <input runat="server" id="txtPaymentTerms" data-validate-length-range="5,15" type="text" class="form-control" placeholder="Payment of terms...">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Vendor Address</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="txtAddress" runat="server" disabled></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Delivery to&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <%--<textarea class="form-control h-150px" rows="3" id="txtDeliveryTo" runat="server"></textarea>--%>
                                                        <asp:DropDownList ID="ddlDeliveryTo" class="custom-select mr-sm-2" AppendDataBoundItems="true"
                                                            runat="server" OnSelectedIndexChanged="ddlDeliveryTo_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Temas Building Lantai 3A, Jl. Yos Sudarso Kav.33, Sunter Jaya, Jakarta Utara 14350, Indonesia" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Soewarna Business Park Blok A, Lot 1-2 Soekarno-Hatta Airport, Pajang, Benda, Tengerang, Banten 15126, Indonesia" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Kawasan Industri MM2100 Blok EE-4, Desa Danau Indah, Cikarang Barat, Bekasi 17520, Indonesia" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, Ruko Permata Juanda, West Wing Super B/8-8A, Jl. Raya Juanda, Sedatiagung - Sedati, Sidoarjo 61253,  Indonesia" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="PT Yusen Logistics Indonesia, HSBC Building 4th Floor Suite 408, Jl. Gajah Mada No. 135, Pekunden, Semarang 50134,  Indonesia" Value="5"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-6'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Other Condition</span>
                                                        </div>
                                                        <textarea class="form-control h-150px" rows="3" id="txtOtherCondition" runat="server" maxlength="100" placeholder="Max. 100 character"></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12' id="divItemPO" runat="server" visible="false">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-file-text-o"></i><span class="nav-text">&nbsp;List of Item PO</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
							                    <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPO" runat="server" CssClass="table table-bordered table-striped text-nowrap verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPO_RowCommand" OnRowDataBound="TableItemPO_RowDataBound" OnSelectedIndexChanged="TableItemPO_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RF No." HeaderText="RF No." />
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code" />
                                                            <asp:BoundField DataField="Item" HeaderText="Item" />
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                            <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("Price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("Amount"))) %>' name="txtAmount" id="txtAmount" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="btnRemove" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnRemove_Click" ToolTip="Remove"><i class="fa fa-trash"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class='col-sm-8'></div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Total Amount (IDR)</span>
                                                        </div>
                                                        <input runat="server" id="txtTotalAmount" data-validate-length-range="5,15" type="text" class="form-control" placeholder="total amount..." disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-8'></div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">VAT</span>
                                                        </div>
                                                        <asp:TextBox runat="server" ID="txtVAT" data-validate-length-range="5,15" type="text" class="form-control" placeholder="vat..." onkeydown="if (event.keyCode == 13) { __doPostBack('<%=txtVAT.UniqueID %>', ''); return false; }" OnTextChanged="txtVAT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">%</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <input runat="server" id="txtVatAmount" data-validate-length-range="5,15" type="text" class="form-control" placeholder="vat amount..." disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-8'></div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="grand total..." disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-sm-8' id="divUploadFile" runat="server" visible="false">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">Doc. Upload&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        </div>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text">
                                                                <asp:FileUpload ID="FileUploadEDocs" AllowMultiple="true" runat="server" /></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hlbKodeBarang" runat="server" />
                                            <asp:HiddenField ID="hlbID" runat="server" />
                                            <div class='col-sm-4' id="divSubmit" runat="server" visible="false">
                                                <button type="button" style="float: right;" onclick="<%=btnSubmitPO.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Submit
							                    <span class="btn-icon-right"><i class="fa fa-check-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSubmitPO" OnClick="btnSubmitPO_Click" OnClientClick="ShowLoading()"></asp:Button>
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
			Modal Add Item
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlAddItem" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalAddItem.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalAddItem" OnClick="btnCloseModalAddItem_Click"></asp:Button>
                    <%--<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span>
					</button>--%>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border"><i class="fa fa-search"></i><span class="nav-text">List of requesition item don't yet have a PO number</span></legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12'>
                                            <br />
                                            <div class="table-responsive">
                                                <asp:GridView ID="TableRequesitionItem" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableRequesitionItem_RowCommand" OnRowDataBound="TableRequesitionItem_RowDataBound" OnSelectedIndexChanged="TableRequesitionItem_SelectedIndexChanged">
                                                    <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                                        <asp:BoundField DataField="nik_requester" HeaderText="nik_requester" />
                                                        <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                                        <asp:BoundField DataField="DivisionRequester" HeaderText="Division" />
                                                        <asp:BoundField DataField="SectionRequester" HeaderText="Section" />
                                                        <asp:BoundField DataField="item_code" HeaderText="item_code" />
                                                        <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                        <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                        <asp:BoundField DataField="description" HeaderText="Description" />
                                                        <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                        <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                        <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                                        <asp:BoundField DataField="remaks" HeaderText="Remark" />
                                                        <asp:BoundField DataField="status" HeaderText="RF Status" />
                                                        <asp:BoundField DataField="type_request" HeaderText="Request Type" />
                                                        <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                                        <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                                        <asp:BoundField DataField="price" HeaderText="Price" />
                                                        <%--<asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" HtmlEncode="False" />--%>
                                                        <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                        <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                        <asp:BoundField DataField="no_po" HeaderText="no_po" />
                                                        <asp:BoundField DataField="amount" HeaderText="amount" />
                                                        <%--<asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:N0}" HtmlEncode="False" />--%>
                                                        <asp:BoundField DataField="catalog_type" HeaderText="catalog_type" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <div class='col-sm-12'>
                            <button type="button" style="float: right;" onclick="<%=btnSubmitItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                Submit
                                <span class="btn-icon-right"><i class="fa fa-check-circle"></i></span>
                            </button>
                            <asp:Button runat="server" Style="display: none;" ID="btnSubmitItem" OnClick="btnSubmitItem_Click"></asp:Button>
                        </div>
                    </div>
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
</asp:Content>
