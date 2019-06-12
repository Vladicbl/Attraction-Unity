using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                //Destroy(hit.transform.GetComponent<GameObject>());
                Destroy(hit.transform.gameObject);
            } 
        }
    }
}
