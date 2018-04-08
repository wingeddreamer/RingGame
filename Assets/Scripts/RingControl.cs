using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RingControl : MonoBehaviour
{

    static public bool RingCol, RingRangeCol, isHitPath = false;
    static public float hitUV;
    public static List<string> calledfun = new List<string>();

    void Start()
    {

    }

    void Update()
    {
        /*if (!RingCol && RingRangeCol)
            GetComponent<Renderer>().material.color = Color.white;
        else
            GetComponent<Renderer>().material.color = Color.red;*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "path")
        {

        }
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
            isHitPath = true;
            StartCoroutine(stopHit(1));
        }

    }
    IEnumerator stopHit(float waittime) {
        yield return new WaitForSeconds(waittime);
        calledfun.Remove("HitPath");
        isHitPath = false;
    }
}
