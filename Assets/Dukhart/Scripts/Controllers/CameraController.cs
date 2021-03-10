using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,10,-6);
    [SerializeField] Vector2 speed = new Vector2(12,5);
    [SerializeField] Vector3 rotation = new Vector3(60,0,0);
    private void LateUpdate()
    {
        UpdateCamera();
    }

    // moves the player camera
    void UpdateCamera()
    {
        // calculate camera position
        Vector3 posCurrent = transform.position;
        Vector3 posTarget = player.transform.position + cameraOffset;
        float verticalMovement = Mathf.SmoothStep(posCurrent.y, posTarget.y, Time.deltaTime * speed.y);
        Vector2 horizontalMovement = Vector2.Lerp(new Vector2(posCurrent.x, posCurrent.z), new Vector2(posTarget.x, posTarget.z), Time.deltaTime * speed.x);
        Vector3 pos = new Vector3(horizontalMovement.x, verticalMovement, horizontalMovement.y);
        // set the camera position
        transform.SetPositionAndRotation(pos, Quaternion.Euler(rotation));
    }
}
