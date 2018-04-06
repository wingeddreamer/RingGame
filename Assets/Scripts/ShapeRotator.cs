using UnityEngine;
using System.Collections;

public class ShapeRotator : MonoBehaviour {

    public bool CW;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, (CW?10:-10)*Time.deltaTime, 0));
	}
}
