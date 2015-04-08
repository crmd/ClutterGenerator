using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CVShape
{
	CVS_CUBE,
	CVS_Cylinder
}

public class ClutterVolume : MonoBehaviour
{
	public int density = 1;
	public float voxelSize = 1;
	public int radius = 1;
	public int height = 1;
	public int length = 1;
	public int depth = 1;

	public CVShape clutterVolumeShape;

	public VoxelisedSpace vSpace = new VoxelisedSpace();

	public bool drawVoxelSolid = false;
	public bool drawVoxelWire = false;

	public List<IgnoreArea> ignoreAreas;

	public void RecalculateVoxels()
	{
		if(vSpace == null)
		{
			vSpace = new VoxelisedSpace();
		}
		vSpace.GenerateVoxels(transform, voxelSize, (int)(length / voxelSize), (int)(height / voxelSize), (int)(depth /voxelSize), ignoreAreas);
	}

	void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		switch(clutterVolumeShape)
		{
		case CVShape.CVS_CUBE:
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(length, height, depth));
			break;
		case CVShape.CVS_Cylinder:
			Debug.Log ("Cannot draw Cylinder at this time");
			break;
		}

		Gizmos.color = Color.magenta;
		for(int i = 0; i < ignoreAreas.Count; ++i)
		{
			Gizmos.DrawWireCube(ignoreAreas[i].pos, ignoreAreas[i].size);
		}


		//Everything below this should be used for drawing the gizmos of the voxels
		if (!drawVoxelSolid && !drawVoxelWire)
		{
			return;
		}

		Gizmos.color = Color.red;
		int x = vSpace.voxels.GetLength(0);
		int y = vSpace.voxels.GetLength(1);
		int z = vSpace.voxels.GetLength(2);

		float stride = voxelSize;
		Vector3 startpos = (Vector3.zero - new Vector3(x/2.0f*voxelSize, y/2.0f*voxelSize, z/2.0f*voxelSize)) + (Vector3.one * stride/2.0f);

		for(int i = 0; i < z; ++i)
		{
			for(int j = 0; j  < y; ++j)
			{
				for(int k = 0; k < x; ++k)
				{
					if(vSpace.voxels[k,j,i])
					{
						Vector3 pos = startpos + new Vector3(k*stride, j*stride, i*stride);
						if(drawVoxelSolid)
						{
							Gizmos.color = Color.red;
							Gizmos.DrawCube(pos, Vector3.one * voxelSize);
						}
						if(drawVoxelWire)
						{
							Gizmos.color = Color.blue;
							Gizmos.DrawWireCube(pos, Vector3.one * voxelSize);
						}
					}
				}
			}
		}
	}
}
