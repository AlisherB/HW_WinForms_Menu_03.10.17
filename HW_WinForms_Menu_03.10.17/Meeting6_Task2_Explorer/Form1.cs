using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meeting6_Task2_Explorer
{
    public partial class Explorer : Form
    {
        public Explorer()
        {
            InitializeComponent();
            GetDrives();
            treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeView_NodeMouseClick);
        }

        private void GetDrives()
        {
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    TreeNode driveNode = new TreeNode { Text = drive.Name };
                    GetDirectories(driveNode, drive.Name);
                    treeView.Nodes.Add(driveNode);
                }
            }
            catch (Exception) { }
        }

        private void GetDirectories(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception) { }
        }

        //private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        //{
        //    TreeNode aNode;
        //    DirectoryInfo[] getSubDirs;
        //    foreach (DirectoryInfo subDir in subDirs)
        //    {
        //        aNode = new TreeNode(subDir.Name, 0, 0);
        //        aNode.Tag = subDir;
        //        aNode.ImageKey = "folder";
        //        getSubDirs = subDir.GetDirectories();
        //        if (getSubDirs.Length != 0)
        //        {
        //            GetDirectories(getSubDirs, aNode);
        //        }
        //        nodeToAddTo.Nodes.Add(aNode);
        //    }
        //}

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                listView.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                listView.Items.Add(item);
            }
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
