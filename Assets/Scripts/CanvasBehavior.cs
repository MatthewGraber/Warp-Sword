using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehavior : MonoBehaviour
{
    public static CanvasBehavior instance;

    [SerializeField] Slider healthBar;
    [SerializeField] GameObject healthTMP;
    public TMP_Text healthText;

    [SerializeField] Slider manaBar;
    [SerializeField] GameObject manaTMP;
    public TMP_Text manaText;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthText = healthTMP.GetComponent<TMP_Text>();
        manaText = manaTMP.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DisplayHealth(float current, float max)
    {
        healthBar.value = current / max;
        healthText.text = ((int)current).ToString() + "/" + ((int)max).ToString();
    }


    public void DisplayMana(float current, float max)
    {
        manaBar.value = current / max;
        manaText.text = ((int)current).ToString() + "/" + ((int)max).ToString();
    }

}
