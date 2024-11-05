using System;
using System.Collections.Generic;


public class BTreeNode
{
    public bool Leaf { get; set; }
    public List<(int, object)> Keys { get; set; } = new List<(int, object)>();
    public List<BTreeNode> Children { get; set; } = new List<BTreeNode>();

    public BTreeNode(bool leaf)
    {
        Leaf = leaf;
    }
}

public class BTree
{
    public BTreeNode root;
    private readonly int t;

    public BTree(int t)
    {
        root = new BTreeNode(true);
        this.t = t;
    }



    public void Insert((int, object) key)
    {
        if (root.Keys.Count == (2 * t) - 1)
        {
            var temp = new BTreeNode(false);
            temp.Children.Insert(0, root);
            root = temp;
            SplitChild(temp, 0);
            InsertNonFull(temp, key);
        }
        else
        {
            InsertNonFull(root, key);
        }
    }

    public (BTreeNode, int)? SearchKey(int key, BTreeNode node = null)
    {
        node ??= root;

        int low = 0, high = node.Keys.Count - 1;
        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (key == node.Keys[mid].Item1)
                return (node, mid);
            else if (key < node.Keys[mid].Item1)
                high = mid - 1;
            else
                low = mid + 1;
        }

        if (node.Leaf)
            return null;

