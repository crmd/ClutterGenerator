using UnityEngine;

[System.Serializable]
public class IgnoreArea {
	public Vector3 pos;
	public Vector3 size;
	public Vector3 rotation;	//Currently unused
	private Vector3[] vertices = new Vector3[8];

	public void UpdateVerts(Vector3 position, Vector3 rot)
	{
		for(int i = 0; i < 2; ++i)
		{
			for(int j = 0; j < 2; ++j)
			{
				for (int k = 0; k < 2; ++k)
				{
					vertices[j + (i*4)] = position + (pos - (size/2)) + new Vector3(((size.x/2) * (k%2)),
					                                                   ((size.y/2) * (j%2)),
					                                                   ((size.z/2) * (k%2)));
					//Debug.Log(vertices[k * (1 + j * (1 + i))]);
				}
			}
		}
	}
	public Vector3 GetVerts()
	{
		for(int i = 0; i < vertices.Length; ++i)
		{
			Debug.Log(vertices[i]);
		}
		return Vector3.zero;
	}
}