using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public Transform GO;
	
	private bool startAnimation;
	
	private int levelIndex = 0;
	
	IEnumerator Start()
	{
		
		DontDestroyOnLoad(gameObject);
		
		GO.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,0);
		
		yield return StartCoroutine (waitAndStart());
		
	}
	
	void Update()
	{
		
		if(startAnimation)
		{
			
			GO.GetComponent<Renderer>().material.color = Color.Lerp (GO.GetComponent<Renderer>().material.color, new Color(1f,1f,1f,1f), 0.5f * Time.deltaTime);
			
		}
		
	}
	
	IEnumerator waitAndStart()
	{
		
		yield return new WaitForSeconds(8.8f);
		
		startAnimation = true;
		
	}
	
	void OnGUI()
	{
		
		if(startAnimation)
		{
			
			if(levelIndex == 0)
			{
				
				if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 1.2f, 200, 30 ), "START"))
				{
					
					startDemo();
					
				}
				
			}else{
				
				if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 1.2f, 200, 30 ), "NEXT DEMO"))
				{
					
					startDemo();
					
				}
				
			}
			
			
		}
		
		
	}
	
	void startDemo()
	{
		
		if(levelIndex >= 9)
		{
			
			levelIndex = 0;
			
			Application.LoadLevel(levelIndex);
			
			Destroy (gameObject);
			
		}else{
			
			transform.position = new Vector3(0,-1000f,0);
			
			levelIndex++;
			
			Application.LoadLevel(levelIndex);
			
		}
		
		
		
	}
	
}
