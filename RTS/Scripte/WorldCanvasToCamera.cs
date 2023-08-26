using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasToCamera : MonoBehaviour
{
    public Transform player;
    Canvas canvas;
    public Camera cam;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = cam;
    }

    private void Update()
    {
        if (canvas.worldCamera == null) canvas.worldCamera = cam;

        Vector3 lookDirection = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;
    }

}
