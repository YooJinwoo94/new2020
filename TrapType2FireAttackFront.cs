﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapType2FireAttackFront : MonoBehaviour
{
    Transform fireBallTransform;
    Rigidbody rid;

    [SerializeField]
    GameObject boomParticle;

    BoxCollider boxCollider;
    MeshRenderer meshRenderer;

    ParticleSystem particleSys;


    // Start is called before the first frame update
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
        fireBallTransform = GetComponent<Transform>();
        rid = GetComponent<Rigidbody>();

        fireBallTransform.rotation = Quaternion.Euler(0, -180, 0);
        rid.velocity = gameObject.transform.forward * 20;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = true;


        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        boomParticle.SetActive(false);
    }






    void off()
    {
        particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        rid.velocity = gameObject.transform.forward * 0;

        boomParticle.SetActive(true);
        Destroy(gameObject, 3);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TrapType2FireAttack" || other.gameObject.tag == "TrapType02" || other.gameObject.tag == null) return;



        if (other.gameObject.tag == "Player")
        {
            if (PlayerInputScript.Instance.isDodge == true) return;

            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            rid.velocity = gameObject.transform.forward * 0;

            boomParticle.SetActive(true);
            Destroy(gameObject, 3);

            return;
        }
        if (other.gameObject.tag == "Wall")
        {
            if (PlayerInputScript.Instance.isDodge == true) return;

            particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            rid.velocity = gameObject.transform.forward * 0;

            boomParticle.SetActive(true);
            Destroy(gameObject, 3);

            return;
        }
        Invoke("off", 3f);
    }
}
