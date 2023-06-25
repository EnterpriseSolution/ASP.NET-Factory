namespace Flextronics.Applications.ApplicationFactory
{
    using System;
    using System.Windows.Forms;

    public static class FormCommon
    {
        public static string GetDbViewSelServer()
        {
            if (Application.OpenForms["DbView"] == null)
            {
                return "";
            }
            DbView view = (DbView) Application.OpenForms["DbView"];
            TreeNode selectedNode = view.treeView1.SelectedNode;
            if (selectedNode == null)
            {
                return "";
            }
            switch (selectedNode.Tag.ToString())
            {
                case "serverlist":
                    return "";

                case "server":
                    return selectedNode.Text;

                case "db":
                    return selectedNode.Parent.Text;

                case "tableroot":
                case "viewroot":
                    return selectedNode.Parent.Parent.Text;

                case "table":
                case "view":
                    return selectedNode.Parent.Parent.Parent.Text;

                case "column":
                    return selectedNode.Parent.Parent.Parent.Parent.Text;
            }
            return "";
        }

        public static DbView DbViewForm
        {
            get
            {
                if (Application.OpenForms["DbView"] == null)
                {
                    return null;
                }
                return (DbView) Application.OpenForms["DbView"];
            }
        }
    }
}

