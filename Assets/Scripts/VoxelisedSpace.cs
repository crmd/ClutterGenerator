using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class VoxelisedSpace {

	public bool[,,] voxels;

	public VoxelisedSpace()
	{
		voxels = new bool[0,0,0];
	}

	public void GenerateVoxels(Transform trans, float voxelSize, int numVoxelsX, int numVoxelsY, int numVoxelsZ, List<IgnoreArea> ignoreAreas)
	{
		voxels = new bool[numVoxelsX,numVoxelsY,numVoxelsZ];
		int x = voxels.GetLength(0);
		int y = voxels.GetLength(1);
		int z = voxels.GetLength(2);

		float stride = voxelSize;
		Vector3 startPos = (trans.position - new Vector3(x/2.0f*voxelSize, y/2.0f*voxelSize, z/2.0f*voxelSize)) + (Vector3.one * stride/2.0f);

		for(int i = 0; i < z; ++i)
		{
			for(int j = 0; j  < y; ++j)
			{
				for(int k = 0; k < x; ++k)
				{
					Vector3 pos = startPos + new Vector3(k*stride, j*stride, i*stride);
					pos = (Quaternion.Euler(trans.rotation.eulerAngles) * (pos - trans.position)) + trans.position;
					if(CheckCollision(pos, voxelSize, ignoreAreas))
					{
						voxels[k,j,i] = true;
					}
				}
			}
			float progress = ((float)i/z);
			if(progress > 0 && progress < voxels.Length)
			{
				EditorUtility.DisplayProgressBar("Generating Voxels",
				                                 "Generating " + voxels.Length + " voxels, this may take some time.", 
				                                 progress); 
			}
		}
		EditorUtility.ClearProgressBar();
	}

	//Check collisions
	bool CheckCollision(Vector3 pos, float voxelSize, List<IgnoreArea> ignoreAreas)
	{
		if(Physics.OverlapSphere(pos, (voxelSize/2) + 0.0001f).Length > 0)
		{
			return true;
		}
		else
		{
			for(int i = 0; i < ignoreAreas.Count; ++i)
			{
				if(CheckBounds(pos, voxelSize, ignoreAreas[i].pos, ignoreAreas[i].size))
				{
					return true;
				}
			}
		}
		return false;
	}

	//Bounding box collision check
	bool CheckBounds(Vector3 voxelPos, float voxelSize, Vector3 iaPos, Vector3 iaSize)
	{
		Debug.Log ("This isn't set up yet");
		return false;
//		Vector3 voxelMinBounds = new Vector3(voxelPos.x - voxelSize/2, voxelPos.y - voxelSize/2, voxelPos.z - voxelSize/2);
//		Vector3 voxelMaxBounds = new Vector3(voxelPos.x + voxelSize/2, voxelPos.y + voxelSize/2, voxelPos.z + voxelSize/2);
//		Vector3 iaMinBounds = new Vector3(iaPos.x - iaSize.x/2, iaPos.y - iaSize.y/2, iaPos.z - iaSize.z/2);
//		Vector3 iaMaxBounds = new Vector3(iaPos.x + iaSize.x/2, iaPos.y + iaSize.y/2, iaPos.z + iaSize.z/2);
//
//		if(/*SpookyShit*/false)
//		{
//			return true;
//		}
//		return false;
	}

	void RandomVoxels()
	{
		for(int i = 0; i < Random.Range (0 ,voxels.Length); ++i)
		{
			voxels[Random.Range(0, voxels.GetLength(0)), Random.Range(0, voxels.GetLength(1)), Random.Range(0, voxels.GetLength(2))] = true;
		}
	}
}
