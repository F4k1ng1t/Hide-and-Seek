using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class BatteryUI : MonoBehaviour
{
    public List<Image> percentBlocks = new List<Image>();

    public float Battery = 100f;
    public CamController camController;
    [Range(0,1000)]
    public int blinkRate = 30;
    public int lowBatteryThreshold = 5;

    private int frames = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int batteryInt = Mathf.CeilToInt(Battery);
        if(camController.powered && Battery > 0)
        {
            
            frames++;
            if (Battery > lowBatteryThreshold)
            {
                if (frames == blinkRate)
                {
                    if (Battery % 25 < 10 && Battery % 25 != 0 && frames > 5f)
                    {
                        percentBlocks[(batteryInt / 25)].enabled = !percentBlocks[(batteryInt / 25)].enabled;
                    }

                }
                if (frames == blinkRate * 2)
                {
                    Battery--;
                    frames = 0;
                    Debug.Log(batteryInt / 25);
                    if (Battery % 25 < 10 && Battery % 25 != 0 && Battery > 5)
                    {
                        percentBlocks[(batteryInt / 25)].enabled = !percentBlocks[(batteryInt / 25)].enabled;
                    }
                    else if (Battery % 25 == 0)
                    {
                        percentBlocks[batteryInt / 25].enabled = false;
                    }
                    if (camController.lightIsOn)
                    {
                        Battery -= 0.5f;
                    }
                }
            }
            else if(Battery <= lowBatteryThreshold)
            {
                if(frames == blinkRate)
                {
                    percentBlocks[(batteryInt / 25)].enabled = !percentBlocks[(batteryInt / 25)].enabled;
                    
                }
                if(frames == blinkRate * 2)
                {
                    Battery--;
                    if (camController.lightIsOn)
                    {
                        Battery-= 0.5f;
                    }
                    percentBlocks[(batteryInt / 25)].enabled = !percentBlocks[(batteryInt / 25)].enabled;
                    percentBlocks[batteryInt / 25].color = Color.red;
                    frames = 0;
                    
                }
                
            }
        }
    }
}
