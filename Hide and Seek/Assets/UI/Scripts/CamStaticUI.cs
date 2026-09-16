using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamStaticUI : MonoBehaviour
{
    // Change this from GameObject to Image
    private Image staticImage;
    public float staticDuration = 1.0f;

    public CamController camController;
    void Start()
    {
        staticImage = GetComponent<Image>();
        if (staticImage != null)
        {
            staticImage.enabled = false;
        }
    }

    public IEnumerator ActivateCamStatic()
    {
        if (staticImage != null)
        {
            staticImage.enabled = true;
            yield return new WaitForSeconds(staticDuration);
            staticImage.enabled = false;
        }
    }
    public IEnumerator PowerOff()
    {
        staticImage.enabled = true;
        yield return new WaitForSeconds(staticDuration);
        staticImage.enabled = false;
        camController.ClearCameraFeed();
    }
}
