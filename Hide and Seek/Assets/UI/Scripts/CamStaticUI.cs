using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamStaticUI : MonoBehaviour
{
    // Change this from GameObject to Image
    private Image staticImage;

    public float staticDuration = 1.0f;
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
            Debug.Log("this is happening");
            staticImage.enabled = true;
            yield return new WaitForSeconds(staticDuration);
            staticImage.enabled = false;
        }
    }
}
