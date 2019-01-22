using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDrag : MonoBehaviour {
    
    
    private Vector3 MouseStart;
    private Vector3 derp;
    public float speed = .0001f;
	public Camera mapCam;
    private float normFactor;
    public float mapQuarterSize;
    public RawImage viewport;
    private float viewportAR;


    void Start()
    {
        normFactor = Screen.width / 3840f; //normfactor reference is against 4k instead of 1920 for viewport calculations. 2f factors implemented for reversibility. 21 Jan 19 Z
        mapCam.orthographicSize = 2f * mapCam.orthographicSize * normFactor;
        mapCam.transform.position = transform.position;
        viewportAR = viewport.rectTransform.rect.width / viewport.rectTransform.rect.height; 
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
        if (mapCam.orthographicSize > 5 * normFactor) mapCam.orthographicSize = 5.0f * normFactor;
        if (mapCam.orthographicSize < 0.2f * normFactor) mapCam.orthographicSize = 0.2f * normFactor;
    }
		
	void LateUpdate()
	{
        mapCam.transform.position = new Vector3(Mathf.Clamp(mapCam.transform.position.x, transform.position.x-mapQuarterSize+(viewportAR*mapCam.orthographicSize/2f), transform.position.x+mapQuarterSize- (viewportAR*mapCam.orthographicSize / 2f)),
                                                Mathf.Clamp(mapCam.transform.position.y, transform.position.y-mapQuarterSize+ (mapCam.orthographicSize / 2f), transform.position.y+mapQuarterSize- (mapCam.orthographicSize / 2f)),
                                                Mathf.Clamp(mapCam.transform.position.z, mapCam.transform.position.z, mapCam.transform.position.z));
        speed = (float)(mapCam.orthographicSize/(normFactor*735*2f));
    }
}
