using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Extensions
{
    public static class Extensions_TreeView
    {
        #region Identity
        public const String ClassName = nameof(Extensions_TreeView);
        #endregion

        #region Image
        public static void UpdateImageIndex(this TreeView treeView, string key, int imageIndex)
        {
            if (treeView != null && treeView.TopNode != null)
            {
                treeView.TopNode.UpdateImageIndex(key, imageIndex);
            }
        }
        #endregion /Image

        #region Expand
        public static void ExpandTree(this TreeView treeView)
        {            
            for (int nodeIndex = treeView.Nodes.Count - 1; nodeIndex >= 0; nodeIndex--)
            {
                treeView.Nodes[nodeIndex].Expand();
                treeView.Nodes[nodeIndex].ExpandTreeAt_Recursive();
            }
        }
        #endregion
    }
}
