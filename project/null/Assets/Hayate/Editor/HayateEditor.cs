using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof ( hayate ) ) ]
public class HayateEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		
		var Hayate = target as hayate;
		
		EditorGUILayout.LabelField( "Debug features ", GUILayout.Width(200));
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Draw turbulence field: ", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.debugDrawTurbulenceField.ToString()))
		{
			
			Hayate.debugDrawTurbulenceField = !Hayate.debugDrawTurbulenceField;
			
		}
		
		
		
		if(Hayate.debugDrawTurbulenceField)
		{
			
			Hayate.debugDetail = EditorGUILayout.IntSlider( "Length of axis", Hayate.debugDetail, 10, 100 );
			
		}
		EditorGUILayout.EndHorizontal();
		
		/*
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Debug lines:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.debugLines.ToString()))
		{
			
			Hayate.debugLines = !Hayate.debugLines;
			
		}
		
		EditorGUILayout.EndHorizontal();
		*/
		GUILayout.Space(30);
		
		EditorGUILayout.LabelField( "Turbulence settings" );
		
		//Hayate.manipulatePosition = EditorGUILayout.Toggle( "Manipulate Position", Hayate.manipulatePosition );
		
		EditorGUILayout.BeginHorizontal();
		
		
		
		
		EditorGUILayout.LabelField( "Manipulate: ", GUILayout.Width(200));
		
		
		if(Hayate.manipulatePosition)
		{
			
			if(GUILayout.Button ("Position"))
			{
				
				Hayate.manipulatePosition = false;
				
				Hayate.manipulateVelocity = true;
				
			}
			
		}else{
			
			if(GUILayout.Button ("Velocity"))
			{
				
				Hayate.manipulateVelocity = false;
				
				Hayate.manipulatePosition = true;
				
			}
			
			
		}

		EditorGUILayout.EndHorizontal();
		
		if(Hayate.manipulateVelocity)
		{
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField( "Absolute manipulation", GUILayout.Width(200));
			
			if(GUILayout.Button (Hayate.absolute.ToString()))
			{
				
				Hayate.absolute = !Hayate.absolute;
				
			}
			
			EditorGUILayout.EndHorizontal();
		
			if(Hayate.absolute)
			{
				
				EditorGUILayout.BeginHorizontal();
		
				EditorGUILayout.LabelField( "Speedmultiplyer:", GUILayout.Width(200));
				
				Hayate.speedMultiplier = EditorGUILayout.FloatField ("Strength", Hayate.speedMultiplier);
		
				EditorGUILayout.EndHorizontal();
			
			}
			
		}
		
		GUILayout.Space(15);;
		
		Hayate.amplitude = EditorGUILayout.Vector3Field( "Amplitude", Hayate.amplitude );
		
		Hayate.frequency = EditorGUILayout.FloatField( "Frequency", Hayate.frequency);
		
		GUILayout.Space(15);
		
		Hayate.globalForce = EditorGUILayout.Vector3Field( "Global force", Hayate.globalForce );
		
		GUILayout.Space(15);
		
		//Hayate.animateOffset = EditorGUILayout.Toggle( "Animate offset", Hayate.animateOffset );
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Animate offset:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.animateOffset.ToString()))
		{
			
			Hayate.animateOffset = !Hayate.animateOffset;
			
		}
		
		if(Hayate.animateOffset)
		{
			
			Hayate.offsetSpeed = EditorGUILayout.FloatField( "Speed", Hayate.offsetSpeed );
			
		}
		
		//Hayate.offset = EditorGUILayout.FloatField( "Offset", Hayate.offset );
	
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Randomize Offset at Start", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.randomizeOffset.ToString()))
		{
			
			Hayate.randomizeOffset = !Hayate.randomizeOffset;
			
		}
		
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		
		EditorGUILayout.LabelField( "Effects", GUILayout.Width(200) );
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Particles follow emitter:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.particlesFollowEmitter.ToString()))
		{
			
			Hayate.particlesFollowEmitter = !Hayate.particlesFollowEmitter;
			
		}
		
		if(Hayate.particlesFollowEmitter)
		{
			
			Hayate.followStrength = EditorGUILayout.FloatField ("Strength", Hayate.followStrength);
			
		}
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Burst on collision:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.burstOnCollision.ToString()))
		{
			
			Hayate.burstOnCollision = !Hayate.burstOnCollision;
			
		}
		
		if(Hayate.burstOnCollision)
		{
			
			Hayate.burstNum = EditorGUILayout.IntField ("Number of particles:", Hayate.burstNum);
			
		}
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Transform particle:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.emitTransform.ToString()))
		{
			
			Hayate.emitTransform = !Hayate.emitTransform;
			
			
		}
		
		if(Hayate.emitTransform)
		{
			
			Hayate.customTransform = EditorGUILayout.ObjectField(Hayate.customTransform, typeof( GameObject ) ,true) as GameObject;
			
		}
	
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Auto Destruct:", GUILayout.Width(200));
		
		if(GUILayout.Button (Hayate.autoDestruct.ToString()))
		{
			
			Hayate.autoDestruct = !Hayate.autoDestruct;
			
		}
		
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.LabelField( "Mesh target", GUILayout.Width(200) );
		
		if(GUILayout.Button (Hayate.moveToMesh.ToString()))
		{
			
			Hayate.moveToMesh = !Hayate.moveToMesh;
			
		}
		
		if(Hayate.moveToMesh)
		{
			
			Hayate.meshTarget = EditorGUILayout.ObjectField(Hayate.meshTarget, typeof( GameObject ) ,true) as GameObject;
			
		}
		
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		
		if(Hayate.moveToMesh)
		{
			
			EditorGUILayout.BeginHorizontal();
			
			Hayate.particleSpeedToMesh = EditorGUILayout.FloatField( "Speed to mesh", Hayate.particleSpeedToMesh);
			
			if(Hayate.byDistance)
			{
				
				if(GUILayout.Button ("By distance"))
				{
					
					Hayate.byDistance = !Hayate.byDistance;
					
				}
				
			}else{
				
				if(GUILayout.Button ("By time"))
				{
					
					Hayate.byDistance = !Hayate.byDistance;
					
				}
				
			}
			
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(30);
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField( "Subdivision Type:", GUILayout.Width(200) );
			
			if(Hayate.centerDivision)
			{
				
				if(GUILayout.Button ("Center"))
				{
					
					Hayate.centerDivision = !Hayate.centerDivision;
					
				}
				
			}else{
				
				if(GUILayout.Button ("Edge"))
				{
					
					Hayate.centerDivision = !Hayate.centerDivision;
					
				}
			}	
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
		
			Hayate.smallestTriangle = EditorGUILayout.FloatField( "Smallest Triangle", Hayate.smallestTriangle);
			
			EditorGUILayout.EndHorizontal();
			
			if(GUILayout.Button ("Retrieve mesh info"))
			{
				
				Hayate.retrieveMeshInfo();
				
			}
			
			if(GUILayout.Button ("Subdivide (Don't forget to make a backup of your mesh!)"))
			{
				
				Hayate.UpdateMeshCoordinates();
					
					
			}
		
			if(Hayate.resetMesh)
			{
				
				if(GUILayout.Button ("Reset mesh"))
				{
					
					Hayate.rstMesh();
						
						
				}
				
			}
			
		
			
		}
		
	}
	
	
}
