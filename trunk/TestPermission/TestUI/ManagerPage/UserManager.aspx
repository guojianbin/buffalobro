﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManager.aspx.cs" Inherits="ManagerPage_UserManager" %>

<%@ Register Src="../WebPageNumberBar/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link type="text/css" href="../css/thickbox.css" rel="stylesheet" />	
	<script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="../js/jquery-ui-1.7.2.custom.min.js"></script>
	<script type="text/javascript" src="../js/jquery-contextmenu-r2.js"></script>
	<script src="../js/thickbox.js" type="text/javascript"></script>
    <style type="text/css">
<!--
body {
	margin-left: 3px;
	margin-top: 0px;
	margin-right: 3px;
	margin-bottom: 0px;
}

-->
</style><script type="text/javascript">
function showdivDialog(id)
{
    tb_show('明细', "UserEdit.aspx?id="+id+"&keepThis=true&amp;TB_iframe=true&amp;height=460&amp;width=950", false);
}
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size:12px">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="30"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="24" bgcolor="#353c44"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td style="height: 31px"><table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="6%" valign="bottom" style="height: 19px"><div align="center">
                <img src="../images/tb.gif" width="14" height="14" /></div></td>
                <td width="94%" valign="bottom" style="height: 19px"><span style="color:White">管理人员基本信息列表</span></td>
              </tr>
            </table></td>
            <td style="height: 31px"><div align="right"><span class="STYLE1">&nbsp;&nbsp;&nbsp;<img src="../images/add.gif" width="10" height="10" /> 添加   &nbsp;
               <span> &nbsp;</span></div></td>
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
            <asp:Label ID="labBelongClass" runat="server" Text="班级:"></asp:Label>
                 <asp:DropDownList ID="ddlBelongClass" runat="server">
                 </asp:DropDownList>
                 <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" /></span></div>
        <asp:GridView ID="gvDisplay" runat="server" AutoGenerateColumns="False"
         Width="100%" ShowFooter="True" HeaderStyle-BackColor="#d3eaef" 
         RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
            <Columns>
            <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%#GetDataIndex(Container.DataItemIndex,pb.CurrentPage) %>'></asp:Label>
                    </ItemTemplate>
                    
                    <FooterTemplate>
                        Σ
                    </FooterTemplate>
                <ItemStyle Width="20px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="姓名">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                    </ItemTemplate>
                    
                    
                </asp:TemplateField>
                <asp:TemplateField HeaderText="年龄">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Age") %>'></asp:Label>
                    </ItemTemplate>
                    
                        <FooterTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%#GetSum("Age") %>'></asp:Label>
                        
                    </FooterTemplate>
                    
                </asp:TemplateField>
                <asp:TemplateField HeaderText="所属班级">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("BelongClass") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <a href="javascript:void(0);" onclick="showdivDialog();">编辑</a>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                    <span onclick="return confirm('是否删除此项？')">
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" CommandName="DoDelete"
                            Text="删除"></asp:LinkButton></span>
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>
            </Columns>
            <RowStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
            <HeaderStyle BackColor="#D3EAEF" />
        </asp:GridView>
        
        
        
        <div>
            <uc1:PagingBar ID="pb" runat="server" /></div>
    </td>
  </tr>
  
</table></div>
    </form>
</body>
</html>
