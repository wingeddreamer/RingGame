using UnityEngine;
using System.Collections;

using System.Collections.Generic;
public class RingCol : MonoBehaviour {

    public static List<string> calledfun = new List<string>();
    void Start () {
	
	}
	
	void Update () {
	
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "path") {
            HitPath();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        RingControl.RingCol = true;
    }

    private void OnTriggerExit(Collider other)
    {
        RingControl.RingCol = false;
    }

    void HitPath()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "HitPath")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("HitPath");
            RingControl.isHitPath = true;
            StartCoroutine(stopHit(1));
        }

    }
    IEnumerator stopHit(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        calledfun.Remove("HitPath");
        RingControl.isHitPath = false;
    }
}
