<%@ Page Title="Price Input" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="input_price.aspx.cs" Inherits="procurement_system.input_price" Async="true" %>

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
    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.min.js"></script>
    <script src="plugins/tables/js/datatable/dataTables.fixedColumns.js"></script>

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
    <link href="vendors/sweetalert.css" rel="stylesheet" />
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
                title: 'Save Success',
                text: 'Form Successfully Submit',
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
            <h1 class="page-header text-overflow" style="color: white;">Price Input</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Order</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="requisition_price_check.aspx">&nbsp;RF Price Check</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Detail Requisition Form (RF) - 
        <asp:Label runat="server" ID="lbRFNumberBreadcrumb"></asp:Label></li>
        </ol>
    </div>

    <asp:HiddenField ID="hlbCodeItem" runat="server" />
    <asp:HiddenField ID="hlbItem" runat="server" />
    <asp:HiddenField ID="hlbMerk" runat="server" />
    <asp:HiddenField ID="hlbType" runat="server" />
    <asp:HiddenField ID="lbApprovedBy" runat="server" />
    <asp:HiddenField ID="hlbEmailRequester" runat="server" />
    <asp:HiddenField ID="hlbAdmGM" runat="server" />
    <asp:HiddenField ID="hlbEmailAdmGM" runat="server" />
    <asp:HiddenField ID="hlbEmailMgrApprover" runat="server" />
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
                                    <legend class="scheduler-border"><i class="fa fa-file-text-o"></i><span class="nav-text">Requesition Form (RF) -
                                        <asp:Label runat="server" ID="lbRFNumberHeader"></asp:Label></span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Request Date</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbRequestDate"></asp:Label>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Division/Section</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbDivision" Visible="false"></asp:Label><asp:Label runat="server" ID="lbDivisionReq" Visible="true"></asp:Label>
                                                    <asp:Label runat="server" ID="Label1">/</asp:Label>
                                                    <asp:Label runat="server" ID="lbSection"></asp:Label>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Request by</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbRequester"></asp:Label>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Location</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbLocation"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">Request Details</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPurchase_RowCommand" OnRowDataBound="TableItemPurchase_RowDataBound" OnSelectedIndexChanged="TableItemPurchase_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="stok_code" HeaderText="Code Stock" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code Item" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                            <asp:BoundField DataField="description" HeaderText="Description" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
                                                            <asp:TemplateField HeaderText="@ Price Input">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" placeholder="enter price per @" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-8'></div>
                            <div class='col-sm-4'>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-append">
                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                        </div>
                                        <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="0" disabled>
                                    </div>
                                </div>
                            </div>
                            <div class='col-sm-8'></div>
                            <div class='col-sm-4' id="divClearPrice" runat="server" visible="false">
                                <button type="button" style="float: right;" onclick="<%=btnClearPrice.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Clear Price
                                <span class="btn-icon-right"><i class="fa fa-close"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnClearPrice" OnClick="btnClearPrice_Click"></asp:Button>
                            </div>
                            <div class='col-sm-4' id="divCheck" runat="server">
                                <button type="button" style="float: right;" onclick="<%=btnCheck.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Price Calculate
                                <span class="btn-icon-right"><i class="fa fa-calculator"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnCheck" OnClick="btnCheck_Click"></asp:Button>
                            </div>
                            <div class='col-sm-12' id="divSubmit" runat="server" visible="false">
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
                    input_val += "";
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
