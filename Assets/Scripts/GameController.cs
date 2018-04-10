using UnityEngine;
using System.Collections;
using zSpace.Core.Samples;

using System.Collections.Generic;
public class GameController : MonoBehaviour
{

    public int Level1TotalTime = 60, Level2TotalTime = 90, Level3TotalTime = 120, penatyTime = 5;
    public bool isInTestMode = false;
    public GameObject ring, ringTarget, level2Text, level3Text, FailText, level1, level2, level3;
    public GameObject ring1Pos, ring2Pos, ring3Pos;
    //public GameObject level1Logo;
    public GameObject OldScript;
    bool disableKey = false;
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
    void Start() { 
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
            calledfun.Remove("replay");
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

                    //撞击路径惩罚
                    if (RingControl.isHitPath)
                    {
                        hitPath();
                    }
                    //离开路径归位
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
                isShowRing(false);
                FailText.SetActive(true);
                en_disableGrab(false);
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
                isShowRing(false);
                FailText.SetActive(true);
                en_disableGrab(false);
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
                isShowRing(false);
                FailText.SetActive(true);
                en_disableGrab(false);
            }
                
        }

        //重新开始游戏
        if (Input.GetKeyDown(KeyCode.R)&&!disableKey)
        {
            replay();
        }

    }
    void InitLevel(int levelNum)
    {
        switch (levelNum)
        {
            case 1:

                isShowRing(true);
                clockController.initializeClock(Level1TotalTime, penatyTime);
                level1.SetActive(true);
                level2.SetActive(false);
                level3.SetActive(false);
                level1.GetComponent<level1Controller>().BeforePlay();
                en_disableGrab(false);
                lightController.IsTurnOff(false);
                //设置ring的起始位置
                {
                    if (isInTestMode)
                    {
                        //test mode
                        ring.transform.position = new Vector3(14.5f, 4.347f, -0.09f);
                        ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                    }
                    else {
                        ring.transform.position = new Vector3(-15.6f, 4.6f, 0f);
                        ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                    }

                    ringTarget.transform.position = new Vector3(15.64f, 4.55f, -0.17f);
                    ringTarget.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                }
                break;
            case 2:
                clockController.initializeClock(Level2TotalTime, penatyTime);
                level2.SetActive(true);
                en_disableGrab(false);
                startLevel(2);

                //设置ring的起始位置
                {
                    if (isInTestMode)
                    { //test mode
                        ring.transform.position = new Vector3(-7.16f, 6.347f, -0.09f);
                        ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                    }
                    else
                    {
                        ring.transform.position = new Vector3(-14.89f, 15.5f, 0.021f);
                        ring.transform.rotation = Quaternion.Euler(0, -90f, -270f);
                    }
                    ringTarget.transform.position = new Vector3(-5.6f, 7.09f, -0.17f);
                    ringTarget.transform.rotation = Quaternion.Euler(22.0381f, -90f, -270f);
                }
                break;
            case 3:
                clockController.initializeClock(Level3TotalTime, penatyTime);
                level3.SetActive(true);
                en_disableGrab(false);
                startLevel(3);
                //设置ring的起始位置
                {

                    if (isInTestMode)
                    { //test mode
                        ring.transform.position = new Vector3(-10.36f, 14.45f, -6.974f);
                        ring.transform.rotation = Quaternion.Euler(82, -270f, -449f);
                    }
                    else
                    {
                        ring.transform.position = new Vector3(-11.536f, 6.7f, -11.167f);
                        ring.transform.rotation = Quaternion.Euler(0, -118.75f, -270f);
                    }
                    ringTarget.transform.position = new Vector3(-10.285f, 17.53f, -7.212f);
                    ringTarget.transform.rotation = Quaternion.Euler(90f, -180f, -360f);
                }
                break;
            default:
                break;
        }
    }
    void startLevel(int levelNum)
    {
        en_disableGrab(true);
        isShowRing(true);
        switch (levelNum)
        {
            case 1:
                level1.GetComponent<level1Controller>().StartPlay();
                disableKey = false;
                break;
            case 2:
                level2.GetComponent<level2Controller>().StartPlay();
                break;
            case 3:
                level3.GetComponent<level3Controller>().StartPlay();
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
                level3.GetComponent<level3Controller>().finishPlay();
                currState = 4;
                break;
            default:
                break;
        }

        isShowRing(false);
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
        bool isCalled = false;
        foreach (string fun in calledfun)
        {
            if (fun == "replay")
                isCalled = true;
        }

        if (!isCalled)
        {
            calledfun.Add("replay");
            failcontroller.Level1Fail = false;
            failcontroller.Level2Fail = false;
            failcontroller.Level3Fail = false;
            if (FailText.activeSelf)
                FailText.SetActive(false);
            playFireWork(false);
            StartCoroutine(ShowWithdelay(0, level2Text, false));
            StartCoroutine(ShowWithdelay(0, level3Text, false));
            resetRing();
            InitLevel(1);
            clockController.endCountDown();
            lightController.IsTurnOff(false);
            OldScript.GetComponent<Multi_Func>().reachTarget = false;
            currState = 0;
        }
          
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
            //1s后才能再次撞击，防止反复碰撞减时间
            StartCoroutine(enableHit(1f));
            OldScript.GetComponent<Multi_Func>().VibratePen();
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
            switch (currState) {
                case 1:
                    ring.transform.position = ring1Pos.transform.position;
                    ring.transform.rotation = ring1Pos.transform.rotation;
                    break;
                case 2:
                    ring.transform.position = ring2Pos.transform.position;
                    ring.transform.rotation = ring2Pos.transform.rotation;
                    break;
                case 3:
                    ring.transform.position = ring3Pos.transform.position;
                    ring.transform.rotation = ring3Pos.transform.rotation;
                    break;
                default:
                    Debug.Log("game in wrong state");
                    break;

            }
            StartCoroutine(enableMoveRingBack(0.1f));
        }
    }


    void resetRing()
    {
        //ring1Pos.transform.localPosition = new Vector3(-15.9f, 4.5f, 0.0f);
        //ring1Pos.transform.localRotation = new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f);
        //ring2Pos.transform.localPosition = new Vector3(0f, 0f, 0f);
        //ring2Pos.transform.localRotation = new Quaternion(0.6f, 0.6f, -0.4f, -0.4f);
        //ring3Pos.transform.localPosition = new Vector3(-11.5f, 6.7f, -11.3f);
        //ring3Pos.transform.localRotation = new Quaternion(0.6f, 0.6f, -0.4f, -0.4f);
        ring1Pos.GetComponent<test>().resetGhostPos();
        ring2Pos.GetComponent<test>().resetGhostPos();
        ring3Pos.GetComponent<test>().resetGhostPos();
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
        disableKey = true;
        yield return new WaitForSeconds(waittime);
        obToshowOrHide.SetActive(isShow);
        
    }

    IEnumerator InitWithdelay(float waittime, int levelNum)
    {
        yield return new WaitForSeconds(waittime);
        InitLevel(levelNum);
        disableKey = false;
    }

    void isShowRing(bool isEnable) {
        ring.GetComponent<MeshRenderer>().enabled = isEnable;
    }
}