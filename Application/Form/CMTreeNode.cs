namespace Codematic
{
    using System;
    using System.Windows.Forms;

    public class CMTreeNode : TreeNode
    {
        public string dbname;
        public string dbtype;
        public string nodeName;
        public string nodetype;
        public string server;

        public CMTreeNode(string nodeName, string nodetype, string server, string dbname, string dbtype)
        {
            base.Text = nodeName;
            this.nodeName = nodeName;
            this.nodetype = nodetype;
            this.server = server;
            this.dbname = dbname;
            this.dbtype = dbtype;
        }

        public enum NodeType
        {
            Database = 2,
            Empty = -1,
            Filed = 11,
            Function = 10,
            FunctionRoot = 6,
            Project = 14,
            Server = 1,
            ServerList = 0,
            StoredProcedure = 9,
            StoredProcedureRoot = 5,
            Table = 7,
            TableRoot = 3,
            Trigger = 13,
            Triggers = 12,
            Unknown = 15,
            View = 8,
            ViewRoot = 4
        }
    }
}

