using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    private PolygonCollider2D polygonCollider;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = new Color(0, 40, 235);
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;


        polygonCollider.points = new Vector2[2];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cut")
        {
            Debug.Log("hit");
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }

}
