<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="ManagerPage_UserEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
        .lab
        {
            text-align:right;
            width:50%
        }
        .txt
        {
            text-align:left
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <table style=" border-width:1px; border-style:solid; width:600px">
    <tr>
        <td colspan="2" style=" text-align:center; background-color:#d3eaef">用户编辑</td>
    </tr>
        <tr>
            <td class="lab">用户名:</td>
            <td class="txt">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
