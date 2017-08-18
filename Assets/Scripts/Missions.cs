﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour {
    public Text mission_name;
    public Text mission_target;
    public Text mission_prize;
    public Image bar;

    public Economics economics;
    public CloneCenter cloneCenter;
    public GameObject notification;

    double target_value = 0;

    public int mission_number = -1;

    public MissionConstructor[] missions;
    MissionConstructor curMission;

    public void Setup()
    {
        print(mission_number);
        curMission = missions[mission_number];

        mission_name.text = curMission.name;
        mission_target.text = "" + curMission.target;
        mission_prize.text = "" + curMission.prize;

        target_value = double.Parse(curMission.target.Substring(0, curMission.target.Length-1));
    }

    private void FixedUpdate()
    {
        if (mission_number == -1)
            return;

        char lastSymbol = curMission.target[curMission.target.Length-1];
        double currentVal = 0;

        switch (lastSymbol)
        {
            case 'd':
                currentVal = economics.totalMoney;
                break;
            case 'p':
                currentVal = cloneCenter.clonesNum;
                break;
            case 'g':
                currentVal = economics.totalGoldCoins;     
                break;
        }

        bar.fillAmount = (float)(currentVal / target_value);

        if(target_value <= currentVal)
        {
            GetPrize();
        }
    }

    void GetPrize()
    {
        if (!notification.activeSelf && UIManager.SharedInstance.uiAnimators[14].GetInteger("State") != 1)
        {
            notification.SetActive(true);
            UIManager.SharedInstance.uiAnimators[14].gameObject.GetComponentInChildren<Text>().text = ""+curMission.prize;
        }
    }

    public void HideAndChangeNum()
    {
        print("hide");
        UIManager.SharedInstance.ShowExtraPersScreen(2);
        mission_number++;
        Setup();
    }

    public void ShowMessage()
    {
        UIManager.SharedInstance.ShowExtraPersScreen(1);
        notification.SetActive(false);
    }
}