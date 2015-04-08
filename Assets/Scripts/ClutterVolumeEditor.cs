using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ClutterVolume))]
public class ClutterVolumeEditor : Editor
{
	SerializedProperty lengthProp;		//The length of the clutter volume
	SerializedProperty depthProp;		//The depth of the clutter volume
	SerializedProperty heightProp;		//The height of the clutter volume
	SerializedProperty voxelSizeProp;	//The size of each voxel
	ClutterVolume cVolume;				//Reference to the actual clutter volume script

	SerializedProperty ignoreAreasProp;	//The list of ignore areas

	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		cVolume = (ClutterVolume)target;

		lengthProp = serializedObject.FindProperty ("length");
		depthProp = serializedObject.FindProperty ("depth");
		heightProp = serializedObject.FindProperty ("height");
		voxelSizeProp = serializedObject.FindProperty ("voxelSize");

		ignoreAreasProp = serializedObject.FindProperty ("ignoreAreas");

		float minSize = Mathf.Min (cVolume.height, cVolume.length, cVolume.height);
		cVolume.voxelSize = Mathf.Clamp(cVolume.voxelSize, 0.01f, minSize);

		//Clamp the rotation
		cVolume.transform.eulerAngles = new Vector3(Mathf.Clamp(cVolume.transform.rotation.x, 0.0f, 0.0f),
		                                            cVolume.transform.eulerAngles.y,
		                                            Mathf.Clamp(cVolume.transform.rotation.z, 0.0f, 0.0f));

		//Clutter Object size sliders
		EditorGUILayout.Slider(voxelSizeProp, 0.01f, minSize, new GUIContent ("Voxel Size"));
		EditorGUILayout.IntSlider(lengthProp, 1, 100, new GUIContent ("Length"));
		EditorGUILayout.IntSlider(heightProp, 1, 100, new GUIContent ("Height"));
		EditorGUILayout.IntSlider(depthProp, 1, 100, new GUIContent ("Depth"));

		//serializedObject.ApplyModifiedProperties ();

		//toggle drawing the voxels
		cVolume.drawVoxelSolid = EditorGUILayout.Toggle ("Draw Voxels", cVolume.drawVoxelSolid);
		cVolume.drawVoxelWire = EditorGUILayout.Toggle ("Draw Wireframe Voxels", cVolume.drawVoxelWire);

		//Calculate the number of voxels that will be drawn
		int numVoxels = (int)(cVolume.length / cVolume.voxelSize) *
			(int)(cVolume.height / cVolume.voxelSize) *
				(int)(cVolume.depth / cVolume.voxelSize);

		EditorGUILayout.HelpBox("Warning: Number of voxels created will be " + /*cVolume.vSpace.voxels.Length*/numVoxels, MessageType.Info);

		//Recalulate the voxels
		if(GUILayout.Button("Recalculate Voxels"))
		{
			if(cVolume.voxelSize <= cVolume.length
			   && cVolume.voxelSize <= cVolume.height
			   && cVolume.voxelSize <= cVolume.depth)
			{
				cVolume.RecalculateVoxels();
				EditorUtility.SetDirty(cVolume);
			}
			else
			{
				Debug.Log ("Voxel Size cannot be larger than Length/Width/Height");
			}
		}

		if(GUILayout.Button("Remove Clutter Volumes"))
		{
			if(cVolume.voxelSize <= cVolume.length
			   && cVolume.voxelSize <= cVolume.height
			   && cVolume.voxelSize <= cVolume.depth)
			{
				cVolume.RecalculateVoxels();
				EditorUtility.SetDirty(cVolume);
			}
		}

		if(GUILayout.Button("Add Ignore Volume"))
		{
			cVolume.ignoreAreas.Add(new IgnoreArea());
		}
		if(cVolume.ignoreAreas.Count > 0)
		{
			EditorGUILayout.PropertyField(ignoreAreasProp, new GUIContent ("Ignore Volumes"), true);
		}
		serializedObject.ApplyModifiedProperties ();
	}

	void OnInspectorUpdate() {
		Repaint();
	}
}
