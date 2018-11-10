using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    public GameObject cutTrailPrefab;
    public float minCuttingVelocity = .001f;
    
    bool isCutting = false;

    Vector3 prevPosition;
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    Camera cam;
    GameObject currentCutTrail;
    
	void Start () {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        } else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCut();
        }
	}

    void UpdateCut()
    {
        Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 1;
        rb.position = newPosition;
        float velocity = (newPosition - prevPosition).magnitude * Time.deltaTime;

        if (velocity > minCuttingVelocity)
        {
            circleCollider.enabled = true;
        }
        else
        {
            circleCollider.enabled = false;
        }

        prevPosition = newPosition;
    }

    void StartCutting()
    {
        isCutting = true;
        currentCutTrail = Instantiate(cutTrailPrefab, transform);
        prevPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        circleCollider.enabled = false;
    }

    void StopCutting()
    {
        isCutting = false;
        currentCutTrail.transform.SetParent(null);
        Destroy(currentCutTrail, 1f);
        circleCollider.enabled = false;
    }

    
}
