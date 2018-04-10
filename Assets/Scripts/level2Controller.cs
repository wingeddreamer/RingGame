using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class level2Controller : MonoBehaviour {
    public ParticleSystem particle;
    public static List<string> calledfun = new List<string>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

            //temp
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            MeshRenderer[] meshrenders = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshren in meshrenders)
            {
                if (meshren.gameObject.name != "Level2_ani")
                    meshren.enabled = true;
            }
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
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            MeshRenderer[] meshrenders = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshren in meshrenders)
            {
                meshren.enabled = false;
            }
            particle.Play();
        }

    }

}
