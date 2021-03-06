﻿namespace TabWebControl
{
    using System;
    using System.Web.UI;
    using System.ComponentModel;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Security.Permissions;
    using System.ComponentModel.Design;
    using System.Web.UI.Design.WebControls;

    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    class TabControlDesigner : CompositeControlDesigner
    {
        #region privates
        private const string HEADER_PREFIX = "Header";
        private const string CONTENT_PREFIX = "Content";
        TabControl tabControl;
        #endregion privates

        #region implementations
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            tabControl = (TabControl)component;
        }

        public override bool AllowResize
        {
            get
            {
                return true;
            }
        }

        public override String GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            int i = 0;
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                regions.Add(new DesignerRegion(this, HEADER_PREFIX + i.ToString()));
                i++;
            }

            EditableDesignerRegion editableRegion =
                new EditableDesignerRegion(this,
                    CONTENT_PREFIX + tabControl.CurrentDesignTab, false);
            regions.Add(editableRegion);
            regions[tabControl.CurrentDesignTab].Highlight = true;
            return base.GetDesignTimeHtml();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Table table = (Table)tabControl.Controls[0];

            if (table != null)
            {
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    table.Rows[0].Cells[i].Attributes[DesignerRegion.DesignerRegionAttributeName] = i.ToString();
                }

                table.Rows[1].Cells[0].Attributes[DesignerRegion.DesignerRegionAttributeName] = (tabControl.TabPages.Count).ToString();
            }
        }

        protected override void OnClick(DesignerRegionMouseEventArgs e)
        {
            if (e.Region == null)
                return;

            if (e.Region.Name.IndexOf(HEADER_PREFIX) != 0)
                return;

            if (e.Region.Name.Substring(HEADER_PREFIX.Length) != tabControl.CurrentDesignTab.ToString())
            {
                tabControl.CurrentDesignTab = int.Parse(e.Region.Name.Substring(HEADER_PREFIX.Length));

                base.UpdateDesignTimeHtml();
            }
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            IDesignerHost host = (IDesignerHost)(new Component().Site.GetService(typeof(IDesignerHost)));

            if (host != null && tabControl.TabPages.Count > 0)
            {
                ITemplate template = tabControl.TabPages[0].TabBody;

                if (region.Name.StartsWith(CONTENT_PREFIX))
                {
                    int tabIndex = int.Parse(region.Name.Substring(CONTENT_PREFIX.Length));
                    template = tabControl.TabPages[tabIndex].TabBody;
                }

                if (template != null)
                    return ControlPersister.PersistTemplate(template, host);
            }
            return String.Empty;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            if (content == null)
                return;

            IDesignerHost host = (IDesignerHost)(new Component().Site.GetService(typeof(IDesignerHost)));
            if (host != null)
            {
                ITemplate template = ControlParser.ParseTemplate(host, content);

                if (template != null)
                {
                    if (region.Name.StartsWith(CONTENT_PREFIX))
                    {
                        int tabIndex = int.Parse(region.Name.Substring(CONTENT_PREFIX.Length));
                        tabControl.TabPages[tabIndex].TabBody = template;
                    }
                }
            }
        }
        #endregion implementations
    }
}
