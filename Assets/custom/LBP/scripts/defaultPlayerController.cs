using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultPlayerController : MonoBehaviour
{
    //@aidenmhill - do whatever you want with this
    
    //the following script is the primary controller script I use for my Oculus GO demos
    //its pretty messy and I would not recommend for anything important/official
    
    public Rigidbody bullet;
    public GameObject tracer;
    public Transform gunExit;
    public int playerSpeed;
    public bool hideCursor;

    public Vector2 joystick;

    public GameObject ToolSet;
    public GameObject leftParent;
    public GameObject RightParent;

    public GameObject gunHand;


    public int invMode = 1; //0=selecting,1=gun(right),2=hammer(forward),3=match(left)
    public bool gunOnlyMode;
    public GameObject quitMenu;

    public bool canMove;

    int resetV = 2; //total reset button time -  0 = complete  -  1 = half

    public Transform leftHandAnchor = null;
    public Transform rightHandAnchor = null;

    void Start()
    {
        //properly fit controller into left or right hand
        if (leftParent.activeSelf == true)
        {
            ToolSet.transform.SetParent(leftParent.transform);
            ToolSet.transform.position = leftParent.transform.position;
            ToolSet.transform.rotation = leftParent.transform.rotation;
        }
        if (RightParent.activeSelf == true)
        {
            ToolSet.transform.SetParent(RightParent.transform);
            ToolSet.transform.position = RightParent.transform.position;
            ToolSet.transform.rotation = RightParent.transform.rotation;
        }
        StartCoroutine(handSwitchCheck());
    }

    void Update()
    {
    }

    Transform Pointer
    {
        get
        {
            OVRInput.Controller controller = OVRInput.GetConnectedControllers();
            if ((controller & OVRInput.Controller.LTrackedRemote) != OVRInput.Controller.None)
            {
                return leftHandAnchor;
            }
            else if ((controller & OVRInput.Controller.RTrackedRemote) != OVRInput.Controller.None)
            {
                return rightHandAnchor;
            }
            // If no controllers are connected, we use ray from the view camera. 
            // This looks super ackward! Should probably fall back to a simple reticle!
            return null;
        }
    }

    void FixedUpdate()
    {
        //OVRInput.Update();

        Vector2 touchPadGo = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.F))//index press
        {
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.Q))
        {
            if (gunOnlyMode == false)//only for gun in main menu
            { // V V temp avoidence for movement sensor V V
                //invMode = 0;

            }
        }

        if (hideCursor == false)
        {

            tracer.SetActive(true);
            RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(gunExit.position);
            //Physics.Raycast(ray, out hit);
            Physics.Raycast(gunExit.position, gunExit.forward, out hit);
            tracer.transform.position = hit.point;

        }
        else
        {
            tracer.SetActive(false);
        }

        if (canMove == true) //walk mode
        {
            joystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if (touchPadGo.y > 0.5 && -0.5 < touchPadGo.x && touchPadGo.x < 0.5) //forward
            {
                transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
            }
            if (touchPadGo.x < -0.5 && -0.5 < touchPadGo.y && touchPadGo.y < 0) //toward left
            {
                //transform.Rotate(Vector3.down * playerSpeed * Time.deltaTime * 15);
                transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
            }
            if (touchPadGo.x > 0.5 && -0.5 < touchPadGo.y && touchPadGo.y < 0.5) //toward right
            {
                //transform.Rotate(Vector3.up * playerSpeed * Time.deltaTime * 15);
                transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            }
            if (touchPadGo.y < -0.5 && -0.5 < touchPadGo.x && touchPadGo.x < 0) //backward
            {
                transform.Translate(Vector3.back * playerSpeed * Time.deltaTime);
            }

            // transform.Translate(Vector3.forward * playerSpeed * joystick.y * Time.deltaTime); //forward and back
            // transform.Translate(Vector3.right * playerSpeed * joystick.x * Time.deltaTime); //left and right
        }
    }

    public IEnumerator returnClickReset()//back button handle
    {

        yield return new WaitForSeconds(0.5f);

        if (resetV <= 0)
        {
            Application.LoadLevel("main");
        }
        else
        {
            yield return new WaitForSeconds(1f);
            resetV = 2;
        }

        //Application.LoadLevel("main");
    }

    public IEnumerator handSwitchCheck()//rapid update left or right
    {
        StartCoroutine(startWaitSetup());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(handSwitchCheck());
    }

    public IEnumerator startWaitSetup()//delay after startup
    {
        yield return new WaitForSeconds(0.5f);
        if (leftParent.activeSelf == true)
        {
            ToolSet.transform.SetParent(leftParent.transform);
            ToolSet.transform.position = leftParent.transform.position;
            ToolSet.transform.rotation = leftParent.transform.rotation;
        }
        if (RightParent.activeSelf == true)
        {
            ToolSet.transform.SetParent(RightParent.transform);
            ToolSet.transform.position = RightParent.transform.position;
            ToolSet.transform.rotation = RightParent.transform.rotation;
        }
    }

}
