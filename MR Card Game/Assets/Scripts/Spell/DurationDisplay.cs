using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationDisplay : MonoBehaviour
{
    [SerializeField]
    private Image fillingCircle;

    private bool activated = false;

    private float passedTime = 0;

    private float duration = 0;

    // Update is called once per frame
    void Update()
    {
        if (activated && GameAdvancement.gamePaused == false)
        {
            passedTime += Time.deltaTime;
            fillingCircle.fillAmount = (duration - passedTime) / duration;
            if(passedTime >= duration)
            {
                activated = false;
                duration = 0;
                passedTime = 0;
                gameObject.SetActive(false);
            }
        }
    }

    public void StartCountDown(float duration)
    {
        gameObject.SetActive(true);
        this.duration = duration;
        fillingCircle.fillAmount = 1;
        activated = true;
    }
}
