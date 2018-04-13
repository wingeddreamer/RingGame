using UnityEngine;
using System.Collections;

public class Hidepiece : MonoBehaviour {
    public GameObject[] SidePieces;

    public GameObject CamRotator;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Hide Sidepieces behind camera to avoid visual occlusion
        foreach (GameObject _sidePiece in SidePieces) _sidePiece.SetActive(true);
        float PCNumber = Mathf.Floor((CamRotator.transform.eulerAngles.y + 22.5f) / 45f);
        int thisPC, prevPC, nextPC;
        if (PCNumber == 8)
            thisPC = 0;
        else
            thisPC = (int)PCNumber;
        SidePieces[thisPC].SetActive(false);
        if (PCNumber + 1 >= 8)
            nextPC = (int)PCNumber - 7;
        else
            nextPC = (int)PCNumber + 1;
        SidePieces[nextPC].SetActive(false);
        if (PCNumber - 1 <= -1)
            prevPC = (int)PCNumber + 7;
        else
            prevPC = (int)PCNumber - 1;
        SidePieces[prevPC].SetActive(false);
    }

}
