<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpDemo.aspx.cs" Inherits="AKSS_Management.EmpDemo"
    EnableEventValidation="false" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="align-items:center">
        <div>
            <asp:Label ID="lblEmpId" runat="server" Text="Emp Id : "></asp:Label>
            <asp:TextBox ID="txtEmpId" runat="server" placeholder="Emp Id " Text="" Enabled="false"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblName" runat="server" Text="Name : "></asp:Label>
            <asp:TextBox ID="txtName" runat="server" placeholder="Enter name" ></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblRole" runat="server" Text="Role : "></asp:Label>
            <asp:TextBox ID="txtRole" runat="server" placeholder="Enter role"></asp:TextBox>
        </div>
          <div>
              <asp:Label ID="lblSal" runat="server" Text="Salary : "></asp:Label>
              <asp:TextBox ID="txtSal" runat="server" placeholder="Enter Salary"></asp:TextBox>
          </div>
        <div>
            <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_OnClick" /> &nbsp;
            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_OnClick"  /> &nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_OnClick" /> &nbsp;
            <asp:Button ID="btnGetEmpData" runat="server" Text="Get Employee List" OnClick="btnGetEmpData_OnClick" Visible="false" /> &nbsp;
            <asp:Button ID="btnSendMessage" runat="server" Text="Send SMS" OnClick="btnSendMessage_Click" />
            <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" Visible="false" />
            <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" Visible="false" />
        </div>

       
      
    </form>
</body>
</html>
