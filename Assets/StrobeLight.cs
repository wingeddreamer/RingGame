using UnityEngine;
using System.Collections;

public class StrobeLight : MonoBehaviour {

    int timeInFrame = 0, interval = 2;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeInFrame>interval)
        {
            GetComponent<Light>().intensity = Random.Range(0, 5f);
            interval = Random.Range(1, 8);
            timeInFrame = 0;
        }
        timeInFrame++;
    }
}
