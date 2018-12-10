<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RecruitmentQnA.AgentChat.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>

<body>
    <form runat="server">
        <div class="container">
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
                <div class="panel panel-info" style="margin-top: 100px">
                    <div class="panel-heading">Login</div>
                    <div class="panel-body">
                        <div class="form-group">

                            <input type="text" class="form-control" id="lblUsername" runat="server">
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnLogin" CssClass="btn btn-primary" runat="server" Text="Login" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4"></div>
        </div>
    </form>
</body>

</html>
