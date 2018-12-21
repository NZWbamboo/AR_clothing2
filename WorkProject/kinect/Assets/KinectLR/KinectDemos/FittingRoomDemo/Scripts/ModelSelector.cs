using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ModelSelector : MonoBehaviour
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;
    //模型的加载文件夹
    [Tooltip("The model category. Used for model discovery and title of the category menu.")]
    public string modelCategory = "Clothing";
    //现有的服装模特总数
    [Tooltip("Total number of the available clothing models.")]
    public int numberOfModels = 3;

    //	[Tooltip("Screen x-position of the model selection window. Negative values are considered relative to the screen width.")]
    //	public int windowScreenX = -160;
    //参考穿着菜单。
    [Tooltip("Reference to the dresing menu.")]
    public RectTransform dressingMenu;
    //菜单预制体
    [Tooltip("Reference to the dresing menu-item prefab.")]
    public GameObject dressingItemPrefab;
    //使初始模型相对于相机的位置，等于玩家相对于传感器的位置。
    [Tooltip("Makes the initial model position relative to this camera, to be equal to the player's position, relative to the sensor.")]
    public Camera modelRelativeToCamera = null;
    //用于估计模型在背景上的覆盖位置的照相机。
    [Tooltip("Camera used to estimate the overlay position of the model over the background.")]
    public Camera foregroundCamera;
    //当模型类别被更改时，是否保留选定的模型。
    [Tooltip("Whether to keep the selected model, when the model category gets changed.")]
    public bool keepSelectedModel = true;
    //在校准姿势完成后，刻度是否持续更新或仅更新一次。
    [Tooltip("Whether the scale is updated continuously or just once, after the calibration pose.")]
    public bool continuousScaling = true;
    //全身比例因子(包括身高、手臂和腿)，可用于身体比例的微调。
    [Tooltip("Full body scale factor (incl. height, arms and legs) that might be used for fine tuning of body-scale.")]
    [Range(0.0f, 2.0f)]
    public float bodyScaleFactor = 1.0f;
    //身体宽度比例因子，可用于微调的宽度比例。如果设置为0，身体尺度因子也将用于宽度。
    [Tooltip("Body width scale factor that might be used for fine tuning of the width scale. If set to 0, the body-scale factor will be used for the width, too.")]
    [Range(0.0f, 2.0f)]
    public float bodyWidthFactor = 1.0f;
    //可用于微调手臂比例尺的附加比例系数。
    [Tooltip("Additional scale factor for arms that might be used for fine tuning of arm-scale.")]
    [Range(0.0f, 2.0f)]
    public float armScaleFactor = 1.0f;
    //腿的额外比例因子，可用于微调腿部比例。
    [Tooltip("Additional scale factor for legs that might be used for fine tuning of leg-scale.")]
    [Range(0.0f, 2.0f)]
    public float legScaleFactor = 1.0f;
    //头像的垂直偏移相对于用户的脊柱基础的位置。
    [Tooltip("Vertical offset of the avatar with respect to the position of user's spine-base.")]
    [Range(-0.5f, 0.5f)]
    public float verticalOffset = 0f;
    //Forward (Z)avatar相对于用户的脊柱基础位置的偏移量。
    [Tooltip("Forward (Z) offset of the avatar with respect to the position of user's spine-base.")]
    [Range(-0.5f, 0.5f)]
    public float forwardOffset = 0f;
    //这个模型选择器的性别筛选器。
    [Tooltip("Gender filter of this model selector.")]
    public UserGender modelGender = UserGender.Unisex;
    //这个模型选择器的最小年龄过滤器。
    [Tooltip("Minimum age filter of this model selector.")]
    public float minimumAge = 0;
    //这个模型选择器的最大年龄过滤器。
    [Tooltip("Maximum age filter of this model selector.")]
    public float maximumAge = 1000;


    [HideInInspector]
    public bool activeSelector = true;

    //	[Tooltip("GUI-Text to display the avatar-scaler debug messages.")]
    //	public GUIText debugText;


    // Reference to the dresing menu list title
    private Text dressingMenuTitle;

    // Reference to the dresing menu list content
    private RectTransform dressingMenuContent;

    // list of instantiated dressing panels
    //修整面板列表
    private List<GameObject> dressingPanels = new List<GameObject>();

    //private Rect menuWindowRectangle;
    private string[] modelNames;
    private Texture2D[] modelThumbs;

    private Vector2 scroll;
    /// <summary>
    /// 当前选择的模型的索引
    /// </summary>
	private int selected = -1;//当前选择的模型的索引
    private int prevSelected = -1;//上一个模型的索引

    private GameObject selModel;//当前选择的模型

    private float curScaleFactor = 0f;
    private float curModelOffset = 0f;


    /// <summary>
    /// 设置模型选择器是活跃的还是不活跃的
    /// Sets the model selector to be active or inactive.
    /// </summary>
    /// <param name="bActive">If set to <c>true</c> b active.</param>
    public void SetActiveSelector(bool bActive)
    {
        activeSelector = bActive;

        if (dressingMenu) //将衣物选择菜单与选择器关联
        {
            dressingMenu.gameObject.SetActive(activeSelector);
        }

        if (!activeSelector && !keepSelectedModel) //当选择器切换且不保存选择模型时
        {
            DestroySelectedModel();//
        }
    }


    /// <summary>
    /// 获取所选模型
    /// Gets the selected model.
    /// </summary>
    /// <returns>The selected model.</returns>
    public GameObject GetSelectedModel()
    {
        return selModel;
    }


    /// <summary>
    /// 销毁当前选定的模型
    /// Destroys the currently selected model.
    /// </summary>
    public void DestroySelectedModel()
    {
        if (selModel)
        {
            AvatarController ac = selModel.GetComponent<AvatarController>();
            KinectManager km = KinectManager.Instance;

            if (ac != null && km != null)
            {
                km.avatarControllers.Remove(ac);//在kinectManger控制模型列表中删除
            }

            GameObject.Destroy(selModel);//将当前模型销毁
            selModel = null;//令模型等于Null

            prevSelected = -1;//令上一个选择等于-1
        }
    }


    /// <summary>
    /// 选择下一个模型
    /// Selects the next model.
    /// </summary>
    public void SelectNextModel()
    {
        selected++;//令选中的索引加一
        if (selected >= numberOfModels)
            selected = 0;//如果超出索引则循环

        //LoadModel(modelNames[selected]);
        OnDressingItemSelected(selected);
    }

    /// <summary>
    /// Selects the previous model.
    /// </summary>
    public void SelectPrevModel()
    {
        selected--;
        if (selected < 0)
            selected = numberOfModels - 1;

        //LoadModel(modelNames[selected]);
        OnDressingItemSelected(selected);
    }


    void Start()
    {
        // get references to menu title and content
        //获取对标题菜单和内容的引用
        if (dressingMenu)
        {
            Transform dressingHeaderText = dressingMenu.transform.Find("Header/Text");
            if (dressingHeaderText)
            {
                dressingMenuTitle = dressingHeaderText.gameObject.GetComponent<Text>();
            }

            Transform dressingViewportContent = dressingMenu.transform.Find("Scroll View/Viewport/Content");
            if (dressingViewportContent)
            {
                dressingMenuContent = dressingViewportContent.gameObject.GetComponent<RectTransform>();
            }
        }

        // create model names and thumbs
        modelNames = new string[numberOfModels];//创建的模型名称数组数量
        modelThumbs = new Texture2D[numberOfModels];//制定贴图数量
        dressingPanels.Clear();

        // instantiate menu items
        //实例化菜单列表
        for (int i = 0; i < numberOfModels; i++)
        {
            modelNames[i] = string.Format("{0:0000}", i);

            string previewPath = modelCategory + "/" + modelNames[i] + "/preview.jpg";
            TextAsset resPreview = Resources.Load(previewPath, typeof(TextAsset)) as TextAsset;

            if (resPreview == null)
            {
                resPreview = Resources.Load("nopreview.jpg", typeof(TextAsset)) as TextAsset;
            }

            //if(resPreview != null)
            {
                modelThumbs[i] = CreatePreviewTexture(resPreview != null ? resPreview.bytes : null);//创建贴图
            }

            //InstantiateDressingItem(i);//实例化菜单
        }

        // select the 1st item
        if (numberOfModels > 0)
        {
            selected = 0;
        }

        // set the panel title
        if (dressingMenuTitle)
        {
            dressingMenuTitle.text = modelCategory;//设置菜单标题名等于加载文件夹名
        }

        // save current scale factors and model offsets
        //保存当前的缩放因子和偏移量
        curScaleFactor = bodyScaleFactor + bodyWidthFactor + armScaleFactor + legScaleFactor;
        curModelOffset = verticalOffset + forwardOffset;
    }

    void Update()
    {
        Debug.LogError(activeSelector);
        // check for selection change
        //检查选择更改
        if ( selected >= 0 && selected < modelNames.Length && prevSelected != selected)//当上一个索引不等于当前索引时activeSelector &&
        {
            KinectManager kinectManager = KinectManager.Instance;
         
            if (kinectManager && kinectManager.IsInitialized() && kinectManager.IsUserDetected(playerIndex))
            {
                Debug.LogError("加载模型");
                OnDressingItemSelected(selected);
            }
        }

        if (selModel != null)
        {
            // update model settings as needed
            if (Mathf.Abs(curModelOffset - (verticalOffset + forwardOffset)) >= 0.001f) //当偏移发生改变时
            {
                // update model offsets
                curModelOffset = verticalOffset + forwardOffset;

                AvatarController ac = selModel.GetComponent<AvatarController>();
                if (ac != null)
                {
                    ac.verticalOffset = verticalOffset;
                    ac.forwardOffset = forwardOffset;
                }
            }

            if (Mathf.Abs(curScaleFactor - (bodyScaleFactor + bodyWidthFactor + armScaleFactor + legScaleFactor)) >= 0.001f) //当缩放发生改变时重新更新缩放
            {
                // update scale factors
                curScaleFactor = bodyScaleFactor + bodyWidthFactor + armScaleFactor + legScaleFactor;

                AvatarScaler scaler = selModel.GetComponent<AvatarScaler>();
                if (scaler != null)
                {
                    scaler.continuousScaling = continuousScaling;
                    scaler.bodyScaleFactor = bodyScaleFactor;
                    scaler.bodyWidthFactor = bodyWidthFactor;
                    scaler.armScaleFactor = armScaleFactor;
                    scaler.legScaleFactor = legScaleFactor;
                }
            }
        }
    }
    // creates preview texture
    private Texture2D CreatePreviewTexture(byte[] btImage)
    {
        Texture2D tex = new Texture2D(4, 4);
        //Texture2D tex = new Texture2D(100, 143);

        if (btImage != null)
        {
            tex.LoadImage(btImage);
        }

        return tex;
    }

    // instantiates dressing menu item
    //实例化穿衣菜单
    private void InstantiateDressingItem(int i)
    {
        if (!dressingItemPrefab && i >= 0 && i < numberOfModels)
            return;

        GameObject dressingItemInstance = Instantiate<GameObject>(dressingItemPrefab);

        GameObject dressingImageObj = dressingItemInstance.transform.Find("DressingImagePanel").gameObject;
        dressingImageObj.GetComponentInChildren<RawImage>().texture = modelThumbs[i];

        if (!string.IsNullOrEmpty(modelNames[i]))
        {
            EventTrigger trigger = dressingItemInstance.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((eventData) => { OnDressingItemSelected(i); });

            trigger.triggers.Add(entry);
        }

        if (dressingMenuContent)
        {
            dressingItemInstance.transform.SetParent(dressingMenuContent, false);
        }

        dressingPanels.Add(dressingItemInstance);
    }

    // invoked when dressing menu-item was clicked
    /// <summary>
    /// invoked when dressing menu-item was clicked
    /// 根据索引加载模型
    /// </summary>
    /// <param name="i"></param>
    private void OnDressingItemSelected(int i)
    {
        if (i >= 0 && i < modelNames.Length && prevSelected != i)
        {
            prevSelected = selected = i;
            LoadDressingModel(modelNames[selected]);
        }
    }

    // sets the selected dressing model as user avatar
    //设置所选模型打扮成用户化身
    private void LoadDressingModel(string modelDir)
    {
        string modelPath = modelCategory + "/" + modelDir + "/model";
        UnityEngine.Object modelPrefab = Resources.Load(modelPath, typeof(GameObject));//加载模型
                                                                                       //   modelPrefab = null;
        Debug.Log("Model: 不加载模型" + modelPath);
        if (modelPrefab == null)
            return;

        Debug.Log("Model: " + modelPath);

        if (selModel != null)
        {
            GameObject.Destroy(selModel);
        }

        selModel = (GameObject)GameObject.Instantiate(modelPrefab, Vector3.zero, Quaternion.Euler(0, 180f, 0));
        selModel.name = "Model" + modelDir;

        AvatarController ac = selModel.GetComponent<AvatarController>();
        if (ac == null) //添加AvatarController
        {
            ac = selModel.AddComponent<AvatarController>();
            ac.playerIndex = playerIndex;

            ac.mirroredMovement = true;
            ac.verticalMovement = true;

            ac.verticalOffset = verticalOffset ;
            ac.forwardOffset = forwardOffset ;
            ac.smoothFactor = 0f;
        }

        ac.posRelativeToCamera = modelRelativeToCamera;
        ac.posRelOverlayColor = (foregroundCamera != null);

        KinectManager km = KinectManager.Instance;
        //ac.Awake();

        if (km && km.IsInitialized())
        {
            long userId = km.GetUserIdByIndex(playerIndex);
            if (userId != 0)
            {
                ac.SuccessfulCalibration(userId, false);//校准
            }

            // locate the available avatar controllers
            //找到可用的avatar
            MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
            km.avatarControllers.Clear();

            foreach (MonoBehaviour monoScript in monoScripts)
            {
                if ((monoScript is AvatarController) && monoScript.enabled)
                {
                    AvatarController avatar = (AvatarController)monoScript;
                    km.avatarControllers.Add(avatar);
                }
            }
        }

        AvatarScaler scaler = selModel.GetComponent<AvatarScaler>();//于模型上添加AvatarScaler脚本
        if (scaler == null)
        {
            scaler = selModel.AddComponent<AvatarScaler>();
            scaler.playerIndex = playerIndex;
            scaler.mirroredAvatar = true;

            scaler.continuousScaling = continuousScaling;
            scaler.bodyScaleFactor = bodyScaleFactor;
            scaler.bodyWidthFactor = bodyWidthFactor;
            scaler.armScaleFactor = armScaleFactor;
            scaler.legScaleFactor = legScaleFactor;
        }

        scaler.foregroundCamera = foregroundCamera;
        //scaler.debugText = debugText;

        //scaler.Start();
    }

}
