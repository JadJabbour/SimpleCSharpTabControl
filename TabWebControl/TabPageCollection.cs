namespace TabWebControl
{
    using System;
    using System.Collections;
    using System.Drawing.Design;
    using System.ComponentModel;
    using System.Security.Permissions;

    [
        Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)),
        PermissionSet(SecurityAction.LinkDemand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\r\n version=\"1\">\r\n <IPermission class=\"System.Web.AspNetHostingPermission, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n version=\"1\"\r\n Level=\"Minimal\"/>\r\n</PermissionSet>\r\n")
    ]
    public class TabPageCollection : CollectionBase
    {
        #region Constructor
        public TabPageCollection()
        {
        }
        #endregion Constructor

        #region Indexer
        public TabPage this[object index]
        {
            get
            {
                int ind = IndexOf(index);
                if (ind < 0)
                    throw new Exception("Out Of Range");
                return (TabPage)this.List[ind];
            }
            set
            {
                this.List[IndexOf(index)] = value;
            }
        }
        #endregion Indexer

        #region Public Methods
        public void Add(TabPage tapPage)
        {
            this.List.Add(tapPage);
        }

        public void Insert(int index, TabPage tapPage)
        {
            this.List.Insert(index, tapPage);
        }

        public void Remove(TabPage tapPage)
        {
            this.List.Remove(tapPage);
        }

        public bool Contains(TabPage tapPage)
        {
            return this.List.Contains(tapPage);
        }

        public int IndexOf(object obj)
        {
            if (obj is int)
                return (int)obj;

            if (obj is string)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    if (((TabPage)List[i]).ID.ToUpper() == obj.ToString().ToUpper())
                        return i;
                }
                return -1;
            }
            else
            {
                throw new Exception("Invalid Index Value");
            }
        }

        public void CopyTo(TabPage[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public bool Contains(string key)
        {
            return List.Contains(key);
        }

        public void Remove(string key)
        {
            List.Remove(key);
        }
        #endregion Public Methods
    }
}
