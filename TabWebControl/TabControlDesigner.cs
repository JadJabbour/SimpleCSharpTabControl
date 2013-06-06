namespace TabWebControl
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
            // Initialize the base
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

            //add design regions for all header cells, the region name will be prefixd with 
            // HEADER_PREFIX, and extended with the tab page index. 

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                regions.Add(new DesignerRegion(this, HEADER_PREFIX + i.ToString()));
                i++;
            }

            // Create an editable region and add it to the regions
            // the design region name will take CONTENT_PREFIX as prefix and the index
            // of the current active tab.

            EditableDesignerRegion editableRegion =
                new EditableDesignerRegion(this,
                    CONTENT_PREFIX + tabControl.CurrentDesignTab, false);
            regions.Add(editableRegion);

            // Set the highlight for the selected region
            regions[tabControl.CurrentDesignTab].Highlight = true;

            // Use the base class to render the markup
            return base.GetDesignTimeHtml();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Get a reference to the table, which is the first child control
            Table table = (Table)tabControl.Controls[0];

            // Add design time markers for all cells which represents the tab body
            if (table != null)
            {
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    table.Rows[0].Cells[i].Attributes[DesignerRegion.DesignerRegionAttributeName] = i.ToString();
                }

                //set the editable region
                table.Rows[1].Cells[0].Attributes[DesignerRegion.DesignerRegionAttributeName] = (tabControl.TabPages.Count).ToString();
            }
        }

        protected override void OnClick(DesignerRegionMouseEventArgs e)
        {
            if (e.Region == null)
                return;

            // If the clicked region is not a header, return
            if (e.Region.Name.IndexOf(HEADER_PREFIX) != 0)
                return;

            // Switch the current view if required
            //only when the clicked region is different than the active region.
            if (e.Region.Name.Substring(HEADER_PREFIX.Length) != tabControl.CurrentDesignTab.ToString())
            {
                //extract the index of the design region, and set the CurrentDesignTab index
                tabControl.CurrentDesignTab = int.Parse(e.Region.Name.Substring(HEADER_PREFIX.Length));

                //then after update the design time HTML
                base.UpdateDesignTimeHtml();
            }
        }


        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            // Get a reference to the designer host
            IDesignerHost host = (IDesignerHost)(new Component().Site.GetService(typeof(IDesignerHost)));

            //only if known host, and we have some TabPages
            if (host != null && tabControl.TabPages.Count > 0)
            {
                //we need to get the contents of the TabBody of the tabControl

                ITemplate template = tabControl.TabPages[0].TabBody;


                if (region.Name.StartsWith(CONTENT_PREFIX))
                {
                    //get the template of the selected tab

                    //extract the tab index of the edited region, from the region name.
                    int tabIndex = int.Parse(region.Name.Substring(CONTENT_PREFIX.Length));

                    //switch the design template to the selected tab index.
                    template = tabControl.TabPages[tabIndex].TabBody;
                }

                // Persist the template in the design host
                if (template != null)
                    return ControlPersister.PersistTemplate(template, host);
            }

            return String.Empty;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            if (content == null)
                return;

            // Get a reference to the designer host
            IDesignerHost host = (IDesignerHost)(new Component().Site.GetService(typeof(IDesignerHost)));
            if (host != null)
            {
                // Create a template from the content string
                ITemplate template = ControlParser.ParseTemplate(host, content);

                if (template != null)
                {
                    // Determine which region should get the template
                    if (region.Name.StartsWith(CONTENT_PREFIX))
                    {
                        int tabIndex = int.Parse(region.Name.Substring(CONTENT_PREFIX.Length));
                        //set back the template for the selected tab body
                        tabControl.TabPages[tabIndex].TabBody = template;
                    }
                }
            }
        }
        #endregion implementations
    }
}
