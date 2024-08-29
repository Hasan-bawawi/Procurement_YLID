<%@ Page Title="Home" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="procurement_system.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-head {
            background-color: #06183d; /* Change this to your desired background color */
            color: white; /* Text color for the header */
            padding: 20px 2.938rem; /* Optional padding for the header */
            height: 88px;
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
            <h2 class="page-header text-center" style="color: white;">Welcome to Purchasing System - YLID</h2>
            <h5 class="page-header text-center" style="color: white;">&nbsp;</h5>
        </div>
    </div>
    <asp:Image ID="Image1" runat="server" Width="100%" Height="100%" ImageUrl="~/images/Background_Icon20210401.png" />
</asp:Content>
