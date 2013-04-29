using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteClickServer
{
    public class MTListBox : ListBox
    {
        public class ListBoxItem
        {
            private string title_;
            private UInt32 id_;

            public ListBoxItem(string title, UInt32 id)
            {
                title_ = title;
                id_ = id;
            }

            public UInt32 id
            {
                get
                {
                    return id_;
                }
            }
            public override string ToString()
            {
                return title_;
            }
        }

        delegate void AddListBoxItem_(ListBoxItem obj);
        delegate void ClearListBox();
        delegate void SetListDataSource(BindingSource source, string displayMember, string valueMember);
        public MTListBox()
        {
        }
        public void SetDataSource(BindingSource source, string displayMember = "", string valueMember = "")
        {
            if (this.InvokeRequired)
            {
                SetListDataSource dlgt = new SetListDataSource(SetDataSource);
                Invoke(dlgt, new object[] { source, displayMember, ValueMember });
            }
            else
            {
                if (source == null)
                    this.DataSource = null;
                else
                    this.DataSource = source.DataSource;

                this.DisplayMember = displayMember;
                this.ValueMember = ValueMember;

            }
        }
        public void Clear()
        {
            if (this.InvokeRequired)
            {
                ClearListBox dlgt = new ClearListBox(Clear);
                Invoke(dlgt);
            }
            else
            {
                this.Items.Clear();
            }
        }
        public void AddItem(ListBoxItem obj)
        {
            if (this.InvokeRequired)
            {
                AddListBoxItem_ dlgt = new AddListBoxItem_(AddItem);
                Invoke(dlgt, new object[] { obj });
            }
            else
            {
                this.Items.Add(obj);
            }
        }

    }

}
