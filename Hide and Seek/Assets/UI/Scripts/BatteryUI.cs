using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class BatteryUI : MonoBehaviour
{
    public List<Image> percentBlocks = new List<Image>();

    public int Battery = 100;
    public CamController camController;

    private float frames = 0f;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(camController.powered && Battery > 0)
        {
            frames++;
            if (Battery > 5)
            {
                if (frames == 30f)
                {
                    if (Battery % 25 < 10 && Battery % 25 != 0 && frames > 5f)
                    {
                        percentBlocks[(Battery / 25)].enabled = !percentBlocks[(Battery / 25)].enabled;
                    }

                }
                if (frames == 60f)
                {
                    Battery--;
                    frames = 0f;
                    Debug.Log(Battery / 25);
                    if (Battery % 25 < 10 && Battery % 25 != 0 && Battery > 5)
                    {
                        percentBlocks[(Battery / 25)].enabled = !percentBlocks[(Battery / 25)].enabled;
                    }
                    else if (Battery % 25 == 0)
                    {
                        percentBlocks[Battery / 25].enabled = false;
                    }
                }
            }
            else if(frames == 60f && Battery <= 5)
            {
                Battery--;
                percentBlocks[Battery / 25].enabled = false;
                frames = 0f;
            }
        }
    }
}
