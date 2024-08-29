<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="procurement_system.login" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="Scripts/style-boxlogin.css" rel="stylesheet" />
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script src="vendors/sweetalert.js"></script>
    <script src="vendors/sweetalert.min.js"></script>
    <link rel="icon" href="images/favicon.ico" type="image/x-icon">
    <link href="vendors/sweetalert.css" rel="stylesheet" />
    <script type="text/javascript">
        function SuccessLogin() {
            swal({
                title: 'Login Success',
                text: '<span style=color:grey>WELCOME <label id=txtFullname runat="server"></label></span>',
                timer: '2000',
                type: 'success',
                showConfirmButton: false,
                html: true
            },
                function redirect() {
                    window.location.href = 'index.aspx';
                }
            );
        }
    </script>

    <script type="text/javascript">
        function FailedLogin() {
            swal('Login Failed!', 'Username or Password incorrect, please try again', 'error');
        }
    </script>

    <%--<script type="text/javascript">
        function InfoBox() {
            swal({
                title: '<strong>Perhatian!</strong>',
                text: '<strong>Welcome to Procurement System Test</strong>' +
                    '<br/> ' +
                    '(Hanya untuk uji coba sistem sebelum Live).' +
                    '<br/> ' +
                    '<br/> ' +
                    'Login sama dengan login windows pada PC/Laptop, ' +
                    '<br/> ' +
                    '<strong>Contoh -> username:YLID-NIK, password:(sama dengan password login windows pada pc/laptop).</strong>' +
                    '<br/> ' +
                    'Jika belum bisa login menggunakan login windows,silahkan hubungi IT' +
                    '<br/> ' +
                    '<strong>Ext. IT: 213 or 212</strong>',
                type: 'info',
                showConfirmButton: true,
                html: true
            }
            );
        }
    </script>--%>

    <script type="text/javascript">
        function InfoBox() {
            swal({
                title: '<strong>Perhatian!</strong>',
                text: '<strong>Welcome to Purchasing System Test</strong>' +
                    '<br/> ' +
                    '(Hanya untuk uji coba sistem sebelum Live).' +
                    '<br/> ' +
                    '<br/> ' +
                    'Login sama dengan login windows pada PC/Laptop, ' +
                    '<br/> ' +
                    '<strong>Contoh -> username:YLID-NIK, password:(sama dengan password login windows pada pc/laptop).</strong>' +
                    '<br/> ' +
                    'atau' +
                    '<br/> ' +
                    'Login menggunakan username & password yang dibuat saat <strong>Create account/Register.</strong>' +
                    '<br/> ' +
                    '<br/> ' +
                    'Jika belum bisa login menggunakan 2 langkah diatas,silahkan hubungi IT' +
                    '<br/> ' +
                    '<strong>Ext. IT (JKT): 213 / 212</strong>' +
                    '<br/> ' +
                    '<strong>Ext. IT (CGK): 272 / 273 / 274</strong>',
                type: 'info',
                showConfirmButton: true,
                html: true
            }
            );
        }
    </script>

    <script src="vendors/toastr/jquery.min.js"></script>
    <link href="vendors/toastr/toastr.min.css" rel="stylesheet" />
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

    <title>Purchasing System - YLID | Login</title>

</head>

