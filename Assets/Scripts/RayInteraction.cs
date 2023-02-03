using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteraction : MonoBehaviour
{
    public LayerMask whatIsTarget;
    private Camera playerCam;
    public float distance = 2f;
    GameObject Player;
    bool cubePressed;
    // Start is called before the first frame update
    void Start()
    {
        Player = transform.parent.gameObject;
        playerCam = Camera.main;
        cubePressed = false;
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
                if (hit.collider.gameObject.name == "Cube")
                {
                    cubePressed = true;
                    Debug.Log("Cube Pressed");
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