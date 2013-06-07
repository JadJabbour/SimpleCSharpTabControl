namespace TabWebControl
{
    using System;
    using System.Web.UI;
    using System.Drawing;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [
        ParseChildren(true, "TabPages"),
        ToolboxData("<{0}:TabControl runat=\"server\" ></{0}:TabControl>"),
        PersistChildren(false),
        Designer(typeof(TabControlDesigner))
    ]
    public class TabControl : CompositeControl
    {
        #region private fields
        private TabPageCollection _tabPages;
        private int _currentDesignTab;
        private int _selectedTab;
        private string _tabStyle;
        private string _tabBodyStyle;
        private string _tabActiveStyle;
        #endregion private fields

        #region public properties
        [
            PersistenceMode(PersistenceMode.InnerProperty),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            MergableProperty(false)
        ]
        public TabPageCollection TabPages
        {
            get
            {
                if (_tabPages == null)
                {
                    _tabPages = new TabPageCollection();
                }
                return _tabPages;
            }
        }

        [
            Browsable(false),
            PersistenceMode(PersistenceMode.InnerProperty),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public int CurrentDesignTab
        {
            get { return _currentDesignTab; }
            set { _currentDesignTab = value; }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; }
        }

        public string TabStyle
        {
            get { return _tabStyle; }
            set { _tabStyle = value; }
        }

        public string TabBodyStyle
        {
            get { return _tabBodyStyle; }
            set { _tabBodyStyle = value; }
        }

        public string TabActiveStyle
        {
            get { return _tabActiveStyle; }
            set { _tabActiveStyle = value; }
        }
        #endregion public properties

        #region private methods
        private void BuildTitles(Table tabControlTable)
        {
            TableRow titlesRow = new TableRow();
            titlesRow.HorizontalAlign = HorizontalAlign.Center;
            titlesRow.CssClass = this._tabStyle;

            int i = 0;
            foreach (TabPage tabPage in _tabPages)
            {
                TableCell tabTitleCell = new TableCell();
                tabTitleCell.Text = tabPage.Title;
                if (!DesignMode)
                {
                    if (_selectedTab == i)
                    {
                        tabTitleCell.CssClass = this._tabActiveStyle;
                    }
                }
                tabTitleCell.Attributes.Add("onclick", "_A871F03706FC449A8264C5E76F46A779(this, " + i.ToString() + ", '" + this._tabActiveStyle + "', '" + this._tabStyle + "')");
                titlesRow.Cells.Add(tabTitleCell);
                i++;
            }

            TableCell tc1 = new TableCell();
            tc1.Width = new Unit("100%");

            titlesRow.Cells.Add(tc1);
            tabControlTable.Rows.Add(titlesRow);
        }

        private void BuildContentRows(Table tabControlTable)
        {
            if (DesignMode)
            {
                TableRow contentRow = new TableRow();
                TableCell contentCell = BuildContentCell(contentRow);
                _tabPages[_currentDesignTab].TabBody.InstantiateIn(contentCell);
                tabControlTable.Rows.Add(contentRow);
            }
            else
            {
                int counter = 0;
                foreach (TabPage tabPage in _tabPages)
                {
                    TableRow contentRow = new TableRow();
                    TableCell contentCell = BuildContentCell(contentRow);
                    if (tabPage.TabBody != null)
                    {
                        tabPage.TabBody.InstantiateIn(contentCell);
                    }

                    if (_selectedTab == counter)
                    {
                        contentRow.Style["display"] = "";
                    }
                    else
                    {
                        contentRow.Style["display"] = "none";
                    }
                    contentRow.Cells.Add(contentCell);
                    tabControlTable.Rows.Add(contentRow);

                    counter++;
                }
            }
        }

        private TableCell BuildContentCell(TableRow tableRow)
        {
            TableCell tc = new TableCell();
            tc.Height = new Unit("100%");
            tc.ColumnSpan = _tabPages.Count + 1;
            tc.CssClass = this._tabBodyStyle;

            tableRow.Cells.Add(tc);

            return tc;
        }
        #endregion private methods

        #region implementations
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DesignMode)
            {
                _tabPages[_currentDesignTab].TabBody.InstantiateIn(this);
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            string
            scriptText = "function _A871F03706FC449A8264C5E76F46A779(tabTitleCell, idp, activeStyle, tabStyle){";
            scriptText += "var tabsTable = tabTitleCell.parentElement.parentElement.parentElement;";
            scriptText += "var activeTabId = Number(tabsTable.getAttribute('ActiveTabID'));";
            scriptText += "tabsTable.rows[0].cells[activeTabId].className = tabStyle;";
            scriptText += "tabsTable.rows[0].cells[idp].className = activeStyle;";
            scriptText += "tabsTable.rows[activeTabId + 1].style.display = 'none';";
            scriptText += "tabsTable.rows[idp + 1].style.display = '';";
            scriptText += "tabsTable.setAttribute('ActiveTabID', idp);}";

            Table tabControlTable = new Table();
            tabControlTable.CellSpacing = 1;
            tabControlTable.CellPadding = 0;
            tabControlTable.CssClass = this.CssClass;

            tabControlTable.Attributes.Add("ActiveTabID", _selectedTab.ToString());
            BuildTitles(tabControlTable);
            BuildContentRows(tabControlTable);
            Controls.Add(tabControlTable);
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(this.GetType(), ClientID, scriptText, true);
        }
        #endregion implementations
    }
}
