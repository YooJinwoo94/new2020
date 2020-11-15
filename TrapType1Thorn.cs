﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum TrapType1State{
    normal,
    readyForAttack,
    attack
}



public class TrapType1Thorn : MonoBehaviour
{
    Renderer rendererColor;
    TrapType1State TrapType1State;
   // BoxCollider trapType1ThornCollider;

    // Start is called before the first frame update
    void Start()
    {
        //  trapType1ThornCollider = GetComponent<BoxCollider>();
        //trapType1ThornCollider.enabled = true;
        rendererColor = gameObject.GetComponent<Renderer>();
        TrapType1State = TrapType1State.normal;
        gameObject.tag = "Untagged";
    }


    void attack()
    {
        rendererColor.material.color = Color.red;
        gameObject.tag = "TrapType1Thorn";
        TrapType1State = TrapType1State.attack;
    }

      void reset()
    {
        rendererColor.material.color = Color.white;
        TrapType1State = TrapType1State.normal;
        gameObject.tag = "Untagged";
    }




    IEnumerator StartTrapType1Thorn()
    {
        yield return null;
        // 애니메이션 작동 
        yield return new WaitForSeconds(0.5f);
        rendererColor.material.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        attack();

        yield return new WaitForSeconds(1.5f);
        reset();
        StopCoroutine("StartTrapType1Thorn");
    }


    private void OnTriggerStay(Collider other)
    {
        if (TrapType1State != TrapType1State.normal) return;

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            TrapType1State = TrapType1State.readyForAttack;
            StartCoroutine("StartTrapType1Thorn");
        }
    }
}