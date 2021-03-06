﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StageClearCheckManager : MonoBehaviour
{
    private static StageClearCheckManager instance = null;

    
    int[,,] monsterCountForAreaClear = new int[10,10, 10];
    [SerializeField]
    int[,,] killedMosterCount = new int[10, 10, 10];
    [SerializeField]
    bool[,,] boolForAreaClear = new bool[10, 10, 10];


    int areaCount;


    [SerializeField]
    Animator[] doorOpenAni;












    void Start()
    {
        instance = this;
        if (null == instance)
        {
            instance = this;
        }

        Invoke("checkStageClear", 0.1f);

        areaCount = 0;
    }


    public static StageClearCheckManager Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }







    IEnumerator CamSetting()
    {
        yield return new WaitForSeconds(2f);

        Destroy(GameObject.FindWithTag("StageClearCam"));
        StopCoroutine("CamSetting");
    }





    // 죽으면 이 함수를 호출 함
    // 함수 호출시 카운트를 함 
    public void numMosterCount(int dungeonNum, int stage)
    {
        killedMosterCount[dungeonNum,stage, areaCount]++;

        //추후에 문이 추가 될 수 있으니 중복으로 놔둠
        if ((killedMosterCount[dungeonNum,stage, areaCount] == monsterCountForAreaClear[dungeonNum,stage, areaCount])
            && (boolForAreaClear[dungeonNum,stage, areaCount] == false))
        {
            boolForAreaClear[dungeonNum,stage, areaCount] = true;

            //해당 스테이지의 1구역을 처리 하면 다음 구역에 있는 문 열기
            switch (areaCount)
            {
                case 0:
                    doorOpenAni[0] = GameObject.Find("areaClearDoor_00").GetComponent<Animator>();
                    doorOpenAni[0].SetBool("openDoor", true);
                    break;
                case 1:
                    doorOpenAni[0] = GameObject.Find("areaClearDoor_01").GetComponent<Animator>();
                    doorOpenAni[0].SetBool("openDoor", true);
                    break;
                case 2:
                    doorOpenAni[0] = GameObject.Find("areaClearDoor_02").GetComponent<Animator>();
                    doorOpenAni[0].SetBool("openDoor", true);
                    break;
                case 3:
                    doorOpenAni[areaCount] = GameObject.Find("areaClearDoor_03").GetComponent<Animator>();
                    doorOpenAni[areaCount].SetBool("openDoor", true);
                    break;
            }
            areaCount++;
            return;
        }
    }




    //한 스테이지에 있는 한 구역당 처리를 하고 싶다.

    void checkStageClear()
    {
        for (int i = 1; i < 3; i++)
        {
            monsterCountForAreaClear[1, i, 0] = 1;
            monsterCountForAreaClear[1, i, 1] = 1;
        }
        for (int i = 3; i < 6; i++)
        {
            monsterCountForAreaClear[1, i, 0] = 2;
            monsterCountForAreaClear[1, i, 1] = 1;
        }
        monsterCountForAreaClear[1, 6, 0] = 1;

        for (int i = 1; i < 3; i++)
        {
            monsterCountForAreaClear[2, i, 0] = 1;
            monsterCountForAreaClear[2, i, 1] = 1;

        }
        for (int i = 3; i < 6; i++)
        {
            monsterCountForAreaClear[2, i, 0] = 2;
            monsterCountForAreaClear[2, i, 1] = 1;
        }
        monsterCountForAreaClear[2, 6, 0] = 1;
    }


    public void ifPlayerGoNextStageReset()
    {
        areaCount = 0;
    }
}
