using UnityEngine;
using System.Collections;

public class ClockControl : MonoBehaviour {

    public Material clockMat;
    float totalTime, penaltyTime, redTime;
    float leftTime, dampVel;
    bool countDown = false;
    public bool timeOut = false;
    public void initializeClock(float inTotalTime, float inPenaltyTime)
    {
        totalTime= leftTime = inTotalTime;
        penaltyTime = inPenaltyTime;
        timeOut = false;
    }

    public void beginCountDown()
    {
        countDown = true;
    }

    public void endCountDown()
    {
        countDown = false;
    }

    public void exePenalty()
    {
        redTime = leftTime;
        if (leftTime - penaltyTime > 0)
            leftTime -= penaltyTime;
        else
        {
            leftTime = 0;
            timeOut = true;
        }
    }

    void Start () {
        initializeClock(60, 5);
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.C)) countDown = true;
        if (Input.GetKeyDown(KeyCode.D)) countDown = false;
        if (countDown)
            if (leftTime - Time.deltaTime > 0)
                leftTime -= Time.deltaTime;
            else
            { leftTime = 0; timeOut = true; }
        redTime = Mathf.SmoothDamp(redTime, leftTime - 1, ref dampVel, 0.2f);
        clockMat.SetFloat("_TimeLeft", leftTime / totalTime);
        clockMat.SetFloat("_Penalty", redTime / totalTime);
        if (timeOut)
        {
            endCountDown();
        }
    }
}
