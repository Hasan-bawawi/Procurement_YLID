<%@ Page Title="Detail Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="detail_requisition_form.aspx.cs" Inherits="procurement_system.detail_requisition_form" Async="true" %>

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

         .action-cell {
             display: flex;
             justify-content: center;   
             align-items: center;       
             height: 100%;
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


        function validateRow(btn) {
            debugger

            var ddlItem = document.getElementById('<%= ddlItem.ClientID %>');
            var txtJumlahBeli = document.getElementById('<%= txtJumlahBeli.ClientID %>');
            var txtRemaks = document.getElementById('<%= txtRemaks.ClientID %>');

            var isValid = true;


            [ddlItem, txtJumlahBeli, txtRemaks].forEach(function (input) {
                if (input) input.style.border = "";
            });

            // Validasi dropdown Item
            if (!ddlItem || ddlItem.value.trim() === "" || ddlItem.value === "00000000-0000-0000-0000-000000000000") {
                $('#' + ddlItem.id).parent().find('.dropdown-toggle').css('border', '2px solid red');
                isValid = false;
            }

            // Validasi Quantity
            if (!txtJumlahBeli || txtJumlahBeli.value.trim() === "" || parseFloat(txtJumlahBeli.value) <= 0) {
                txtJumlahBeli.style.border = "2px solid red";
                isValid = false;
            }

            // Validasi Remarks
            if (!txtRemaks || txtRemaks.value.trim() === "") {
                txtRemaks.style.border = "2px solid red";
                isValid = false;
            }

            //if (!isValid) {
            //    swal('Validation Failed', 'Please fill all required fields: Item, Quantity, and Remarks.', 'error');
            //}

            return isValid;
        }
    </script>

    <script type="text/javascript">
        //function DeleteSuccess() {
        //   swal('Delete Success!', 'Item successfully deleted', 'success');

        //}
        function DeleteSuccess() {
            var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
                swal("Delete Success!", "Item successfully deleted", "success");
                setTimeout(function () {
                    window.location.href = 'detail_requisition_form.aspx?rf_no=' + encodeURIComponent(rf_no);
                }, 2000);
        }

    </script>

    <script type="text/javascript">
        //function AddItemsSuccess() {
        //    swal('Save Success!', 'Item successfully Submited', 'success');
        //    //swal({
        //    //    title: 'Save Success!',
        //    //    text: 'Item successfully Added',
        //    //    icon: 'success'
        //    //}).then(() => {
        //    //    location.reload(); // atau window.location.href = 'somepage.aspx';
        //    //});
        //}
        function AddItemsSuccess() {
            var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
            swal("Save Success!", "Item successfully Added", "success");
            setTimeout(function () {
                window.location.href = 'detail_requisition_form.aspx?rf_no=' + encodeURIComponent(rf_no);
            }, 2000);
        }
    </script>
     <script type="text/javascript">
            //function UpdateItemsSuccess() {
            //    swal('Update Success!', 'Item successfully Update', 'success');

            //    //swal({
            //    //    title: 'Update Success!',
            //    //    text: 'Item successfully Updated',
            //    //    icon: 'success'
            //    //}).then(() => {
            //    //    location.reload(); // atau window.location.href = 'somepage.aspx';
            //    //});
            // }
         function UpdateItemsSuccess() {
             var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
             swal("Update Success!", "Item successfully updated", "success");
             setTimeout(function () {
                 window.location.href = 'detail_requisition_form.aspx?rf_no=' + encodeURIComponent(rf_no);
             }, 2000);
         }



     </script>
    <script type="text/javascript">
        function confirmDelete(linkButton) {

            var href = linkButton.getAttribute("href");
            var match = href.match(/__doPostBack\('([^']+)'/);
            if (match && match.length > 1) {
                var postBackTarget = match[1];
                console.log("PostBack Target:", postBackTarget);
            }
            swal({

                title: 'Are you sure?',
                text: "Do you want to remove selected record?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, remove it!',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#d33',
                showCloseButton: true,

            }, function (willDelete) {
                if (willDelete) {
                    __doPostBack(postBackTarget, '');
                }
            });

            return false;
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
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="requisition_form.aspx">&nbsp;Requisition Form (RF)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Detail Requisition Form (RF) - 
            <asp:Label runat="server" ID="lbRFNumberBreadcrumb"></asp:Label></li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:Label ID="lblNamaBranch" runat="server" Text=""></asp:Label>
        <asp:Label ID="hblNIK" runat="server" Text=""></asp:Label>
    </div>

    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <asp:Image ID="imgQRCode" runat="server" />
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
    <asp:HiddenField ID="lbNikMGRNew" runat="server" />
    <asp:HiddenField ID="lbEmailMGRNew" runat="server" />
    <asp:HiddenField ID="lbNikGMNew" runat="server" />
    <asp:HiddenField ID="lbEmailGMNew" runat="server" />
    <asp:HiddenField ID="hlbNIKApprover" runat ="server" />
    <asp:HiddenField ID="hlbiddet" runat="server" />
    <asp:HiddenField ID="hlbsendpur" runat="server" />

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
                                                            <li id="price_estimated" runat="server" class="StepProgress-item current"><strong>Price Check</strong><asp:Label ID="lbDatePriceEstimate" runat="server"></asp:Label></li>
                                                            <li id="manager" runat="server" class="StepProgress-item current"><strong>Manager</strong><asp:Label ID="lbDateMgr" runat="server"></asp:Label></li>
                                                            <li id="gm" runat="server" class="StepProgress-item current"><strong>General Manager</strong><asp:Label ID="lbDateGM" runat="server"></asp:Label></li>
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
                                                                <asp:Label runat="server" ID="hlbSection" Visible="false"></asp:Label><asp:Label runat="server" ID="lbSection"></asp:Label>
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
                                                        <div class='col-sm-6'>
                                                            <button type="button" onclick="<%=btnDownloadRF.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                                Download Form
                                                            <span class="btn-icon-right"><i class="fa fa-download"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnDownloadRF" OnClick="btnDownloadRF_Click"></asp:Button>
                                                        </div>
                                                        <div class='col-sm-6' id="divChangeApprover" runat="server">
                                                            <button type="button" style="float: right;" onclick="<%=btnChangeApprover.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                                Change Appover
                                                            <span class="btn-icon-right"><i class="fa fa-refresh"></i></span>
                                                            </button>
                                                            <asp:Button runat="server" Style="display: none;" ID="btnChangeApprover" OnClick="btnChangeApprover_Click"></asp:Button>
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
                                        <%--<asp:LinkButton ID="btnEditForm" runat="server" class="badge badge-pill badge-light" OnClick="btnEditForm_Click" Text="">Edit Item</asp:LinkButton>--%>
                                    </span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12' id="divBtnAddCart" runat="server">
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
									                <span class="btn-icon-right"><i class="fa-solid fa-cart-plus"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPurchase_RowCommand" OnRowDataBound="TableItemPurchase_RowDataBound" OnSelectedIndexChanged="TableItemPurchase_SelectedIndexChanged"
                                                        DataKeyNames="id">
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
                                                            <%--<asp:BoundField DataField="description" HeaderText="Description" />--%>
                                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <div Class="action-cell">
                                                                     
                                                                    <asp:LinkButton runat="server" ID="btnEdit" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm btn-primary " OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                    <asp:LinkButton runat="server" ID="btnRemove" CommandName="Hapus" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm btn-danger ml-2 <%--buttonColorGridview--%>" OnClick="btnRemove_Click" OnClientClick="return confirmDelete(this);" ToolTip="Remove"><i class="fa fa-trash"></i></asp:LinkButton>

                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class='col-sm-12' id="divCancelRF" runat="server">
                                                <button type="button" onclick="<%=btnCancelRF.ClientID %>.click()" class="btn btn-danger mb-1 <%--buttonColor--%>">
                                                    Cancel RF
                                                <span class="btn-icon-right"><i class="fa-solid  fa-remove"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnCancelRF" OnClick="btnCancelRF_Click" OnClientClick="ShowLoading()"></asp:Button>
                                            </div>
<%--                                            <div class='col-sm-12' id="divSend" runat="server">
                                                <button type="button" style="float: right;" onclick="<%=btnSend.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Send to Purchasing
									                <span class="btn-icon-right"><i class="fa-solid fa-paper-plane"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnSend" OnClick="btnSend_Click" OnClientClick="ShowLoading()"></asp:Button>
                                            </div>--%>
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
			Modal List Items
		***********************************-->
    <div class="modal fade bs-example-modal-lg" id="mdlListItems" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel12">List of Goods Catalog</h4>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="x_content">
                        <div class="row">
                            <div class='col-sm-12'>
                                Items
					            <div class="form-group">
                                    <div class='input-group'>
                                        <asp:DropDownList ID="ddlItem" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="true"
                                            runat="server" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class='col-sm-12'>
                                Qty
					            <div class="form-group">
                                    <div class='input-group'>
                                        <input runat="server" id="txtJumlahBeli" data-validate-length-range="5,15" type="number" class="form-control" placeholder="Quantity" onkeypress="return isNumberKey(event)">
                                    </div>
                                </div>
                            </div>
                            <%--<div class='col-sm-12'>
                                Description
					            <div class="form-group">
                                    <div class='input-group'>
                                        <textarea class="form-control h-150px" rows="2" id="txtDescription" runat="server"></textarea>
                                    </div>
                                </div>
                            </div>--%>
                            <div class='col-sm-12'>
                                Remarks
					            <div class="form-group">
                                    <div class='input-group'>
                                        <textarea class="form-control h-150px" rows="2" id="txtRemaks" runat="server"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" onclick="<%=btnUpdate.ClientID %>.click()" class="btn buttonColor">
                        Update
			        <span class="btn-icon-right"><i class="fa fa-refresh"></i></span>
                    </button>
                    <asp:Button runat="server" Style="display: none;" ID="btnUpdate" OnClick="btnUpdate_Click" OnClientClick="return validateRow(this);"></asp:Button>
                    <button type="button" class="btn buttonColor" data-dismiss="modal">
                        Close
			        <span class="btn-icon-right"><i class="fa fa-remove"></i></span>
                    </button>
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
                            <div class='col-sm-12' id="divManagerDivision" runat="server">
                                Manager
				            <div class="form-group">
                                <div class='input-group'>
                                    <asp:DropDownList ID="ddlManagerDivision" class="selectpicker form-control" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false"
                                        runat="server" OnSelectedIndexChanged="ddlManagerDivision_SelectedIndexChanged">
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

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
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
