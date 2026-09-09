using UnityEngine;
using UnityEngine.InputSystem;

public class CamInput : MonoBehaviour
{
    private InputSystem_Actions controls;

    public bool Cam1{ get; private set; }
    public bool Cam2 { get; private set; }
    public bool Cam3 { get; private set; }
    public bool Cam4 { get; private set; }
    private void Awake()
    {
        controls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        controls.Cameras.Camera1.performed += OnCam1;
        controls.Cameras.Camera2.performed += OnCam2;
        controls.Cameras.Camera3.performed += OnCam3;
        controls.Cameras.Camera4.performed += OnCam4;

        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void OnCam1(InputAction.CallbackContext ctx)
    {
        Cam1 = true;
    }
    private void OnCam2(InputAction.CallbackContext ctx)
    {
        Cam2 = true;
    }
    private void OnCam3(InputAction.CallbackContext ctx)
    {
        Cam3 = true;
    }
    private void OnCam4(InputAction.CallbackContext ctx)
    {
        Cam4 = true;
    }
    private void LateUpdate()
    {
        Cam1 = false;
        Cam2 = false;
        Cam3 = false;
        Cam4 = false;
    }

}
