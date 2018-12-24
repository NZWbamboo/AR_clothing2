using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class NewBehaviourScript : MonoBehaviour {
    string url = "http://app.maymotion.com/apps/ceshi/photo/api.php?api=upl_photo";
    // Use this for initialization
    void Start () {
        StartCoroutine(haha());
	}
	IEnumerator haha()
    {
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.text);
        JsonData text = JsonMapper.ToObject(www.text);
      Debug.Log(text["msg"].ToString());
    }
	// Update is called once per frame
	void Update () {
		
	}
}
