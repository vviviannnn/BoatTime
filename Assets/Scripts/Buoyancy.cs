using LowPolyWater;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Buoyancy : MonoBehaviour
{
    public Transform[] floaters;

    [SerializeField] float buoyancy;
    [SerializeField] float weight;

    public float underwaterDrag = 3f;
    public float underwaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;

    public float waterHeight = 0f;

    Rigidbody rb;

    bool underwater = false;


    [SerializeField] GameObject water;



    public float waveHeight = 1f;
    public float waveFrequency = 0.2f;
    public float waveLength = 100f;
    public Vector3 waveOriginPosition = new Vector3(50.0f, 0.0f, 50.0f);






    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        int floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float distance = Vector3.Distance(floaters[i].position, waveOriginPosition);
            distance = (distance % waveLength) / waveLength;

            float waterTall = -(waveHeight * Mathf.Cos(Time.time * Mathf.PI * 2.0f * waveFrequency
            + (Mathf.PI * 2.0f * distance)));
            if (i==0)
            {
                Debug.Log(waterTall);
            }


            float difference = floaters[i].position.y - waterTall;

            if (difference < 0f)
            {
                rb.AddForceAtPosition(Vector3.up * buoyancy * Mathf.Abs(difference), floaters[i].position, ForceMode.Acceleration);
                floatersUnderwater++;

                if (!underwater)
                {
                    underwater = true;
                    SwitchState(underwater);
                }
            }
            else
            {
                rb.AddForceAtPosition(Vector3.down * weight, floaters[i].position, ForceMode.Acceleration);
            }
        }
        if (underwater && floatersUnderwater == 0)
        {
            underwater = false;
            SwitchState(underwater);
        }
    }

    public void SwitchState(bool waterState)
    {
        if (waterState)
        {
            rb.drag = underwaterDrag;
            rb.angularDrag = underwaterAngularDrag;
        }
        else
        {
            rb.drag = airDrag;
            rb.angularDrag = airAngularDrag;
        }
    }
}
