using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    public Transform CameraRotator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnEnable()
    {
        transform.rotation = CameraRotator.rotation;
    }
}
