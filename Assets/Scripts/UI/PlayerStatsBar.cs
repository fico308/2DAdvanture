using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBar : MonoBehaviour
{
    public Image heartImage;
    public Image heartDelayImage;
    public Image powerImage;

    public float waitDuration;
    private float waitCounter;

    private void Awake()
    {
        waitCounter = waitDuration;
    }

    private void Update()
    {
        if (heartDelayImage.fillAmount != heartImage.fillAmount)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                heartDelayImage.fillAmount = heartImage.fillAmount;
                waitCounter = waitDuration;
            }
        }
    }

    public void OnHeartChange(float percentage)
    {
        heartImage.fillAmount = percentage;
        if (percentage == 1)
        {
            heartDelayImage.fillAmount = 1;
        }
    }
}
