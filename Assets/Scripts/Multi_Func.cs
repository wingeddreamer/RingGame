using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

namespace zSpace.Core.Samples
{
	public class Multi_Func : MonoBehaviour
	{

		public Material GhostMat;
		public GameObject CamRotator;
		public Part[] SatParts;
		public GameObject[] SidePieces;
		private ZCore _core = null;
        public bool reachTarget =false;
        //for Camera rotator
        private float dampedvalue1 = 0, dampedvalue2 = 0, dampV1 = 0, dampV2 = 0;
		[HideInInspector]
		public GameObject CurrentlyGrabbedObject = null;
		[HideInInspector]
		public GameObject LastGrabbedObject = null;
		//from StylusObjectMani Script
		[HideInInspector]
		private int IndexOfCurrentlyGrabbedObject, IndexOfLastGrabbedObject;
		//from the list
		[HideInInspector]
		public Vector3 grabbedObjectPosition;
		[HideInInspector]
		public Quaternion grabbedObjectRotation;
		[HideInInspector]
		public bool lastFrameInpos=false;
        public static List<string> calledfun = new List<string>();
        // Use this for initialization
        void Start ()
		{
			_core = GameObject.FindObjectOfType<ZCore> ();

			if (_core == null) {
				Debug.LogError ("Unable to find reference to zSpace.Core.Core Monobehaviour.");
				this.enabled = false;
				return;
			}

			foreach (Part examingPart in SatParts) {
				examingPart.PosBeforeGrab = examingPart.RealPart.gameObject.transform.position;
				examingPart.RotBeforeGrab = examingPart.RealPart.gameObject.transform.rotation;
			}
		}
	
		// Update is called once per frame
		void Update ()
		{

			//Make Ghost Material Change over time
			GhostMat.SetColor ("_TintColor", new Color (0f, 1.0f, 1.0f, 0.07f + Mathf.Sin (Time.fixedTime * 2) * 0.03f));

			//find Currently Grabbed Object Index
			Part GrabPart = null;
			if (CurrentlyGrabbedObject != null) {
				for (int i = 0; i < SatParts.Length; i++) {
					if (SatParts [i].RealPart.gameObject == CurrentlyGrabbedObject.gameObject) {
						IndexOfCurrentlyGrabbedObject = i;
						GrabPart = SatParts [IndexOfCurrentlyGrabbedObject];
					}
				}
			} else {
				IndexOfCurrentlyGrabbedObject = -1;
				GrabPart = null;
			}
				
			//Rotate camera if no object is grabbed
			float secondButtonPressed = 0, thirdButtonPressed = 0;
			if (CurrentlyGrabbedObject == null) {
				if (_core.IsTargetButtonPressed (ZCore.TargetType.Primary, 1))
					secondButtonPressed = 20.0f;
				else
					secondButtonPressed = 0.0f;
				if (_core.IsTargetButtonPressed (ZCore.TargetType.Primary, 2))
					thirdButtonPressed = 20.0f;
				else
					thirdButtonPressed = 0.0f;
			}
			dampedvalue1 = Mathf.SmoothDamp (dampedvalue1, secondButtonPressed, ref dampV1, 0.5f);
			dampedvalue2 = Mathf.SmoothDamp (dampedvalue2, thirdButtonPressed, ref dampV2, 0.5f);
			CamRotator.transform.Rotate (new Vector3 (0.0f, (dampedvalue1 - dampedvalue2) * Time.deltaTime, 0.0f));


            //Snapping function
            if (CurrentlyGrabbedObject != null)
            {
                //position angle judgement
                bool posOK = (Vector3.Distance(grabbedObjectPosition, GrabPart.GhostPart.transform.position) < GrabPart.PositionTolerance);
                bool angOK = (Quaternion.Angle(grabbedObjectRotation, GrabPart.GhostPart.transform.rotation) < GrabPart.AngleTolerance);
                bool allOK;
                if (UI_Control.useAngleCheck)
                    allOK = posOK && angOK;
                else
                    allOK = posOK;
                if (allOK)
                {
                    isReachTarget();
                    CurrentlyGrabbedObject.transform.position = GrabPart.GhostPart.transform.position;
                    CurrentlyGrabbedObject.transform.eulerAngles = GrabPart.GhostPart.transform.eulerAngles;
                    GhostMat.SetColor("_TintColor", new Color(0.0f, 1.0f, 0.0f, 0.1f));
                    GrabPart.InTargetPosition = true;
                    if (!lastFrameInpos) _core.StartTargetVibration(ZCore.TargetType.Primary, 0.1f, 1f, 1, 0.1f);
                    //Update lastframe in position status for stylus vibration
                    lastFrameInpos = true;
                }
                else
                {
                    CurrentlyGrabbedObject.transform.position = grabbedObjectPosition;
                    CurrentlyGrabbedObject.transform.rotation = grabbedObjectRotation;
                    GrabPart.InTargetPosition = false;
                    lastFrameInpos = false;
                }
            }
            else if (LastGrabbedObject != null)
            {
                //disalbe further grabbing when in target position
                if (SatParts[IndexOfLastGrabbedObject].InTargetPosition == true)
                {
                    if (UI_Control.noMoveInPos)
                    {
                        SatParts[IndexOfLastGrabbedObject].allowGrab = false;
                        en_disableGrab(LastGrabbedObject, false);
                    }
                }
                else
                {
                    //return part to it's original position
                    if (UI_Control.autoReturn)
                    {
                        SatParts[IndexOfLastGrabbedObject].OnWayHome = true;
                        SatParts[IndexOfLastGrabbedObject].RotDampV = 1.0f;
                    }
                }
            }
            
			

			//return objects marked with onWayHome back to their origins
			for (int i = 0; i < SatParts.Length; i++) {
				if (SatParts [i].OnWayHome == true) {
					returnObject (i);
					if ((Vector3.Distance (SatParts [i].RealPart.transform.position, SatParts [i].PosBeforeGrab) < 0.01f)
						//Angel judgement
					    && Quaternion.Angle (SatParts [i].RealPart.transform.rotation, SatParts [i].RotBeforeGrab) < 0.01f) {
						SatParts [i].OnWayHome = false;
					}
				}
			}

			//Show ghost object when real one is grabbed
			for (int i = 0; i < SatParts.Length; i++) {
				if (i == IndexOfCurrentlyGrabbedObject)
					SatParts [i].GhostPart.SetActive (true);
				else {
					SatParts [i].GhostPart.SetActive (false);
				}
			}

			//Return All Parts
			if (Input.GetKeyDown (KeyCode.Space) || (UI_Control.useTwoButtonDis&&!_core.IsTargetButtonPressed (ZCore.TargetType.Primary, 0)&&_core.IsTargetButtonPressed (ZCore.TargetType.Primary, 1)&&_core.IsTargetButtonPressed (ZCore.TargetType.Primary, 2))) {
				for (int i = 0; i < SatParts.Length; i++) {
					SatParts [i].OnWayHome = true;
					SatParts [i].InTargetPosition = false;
					SatParts [i].allowGrab = true;
					en_disableGrab (SatParts[i].RealPart, true);
				}
			}

			//Update Lastgrabbed Object
			LastGrabbedObject = CurrentlyGrabbedObject;
			IndexOfLastGrabbedObject = IndexOfCurrentlyGrabbedObject;

			//Hide Sidepieces behind camera to avoid visual occlusion
			foreach (GameObject _sidePiece in SidePieces) _sidePiece.SetActive(true);
			float PCNumber=Mathf.Floor((CamRotator.transform.eulerAngles.y + 22.5f) / 45f);
			int thisPC, prevPC, nextPC;
			if (PCNumber == 8)
				thisPC = 0;
			else
				thisPC = (int)PCNumber;
			SidePieces [thisPC].SetActive (false);
			if (PCNumber + 1 >= 8)
				nextPC = (int) PCNumber-7;
			else
				nextPC = (int) PCNumber+1;
			SidePieces [nextPC].SetActive (false);
			if (PCNumber - 1 <= -1)
				prevPC = (int) PCNumber +7;
			else
				prevPC = (int) PCNumber-1;
			SidePieces [prevPC].SetActive (false);
		}

