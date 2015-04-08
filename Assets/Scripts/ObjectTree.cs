using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectTree : MonoBehaviour {

	public bool ignoreClutter;			//
	public List<GameObject> branches;	//
	public float maxBranchDist;			//
	public float minBranchDist;			//

	void Start()
	{
		for(int i = 0; i < 10; ++i)
		{
			PlaceBranch();
		}
	}

	public void PlaceBranch ()
	{
		Vector3 pos = new Vector3(Random.Range(-1,1) * Random.Range(minBranchDist, maxBranchDist),
		                          Random.Range(-1,1) * Random.Range(minBranchDist, maxBranchDist),
		                          Random.Range(-1,1) * Random.Range(minBranchDist, maxBranchDist));
		//pos -= pos/2;
		pos += gameObject.transform.position;
		GameObject.Instantiate (branches[Random.Range(0, branches.Count)], pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
	}
}
