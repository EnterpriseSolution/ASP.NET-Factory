using Flextronics.Applications.ApplicationFactory.UserControls;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Flextronics.Applications.Library.Utility;


namespace Flextronics.Applications.ApplicationFactory
{
   
    public class FrmTemplateView : Form
    {
        private ToolStripButton btn_NewFile;
        private ToolStripButton btn_NewFolder;
        private ToolStripButton btn_Refrush;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private DataSet ds;
        private ImageList imageList1;
        private AppSettings settings;
        private string tempfilepath = "temptree.xml";
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private TemplateNode TreeClickNode;
        private TreeView treeView1;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private ToolStripMenuItem 模版ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 刷新ToolStripMenuItem;
        private ToolStripMenuItem 文件夹ToolStripMenuItem;
        private ToolStripMenuItem 新建ToolStripMenuItem;
        private ToolStripMenuItem 重命名ToolStripMenuItem;

        public FrmTemplateView()
        {
            this.InitializeComponent();
            this.settings = AppConfig.GetSettings();
            this.LoadTreeview();
        }

        public void CreateNode(int parentid, TreeNode parentnode, DataTable dt)
        {
            foreach (DataRow row in dt.Select("ParentID= " + parentid))
            {
                string s = row["NodeID"].ToString();
                string nodeName = row["Text"].ToString();
                string str3 = row["FilePath"].ToString();
                string str4 = row["NodeType"].ToString();
                TemplateNode node = new TemplateNode(nodeName);
                node.NodeID = s;
                node.NodeType = str4;
                node.FilePath = str3;
                if (str4 == "folder")
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                }
                parentnode.Nodes.Add(node);
                int num = int.Parse(s);
                this.CreateNode(num, node, dt);
            }
        }

        private void CreatMenu(string NodeType)
        {
            string str = NodeType;
            if (str != null)
            {
                if (!(str == "folder"))
                {
                    if (!(str == "file"))
                    {
                        return;
                    }
                }
                else
                {
                    this.打开ToolStripMenuItem.Enabled = false;
                    this.新建ToolStripMenuItem.Visible = true;
                    return;
                }
                this.打开ToolStripMenuItem.Enabled = true;
                this.新建ToolStripMenuItem.Visible = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetMaxNodeID(DataTable dt)
        {
            int num = 1;
            foreach (DataRow row in dt.Rows)
            {
                string s = row["NodeID"].ToString();
                if (num < int.Parse(s))
                {
                    num = int.Parse(s);
                }
            }
            int num2 = num + 1;
            return num2.ToString();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmTemplateView));
            TreeNode node = new TreeNode("默认", 2, 2);
            TreeNode node2 = new TreeNode("实体类", new TreeNode[] { node });
            TreeNode node3 = new TreeNode("C#代码");
            TreeNode node4 = new TreeNode("VB代码");
            TreeNode node5 = new TreeNode("页面");
            TreeNode node6 = new TreeNode("代码模版", new TreeNode[] { node2, node3, node4, node5 });
            this.toolStrip1 = new ToolStrip();
            this.btn_NewFolder = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btn_NewFile = new ToolStripButton();
            this.btn_Refrush = new ToolStripButton();
            this.treeView1 = new TreeView();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.新建ToolStripMenuItem = new ToolStripMenuItem();
            this.文件夹ToolStripMenuItem = new ToolStripMenuItem();
            this.模版ToolStripMenuItem = new ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new ToolStripMenuItem();
            this.删除ToolStripMenuItem = new ToolStripMenuItem();
            this.重命名ToolStripMenuItem = new ToolStripMenuItem();
            this.imageList1 = new ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
//            this.toolStrip1.BackgroundImage = (Image) manager.GetObject("toolStrip1.BackgroundImage");
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btn_NewFolder, this.toolStripSeparator1, this.btn_NewFile, this.btn_Refrush });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0xab, 0x19);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.btn_NewFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
//            this.btn_NewFolder.Image = (Image) manager.GetObject("btn_NewFolder.Image");
            this.btn_NewFolder.ImageTransparentColor = Color.Magenta;
            this.btn_NewFolder.Name = "btn_NewFolder";
            this.btn_NewFolder.Size = new Size(0x17, 0x16);
            this.btn_NewFolder.Text = "toolStripButton1";
            this.btn_NewFolder.ToolTipText = "新建文件夹";
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btn_NewFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
//            this.btn_NewFile.Image = (Image) manager.GetObject("btn_NewFile.Image");
            this.btn_NewFile.ImageTransparentColor = Color.Magenta;
            this.btn_NewFile.Name = "btn_NewFile";
            this.btn_NewFile.Size = new Size(0x17, 0x16);
            this.btn_NewFile.Text = "toolStripButton2";
            this.btn_NewFile.ToolTipText = "新建模版文件";
            this.btn_Refrush.DisplayStyle = ToolStripItemDisplayStyle.Image;
