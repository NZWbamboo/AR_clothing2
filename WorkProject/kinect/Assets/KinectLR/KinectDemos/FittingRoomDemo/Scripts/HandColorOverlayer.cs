using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using Windows.Kinect;


public class HandColorOverlayer : MonoBehaviour 
{
	[Tooltip("GUI-texture used to display the color camera feed on the scene background.")]
	public GUITexture backgroundImage;

	[Tooltip("Camera used to estimate the overlay positions of 3D-objects over the background. By default it is the main camera.")]
	public Camera foregroundCamera;
	
	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;
	
	[Tooltip("Game object used to overlay the left hand.")]
	public Transform leftHandOverlay;

	[Tooltip("Game object used to overlay the right hand.")]
	public Transform rightHandOverlay;
	
	//public float smoothFactor = 10f;

	// reference to KinectManager
	private KinectManager manager;
	

	void Update () 
	{
		if (foregroundCamera == null) 
		{
			// by default use the main camera
			foregroundCamera = Camera.main;
		}

		if(manager == null)
		{
			manager = KinectManager.Instance;
		}

		if(manager && manager.IsInitialized() && foregroundCamera)
        {
            Rect backgroundRect = foregroundCamera.pixelRect;
            //backgroundImage.renderer.material.mainTexture = manager.GetUsersClrTex();
            if (backgroundImage && (backgroundImage.texture == null))
            {
                backgroundImage.texture = manager.GetUsersClrTex();
            }
   //         if (backgroundImage && (backgroundImage.sprite == null))
			//{ Texture2D texture2D = manager.GetUsersClrTex();

   //             backgroundImage.sprite =Sprite.Create(texture2D, new Rect(0,0, texture2D.width, texture2D.height) ,new Vector2(0,0));
			//}

			// get the background rectangle (use the portrait background, if available)
		
			PortraitBackground portraitBack = PortraitBackground.Instance;
            
			if(portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}
            Debug.Log(backgroundRect);
			// overlay the joints
			if(manager.IsUserDetected(playerIndex))
			{
				long userId = manager.GetUserIdByIndex(playerIndex);

				OverlayJoint(userId, (int)KinectInterop.JointType.SpineBase, leftHandOverlay, backgroundRect);
			//	OverlayJoint(userId, (int)KinectInterop.JointType.HandRight, rightHandOverlay, backgroundRect);
			}
			
		}
	}

    /// <summary>
    /// ��������ؽ�
    /// </summary>
    /// <param name="userId"> �û���ID</param>
    /// <param name="jointIndex"> Ҫ����Ĺؽ�</param>
    /// <param name="overlayObj"> �������Ʒ</param>
    /// <param name="backgroundRect">  ��ͼ��Rect</param>
	private void OverlayJoint(long userId, int jointIndex, Transform overlayObj, Rect backgroundRect)
	{//�ж�Ҫ����Ĺؽ��Ƿ���ܸ���
		if(manager.IsJointTracked(userId, jointIndex))
		{//�õ��ؽ������Kinect��λ����Ϣ
			Vector3 posJoint = manager.GetJointKinectPosition(userId, jointIndex);
			
			if(posJoint != Vector3.zero)
			{
				// 3d position to depth
                //��ùؽڵ����ӳ������
				Vector2 posDepth = manager.MapSpacePointToDepthCoords(posJoint);
                //��ȡָ���ؽ���������ֵ
				ushort depthValue = manager.GetDepthForPixel((int)posDepth.x, (int)posDepth.y);
				
				if(depthValue > 0)
				{
					// depth pos to color pos
                    //�õ�����������ɫӳ������
					Vector2 posColor = manager.MapDepthPointToColorCoords(posDepth, depthValue);
					
					float xNorm = (float)posColor.x / manager.GetColorImageWidth();
					float yNorm =1- (float)posColor.y/ manager.GetColorImageHeight();
					
					if(overlayObj && foregroundCamera)
					{
						float distanceToCamera = overlayObj.position.z - foregroundCamera.transform.position.z;
						posJoint = foregroundCamera.ViewportToWorldPoint(new Vector3(xNorm, yNorm, distanceToCamera));

						overlayObj.position = posJoint;
					}
				}
			}
		}
	}

}
