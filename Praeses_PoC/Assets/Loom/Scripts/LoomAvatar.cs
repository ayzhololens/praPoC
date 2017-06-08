using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Runtime.InteropServices;

class LoomAvatarMesh
{
	public string name;
	public GameObject gameObject;
	public GameObject reference;
	public LoomAvatarMesh submesh;

	public LoomAvatarMesh() {
	}
}

public class LoomAvatar : MonoBehaviour
{
	[Header("Avatar Setup")]
	public bool serverDownload = false;
	public bool localDownload = false;
	public string avatarId = "";
	public string defaultAvatarId = ""; // Used only when in demo mode when triggering download
	public string customApiStyle = "";

	bool uploadSelfie = false;
	bool exportGeometry = false;

	GameObject loomGameObject;
	string avatar;
	List<LoomAvatarMesh> avatarMeshes = new List<LoomAvatarMesh> ();
	string avatarSrcName = "";

	public LoomAvatar LoomLoadAvatar (string avatarName)
	{
		// Check on environment
		if (Loom.apiKey == "") {
			Loom.apiKey = System.Environment.GetEnvironmentVariable ("LOOM_API_KEY");
			if (Loom.apiKey == null) {
				Loom.apiKey = "";
			}
		}
		if (Loom.licenseKey == "") {
			Loom.licenseKey = System.Environment.GetEnvironmentVariable ("LOOM_LICENSE_KEY");
			if (Loom.licenseKey == null) {
				Loom.licenseKey = "";
			}
		}

		LoomSdkSetToken(Loom.licenseKey);

		LoomSdkSetLogLevel (2); // Info

		loomGameObject = new GameObject("loom");
		loomGameObject.transform.parent = this.gameObject.transform;
		loomGameObject.transform.localPosition = new Vector3 (0, 0, 0);
		loomGameObject.transform.localEulerAngles = new Vector3 (0, 0, 0);
		loomGameObject.transform.localScale = new Vector3 (1, 1, 1);

		if (LoomSdkLoadAvatar (avatarName, gameObject.name) != -1) {
			avatar = gameObject.name;
			avatarSrcName = avatarName;
			for (int meshI = 0; meshI < Loom.meshMapping.Length; meshI++) {
				ImportSdkMesh (Loom.meshMapping [meshI].Item1, Loom.meshMapping [meshI].Item2);
			}
			Debug.Log ("loaded " + avatarName + " as " + gameObject.name);
		} else {
			avatarName = "";
		}

		FlushLog ("Avatar load");

		// Force this to false so it can only be explicitly triggered
		uploadSelfie = false;
		exportGeometry = false;

		return this;
	}

	public static void CalculateMeshTangents(Mesh mesh)
	{
		//speed up math by copying the mesh arrays
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;

		//variable definitions
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;

		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];

		Vector4[] tangents = new Vector4[vertexCount];

