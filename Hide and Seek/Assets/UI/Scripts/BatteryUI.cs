using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class BatteryUI : MonoBehaviour
{
    public List<SpriteRenderer> percentBlocks = new List<SpriteRenderer>();
    public int Battery = 100;
    public CamInput c;

    private float frames = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(c.Power && Battery > 0)
        {
            frames++;
            if (frames == 30f)
            {
                Battery--;
                frames = 0f;
                Debug.Log(Battery / 25);
                if (Battery % 25 < 10 && Battery % 25 != 0)
                {
                    percentBlocks[(Battery / 25)].enabled = !percentBlocks[(Battery / 25)].enabled;
                }
                else if(Battery % 25 == 0)
                {
                    percentBlocks[Battery / 25].enabled = false;
                }
            }
        }
    }
}