//            this.btn_Refrush.Image = (Image) manager.GetObject("btn_Refrush.Image");
            this.btn_Refrush.ImageTransparentColor = Color.Magenta;
            this.btn_Refrush.Name = "btn_Refrush";
            this.btn_Refrush.Size = new Size(0x17, 0x16);
            this.btn_Refrush.Text = "toolStripButton3";
            this.btn_Refrush.ToolTipText = "刷新";
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new Point(0, 0x19);
            this.treeView1.Name = "treeView1";
            node.ImageIndex = 2;
            node.Name = "节点6";
            node.SelectedImageIndex = 2;
            node.Text = "默认";
            node2.Name = "节点4";
            node2.Text = "实体类";
            node3.Name = "节点2";
            node3.Text = "C#代码";
            node4.Name = "节点5";
            node4.Text = "VB代码";
            node5.Name = "节点3";
            node5.Text = "页面";
            node6.Name = "节点0";
            node6.Text = "代码模版";
            this.treeView1.Nodes.AddRange(new TreeNode[] { node6 });
            this.treeView1.SelectedImageIndex = 1;
            this.treeView1.Size = new Size(0xab, 0x157);
            this.treeView1.TabIndex = 1;
            this.treeView1.DragDrop += new DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseUp += new MouseEventHandler(this.treeView1_MouseUp);
            this.treeView1.ItemDrag += new ItemDragEventHandler(this.treeView1_ItemDrag);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.打开ToolStripMenuItem, this.toolStripMenuItem1, this.新建ToolStripMenuItem, this.刷新ToolStripMenuItem, this.删除ToolStripMenuItem, this.重命名ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0x7d, 120);
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.打开ToolStripMenuItem.Text = "打开(&O)";
            this.打开ToolStripMenuItem.Click += new EventHandler(this.打开ToolStripMenuItem_Click);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(0x79, 6);
            this.新建ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.文件夹ToolStripMenuItem, this.模版ToolStripMenuItem });
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.新建ToolStripMenuItem.Text = "新建(&N)";
            this.文件夹ToolStripMenuItem.Name = "文件夹ToolStripMenuItem";
            this.文件夹ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.文件夹ToolStripMenuItem.Text = "文件夹(&F)";
            this.文件夹ToolStripMenuItem.Click += new EventHandler(this.文件夹ToolStripMenuItem_Click);
//            this.模版ToolStripMenuItem.Image = (Image) manager.GetObject("模版ToolStripMenuItem.Image");
            this.模版ToolStripMenuItem.Name = "模版ToolStripMenuItem";
            this.模版ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.模版ToolStripMenuItem.Text = "模版(&T)";
            this.模版ToolStripMenuItem.Click += new EventHandler(this.模版ToolStripMenuItem_Click);
//            this.刷新ToolStripMenuItem.Image = (Image) manager.GetObject("刷新ToolStripMenuItem.Image");
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.刷新ToolStripMenuItem.Text = "刷新(&R)";
            this.刷新ToolStripMenuItem.Click += new EventHandler(this.刷新ToolStripMenuItem_Click);