		for (long a = 0; a < triangleCount; a += 3)
		{
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];

			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];

			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float r = 1.0f / (s1 * t2 - s2 * t1);

			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}


		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = normals[a];
			Vector3 t = tan1[a];

			//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;

			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}

		mesh.tangents = tangents;
	}

	void SynchSdkMesh (LoomAvatarMesh avatarMesh)
	{
		string meshName = avatarMesh.name;
		if (LoomSdkUnweldMesh (meshName) != -1) {
			int vertexCount = LoomSdkGetMeshVertexCount (meshName);
			if (vertexCount > 0) {
				Mesh mesh = avatarMesh.gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh;

				// Reset the triangles otherwise the new temporary vertex data will generate errors
				//	when assigned below if smaller than the triangles range
				mesh.triangles = new int[0];

				Vector3[] vtx = new Vector3[vertexCount];
				LoomSdkGetReferenceMeshVertices (meshName, vtx);
				mesh.vertices = vtx;

				int faceCount = LoomSdkGetMeshFaceCount (meshName);
				if (faceCount > 0) {
					int[] triangles = new int[faceCount * 3];
					LoomSdkGetMeshFaces (meshName, triangles);
					mesh.triangles = triangles;
				}

				int uvCount = LoomSdkGetMeshUvCount (meshName);
				if (uvCount > 0) {
					Vector2[] uvs = new Vector2[uvCount];
					LoomSdkGetMeshUvs (meshName, uvs);
					mesh.uv = uvs;
				}

				Vector3[] normals = new Vector3[vertexCount];
				LoomSdkGetRenderMeshNormals (meshName, normals);
				mesh.normals = normals;

				CalculateMeshTangents (mesh);
			}
		}
	}

	// This should be used in case you want to switch one SDK geometry with another
	//	and let it be deformed by the main head geometry. Useful for anything that should
	//	follow hair deformations
	public void SetSdkMesh (string meshName, GameObject unityMesh)
	{
		for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
			LoomAvatarMesh loomAvatarMesh = avatarMeshes [nodeI];
			if (loomAvatarMesh.gameObject.name == meshName) {
				SetSdkMesh (loomAvatarMesh, unityMesh);
			}
		}
	}

	void SetSdkMesh (LoomAvatarMesh avatarMesh, GameObject referenceMesh)
	{
		if (avatarMesh.reference != referenceMesh) {
			string meshName = avatarMesh.name;
			MeshRenderer meshRenderer = referenceMesh.GetComponentInChildren<MeshRenderer> ();
			if (meshRenderer != null) {
				Mesh mesh = meshRenderer.GetComponent<MeshFilter>().mesh;
				if (mesh != null) {
					Vector3[] vtx = mesh.vertices;
					Vector2[] uvs = mesh.uv;
					int[] faces = mesh.triangles;
					avatarMesh.reference = referenceMesh;

					LoomSetReferenceMesh (meshName, vtx.Length, vtx, uvs.Length, uvs, faces.Length / 3, faces, faces.Length / 3, faces);
					LoomSetMeshDeformer(avatar, avatarMesh.gameObject.name, "head_GEO", 10);

					SynchSdkMesh (avatarMesh);
				}
			}
		}
	}
		
	LoomAvatarMesh ImportSdkMesh (string meshName, string unityMeshName)
	{
		string localMeshName = meshName;
		if (unityMeshName == "") {
			unityMeshName = localMeshName;
		}

		meshName = avatar + "/" + meshName;
		if (LoomSdkUnweldMesh (meshName) != -1) {
			// Look for a matching Mesh in the current Hierarchy
			Transform referenceNode = this.gameObject.transform.Find("MODEL/" + unityMeshName);
			if (referenceNode == null) {
				referenceNode = this.gameObject.transform.Find("MODEL/" + avatarSrcName + "-" + unityMeshName);
			}
			if (referenceNode == null) {
				return null;
			}

			int vertexCount = LoomSdkGetMeshVertexCount (meshName);
			if (vertexCount > 0) {
				LoomAvatarMesh avatarMesh = new LoomAvatarMesh ();
					
				avatarMesh.name = meshName;

				avatarMesh.gameObject = new GameObject (localMeshName);
				avatarMesh.gameObject.transform.parent = loomGameObject.gameObject.transform;

				avatarMesh.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
				avatarMesh.gameObject.transform.localEulerAngles = new Vector3 (0, 0, 0);

				SkinnedMeshRenderer skinnedMeshRenderer = avatarMesh.gameObject.AddComponent<SkinnedMeshRenderer> ();

				bool hasParentMesh = false;
				for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
					if (avatarMeshes [nodeI].name == meshName) {
						avatarMeshes [nodeI].submesh = avatarMesh;
						skinnedMeshRenderer.sharedMesh = avatarMeshes [nodeI].gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh;
						hasParentMesh = true;
						break;
					}
				}
				if (!hasParentMesh) {
					Mesh mesh = new Mesh ();
					skinnedMeshRenderer.sharedMesh = mesh;
					skinnedMeshRenderer.receiveShadows = false;

					SynchSdkMesh (avatarMesh);
					avatarMeshes.Add (avatarMesh);
				}

				// Assign a default material in case it's not set by the user to make it fully visible
				skinnedMeshRenderer.material = new Material(Shader.Find("Diffuse"));

				if (referenceNode != null) {
					avatarMesh.reference = referenceNode.gameObject;
					if (avatarMesh.gameObject != null) {
						SkinnedMeshRenderer skinnedMesh = avatarMesh.gameObject.GetComponentInChildren<SkinnedMeshRenderer> ();
						if (skinnedMesh != null) {
							MeshRenderer meshRef = avatarMesh.reference.GetComponentInChildren<MeshRenderer> ();
							if (meshRef != null) {
								skinnedMesh.materials = meshRef.materials;
								skinnedMesh.receiveShadows = meshRef.receiveShadows;
								//skinnedMesh.shadowCastingMode = meshRef.shadowCastingMode;
							} else {
								SkinnedMeshRenderer skinnedMeshRef = avatarMesh.reference.GetComponentInChildren<SkinnedMeshRenderer> ();
								if (skinnedMeshRef != null) {
									skinnedMesh.materials = skinnedMeshRef.materials;
									skinnedMesh.receiveShadows = skinnedMeshRef.receiveShadows;
									//skinnedMesh.shadowCastingMode = skinnedMeshRef.shadowCastingMode;
								}
							}
						}

						// And now hide the reference mesh, it's not needed anymore
						avatarMesh.reference.SetActive (false);
					}
				}

				return avatarMesh;
			}
		}

		return null;
	}

	#if UNITY_IPHONE && !UNITY_EDITOR
	const string loomSdkName = "__Internal";
	#else
	const string loomSdkName = "loom";
	#endif

	[DllImport (loomSdkName)] private static extern int LoomSdkLoadAvatar(string avatarName, string instanceName);
	[DllImport (loomSdkName)] private static extern int LoomSdkSetToken(string token);
	[DllImport (loomSdkName)] private static extern int LoomSdkTrackTargetTranslation(string avatarName, float x, float y, float z);

	[DllImport (loomSdkName)] private static extern int LoomSdkRigStart(string avatarName);
	[DllImport (loomSdkName)] private static extern int LoomSdkSetControl(string avatarName, string controlName, float weight);

	[DllImport (loomSdkName)] private static extern int LoomSdkSetBehaviorActive(string avatar, string name, int isActive);
	[DllImport (loomSdkName)] private static extern int LoomSdkSetBehaviorPlayOnce(string avatar, string name);

	[DllImport (loomSdkName)] private static extern int LoomSdkRigApplyAtTime(string avatarName, float time);

	[DllImport (loomSdkName)] private static extern int LoomSdkUnweldMesh(string meshName);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshVertexCount(string meshName);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshUvCount(string meshName);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshFaceCount(string meshName);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshUvs(string meshName, Vector2[] uvs);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshFaces(string meshName, int[] faces);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetMeshFaceUvs(string meshName, int[] faceUvs);

	[DllImport (loomSdkName)] private static extern int LoomSdkGetReferenceMeshVertices(string meshName, Vector3[] vtx);

	[DllImport (loomSdkName)] private static extern int LoomSdkGetRenderMeshVertices(string name, Vector3[] vtx);
	[DllImport (loomSdkName)] private static extern int LoomSdkGetRenderMeshNormals(string name, Vector3[] normals);

	[DllImport (loomSdkName)] private static extern int LoomApplySolvedLEyeTranslation(string avatar, float x, float y, float z);
	[DllImport (loomSdkName)] private static extern int LoomApplySolvedREyeTranslation(string avatar, float x, float y, float z);
	[DllImport (loomSdkName)] private static extern int LoomApplySolvedData(string avatar, string dataPath, byte[] data, int dataSize);

	[DllImport (loomSdkName)] private static extern int LoomSdkGetNodeGlobalMatrix(string name, float[] matrix4x4);

	[DllImport (loomSdkName)] private static extern int LoomSetReferenceMesh(string meshName, int vtxCount, Vector3[] vtx, int uvCount, Vector2[] uvs, int faceCount, int[] faces, int faceUvCount, int[] faceUvs);
	[DllImport (loomSdkName)] private static extern int LoomSetMeshDeformer(string avatar, string geom, string srcGeom, float maxBoneDistance);

	[DllImport (loomSdkName)] private static extern void LoomSdkSetLogLevel(int level);

	[DllImport (loomSdkName)] private static extern IntPtr LoomSdkExportObj(string meshName);
	public string LoomSdkGetMeshObjString(string meshName) {
		return Marshal.PtrToStringAnsi (LoomSdkExportObj (meshName));
	}

	[DllImport (loomSdkName)] private static extern int LoomSdkClear();

	// Internal wrapper only, not to be used directly, use the typed one below instead
	[DllImport (loomSdkName)] private static extern IntPtr LoomSdkFlushLog();

	public void FlushLog(string location) {
		string log = Marshal.PtrToStringAnsi (LoomSdkFlushLog ());
		if (log != "") {
			log = "Loom (on " + location + "): " + log;
			if (log.IndexOf ("Error") != -1) {
				Debug.LogError (log);
			} else if (log.IndexOf ("Warn") != -1) {
				Debug.LogWarning (log);
			} else {
				Debug.Log (log);
			}
		}
	}

	public void SetControl (string name, float weight) {
		if (avatar != null) {
			LoomSdkSetControl (avatar, name, weight);
		}
	}

	public void SetBehaviorActive (string animName, bool isActive) {
		if (avatar != null) {
			LoomSdkSetBehaviorActive (avatar, animName, isActive ? 1 : 0);
		}
	}

	public void SetBehaviorPlayOnce (string animName) {
		if (avatar != null) {
			LoomSdkSetBehaviorPlayOnce (avatar, animName);
		}
	}

	public void SetHeadTracking(float value) {
		SetControl ("trackHead", value);
	}

	public void SetEyesTracking(float value) {
		SetControl ("trackEyes", value);
	}

	public void SetHeadTrackingTargetPosition(Vector3 value) {
		if (avatar != null) {
			LoomSdkTrackTargetTranslation (avatar, value.x, value.y, value.z);
		}
	}

	public void UpdateStart ()
	{
		if (avatar != null) {
			LoomSdkRigStart (avatar);
			FlushLog ("Avatar rig start");
		}
	}

	public static void SetTransformFromMatrix(Transform transform, float[] matrix) {
		transform.localPosition = new Vector3(matrix[12], matrix[13], matrix[14]);
		transform.localRotation =
			Quaternion.LookRotation(
				new Vector3(matrix[8], matrix[9], matrix[10]), 
				new Vector3(matrix[4], matrix[5], matrix[6]));
	}

	public void UpdateEnd ()
	{
		if (avatar != null) {
			LoomSdkRigApplyAtTime (avatar, Time.fixedTime);
			FlushLog ("Avatar rig apply");

			for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
				LoomAvatarMesh loomAvatarMesh = avatarMeshes [nodeI];
				if (loomAvatarMesh.gameObject.activeInHierarchy) {
					Mesh mesh = loomAvatarMesh.gameObject.GetComponent<SkinnedMeshRenderer> ().sharedMesh;
					if (mesh != null) {
						Vector3[] vertices = mesh.vertices;
						LoomSdkGetRenderMeshVertices (loomAvatarMesh.name, vertices);
						mesh.vertices = vertices;
						Vector3[] normals = mesh.normals;
						LoomSdkGetRenderMeshNormals (loomAvatarMesh.name, normals);
						mesh.normals = normals;

						if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
							// Do nothing there, it is too slow to process tangents
						} else if (GetTexture (loomAvatarMesh.gameObject, TEXTURE_BUMP) != null) {
							CalculateMeshTangents (mesh);
						}
					}
				}
            }

            FlushLog ("Avatar update done");

			if (serverDownload) {
				if (!serverDownloadDone) {
					serverDownloadDone = true;
					serverCanCheck = true;
				}
			}
			if (localDownload) {
				if (!localDownloadDone) {
					LocalDownloadRig ();
					localDownloadDone = true;
				}
			}
			if (localZipDownload) {
				if (!localZipDownloadDone) {
					localZipDownloadDone = true;
					DownloadLocalZip ();
				}
			}

			ServerCheckAvatar ();
			if (uploadSelfie) {
				ServerUploadSelfie ();
			}
			if (exportGeometry) {
				ExportGeometry ();
				exportGeometry = false;
			}

			// Update Skeleton nodes
			Transform skeleton = transform.FindChild("SKELETON");
			if (skeleton) {
				float[] matrix4x4 = new float[16];
				int nodeCount = skeleton.childCount;
				for (int nodeI = 0; nodeI < nodeCount; nodeI++) {
					Transform node = skeleton.GetChild (nodeI);
					if (LoomSdkGetNodeGlobalMatrix (avatar + "/" + node.name, matrix4x4) != -1) {
						SetTransformFromMatrix (node, matrix4x4);
					}
				}
			}
		}
	}

	void OnDestroy()
	{
		LoomSdkClear ();
	}

	public void ExportObjs() {
		exportGeometry = true;
	}

	void ExportGeometry()
	{
		for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
			LoomAvatarMesh loomAvatarMesh = avatarMeshes [nodeI];
			string objString = LoomSdkGetMeshObjString (loomAvatarMesh.name);
			string[] names = loomAvatarMesh.name.Split ('/');
			string basename = names [names.Length - 1];
			string filename = Application.persistentDataPath + "/" + basename + ".obj";
			File.WriteAllText(filename, objString);
			Debug.Log ("Wrote " + filename);
		}
	}

	string apiUrlProtocol = "https";
	string apiUrl = "api.loomai.com/v1/avatar/";
	bool uploadPending = false;
	float uploadStartTime = 0;
	float uploadEndTime = 0;
	bool serverDownloadDone = false;
	bool localDownloadDone = false;
	bool serverCanUpload = true;
	bool serverCanCheck = true;
	float serverCheckTime = -100;
	bool serverCanDownload = true;

	public float GetUploadProgress() {
		if (uploadPending) {
			if (uploadEndTime <= Time.fixedTime) {
				uploadEndTime += 10;
			}
		}

		if (uploadEndTime != 0) {
			if (uploadEndTime <= Time.fixedTime) {
				uploadEndTime = 0;
				return 1;
			} else {
				serverCanCheck = true;
				return (Time.fixedTime - uploadStartTime) / (uploadEndTime - uploadStartTime);
			}
		}

		return 1;
	}

	string URLAntiCacheRandomizer(string url)
	{
		string r = "";
		r += UnityEngine.Random.Range(1000000,8000000).ToString();
		string result = url + "?p=" + r;
		return result;
	}

	string GetUrl (string path) {
		return GetUrl (path, false);
	}

	string GetUrl (string path, bool randomize)
	{
		string url = apiUrlProtocol + "://" + apiUrl + path;
		if (randomize) {
			return URLAntiCacheRandomizer(url);
		} else {
			return url;
		}
	}

	public void ApiUploadSelfie() {
		if (serverCanUpload) {
			uploadSelfie = true;
		}
	}

	void ApiDownloadSynchRig() {
		for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
			SynchSdkMesh (avatarMeshes [nodeI]);
		}
	}

	void ApiApplyData (string path, byte[] data)
	{
		string[] names = path.Split (Path.DirectorySeparatorChar);
		string basename = names [names.Length - 1];
        names = basename.Split('/'); // split twice in case unix files got in the windows env
        basename = names[names.Length - 1];
        string ext = Path.GetExtension(basename);
		if (ext == ".bin") {
			LoomApplySolvedData (avatar, basename, data, data.Length);
            FlushLog("Avatar Rig Solve Apply");
		} else if (basename == "eye-data.json") {
			string dataString = System.Text.Encoding.UTF8.GetString(data);
			ApiAvatarEyesData eyesData = ApiAvatarEyesData.CreateFromJSON (dataString);
			if (eyesData != null) {
				LoomApplySolvedLEyeTranslation(avatar, eyesData.leyeTranslation[0], eyesData.leyeTranslation[1], eyesData.leyeTranslation[2]);
				LoomApplySolvedREyeTranslation(avatar, eyesData.reyeTranslation[0], eyesData.reyeTranslation[1], eyesData.reyeTranslation[2]);

				float irisScale = eyesData.irisScale [0];
				SetControl ("l_iris_Scale", irisScale);
				SetControl ("r_iris_Scale", irisScale);
				// Check on Deformer's taking over the control so it does not get overwritten
				var property = GetType().GetField("LeftIrisScale");
				if (property != null) {
					property.SetValue (this, irisScale);
				}
				property = GetType().GetField("RightIrisScale");
				if (property != null) {
					property.SetValue (this, irisScale);
				}
			}
		} else if (ext == ".png") {
			List<string> meshNames = new List<string>();
			string textureName = "";
			if (basename == ApiDownloadTexturePrefix () + "high.png") {
				meshNames.Add ("head_GEO");
				textureName = TEXTURE_ALBEDO;
			} else if (basename == ApiDownloadTexturePrefix () + "eyes.png") {
				meshNames.Add ("l_eye_GEO");
				meshNames.Add ("r_eye_GEO");
				textureName = TEXTURE_ALBEDO;
			} else if (basename == ApiDownloadTexturePrefix () + "mouth.png") {
				meshNames.Add ("mouth_GEO");
				textureName = TEXTURE_ALBEDO;
			} else if (basename == ApiDownloadTexturePrefix () + "hair.png") {
				meshNames.Add ("hair_GEO");
				textureName = TEXTURE_ALBEDO;
			}

			for (int meshI = 0; meshI < meshNames.Count; meshI++) {
				for (int nodeI = 0; nodeI < avatarMeshes.Count; nodeI++) {
					LoomAvatarMesh avatarMesh = avatarMeshes [nodeI];
					if (avatarMesh.gameObject.name == meshNames[meshI]) {
						SetTexture (avatarMesh.gameObject.GetComponent<SkinnedMeshRenderer> (), textureName, data);
						break;
					}
				}
			}
		}
	}

	const string TEXTURE_ALBEDO = "albedo";
	const string TEXTURE_BUMP = "bump";

	// Shader names for Unity are different than what we call them. This function
	// conveniently translates what unity expects.
	private static string GetUnityTextureName (string textureName)
	{
		switch (textureName) {

		case TEXTURE_ALBEDO:
			return "_MainTex";
		case TEXTURE_BUMP:
			return "_BumpMap";
		default:
			return "";
		}
	}

	Texture2D GetTexture (GameObject gameObject, string textureName)
	{
		SkinnedMeshRenderer renderer = gameObject.GetComponent<SkinnedMeshRenderer> ();
		if (renderer != null) {
			if (renderer.materials.Length > 0) {
				Material material = renderer.materials [0];
				if (material != null) {
					return (Texture2D) material.GetTexture (GetUnityTextureName (textureName));
				}
			}
		}
		return null;
	}

	Color GetTexturePixel (Texture2D texture, float u, float v) {
		float x = (texture.width - 1) * u;
		float y = (texture.height - 1) * v;
		Color color = texture.GetPixel ((int) x, (int) y);
		return color;
	}

	void SetTexture (Renderer renderer, string textureName, byte[] data)
	{
		Material material = renderer.materials [0];
		string textureId = GetUnityTextureName (textureName);
		Texture2D texture = (Texture2D) material.GetTexture (textureId);
		if (texture != null) {
			texture = new Texture2D (2, 2);
			texture.LoadImage (data);
			material.SetTexture (textureId, texture);
			if (textureName == TEXTURE_ALBEDO && material.GetTexture ("_EmissionMap") != null) {
				// In case the texture is used too for the emission map:
				material.SetTexture ("_EmissionMap", texture);
			}
		}
	}

	public string GetApiStyle() 
	{
		if (customApiStyle != "") {
			return customApiStyle;
		} else {
			return Loom.apiStyle;
		}
	}

	string  ApiDownloadTexturePrefix ()
	{
		var style = GetApiStyle ();
		if (style != "") {
			return style.Substring (0, 1).ToUpper() + style.Substring (1) + "_";
		} else {
			return "Loom_Solved_";
		}
	}

	IEnumerator ApiDownloadTextures ()
	{
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers ["Authorization"] = "Bearer " + Loom.apiKey;
		string url = GetUrl(ServerGetAvatarId () + "/download") + "?type=" + ((GetApiStyle() != "") ? "stylizedTexture" : "texture");
		WWW www = new WWW (url, null, headers);

		yield return www;

		string contentType = www.responseHeaders ["CONTENT-TYPE"];

		if (www.error != null) {
			Debug.LogError ("Can't load Solved Avatar Rig: " + www.error);
		} else if (contentType == "application/json,application/zip" || contentType == "application/zip") {
			Stream byteStream = new MemoryStream (www.bytes);
			ZipInputStream zipStream = new ZipInputStream (byteStream);
			if (zipStream.CanRead && avatar != null) {
				ZipEntry entry;
				while ((entry = zipStream.GetNextEntry ()) != null) {
					int numBytes = (int)entry.Size;
					if (numBytes > 0) {
						byte[] data = new byte[numBytes];
						zipStream.Read (data, 0, numBytes);
						ApiApplyData (entry.Name, data);
					}
				}

				Debug.Log ("Applied Solved Avatar Textures");
			} else {
				Debug.LogError ("Can't load Solved Avatar Textures");
			}
		} else {
			// It may be a redirect issue, try again from the LOCATION in the response
			string location = www.responseHeaders ["LOCATION"];
			if (location != null && location != "") {
				www = new WWW (location);
				yield return www;

				Debug.Log ("Redirecting Download URL: " + location);

				contentType = www.responseHeaders ["CONTENT-TYPE"];
				if (contentType == "application/json,application/zip" || contentType == "application/zip") {
					Stream byteStream = new MemoryStream (www.bytes);
					ZipInputStream zipStream = new ZipInputStream (byteStream);
					if (zipStream.CanRead && avatar != null) {
						ZipEntry entry;
						while ((entry = zipStream.GetNextEntry ()) != null) {
							int numBytes = (int)entry.Size;
							if (numBytes > 0) {
								byte[] data = new byte[numBytes];
								zipStream.Read (data, 0, numBytes);

								ApiApplyData (entry.Name, data);
							}
						}

						Debug.Log ("Applied Solved Avatar Textures");
					}
				}
			} else {
				Debug.LogError ("Can't load Solved Avatar Textures, invalid data returned: " + contentType);			
			}
		}

		serverCanDownload = true;
		serverDownload = false;
		serverDownloadDone = false;
		FlushLog ("Avatar Textures Solve Apply");
	}

	int localDownloadCount = 0;

	IEnumerator ApiDownloadRigLocalFile (string url)
	{
		WWW www = new WWW (url);
		yield return www;

		if (www.bytes.Length > 0) {
			ApiApplyData (url, www.bytes);
		}

		localDownloadCount--;

		if (localDownloadCount == 0) {
			ApiDownloadSynchRig ();
		}
	}

	void ApiDownloadRigLocalFiles ()
	{
		string path = Application.streamingAssetsPath;
		path += Path.DirectorySeparatorChar + gameObject.name;
		var info = new DirectoryInfo(path);
		if (!info.Exists) {
			return;
		}
		FileInfo[] fileInfo = info.GetFiles ();

		// Make sure it's > 0 to block final trigger of synch
		localDownloadCount++;

		foreach (FileInfo file in fileInfo) {
			string url;
			if (Application.platform == RuntimePlatform.Android) {
				url = System.IO.Path.Combine (path, file.Name);
			} else {
				url = "file:///" + System.IO.Path.Combine (path, file.Name);
			}

			string ext = Path.GetExtension(file.Name);
            if (ext == ".bin" || ext == ".json" || ext == ".png")
            {
                localDownloadCount++;
                StartCoroutine(ApiDownloadRigLocalFile(url));
            }
		}

		// Release final trigger of synch when done with all files
		localDownloadCount--;
	}

	IEnumerator ApiDownloadLocalZip (string url)
	{
		WWW www = new WWW (url);

		yield return www;

		Debug.Log (www.bytes.Length);
		if (www.bytes.Length > 0) {
			Stream byteStream = new MemoryStream (www.bytes);
			ZipInputStream zipStream = new ZipInputStream (byteStream);
			if (zipStream.CanRead && avatar != null) {
				ZipEntry entry;
				while ((entry = zipStream.GetNextEntry ()) != null) {
					int numBytes = (int)entry.Size;
					if (numBytes > 0) {
						byte[] data = new byte[numBytes];
						zipStream.Read (data, 0, numBytes);
						ApiApplyData (entry.Name, data);
					}
				}

				ApiDownloadSynchRig ();

				Debug.Log ("Applied Solved Avatar Rig");
			} else {
				Debug.LogError ("Can't load Solved Avatar Rig");
			}
		}

		FlushLog ("Avatar Rig Local Solve Apply");
	}

	// This is to be used in the future once we can properly extract local archives
	//	there is still an issue with each entry size being incorrect
	string localZipPath = "";
	bool localZipDownload = false;
	bool localZipDownloadDone = false;

	public void DownloadLocalZip ()
	{
		if (localZipPath != "") {
			string path = Application.streamingAssetsPath;
			string url;
			if (Application.platform == RuntimePlatform.Android) {
				url = System.IO.Path.Combine (path, localZipPath);
			} else {
				url = "file:///" + System.IO.Path.Combine (path, localZipPath);
			}
					
			StartCoroutine (ApiDownloadLocalZip (url));
		}
	}


	IEnumerator ApiDownloadRig ()
	{
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers ["Authorization"] = "Bearer " + Loom.apiKey;
		string url = GetUrl(ServerGetAvatarId () + "/download") + "?type=" + ((GetApiStyle() != "") ? "stylizedSolver" : "solver");
		WWW www = new WWW (url, null, headers);

		yield return www;

		string contentType = www.responseHeaders ["CONTENT-TYPE"];
		if (www.error != null) {
			Debug.LogError ("Can't load Solved Avatar Rig: " + www.error);
		} else if (contentType == "application/json,application/zip" || contentType == "application/zip") {
			Stream byteStream = new MemoryStream (www.bytes);
			ZipInputStream zipStream = new ZipInputStream (byteStream);
			if (zipStream.CanRead && avatar != null) {
				ZipEntry entry;
				while ((entry = zipStream.GetNextEntry ()) != null) {
					int numBytes = (int)entry.Size;
					if (numBytes > 0) {
						byte[] data = new byte[numBytes];
						zipStream.Read (data, 0, numBytes);

						ApiApplyData (entry.Name, data);
					}
				}

				ApiDownloadSynchRig ();

				Debug.Log ("Applied Solved Avatar Rig");
			} else {
				Debug.LogError ("Can't load Solved Avatar Rig");
			}
		} else {
			// It may be a redirect issue, try again from the LOCATION in the response
			string location = www.responseHeaders ["LOCATION"];
			if (location != null && location != "") {
				www = new WWW (location);
				yield return www;

				Debug.Log ("Redirecting Download URL: " + location);

				contentType = www.responseHeaders ["CONTENT-TYPE"];
				if (contentType == "application/json,application/zip" || contentType == "application/zip") {
					Stream byteStream = new MemoryStream (www.bytes);
					ZipInputStream zipStream = new ZipInputStream (byteStream);
					if (zipStream.CanRead && avatar != null) {
						ZipEntry entry;
						while ((entry = zipStream.GetNextEntry ()) != null) {
							int numBytes = (int)entry.Size;
							if (numBytes > 0) {
								byte[] data = new byte[numBytes];
								zipStream.Read (data, 0, numBytes);

								ApiApplyData (entry.Name, data);
							}
						}

						ApiDownloadSynchRig ();

						Debug.Log ("Applied Solved Avatar Rig");
					}
				}
			} else {
				Debug.LogError ("Can't load Solved Avatar Rig, invalid data returned: " + contentType);			
			}
		}

		FlushLog ("Avatar Rig Solve Apply");
	}

	IEnumerator DownloadRig ()
	{
		yield return StartCoroutine (ApiDownloadRig ());
		yield return StartCoroutine (ApiDownloadTextures ());
	}

	public void ServerDownloadRig ()
	{
		StartCoroutine (DownloadRig ());
	}

	public void LocalDownloadRig ()
	{
		ApiDownloadRigLocalFiles ();
	}

	IEnumerator ApiGetAvatarStatus ()
	{
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers ["Authorization"] = "Bearer " + Loom.apiKey;

		string url = GetUrl(ServerGetAvatarId ());
		WWW www = new WWW (url, null, headers);
		yield return www;

		string jsonString = System.Text.Encoding.UTF8.GetString (www.bytes, 0, www.bytes.Length);
		ApiAvatarStatusData data = ApiAvatarStatusData.CreateFromJSON (jsonString);
		if (data != null) {
			if (data.status == "ready") {
				uploadPending = false;
				serverCanUpload = true;
				Debug.Log ("Avatar is ready, downloading...");
				if (serverCanDownload) {
					serverCanDownload = false;
					ServerDownloadRig ();
				}
			} else if (data.status == "pending" || data.status == "processing") {
				Debug.Log ("Avatar is processing...");
				serverCanCheck = true;
				serverCanDownload = true;
			} else {
				uploadPending = false;
				if (data.error != null && data.error != "") {
					Debug.LogError ("Avatar status error: " + data.error);
				} else {
					Debug.LogError ("Avatar unknown server error");
				}
			}
		} else if (www.error != null) {
			Debug.LogError ("Avatar status error: " + www.error);
		}
	}

	IEnumerator DownloadStatus ()
	{
		yield return StartCoroutine (ApiGetAvatarStatus ());
	}

	public void ServerCheckAvatar ()
	{
		// Check status no more than once every 5 seconds
		if (serverCanCheck && ((Time.fixedTime - serverCheckTime) >= 5)) {
			if (ServerGetAvatarId () != "") {
				serverCanCheck = false;
				serverCheckTime = Time.fixedTime;
				StartCoroutine (DownloadStatus ());
			}
		}
	}

	IEnumerator ApiUploadAvatar (Texture2D texture)
	{
		WWWForm form = new WWWForm();
		form.AddBinaryData ("sourceImage", texture.EncodeToJPG(), "image.png", "image/png");

		// Avatar specialization
		if (GetApiStyle() != "") {
			form.AddField ("style", GetApiStyle());
		}
		if (Loom.apiFacs) {
			form.AddField ("facs", "true");
		}
		Dictionary<string, string> headers = form.headers;
		byte[] rawData = form.data;
		headers["Authorization"] = "Bearer " + Loom.apiKey;

		string url = GetUrl ("", true);
		Debug.Log ("Avatar Upload Calling " + url);
		WWW www = new WWW (url, rawData, headers);
		yield return www;

		string jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);
		ApiAvatarData data = ApiAvatarData.CreateFromJSON (jsonString);

		if (www.error != null) {
			// {"status": "404 Not Found", "message": "Missing parameters: sourceImage"} 
			if (data != null) {
				if (data.message != null) {
					Debug.LogError ("Avatar Upload Error: " + data.message);
				} else if (data.status != null) {
					Debug.LogError ("Avatar Upload Error: " + data.status);
				} else {
					Debug.LogError ("Avatar Upload Error: " + www.error);
				}
			} else {
				Debug.LogError ("Avatar Upload Error: " + www.error);
			}
		} else {
			// { "avatar_id": 5104436393279488, "status": "pending" }
			if (data.avatar_id != null) {
				avatarId = data.avatar_id;
				PlayerPrefs.SetString (name + "_avatar_id", avatarId);
				Debug.Log ("Avatar Uploaded " + avatarId);
				serverCanCheck = true;
			}
		}
	}

	public void ServerUploadSelfie ()
	{	
		if (serverCanUpload) {
			uploadSelfie = false;

			Texture2D texture = null;
			Transform selfie = transform.Find ("Selfie");
			if (selfie != null) {
				SpriteRenderer renderer = selfie.gameObject.GetComponent<SpriteRenderer> ();
				if (renderer != null) {
					Sprite sprite = renderer.sprite;
					if (sprite != null) {
						texture = sprite.texture;
					}
				}
			}

			if (texture != null) {
				serverCanUpload = false;
				uploadStartTime = Time.fixedTime;
				uploadEndTime = uploadStartTime + 120;
				uploadPending = true;
				StartCoroutine (ApiUploadAvatar (texture));
			}
		}
	}

	public string ServerGetAvatarIdRaw () {
		return PlayerPrefs.GetString (name + "_avatar_id");
	}

	public bool ServerCanDownload () {
		return ServerGetAvatarIdRaw () != "" && serverCanDownload;
	}

	public bool ServerCanDownloadDefault () {
		return defaultAvatarId != "" && serverCanDownload;
	}

	public string ServerGetAvatarId () {
		if (avatarId == "" && serverDownload) {
			// Check on uploaded AvatarId for this object
			avatarId = ServerGetAvatarIdRaw();
		}
		return avatarId;
	}
}
	
[System.Serializable]
public class ApiAvatarData
{
	public string message;
	public string status;
	public string avatar_id;

	public static ApiAvatarData CreateFromJSON (string jsonString)
	{
		return JsonUtility.FromJson<ApiAvatarData> (jsonString);
	}
}

[System.Serializable]
public class ApiAvatarStatusData
{
	public string ethnicity;
	public string gender;
	public string status;
	public string error;

	public static ApiAvatarStatusData CreateFromJSON (string jsonString)
	{
		return JsonUtility.FromJson<ApiAvatarStatusData> (jsonString);
	}
}

[System.Serializable]
public class ApiAvatarEyesData
{
	public float[] irisScale;
	public float[] reyeTranslation;
	public float[] leyeTranslation;

	public static ApiAvatarEyesData CreateFromJSON (string jsonString)
	{
		return JsonUtility.FromJson<ApiAvatarEyesData> (jsonString);
	}
}

