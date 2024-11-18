<%@ Page Title="View Approval Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="approval_requisition_form_view.aspx.cs" Inherits="procurement_system.approval_requisition_form_view" Async="true" %>

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
        function FuncSave() {
            swal({
                title: 'Approval Success',
                text: 'Form Successfully Approved',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'approval_requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function EmptyApproval() {
            swal('Save Failed!', 'Please select approval/date time!', 'error');
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
            <h1 class="page-header text-overflow" style="color: white;">View Requisition Form (RF)</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Approval</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li><a href="approval_requisition_form.aspx">&nbsp;Approval Requisition Form (RF)</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;View Requisition Form (RF) - 
        <asp:Label runat="server" ID="lbRFNumberBreadcrumb"></asp:Label></li>
        </ol>
    </div>

    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
        <asp:Image ID="imgQRCode" runat="server" />
    </div>

    <asp:HiddenField ID="hblEmailRequester" runat="server" />
    <asp:HiddenField ID="hblEmailManager" runat="server" />
    <asp:HiddenField ID="hblEmailGM" runat="server" />
    <asp:HiddenField ID="hlbEmailManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbEmailGMAdm" runat="server" />
    <asp:HiddenField ID="hlbEmailDirector" runat="server" />
    <asp:HiddenField ID="hlbEmailDirectorAdm" runat="server" />
    <asp:HiddenField ID="hlbEmailITmanager" runat="server" />
    <asp:HiddenField ID="hlbEmailDeputyDirector" runat="server" />
    <asp:HiddenField ID="hlbNIKManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbNIKGMAdm" runat="server" />
    <asp:HiddenField ID="hlbManagerAdm" runat="server" />
    <asp:HiddenField ID="hlbGMAdm" runat="server" />
    <asp:HiddenField ID="hlbGroupName" runat="server" />

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
                                                            <li id="price_estimated" runat="server" class="StepProgress-item current"><strong>Price Checked</strong><asp:Label ID="lbDatePriceEstimate" runat="server"></asp:Label></li>
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
                                    <legend class="scheduler-border"><i class="fa-solid fa-file-lines"></i><span class="nav-text">&nbsp;Item Request Details</span></legend>
                                    <div class="control-group">
                                        <div class="row">
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="stok_code" HeaderText="Code Stock" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code Item" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                            <%--<asp:BoundField DataField="description" HeaderText="Description" />--%>
                                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
                                                            <asp:TemplateField HeaderText="Delete Selected Items">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ckSelectRemove" runat="server" Text="&nbsp;selected to delete" />
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
                            <div class='col-sm-6'>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-append">
                                            <span class="input-group-text">Approval</span>
                                        </div>
                                        <asp:DropDownList ID="ddlApproval" class="selectpicker form-control" AppendDataBoundItems="true"
                                            runat="server" OnSelectedIndexChanged="ddlApproval_SelectedIndexChanged" ValidateRequestMode="Enabled">
                                            <asp:ListItem Enabled="true" Text="" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Reject" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Cancel" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class='col-sm-6'>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-append">
                                            <span class="input-group-text">Approval Date</span>
                                        </div>
                                        <input runat="server" id="txtApprovalDate" data-validate-length-range="5,15" type="text" class="form-control datepicker1" placeholder="Select date...">
                                        <div class="input-group-append">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                        </div>
                                    </div>
                                </div>
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

    <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>
</asp:Content>
