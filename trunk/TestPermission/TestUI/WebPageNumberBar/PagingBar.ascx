<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagingBar.ascx.cs" Inherits="PagingBar" %>

<div style="font-size:9pt; margin-top:3px">��[<SPAN   
id="lblCP" runat="server">0</SPAN>]����¼ &nbsp;<asp:LinkButton ID="btnFirsh" runat="server" CausesValidation="False" OnClick="btnFirsh_Click"
        Text="��ҳ"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton id="btnUp" onclick="btnUp_Click" Text="��һҳ" runat="server" CausesValidation="False"></asp:LinkButton> 
&nbsp; &nbsp;<asp:LinkButton id="btnNext" onclick="btnNext_Click" Text="��һҳ" runat="server" CausesValidation="False" ></asp:LinkButton>&nbsp; &nbsp;<asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" OnClick="btnLast_Click"
        Text="ĩҳ"></asp:LinkButton>&nbsp;
        
  <asp:TextBox runat="server" id="txtPage" Width="25px"  Height="10px" ></asp:TextBox>/<asp:Label
     ID="lblPage" runat="server" Text="0"></asp:Label>ҳ
    <div style="display:none">
         <asp:Button ID="btnGo" onclick="btnGo_Click" runat="server" CausesValidation="False" Width="0px"/>
    </div>
    </div>
    <script language="javascript" type="text/javascript">
        btnGo=document.getElementById("<%=btnGo.ClientID %>");
    </script>