using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class gridLockGrab : MonoBehaviour {

    //@aidenmhill - do whatever you want with this
    //attach to actual grip
    public GameObject predictedCube; //child cube (I made it same size with different grid material)
    public float gridLockSize = 0.1f;

    float posXrating;
    float posYrating;
    float posZrating; //positions of gameObject rounded

    bool isGrabbed = false;

	void Start () {
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUngrabbed);
        //grab and ungrab handler setup, uses VRTK components
    }
	

	void Update () {

        if (isGrabbed == true)
        {
            posXrating = gameObject.transform.position.x;//set to current X postion
            posYrating = gameObject.transform.position.y;
            posZrating = gameObject.transform.position.z;

            posXrating = posXrating - (posXrating % gridLockSize);//round X position to 0.5 scale
            posYrating = posYrating - (posYrating % gridLockSize);
            posZrating = posZrating - (posZrating % gridLockSize);

            predictedCube.transform.position = new Vector3(posXrating, posYrating, posZrating);//update box position to grid
        }
    }

    void FixedUpdate()
    {
        
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("being Grabbed true");
        predictedCube.SetActive(true);
        isGrabbed = true;
    }

    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("being Ungrabbed true");
        predictedCube.SetActive(false);
        isGrabbed = false;

        gameObject.transform.position = predictedCube.transform.position;//update position to child cube
    }
}
