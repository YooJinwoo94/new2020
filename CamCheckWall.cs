﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider))]
public class CamCheckWall : MonoBehaviour
{

    private CapsuleCollider capsuleCollider;
    private List<GameObject> listPrevObstacleObject = new List<GameObject>();

    [SerializeField]
    Transform cam;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

    GameObject obj;

    bool stayCheck;

    // Use this for initialization
    void Awake()
    {
        stayCheck = false;
        StartCoroutine("turnOn");

        capsuleCollider = GetComponent<CapsuleCollider>();

        skinnedMeshRenderer.materials[0].SetFloat("_Outline", 0f);
        skinnedMeshRenderer.materials[1].SetFloat("_Outline", 0f);


    }


    IEnumerator turnOn()
    {
        yield return new WaitForSeconds(0.1f);
        stayCheck = true;
        StopCoroutine("turnOn");
    }








    // Update is called once per frame
    void FixedUpdate()
    {
    

      if (cam == null)
        {
            obj = GameObject.Find("Camera");

            cam = obj.transform;
        }



        Vector3 pointCenter = transform.TransformPoint(capsuleCollider.center);
        Vector3 pointLeft = transform.TransformPoint(capsuleCollider.center) - new Vector3(capsuleCollider.radius, 0, 0);
        Vector3 pointRight = transform.TransformPoint(capsuleCollider.center) + new Vector3(capsuleCollider.radius, 0, 0);
        Vector3 pointUp = transform.TransformPoint(capsuleCollider.center) + new Vector3(0, capsuleCollider.height / 2.0f, 0);
        Vector3 pointDown = transform.TransformPoint(capsuleCollider.center) - new Vector3(0, capsuleCollider.height / 2.0f, 0);

        List<Ray> listRay = new List<Ray>();
        Vector3 targetPosition = cam.position;    // camera world position

        listRay.Add(new Ray(pointCenter, targetPosition - pointCenter));
        listRay.Add(new Ray(pointLeft, targetPosition - pointLeft));
        listRay.Add(new Ray(pointRight, targetPosition - pointRight));
        listRay.Add(new Ray(pointUp, targetPosition - pointUp));
        listRay.Add(new Ray(pointDown, targetPosition - pointDown));

        List<RaycastHit[]> listHitInfo = new List<RaycastHit[]>();

        //
        foreach (Ray ray in listRay)
        {
            RaycastHit[] hitInfo = Physics.RaycastAll(ray, 1000.0f);
            listHitInfo.Add(hitInfo);

            // Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);
        }

        //
        RaycastHit[] listHit = listHitInfo[0];

        List<GameObject> listNewObstacleObject = new List<GameObject>();

        foreach (RaycastHit hitInfo in listHit)
        {
            if (gameObject.name == hitInfo.collider.name)
            {
                continue;
            }
            if (hitInfo.collider.name.Contains("Bip"))
            {
                continue;
            }

            if (FindColliderByName(listHitInfo[1], hitInfo.collider.name)
                && FindColliderByName(listHitInfo[2], hitInfo.collider.name)
                && FindColliderByName(listHitInfo[3], hitInfo.collider.name)
                && FindColliderByName(listHitInfo[4], hitInfo.collider.name)
                )
            {
                listNewObstacleObject.Add(hitInfo.transform.gameObject);
            }
        }

        // new
        foreach (GameObject obstacleObject in listNewObstacleObject)
        {
            // add
            if (stayCheck == true &&  !listPrevObstacleObject.Find(delegate (GameObject inObject) { return (inObject.name == obstacleObject.name); }))
            {
                // changed to transparent
                string nameShader = "Transparent/VertexLit";

                MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
                renderer.material.shader = Shader.Find(nameShader);
                if (renderer.material.HasProperty("_Color"))
                {
                    Color prevColor = renderer.material.GetColor("_Color");
                    renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0.5f));

                   
                    skinnedMeshRenderer.materials[0].SetFloat("_Outline", 0.15f);
                    skinnedMeshRenderer.materials[1].SetFloat("_Outline", 0.15f);
                    /*
                   Debug.Log("dd");
                   */
                }

            }
        }
        // prev
        foreach (GameObject obstacleObject in listPrevObstacleObject)
        {
            // remove
            if (!listNewObstacleObject.Find(delegate (GameObject inObject) { return (inObject.name == obstacleObject.name); }))
            {
                // changed to opaque
                string nameShader = "Standard";
                MeshRenderer renderer = obstacleObject.GetComponent<MeshRenderer>();
                renderer.material.shader = Shader.Find(nameShader);
                Color prevColor = renderer.material.GetColor("_Color");
                renderer.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 1f));

               
                skinnedMeshRenderer.materials[0].SetFloat("_Outline", 0f);
                skinnedMeshRenderer.materials[1].SetFloat("_Outline", 0f);
                /*
               Debug.Log("xx");
               */
            }
        }
        // swap
        listPrevObstacleObject = listNewObstacleObject;

        
        if (stayCheck == false)
        {
            skinnedMeshRenderer.materials[0].SetFloat("_Outline", 0f);
            skinnedMeshRenderer.materials[1].SetFloat("_Outline", 0f);
        }
        
    }

    private bool FindColliderByName(RaycastHit[] inListRayCastInfo, string inName)
    {
        foreach (RaycastHit hitInfo in inListRayCastInfo)
        {

            if (hitInfo.collider.name == inName)
            { return true; }
        }

        return false;
    }
}