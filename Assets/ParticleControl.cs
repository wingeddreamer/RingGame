using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleControl : MonoBehaviour {

    public GameObject particleTypeA, particleTypeB;
    public GameObject particleB;
    public float timer=0,interval=1.5f;
    Color[] particleColor = new Color[6] { Color.white, Color.green, Color.red, Color.yellow, Color.cyan, Color.magenta};

    // Use this for initialization
    void Start () {
    }

    void startParticles()
    {
        GameObject newParticle = Instantiate(particleTypeA);
        newParticle.transform.position = new Vector3(Random.Range(-10f,10f),Random.Range(13,23), Random.Range(-10f, 10f));
        newParticle.GetComponent<ParticleSystem>().playbackSpeed = 2;
        newParticle.GetComponent<ParticleSystem>().startColor = particleColor[Random.Range(0,5)];
        Destroy(newParticle, 2);
    }

    private void OnEnable()
    {
        particleB = Instantiate(particleTypeB);
        particleB.transform.position = new Vector3(0, 18, 0);
        particleB.GetComponent<ParticleSystem>().playbackSpeed = 2;
        particleB.transform.GetChild(0).GetComponent<ParticleSystem>().playbackSpeed = 2;
        particleB.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().playbackSpeed = 3;
    }

    private void OnDisable()
    {
        particleB.GetComponent<ParticleSystem>().emissionRate = 0;
        Destroy(particleB,5);
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if(timer> interval)
        {
            startParticles();
            timer = 0;
            interval = Random.Range(0f, 2f);
        }
    }

}
