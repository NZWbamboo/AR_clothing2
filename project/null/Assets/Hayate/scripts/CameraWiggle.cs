using UnityEngine;
using System.Collections;

public class CameraWiggle : MonoBehaviour {
	
	// Bools
	public bool wiggleX;
	public bool wiggleY;
	public bool wiggleZ;
	
	public bool xCos;
	public bool yCos;
	public bool zCos;
	
	// Floats
	public float amountX;
	public float amountY;
	public float amountZ;
	public float animationSpeed;
	
	float animationProgres;
	
	// Vectors
	Vector3 tempVector;
	
	void Start()
	{
		
		tempVector = transform.position;
		
	}
	
	void Update () {
	
		if(wiggleX)
		{
			
			float temp = 0;

			if(xCos)
			{
				
				temp = tempVector.x + Mathf.Cos (animationProgres) * amountX;
				
			}else{
				
				temp = tempVector.x + Mathf.Sin (animationProgres) * amountX;
				
			}
			
			transform.position = new Vector3(temp, transform.position.y, transform.position.z);
			
		}
		
		if(wiggleY)
		{
			
			float temp = 0;

			if(yCos)
			{
				
				temp = tempVector.y + Mathf.Cos (animationProgres) * amountY;
				
			}else{
				
				temp = tempVector.y + Mathf.Sin (animationProgres) * amountY;
				
			}
			
			transform.position = new Vector3(transform.position.x, temp, transform.position.z);
			
		}
		
		if(wiggleZ)
		{
			
			float temp = 0;

			if(zCos)
			{
				
				temp = tempVector.z + Mathf.Cos (animationProgres) * amountZ;
				
			}else{
				
				temp = tempVector.z + Mathf.Sin (animationProgres) * amountZ;
				
			}
			
			transform.position = new Vector3(transform.position.x, transform.position.y, temp);
			
		}
		
		animationProgres += animationSpeed;
		
	}
}
