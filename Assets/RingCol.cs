using UnityEngine;
using System.Collections;

public class RingCol : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    private void OnTriggerStay(Collider other)
    {
        RingControl.RingCol = true;
    }

    private void OnTriggerExit(Collider other)
    {
        RingControl.RingCol = false;
    }
}
