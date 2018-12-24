using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public float rot;
	
	// Update is called once per frame
	void Update () {
	
		transform.Rotate(new Vector3(0,rot * Time.deltaTime,0));
		
	}
}
