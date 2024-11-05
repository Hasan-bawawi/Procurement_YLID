<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error_page.aspx.cs" Inherits="procurement_system.error_page" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,initial-scale=1">
    <title>Purchasing System - YLID - Error Page</title>
    <!-- Favicon icon -->
    <link rel="icon" href="images/favicon.ico" type="image/x-icon">
    <link href="OtherThemes/quixlab/style.css" rel="stylesheet" />
</head>
<body>
        <!--*******************
	        Preloader start
        ********************-->

        <div id="preloader">
            <div class="loader">
                <svg class="circular" viewBox="25 25 50 50">
                    <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="3" stroke-miterlimit="10" />
                </svg>
            </div>
        </div>

        <div class="login-form-bg h-100">
            <div class="container h-100">
                <div class="row justify-content-center h-100">
                    <div class="col-xl-6">
                        <div class="error-content">
                            <div class="card mb-0">
                                <div class="card-body text-center">
                                    <h1 class="error-text text-primary"><i class="fa fa-warning text-danger"></i></h1>
                                    <h4 class="mt-4">Error Page</h4>
                                    <p>Your Operation ERROR!!, please login again for continue.</p>
                                    <p><asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label></p>
                                    <p><asp:Label ID="lblErrorDescription" runat="server" ForeColor="Red"></asp:Label></p>
                                    <form class="mt-5 mb-5" runat="server">
                                        <asp:HiddenField ID="lblUrl" runat="server" />
                                        <%--<div class="text-center mb-4 mt-4"><a href="index.html" class="btn btn-primary">Go to Login</a>
                                    </div>--%>
                                        <div class="text-center mb-4 mt-4">
                                            <button type="button" class="btn btn-sm btn-success shadow-sm" onclick="<%=btnLogin.ClientID %>.click()">
                                                <i class="fa fa-sign-in"></i>
                                                Go to Login
                                            </button>
                                            <asp:Button runat="server" Style="display: none;" ID="btnLogin" OnClick="btnLogin_Click"></asp:Button>
                                        </div>
                                    </form>
                                    <div class="text-center">
                                        <p>©2023 All Rights Reserved. PT Yusen Logistics Indonesia. Privacy and Terms</p>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <!--**********************************
    Scripts
    ***********************************-->
    <script src="OtherThemes/quixlab/plugins/common/common.min.js"></script>
    <script src="OtherThemes/quixlab/js/custom.min.js"></script>
    <script src="OtherThemes/quixlab/js/settings.js"></script>
    <script src="OtherThemes/quixlab/js/gleek.js"></script>
    <script src="OtherThemes/quixlab/js/styleSwitcher.js"></script>
</body>
</html>
