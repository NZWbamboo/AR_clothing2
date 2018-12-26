using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerIndex = 0;
    private KinectManager KM;
    // Use this for initialization
    void Start()
    {
        KM = KinectManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (KM&&KM.IsInitialized())
        {
            if (KM.GetBodyCount() == 0);
            {

            }
        }

    }
}
