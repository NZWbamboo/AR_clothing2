using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColthingControler : MonoBehaviour
{
    public int playerIndex = 0;
    private long playerUserID = 0;
    [Tooltip("The image that may be used to show the hand-moved cursor on the screen or not. The sprite textures below need to be set too.")]

    public Image guiHandCursor;
    private Image cursorProgressBar;
    private bool interactionInited = false;
    public Sprite[] ColthingSprite;
    // Use this for initialization
    void Start()
    {
        // get the progress bar reference if any
        GameObject objProgressBar = guiHandCursor && guiHandCursor.gameObject.transform.childCount > 0 ? guiHandCursor.transform.GetChild(0).gameObject : null;
        cursorProgressBar = objProgressBar ? objProgressBar.GetComponent<Image>() : null;

        interactionInited = true;//初始化完毕
        
    }
    private Vector3 rightHandScreenPos = Vector3.zero;
    private Vector3 cursorScreenPos = Vector3.zero;
    [Tooltip("Smooth factor for cursor movement.")]
    public float smoothFactor = 10f;
    private int num = 0;
    public GameObject gameObject;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            num++;

            gameObject.SetActive(num % 2 == 0);

        }
        KinectManager kinectManager = KinectManager.Instance;

        // update Kinect interaction

        if (kinectManager && kinectManager.IsInitialized())
        {
            playerUserID = kinectManager.GetUserIdByIndex(playerIndex);

            if (playerUserID != 0)
            {
                bool buer = GetHandOverlayScreenPos(kinectManager, (int)KinectInterop.JointType.SpineBase, ref rightHandScreenPos);
            }

        }
    }
    private bool GetHandOverlayScreenPos(KinectManager kinectManager, int iHandJointIndex, ref Vector3 handScreenPos)
    {
        Vector3 posJointRaw = kinectManager.GetJointKinectPosition(playerUserID, iHandJointIndex);

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
}
