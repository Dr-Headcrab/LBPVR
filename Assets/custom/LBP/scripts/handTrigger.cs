using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handTrigger : MonoBehaviour
{

    //@aidenmhill - Do whatever you want with this
    
    //The following script is my quick solution to a grab function for my Oculus GO controllers
    //It works based on an extended trigger volume from the hand to act as the grab range
    //There are dozens of better ways to do this, please avoid using this for anything important
    public GameObject playerHand;

    public bool isHovering;
    public bool canGrab = true;

    public GameObject tracerNorm;
    public GameObject tracerGrab;
    public GameObject tracerUse;
    public int tracerState = 0;
    //0=normal,1=grab,2=use

    //info about holding Gameobject
    public GameObject grabbedObject;
    public Transform grabbedPivot;
    public GameObject grabbedOgParent;

    public GameObject interactedObject;
    //used to find identity in script calls

    void Update()
    {
        if (tracerState != 0)//0 = nothing detected
            isHovering = true;
        else
            isHovering = false;//for debug

        if (OVRInput.GetUp(OVRInput.Button.Two) || Input.GetKeyUp(KeyCode.S) || OVRInput.GetUp(OVRInput.Button.Back)) //back button
        {
            throwObject();//release
        }


        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.F))//index press
        {
            if(canGrab == true)
            {
                if (tracerState == 1)
                {
                    canGrab = false;

                    //Disable gravity on object
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                    //switch child and parent under hand in hierarchy to keep pivot distance
                    grabbedPivot.parent = playerHand.transform;
                    grabbedObject.transform.parent = grabbedPivot;

                    grabbedPivot.transform.position = playerHand.transform.position;
                    grabbedPivot.transform.rotation = playerHand.transform.rotation;
                    //set back to original ownership of gameobjects
                    grabbedObject.transform.parent = playerHand.transform;
                    grabbedPivot.transform.parent = grabbedObject.transform;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "handGrab")
        {
            tracerState = 1;
            updateTracer();
            if (canGrab == true)
            {
                //set grabbable object to proper variables to be edited
                grabbedObject = other.gameObject;
                grabbedPivot = grabbedObject.transform.Find("HandPivot");
                if (grabbedObject.transform.parent != null)//if object has a parent
                    grabbedOgParent = grabbedObject.transform.parent.gameObject;
                else
                    grabbedOgParent = null;
            }
        }
        else if (other.gameObject.tag == "handUse")
        {
            tracerState = 2;
            updateTracer();
            interactedObject = other.gameObject;
        }
        else
            tracerState = 0; updateTracer();
    }

    void OnTriggerExit(Collider other)
    {  
        if(other.gameObject.tag == "handUse")
        {
            interactedObject = null;
        }
        tracerState = 0;
        updateTracer();
    }

    void updateTracer()
    {
        if (tracerState == 0)
        {
            tracerNorm.SetActive(true); tracerGrab.SetActive(false); tracerUse.SetActive(false);
        }
        else if (tracerState == 1)
        {
            tracerNorm.SetActive(false); tracerGrab.SetActive(true); tracerUse.SetActive(false);
        }
        else if (tracerState == 2)
        {
            tracerNorm.SetActive(false); tracerGrab.SetActive(false); tracerUse.SetActive(true);
        }
    }

    void throwObject()
    {
            //grabbedObject.GetComponent<Rigidbody>().AddForce(playerHand.transform.forward * 100);
            if (canGrab == false)
            {
                if (grabbedOgParent == null)
                    grabbedObject = null;//remove variable space
                else
                    grabbedObject.transform.parent = grabbedOgParent.transform;

                grabbedObject.GetComponent<Rigidbody>().useGravity = true;//re-enable physics
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject.GetComponent<Rigidbody>().AddForce(playerHand.transform.forward * 100);
                //release object at force of charge and reset value

                canGrab = true;
                grabbedOgParent = null;
                grabbedObject = null;
                grabbedPivot = null;//remove from variables for next object
            }
    }
}
