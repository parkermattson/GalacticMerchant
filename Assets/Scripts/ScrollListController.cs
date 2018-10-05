using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollListController : MonoBehaviour {

    [SerializeField]
    private GameObject boxTemplate;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 20; i++)
        {
            GameObject box = Instantiate(boxTemplate) as GameObject;
            box.SetActive(true);

            box.transform.SetParent(boxTemplate.transform.parent, false);
        }
	}
	
	
}
