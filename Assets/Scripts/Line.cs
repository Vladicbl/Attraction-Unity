using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    public GameObject FirstSphere { get; set; }
    public GameObject SecondSphere { get; set; }
       

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, FirstSphere.transform.position);
        lineRenderer.SetPosition(1, SecondSphere.transform.position);
    }

    void Start()
    {
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = new Color(0, 40, 235);
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        Debug.Log(gameObject.transform.GetChild(0).GetType());
        //transform.rotation = Quaternion.identity;
        
        Transform Child = gameObject.transform.GetChild(0);
        Child.rotation = Quaternion.identity;
        //Child.rotation = transform.rotation;
        //Child.GetComponent<BoxCollider2D>().offset = new Vector2(2,4);
        //Child.GetComponent<BoxCollider2D>().size = new Vector2(2,4);
        //Child.localPosition = new Vector3(0, 0, 0);
        //Quaternion.
        //Child.localRotation = new Quaternion(0, 0, 40, 0);
            //gameObject.transform.rotation;
        //gameObject.transform.GetChild(0).localPosition = new Vector3(0,0,0);
        //gameObject.transform.
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
