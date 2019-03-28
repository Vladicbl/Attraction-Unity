using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    public GameObject FirstSphere { get; set; }
    public GameObject SecondSphere { get; set; }

    public GameInit gameInit;

    private EdgeCollider2D edgeCollider;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        


    }
    

    void Start()
    {
        lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        lineRenderer.startColor = new Color(0, 40, 235);
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        
        //edgeCollider.isTrigger = true;

        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        edgeCollider.edgeRadius = .1f;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, FirstSphere.transform.position);
        lineRenderer.SetPosition(1, SecondSphere.transform.position);

        transform.position = Vector3.zero;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("TRIGGERED " + collision.collider.name);

        if (collision.collider.tag == "Cut" && gameObject != null)
        {
            GameInit gameInit = GameObject.Find("Initialization").GetComponent<GameInit>();

            Debug.Log("hit line");


            GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount += .1f;

            gameInit.lines.Remove(
                gameInit.lines.Find(_ => _ == gameObject));

            gameInit.Score += 5;

            FirstSphere.GetComponent<Sphere>().IsTied = false;
            SecondSphere.GetComponent<Sphere>().IsTied = false;

            byte disappearProbability = (byte)Random.Range(0, 9);
            byte sphereProbability = (byte)Random.Range(0, 2);

            if (disappearProbability <= 8)
            {


                if (sphereProbability == 0 && FirstSphere != null)
                {
                    gameInit.spheres.Remove(
                        gameInit.spheres.Find(_ => _ == FirstSphere.gameObject));
                    gameInit.NumberOfSpheres -= 1;
                    Destroy(FirstSphere);

                    gameInit.spheres.Remove(
                        gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
                    gameInit.NumberOfSpheres -= 1;
                    Destroy(SecondSphere);
                }
                else if (SecondSphere != null)
                {
                    gameInit.spheres.Remove(
                        gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
                    gameInit.NumberOfSpheres -= 1;
                    Destroy(SecondSphere);
                }
            }

            //collision.GetComponent<Collision2D>().contacts[0].point;
            //SliceLine(collision.);

            Destroy(gameObject);

        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("TRIGGERED " + collision.name);

    //    if (collision.tag == "Cut" && gameObject != null)
    //    {
    //        GameInit gameInit = GameObject.Find("Initialization").GetComponent<GameInit>();

    //        Debug.Log("hit line");
            

    //        GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount += .1f;

    //        gameInit.lines.Remove(
    //            gameInit.lines.Find(_ => _ == gameObject));
            
    //        gameInit.Score += 5;

    //        FirstSphere.GetComponent<Sphere>().IsTied = false;
    //        SecondSphere.GetComponent<Sphere>().IsTied = false;

    //        byte disappearProbability = (byte) Random.Range(0, 9);
    //        byte sphereProbability = (byte) Random.Range(0, 2);

    //        if (disappearProbability <= 8) 
    //        {
                

    //            if (sphereProbability == 0 && FirstSphere != null)
    //            {
    //                gameInit.spheres.Remove(
    //                    gameInit.spheres.Find(_ => _ == FirstSphere.gameObject));
    //                gameInit.NumberOfSpheres -= 1;
    //                Destroy(FirstSphere);

    //                gameInit.spheres.Remove(
    //                    gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
    //                gameInit.NumberOfSpheres -= 1;
    //                Destroy(SecondSphere);
    //            }
    //            else if (SecondSphere != null)
    //            {
    //                gameInit.spheres.Remove(
    //                    gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
    //                gameInit.NumberOfSpheres -= 1;
    //                Destroy(SecondSphere);
    //            }
    //        }

    //        //collision.GetComponent<Collision2D>().contacts[0].point;
    //        //SliceLine(collision.);

    //        Destroy(gameObject);

    //    }
    //}

    private void SliceLine()
    {

    }

}
