using UnityEngine;
using System.Collections;

public class LightCoatControl : MonoBehaviour {

    Vector3 Vec;
    float VecX=0;
    public float Speed,Offset;

	void Start () {
	
	}
	
	void Update () {

        float xScale=GetComponent<Renderer>().sharedMaterial.GetTextureScale("_MainTex").x;

        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector3(1-RingControl.hitUV*xScale+Offset,0,0));

        //Control LightFlow
        if (VecX <= -1) VecX = 1;
        else VecX -= Speed * Time.deltaTime;
        Vec = new Vector3(VecX, 0, 0);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_DetailAlbedoMap",Vec);
	}
}
