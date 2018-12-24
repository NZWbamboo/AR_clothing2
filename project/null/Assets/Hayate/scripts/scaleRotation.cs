using UnityEngine;
using System.Collections;

public class scaleRotation : MonoBehaviour {

	float p = 0;
	public float speed;
	
	Vector3 oS;
	
	void Awake()
	{
		
		oS = transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.localScale = new Vector3( .1f + oS.x + Mathf.Sin(p) / 2, .1f + oS.y + Mathf.Sin(p) / 2, .1f + oS.z + Mathf.Sin(p) / 2);
		
		p += speed * Time.deltaTime;
		
		transform.Rotate(new Vector3(1,1,0) * 20 * Time.deltaTime);
		
	}
}
