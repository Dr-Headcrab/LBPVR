using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCubeSize : MonoBehaviour {

    //@aidenmhill - do whatever you want with this
    //attach to anything

    public GameObject editorCube; //cube that will be the area between 2 grips
    public GameObject reziseSpawnTool; //grips and editorCube are children to an empty object 

    public Material newCubeMat; //material of new primitive

	void Start () {
		
	}
	

	void Update () {

        if (reziseSpawnTool.activeSelf == true)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                GameObject spawnedCube = GameObject.CreatePrimitive(PrimitiveType.Cube);//create new box primitive
                spawnedCube.transform.position = editorCube.transform.position;
                spawnedCube.transform.localScale = editorCube.transform.localScale;

                spawnedCube.GetComponent<Renderer>().material = newCubeMat;//set material to selected material variable

                reziseSpawnTool.SetActive(false);//temp hide and disable tool after usage
            }
        }
	}
}
