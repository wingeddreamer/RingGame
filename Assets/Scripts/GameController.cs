using UnityEngine;
using System.Collections;
using zSpace.Core.Samples;

using System.Collections.Generic;
public class GameController : MonoBehaviour
{
    public GameObject ring, ringTarget, level2Text, level3Text, FailText, level1, level2, level3;
    public GameObject ring1Pos, ring2Pos, ring3Pos;
    //public GameObject level1Logo;
    public GameObject OldScript;
    ClockControl clockController;
    EnterExitPlayMode lightController;
    Multi_Func multiFuc;
    ParticleControl fireWork;
    Penalty errorcontroller;
    LevelFailVisControl failcontroller;
    public static List<string> calledfun = new List<string>();
    bool isLevel1Finished = false, isLevel2Finished = false, islevel3Finished = false;
    /// <summary>
    /// 0:ready
    /// 1:level1                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    /// 2:level2
    /// 3:level3
    /// </summary>
    public static int currState = 0;
    // Use this for initialization
    void Start()
    {
        en_disableGrab(false);
        clockController = gameObject.GetComponent<ClockControl>();
        lightController = gameObject.GetComponent<EnterExitPlayMode>();
        fireWork=gameObject.GetComponent<ParticleControl>();
        errorcontroller = gameObject.GetComponent<Penalty>();
        failcontroller = gameObject.GetComponent<LevelFailVisControl>();
        multiFuc = OldScript.GetComponent<Multi_Func>();
        InitLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && currState==0)
        {
            currState = 1;
            startLevel(1);
        }

        if (currState == 1)
        {
            if (!clockController.timeOut)
            {
                if (OldScript.GetComponent<Multi_Func>().reachTarget)
                {
                    FinishLevel(1);
                    StartCoroutine(InitWithdelay(6, 2));
                }
                else
                {
                    if (RingControl.isHitPath)
                    {
                        hitPath();
                    }

                    if (!RingControl.RingRangeCol)
                    {
                        en_disableGrab(false);
                        MoveRingBack();
                    }
                    else {
                        en_disableGrab(true);
                    }
                }
            }
            else {
                failcontroller.Level1Fail = true;
                FailText.SetActive(true);
            }
          
        }

        if (currState == 2)
        {
            if (!clockController.timeOut)
            {
                if (OldScript.GetComponent<Multi_Func>().reachTarget)
                {
                    FinishLevel(2);
                    StartCoroutine(InitWithdelay(6, 3));
                }
                else
                {
                    if (RingControl.isHitPath)
                    {
                        hitPath();
                    }

                    if (!RingControl.RingRangeCol)
                    {
                        en_disableGrab(false);
                        MoveRingBack();
                    }
                    else
                    {
                        en_disableGrab(true);
                    }
                }
            }
            else {
                failcontroller.Level2Fail = true;
                FailText.SetActive(true);
            }
               
        }
        
        if (currState == 3)
        {
            if (!clockController.timeOut)
            {
                if (OldScript.GetComponent<Multi_Func>().reachTarget)
                {
                    FinishLevel(3);
                    playFireWork(true);
                }
                else
                {
                    if (RingControl.isHitPath)
                    {
                        hitPath();
                    }

                    if (!RingControl.RingRangeCol)
                    {
                        en_disableGrab(false);
                        MoveRingBack();
                    }
                    else
                    {
                        en_disableGrab(true);
                    }
                }
            }
            else {
                failcontroller.Level3Fail = true;
                FailText.SetActive(true);
            }
                
        }

