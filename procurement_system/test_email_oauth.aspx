<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="test_email_oauth.aspx.cs" Inherits="procurement_system.test_email_oauth" Async="true" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function SendSuccess() {
             swal({
                 title: 'Send Success',
                 text: 'Successfully Send',
                 timer: '2000',
                 type: 'success',
                 showConfirmButton: false,
                 html: true
             },
                 function redirect() {
                     window.location.href = 'test_email_oauth.aspx';
                 }
             );
         }
    </script>
    <script type="text/javascript">
        function FailedSend() {
            swal('Send Failed!', 'Testtttttttt!', 'error');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div hidden="hidden">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<rsweb:ReportViewer ID="ReportViewerPurchase" runat="server" Width="100%" Height="100%">
	</rsweb:ReportViewer>
</div>
    <div class='col-sm-12'>
        <button type="button" onclick="<%=btnCreateRF.ClientID %>.click()" class="btn mb-1 buttonColor">
            Test Send
											    <span class="btn-icon-right"><i class="fa-solid fa-file-circle-plus"></i></span>
        </button>
        <asp:Button runat="server" Style="display: none;" ID="btnCreateRF" OnClick="btnCreateRF_Click"></asp:Button>
    </div>

   <div class='col-sm-12'>
           <button type="button" onclick="<%=btnDownloadRF.ClientID %>.click()" class="btn mb-1 buttonColor">
        Download Form
<span class="btn-icon-right"><i class="fa fa-download"></i></span>
    </button>
    <asp:Button runat="server" Style="display: none;" ID="btnDownloadRF" OnClick="btnDownloadRF_Click"></asp:Button>
   </div>

    <asp:Image ID="QRCodeImage" runat="server" />

    <div class='col-sm-12'>
    <fieldset class="scheduler-border">
        <legend class="scheduler-border">List of Requisition Form (RF) - Need Director Approved</legend>
        <div class="control-group">
            <div class="row">
                <div class='col-sm-12'>
                    <div class="table-responsive">
                        <asp:GridView ID="TableDirectorApproval" runat="server" CssClass="table table-striped table-bordered zero-configuration nowrap grid" AutoGenerateColumns="False" Style="width: 100%"
                            ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found">
                            <HeaderStyle BackColor="#06183d" ForeColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnViewDirector" CommandName="Lihat" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn buttonColorGridview" OnClick="btnViewDirector_Click" ToolTip="View Details"><i class="fa-solid fa-eye"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id" HeaderText="id" />
                                <asp:BoundField DataField="catalog_type" HeaderText="Catalog Type" />
                                <asp:BoundField DataField="rf_no" HeaderText="RF Number" />
                                <asp:BoundField DataField="request_date" HeaderText="Request Date" />
                                <asp:BoundField DataField="Requester" HeaderText="Requester" />
                                <asp:BoundField DataField="ManagerApprove" HeaderText="Manager Approve" />
                                <asp:BoundField DataField="GMApprove" HeaderText="GM Approve" />
                                <asp:BoundField DataField="DeputyDirectorApprove" HeaderText="Deputy Director Approve" />
                                <asp:BoundField DataField="AdmManagerApprove" HeaderText="Adm. Manager Approve" />
                                <asp:BoundField DataField="AdmGMApprove" HeaderText="Adm. GM Approve" />
                                <asp:BoundField DataField="ITManagerApprove" HeaderText="IT Manager Approve" />
                                <asp:BoundField DataField="DirectorApprove" HeaderText="Director Approve" />
                                <asp:BoundField DataField="status_approve" HeaderText="Approval Status" />
                                <asp:BoundField DataField="status" HeaderText="RF Status" />
                                <asp:BoundField DataField="nama_branch" HeaderText="Location" />
                                <asp:BoundField DataField="no_po" HeaderText="no_po" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
</div>

     <asp:HiddenField ID="hlbID" runat="server" />
 <asp:HiddenField ID="hlbiduser" runat="server" />
 <asp:HiddenField ID="hblNIK" runat="server" />
 <asp:HiddenField ID="hlbRequester" runat="server" />
 <asp:HiddenField ID="hblEmail" runat="server" />

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

     <!-- Datatables -->

 <link href="./plugins/tables/css/datatable/dataTables.bootstrap4.min.css" rel="stylesheet">
 <script src="./plugins/tables/js/jquery.dataTables.min.js"></script>
 <script src="./plugins/tables/js/datatable/dataTables.bootstrap4.min.js"></script>
 <script src="./plugins/tables/js/datatable-init/datatable-basic.min.js"></script>

 <script type="text/javascript" src="vendors/date/JS/jquery-1.10.2.min.js"></script>

 <script src="vendors/sweetalert.js"></script>
 <script src="vendors/sweetalert.min.js"></script>

 <script>
     $(document).ready(function () {
         $(".datepicker1").datepicker({ format: 'mm/dd/yyyy', autoclose: true, todayBtn: 'linked' })
     });
 </script>
</asp:Content>
