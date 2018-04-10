using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RingControl : MonoBehaviour
{

    static public bool RingCol, RingRangeCol, isHitPath = false;
    static public float hitUV;
    public static List<string> calledfun = new List<string>();
    public GameObject shockLight;
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

    public void playShockLight() {
        shockLight.SetActive(true);
        shockLight.GetComponent<ParticleSystem>().Play();
       // StartCoroutine(stopshockLight(0.05f));
    }

    IEnumerator stopshockLight(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        shockLight.SetActive(false);
        shockLight.GetComponent<ParticleSystem>().Stop();
    }
}
