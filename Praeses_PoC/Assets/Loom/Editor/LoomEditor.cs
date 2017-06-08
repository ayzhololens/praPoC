using UnityEditor;
using UnityEngine;
using System.Collections;

public class LoomEditor : Editor 
{
	public override void OnInspectorGUI () {
		LoomAvatar avatar = (LoomAvatar) target;

		if(GUILayout.Button("Upload Current Selfie")) {
			avatar.ApiUploadSelfie();
		}
		if (GUILayout.Button ("Download Solve")) {
			avatar.serverDownload = true;
		}
		if(GUILayout.Button("Load Streaming Assets")) {
			avatar.localDownload = true;
		}
		if(GUILayout.Button("Export OBJs")) {
			avatar.ExportObjs();
		}

		DrawDefaultInspector ();
	}
}

[CustomEditor(typeof(LoomDeformerLoomie_male))]
[CanEditMultipleObjects]
public class LoomEditorMale : LoomEditor 
{
}

[CustomEditor(typeof(LoomDeformerLoomie_female))]
[CanEditMultipleObjects]
public class LoomEditorFemale : LoomEditor 
{
}
