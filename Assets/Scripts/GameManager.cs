using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject messenger;
    public TMP_Text messageBox;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        messageBox = messenger.GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Message(string message, float delay)
    {
        StartCoroutine(ShowMessage(message, delay));
    }


    IEnumerator ShowMessage(string message, float delay)
    {
        messageBox.text = message;
        yield return new WaitForSeconds(delay);
        messageBox.text = "";
    }
}
