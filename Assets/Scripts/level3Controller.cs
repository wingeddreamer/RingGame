using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class level3Controller : MonoBehaviour {
    public GameObject ring3pos,shape1,shape2,shape3,shape4,shape5;
    Vector3 shape1Point = new Vector3(11.8f, 12.9f, -6.7f);
    Vector3 shape2Point = new Vector3(0.0f, 14.5f, 13.5f);
    Vector3 shape3Point = new Vector3(-12.7f, 2.5f, -5.0f);
    Vector3 shape4Point = new Vector3(-10.3f, 16.4f, -7.0f);

    public static List<string> calledfun = new List<string>();
    // Use this for initialization
    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update () {

        if (test.pos > 0.229f)
        {
            shape1.SetActive(true);
        }

        if (test.pos > 0.6f)
        {
            shape2.SetActive(true);
        }

        if (test.pos > 0.9f)
        {
            shape3.SetActive(true);
        }
        if (test.pos > 0.985f)
        {
            shape4.SetActive(true);
        }
        if (test.pos > 0.997f)
        {
            shape5.SetActive(true);
        }
    }

    public void StartPlay()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "StartPlay")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("StartPlay");
            calledfun.Remove("finishPlay");

            shape1.SetActive(false);
            shape2.SetActive(false);
            shape3.SetActive(false);
            shape4.SetActive(false);
            shape5.SetActive(false);
        }

    }

    public void finishPlay()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "finishPlay")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("finishPlay");
            calledfun.Remove("StartPlay");
         
        }

    }
}
