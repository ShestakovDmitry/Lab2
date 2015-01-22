<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_Calculator.aspx.cs" Inherits="Calc._Calculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <div style="margin-top: 75px; margin-left: 100px; width: 500px; padding: 30px; border:1px solid cadetblue;">
    <div style="font:bolder 20px Arial; align-content:center; color:cadetblue;"> 
        <h1 style="margin-left:0px; margin-top:0px;">Калькулятор</h1>
    </div>
    <form id="form1" runat="server">
    <div style="margin:auto;">
            Начальное значение:&nbsp
            <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" AutoPostBack="True" Width="120px"></asp:TextBox>
            &nbsp&nbsp
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Добавить действие" OnClick="Button1_Click" />
    </div>
    </form>
    </div>
</body>
</html>
