// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// public class KING : MonoBehaviour
// {
//     public Transform kingsEye;
//     public float fieldOfView = 50f;
//     public float viewDistance = 10f;

// #if UNITY_EDITOR

//     private void OnDrawGizmosSelected()
//     {


//         var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
//         var leftRayDirection = leftRayRotation * transform.forward;
//         Handles.color = new Color(1f, 1f, 1f, 0.2f);
//         Handles.DrawSolidArc(kingsEye.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
//     }

// #endif
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }
