namespace Mercury.Web.Library
{
    using System.Web.UI;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [
        ToolboxData("<{0}:TabPage runat=server></{0}:TabPage>"),
        ParseChildren(true),
        PersistChildren(false)
    ]
    public class TabPage : PlaceHolder
    {
        #region private fields
        private string _title = "";
        private ITemplate _tabBody;
        #endregion private fields

        #region public properties
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [
            PersistenceMode(PersistenceMode.InnerProperty),
            DefaultValue(null),
            Browsable(false)
        ]
        public virtual ITemplate TabBody
        {
            get { return _tabBody; }
            set
            {
                _tabBody = value;
            }
        }
        #endregion public properties

        #region not implemented properties
        [Browsable(false)]
        public override bool EnableTheming
        {
            get
            {
                return base.EnableTheming;
            }
            set
            {
                base.EnableTheming = value;
            }
        }

        [Browsable(false)]
        public override bool EnableViewState
        {
            get
            {
                return base.EnableViewState;
            }
            set
            {
                base.EnableViewState = value;
            }
        }

        [Browsable(false)]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }
        #endregion not implemented properties
    }
}
