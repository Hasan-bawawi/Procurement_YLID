<%@ Page Title="Edit Requisition Form (RF)" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="requisition_form_edit.aspx.cs" Inherits="procurement_system.requisition_form_edit" %>

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
        <!-- Progress with steps -->
        ol.progtrckr {
            margin: 0;
            padding: 0;
            list-style-type: none;
        }

        ol.progtrckr li {
            display: inline-block;
            text-align: center;
            line-height: 3em;
        }

        ol.progtrckr[data-progtrckr-steps="2"] li {
            width: 49%;
        }

        ol.progtrckr[data-progtrckr-steps="3"] li {
            width: 33%;
        }

        ol.progtrckr[data-progtrckr-steps="4"] li {
            width: 24%;
        }

        ol.progtrckr[data-progtrckr-steps="5"] li {
            width: 19%;
        }

        ol.progtrckr[data-progtrckr-steps="6"] li {
            width: 16%;
        }

        ol.progtrckr[data-progtrckr-steps="7"] li {
            width: 14%;
        }

        ol.progtrckr[data-progtrckr-steps="8"] li {
            width: 12%;
        }

        ol.progtrckr[data-progtrckr-steps="9"] li {
            width: 11%;
        }

        ol.progtrckr li.progtrckr-done {
            color: black;
            border-bottom: 4px solid yellowgreen;
        }

        ol.progtrckr li.progtrckr-todo {
            color: silver;
            border-bottom: 4px solid silver;
        }

        ol.progtrckr li:after {
            content: "\00a0\00a0";
        }

        ol.progtrckr li:before {
            position: relative;
            bottom: -2.5em;
            float: left;
            left: 50%;
            line-height: 1em;
        }

        ol.progtrckr li.progtrckr-done:before {
            font-family: FontAwesome;
            content: "\f058";
            color: white;
            background-color: yellowgreen;
            height: 1.2em;
            width: 1.2em;
            line-height: 1.2em;
            border: none;
            border-radius: 1.2em;
        }

        ol.progtrckr li.progtrckr-todo:before {
            font-family: FontAwesome;
            content: "\f252";
            color: silver;
            background-color: white;
            font-size: 1.5em;
            bottom: -1.6em;
        }
    </style>
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function FuncSave() {
            swal({
                title: 'Save Success',
                text: 'Requisition Form Successfully Submited',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncUpdate() {
            swal({
                title: 'Update Success',
                text: 'Requisition Form Successfully Updated',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'requisition_form.aspx';
                }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncDelete() {
            swal({
                title: 'Delete Success',
                text: 'Item Successfully Deleted from list',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            }
            );
        }
    </script>
    <script type="text/javascript">
        function FuncAdd() {
            swal({
                title: 'Add Success',
                text: 'Item successfully added to the list',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            }
            );
        }
    </script>
    <script type="text/javascript">
        function SelectCategory() {
            swal('Save Failed!', 'Please select category name!', 'error');
        }
    </script>
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
        function SelectItem() {
            swal('Save Failed!', 'Please select Item first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectReqType() {
            swal('Save Failed!', 'Please select Request Type first', 'error');
        }
    </script>
    <script type="text/javascript">
        function SelectApprover() {
            swal('Save Failed!', 'Please select Approver first', 'error');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ol class="breadcrumb">
        <li><a href="javascript:void(0)"><i class="fa-solid fa-cart-arrow-down"></i>&nbsp;Orders</a></li>
        <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
        <li><a href="requisition_form.aspx">&nbsp;Requisition Form (RF)</a></li>
        <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
        <li class="active">&nbsp;&nbsp;<asp:Label runat="server" ID="lbRFNumberBreadcrumb"></asp:Label></li>
    </ol>
    <asp:HiddenField ID="lblNamaBranch" runat="server" />
    <asp:HiddenField ID="hblNIKRequester" runat="server" />
    <asp:HiddenField ID="txtRequester" runat="server" />
    <asp:HiddenField ID="txtPosition" runat="server" />
    <asp:HiddenField ID="txtSection" runat="server" />
    <asp:HiddenField ID="hlbItem" runat="server" />
    <asp:HiddenField ID="hlbMerk" runat="server" />
    <asp:HiddenField ID="hlbType" runat="server" />
    <asp:HiddenField ID="hlbNameApprover" runat="server" />
    <asp:HiddenField ID="hlbNIKApprover" runat="server" />
    <asp:HiddenField ID="hlbBranchApprover" runat="server" />
    <asp:HiddenField ID="hlbEmailApprover" runat="server" />
    <asp:HiddenField ID="hlbSectionApprover" runat="server" />
    <asp:HiddenField ID="hlbStatus" runat="server" />
    <asp:HiddenField ID="hlbStatusApprove" runat="server" />

    <div hidden="hidden">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
        </rsweb:ReportViewer>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                             <div class='col-sm-12'>
                                 <fieldset class="scheduler-border">
                                <legend class="scheduler-border">
                                    <i class="fa-solid fa-circle-info"></i>
                                    <span class="nav-text">&nbsp;RF Status</span>
                                </legend>
                                <div class="control-group">
                                    <div class="row">
                                        <div class='col-sm-12' id="divRFCreated" runat="server">
                                            <ol class="progtrckr" data-progtrckr-steps="5">
                                                <li class="progtrckr-done">RF Created</li>
                                                <li class="progtrckr-todo">Manager Approved</li>
                                                <li class="progtrckr-todo">GM Approved</li>
                                                <li class="progtrckr-todo">PO Issued</li>
                                                <li class="progtrckr-todo">Completed</li>
                                            </ol>
                                        </div>
                                        <div class='col-sm-12' id="divManagerApprove" runat="server">
                                            <ol class="progtrckr" data-progtrckr-steps="6">
                                                <li class="progtrckr-done">RF Created</li>
                                                <li class="progtrckr-done">Manager Approved</li>
                                                <li class="progtrckr-todo">GM Approved</li>
                                                <li class="progtrckr-todo">PO Issued</li>
                                                <li class="progtrckr-todo">Completed</li>
                                            </ol>
                                        </div>
                                        <div class='col-sm-12' id="divGMApprove" runat="server">
                                            <ol class="progtrckr" data-progtrckr-steps="7">
                                                <li class="progtrckr-done">RF Created</li>
                                                <li class="progtrckr-done">Manager Approved</li>
                                                <li class="progtrckr-done">GM Approved</li>
                                                <li class="progtrckr-todo">PO Issued</li>
                                                <li class="progtrckr-todo">Completed</li>
                                            </ol>
                                        </div>
                                        <div class='col-sm-12' id="divPOIssued" runat="server">
                                            <ol class="progtrckr" data-progtrckr-steps="6">
                                                <li class="progtrckr-done">RF Created</li>
                                                <li class="progtrckr-done">Manager Approved</li>
                                                <li class="progtrckr-done">GM Approved</li>
                                                <li class="progtrckr-done">PO Issued</li>
                                                <li class="progtrckr-todo">Completed</li>
                                            </ol>
                                        </div>
                                        <div class='col-sm-12' id="divComplete" runat="server">
                                            <ol class="progtrckr" data-progtrckr-steps="7">
                                                <li class="progtrckr-done">RF Created</li>
                                                <li class="progtrckr-done">Manager Approved</li>
                                                <li class="progtrckr-done">GM Approved</li>
                                                <li class="progtrckr-done">PO Issued</li>
                                                <li class="progtrckr-done">Completed</li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                             </div>
                            
                            <div class='col-sm-12'>
                                <br />
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">
                                        <i class="fa fa-file-text-o"></i>
                                        <span class="nav-text">&nbsp;Requisition Form (RF) -
                                        <asp:Label runat="server" ID="lbRFNumberHeader"></asp:Label>
                                        <asp:LinkButton ID="btnDownloadRF" runat="server" class="badge badge-pill badge-light" OnClick="btnDownloadRF_Click" Text="">Download Form</asp:LinkButton>
                                        </span>

                                    </legend>
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
                                                    <label>Approved by</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbApprovedBy"></asp:Label>
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
                                                    <label>Acknowledge by</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbAcknowledgeBy"></asp:Label>
                                                </div>
                                            </div>
                                            <div class='col-sm-2'>
                                                <div class="form-group">
                                                    <label>Division/Section</label>
                                                </div>
                                            </div>
                                            <div class='col-sm-4'>
                                                <div class="form-group">
                                                    :&nbsp;<asp:Label runat="server" ID="lbDivision"></asp:Label>
                                                    <asp:Label runat="server" ID="Label1">/</asp:Label>
                                                    <asp:Label runat="server" ID="lbSection"></asp:Label>
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
                                                            <asp:BoundField DataField="description" HeaderText="Description" />
                                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
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
</asp:Content>
