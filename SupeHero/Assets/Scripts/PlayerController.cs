using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController pc;
    private void Awake()
    {
        pc = this;
    }
    public GameObject car;
    public GameObject heli;
    public GameObject boat;

    public float speedx;
    public GameObject endingPanel;
    float carX;
    float heliX;
    float boatX;

    public float currentSpeed;

    public Transform targetPos;
    public Transform targetCube;
    LayerMask lm = 11;
    private void Start()
    {
        carX = 2 * speedx;
        heliX = 0.5f * speedx;
        boatX = 1.5f * speedx;

        currentSpeed = carX;
    }
    public void SelectVehicle(string vehicle)
    {
        car.SetActive(false);
        heli.SetActive(false);
        boat.SetActive(false);

        if (vehicle == "car")
        {
            car.SetActive(true);
            currentSpeed = carX;
        }
        if (vehicle == "heli")
        {
            heli.SetActive(true);
            currentSpeed = heliX*3;
        }
        if (vehicle == "boat")
        {
            boat.SetActive(true);
            currentSpeed = boatX;
        }
    }
    public bool tapped = false;
    private void Update()
    {
        taptext.SetActive(!tapped);
        if (Vector3.Distance(transform.position, targetCube.position) <= 1)
        {
            Debug.Log("player win");
            endingPanel.SetActive(true);
            endingPanel.transform.GetChild(0).GetComponent<Text>().text = "Player Won";
        }
        if (Input.touchCount > 0)
        {
            tapped = true;
        }
        if (tapped )
        {
            transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * currentSpeed);
        }
        RaycastChecking();
    }

    void RaycastChecking()
    {
        RaycastHit hit;
        Vector3 forw = transform.forward * 1f;

        Debug.DrawRay(transform.position, transform.forward * 1f);

        if(Physics.Raycast(transform.position,forw,out hit, 1f,lm))
        {

        }
        else
        {
            Vector3 neworigin = transform.position+transform.forward;
            Debug.DrawRay(neworigin, -Vector3.up ,Color.cyan);

            if(Physics.Raycast(neworigin,-Vector3.up,out hit, 1f))
            {
                int layernum = hit.transform.gameObject.layer;

                if (layernum == 10)
                {
                    if (!car.activeInHierarchy && !heli.activeInHierarchy) 
                    {
                        currentSpeed = 0.01f;
                    }
                }
                if (layernum == 4)
                {
                    if (!boat.activeInHierarchy)
                    {
                        currentSpeed = 0.01f;
                    }
                    else
                    {
                        currentSpeed = boatX;
                    }
                }
            }
        }
    }
    public GameObject AI;
    public Transform playerstartPos;
    public Transform AIstartPos;
    public GameObject taptext;
    public void Refresh()
    {
        this.transform.position = playerstartPos.position;
        AI.transform.position = playerstartPos.position;
        SelectVehicle("car");
        tapped = false;

        AIcontroller.ai.SelectVehicle("car");

        endingPanel.SetActive(false);
    }

}
