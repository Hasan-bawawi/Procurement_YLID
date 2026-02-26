<%@ Page Title="Detail Purchase Order" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="detail_purchase_order_standart.aspx.cs" Inherits="procurement_system.detail_purchase_order_standart" Async="true" %>

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

       .btn-match {
           height: 41px;           /* ikut Change Approver */
           padding-top: 0;
           padding-bottom: 0;
           line-height: 41px;      /* kunci tinggi */
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


        function confirmCancelPO() {
            debugger
            swal({
                title: 'Are you sure?',
                text: `
            <div style="margin-top:10px;">
                Do you want to cancel this PO?<br/><br/>
                <textarea id="cancelReason"
                    placeholder="Enter cancel reason..."
                    style="width:100%; height:80px; padding:8px; border:1px solid #ccc; border-radius:4px;"></textarea>
                <div id="reasonError" style="color:red; display:none; margin-top:6px; font-size:13px;">
                    Cancel reason is required
                </div>
            </div>
        `,
                type: 'warning',
                html: true,
                showCancelButton: true,
                confirmButtonText: 'Yes, Cancel it!',
                cancelButtonText: 'No',
                confirmButtonColor: '#d33',
                closeOnConfirm: false
            }, function (isConfirm) {

                if (!isConfirm) return;

                var txt = document.getElementById("cancelReason");
                var error = document.getElementById("reasonError");
                var reason = txt.value.trim();

                if (reason === "") {
                    error.style.display = "block";
                    txt.style.border = "2px solid red";
                    txt.focus();
                    return false;
                }

                // Simpan ke hidden field
                var reason = document.getElementById("cancelReason").value.trim();

                var hidden = document.querySelector("input[id$='hfCancelReason']");
                hidden.value = reason;

                swal.close();

                setTimeout(function () {
                    ShowLoading();
                    document.getElementById('<%= btnCancelForm.ClientID %>').click();
                }, 200);
                    });

                    setTimeout(function () {
                        var txt = document.getElementById("cancelReason");
                        if (txt) {
                            txt.addEventListener("input", function () {
                                this.style.border = "1px solid #ccc";
                                document.getElementById("reasonError").style.display = "none";
                            });
                        }
                    }, 300);

                    return false;
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
            <h1 class="page-header text-overflow" style="color: white;">Detail Purchase Order (PO)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="purchase_order.aspx">&nbsp;Purchase Order (PO)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Detail Purchase Order (PO) - 
            <asp:Label runat="server" ID="lbPONumberBreadcrumb"></asp:Label></li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <asp:Image ID="imgQRCode" runat="server" />
    </div>
    <asp:HiddenField ID="lblNamaBranch" runat="server" />
    <asp:HiddenField ID="hblNIK" runat="server" />
    <asp:HiddenField ID="txtRequester" runat="server" />
    <asp:HiddenField ID="txtPosition" runat="server" />
    <asp:HiddenField ID="txtSection" runat="server" />
    <asp:HiddenField ID="hlbGrandTotal" runat="server" />
    <asp:HiddenField ID="hlbRFNumber" runat="server" />
    <asp:HiddenField ID="hlbYears" runat="server" />
    <asp:HiddenField ID="hlbYearsNew" runat="server" />
    <asp:HiddenField ID="hlblast_numberNew" runat="server" />
    <asp:HiddenField ID="hlbCatalog" runat="server" />
    <asp:HiddenField ID="hlbIDVendor" runat="server" />
    <asp:HiddenField ID="hlbGRNo" runat="server" />
    <asp:HiddenField ID="hfCancelReason" runat="server" />
    <asp:HiddenField ID="txtIssuedDate" runat="server" />
    <asp:HiddenField ID="lblnikhead" runat="server" />
    <asp:HiddenField ID ="lbEmailHead" runat="server" />


    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-6'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border"><i class="fa-solid fa-circle-info"></i><span class="nav-text">&nbsp;PO Status</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12' id="divPOStatus" runat="server">
                                                <div style="margin-left: 10px; height: 300px; overflow: auto;">
                                                    <div class="wrapper">
                                                        <ul class="StepProgress">
                                                            <li id="po_created" runat="server" class="StepProgress-item is-done"><strong>PO Created</strong><asp:Label ID="lbDateCreatePO" runat="server"></asp:Label></li>
                                                            <li id="it_section_head" runat="server" class="StepProgress-item current"><strong>IT Head</strong><asp:Label ID="lbDateITHead" runat="server"></asp:Label></li>
                                                            <li id="ga_section_head" runat="server" class="StepProgress-item current"><strong>GA Head</strong><asp:Label ID="lbDateGAHead" runat="server"></asp:Label></li>
                                                            <li id="admin_gm" runat="server" class="StepProgress-item current"><strong>GM Admin</strong><asp:Label ID="lbDateGMAdm" runat="server"></asp:Label></li>
                                                            <li id="admin_director" runat="server" class="StepProgress-item current"><strong>Presdir/Director</strong><asp:Label ID="lbDateDirAdm" runat="server"></asp:Label></li>
                                                            <li id="po_onprocess" runat="server" class="StepProgress-item current"><strong>On Process</strong><asp:Label ID="lbDatePO" runat="server"></asp:Label></li>
                                                            <li id="good_receipt" runat="server" class="StepProgress-item current"><strong>Goods Receipt</strong><asp:Label ID="lbDateGR" runat="server"></asp:Label></li>
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
                                        <span class="nav-text">&nbsp;PO Detail
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
                                                                    <label>PO Number</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbPONumberHeader"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Issued Date</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbIssuedDate"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div hidden>
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
                                                            <div class='col-sm-6'>
                                                                <div class="form-group">
                                                                    <label>Delivery Date</label>
                                                                </div>
                                                            </div>
                                                            <div class='col-sm-6'>
                                                                <div class="form-group">
                                                                    :&nbsp;<asp:Label runat="server" ID="lbDeliveryDate"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Asset Type</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbAssetType"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Vendor Name</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbVendorName"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-4'>
                                                            <div class="form-group">
                                                                <strong>
                                                                    <label>Payment Terms</label></strong>
                                                            </div>
                                                        </div>
                                                        <div class='col-sm-8'>
                                                            <div class="form-group">
                                                                :&nbsp;<asp:Label runat="server" ID="lbPaymentTerms"></asp:Label>
                                                            </div>
                                                        </div>
                                                     <%--   <div class='col-sm-6'>
                                                            <button type="button" onclick="<%=btnDownloadPO.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                                Download Form
                                                                <span class="btn-icon-right"><i class="fa fa-download"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnDownloadPO" OnClick="btnDownloadPO_Click"></asp:Button>
                                                        </div>--%>

                                                        <div class="col-sm-6 d-flex align-items-center">
                                                             <div style="margin-right:12px;">
                                                                <button type="button"
                                                                    onclick="<%=btnDownloadPO.ClientID %>.click()"
                                                                    class="btn buttonColor btn-match">
                                                                    Download Form
                                                                    <i class="fa fa-download ms-1"></i>
                                                                </button>
                                                            </div>

                                                            <asp:Button runat="server" Style="display:none;"
                                                                ID="btnDownloadPO"
                                                                OnClick="btnDownloadPO_Click" />

                                                            <div id="divbtnseeattach" runat="server"  style="margin-right:12px;">
                                                                <button type="button"
                                                                    onclick="openPdfModal()"
                                                                    class="btn buttonColor btn-match">
                                                                    See Attach doc
                                                                    <i class="fa fa-eye ms-1"></i>
                                                                </button>
                                                            </div> 

                                                           <div id="divChangeApprover" runat="server">
                                                                <button type="button"
                                                                    onclick="<%=btnChangeApprover.ClientID %>.click()"
                                                                    class="btn buttonColor btn-match">
                                                                  Change Appover
                                                                    <i class="fa fa-refresh ms-1"></i>
                                                                </button>
                                                            </div>

                                                            <asp:Button runat="server" Style="display:none;"
                                                                ID="btnChangeApprover"
                                                                OnClick="btnChangeApprover_Click" />





<%--                                                            <div id="divChangeApprover" runat="server">
                                                                <button type="button"
                                                                    onclick="<%=btnChangeApprover.ClientID %>.click()"
                                                                    class="btn buttonColor btn-match">
                                                                  Change Appover
                                                                    <i class="fa fa-refresh ms-1"></i>
                                                                </button>
                                                            </div>--%>

<%--                                                            <asp:Button runat="server" Style="display:none;"
                                                                ID="btnChangeApprover"
                                                                OnClick="btnChangeApprover_Click" />--%>

                                                       </div>


<%--                                                        <div class='col-sm-6' id="divCancel" runat="server">
                                                            <button type="button" style="float: right;" onclick="<%=btnCancelForm.ClientID %>.click()" class="btn btn-danger mb-1">
                                                                Cancel Form
                                                                <span class="btn-icon-right"><i class="fa fa-cancel"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnCancelForm" OnClick="btnCancelForm_Click"></asp:Button>
                                                        </div>--%>


                                              <div class='col-sm-6' id="divCancel" runat="server" style="margin-top:3px;">
                                                <button type="button" style="float: right;"  onclick="confirmCancelPO()" class="btn btn-danger mb-1">
                                                    Cancel Form
                                                    <span class="btn-icon-right"><i class="fa-solid fa-remove"></i></span>
                                                </button>
                                                <asp:Button 
                                                    runat="server" 
                                                    ID="btnCancelForm" 
                                                    Style="display:none;" 
                                                    OnClick="btnCancelForm_Click" />
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
                                    <legend class="scheduler-border"><i class="fa fa-pencil-square-o"></i><span class="nav-text">&nbsp;Request Details</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <%--<div class='col-sm-12' id="divBtnAddItem" runat="server">
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
							                    <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>--%>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPO" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPO_RowCommand" OnRowDataBound="TableItemPO_RowDataBound" OnSelectedIndexChanged="TableItemPO_SelectedIndexChanged">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="catalog_type" HeaderText="Catalog" />
                                                            <asp:BoundField DataField="rf_no" HeaderText="RF No." />
                                                            <asp:BoundField DataField="po_type" HeaderText="PO Type" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="remarks" HeaderText="Remark" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Qty" />
                                                            <asp:BoundField DataField="unit_name" HeaderText="UOM" />
                                                            <asp:TemplateField HeaderText="Price">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-type="currency" disabled />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("amount"))) %>' name="txtAmount" id="txtAmount" data-type="currency" disabled />
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
                                                        <input runat="server" id="txtVAT" data-validate-length-range="5,15" type="text" class="form-control" placeholder="vat..." disabled>
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
                                            <div class='col-sm-8'></div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <%--<div class='col-sm-12' id="divSend" runat="server">
                                <button type="button" style="float: right;" onclick="<%=btnUpdatePO.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Update
                                    <span class="btn-icon-right"><i class="fa-solid fa-check-circle"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnUpdatePO" OnClick="btnUpdatePO_Click" OnClientClick="ShowLoading()"></asp:Button>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>





    <!--**********************************
		Modal Change Approver
	***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlChangeApprover" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="mdlChangeApprover1">List of Approver</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12' id="divIThead" runat="server">
                                IT HEAD
				            <div class="form-group">
                                <div class='input-group'>
                                    <asp:DropDownList ID="ddlITHead" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false"
                                        runat="server" OnSelectedIndexChanged="ddlITHead_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            </div>
                            <div class='col-sm-12' id="divGAHead" runat="server">
                               GA HEAD
                                <div class="form-group">
                                    <div class='input-group'>
                                        <asp:DropDownList ID="ddlGAHead" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false"
                                            runat="server" OnSelectedIndexChanged="ddlGAHead_SelectedIndexChanged">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class='col-sm-12' id="divGMDivision" runat="server">
                                General Manager
                                <div class="form-group">
                                    <div class='input-group'>
                                        <asp:DropDownList ID="ddlGMDivision" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false"
                                            runat="server" OnSelectedIndexChanged="ddlGMDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" onclick="<%=btnUpdateApproval.ClientID %>.click()" class="btn mb-1 buttonColor">
                        Update
		            <span class="btn-icon-right"><i class="fa fa-refresh"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnUpdateApproval" OnClick="btnUpdateApproval_Click" OnClientClick="ShowLoading()"></asp:Button>
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
                                <legend class="scheduler-border"><i class="fa fa-search"></i><span class="nav-text">Find of requesition item</span></legend>
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
                                                        <%--<asp:BoundField DataField="price" HeaderText="Price" />--%>
                                                        <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:N0}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                                                        <asp:BoundField DataField="id_vendor" HeaderText="id_vendor" />
                                                        <asp:BoundField DataField="no_po" HeaderText="no_po" />
                                                        <%--<asp:BoundField DataField="amount" HeaderText="amount" />--%>
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:N0}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="catalog_type" HeaderText="catalog_type" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
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
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


        <div class="modal fade" id="pdfModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered"
         style="max-width:95%; width:95%;">
        <div class="modal-content">

            <!-- HEADER -->
            <div class="modal-header d-flex justify-content-between align-items-center">
                <h5 class="modal-title mb-0">Attachment Document</h5>

                <!-- CLOSE MANUAL (TIDAK TERGANTUNG BOOTSTRAP) -->
                <button type="button"
                        onclick="closePdfModal()"
                        style="border:none; background:transparent;
                               font-size:28px; line-height:1; cursor:pointer;">
                    &times;
                </button>
            </div>

            <!-- BODY -->
            <div class="modal-body p-0" style="height:85vh;">
                <iframe id="pdfFrame"
                        style="width:100%; height:100%; border:none;">
                </iframe>
            </div>

        </div>
    </div>
</div>


    <script>
        function openPdfModal() {
            const frame = document.getElementById('pdfFrame');

            frame.src = '<%= ResolveUrl(Convert.ToString(Session["AttachmentPO"])) %>';

            $('#pdfModal').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function closePdfModal() {
            $('#pdfFrame').attr('src', '');
            $('#pdfModal').modal('hide');
        }
    </script>


    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>
</asp:Content>
