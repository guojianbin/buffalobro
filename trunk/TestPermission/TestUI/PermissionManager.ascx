<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PermissionManager.ascx.cs" Inherits="PermissionManager" %>
<asp:Repeater ID="Repeater1" runat="server">
<HeaderTemplate>
    <table>
</HeaderTemplate>
<ItemTemplate>
    <tr>
        <td>
            <asp:Label ID="labName" runat="server" Text="Label"></asp:Label>
        </td>
        <td>
        <asp:RadioButtonList ID="rbModelRight" runat="server" RepeatColumns="2" >
        </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            
        </td>
        <td>
            <asp:Repeater ID="Repeater1" runat="server">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                    <td>
                        <asp:Label ID="labName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                    <asp:RadioButtonList ID="rbModelItemRight" runat="server" RepeatColumns="2" >
                    </asp:RadioButtonList>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
            </asp:Repeater>
        </td>
    </tr>
</ItemTemplate>
<FooterTemplate>
    </table>
</FooterTemplate>
</asp:Repeater>
