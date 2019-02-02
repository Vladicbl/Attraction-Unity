using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class GameInit : MonoBehaviour {

    public GameObject spherePrefab;
    public GameObject linePrefab;
    public List<GameObject> spheres;
    public List<GameObject> lines;

    private readonly float HORIZONTAL_BORDER_LEFT = -2.3f;
    private readonly float HORIZONTAL_BORDER_RIGHT = 2.3f;
    private readonly float VERTICAL_BORDER_LEFT = -4.3f;
    private readonly float VERTICAL_BORDER_RIGHT = 4.3f;

    private readonly byte maxNumOfSpheres = 7;

    private AudioSource audioSource;

    public byte NumberOfSpheres { get; set; }

    private byte currentSphereIndex = 0;
    private byte _numberOfUntiedSpheres = 0;



    void Start() {
        NumberOfSpheres = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartCoroutine(InitGameplay());
        StartCoroutine(CountUntiedSpheres());
        StartCoroutine(TiedSpheres());
        // AsyncOperation asyncOperation = Application.LoadLevelAsync(); https://www.youtube.com/watch?v=BMWzxdrq8uc
    }

    IEnumerator CountUntiedSpheres()
    {
        while (true)
        {
            byte numberOfUntiedSpheres = 0;
            for (int i = 0; i < spheres.Count; i++)
            {
                if (!spheres[i].GetComponent<Sphere>().IsTied)
                {
                    numberOfUntiedSpheres++;
                }
            }
            _numberOfUntiedSpheres = numberOfUntiedSpheres;
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator InitGameplay()
    {
        while (true)
        {
            if (NumberOfSpheres < maxNumOfSpheres)
            {
                CreateSphere();
                NumberOfSpheres++;
                string info = "sphere " + NumberOfSpheres.ToString();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator TiedSpheres()
    {
        while (true)
        {
            if (spheres.Count >= 2 && _numberOfUntiedSpheres >= 2)
            {
                for (int i = 0; i < spheres.Count; i++)
                {
                    for (int j = i; j < spheres.Count; j++)
                    {
                        if (LineCanBeCreated(spheres[i].GetComponent<Sphere>(), spheres[j].GetComponent<Sphere>()) && spheres[i] != spheres[j])
                        {
                            CreateLineNew(spheres[i].GetComponent<Sphere>(), spheres[j].GetComponent<Sphere>());
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private bool LineCanBeCreated(Sphere firstSphere, Sphere secondSphere)
    {
        if (firstSphere.IsTied == false && secondSphere.IsTied == false)
        {
            return true;
        }
        return false;
    }

    private void CreateSphere()
    {
        GameObject gameObject = Instantiate(spherePrefab, SampleCandidate(), new Quaternion());
        gameObject.name = "Sphere" + currentSphereIndex.ToString();
        gameObject.tag = "Sphere"; //  ??? prefab has this tag already
        currentSphereIndex++;
        spheres.Add(gameObject);

        if (currentSphereIndex == 9)
            currentSphereIndex = 0;
    }

    private void CreateLine(int startSphere) // ??? чек только тех сфер которые антайд
    {
        //Debug.Log(Intersect(new Vector3(1,1,0), new Vector3(6, 6, 0), new Vector3(1, 1, 0), new Vector3(6, 6, 0)));
        
        for (int i = 1; i < spheres.Count; i++)
        {
            //&& !CheckForIntersect(spheres[startSphere].transform, spheres[i].transform)
            if (!spheres[i].GetComponent<Sphere>().IsTied )
            {

                GameObject gameObject = Instantiate(linePrefab, spheres[startSphere].GetComponent<Transform>().position, new Quaternion());
                gameObject.name = "Line " + lines.Count;
                lines.Add(gameObject);
                lines[lines.Count - 1].GetComponent<Line>().lineRenderer.SetPosition(0, spheres[startSphere].GetComponent<Transform>().position); // index of a sp!
                lines[lines.Count - 1].GetComponent<Line>().tiedWith.Add(spheres[startSphere]);
                spheres[startSphere].GetComponent<Sphere>().IsTied = true;
                

                spheres[i].GetComponent<Sphere>().TiedWith = spheres[startSphere];
                spheres[startSphere].GetComponent<Sphere>().TiedWith = spheres[i];
                lines[lines.Count - 1].GetComponent<Line>().lineRenderer.SetPosition(1, spheres[i].GetComponent<Transform>().position);
                lines[lines.Count - 1].GetComponent<Line>().tiedWith.Add(spheres[i]);
                spheres[i].GetComponent<Sphere>().IsTied = true;
                break;
            }
        }
    }

    private void CreateLineNew(Sphere firstSphere, Sphere secondSphere)
    {
        GameObject gameObject = Instantiate(linePrefab, firstSphere.GetComponent<Transform>().position, new Quaternion());
        gameObject.name = "Line " + lines.Count;

        gameObject.GetComponent<Line>().lineRenderer.SetPosition(0, firstSphere.GetComponent<Transform>().position);
        firstSphere.GetComponent<Sphere>().IsTied = true;
        firstSphere.GetComponent<Sphere>().TiedWith = secondSphere.gameObject;

        gameObject.GetComponent<Line>().lineRenderer.SetPosition(1, secondSphere.GetComponent<Transform>().position);
        secondSphere.GetComponent<Sphere>().IsTied = true;
        secondSphere.GetComponent<Sphere>().TiedWith = firstSphere.gameObject;

        lines.Add(gameObject);
        Debug.Log("CRLINE");
    }







    private bool CheckForIntersect(Transform firstSphere, Transform secondSphere)
    {
        bool result = false;
        if (lines.Count > 1)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                result = Intersect(firstSphere.position, secondSphere.position,
                    lines[i].GetComponent<Line>().tiedWith[0].transform.position, lines[i].GetComponent<Line>().tiedWith[1].transform.position);
                if (result == true)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    //struct pt http://e-maxx.ru/algo/segments_intersection_checking
    //{
    //    int x, y;
    //};

    private float Area(Vector3 firstPoint, Vector3 secondPoint, Vector3 thirdPoint)
    {
        return (secondPoint.x - firstPoint.x) * (thirdPoint.y - firstPoint.y) - 
            (secondPoint.y - firstPoint.y) * (thirdPoint.x - firstPoint.x);
    }

    private bool Intersect_1(float a, float b, float c, float d)
    {
        //a = a + b;
        //b = a - b;
        //a = a - b;
        if (a > b) b = a - (a = b); //swap(a, b);
        if (c > d) c = d - (d = c); //swap(c, d);
        return Mathf.Max(a, c) <= Mathf.Min(b, d);
    }
    
    private bool Intersect(Vector3 firstPoint, Vector3 secondPoint, Vector3 thirdPoint, Vector3 fourthPoint)
    {
        return Intersect_1(firstPoint.x, secondPoint.x, thirdPoint.x, fourthPoint.x)
            && Intersect_1(firstPoint.y, secondPoint.y, thirdPoint.y, fourthPoint.y)
            && Area(firstPoint, secondPoint, thirdPoint) * Area(firstPoint, secondPoint, fourthPoint) <= 0
            && Area(thirdPoint, fourthPoint, firstPoint) * Area(thirdPoint, fourthPoint, secondPoint) <= 0;
    }

    private Vector3 SampleCandidate()
    {
        byte numCandidates = 20;

        float bestDistance = 0f;
        float distance;

        Vector3 candidate = new Vector3();
        Vector3 bestCandidate = new Vector3();

        for (byte i = 0; i < numCandidates; i++)
        {
            candidate.Set(Random.Range(HORIZONTAL_BORDER_LEFT, HORIZONTAL_BORDER_RIGHT), Random.Range(VERTICAL_BORDER_LEFT, VERTICAL_BORDER_RIGHT), 0);
            distance = Vector3.Distance(candidate, FindClosest(ref candidate));

            if (distance > bestDistance)
            {
                bestCandidate.Set(candidate.x, candidate.y, 0);
                bestDistance = distance;
            }
        }

        return bestCandidate;
    }

    private Vector3 FindClosest(ref Vector3 sample)
    {
        float bestDistance = 10f;

        Vector3 closest = new Vector3();

        for (byte i = 0; i < spheres.Count; i++)
        {

            if (Vector3.Distance(sample, spheres[i].transform.position) < bestDistance)
            {
                bestDistance = Vector3.Distance(sample, spheres[i].transform.position);
                closest = spheres[i].transform.position;
            }
        }

        return closest;
    }



    //return true;
    //}
    //[Header("Name")] разделение переменных
    //[Space]
    //[Header("Name")]

    // Организация взаимодействия между скриптами https://habrahabr.ru/post/212055/
    // блюр который сейчас используется https://www.youtube.com/watch?v=YKTjVACAfqE



    // swipe https://github.com/mattdesl/lwjgl-basics/wiki/LibGDX-Finger-Swipe
    // https://vk.com/gdevs  ПАБЛИК ПО ГЕЙМДЕВУ.

    //https://unity3d.com/ru/how-to/highlights-from-end-to-end-2D-toolset

    //void Update()
    //{
    //    float distance = speed * Time.deltaTime * Input.GetAxis("Horizontal");
    //    transform.Translate(Vector3.right * distance);
    //}


    //void Update()
    //{
    //    transform.Translate(0, 0, distancePerSecond * Time.deltaTime); перемещение объекта. умножать на дельтаТайм, чтобы при любом фпс все было ути-пути.
    //При применении расчётов передвижения внутри FixedUpdate, вам не нужно умножать ваши значения на Time.deltaTime.
    //Потому что FixedUpdate вызывается в соответствии с надёжным таймером, независящим от частоты кадров.
    //    https://docs.unity3d.com/ru/current/Manual/TimeFrameManagement.html
    //}

    //Основное правило заключается в том, чтобы не было ссылок на скрипты, компилирующиеся в фазе после.
    //Все, что компилируется в текущей или ранее выполненной фазе, должно быть полностью доступно.


    // 50 советов https://freedevgame.ru/prog/50-sovetov-po-rabote-s-unity.html
}
