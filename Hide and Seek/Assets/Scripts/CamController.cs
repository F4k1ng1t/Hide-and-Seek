using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Cams
{
    Cam1,
    Cam2, 
    Cam3, 
    Cam4,
    Cam5,
    Off
}


public class CamController : MonoBehaviour
{
    

    CamInput c;
    Cams currentCam = Cams.Off;

    [Header("Components")]
    [Tooltip("List of cameras that will be iterated through")]
    public List<Camera> cameraList = new List<Camera>();
    
    [Tooltip("Render texture that will render to the mini display")]
    public RenderTexture camTexture;
    
    [Tooltip("The mini-display UI")]
    public Canvas canvas;

    [Tooltip("The static that plays when switching between cameras")]
    public CamStaticUI camStatic;
    
    [Tooltip("The battery UI that will decrement over time")]
    public BatteryUI batteryUI;

    [Space(10)]
    [Header("Stats")]

    [Tooltip("The maximum angles the cameras can move")]
    public float cameraClamp = 45f;

    [Tooltip("The range of the laser to kill the monsters")]
    public float laserRange = 100f;

    private float rotationX;
    private float rotationY;

    public bool powered { get; private set; }

    [HideInInspector]
    public bool lightIsOn = false;

    private void Start()
    {
        c = GetComponent<CamInput>();
        foreach(Camera camera in cameraList)
        {
            camera.enabled = false;
        }
        canvas.worldCamera = cameraList[0];
        canvas.planeDistance = 0.1f;
        powered = false;
    }
    public void ClearCameraFeed()
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].targetTexture = null;
            cameraList[i].enabled = false;
        }
        if(camTexture != null)
        {
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture.active = camTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = previousActive;
        }
    }
    void ChangeRenderedCamera(Cams camera)
    {
        if (currentCam == camera)
        {
            StartCoroutine(camStatic.PowerOff());
            currentCam = Cams.Off;
            powered = false;
            return;
        }
        powered = true;
        ClearCameraFeed();
        StartCoroutine(camStatic.ActivateCamStatic());

        if ((int)camera < cameraList.Count)
        {
            cameraList[(int)camera].enabled = true;
            cameraList[(int)camera].targetTexture = camTexture;
            canvas.worldCamera = cameraList[(int)camera];
            canvas.planeDistance = 1f;
            currentCam = camera;
        }
    }
    public Light FindCamLight(Cams camera)
    {
        if (cameraList[(int)camera].gameObject.GetComponentInChildren<Light>() == null)
        {
            Debug.Log("Light is missing.");
            return null;
        }
        return cameraList[(int)camera].gameObject.GetComponentInChildren<Light>();
    }
    public void TurnOnLight(Cams camera)
    {
        
        Light light = FindCamLight(camera);
        light.enabled = true;
    }
    public void TurnOffLight(Cams camera)
    {
        
        Light light = FindCamLight((Cams)camera);
        light.enabled = false;
    }
    void RotateCamera(Cams camera)
    {
        // Prevent trying to rotate if the cameras are currently turned off
        if (camera == Cams.Off || (int)camera >= cameraList.Count) return;

        rotationX -= c.RotateInput.y * 50f * Time.deltaTime;
        rotationY += c.RotateInput.x * 50f * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -cameraClamp, cameraClamp);
        rotationY = Mathf.Clamp(rotationY, -cameraClamp, cameraClamp);

        cameraList[(int)camera].transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    void FireCameraLaser()
    {
        if (currentCam == Cams.Off || (int)currentCam >= cameraList.Count) return;

        Camera cam = cameraList[(int)currentCam];
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        RaycastHit hit;
        Debug.DrawRay(origin, direction * laserRange, Color.red);
        if (Physics.Raycast(origin, direction, out hit, laserRange) && hit.transform.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Debug.Log("Hit!");
            enemy.Die();
        }
    }
    
    void Update()
    {
        bool hasBattery = batteryUI.Battery > 0;
        
        if (hasBattery)
        {
            // Camera has battery

            if (c.Cam1)
            {
                ChangeRenderedCamera(Cams.Cam1);
            }
            if(c.Cam2)
            {
                ChangeRenderedCamera(Cams.Cam2);
            }
            if (c.Cam3)
            {
                ChangeRenderedCamera(Cams.Cam3);
            }
            if (c.Cam4)
            {
                ChangeRenderedCamera(Cams.Cam4);
            }
            if (c.Cam5)
            {
                ChangeRenderedCamera(Cams.Cam5);
            }
            if (c.Fire)
            {
                FireCameraLaser();
            }
            if (currentCam != Cams.Off)
            {
                if (c.Flashlight && !lightIsOn)
                {
                    TurnOnLight(currentCam);
                    lightIsOn = true;
                }
                else if (!c.Flashlight && lightIsOn)
                {
                    TurnOffLight(currentCam);
                    lightIsOn = false;
                }
            }
            RotateCamera(currentCam);
        }
        else
        {
            ClearCameraFeed();
        }

    }

}