        //if (RingControl.RingRangeCol)
        //{
        //    en_disableGrab(true);
        //}
        //else {
        //    en_disableGrab(false);
        //}
        //重新开始游戏
        if (Input.GetKeyDown(KeyCode.R))
        {
            replay();
        }

    }
    void InitLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                level1.SetActive(true);
                level2.SetActive(false);
                level3.SetActive(false);
                level1.GetComponent<level1Controller>().BeforePlay();
                en_disableGrab(false);
                ring.transform.position = new Vector3(-15.625f, 4.523f, 0f);
                ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                ringTarget.transform.position = new Vector3(14.51f,4.55f, -0.17f);
                ringTarget.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                break;
            case 2:
                level2.SetActive(true);
                startLevel(2);
                ring.transform.position = new Vector3(-14.89f, 15.5f, 0.021f);
                ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                ringTarget.transform.position = new Vector3(-5.6f, 7.09f, -0.17f);
                ringTarget.transform.rotation = Quaternion.Euler(22.0381f, -90f, -270f);
                break;
            case 3:
                level3.SetActive(true);
                startLevel(3);
                ring.transform.position = new Vector3(-11.536f, 6.7f, -11.167f);
                ring.transform.rotation = Quaternion.Euler(0, -118.75f, -270f);
                ringTarget.transform.position = new Vector3(-10.285f, 17.53f, -7.212f);
                ringTarget.transform.rotation = Quaternion.Euler(90f, -180f, -360f);
                break;
            default:
                break;
        }
        ring.SetActive(true);
    }
    void startLevel(int levelNum)
    {
        en_disableGrab(true);
        switch (levelNum)
        {
            case 1:
                clockController.initializeClock(90, 5);
                level1.GetComponent<level1Controller>().StartPlay();
                break;
            case 2:
                clockController.initializeClock(90, 5);
                
                break;
            case 3:
                clockController.initializeClock(120, 5);
                break;

            default:
                break;
        }
        clockController.beginCountDown();
        lightController.IsTurnOff(true);
    }
    void FinishLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                level1.GetComponent<level1Controller>().finishPlay();
                StartCoroutine(ShowWithdelay(2, level2Text,true));
                StartCoroutine(ShowWithdelay(2, level1, false));
                currState = 2;
                OldScript.GetComponent<Multi_Func>().reachTarget = false;

                break;
            case 2:
                level2.GetComponent<level2Controller>().finishPlay();
                StartCoroutine(ShowWithdelay(2, level3Text, true));
                StartCoroutine(ShowWithdelay(2, level2, false));
                OldScript.GetComponent<Multi_Func>().reachTarget = false;
                currState = 3;
                break;
            case 3:
                

                break;

            default:
                break;
        }

        ring.SetActive(false);
        clockController.endCountDown();
        lightController.IsTurnOff(false);
        //ringState.allOK = false;
    }


    void en_disableGrab(bool isEnable)
    {
        if (!isEnable) {
            OldScript.GetComponent<Multi_Func>().CurrentlyGrabbedObject = null;
            OldScript.GetComponent<Multi_Func>().LastGrabbedObject = null;
            OldScript.GetComponent<StylusObjectManipulationSample>()._stylusState = 0;
        }
        OldScript.GetComponent<Multi_Func>().enabled = isEnable;
        OldScript.GetComponent<StylusObjectManipulationSample>().enabled = isEnable;
    }

    void replay()
    {
        FailText.SetActive(false);
        failcontroller.Level1Fail = false;
        failcontroller.Level2Fail = false;
        failcontroller.Level3Fail = false;
        currState = 0;
        InitLevel(1);
        clockController.endCountDown();
        lightController.IsTurnOff(false);
    }

    void playFireWork(bool isPlay) {
        fireWork.enabled = isPlay;
    }


    void hitPath()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "hitPath")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("hitPath");
            ring.GetComponent<RingControl>().playShockLight();
            errorcontroller.startPenalty();
            //en_disableGrab(false);
            //MoveRingBack();
            StartCoroutine(enableHit(1f));
        }
    }

    void MoveRingBack()
    {
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "MoveRingBack")
                isCalled = true;
        }

       // if (!isCalled)
        {
            calledfun.Add("MoveRingBack");
            ring.transform.position = ring1Pos.transform.position;
            ring.transform.rotation = ring1Pos.transform.rotation;
            StartCoroutine(enableMoveRingBack(0.1f));
        }
    }


    IEnumerator enableHit(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        calledfun.Remove("hitPath");
    }

    IEnumerator enableMoveRingBack(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        
        calledfun.Remove("MoveRingBack");
    }
    IEnumerator ShowWithdelay(float waittime, GameObject obToshowOrHide,bool isShow)
    {
        yield return new WaitForSeconds(waittime);

        obToshowOrHide.SetActive(isShow);
    }

    IEnumerator InitWithdelay(float waittime, int levelNum)
    {
        yield return new WaitForSeconds(waittime);
        InitLevel(levelNum);
    }
}