<body>
    <div id="container" class="container">
        <asp:Panel DefaultButton="btnLogin" runat="server">
            <form runat="server">

                <!-- FORM SECTION -->
                <div class="row">
                    <!-- SIGN UP -->
                    <div class="col align-items-center flex-col sign-up">
                        <div class="form-wrapper align-items-center">
                            <div class="form sign-up">
                                <div class="input-group">
                                    <i class='bx bxs-user'></i>
                                    <input type="text" placeholder="Username">
                                </div>
                                <div class="input-group">
                                    <i class='bx bx-mail-send'></i>
                                    <input type="email" placeholder="Email">
                                </div>
                                <div class="input-group">
                                    <i class='bx bxs-lock-alt'></i>
                                    <input type="password" placeholder="Password">
                                </div>
                                <div class="input-group">
                                    <i class='bx bxs-lock-alt'></i>
                                    <input type="password" placeholder="Confirm password">
                                </div>
                                <button>
                                    Register
                                </button>
                                <p>
                                    <span>Already have an account?
                                    </span>
                                    <b onclick="toggle()" class="pointer">Login here
                                    </b>
                                </p>
                            </div>
                        </div>

                    </div>
                    <!-- END SIGN UP -->
                    <!-- SIGN IN -->

                    <div class="col align-items-center flex-col sign-in">
                        <div class="form-wrapper align-items-center">
                            <h1 style="color: white">System Testing</h1>
                        </div>
                        <div class="form-wrapper align-items-center">
                            <h1 style="color: white">Purchasing - YLID</h1>
                        </div>
                        <div class="form-wrapper align-items-center">
                            &nbsp;
                        </div>

                        <div class="form-wrapper align-items-center">
                            <div class="form sign-in">
                                <br />
                                <div>
                                    <label class="field field_v1">
                                        <input class="field__input" id="txtusername" runat="server" type="text" placeholder="type your IDNIK...">
                                        <span class="field__label-wrap">
                                            <span class="field__label">Username</span>
                                        </span>
                                    </label>
                                </div>

                                <div>
                                    <label class="field field_v1">
                                        <input class="field__input" id="txtpassword" runat="server" type="password" placeholder="type your password...">
                                        <span class="field__label-wrap">
                                            <span class="field__label">Password</span>
                                        </span>
                                    </label>
                                </div>
                                <div>
                                    <input type="checkbox" onchange="document.getElementById('txtpass').type = this.checked ? 'text' : 'password'">
                                    Show password
                                </div>



                                <%--<div class="input-group">
							<input id="txtpass" runat="server" type="password" placeholder="Password" required="" />
						</div>--%>
                                <p>
                                    <b>
                                        <button type="button" class="btn btn-sm btn-success shadow-sm" onclick="<%=btnLogin.ClientID %>.click()">
                                            <i class="fa fa-sign-in"></i>
                                            Login
                                        </button>
                                        <asp:Button runat="server" Style="display: none;" ID="btnLogin" OnClick="btnLogin_Click"></asp:Button>

                                    </b>
                                </p>
                                <p>

                                <%--<p>
                                    <b>
                                        <a href="ForgotPassword.aspx">Forgot Password ? </a>
                                    </b>
                                </p>
                                <p>
                                    <span>Don't have an account?
                                    </span>
                                    <a href="Register.aspx">Create Account</a>


                                </p>--%>
                                <div class="clearfix"></div>
                                <br />
                            </div>
                        </div>
                        <div class="form-wrapper align-items-center">
                            &nbsp;
                        </div>
                        <div class="form-wrapper align-items-center">
                            <a style="color: white">©2023 All Rights Reserved. PT Yusen Logistics Indonesia</a>
                        </div>


                    </div>
                    <%--<marquee>
					<i class="fa fa-bullhorn" style="font-size:30px;color:white">&nbsp</i>
					<Label style="font-size:30px;color:#ffffff";">
						Login sama dengan login windows pada PC/Laptop 
					</Label>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<i class="fa fa-bullhorn" style="font-size:30px;color:white">&nbsp</i>
					<Label style="font-size:30px;color:#ffffff";">
						Contoh -> Username: YLID-NIK , Password : (password sama seperti saat masuk windows di PC/Laptop)
					</Label>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<i class="fa fa-bullhorn" style="font-size:30px;color:white">&nbsp</i>
					<Label style="font-size:30px;color:#ffffff";">
						Jika belum bisa login menggunakan YLID-NIK, segera hubungi IT
					</Label>
				</marquee>--%>
                    <!-- END SIGN IN -->
                </div>

                <!-- END FORM SECTION -->
            </form>
        </asp:Panel>

        <!-- CONTENT SECTION -->
        <div class="row content-row">
            <!-- SIGN IN CONTENT -->
            <div class="col align-items-center flex-col">
                <%--<div class="text sign-in">
					<h2>
						Welcome Back
					</h2>
	
				</div>--%>
                <div class="img sign-in">
                    <img src="images/img-log-yusen.png" class="image" alt="" />
                </div>
            </div>
            <!-- END SIGN IN CONTENT -->
            <!-- SIGN UP CONTENT -->
            <div class="col align-items-center flex-col">
                <div class="img sign-up">
                </div>
                <div class="text sign-up">
                </div>

            </div>
            <!-- END SIGN UP CONTENT -->
        </div>
        <!-- END CONTENT SECTION -->
    </div>

    <script src="Scripts/style-login.js"></script>
    <script src="vendors/sweetalert.js"></script>
    <script src="vendors/sweetalert.min.js"></script>
</body>
</html>
