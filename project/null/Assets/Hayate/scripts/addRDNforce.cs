using UnityEngine;
using System.Collections;

public class addRDNforce : MonoBehaviour {

	public GameObject emitter;
	
	void Update()
	{
		
		if(Input.GetMouseButtonDown(0))
		{
			
			emitter.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f) * 10000, Random.Range(-1f,1f) * 10000, Random.Range(-1f,1f) * 10000));
			
		}
		
	}
}
