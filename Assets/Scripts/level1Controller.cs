using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class level1Controller : MonoBehaviour {
    public GameObject startlogo;
    public ParticleSystem particle;
    public static List<string> calledfun = new List<string>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeforePlay()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "BeforePlay")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("BeforePlay");
            calledfun.Remove("StartPlay");
            calledfun.Remove("finishPlay");

            //temp
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            MeshRenderer[] meshrenders = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshren in meshrenders)
            {
                if (meshren.gameObject.name != "Level1_ani")
                    meshren.enabled = true;
            }
            startlogo.SetActive(true);
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
            calledfun.Remove("BeforePlay");
            calledfun.Remove("finishPlay");

            //temp
            gameObject.GetComponent<MeshRenderer>().enabled = true;

            MeshRenderer[] meshrenders = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshren in meshrenders)
            {
                if(meshren.gameObject.name != "Level1_ani")
                    meshren.enabled = true;
            }
            startlogo.SetActive(false);
        }

    }

    public void finishPlay() {
        bool isCalled = false;
        foreach (string fun in calledfun) {
            if (fun == "finishPlay")
                isCalled = true;
        }

        if (!isCalled) {
            calledfun.Add("finishPlay");
            calledfun.Remove("BeforePlay");
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
