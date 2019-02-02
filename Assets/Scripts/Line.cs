using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    public List<GameObject> tiedWith;


    private PolygonCollider2D polygonCollider;

    private void Awake()
    {
        tiedWith = new List<GameObject>(2);
        lineRenderer = GetComponent<LineRenderer>();
        //polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = new Color(0, 40, 235);
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        //lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        //lineRenderer.SetPosition(1, new Vector3(0, 0, 0));


        //polygonCollider.points = new Vector2[2];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cut" && gameObject != null)
        {
            Debug.Log("hit line");
            GameObject.Find("Initialization").GetComponent<GameInit>().lines.Remove(
                GameObject.Find("Initialization").GetComponent<GameInit>().lines.Find(_ => _ == gameObject));
            Destroy(gameObject);
        }
    }

}
