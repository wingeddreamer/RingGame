using UnityEngine;
using System.Collections;

public class LevelFailVisControl : MonoBehaviour {

    public bool Level1Fail = false, Level2Fail = false, Level3Fail = false;
    public Animator[] Level1Objs, Level2Objs, Level3Objs;
    public GameObject[] Level3HideObjs;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Level1Fail)
        {
            foreach (var temp in Level1Objs) temp.SetBool("Visible", false);
        }
        else
        {
            foreach (var temp in Level1Objs) temp.SetBool("Visible", true);
        }
        if(Level2Fail)
        {
            foreach (var temp in Level2Objs) temp.SetBool("Visible", false);
        }
        else
        {
            foreach (var temp in Level2Objs) temp.SetBool("Visible", true);
        }
        if (Level3Fail)
        {
            foreach (var temp in Level3Objs) temp.SetBool("Visible", false);
            foreach (var temp in Level3HideObjs) temp.GetComponent<MeshRenderer>().enabled=false;
        }
        else
        {
            foreach (var temp in Level3Objs) temp.SetBool("Visible", true);
            foreach (var temp in Level3HideObjs) temp.GetComponent<MeshRenderer>().enabled = true;
        }
	}
}
