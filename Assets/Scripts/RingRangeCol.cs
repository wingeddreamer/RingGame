using UnityEngine;
using System.Collections;

public class RingRangeCol : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    private void OnTriggerStay(Collider other)
    {
        RingControl.RingRangeCol = true;
    }

    private void OnTriggerExit(Collider other)
    {
        RingControl.RingRangeCol = false;
    }
}
