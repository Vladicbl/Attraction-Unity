using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {

    public GameObject cutTrailPrefab;
    public float minCuttingVelocity = .001f;
    
    bool isCutting = false;

    Vector2 prevPosition;
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
        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);

        rb.position = newPosition;
        //transform.position = newPosition;
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
        Transform t = transform;
        t.position = cam.ScreenToWorldPoint(Input.mousePosition);
        currentCutTrail = Instantiate(cutTrailPrefab, t);
        prevPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        circleCollider.enabled = false;
    }

    void StopCutting()
    {
        isCutting = false;
        currentCutTrail.transform.SetParent(null);
        Destroy(currentCutTrail);
        circleCollider.enabled = false;
    }

    
}
