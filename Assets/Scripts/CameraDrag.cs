using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDrag : MonoBehaviour {

    //ripped from DrOmega, StackOverflow
    //https://stackoverflow.com/questions/43702017/unity-script-to-drag-camera

    //public float Zdist;
    private Vector3 MouseStart;
    private Vector3 derp;
    public float speed = .0001f;
	public Camera mapCam;
    public RawImage mapscreen;
    private Rect viewport;
    float normFactor;

    void Start()
    {
        //Zdist = transform.position.z;  // Distance camera is above map
        normFactor = Screen.width / 1920f;
        mapCam.orthographicSize = mapCam.orthographicSize * normFactor;
        viewport.size = mapscreen.rectTransform.rect.size*normFactor;
        Vector2 unfuckedVect = new Vector2(transform.position.y, transform.position.x);
        viewport.position = normFactor*(unfuckedVect + mapscreen.rectTransform.anchoredPosition);
        
    }

    public void InitialMouse()
    {
            MouseStart = new Vector3(Input.mousePosition.x * speed, Input.mousePosition.y * speed, 0);
	}
	
	public void DragMouse()
        {
            var MouseMove = new Vector3(Input.mousePosition.x * speed, Input.mousePosition.y* speed, 0);
            mapCam.transform.position = mapCam.transform.position - (MouseMove - MouseStart);
			MouseStart = MouseMove;
		}

    public void OnScroll()
    {
        mapCam.orthographicSize = mapCam.orthographicSize - Input.GetAxis("Mouse ScrollWheel");
        if (mapCam.orthographicSize > 2.5 * normFactor) mapCam.orthographicSize = 2.5f * normFactor;
        if (mapCam.orthographicSize < 0.1f * normFactor) mapCam.orthographicSize = 0.1f * normFactor;
    }
		
	void LateUpdate()
	{
		
        //if (mapCam.transform.localPosition.x > 3 / mapCam.orthographicSize) mapCam.transform.localPosition = new Vector3((float)(3 / mapCam.orthographicSize), mapCam.transform.localPosition.y, 0);
        //if (mapCam.transform.localPosition.x < -3 / mapCam.orthographicSize) mapCam.transform.localPosition = new Vector3((float)(-3 / mapCam.orthographicSize), mapCam.transform.localPosition.y, 0);
        //if (mapCam.transform.localPosition.y > 3.9 / mapCam.orthographicSize) mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, (float)(3.9 / mapCam.orthographicSize), 0);
        //if (mapCam.transform.localPosition.y < -3.9 / mapCam.orthographicSize) mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, (float)(-3.9 / mapCam.orthographicSize), 0);
        speed = (float)(mapCam.orthographicSize/(normFactor*735));

        
		
        /* if (Input.GetMouseButtonDown(0))
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

        } */
    }
}
