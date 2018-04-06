using UnityEngine;
using System.Collections;

public class Penalty : MonoBehaviour {

    public ClockControl CCscript;
    public GameObject targetCamera,errLight,lightning,ring;
    public Vector3 camOriginalPos;
    public bool inPenaltyMode = false;

	// Use this for initialization
	void Start () {
        camOriginalPos = targetCamera.transform.localPosition;
        endPenalty();
	}
	
	// Update is called once per frame
	void Update () {
        if(!CCscript.timeOut)
            if (Input.GetKeyDown(KeyCode.E))
            {
                CCscript.exePenalty();
                inPenaltyMode = true;
                Invoke("endPenalty",0.25f);
                errLight.SetActive(true);
                lightning.SetActive(true);
                ring.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", Color.red);
                ring.GetComponent<Renderer>().sharedMaterial.color = Color.red;
            }
        if (inPenaltyMode)
        {
            cameraShake(0.5f);
        }
	}

    void cameraShake(float vibration)
    {
        targetCamera.transform.localPosition = camOriginalPos + new Vector3(Random.Range(-vibration, vibration), Random.Range(-vibration, vibration),Random.Range(-vibration, vibration));
    }

    void endPenalty()
    {
        inPenaltyMode = false;
        targetCamera.transform.localPosition = camOriginalPos;
        errLight.SetActive(false);
        lightning.SetActive(false);
        ring.GetComponent<Renderer>().sharedMaterial.color = Color.white;
        ring.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", new Color(0.01f, 0.01f, 0.01f));
    }
}
