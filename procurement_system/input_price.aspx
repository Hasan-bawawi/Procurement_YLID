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


        function validchek(btn) {

            debugger;
            var isValid = true;

            var dropdowns = document.querySelectorAll("select[id*='ddlVendor']");

            dropdowns.forEach(function (dropdown) {


                dropdown.style.border = "";
                var btnVisual = dropdown.closest("td").querySelector(".bootstrap-select button");
                if (btnVisual) btnVisual.style.border = "";

                // cek valid
                if (dropdown.value === "" || dropdown.value === "00000000-0000-0000-0000-000000000000") {
                    dropdown.style.border = "3px solid red";
                    isValid = false;

                    var btnVisualEmp = dropdown.closest("td").querySelector(".bootstrap-select button");
                    if (btnVisualEmp) btnVisualEmp.style.border = "3px solid red";
                }
            });

            if (isValid) {
                ShowLoading();
            }

            return isValid;
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
<%--function DeleteSuccess() {
             /*swal('Delete Success!', 'Item successfully deleted', 'success');*/

             swal({
                 title: 'Delete Success!',
                 text: 'Item successfully deleted',
                 icon: 'success',
                 timer: 2000,
                 showConfirmButton: false
             },
                 function redirect() {
                     var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
                      window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
                  });

         }--%>
     function DeleteSuccess() {
         var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
         swal("Delete Success!", "Item successfully deleted", "success");
         setTimeout(function () {
             window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
         }, 2000);
     }
 </script>

 <script type="text/javascript">
     <%-- function AddItemsSuccess() {

        /* swal('Save Success!', 'Item successfully Submited', 'success');*/
         swal({
             title: 'Save Success!',
             text: 'Item successfully Added',
             icon: 'success',
             timer: 2000,
             showConfirmButton: false
         },
             function redirect() {
                 var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
                   window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
         });

     }--%>
     function AddItemsSuccess() {
         var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
         swal("Save Success!", "Item successfully Added", "success");
              setTimeout(function () {
                  window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
              }, 2000);
          }
 </script>
  <script type="text/javascript">
<%--      function UpdateItemsSuccess() {
          swal({
              title: 'Update Success!',
              text: 'Item successfully updated',
              icon: 'success',
              timer: 2000,
              showConfirmButton: false
          },
              function redirect() {
                  var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
            window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
        });
      }--%>

      function UpdateItemsSuccess() {
          var rf_no = document.getElementById('<%= lbRFNumberHeader.ClientID %>').innerText;
          swal("Update Success!", "Item successfully updated", "success");
          setTimeout(function () {
              window.location.href = 'input_price.aspx?rf_no=' + encodeURIComponent(rf_no);
          }, 2000);
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
<%--    <script type="text/javascript">
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
    </script>--%>
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
    <script type="text/javascript">
        function ErrorSubmit() {
            swal('Submit Failed!', 'Grant total cannot be 0 ', 'error');
        }
    </script>
        <script type="text/javascript">
            function ErrorSubmit_() {
                swal('Submit Failed!', 'Details cannot empty ', 'error');
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
    <asp:HiddenField ID="hlbiddet" runat="server" />
    <asp:HiddenField ID="lbCatalogType" runat="server" />
    <asp:HiddenField ID="hlbNIKApprover" runat="server" />
    <asp:HiddenField ID="hlbOK" runat="server" />
    <asp:HiddenField ID="hfAttachmentPath" runat="server" />


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

<%--                                           <div class='col-sm-12' id="divBtnAddCart" runat="server">
                                                <button type="button" style="float: right;" onclick="<%=btnAddItem.ClientID %>.click()" class="btn mb-1 buttonColor">
                                                    Add Item
									                <span class="btn-icon-right"><i class="fa-solid fa-cart-plus"></i></span>
                                                </button>
                                                <asp:Button runat="server" Style="display: none;" ID="btnAddItem" OnClick="btnAddItem_Click"></asp:Button>
                                            </div>--%>
                                            <div class='col-sm-12'>
                                                <div class="table-responsive">
                                                    <asp:GridView ID="TableItemPurchase" runat="server" CssClass="table table-bordered table-striped verticle-middle" AutoGenerateColumns="False" Style="width: 100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found" OnRowCommand="TableItemPurchase_RowCommand" OnRowDataBound="TableItemPurchase_RowDataBound" OnSelectedIndexChanged="TableItemPurchase_SelectedIndexChanged" DataKeyNames="id">
                                                        <HeaderStyle BackColor="#06183d" ForeColor="White" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

<%--                                                           <asp:TemplateField HeaderText ="Action">
                                                           <ItemTemplate>
                                                               <div Class="action-cell">
                                                                 <asp:LinkButton runat="server" ID="btnEdit" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm btn-primary ml-2" OnClick="btnEdit_Click" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                                 <asp:LinkButton runat="server" ID="btnRemove" CommandArgument="<%# Container.DataItemIndex %>" CssClass="btn btn-sm btn-danger ml-2" OnClick="btnRemove_Click" OnClientClick="return confirmDelete(this);" ToolTip="Remove"><i class="fa fa-trash"></i></asp:LinkButton>
                                                               </div>
                                                           </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:BoundField DataField="id" HeaderText="id" />
                                                            <asp:BoundField DataField="stok_code" HeaderText="Code Stock" />
                                                            <asp:BoundField DataField="item_code" HeaderText="Code Item" />
                                                            <asp:BoundField DataField="item_name" HeaderText="Item" />
                                                            <asp:BoundField DataField="merk_name" HeaderText="Merk" />
                                                            <%--<asp:BoundField DataField="description" HeaderText="Description" />--%>
                                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                                                            <asp:BoundField DataField="remaks" HeaderText="Remarks" />
<%--                                                            <asp:TemplateField HeaderText="@ Price Input">
                                                                <ItemTemplate>
                                                                    <input type="text" class="input-group-text" runat="server" value='<%# string.Format("{0:#,#}", Convert.ToDecimal(Eval("price"))) %>' name="txtPrice" id="txtPrice" data-qty='<%# Eval("quantity") %>' data-type="currency" placeholder="enter price per @" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                             <asp:TemplateField HeaderText="Vendor">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList class="input-group-text" ID="ddlVendor" data-width="80%" data-show-subtext="true" data-live-search="true" AppendDataBoundItems="true" AutoPostBack="false" runat="server" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                             </asp:TemplateField>                                                           
                                                            <asp:TemplateField HeaderText="@ Price Input">
                                                                <ItemTemplate>
                                                                    <input type="text" 
                                                                           name="txtPrice" id="txtPrice"
                                                                           class="input-price form-control"
                                                                           runat="server"
                                                                           value=""
                                                                           data-quantity='<%# Eval("quantity") %>' 
                                                                           placeholder="enter price per @" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="@ Previous Price Input">
                                                             <ItemTemplate>
                                                                 <input type="text" 
                                                                        name="txtPrevPrice" id="txtPrevPrice"
                                                                        class="input-priceprev form-control"
                                                                        runat="server"
                                                                        value=""
                                                                        data-quantity='<%# Eval("quantity") %>' 
                                                                        placeholder="enter price per @" />
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
<%--                            <div class='col-sm-4'>
                                <div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-append">
                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                        </div>
                                        <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="0" value="0" readonly>
                                        <button type="button" class="btn btn-sm btn-danger ml-2" onclick="clearEstimasi();"> Clear <i class="fa-solid  fa-refresh"></i></button>
                                    </div>
                                </div>
                            </div>--%>
                            
                                      <%--     <div class='col-sm-8' id="divUploadFile" runat="server" visible="true">
                                                 <div class="form-group">
                                                     <div class="input-group">
                                                         <div class="input-group-append">
                                                             <span class="input-group-text">Upload Offering&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                         </div>
                                                         <div class="input-group-append">
                                                             <span class="input-group-text">
                                                                 <asp:FileUpload ID="FileUploadEDocs" AllowMultiple="true" runat="server" /></span>
                                                         </div>
                                                     </div>
                                                 </div>
                                             </div>--%>


                            <div class='col-sm-4'>
                                <div class="form-group">
                                    <div class="input-group">
                                         <div class="input-group-append ml-1 d-flex align-items-center mr-2">
                                            <div class="form-check mb-0">
                                                <input type="checkbox" runat="server" id="chkNoNeedPO" class="form-check-input" />
                                                <label class="form-check-label" for="chkNoNeedPO">
                                                    No Need PO
                                                </label>
                                            </div>
                                        </div>


                                        <div class="input-group-append">
                                            <span class="input-group-text">Grand Total (IDR)&nbsp;&nbsp;&nbsp;</span>
                                        </div>

                                       <input runat="server" id="txtGrandTotal" data-validate-length-range="5,15" type="text" class="form-control" placeholder="0" value="0" readonly>


                                        <!-- ✅ Checkbox No Need PO -->


                                        <button type="button" class="btn btn-sm btn-danger ml-2" onclick="clearEstimasi();">
                                            Clear <i class="fa-solid fa-refresh"></i>
                                        </button>

                                    </div>
                                </div>
                            </div>



                            <div class='col-sm-8'></div>
                           <%-- <div class='col-sm-4' id="divClearPrice" runat="server" visible="true">--%>
<%--                                <button type="button" style="float: right;" class="btn btn-danger mb-1">
                                    Clear Price
                                <span class="btn-icon-right"><i class="fa fa-close"></i></span>
                                </button>--%>
                                <%--<asp:Button runat="server" Style="display: none;" ID="btnClearPrice" OnClick="btnClearPrice_Click"></asp:Button>--%>
                                <%--onclick="<%=btnClearPrice.ClientID %>.click()" --%>
                                <%--<asp:Button ID="btnClearEstimasi" runat="server" Text="Clear Estimasi" CssClass="btn btn-danger" OnClientClick="clearEstimasi(); return false;" />--%>

                            <%--</div>--%>
<%--                            <div class='col-sm-4' id="divCheck" runat="server">
                                <button type="button" style="float: right;" onclick="<%=btnCheck.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Price Calculate
                                <span class="btn-icon-right"><i class="fa fa-calculator"></i></span>
                                </button>
                                <asp:Button runat="server" Style="display: none;" ID="btnCheck" OnClick="btnCheck_Click"></asp:Button>
                            </div>--%>
                            <div class='col-sm-12' id="divSubmit" runat="server" visible="true">
                                <button type="button" style="float: right;" onclick="<%=btnSubmit.ClientID %>.click()" class="btn mb-1 buttonColor">
                                    Submit
                                <span class="btn-icon-right"><i class="fa fa-save"></i></span>
                                </button>
                               
<%--                                OnClick="btnSubmit_Click" OnClientClick="ShowLoading()"--%>
                                <asp:Button runat="server" Style="display: none;" ID="btnSubmit" OnClick="btnSubmit_Click" OnClientClick="return validchek(this)" ></asp:Button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


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

    <script type="text/javascript">

        $(document).ready(function () {

            $(document).on("keyup blur", ".input-priceprev", function () {
                formatCurrency($(this));
            });


            $(document).on("keyup blur", ".input-price", function () {
                formatCurrency($(this)); 
                calculateGrandTotal();
            });

            function calculateGrandTotal() {
                var grandTotal = 0;

                $(".input-price").each(function () {
                    var priceText = $(this).val().replace(/,/g, "");
                    var price = parseFloat(priceText) || 0;
                    var qty = parseFloat($(this).data("quantity")) || 0;

                    grandTotal += (price * qty);
                });

                $("#<%= txtGrandTotal.ClientID %>").val(grandTotal.toLocaleString('en-US'));

            }

        });


        function clearEstimasi() {
            $(".input-price").each(function () {
                $(this).val("");
            });

            $("#<%= txtGrandTotal.ClientID %>").val("0");
        }

      
        function formatNumber(n) {
            return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        }


        function formatCurrency(input, blur) {

            var input_val = input.val();

            if (input_val === "") { return; }

            var original_len = input_val.length;

            var caret_pos = input.prop("selectionStart");

            if (input_val.indexOf(".") >= 0) {

            
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
