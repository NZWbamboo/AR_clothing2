using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Showpeop : MonoBehaviour {

	// Use this for initialization
	void Start () {
        KinectManager km = KinectManager.Instance;
        this.GetComponent<Image>().sprite = Sprite.Create(km.GetUsersClrTex(), new Rect(0, 0, km.GetUsersClrTex().width, km.GetUsersClrTex().height), new Vector2(0, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
