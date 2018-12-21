using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {
    public Image clothingImage;
    public int Clothing_categories;//0为男,1为女,2为卡通   -1不为衣服种类
    public ModelSelector modelSelector;
    public int UPorDOWN;
    public ColthingControler instence;
    int clothinIndex;
	// Use this for initialization
	void Start () {
        KinectManager KM = KinectManager.Instance;
        modelSelector = KM.gameObject.GetComponent<ModelSelector>();
        this.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener((ison)=> { Selected(ison); });
       // clothingImage = GameObject.Find("UI-Canvas/clothing").GetComponent<Image>();
        //instence= GameObject.Find("UI-Canvas/clothing").GetComponent<ColthingControler>();
       // clothinIndex = Random.Range(0, instence.ColthingSprite.Length);
      //  clothingImage.sprite = instence.ColthingSprite[clothinIndex];
    }
    void Selected(bool ison)
    {
       
       
            this.transform.GetChild(2).GetComponent<Image>().enabled = ison;
        
        
    }

    // Update is called once per frame

    float time = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        time = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (Clothing_categories>=0)//是服装种类按钮
        {
            if (this.transform.GetChild(2).GetComponent<Toggle>().isOn)
            {
                return;
            }
            time += Time.deltaTime;
            this.transform.GetChild(1).GetComponent<Image>().enabled =true;
            this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
            if (time > 2)
            {
                this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                this.transform.GetChild(1).GetComponent<Image>().enabled = false;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount =0;
                time = 0;
                //TODO 选中按钮的操作

                clothingImage.sprite = instence.ColthingSprite[Mathf.Abs(Clothing_categories) % 3];
                clothinIndex = Clothing_categories;
                   
            }
        }
        
       
      
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.transform.GetChild(1).GetComponent<Image>().enabled = false;
        this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount =0;
    }


}
