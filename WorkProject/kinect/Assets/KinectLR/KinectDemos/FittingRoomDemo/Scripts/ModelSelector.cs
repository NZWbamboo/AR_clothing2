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
    public GameObject Tip;

    [HideInInspector]
    public bool activeSelector = true;

    private GameObject selModel;//当前选择的模型

    private float curScaleFactor = 0f;
    private float curModelOffset = 0f;       //
    private Vector3 rightHandScreenPos;      //关节在屏幕里的位置
    private bool LoadModel = true;           //是否自动加载模型




    /// <summary>
    /// 获取所选模型
    /// Gets the selected model.
    /// </summary>
    /// <returns>The selected model.</returns>
    public GameObject GetSelectedModel()
    {
        return selModel;
    }

    // invoked when dressing menu-item was clicked
    /// <summary>
    /// invoked when dressing menu-item was clicked
    /// 根据索引加载模型
    /// </summary>
    /// <param name="i">模型文件夹索引</param>
    /// <param name="modelClass">文件夹名</param>
    public void OnDressingItemSelected(int i, string modelClass)
    {
        LoadDressingModel(string.Format("{0:0000}", i), modelClass);
    }
    KinectManager kinectManager;
    void Start()
    {
        kinectManager = KinectManager.Instance;
        // save current scale factors and model offsets
        //保存当前的缩放因子和偏移量
        curScaleFactor = bodyScaleFactor + bodyWidthFactor + armScaleFactor + legScaleFactor;
        curModelOffset = verticalOffset + forwardOffset;
    }
   
    void Update()
    {


        if (!GetJointOverlayScreenPos(kinectManager, (int)KinectInterop.JointType.SpineBase, ref rightHandScreenPos)&& !GetJointOverlayScreenPos(kinectManager, (int)KinectInterop.JointType.SpineMid, ref rightHandScreenPos))//如果脊椎关节在屏幕里
        {
            Tip.SetActive(true);
            GameObject.Destroy(selModel);
            LoadModel = true;
        }
        else if (selModel == null && LoadModel)//如果
        {
            Tip.SetActive(false);
            int selectedFileName = Random.Range(0, 3);
            string FileName = null;
            switch (selectedFileName)
            {
                case 0:
                    FileName = "man";
                    break;
                case 1:
                    FileName = "woman";
                    break;
                case 2:
                    FileName = "katon";
                    break;
            }
            Debug.LogError(FileName+"  自动加载模型");
            GameObject.Find("UI-Canvas/Page/Right_Panel/UP/" + FileName).transform.GetChild(2).GetComponent<Toggle>().isOn = true;
            LoadModel = false;
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



    private bool GetJointOverlayScreenPos(KinectManager kinectManager, int iHandJointIndex, ref Vector3 handScreenPos)
    {
        Vector3 posJointRaw = kinectManager.GetJointKinectPosition(kinectManager.GetUserIdByIndex(playerIndex), iHandJointIndex);

        if (posJointRaw != Vector3.zero)
        {
            Vector2 posDepth = kinectManager.MapSpacePointToDepthCoords(posJointRaw);
            ushort depthValue = kinectManager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);

            if (posDepth != Vector2.zero && depthValue > 0)
            {
                // depth pos to color pos
                Vector2 posColor = kinectManager.MapDepthPointToColorCoords(posDepth, depthValue);

                if (!float.IsInfinity(posColor.x) && !float.IsInfinity(posColor.y))
                {
                    // get the color image x-offset and width (use the portrait background, if available)
                    float colorWidth = kinectManager.GetColorImageWidth();
                    float colorOfsX = 0f;

                    PortraitBackground portraitBack = PortraitBackground.Instance;
                    if (portraitBack && portraitBack.enabled)
                    {
                        colorWidth = kinectManager.GetColorImageHeight() * kinectManager.GetColorImageHeight() / kinectManager.GetColorImageWidth();
                        colorOfsX = (kinectManager.GetColorImageWidth() - colorWidth) / 2f;
                    }

                    float xScaled = (posColor.x - colorOfsX) / colorWidth;
                    float yScaled = posColor.y / kinectManager.GetColorImageHeight();

                    handScreenPos.x = xScaled;
                    handScreenPos.y = 1f - yScaled;

                    return true;
                }
            }
        }

        return false;
    }
    // sets the selected dressing model as user avatar
    //设置所选模型打扮成用户化身
    private void LoadDressingModel(string modelDir, string modelCategory)
    {
        string modelPath = "Models" + "/" + modelCategory + "/" + modelDir + "/model";
        //  Debug.LogWarning(modelPath);
        UnityEngine.Object modelPrefab = Resources.Load(modelPath, typeof(GameObject));//加载模型
                                                                                       //   modelPrefab = null;
        Debug.Log("Model: 加载模型" + modelDir + modelCategory);
        if (modelPrefab == null)
            return;

        //  Debug.Log("Model: " + modelPath);

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

            ac.verticalOffset = verticalOffset;
            ac.forwardOffset = forwardOffset;
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
      

        scaler.Start();
    }

}
