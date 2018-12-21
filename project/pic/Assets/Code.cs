using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Code : MonoBehaviour {
    public Image code;
	// Use this for initialization
	void Start () {
        Texture2D tex = new Texture2D(4, 4);
        tex.LoadImage(File.ReadAllBytes(Application.streamingAssetsPath + "/urlpic.png"));
        tex.Apply();
        code.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
