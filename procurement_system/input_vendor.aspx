<%@ Page Title="Detail Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="input_vendor.aspx.cs" Inherits="procurement_system.input_vendor" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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


    <style>
        .wrapper {
            width: 330px;
            font-family: 'Helvetica';
            font-size: 14px;
        }

        .StepProgress {
            position: relative;
            padding-left: 45px;
            list-style: none;
        }

            .StepProgress::before {
                display: inline-block;
                content: '';
                position: absolute;
                top: 0;
                left: 15px;
                width: 10px;
                height: 100%;
            }

        .StepProgress-item {
            position: relative;
            counter-increment: list;
        }

            .StepProgress-item:not(:last-child) {
                padding-bottom: 20px;
            }

            .StepProgress-item::before {
                display: inline-block;
                content: '';
                position: absolute;
                left: -30px;
                height: 100%;
                width: 10px;
            }

            .StepProgress-item::after {
                content: '';
                display: inline-block;
                position: absolute;
                top: 0;
                left: -37px;
                width: 20px;
                height: 20px;
                border: 2px solid #CCC;
                border-radius: 50%;
                background-color: #FFF;
            }

            .StepProgress-item.is-done::before {
                border-left: 2px solid green;
            }

            .StepProgress-item.is-done::after {
                content: "✔";
                font-size: 13px;
                color: #FFF;
                text-align: center;
                border: 2px solid green;
                background-color: green;
            }

            .StepProgress-item.is-reject::before {
                border-left: 2px solid red;
            }

            .StepProgress-item.is-reject::after {
                content: "✗";
                font-size: 13px;
                color: #FFF;
                text-align: center;
                border: 2px solid red;
                background-color: red;
            }

            .StepProgress-item.current::before {
                border-left: 2px solid green;
            }

            .StepProgress-item.current::after {
                content: counter(list);
                padding-top: 1px;
                width: 25px;
                height: 25px;
                top: -4px;
                left: -40px;
                font-size: 14px;
                text-align: center;
                color: green;
                border: 2px solid green;
                background-color: white;
            }

        .StepProgress strong {
            display: block;
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
        function DeleteSuccess() {
            swal('Delete Success!', 'Item successfully deleted', 'success');
        }
    </script>

    <script type="text/javascript">
        function AddItemsSuccess() {
            swal('Save Success!', 'Item successfully Submited', 'success');
        }
    </script>
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
                title: 'Successfully Submit',
                text: 'This RF can now be made into a PO',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true,
            },
                function redirect() {
                    var RFNumber = document.getElementById('<%=lbRFNumberHeader.ClientID %>').innerText;
                    window.location.href = 'create_purchase_order_standart.aspx?rf_no=' + RFNumber;
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
            <h1 class="page-header text-overflow" style="color: white;">Detail Requisition Form (RF)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Order</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="purchase_order.aspx">&nbsp;Purchase Order (PO)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Detail Requisition Form (RF) - 
            <asp:Label runat="server" ID="lbRFNumberBreadcrumb"></asp:Label></li>
        </ol>
    </div>


    <asp:HiddenField ID="hlbCodeItem" runat="server" />
    <asp:HiddenField ID="hlbItem" runat="server" />
    <asp:HiddenField ID="hlbMerk" runat="server" />
    <asp:HiddenField ID="hlbType" runat="server" />
    <asp:HiddenField ID="hblEmailRequester" runat="server" />
    <asp:HiddenField ID="hblEmailManager" runat="server" />
    <asp:HiddenField ID="hblEmailGM" runat="server" />
    <asp:HiddenField ID="hlbEmailManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbEmailGMAdm" runat="server" />
    <asp:HiddenField ID="hlbNIKManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbNIKGMAdm" runat="server" />
    <asp:HiddenField ID="hlbManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbGMAdm" runat="server" />

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
                            <div class='col-sm-6'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-circle-info"></i><span class="nav-text">&nbsp;RF Status</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12' id="divRFStatus" runat="server">
                                                <div style="margin-left: 10px; height: 300px; overflow: auto;">
                                                    <div class="wrapper">
                                                        <ul class="StepProgress">
                                                            <li id="rf_created" runat="server" class="StepProgress-item is-done"><strong>RF Created</strong><asp:Label ID="lbDateCreateRF" runat="server"></asp:Label></li>
                                                            <li id="price_estimated" runat="server" class="StepProgress-item current"><strong>Price Estimate</strong><asp:Label ID="lbDatePriceEstimate" runat="server"></asp:Label></li>
                                                            <li id="manager" runat="server" class="StepProgress-item current"><strong>Div. Manager</strong><asp:Label ID="lbDateMgr" runat="server"></asp:Label></li>
                                                            <li id="gm" runat="server" class="StepProgress-item current"><strong>Div. GM</strong><asp:Label ID="lbDateGM" runat="server"></asp:Label></li>
                                                            <li id="deputy_director" runat="server" class="StepProgress-item current"><strong>Deputy Director</strong><asp:Label ID="lbDateDepDir" runat="server"></asp:Label></li>
                                                            <li id="director" runat="server" class="StepProgress-item current"><strong>Director</strong><asp:Label ID="lbDateDir" runat="server"></asp:Label></li>
                                                            <%--<li id="it_section_head" runat="server" class="StepProgress-item current"><strong>IT Head</strong><asp:Label ID="lbDateITHead" runat="server"></asp:Label></li>
                                                            <li id="ga_section_head" runat="server" class="StepProgress-item current"><strong>GA Head</strong><asp:Label ID="lbDateGAHead" runat="server"></asp:Label></li>
                                                            <li id="admin_gm" runat="server" class="StepProgress-item current"><strong>Admin GM</strong><asp:Label ID="lbDateGMAdm" runat="server"></asp:Label></li>
                                                            <li id="admin_director" runat="server" class="StepProgress-item current"><strong>Admin Director</strong><asp:Label ID="lbDateDirAdm" runat="server"></asp:Label></li>--%>
                                                            <li id="status_completed" runat="server" class="StepProgress-item current"><strong>Completed</strong><asp:Label ID="lbDateComplete" runat="server"></asp:Label></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-6'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">
                                        <i class="fa-solid fa-circle-info"></i>
                                        <span class="nav-text">&nbsp;RF Detail
                                        </span>
                                    </legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div style="margin-left: 10px; height: 300px;">
                                                    <div class="row">
                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>RF Number</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbRFNumberHeader"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Request Date</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbRequestDate"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div hidden>
                                                            <div class='col-sm-4'>
                                                                <div class="form-group">
                                                                    <strong>
                                                                        <label>Approved by</label></strong>
                                                                </div>
                                                            </div>
                                                            <div class='col-sm-8'>
                                                                <div class="form-group">
                                                                    :&nbsp;<asp:Label runat="server" ID="lbApprovedBy"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class='col-sm-6'>
                                                                <div class="form-group">
                                                                    <label>Acknowledge by</label>
                                                                </div>
                                                            </div>
                                                            <div class='col-sm-6'>
                                                                <div class="form-group">
                                                                    :&nbsp;<asp:Label runat="server" ID="lbAcknowledgeBy"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Request by</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbRequester"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Division/Section</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbDivision" Visible="false"></asp:Label><asp:Label runat="server" ID="lbDivisionReq" Visible="true"></asp:Label>
                                                                <asp:Label runat="server" ID="Label1">/</asp:Label>
                                                                <asp:Label runat="server" ID="lbSection"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Location</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbLocation"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-12'>
                                                            <button type="button" onclick="<%=btnDownloadRF.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                                Download Form
                                                    <span class="btn-icon-right"><i class="fa fa-download"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnDownloadRF" OnClick="btnDownloadRF_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-file-lines"></i><span class="nav-text">&nbsp;Item Request Details -
                                    <asp:Label runat="server" ID="lbCatalogType"></asp:Label>&nbsp;Catalogs
                                    </span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableDetailsRF" runat="server" CssClass="table table-striped row-border order-column table-bordered text-nowrap zero-configuration grid" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableDetailsRF_RowCommand" OnRowDataBound="TableDetailsRF_RowDataBound" OnSelectedIndexChanged="TableDetailsRF_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:BoundField DataField="item_code" HeaderText="Item Code" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remark" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" placeholder="enter fixed price" disabled/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList class="input-group-text" ID="ddlVendor" data-width="100%" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false" runat="server" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="vendor_name" HeaderText="Vendor" />
                                                            <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                            <asp:BoundField DataField="no_po" HeaderText="PO Number" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <button type="button" style="float: right;" onclick="<%=btnSubmit.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Submit
                                <span class="btn-icon-right"><i class="fa fa-save"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click" OnClientClick="ShowLoading()"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $("input[data-type='currency']").on({
            keyup: function () {
                formatCurrency($(this));
            },
            blur: function () {
                formatCurrency($(this), "blur");
            }
        });


        function formatNumber(n) {
            // format number 1000000 to 1,234,567
            return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        }


        function formatCurrency(input, blur) {
            // appends $ to value, validates decimal side
            // and puts cursor back in right position.

            // get input value
            var input_val = input.val();

            // don't validate empty input
            if (input_val === "") { return; }

            // original length
            var original_len = input_val.length;

            // initial caret position 
            var caret_pos = input.prop("selectionStart");

            // check for decimal
            if (input_val.indexOf(".") >= 0) {

                // get position of first decimal
                // this prevents multiple decimals from
                // being entered
                var decimal_pos = input_val.indexOf(".");

                // split number by decimal point
                var left_side = input_val.substring(0, decimal_pos);
                var right_side = input_val.substring(decimal_pos);

                // add commas to left side of number
                left_side = formatNumber(left_side);

                // validate right side
                right_side = formatNumber(right_side);

                // On blur make sure 2 numbers after decimal
                if (blur === "blur") {
                    right_side += "00";
                }

                // Limit decimal to only 2 digits
                right_side = right_side.substring(0, 2);

                // join number by .
                input_val = "" + left_side + "." + right_side;

            } else {
                // no decimal entered
                // add commas to number
                // remove all non-digits
                input_val = formatNumber(input_val);
                input_val = "" + input_val;

                // final formatting
                if (blur === "blur") {
                    input_val += ".00";
                }
            }

            // send updated string to input
            input.val(input_val);

            // put caret back in the right position
            var updated_len = input_val.length;
            caret_pos = updated_len - original_len + caret_pos;
            input[0].setSelectionRange(caret_pos, caret_pos);
        }
    </script>

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>
</asp:Content>