//            this.删除ToolStripMenuItem.Image = (Image) manager.GetObject("删除ToolStripMenuItem.Image");
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.删除ToolStripMenuItem.Text = "删除(&D)";
            this.删除ToolStripMenuItem.Click += new EventHandler(this.删除ToolStripMenuItem_Click);
            this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
            this.重命名ToolStripMenuItem.Size = new Size(0x7c, 0x16);
            this.重命名ToolStripMenuItem.Text = "重命名(&M)";
            this.重命名ToolStripMenuItem.Click += new EventHandler(this.重命名ToolStripMenuItem_Click);
//            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folderclose.gif");
            this.imageList1.Images.SetKeyName(1, "Folderopen.gif");
            this.imageList1.Images.SetKeyName(2, "te.gif");
            base.AutoScaleDimensions = new SizeF(6f, 12f);
     //       base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xab, 0x170);
            base.Controls.Add(this.treeView1);
            base.Controls.Add(this.toolStrip1);
            base.Name = "TempView";
//            base.SizeGripStyle = SizeGripStyle.Show;
            this.Text = "TempView";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadTreeview()
        {
            this.ds = new DataSet();
            this.treeView1.Nodes.Clear();
            TemplateNode node = new TemplateNode("代码模版");
            node.NodeType = "root";
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            node.Expand();
            this.treeView1.Nodes.Add(node);
            this.ds.ReadXml(this.tempfilepath);
            DataTable dt = this.ds.Tables[0];
            foreach (DataRow row in dt.Select("ParentID= " + 0))
            {
                string s = row["NodeID"].ToString();
                string nodeName = row["Text"].ToString();
                string str3 = row["FilePath"].ToString();
                string str4 = row["NodeType"].ToString();
                TemplateNode node2 = new TemplateNode(nodeName);
                node2.NodeID = s;
                node2.NodeType = str4;
                node2.FilePath = str3;
                if (str4 == "folder")
                {
                    node2.ImageIndex = 0;
                    node2.SelectedImageIndex = 1;
                }
                else
                {
                    node2.ImageIndex = 2;
                    node2.SelectedImageIndex = 2;
                }
                node.Nodes.Add(node2);
                int parentid = int.Parse(s);
                this.CreateNode(parentid, node2, dt);
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        e.Node.EndEdit(false);
                        TemplateNode node = (TemplateNode) e.Node;
                        string nodeID = node.NodeID;
                        string nodeType = node.NodeType;
                        string label = e.Label;
                        string filePath = node.FilePath;
                        string destFileName = filePath;
                        if (nodeType == "file")
                        {
                            int num = filePath.LastIndexOf(@"\");
                            destFileName = filePath.Substring(0, (filePath.Length - num) - 1) + label + ".cmt";
                            File.Move(filePath, destFileName);
                        }
                        foreach (DataRow row in this.ds.Tables[0].Select("NodeID='" + nodeID + "'"))
                        {
                            row["Text"] = label;
                            row["FilePath"] = destFileName;
                        }
                        this.ds.Tables[0].AcceptChanges();
                        this.ds.WriteXml(this.tempfilepath);
                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("无效节点或无效字符: '@','.', ',', '!'", "节点编辑");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("无效节点或节点名称不能为空！", "节点编辑");
                    e.Node.BeginEdit();
                }
                this.treeView1.LabelEdit = false;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point pt = new Point(e.X, e.Y);
                this.TreeClickNode = (TemplateNode) this.treeView1.GetNodeAt(pt);
                this.treeView1.SelectedNode = this.TreeClickNode;
                if ((this.TreeClickNode != null) && (e.Button == MouseButtons.Right))
                {
                    this.CreatMenu(this.TreeClickNode.NodeType);
                    this.contextMenuStrip1.Show(this.treeView1, pt);
                }
            }
            catch
            {
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TemplateNode selectedNode = (TemplateNode) this.treeView1.SelectedNode;
                string text = selectedNode.Text;
                string filePath = selectedNode.FilePath;
                if (filePath.Trim() != "")
                {
                    FrmCodeTemplate template = (FrmCodeTemplate) Application.OpenForms["CodeTemplate"];
                    if (template != null)
                    {
                        template.SettxtTemplate(filePath);
                    }
                    else
                    {
                        MessageBox.Show("尚未打开模版代码生成编辑器！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show("所选文件已经不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void 模版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TemplateNode selectedNode = (TemplateNode) this.treeView1.SelectedNode;
                string nodeID = selectedNode.NodeID;
                int num = 1;
                if (selectedNode != null)
                {
                    num = selectedNode.Nodes.Count + 1;
                }
                string maxNodeID = this.GetMaxNodeID(this.ds.Tables[0]);
                DataRow row = this.ds.Tables[0].NewRow();
                row["NodeID"] = maxNodeID;
                row["Text"] = "新建模版";
                string path = "新建模版.cmt";
                if ((this.settings.TemplateFolder != null) && (this.settings.TemplateFolder != ""))
                {
                    path = this.settings.TemplateFolder + @"\" + path;
                }
                else
                {
                    path = @"Template\新建模版.cmt";
                }
                row["FilePath"] = path;
                row["NodeType"] = "file";
                row["ParentID"] = nodeID;
                row["OrderID"] = num;
                this.ds.Tables[0].Rows.Add(row);
                this.ds.Tables[0].AcceptChanges();
                this.ds.WriteXml(this.tempfilepath);
                File.Create(path);
                TemplateNode node = new TemplateNode("新建模版");
                node.NodeID = maxNodeID;
                node.ParentID = nodeID;
                node.FilePath = path;
                node.NodeType = "file";
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                selectedNode.Nodes.Add(node);
                selectedNode.Expand();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TemplateNode selectedNode = (TemplateNode) this.treeView1.SelectedNode;
                string nodeID = selectedNode.NodeID;
                string text = selectedNode.Text;
                string filePath = selectedNode.FilePath;
                string nodeType = selectedNode.NodeType;
                foreach (DataRow row in this.ds.Tables[0].Select("NodeID='" + nodeID + "'"))
                {
                    this.ds.Tables[0].Rows.Remove(row);
                }
                this.ds.Tables[0].AcceptChanges();
                this.ds.WriteXml(this.tempfilepath);
                if ((nodeType == "file") && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                this.treeView1.Nodes.Remove(selectedNode);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoadTreeview();
        }

        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TemplateNode selectedNode = (TemplateNode) this.treeView1.SelectedNode;
                string nodeID = selectedNode.NodeID;
                int num = 1;
                if (selectedNode != null)
                {
                    num = selectedNode.Nodes.Count + 1;
                }
                string text = selectedNode.Text;
                string filePath = selectedNode.FilePath;
                string nodeType = selectedNode.NodeType;
                string maxNodeID = this.GetMaxNodeID(this.ds.Tables[0]);
                DataRow row = this.ds.Tables[0].NewRow();
                row["NodeID"] = maxNodeID;
                row["Text"] = "新建文件夹";
                row["FilePath"] = "";
                row["NodeType"] = "folder";
                row["ParentID"] = nodeID;
                row["OrderID"] = num;
                this.ds.Tables[0].Rows.Add(row);
                this.ds.Tables[0].AcceptChanges();
                this.ds.WriteXml(this.tempfilepath);
                TemplateNode node = new TemplateNode("新建文件夹");
                node.NodeID = maxNodeID;
                node.ParentID = nodeID;
                node.NodeType = "folder";
                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;
                selectedNode.Nodes.Add(node);
                selectedNode.Expand();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TemplateNode selectedNode = (TemplateNode) this.treeView1.SelectedNode;
                if ((selectedNode != null) && (selectedNode.Parent != null))
                {
                    this.treeView1.SelectedNode = selectedNode;
                    this.treeView1.LabelEdit = true;
                    if (!selectedNode.IsEditing)
                    {
                        selectedNode.BeginEdit();
                    }
                }
                else
                {
                    MessageBox.Show("没有选择节点或该节点是根节点.\n", "无效选择");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}

