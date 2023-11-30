using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBehavior : MonoBehaviour
{
    public static TutorialBehavior Instance;

    [SerializeField] TMP_Text message;

    enum TutorialState
    {
        Throw,
        Teleport,
        Others,
        Over
    }

    TutorialState state = TutorialState.Throw;

    bool dashed = false;
    bool healed = false;
    bool jump = false;
    bool murder = false;


    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case TutorialState.Throw:
                if (Input.GetButtonDown("Fire2"))
                {
                    message.text = "Left Click to teleport";
                    state = TutorialState.Teleport;
                }
                break;

            case TutorialState.Teleport:
                if (Input.GetButtonDown("Fire1"))
                {
                    UpdateText();
                    state = TutorialState.Others;
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    message.text = "Right Click to throw the warp sword";
                    state = TutorialState.Throw;
                }
                break;

            case TutorialState.Others:
                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    healed = true;
                    UpdateText();
                }
                if (Input.GetKeyDown(KeyCode.LeftShift)) 
                {
                    dashed = true;
                    UpdateText();
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    murder = true;
                    UpdateText();
                }
                break;
            
            case TutorialState.Over:
                break;

        }
    }


    public void UpdateText()
    {
        message.text = "";
        if (!murder)
        {
            message.text += "Right Click to Murder\n";
        }
        if (!dashed)
        {
            message.text += "Hold Left Shift to Dash\n";
        }
        if (!healed)
        {
            message.text += "Hold E to heal\n";
        }
        if (!jump)
        {
            message.text += "Press Space in the air to do an extra jump";
        }
    }


    public void Jump()
    {
        jump = true;
        UpdateText();
    }
}
