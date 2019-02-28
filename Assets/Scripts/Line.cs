using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    public GameObject FirstSphere { get; set; }
    public GameObject SecondSphere { get; set; }

    private EdgeCollider2D edgeCollider;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }
    

    void Start()
    {
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = new Color(0, 40, 235);
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;

        edgeCollider.isTrigger = true;

        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        edgeCollider.edgeRadius = .1f;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, FirstSphere.transform.position);
        lineRenderer.SetPosition(1, SecondSphere.transform.position);

        Vector2[] tempArray = edgeCollider.points;


        tempArray[0].x = lineRenderer.GetPosition(0).x;
        tempArray[0].y = lineRenderer.GetPosition(0).y;
        tempArray[1].x = lineRenderer.GetPosition(1).x;
        tempArray[1].y = lineRenderer.GetPosition(1).y;

        //tempArray[0].x = FirstSphere.transform.position.x;
        //tempArray[0].y = FirstSphere.transform.position.y;
        //tempArray[1].x = SecondSphere.transform.position.x;
        //tempArray[1].y = SecondSphere.transform.position.y;

        edgeCollider.points = tempArray;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cut" && gameObject != null)
        {
            Debug.Log("hit line");
            GameObject.Find("Initialization").GetComponent<GameInit>().lines.Remove(
                GameObject.Find("Initialization").GetComponent<GameInit>().lines.Find(_ => _ == gameObject));

            FirstSphere.GetComponent<Sphere>().IsTied = false;
            SecondSphere.GetComponent<Sphere>().IsTied = false;


            Destroy(gameObject);
            
        }
    }

}
