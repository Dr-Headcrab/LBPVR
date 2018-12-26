using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botWandControl : MonoBehaviour {

    public GameObject TargetBotController;
    public GameObject TargetTouchDetect;
    public GameObject TouchDetectEnd;

    public int selectState = 0; //0 = no touch, 1 = first touch, 2 = commanded

    public GameObject destObject; //actual dest object
    public GameObject destTracker; //oriented x and z position of dest

    public GameObject effectResp;
	void Start () {
		
	}
	

	void Update () {

        if (selectState == 0)
        {
            if (TargetTouchDetect.activeSelf == true)
            {
                selectState = 1;
                TargetTouchDetect.SetActive(false);
            }
        }
	}

    void FixedUpdate()
    {
        destTracker.transform.position = new Vector3(destObject.transform.position.x, TargetBotController.transform.position.y, destObject.transform.position.z);
        //X,Z = dest object  | y = bot target location

        if (selectState == 1)
        {
            //Debug.Log("entered second state");
            effectResp.SetActive(true);
            destObject.SetActive(true);

            destObject.transform.position = new Vector3(gameObject.transform.position.x, TargetBotController.transform.position.y, gameObject.transform.position.z);

            destTracker.transform.position = new Vector3(gameObject.transform.position.x, TargetBotController.transform.position.y, gameObject.transform.position.z);
            //X,Z = self location  | y = bot target location

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))//trigger on touch
            {
                StartCoroutine(resetEffect());
                selectState = 2;
            }

        }
        if (selectState == 2)//command toggle effect
        {
            TargetBotController.transform.LookAt(destTracker.transform);//level rotation on Y
            TargetBotController.transform.Translate(Vector3.forward * Time.deltaTime * 1f); //move toward target

            if(TouchDetectEnd.activeSelf == true)
            {
                selectState = 0;
                TouchDetectEnd.SetActive(false); //end of command
            }
        }
    }

    IEnumerator resetEffect()
    {
        yield return new WaitForSeconds(5f);
        //destObject.SetActive(false);
        effectResp.SetActive(false);
    }
}
