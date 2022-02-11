using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIcontroller : MonoBehaviour
{
    public static AIcontroller ai;
    private void Awake()
    {
        ai = this;
    }
    public GameObject car;
    public GameObject heli;
    public GameObject boat;

    public float speedx;

    float carX;
    float heliX;
    float boatX;

    public float currentSpeed;

    public Transform targetPos;
    public Transform targetcube;
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
    private void Update()
    {
        if (Vector3.Distance(transform.position,targetcube.position)<=1)
        {
            Debug.Log("ai win");
            PlayerController.pc.endingPanel.SetActive(true);
            PlayerController.pc.endingPanel.transform.GetChild(0).GetComponent<Text>().text = "AI Won";
        }
        if (PlayerController.pc.tapped)
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

        if (Physics.Raycast(transform.position, forw, out hit, 1f,lm))
        {
            if (hit.transform.tag == "hurdle")
            {
                SelectVehicle("heli");
            }
        }
        else
        {
            Vector3 neworigin = transform.position + transform.forward;
            Debug.DrawRay(neworigin, -Vector3.up, Color.cyan);

            if (Physics.Raycast(neworigin, -Vector3.up, out hit, 1f))
            {
                int layernum = hit.transform.gameObject.layer;

                if (layernum == 10)
                {
                    SelectVehicle("car");
                }
                if (layernum == 4)
                {
                    SelectVehicle("boat");
                }
            }
        }
    }
}
