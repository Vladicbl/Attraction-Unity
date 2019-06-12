using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer { get; set; }
    public GameObject FirstSphere { get; set; }
    public GameObject SecondSphere { get; set; }

    private EdgeCollider2D edgeCollider;

    private GameInit gameInit;

    private bool createAnimationFinished = false;

    public void startInitAnimation(Sphere firstSphere, Sphere secondSphere)
    {
        StartCoroutine(initAnimation(firstSphere, secondSphere));
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.enabled = false;


        gameInit = GameObject.Find("Initialization").GetComponent<GameInit>();
    }

    private void Start()
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
        if (createAnimationFinished)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Cut" && gameObject != null)
        {

            GameObject waterSplash = Instantiate(gameInit.waterSplashPrefab, collision.transform.position, new Quaternion());
            Destroy(waterSplash, 1f);
            
            Destroy(gameObject);

            GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount += .1f;

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
                    GameObject sphereDeathEffect = Instantiate(gameInit.sphereDeathEffectPrefab, FirstSphere.transform.position, new Quaternion());
                    Destroy(sphereDeathEffect, 1f);
                    FirstSphere.GetComponent<Sphere>().DestroySphere();
                    //Destroy(FirstSphere);

                    gameInit.spheres.Remove(
                        gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
                    gameInit.NumberOfSpheres -= 1;
                    GameObject sphereDeathEffect_1 = Instantiate(gameInit.sphereDeathEffectPrefab, SecondSphere.transform.position, new Quaternion());
                    Destroy(sphereDeathEffect_1, 1f);
                    SecondSphere.GetComponent<Sphere>().DestroySphere();
                    //Destroy(SecondSphere);
                }
                else if (SecondSphere != null)
                {
                    gameInit.spheres.Remove(
                        gameInit.spheres.Find(_ => _ == SecondSphere.gameObject));
                    gameInit.NumberOfSpheres -= 1;
                    GameObject sphereDeathEffect = Instantiate(gameInit.sphereDeathEffectPrefab, SecondSphere.transform.position, new Quaternion());
                    Destroy(sphereDeathEffect, 1f);
                    SecondSphere.GetComponent<Sphere>().DestroySphere();
                    //Destroy(SecondSphere);
                }
            }
            
            

        }
    }

    private IEnumerator initAnimation(Sphere firstSphere, Sphere secondSphere)
    {
        float counter = 0;
        float lineDrawSpeed = .8f;
        float distance = Vector3.Distance(firstSphere.transform.position, secondSphere.transform.position);

        Vector3 pointA = firstSphere.transform.position;
        Vector3 pointB = secondSphere.transform.position;
        Vector3 pointAlongLine = firstSphere.transform.position;
        
        firstSphere.IsTied = true;
        secondSphere.IsTied = true;
        
        while (pointAlongLine != secondSphere.transform.position)
        {
            if (counter < distance)
            {
                counter += .1f / lineDrawSpeed;

                float x = Mathf.Lerp(0, distance, counter);

                pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                lineRenderer.SetPosition(1, pointAlongLine);
            }

            yield return new WaitForSeconds(0f);

        }
        // check null
        firstSphere.TiedWith = secondSphere.gameObject;
        firstSphere.Line = gameObject;
        
        secondSphere.TiedWith = firstSphere.gameObject;
        secondSphere.Line = gameObject;

        createAnimationFinished = true;
        edgeCollider.enabled = true;
        

        yield return null;
    }
}
