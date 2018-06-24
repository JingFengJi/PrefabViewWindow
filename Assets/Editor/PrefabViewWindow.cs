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
	}

	[MenuItem ("PrefabView/PrefabViewWindow")]
	private static void ShowWindow () {
		GetWindow<PrefabViewWindow> ().Show ();
	}

	private void OnGUI () {
		if (null != selectPrefab) {
			Rect rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
			prefabTreeView = new PrefabTreeView (treeViewState, selectPrefab);

			prefabTreeView.OnGUI (rect);
			prefabTreeView.ExpandAll ();
		} else {

		}
	}

	void OnSelectionChange () {
		selectPrefab = Selection.activeObject as GameObject;
        Repaint();
	}
}