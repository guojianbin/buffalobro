<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManager.aspx.cs" Inherits="ManagerPage_UserManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <style type="text/css">
<!--
body {
	margin-left: 3px;
	margin-top: 0px;
	margin-right: 3px;
	margin-bottom: 0px;
}
.STYLE1 {
	color: #e1e2e3;
	font-size: 12px;
}
.STYLE6 {color: #000000; font-size: 12; }
.STYLE10 {color: #000000; font-size: 12px; }
.STYLE19 {
	color: #344b50;
	font-size: 12px;
}
.STYLE21 {
	font-size: 12px;
	color: #3b6375;
}
.STYLE22 {
	font-size: 12px;
	color: #295568;
}
-->
</style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="30"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="24" bgcolor="#353c44"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="6%" valign="bottom" style="height: 19px"><div align="center">
                <img src="../images/tb.gif" width="14" height="14" /></div></td>
                <td width="94%" valign="bottom" style="height: 19px"><span class="STYLE1"> 管理人员基本信息列表</span></td>
              </tr>
            </table></td>
            <td><div align="right"><span class="STYLE1">
              <input type="checkbox" name="checkbox11" id="checkbox11" />
              全选      &nbsp;&nbsp;<img src="../images/add.gif" width="10" height="10" /> 添加   &nbsp;
               <span class="STYLE1"> &nbsp;</span></div></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td>
    <div>
        <span runat="server" id="spnName">
            <asp:Label ID="labName" runat="server" Text="名称"></asp:Label><asp:TextBox ID="txtName"
                runat="server"></asp:TextBox>
         </span>
             <span runat="server" id="spnBelongClass">
            <asp:Label ID="labBelongClass" runat="server" Text="班级:"></asp:Label><asp:TextBox ID="txtBelongClass"
                runat="server"></asp:TextBox>
         </span>   
    </div>
        <asp:GridView ID="gvDisplay" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="姓名" >
                    <HeaderStyle CssClass="STYLE6" />
                </asp:BoundField>
                <asp:BoundField HeaderText="所属班级" />
            </Columns>
        </asp:GridView>
        <div>共 页</div>
    </td>
  </tr>
  
</table>
    </form>
</body>
</html>
