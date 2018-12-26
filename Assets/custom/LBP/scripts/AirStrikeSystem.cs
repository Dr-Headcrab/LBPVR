using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrikeSystem : MonoBehaviour {

    public Transform planeSpawn;
    public GameObject BombExit;
    public GameObject flareDestination;
    public GameObject RemoteTrigger;
    public GameObject HasReachedTarget;
    public GameObject FlareSmoke;

    public Rigidbody PlaneBomb;
    public GameObject PlanePrefab;

    int AirStrikePhase = 0;
    
	void FixedUpdate()
    {
        //PlanePrefab.transform.SetParent(BombExit.transform);
        PlanePrefab.transform.Translate(Vector3.forward * 1);
        if (AirStrikePhase == 2)
        {

            PlanePrefab.transform.LookAt(flareDestination.transform);
        }

        flareDestination.transform.position = new Vector3(FlareSmoke.transform.position.x, planeSpawn.transform.position.y, FlareSmoke.transform.position.z);
    }

	void Update () {
        if (AirStrikePhase == 0)
        {
            if (RemoteTrigger.activeSelf == true)
            {
                PlanePrefab.transform.position = planeSpawn.position;
                AirStrikePhase = 1;
            }
        }
        if(AirStrikePhase == 1)
        {
            PlanePrefab.SetActive(true);
            FlareSmoke.SetActive(true);
            AirStrikePhase = 2;
        }
        if(AirStrikePhase == 2)
        {
            if(HasReachedTarget.activeSelf == true)
            {
                AirStrikePhase = 3;
                StartCoroutine(DespawnPlane());
            }
        }
	}

    IEnumerator DespawnPlane()
    {
        HasReachedTarget.SetActive(false);
        Rigidbody Bomb;
        Bomb = Instantiate(PlaneBomb, BombExit.transform.position, BombExit.transform.rotation) as Rigidbody;
        yield return new WaitForSeconds(7f);
        FlareSmoke.SetActive(false);
        yield return new WaitForSeconds(15f);
        PlanePrefab.transform.position = planeSpawn.position;
        PlanePrefab.SetActive(false);
        RemoteTrigger.SetActive(false);
        AirStrikePhase = 0;
    }
}
