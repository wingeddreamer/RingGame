﻿using UnityEngine;
using System.Collections;
using zSpace.Core.Samples;

using System.Collections.Generic;
public class GameController : MonoBehaviour
{
    public int Level1TotalTime = 60, Level2TotalTime = 90, Level3TotalTime = 120, penatyTime1 = 5, penatyTime2 = 5, penatyTime3 = 5;
    public bool isInTestMode = false;
    public GameObject ring, ringTarget, level2Text, level3Text, FailText, level1, level2, level3;
    public GameObject ring1Pos, ring2Pos, ring3Pos;
    //public GameObject level1Logo;
    public GameObject OldScript;
    bool disableKey = false;
    ClockControl clockController;
    EnterExitPlayMode lightController;
    Multi_Func multiFuc;
    StylusObjectManipulationSample smultifun;
    ParticleControl fireWork;
    Penalty errorcontroller;
    LevelFailVisControl failcontroller;
    public ColPointFinder CPF;
    public RingRangeCol RRC;

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
        clockController = gameObject.GetComponent<ClockControl>();
        lightController = gameObject.GetComponent<EnterExitPlayMode>();
        fireWork=gameObject.GetComponent<ParticleControl>();
        errorcontroller = gameObject.GetComponent<Penalty>();
        failcontroller = gameObject.GetComponent<LevelFailVisControl>();
        multiFuc = OldScript.GetComponent<Multi_Func>();
        smultifun = OldScript.GetComponent<StylusObjectManipulationSample>();
        InitLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == 0) {
            if (Input.anyKeyDown|| smultifun.isanyButtonPressed)
            {
                currState = 1;
                startLevel(1);
            }
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
                    if (RingControl.isHitPath|| !RingControl.RingRangeCol)
                    {
                        
                        en_disableGrab(false);
                        MoveRingBack();
                        hitPath();
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
                    if (RingControl.isHitPath || !RingControl.RingRangeCol)
                    {

                        en_disableGrab(false);
                        MoveRingBack();
                        hitPath();
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
                    if (RingControl.isHitPath || !RingControl.RingRangeCol)
                    {

                        en_disableGrab(false);
                        MoveRingBack();
                        hitPath();
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
        CPF.resetUV();
        switch (levelNum)
        {
            case 1:

                isShowRing(true);
                clockController.initializeClock(Level1TotalTime, penatyTime1);
                level1.SetActive(true);
                level2.SetActive(false);
                level3.SetActive(false);
                level1.GetComponent<level1Controller>().BeforePlay();
                //en_disableGrab(false);
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
                clockController.initializeClock(Level2TotalTime, penatyTime2);
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
                    ringTarget.transform.position = new Vector3(-6.82f, 6.599f, -0.17f);
                    ringTarget.transform.rotation = Quaternion.Euler(28.2326f, -90f, -270f);
                }
                break;
            case 3:
                clockController.initializeClock(Level3TotalTime, penatyTime3);
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
        calledfun.Remove("replay");
        switch (levelNum)
        {
            case 1:
                level1.GetComponent<level1Controller>().StartPlay();
                disableKey = false;
                break;
            case 2:
                level2.GetComponent<level2Controller>().StartPlay();
                currState = 2;
                break;
            case 3:
                level3.GetComponent<level3Controller>().StartPlay();
                currState = 3;
                break;

            default:
                break;
        }
        clockController.beginCountDown();
        lightController.IsTurnOff(true);
    }
    void FinishLevel(int levelNum)
    {
        en_disableGrab(false);
        RingControl.isHitPath = false;
        RingControl.RingRangeCol = true;
        switch (levelNum)
        {
            case 1:
                lightController.IsTurnOff(false);
                level1.GetComponent<level1Controller>().finishPlay();
                StartCoroutine(ShowWithdelay(2, level2Text,true));
                StartCoroutine(ShowWithdelay(2, level1, false));
                
                break;
            case 2:
                lightController.IsTurnOff(false);
                level2.GetComponent<level2Controller>().finishPlay();
                StartCoroutine(ShowWithdelay(2, level3Text, true));
                StartCoroutine(ShowWithdelay(2, level2, false));
                break;
            case 3:
                level3.GetComponent<level3Controller>().finishPlay();
                break;
            default:
                break;
        }

        OldScript.GetComponent<Multi_Func>().reachTarget = false;
        isShowRing(false);
        clockController.endCountDown();
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

    void en_disableDetect(bool isEnable)
    {
        RRC.enabled = isEnable;
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
            smultifun.isanyButtonPressed = false;
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
            //0.5s后才能再次撞击，防止反复碰撞减时间
            StartCoroutine(enableHit(0.5f));
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

        if (!isCalled)
        {
            calledfun.Add("MoveRingBack");

            switch (currState)
            {
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
            StartCoroutine(EnableLater(0.1f));
        }
    }

    void resetRing()
    {
        ring1Pos.GetComponent<test>().resetGhostPos();
        ring2Pos.GetComponent<test>().resetGhostPos();
        ring3Pos.GetComponent<test>().resetGhostPos();
    }


    IEnumerator enableHit(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        calledfun.Remove("hitPath");
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

    IEnumerator EnableLater(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        calledfun.Remove("MoveRingBack");
    }


    void isShowRing(bool isEnable) {
        ring.GetComponent<MeshRenderer>().enabled = isEnable;
    }
}