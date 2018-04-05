using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour {

	public GameObject canv;
	static public bool noMoveInPos=false;
	static public bool autoReturn=false;
	static public bool useAngleCheck=true;
	static public bool useTwoButtonDis = true;
	public Toggle TG_noMoveInPos,TG_autoRetrun,TG_useAngleCheck,TG_useTowButtonDis;

	// Use this for initialization
	void Start () {
		canv.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F1))
			canv.SetActive (!canv.activeSelf);
		noMoveInPos = TG_noMoveInPos.GetComponent<Toggle> ().isOn;
		autoReturn = TG_autoRetrun.GetComponent<Toggle> ().isOn;
		useAngleCheck = TG_useAngleCheck.GetComponent<Toggle> ().isOn;
		useTwoButtonDis = TG_useTowButtonDis.GetComponent<Toggle> ().isOn;
	}
}
