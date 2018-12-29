using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonScript : MonoBehaviour
{

    public int Clothing_categories;//0为男,1为女,2为卡通   -1不为衣服种类
    public ModelSelector modelSelector;
    // public ColthingControler instence;       
    ParticleSystem toonMacige;               //粒子控制器
    public string fileName;                  //模型的文件夹
    [Tooltip("本类模型的总数量")]
    public int mouldCount;                   //模型的数量
    private static int selectorcount;        //当前选择的模型的最大数量
    private static string selector;          //当前选择的模型列表
    private static int modelIndex;                  //模型索引
    float time = 0;                          //时间

    // Use this for initialization
    void Start()
    {
        toonMacige = transform.parent.parent.Find("ToonMagic").GetComponent<ParticleSystem>();
        KinectManager KM = KinectManager.Instance;
        modelSelector = KM.gameObject.GetComponent<ModelSelector>();
        //this.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener((Ison) => { Selected(Ison); });
    }
    public void Selected(bool ison)
    {
        this.transform.GetChild(2).GetComponent<Image>().enabled = ison;
        //if (ison)
        //{
        //    return;
        //}
        if (ison && this.Clothing_categories >= 0)
        {
            selector = this.fileName;         //令当前选择的模型列表为选中的文件夹里的内容
            selectorcount = this.mouldCount;  //令当前选择的模型列表的最大数量为选中种类的最大数量
            SelectedState();//选中后UI更改的状态
            //TODO 选中按钮的操作
            modelIndex = Random.Range(0, selectorcount); //当前选项范围的随机衣服.

            Debug.Log("选择");

            StartCoroutine(LoadModel());//延迟加载服装
        }
    }
    IEnumerator LoadModel()
    {
        switch (selector)
        {
            case "woman":
                switch (modelIndex)
                {
                    case 0:
                        Adaptation(-0.041f, 0.101f, 1.11f, 0.88f, 1.04f, 0.84f);
                        break;
                    case 1:
                        Adaptation(0.076f, 0.107f, 1.04f, 0.84f, 0.74f, 0.57f);
                        break;
                    case 2:
                        Adaptation(0.056f, 0.01f, 1.05f, 1.05f, 1f, 0.9f);
                        break;
                    case 3:
                        Adaptation(0f, 0f, 1.05f, 1.05f, 1f, 0.9f);
                        break;
                    case 4:
                        Adaptation(0f, 0f, 1.05f, 1.05f, 1f, 0.9f);
                        break;
                    case 5:
                        Adaptation(0.077f, 0.129f, 1.05f, 0.97f, 1.01f, 1.19f);
                        break;
                    case 6:
                        Adaptation(0.02f, 0.071f, 1.15f, 0.92f, 0.99f, 0.91f);
                        break;
                    default:
                        break;
                }
                break;
            default:
                Adaptation(0f, 0f, 1.05f, 1.05f, 1f, 0.9f);
                break;
        }
        yield return new WaitForSeconds(0.3f);

        modelSelector.OnDressingItemSelected(modelIndex, selector);
    }

    void Adaptation(float vertical, float forward, float bodyScale, float bodyWidth, float armScale, float legScale)
    {
        modelSelector.verticalOffset = vertical;
        modelSelector.forwardOffset = forward;
        modelSelector.bodyScaleFactor = bodyScale;
        modelSelector.bodyWidthFactor = bodyWidth;
        modelSelector.armScaleFactor = armScale;
        modelSelector.legScaleFactor = legScale;
    }
    // Update is called once per frame



    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.transform.GetChild(0).localScale = new Vector3(1.3f, 1.3f, 1);
        this.transform.GetChild(1).localScale = new Vector3(1.5f, 1.5f, 1);
        this.transform.GetChild(3).localScale = new Vector3(1.5f, 1.5f, 1);
        time = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // toonMacige.Stop();

        switch (Clothing_categories)
        {
            case 0://男

            case 1://女

            case 2://卡通

                if (this.transform.GetChild(2).GetComponent<Toggle>().isOn == false)
                {

                    time += Time.deltaTime;
                    this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                    this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;

                    if (time > 2)
                    {
                        Debug.Log("haha");
                        this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                    }
                }

                break;
            case -1://上一个
                time += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
                if (time > 2)
                {
                    this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;

                    SelectedState();//选中后UI更改的状态

                    //TODO 选中按钮的操作
                    modelIndex += 1;//令选中的索引加一
                    if (modelIndex >= selectorcount)
                        modelIndex = 0;//如果超出索引则循环
                    Debug.Log(modelIndex + " 1 上 " + selector + "  2 " + selectorcount);
                    StartCoroutine(LoadModel());
                }
                break;
            case -2://下一个
                time += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
                if (time > 2)
                {
                    SelectedState();//选中后UI更改的状态

                    this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                    //TODO 选中按钮的操作
                    modelIndex += -1;//令选中的索引加一
                    if (modelIndex < 0)
                        modelIndex = selectorcount - 1;
                    Debug.Log(modelIndex + " -1 下 " + selector + "  2 " + selectorcount);
                    StartCoroutine(LoadModel());

                }
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        time = 0;
        this.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1);
        this.transform.GetChild(1).localScale = new Vector3(1f, 1f, 1);
        this.transform.GetChild(3).localScale = new Vector3(1f, 1f, 1);
        this.transform.GetChild(1).GetComponent<Image>().enabled = false;
        this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
    }
    private void SelectedState()
    {
        toonMacige.Play();
        this.transform.GetChild(0).transform.localScale = Vector3.one;
        this.transform.GetChild(1).transform.localScale = Vector3.one;
        this.transform.GetChild(3).transform.localScale = Vector3.one;
        this.transform.GetChild(1).GetComponent<Image>().enabled = false;
        this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
        time = 0;
    }
}
