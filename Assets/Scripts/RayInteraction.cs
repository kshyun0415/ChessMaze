using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteraction : MonoBehaviour
{
    public LayerMask whatIsTarget;
    private Camera playerCam;
    public float distance = 2f;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.gameObject;
        playerCam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 rayDir = playerCam.transform.forward;
        Debug.DrawRay(rayOrigin, rayDir * distance, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, rayDir, out hit, distance, whatIsTarget))
            {
                if (hit.collider.gameObject.tag == "SafeZone")
                {
                    Debug.Log("Safe");
                    GameManager.Instance.isPlayerHidden = true;
                    GameManager.Instance.safeView = hit.collider.gameObject.transform.GetChild(0).transform;
                }
                if (hit.collider.gameObject.tag == "Feather")
                {
                    Debug.Log("Feather Clicked");
                    Destroy(hit.collider.gameObject);
                    GameManager.Instance.featherCount += 1;
                }
                if (hit.collider.gameObject.tag == "Potion")
                {
                    Debug.Log("Potion");
                    Destroy(hit.collider.gameObject);
                    GameManager.Instance.RestoreHealth(50f);
                }

            }

        }
    }
}

// if (hit.collider.gameObject.tag == "Pawn")
//                 {
//                     Destroy(hit.collider.gameObject);
//                     GameManager.Instance.AddScore(1);

//                 }