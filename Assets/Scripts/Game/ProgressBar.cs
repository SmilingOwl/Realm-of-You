using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;

    private float fillSpeed = 0.2f;
    private float targetProgressInc = -1, targetProgressDec = -1;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float val = slider.value;
        if (targetProgressInc != -1 && slider.value < targetProgressInc) {
            slider.value += fillSpeed * Time.deltaTime;
        } else if (targetProgressInc != -1 && slider.value >= targetProgressInc) {
            targetProgressInc = -1;
        }
        if (targetProgressDec != -1 && slider.value > targetProgressDec) {
            slider.value -= fillSpeed * Time.deltaTime;
        } else if (targetProgressDec != -1 && slider.value <= targetProgressDec) {
            targetProgressDec = -1;
        }
    }

    //Add progress to the bar
    public void IncProgress(int value, float newProgress)
    {
        slider.value = value / 100f;
        targetProgressInc = slider.value + newProgress / 100f; //Update with normalized value
    }

    public void DecProgress(int value, float newProgress)
    {
        slider.value = value / 100f;
        targetProgressDec = slider.value - newProgress / 100f; //Update with normalized value
    }

    public void SetProgress(float newProgress)
    {
        slider.value = newProgress / 100; //Update with normalized value
    }

}