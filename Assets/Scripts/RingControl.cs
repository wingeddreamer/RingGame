using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RingControl : MonoBehaviour
{

    static public bool RingRangeCol, isHitPath = false;
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
    }

    IEnumerator stopshockLight(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        shockLight.SetActive(false);
        shockLight.GetComponent<ParticleSystem>().Stop();
    }
}
