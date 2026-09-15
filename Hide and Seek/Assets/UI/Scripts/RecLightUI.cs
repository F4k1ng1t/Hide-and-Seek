using UnityEngine;
using UnityEngine.UI;
public class RecLightUI : MonoBehaviour
{
    Image recLight;
    private int frames = 0;
    void Start()
    {
        recLight = this.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        frames++;
        if (frames == 30)
        {
            recLight.enabled = !recLight.enabled;
            frames = 0;
        }
        
    }

}
