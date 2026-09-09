using UnityEngine;

public class CamController : MonoBehaviour
{
    CamInput c;

    public Camera Cam1;
    public Camera Cam2;
    public Camera Cam3;
    public Camera Cam4;
    private void Start()
    {
        c = GetComponent<CamInput>();
    }
    void Update()
    {
        if(c.Cam1)
        {
            Debug.Log("cam1");
        }
        if(c.Cam2)
        {
            Debug.Log("cam2");
        }
        if(c.Cam3)
        {
            Debug.Log("cam3");
        }
        if(c.Cam4)
        {
            Debug.Log("cam4");
        }
    }
}