        return SearchKey(key, node.Children[low]);
    }

    private void InsertNonFull(BTreeNode node, (int, object) key)
    {
        int i = node.Keys.Count - 1;
        if (node.Leaf)
        {
            node.Keys.Add((0, null));
            while (i >= 0 && key.Item1 < node.Keys[i].Item1)
            {
                node.Keys[i + 1] = node.Keys[i];
                i--;
            }
            node.Keys[i + 1] = key;
        }
        else
        {
            while (i >= 0 && key.Item1 < node.Keys[i].Item1)
                i--;

            i++;
            if (node.Children[i].Keys.Count == (2 * t) - 1)
            {
                SplitChild(node, i);
                if (key.Item1 > node.Keys[i].Item1)
                    i++;
            }
            InsertNonFull(node.Children[i], key);
        }
    }

    private void SplitChild(BTreeNode parent, int index)
    {
        var newChild = new BTreeNode(parent.Children[index].Leaf);
        var oldChild = parent.Children[index];

        parent.Children.Insert(index + 1, newChild);
        parent.Keys.Insert(index, oldChild.Keys[t - 1]);

        newChild.Keys.AddRange(oldChild.Keys.GetRange(t, t - 1));
        oldChild.Keys.RemoveRange(t - 1, t);

        if (!oldChild.Leaf)
        {
            newChild.Children.AddRange(oldChild.Children.GetRange(t, t));
            oldChild.Children.RemoveRange(t, t);
        }
    }

    public void Delete(BTreeNode node, (int, object) key)
    {
        int index = 0;
        while (index < node.Keys.Count && key.Item1 > node.Keys[index].Item1)
            index++;

        if (node.Leaf)
        {
            if (index < node.Keys.Count && node.Keys[index].Item1 == key.Item1)
                node.Keys.RemoveAt(index);
            return;
        }

        if (index < node.Keys.Count && node.Keys[index].Item1 == key.Item1)
        {
            DeleteInternalNode(node, key, index);
        }
        else
        {
            if (node.Children[index].Keys.Count >= t)
            {
                Delete(node.Children[index], key);
            }
            else
            {
                if (index != 0 && index + 2 < node.Children.Count)
                {
                    if (node.Children[index - 1].Keys.Count >= t)
                        DeleteSibling(node, index, index - 1);
                    else if (node.Children[index + 1].Keys.Count >= t)
                        DeleteSibling(node, index, index + 1);
                    else
                        DeleteMerge(node, index, index + 1);
                }
                else if (index == 0)
                {
                    if (node.Children[index + 1].Keys.Count >= t)
                        DeleteSibling(node, index, index + 1);
                    else
                        DeleteMerge(node, index, index + 1);
                }
                else if (index + 1 == node.Children.Count)
                {
                    if (node.Children[index - 1].Keys.Count >= t)
                        DeleteSibling(node, index, index - 1);
                    else
                        DeleteMerge(node, index, index - 1);
                }
                Delete(node.Children[index], key);
            }
        }
    }

    private void DeleteInternalNode(BTreeNode node, (int, object) key, int index)
    {
        if (node.Leaf)
        {
            if (node.Keys[index].Item1 == key.Item1)
                node.Keys.RemoveAt(index);
            return;
        }

        if (node.Children[index].Keys.Count >= t)
        {
            node.Keys[index] = DeletePredecessor(node.Children[index]);
            return;
        }
        else if (node.Children[index + 1].Keys.Count >= t)
        {
            node.Keys[index] = DeleteSuccessor(node.Children[index + 1]);
            return;
        }
        else
        {
            DeleteMerge(node, index, index + 1);
            DeleteInternalNode(node.Children[index], key, t - 1);
        }
    }

    private (int, object) DeletePredecessor(BTreeNode node)
    {
        if (node.Leaf)
            return node.Keys[^1];

        var lastChild = node.Children[^1];
        if (lastChild.Keys.Count >= t)
        {
            DeleteSibling(node, node.Keys.Count, node.Keys.Count - 1);
        }
        else
        {
            DeleteMerge(node, node.Keys.Count - 1, node.Keys.Count);
        }
        return DeletePredecessor(node.Children[^1]);
    }

    private (int, object) DeleteSuccessor(BTreeNode node)
    {
        if (node.Leaf)
            return node.Keys[0];

        if (node.Children[1].Keys.Count >= t)
        {
            DeleteSibling(node, 0, 1);
        }
        else
        {
            DeleteMerge(node, 0, 1);
        }
        return DeleteSuccessor(node.Children[0]);
    }

    private void DeleteMerge(BTreeNode x, int i, int j)
    {
        BTreeNode cNode = x.Children[i];
        BTreeNode rsNode = x.Children[j];

        if (j > i)
        {
            cNode.Keys.Add(x.Keys[i]);
            cNode.Keys.AddRange(rsNode.Keys);
            cNode.Children.AddRange(rsNode.Children);
            x.Keys.RemoveAt(i);
            x.Children.RemoveAt(j);
        }
        else
        {
            rsNode.Keys.Add(x.Keys[j]);
            rsNode.Keys.AddRange(cNode.Keys);
            rsNode.Children.AddRange(cNode.Children);
            x.Keys.RemoveAt(j);
            x.Children.RemoveAt(i);
        }

        if (x == root && x.Keys.Count == 0)
            root = cNode;
    }

    private void DeleteSibling(BTreeNode x, int i, int j)
    {
        BTreeNode cNode = x.Children[i];

        if (i < j)
        {
            BTreeNode rsNode = x.Children[j];
            cNode.Keys.Add(x.Keys[i]);
            x.Keys[i] = rsNode.Keys[0];
            if (rsNode.Children.Count > 0)
                cNode.Children.Add(rsNode.Children[0]);
            rsNode.Keys.RemoveAt(0);
            if (rsNode.Children.Count > 0)
                rsNode.Children.RemoveAt(0);
        }
        else
        {
            BTreeNode lsNode = x.Children[j];
            cNode.Keys.Insert(0, x.Keys[i - 1]);
            x.Keys[i - 1] = lsNode.Keys[lsNode.Keys.Count - 1];
            if (lsNode.Children.Count > 0)
                cNode.Children.Insert(0, lsNode.Children[lsNode.Children.Count - 1]);
            lsNode.Keys.RemoveAt(lsNode.Keys.Count - 1);
            if (lsNode.Children.Count > 0)
                lsNode.Children.RemoveAt(lsNode.Children.Count - 1);
        }
    }

    public void PrintTree(BTreeNode node, int level = 1)
    {
        Console.Write("Level " + level + " " + node.Keys.Count + ": ");
        foreach (var key in node.Keys)
            Console.Write(key + " ");
        Console.WriteLine();

        if (!node.Leaf)
        {
            level++;
            foreach (var child in node.Children)
                PrintTree(child, level);
        }
    }
    public bool ContainsKey(int k)
    {
        return ContainsKey(root, k);
    }

    public bool Update(int key, object newValue)
    {
        var result = SearchKey(key);
        if (result.HasValue)
        {
            var (node, index) = result.Value;
            node.Keys[index] = (key, newValue);
            return true;
        }
        return false;
    }

    private bool ContainsKey(BTreeNode node, int k)
    {
        int i = 0;
        while (i < node.Keys.Count && k > node.Keys[i].Item1)
            i++;

        if (i < node.Keys.Count && node.Keys[i].Item1 == k)
            return true;

        if (node.Leaf)
            return false;

        return ContainsKey(node.Children[i], k);
    }
    
}

