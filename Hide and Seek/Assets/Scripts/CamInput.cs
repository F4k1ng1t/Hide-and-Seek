using UnityEngine;
using UnityEngine.InputSystem;

public class CamInput : MonoBehaviour
{
    private InputSystem_Actions controls;

    public bool Cam1 { get; private set; }
    public bool Cam2 { get; private set; }
    public bool Cam3 { get; private set; }
    public bool Cam4 { get; private set; }
    public bool Cam5 { get; private set; }

    // This will now stay true for as long as you hold the button down
    public bool Flashlight => controls.Cameras.Flashlight.IsPressed();

    public bool Fire { get; private set; }

    public Vector2 RotateInput { get; private set; }

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
        controls.Cameras.Camera5.performed += OnCam5;
        controls.Cameras.CameraRotation.performed += OnRotate;
        controls.Cameras.Fire.performed += OnFire;

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Cameras.Camera1.performed -= OnCam1;
        controls.Cameras.Camera2.performed -= OnCam2;
        controls.Cameras.Camera3.performed -= OnCam3;
        controls.Cameras.Camera4.performed -= OnCam4;
        controls.Cameras.Camera5.performed -= OnCam5;
        controls.Cameras.CameraRotation.performed -= OnRotate;
        controls.Cameras.Fire.performed -= OnFire;

        controls.Disable();
    }

    private void OnCam1(InputAction.CallbackContext ctx) => Cam1 = true;
    private void OnCam2(InputAction.CallbackContext ctx) => Cam2 = true;
    private void OnCam3(InputAction.CallbackContext ctx) => Cam3 = true;
    private void OnCam4(InputAction.CallbackContext ctx) => Cam4 = true;
    private void OnCam5(InputAction.CallbackContext ctx) => Cam5 = true;
    private void OnRotate(InputAction.CallbackContext ctx) => RotateInput = ctx.ReadValue<Vector2>();
    private void OnFire(InputAction.CallbackContext ctx) => Fire = true;

    private void LateUpdate()
    {
        // Clear frame-impulse actions, but leave Flashlight alone since it checks .IsPressed()
        Cam1 = false;
        Cam2 = false;
        Cam3 = false;
        Cam4 = false;
        Cam5 = false;
        Fire = false;
    }
}