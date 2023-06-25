namespace Flextronics.Applications.Library.Utility
{
    using System;
    using System.Windows.Forms;

    public class TemplateNode : TreeNode
    {
        private string filepath;
        private string nodeid;
        private string nodetype;
        private string parentid;

        public TemplateNode()
        {
        }

        public TemplateNode(string NodeName)
        {
            base.Text = NodeName;
        }

        public string FilePath
        {
            get
            {
                return this.filepath;
            }
            set
            {
                this.filepath = value;
            }
        }

        public string NodeID
        {
            get
            {
                return this.nodeid;
            }
            set
            {
                this.nodeid = value;
            }
        }

        public string NodeType
        {
            get
            {
                return this.nodetype;
            }
            set
            {
                this.nodetype = value;
            }
        }

        public string ParentID
        {
            get
            {
                return this.parentid;
            }
            set
            {
                this.parentid = value;
            }
        }
    }
}

