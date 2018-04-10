using UnityEngine;
using System.Collections;

public class RingRangeCol : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="path")
        RingControl.RingRangeCol = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "path") {
            RingControl.RingRangeCol = false;
        }
    }
}
