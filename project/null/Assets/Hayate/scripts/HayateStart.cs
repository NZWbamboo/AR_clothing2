using UnityEngine;
using System.Collections;

public class HayateStart : MonoBehaviour {
	
	hayate Hayate;
	
	private Vector3 tempGF;
	private Vector3 tempDS;
	
	void Start()
	{
		
		Hayate = transform.GetComponent<hayate>();
		
	}
	
	void OnGUI()
	{
		
		tempGF = new Vector3(Hayate.globalForce.x, Hayate.globalForce.y, Hayate.globalForce.z);
		
		tempDS = new Vector3(Hayate.amplitude.x, Hayate.amplitude.y, Hayate.amplitude.z);
		
		GUILayout.BeginVertical();
		
		GUILayout.Space(10);
		
		GUILayout.Label("HAYATE CONTROLS");
		
		if(GUILayout.Button ("HAYATE ENABLE" + " : "+ Hayate.enabled))
		{
			
			Hayate.enabled = !Hayate.enabled;
			
		}
		
		GUILayout.Label("");
		
		if(GUILayout.Button ("SWITCH LINES" + " : "+ Hayate.debugLines))
		{
			
			Hayate.debugLines = !Hayate.debugLines;
			
		}
		
		if(GUILayout.Button ("ANIMATE OFFSET" + " : "+ Hayate.animateOffset))
		{
			
			Hayate.animateOffset = !Hayate.animateOffset;
			
		}
		
		GUILayout.Label("OFFSET SPEED");
		
		Hayate.offsetSpeed = float.Parse(GUILayout.TextField(Hayate.offsetSpeed.ToString()));
		
		if(GUILayout.Button ("PARTICLES FOLLOW EMITTER" + " : "+ Hayate.particlesFollowEmitter))
		{
			
			Hayate.particlesFollowEmitter = !Hayate.particlesFollowEmitter;
			
		}
		
		
		
		GUILayout.Label("FOLLOW STRENGTH");
		
		Hayate.followStrength = float.Parse(GUILayout.TextField(Hayate.followStrength.ToString()));
		
		if(GUILayout.Button ("Manipulate Position" + " : "+ Hayate.manipulatePosition))
		{
			
			Hayate.manipulatePosition = !Hayate.manipulatePosition;
			
		}
		
		if(GUILayout.Button ("Manipulate Velocity" + " : "+ Hayate.manipulateVelocity))
		{
			
			Hayate.manipulateVelocity = !Hayate.manipulateVelocity;
			
			
		
		}
		
		if(Hayate.manipulateVelocity)
		{
				
			if(GUILayout.Button ("Absolute manipulation:" + " : "+ Hayate.absolute))
			{
				
				Hayate.absolute = !Hayate.absolute;
				
			}
				
		}
		
		GUILayout.Label("GLOBAL FORCE X Y Z");
		
		tempGF = new Vector3(float.Parse(GUILayout.TextField(Hayate.globalForce.x.ToString())), float.Parse(GUILayout.TextField(Hayate.globalForce.y.ToString())), float.Parse(GUILayout.TextField(Hayate.globalForce.z.ToString())));
		
		Hayate.globalForce = tempGF;
		
		GUILayout.Label("Amplitude X Y Z");
		
		tempDS = new Vector3(float.Parse(GUILayout.TextField(Hayate.amplitude.x.ToString())), float.Parse(GUILayout.TextField(Hayate.amplitude.y.ToString())), float.Parse(GUILayout.TextField(Hayate.amplitude.z.ToString())));
		
		Hayate.amplitude = tempDS;
		
		GUILayout.Label("Frequency");
		
		Hayate.frequency = float.Parse(GUILayout.TextField(Hayate.frequency.ToString()));
		
		GUILayout.Label(Hayate.offset.ToString());
		
		if(GUILayout.Button ("BURSTE ON COLLISION" + " : "+ Hayate.burstOnCollision))
		{
			
			Hayate.burstOnCollision = !Hayate.burstOnCollision;
			
		}
		
		GUILayout.Label("BURSTNUM");
		
		Hayate.burstNum = int.Parse(GUILayout.TextField(Hayate.burstNum.ToString()));
		
		GUILayout.EndVertical();
		
	}
	
	
}
