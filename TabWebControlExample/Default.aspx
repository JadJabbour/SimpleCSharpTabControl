<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TabWebControlExample.Default" %>
<%@ Register Assembly="TabWebControl" Namespace="TabWebControl" TagPrefix="ctl" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
            <ctl:TabControl ID="MyTabControl1" runat="server" SelectedTab="0" CssClass="" TabStyle="" TabActiveStyle="" TabBodyStyle="" >
                <TabPages>
                    <ctl:TabPage ID="MyTabPage1" runat="server" Title="Tab 1">
                        <TabBody>
                            <asp:Label ID="Label1" runat="server" Text="Tab 1 Body"></asp:Label>
                        </TabBody>
                    </ctl:TabPage>
                    <ctl:TabPage ID="MyTabPage2" runat="server" Title="Tab 2">
                        <TabBody>
                            <asp:Label ID="Label2" runat="server" Text="Tab 2 Body"></asp:Label>
                        </TabBody>
                    </ctl:TabPage>
                    <ctl:TabPage ID="MyTabPage3" runat="server" Title="Tab 3">
                        <TabBody>
                            <asp:Label ID="Label3" runat="server" Text="Tab 3 Body"></asp:Label>
                        </TabBody>
                    </ctl:TabPage>
                    <ctl:TabPage ID="MyTabPage4" runat="server" Title="Tab 4">
                        <TabBody>
                            <asp:Label ID="Label4" runat="server" Text="Tab 4 Body"></asp:Label>
                        </TabBody>
                    </ctl:TabPage>
                </TabPages>
            </ctl:TabControl>
    </form>
</body>
</html>
