using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Image clothingImage;
    public int Clothing_categories;//0为男,1为女,2为卡通   -1不为衣服种类
    public ModelSelector modelSelector;
    public int UPorDOWN;
    public ColthingControler instence;
    int clothinIndex;
    // Use this for initialization
    void Start()
    {
        KinectManager KM = KinectManager.Instance;
        modelSelector = KM.gameObject.GetComponent<ModelSelector>();
        this.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener((ison) => { Selected(ison); });
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
    public GameObject[] nan;
    public GameObject[] nv;
    public GameObject[] katon;
    private GameObject[] selector;
    private int modelIndex;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (Clothing_categories)
        {
            case 0://男

            case 1://女

            case 2://卡通
                if (this.transform.GetChild(2).GetComponent<Toggle>().isOn)
                {
                    return;
                }
                switch (Clothing_categories)
                {
                    case 0://男
                        selector = nan;
                        break;
                    case 1://女
                        selector = nv;
                        break;
                    case 2://卡通
                        selector = katon;
                        break;
                    default:
                        break;
                }
                time += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
                if (time > 2)
                {
                    this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                    this.transform.GetChild(1).GetComponent<Image>().enabled = false;
                    this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
                    time = 0;
                    //TODO 选中按钮的操作
                    modelIndex = Random.Range(0, selector.Length);
                    LoadModel(modelIndex,selector);
                   // clothingImage.sprite = instence.ColthingSprite[Mathf.Abs(Clothing_categories) % 3];
                   // clothinIndex = Clothing_categories;

                }
                break;
            case -1://上一个
                if (this.transform.GetChild(2).GetComponent<Toggle>().isOn)
                {
                    return;
                }
                time += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
                if (time > 2)
                {
                    this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                    this.transform.GetChild(1).GetComponent<Image>().enabled = false;
                    this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
                    time = 0;
                    //TODO 选中按钮的操作

                    modelIndex++;//令选中的索引加一
                    if (modelIndex >= selector.Length)
                        modelIndex = 0;//如果超出索引则循环
                    LoadNextModel(modelIndex,selector);
                    //  clothingImage.sprite = instence.ColthingSprite[Mathf.Abs(Clothing_categories) % 3];
                    //  clothinIndex = Clothing_categories;

                }
                break;
            case -2://下一个
                if (this.transform.GetChild(2).GetComponent<Toggle>().isOn)
                {
                    return;
                }
                time += Time.deltaTime;
                this.transform.GetChild(1).GetComponent<Image>().enabled = true;
                this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = time * 0.5f;
                if (time > 2)
                {
                    this.transform.GetChild(2).GetComponent<Toggle>().isOn = true;
                    this.transform.GetChild(1).GetComponent<Image>().enabled = false;
                    this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
                    time = 0;
                    //TODO 选中按钮的操作
                    modelIndex--;//令选中的索引加一
                    if (modelIndex < 0)
                        modelIndex = selector.Length - 1;
                    LoadPreviousModel(modelIndex,selector);
                    // clothingImage.sprite = instence.ColthingSprite[Mathf.Abs(Clothing_categories) % 3];
                    // clothinIndex = Clothing_categories;
                }
                break;
            default:
                break;
        }
    }
    private void LoadModel(int index, GameObject[] clothing)
    {
        selModel = clothing[index];
    }
    private void LoadNextModel(int index, GameObject[] clothing)
    {
        selModel = clothing[index];
    }
    private void LoadPreviousModel(int index, GameObject[] clothing)
    {
        selModel = clothing[index];
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.transform.GetChild(1).GetComponent<Image>().enabled = false;
        this.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 0;
    }
    public string modelCategory = "Clothing";
    private GameObject selModel;//当前选择的模型
    public int playerIndex = 0;
    // sets the selected dressing model as user avatar
    //设置所选模型打扮成用户化身
//    private void LoadDressingModel(string modelDir)
//    {
//        string modelPath = modelCategory + "/" + modelDir + "/model";
//        UnityEngine.Object modelPrefab = Resources.Load(modelPath, typeof(GameObject));//加载模型
//                                                                                       //   modelPrefab = null;
//        Debug.Log("Model: 不加载模型" + modelPath);
//        if (modelPrefab == null)
//            return;

//        Debug.Log("Model: " + modelPath);

//        if (selModel != null)
//        {
//            GameObject.Destroy(selModel);
//        }

//        selModel = (GameObject)GameObject.Instantiate(modelPrefab, Vector3.zero, Quaternion.Euler(0, 180f, 0));
//        selModel.name = "Model" + modelDir;

//        AvatarController ac = selModel.GetComponent<AvatarController>();
//        if (ac == null) //添加AvatarController
//        {
//            ac = selModel.AddComponent<AvatarController>();
//            ac.playerIndex = playerIndex;

//            ac.mirroredMovement = true;
//            ac.verticalMovement = true;

//            ac.verticalOffset = verticalOffset;
//            ac.forwardOffset = forwardOffset;
//            ac.smoothFactor = 0f;
//        }

//        ac.posRelativeToCamera = modelRelativeToCamera;
//        ac.posRelOverlayColor = (foregroundCamera != null);

//        KinectManager km = KinectManager.Instance;
//        //ac.Awake();

//        if (km && km.IsInitialized())
//        {
//            long userId = km.GetUserIdByIndex(playerIndex);
//            if (userId != 0)
//            {
//                ac.SuccessfulCalibration(userId, false);//校准
//            }

//            // locate the available avatar controllers
//            //找到可用的avatar
//            MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
//            km.avatarControllers.Clear();

//            foreach (MonoBehaviour monoScript in monoScripts)
//            {
//                if ((monoScript is AvatarController) && monoScript.enabled)
//                {
//                    AvatarController avatar = (AvatarController)monoScript;
//                    km.avatarControllers.Add(avatar);
//                }
//            }
//        }

//        AvatarScaler scaler = selModel.GetComponent<AvatarScaler>();//于模型上添加AvatarScaler脚本
//        if (scaler == null)
//        {
//            scaler = selModel.AddComponent<AvatarScaler>();
//            scaler.playerIndex = playerIndex;
//            scaler.mirroredAvatar = true;

//            scaler.continuousScaling = continuousScaling;
//            scaler.bodyScaleFactor = bodyScaleFactor;
//            scaler.bodyWidthFactor = bodyWidthFactor;
//            scaler.armScaleFactor = armScaleFactor;
//            scaler.legScaleFactor = legScaleFactor;
//        }

//        scaler.foregroundCamera = foregroundCamera;
//        //scaler.debugText = debugText;

//        //scaler.Start();
//    }

}
