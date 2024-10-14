<%@ Page Title="Event Calender" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="event_calender.aspx.cs" Inherits="procurement_system.event_calender" %>

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

    <script>
        $(document).ready(function () {
            $(".datepicker1").datepicker({ format: 'mm/yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        });
    </script>
    <script>
        $(document).ready(function () {
            $(".datepicker2").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
        });
    </script>

    <style>
    .width-150 {
        width: 150px;
    }
</style>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-head">
        <div id="page-title">
            <h1 class="page-header text-overflow" style="color: white;">Event Calender</h1>
        </div>
        <ol class="breadcrumb">
            <li><a href="javascript:void(0)"><i class="fa-solid fa-server"></i>&nbsp;Master Data</a></li>
            <li class="active">&nbsp;&nbsp;<i class="fa fa-caret-right"></i></li>
            <li class="active">&nbsp;&nbsp;Event Calender</li>
        </ol>
    </div>

    <asp:HiddenField ID="txtOid" runat="server" />

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 ">
                <div class="card custom-card-body">
                    <div class="card-body">
                        <div class="row">
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Create New & Filter Event</legend>
                                    <div class='col-sm-6'>
                                        <div class="form-group">
                                            <div class='input-group'>
                                                <input type="text" id="txtMonth" data-validate-length-range="5,15" runat="server" class="form-control input-rounded datepicker1" placeholder="Month of log">
                                                <div class="input-group-append">
                                                    <button type="button" class="btn buttonColor" onclick="<%=btnCheck.ClientID %>.click()">
                                                        Check
                                                        <span class="btn-icon-right"><i class="fa fa-search"></i></span>
                                                    </button>
                                                    <asp:Button runat="server" Style="display: none;" ID="btnCheck" OnClick="btnCheck_Click"></asp:Button>
                                                </div>
                                                <button type="button" onclick="<%=btnAddNew.ClientID %>.click()" class="btn buttonColor">
                                                    Add
                                                        <span class="btn-icon-right"><i class="fa fa-plus-circle"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddNew" OnClick="btnAddNew_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class='col-sm-12'>
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">List of Event Calender</legend>
                                    <div class='col-sm-12'>
                                        <asp:GridView ID="TableEventCalender" runat="server" CssClass="table table-striped table-bordered zero-configuration nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                                            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                                            <HeaderStyle BackColor="#06183d" ForeColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnEdit" CommandName="Ubah" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa-solid fa-pen-to-square"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Oid" HeaderText="Oid" />
                                                <asp:BoundField DataField="EventDate" HeaderText="Event Date" />
                                                <asp:BoundField DataField="EventRemark" HeaderText="Event Remark" />
                                                <asp:BoundField DataField="Active" HeaderText="Active" />
                                                <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" />
                                                <asp:BoundField DataField="CreateBy" HeaderText="CreateBy" />
                                                <asp:BoundField DataField="ModifiedDate" HeaderText="ModifiedDate" />
                                                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" />
                                            </Columns>
                                        </asp:GridView>
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
		Modal Add
	***********************************-->
    <div class="modal fade bd-example-modal-lg" id="mdlAddEvent" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="<%=btnCloseModalNew.ClientID %>.click()" class="close"><span>×</span></button>
                    <asp:Button runat="server" Style="display: none;" ID="btnCloseModalNew" OnClick="btnCloseModalNew_Click"></asp:Button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class='col-sm-12'>
                            <fieldset class="scheduler-border">
                                <legend class="scheduler-border">Add Event</legend>
                                <div class="row">
                                    <div class='col-sm-12'>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <div class="input-group-append">
                                                    <span class="input-group-text width-150"><strong>Event Date</strong></span>
                                                </div>
                                                <input type="text" id="txtEventDate" data-validate-length-range="5,15" runat="server" class="form-control datepicker2" placeholder="Event Date">
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='col-sm-12'>
                                        <div class="form-group">
                                            <div class="input-group">
                                                <div class="input-group-append">
                                                    <span class="input-group-text width-150"><strong>Event Remark</strong></span>
                                                </div>
                                                <textarea class="form-control h-150px" rows="3" id="txtEventRemark" maxlength="100" runat="server" placeholder="Max. 100 Character"></textarea>
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
                                    <div class='col-sm-12'>
                                        &nbsp;
									<div class="form-group">
                                        <div class='input-group'>
                                            <asp:Button runat="server" Visible="false" ID="btnSubmit" CssClass="btn buttonColor" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                            <asp:Button runat="server" Visible="false" ID="btnUpdate" CssClass="btn buttonColor" Text="Update" OnClick="btnUpdate_Click"></asp:Button>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
