<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpList.aspx.cs" Inherits="AKSS_Management.EmpList" 
    EnableEventValidation="false" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Employee List : 
            <asp:Button ID="btnAddEmp" runat="server" Text="Add (+)" OnClick="btnAddEmp_Click"  style="align-items:flex-end" />
        </div>
        <div>

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged"                
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnGridView1_RowCommand="GridView1_RowCommand"
                EmptyDataText="Data Not Present"
                AllowPaging="true"
                AllowSorting="true" PageSize="10" ShowFooter="false" DataKeyNames="Id"
                ShowHeader="true" Width="50%">
                <%--OnRowDataBound="OnRowDataBound"--%>
                <%--SelectedIndexChanging="GridView1_SelectedIndexChanging" AutoGenerateSelectButton="true" OnRowCreated="GridView1_RowCreated" --%>
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="role" HeaderText="Role" />
                    <asp:BoundField DataField="salary" HeaderText="Salary" />
                    
                     <asp:TemplateField HeaderText="Action" ItemStyle-Width="120">
                        <ItemTemplate>
                            <asp:Button Text="Select" runat="server" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" />
                            <%--<asp:Button  runat="server" Text="Edit" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" ItemStyle-Width="150" ></asp:Button>--%>
                            <%--<asp:Button runat="server" Text="Delete" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" ItemStyle-Width="150" ></asp:Button>--%>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

            <%--  <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="Customer Id" ItemStyle-Width="80" />
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="120" />
        <asp:TemplateField HeaderText="Country" ItemStyle-Width="120">
            <ItemTemplate>
                <asp:DropDownList ID = "ddlCountries" runat="server" AutoPostBack = "true" OnSelectedIndexChanged = "SelectedIndexChanged" SelectedValue='<%# Eval("Country") %>'>
                    <asp:ListItem Text = "United States" Value = "United States"></asp:ListItem>
                    <asp:ListItem Text = "India" Value = "India"></asp:ListItem>
                    <asp:ListItem Text = "France" Value = "France"></asp:ListItem>
                    <asp:ListItem Text = "Russia" Value = "Russia"></asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>
        </div>
    </form>
</body>
</html>
