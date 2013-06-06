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
        #endregion public properties

        #region private methods
        private void BuildTitles(Table tabControlTable)
        {
            TableRow titlesRow = new TableRow();
            titlesRow.HorizontalAlign = HorizontalAlign.Center;

            int i = 0;
            foreach (TabPage tabPage in _tabPages)
            {
                TableCell tabTitleCell = new TableCell();
                tabTitleCell.Text = tabPage.Title;
                tabTitleCell.Width = new Unit("");
                tabTitleCell.BorderStyle = BorderStyle.Outset;
                tabTitleCell.BorderWidth = new Unit("2");
                tabTitleCell.Style["padding"] = "0px 4px 0px 4px";
                tabTitleCell.Style["cursor"] = "hand";
                tabTitleCell.Wrap = false;
                tabTitleCell.Height = new Unit("20");
                if (!DesignMode)
                {
                    if (_selectedTab == i)
                    {
                        tabTitleCell.Style["background-color"] = ColorTranslator.ToHtml(Color.DarkGray);
                    }
                }

                tabTitleCell.Attributes.Add("onclick", "ShowTab(this, " + i.ToString() + ")");

                titlesRow.Cells.Add(tabTitleCell);
                i++;
            }

            TableCell tc1 = new TableCell();
            tc1.Width = new Unit("100%");
            tc1.Height = new Unit("20");
            titlesRow.Cells.Add(tc1);
            titlesRow.Height = new Unit("20");
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
                        contentRow.Style["display"] = "block";
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
            tc.ColumnSpan = _tabPages.Count + 1;
            tc.BackColor = Color.White;
            tc.BorderWidth = new Unit("1");
            tc.BorderStyle = BorderStyle.Ridge;
            tc.BorderColor = Color.Silver;
            tc.Style["padding"] = "5px 5px 5px 5px";
            tc.Height = new Unit("100%");

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

            Table tabControlTable = new Table();
            tabControlTable.CellSpacing = 1;
            tabControlTable.CellPadding = 0;
            tabControlTable.BorderStyle = BorderStyle;
            tabControlTable.Width = this.Width;
            tabControlTable.Height = this.Height;
            tabControlTable.BackColor = ColorTranslator.FromHtml("inactiveborder");

            tabControlTable.Attributes.Add("ActiveTabIdx", _selectedTab.ToString());

            BuildTitles(tabControlTable);

            BuildContentRows(tabControlTable);

            Controls.Add(tabControlTable);
        }

        #endregion implementations
    }
}
