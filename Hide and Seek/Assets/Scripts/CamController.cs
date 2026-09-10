using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum Cams
{
    Cam1,
    Cam2, 
    Cam3, 
    Cam4
}


public class CamController : MonoBehaviour
{
    CamInput c;
    Cams currentCam = Cams.Cam1;

    public List<Camera> cameraList = new List<Camera>();
    public RenderTexture camTexture;

    private float rotationX;
    private float rotationY;

    private bool wasPowerOn = false;

    private void Start()
    {
        c = GetComponent<CamInput>();
        foreach(Camera camera in cameraList)
        {
            camera.enabled = false;
        }
    }
    void ClearCameraFeed()
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].targetTexture = null;
            cameraList[i].enabled = false;
        }
    }
    void ChangeRenderedCamera(Cams camera)
    {
        ClearCameraFeed();
        cameraList[(int)camera].enabled = true;
        cameraList[(int)camera].targetTexture = camTexture;
        currentCam = camera;

    }
    void RotateCamera(Cams camera)
    {
        rotationX -= c.RotateInput.y * 50f * Time.deltaTime;
        rotationY += c.RotateInput.x * 50f * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -45f, 45f);
        rotationY = Mathf.Clamp(rotationY, -45f, 45f);

        cameraList[(int)camera].transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
    void Update()
    {
        if(c.Power)
        {
            if(!wasPowerOn)
            {
                ChangeRenderedCamera(currentCam);
            }
            if (c.Cam1)
            {

                Debug.Log("cam1");
                ChangeRenderedCamera(Cams.Cam1);

            }
            if (c.Cam2)
            {
                Debug.Log("cam2");
                ChangeRenderedCamera(Cams.Cam2);
            }
            if (c.Cam3)
            {
                Debug.Log("cam3");
                ChangeRenderedCamera(Cams.Cam3);
            }
            if (c.Cam4)
            {
                Debug.Log("cam4");
                ChangeRenderedCamera(Cams.Cam4);
            }
            switch (currentCam)
            {
                case Cams.Cam1:
                    RotateCamera(Cams.Cam1);
                    break;
                case Cams.Cam2:
                    RotateCamera(Cams.Cam2);
                    break;
                case Cams.Cam3:
                    RotateCamera(Cams.Cam3);
                    break;
                case Cams.Cam4:
                    RotateCamera(Cams.Cam4);
                    break;

                default:

                    break;

            }
        }
        else
        {
            if (wasPowerOn)
            {
                ClearCameraFeed();
            }
        }
        wasPowerOn = c.Power;
    }
}