		//Disable objects not allowing grab
		public void en_disableGrab (GameObject targetObj, bool status)
		{
			BoxCollider[] tempBoxColList;
			CapsuleCollider[] tempCapColList;
			SphereCollider[] tempSphColList;
			tempBoxColList = targetObj.GetComponents<BoxCollider> ();
			tempCapColList = targetObj.GetComponents<CapsuleCollider> ();
			tempSphColList = targetObj.GetComponents<SphereCollider> ();
			foreach (BoxCollider tempBC in tempBoxColList)
				tempBC.enabled = status;
			foreach (CapsuleCollider tempCC in tempCapColList)
				tempCC.enabled = status;
			foreach (SphereCollider tempSC in tempSphColList)
				tempSC.enabled = status;
		}

		//Move an object back to its original position
		public void returnObject (int objIndex)
		{
			Part returnPart = SatParts [objIndex];
			Vector3 rPartPos = returnPart.RealPart.transform.position;
			Quaternion rPartRot = returnPart.RealPart.transform.rotation;
			returnPart.RealPart.transform.position = Vector3.SmoothDamp (rPartPos, returnPart.PosBeforeGrab, ref returnPart.PosDampV, 0.1f);
			returnPart.RealPart.transform.rotation = Quaternion.Slerp (rPartRot, returnPart.RotBeforeGrab, (1 - damping (ref returnPart.RotDampV)));
		}

		//Damping Rotation
		private float damping (ref float dampv)
		{
			dampv = dampv * 0.99f;
			return dampv;
		}

		//Check and return all floating objects
		public void returnFloatingObj()
		{
			if (!UI_Control.autoReturn) {
				for (int i = 0; i < SatParts.Length; i++) {
					print (SatParts [i].RealPart.name + SatParts [i].InTargetPosition);
					if (!SatParts [i].InTargetPosition)
						SatParts [i].OnWayHome = true;
				}
			}
		}
        void isReachTarget()
        {
            bool isCalled = false;
            foreach (string fun in calledfun)
            {
                if (fun == "isReachTarget")
                    isCalled = true;
            }

            if (!isCalled)
            {
                calledfun.Add("isReachTarget");
                reachTarget = true;
                StartCoroutine(stopReach(2));
            }
        }


        IEnumerator stopReach(float waittime)
        {
            yield return new WaitForSeconds(waittime);
            calledfun.Remove("isReachTarget");
        }
    }

   

	[System.Serializable]
	public class Part
	{
		public GameObject RealPart;
		public GameObject GhostPart;
		public float AngleTolerance = 15.0f;
		public float PositionTolerance = 1.0f;
		[HideInInspector]	public Vector3 PosBeforeGrab;
		[HideInInspector]	public Quaternion RotBeforeGrab;
		[HideInInspector]	public bool allowGrab = true;
		[HideInInspector]	public bool InTargetPosition = false;
		[HideInInspector]	public bool OnWayHome = false;
		[HideInInspector]	public bool Grabbed = false;
		[HideInInspector]	public Vector3 PosDampV;
		[HideInInspector]	public float RotDampV;
	}
}