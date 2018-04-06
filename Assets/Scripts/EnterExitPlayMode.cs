using UnityEngine;
using System.Collections;

public class EnterExitPlayMode : MonoBehaviour {

    public Animator[] parts;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            foreach (var part in parts) if (part!=null) part.SetBool("Playmode", true);
        if(Input.GetKeyDown(KeyCode.B))
            foreach (var part in parts) if (part != null) part.SetBool("Playmode", false);
    }
}
