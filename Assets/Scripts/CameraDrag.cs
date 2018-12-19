using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    //ripped from DrOmega, StackOverflow
    //https://stackoverflow.com/questions/43702017/unity-script-to-drag-camera

    public float Zdist;
    private Vector3 MouseStart;
    private Vector3 derp;
    public float speed = .0001f;

    void Start()
    {
        Zdist = transform.position.z;  // Distance camera is above map
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseStart = new Vector3(Input.mousePosition.x * speed, Input.mousePosition.y * speed, Zdist);
            MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
            MouseStart.z = Zdist;

        }
        else if (Input.GetMouseButton(0))
        {
            var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zdist);
            MouseMove = Camera.main.ScreenToWorldPoint(MouseMove);
            MouseMove = new Vector3(-MouseMove.x * speed, -MouseMove.y * speed, MouseMove.z);
            MouseMove.z = Zdist;
            transform.position = (MouseMove - MouseStart);

        }
    }
}
