﻿using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public AnimationClip testAni;
    public GameObject ring;
    public static float pos = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Animation>().Play();
        //GetComponent<Animation>()["Take 001"].normalizedTime = 0;
        GetComponent<Animation>()["Take 001"].speed = 0f;
    }

    // Update is called once per frame
    void Update () {
            pos = RingControl.hitUV;
            GetComponent<Animation>()["Take 001"].normalizedTime = pos;
    }
    public void resetGhostPos() {
        GetComponent<Animation>().Play();
        GetComponent<Animation>()["Take 001"].speed = 0f;
        pos = 0;
        GetComponent<Animation>()["Take 001"].normalizedTime = pos;
    }
}
