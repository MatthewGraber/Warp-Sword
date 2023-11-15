using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void SetValue(float current, float max)
    {
        slider.value = current/max;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        slider.transform.rotation = PlayerBehavior.Instance.mainCam.transform.rotation;
    }
}
