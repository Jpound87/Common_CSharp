using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Common
{
    public static class Extensions_TreeNode
    {
        #region Identity
        public const string ClassName = nameof(Extensions_TreeNode);
        #endregion

        #region Find
        public static void FindTypeInTreeTags<T>(this TreeView treeView, out HashSet<T> foundItems)
        {
            foundItems = new HashSet<T>();
            foreach (TreeNode node in treeView.Nodes)
            {
                node.FindTypeInTreeTags_Recursive(ref foundItems);
                if (node.Tag is T lookupTypeObject)
                {
                    foundItems.Add(lookupTypeObject);
                }
            }
        }

        public static void FindTypeInTreeTags_Recursive<T>(this TreeView treeView, ref HashSet<T> foundItems)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                node.FindTypeInTreeTags_Recursive(ref foundItems);
                if (node.Tag is T lookupTypeObject)
                {
                    foundItems.Add(lookupTypeObject);
                }
            }
        }

        public static void FindTypeInTreeTags_Recursive<T>(this TreeNode treeNode, ref HashSet<T> foundItems)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.FindTypeInTreeTags_Recursive(ref foundItems);
                if (node.Tag is T lookupTypeObject)
                {
                    foundItems.Add(lookupTypeObject);
                }
            }
        }
        #endregion

        #region Expand
        /// <summary>
        /// This method recursively expands all options under the given top node
        /// </summary>
        /// <param name="topNode">The node under which to expand all options</param>
        public static void ExpandTreeAt_Recursive(this TreeNode topNode)
        {
            for (int nodeIndex = topNode.Nodes.Count - 1; nodeIndex >= 0; nodeIndex--)
            {
                topNode.Nodes[nodeIndex].Expand();
                ExpandTreeAt_Recursive(topNode.Nodes[nodeIndex]);
            }
        }
        #endregion

        #region Size
        public static void NodeDimension_Recursive(this TreeNodeCollection treeNodes, int padding, int indent, ref int height, ref int maxWidth)
        {
            Rectangle bounds;
            foreach (TreeNode node in treeNodes)
            {
                if (node.Nodes.Count > 0)
                {
                    node.Nodes.NodeDimension_Recursive(padding, indent, ref height, ref maxWidth);
                }
                bounds = node.Bounds;
                maxWidth = Math.Max(maxWidth, bounds.Width + padding + (indent * (node.Level + 1)));
                height += bounds.Height;
            }
        }
        #endregion

        #region Image
        public static TreeNode AddNodeWithImage(this TreeNode node, String key, String title, int imageIndex)
        {
            return node.Nodes.Add(key, title, imageIndex, imageIndex);
        }

        public static void UpdateImageIndex(this TreeNode treeNode, string key, int imageIndex)
        {
            for (int ni = 0; ni < treeNode.Nodes.Count; ni++)// We can say 'ni' as it stands for node index. No shrubbery required. 
            {
                UpdateImageIndex(treeNode.Nodes[ni], key, imageIndex);
            }
            if (treeNode.Text == key || treeNode.Name == key)
            {
                treeNode.ImageIndex = imageIndex;
            }
        }
        #endregion
    }
}
