using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class laptopTerminalControl : MonoBehaviour
{
    public Text mainTerminalText;
    public Text logTextBox;
    public bool commandEntered;
    string TempOutput;

    public Material spaceSkybox;
    public Material sunnySkybox;

    public GameObject targetBot;

    bool canMove = false;
    GameObject tempObject;
    
    void Update()
    {
        if (commandEntered == true)
        {
            EnterCommand(("dev"), (">Working 100%"), 1);
            EnterCommand(("hack sky"), (">Sky hacked"), 2);
            EnterCommand(("unhack sky"), (">Sky unhacked"), 3);
            EnterCommand(("bot forward"), (">Moving the bot forward"), 4);
        }
    }

    void FixedUpdate()
    {
        if(canMove == true)//temp bool
        {
            tempObject.transform.Translate(Vector3.forward * Time.deltaTime * 3f); //move selected object
        }
    }

    public void EnterCommand(string textInput, string textOutput, int intentMode)//1=normal
    {
        commandEntered = false;
        if (logTextBox.text == textInput)
        {
            TempOutput = textOutput;
            StartCoroutine(respondWait());
            if (intentMode == 2)
            {
                RenderSettings.skybox = spaceSkybox;
                DynamicGI.UpdateEnvironment();
            }
            if (intentMode == 3)
            {
                RenderSettings.skybox = sunnySkybox;
                DynamicGI.UpdateEnvironment();
            }
            if(intentMode == 4)
            {
                StartCoroutine(moveForward(targetBot, 5f));//move bot only (object, move time)
            }
        }
    }

    IEnumerator respondWait()
    {
        yield return new WaitForSeconds(0.1f);
        mainTerminalText.text += ("\n") + TempOutput;//return spacing
    }

    IEnumerator moveForward(GameObject target, float moveTime)
    {
        tempObject = target; //set at fixed update
        canMove = true;
        yield return new WaitForSeconds(moveTime);//move threshold
        canMove = false;
    }
}
