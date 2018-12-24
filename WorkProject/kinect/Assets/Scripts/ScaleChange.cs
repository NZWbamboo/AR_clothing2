using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
        A  = DOTween.Sequence();
    }
    Sequence A;
      
    // Update is called once per frame
    void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hahaha");
       
        A.Append(this.transform.DOScale( new Vector3(0.1f, 0.1f, 1),1));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        A.Append(this.transform.DOScale(new Vector3(1f, 1f, 1), 1));
    }
   
}
