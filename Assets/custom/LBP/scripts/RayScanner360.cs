using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScanner360 : MonoBehaviour
{

    //@aidenmhill - do whatever you want with this

    public GameObject deviceNeck;
    public GameObject raycastExit;
    public GameObject pointRayPrefab;

    bool rotateNeck = false;
    bool isFree = true;
    bool plotPointMode = false;

    RaycastHit hitPointInfo;

    int eyeScanMode = 0;//0=null, 1=scan down, 2=scan up

    float eyescanRotateTime = 0.1f;

    void Start()
    {

    }


    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.F))//press B or F
        {
            if (isFree == true)
            {
                isFree = false;
                StartCoroutine(StartupLoad());
            }
        }

        Debug.DrawRay(raycastExit.transform.position, raycastExit.transform.up * 20);//renders seperate ray

        if (rotateNeck == true)//rotate neck check
        {
            //deviceNeck.transform.Rotate(Vector3.up * 0.5f);
        }

        if (eyeScanMode == 1)//rotate down
        {
            raycastExit.transform.Rotate(Vector3.forward * 3f);//distance to rotate
        }
        else if (eyeScanMode == 2)//rotate up
        {
            raycastExit.transform.Rotate(Vector3.forward * -3f);
        }
    }

    private void FixedUpdate()
    {
        


    }

    IEnumerator StartupLoad()
    {
        yield return new WaitForSeconds(2f);
        rotateNeck = true;
        plotPointMode = true;
        StartCoroutine(MassPointPlot());
        StartCoroutine(full360Rotation());

        yield return new WaitForSeconds(130f);//wait till end

        rotateNeck = false;
        plotPointMode = false;
        isFree = true;

    }

    IEnumerator MassPointPlot()//place points on loop
    {
        yield return new WaitForSeconds(0.01f);

        Ray ray = new Ray(raycastExit.transform.position, raycastExit.transform.up); //loads up ray to detect hit
        if (Physics.Raycast(ray, out hitPointInfo, 20f))
        {
            Debug.Log("ray hit something"); //event for on ray hit

            GameObject pointRayPrefabInstance;
            pointRayPrefabInstance = Instantiate(pointRayPrefab, hitPointInfo.point, transform.rotation) as GameObject;
            //spawns temp marker for location
        }
        else
        {
            // Debug.Log("ray hit nothing");
        }

        if (plotPointMode == true)//use for loops until false
        {
            StartCoroutine(MassPointPlot());
        }
    }

    IEnumerator full360Rotation()//used to animate eye and neck
    {
        if (plotPointMode == true)
        {
            yield return new WaitForSeconds(0.1f);
            raycastExit.transform.rotation = Quaternion.Euler(raycastExit.transform.eulerAngles.x, deviceNeck.transform.eulerAngles.y, 0);//reset with sensor looking up
            deviceNeck.transform.Rotate(Vector3.up * 2f);

            eyeScanMode = 1; //rotate down
            yield return new WaitForSeconds(eyescanRotateTime * 6f);//time to rotate

            //eyeScanMode = 2; //rotate up
            //yield return new WaitForSeconds(eyescanRotateTime * 5f);

            eyeScanMode = 0;
            StartCoroutine(full360Rotation());
        }
    }
}
