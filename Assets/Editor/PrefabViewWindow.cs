using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PrefabViewWindow : EditorWindow {

	private GameObject selectPrefab = null;
	[SerializeField]
	TreeViewState treeViewState;
	PrefabTreeView prefabTreeView;

	void OnEnable () {
		if (null == treeViewState)
			treeViewState = new TreeViewState ();
		if (selectPrefab != null)
			prefabTreeView = new PrefabTreeView(treeViewState, selectPrefab);
		else prefabTreeView = null;
	}

	[MenuItem ("PrefabView/PrefabViewWindow")]
	private static void ShowWindow () {
		GetWindow<PrefabViewWindow> ().Show ();
	}

	private void OnGUI () {
		if (null != selectPrefab && prefabTreeView != null) {
			Rect rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
			prefabTreeView.OnGUI (rect);
		}
	}

	void OnSelectionChange ()
	{
		GameObject curSelectPrefab = Selection.activeObject as GameObject;
		if (curSelectPrefab == null) return;
		if (curSelectPrefab != selectPrefab)
		{
			selectPrefab = Selection.activeObject as GameObject;
			prefabTreeView = new PrefabTreeView(treeViewState,selectPrefab);
		}
        Repaint();
	}
}