using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeShapeSizer : MonoBehaviour {

    //@aidenmhill - do whatever you want with this
    //attach to parent of editorcube and 2 grip corners
    public GameObject StartCorner;
    public GameObject EndCorner;

    public GameObject editorCube; //center cube that will be the area between start and end corners

    Vector3 startPos;
    Vector3 endPos;

	void Start () {
		
	}
	

	void Update () {
        Vector3 startPos = StartCorner.transform.position;
        Vector3 endPos = EndCorner.transform.position; //connect vector vars to actual positions

        Vector3 centerPos = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) /2; //get center position

        float scaleX = Mathf.Abs(startPos.x - endPos.x);
        float scaleY = Mathf.Abs(startPos.y - endPos.y);
        float scaleZ = Mathf.Abs(startPos.z - endPos.z);//calculate scale between vectors per axis

       //centerPos.x -= 0.1f;
       //centerPos.y += 0.1f;

        editorCube.transform.position = centerPos;
        editorCube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
}