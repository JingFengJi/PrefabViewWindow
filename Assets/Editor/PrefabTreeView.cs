using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class GameObjectTreeViewItem {
    public TreeViewItem viewItem;
    public Transform treeViewTrans;
    public Transform parent;
    public List<Transform> childs = new List<Transform> ();

    public bool IsChild (Transform tran) {
        if (null == tran) return false;
        if (tran.parent == treeViewTrans) return true;
        return false;
    }

    public void AddChildTransform (Transform childTransform) {
        if (null != childTransform)
            childs.Add (childTransform);
    }

    public bool HasChildTransform (Transform tran) {
        if (null == tran) return false;
        return childs.Contains (tran);
    }
}

public class PrefabTreeView : TreeView {
    private Transform[] childs;
    private GameObjectTreeViewItem[] items;
    public PrefabTreeView (TreeViewState state, GameObject go) : base (state) {
        treeGameObject = go;
        childs = treeGameObject.GetComponentsInChildren<Transform> (true);
        items = new GameObjectTreeViewItem[childs.Length];
        #region Init Items Parent
        for (int i = 0; i < childs.Length; i++) {
            items[i] = new GameObjectTreeViewItem ();
            items[i].treeViewTrans = childs[i];
            if (childs[i].parent != null)
                items[i].parent = childs[i].parent;
        }
        #endregion

        #region Init Child Transform
        for (int i = 0; i < childs.Length; i++) {
            for (int j = 0; j < items.Length; j++) {
                if (items[j].IsChild (childs[i])) {
                    items[j].AddChildTransform (childs[i]);
                    break;
                }
            }
        }
        #endregion
        Reload ();
    }

    private GameObject treeGameObject;

    private GameObjectTreeViewItem GetGameObjectTreeViewItem (GameObjectTreeViewItem[] items, Transform trans) {
        if (null == items || items.Length == 0 || trans == null)
            return null;
        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null && items[i].treeViewTrans == trans)
                return items[i];
        }
        return null;
    }

    private GameObjectTreeViewItem GetGameObjectTreeViewItem (GameObjectTreeViewItem[] items, TreeViewItem viewItem) {
        if (null == items || items.Length == 0 || viewItem == null)
            return null;
        for (int i = 0; i < items.Length; i++) {
            if (null != items && items[i].viewItem == viewItem)
                return items[i];
        }
        return null;
    }

    protected override TreeViewItem BuildRoot () {
        TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "root" };
        TreeViewItem[] childItems = new TreeViewItem[childs.Length];
        for (int i = 0; i < childs.Length; i++) {
            childItems[i] = new TreeViewItem { id = i + 1, displayName = childs[i].name };
            GameObjectTreeViewItem item = GetGameObjectTreeViewItem (items, childs[i]);
            if (null != item) {
                item.viewItem = childItems[i];
            }
        }
        root.AddChild (childItems[0]);
        for (int i = 0; i < childItems.Length; i++) {
            GameObjectTreeViewItem temp = GetGameObjectTreeViewItem (items, childItems[i]);
            if (temp != null && temp.childs != null) {
                for (int j = 0; j < temp.childs.Count; j++) {
                    GameObjectTreeViewItem childItem = GetGameObjectTreeViewItem (items, temp.childs[j]);
                    if (childItem != null && childItem.viewItem != null) {
                        childItems[i].AddChild (childItem.viewItem);
                    }
                }
            }
        }

        SetupDepthsFromParentsAndChildren (root);
        return root;
    }
}