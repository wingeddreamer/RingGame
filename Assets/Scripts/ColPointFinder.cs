using UnityEngine;
using System.Collections;

public class ColPointFinder : MonoBehaviour {

    public GameObject finder1, finder2, finderParent;

	void Start () {
	
	}
	
	void Update () {
        RaycastHit hitInfo;
        float rotAngle = 0, step = 15;
        while(!Physics.Raycast(finder1.transform.position, finder2.transform.position- finder1.transform.position, out hitInfo, (finder2.transform.position - finder1.transform.position).magnitude,1<<13))
        {
            rotAngle += step;
            finderParent.transform.eulerAngles = new Vector3(finderParent.transform.eulerAngles.x, finderParent.transform.eulerAngles.y, rotAngle);
            if (rotAngle == 360)
                step = 5;
            if (rotAngle == 720)
                step = 1;
            if (rotAngle == 1080)
                break;
        }
        RingControl.hitUV=hitInfo.textureCoord.x;
	}
}
