using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class GameInit : MonoBehaviour {

    public GameObject spherePrefab;
    public GameObject linePrefab;
    public List<GameObject> spheres;
    public List<GameObject> lines;
    public Sprite sprite;
    
    private readonly float HORIZONTAL_BORDER_LEFT = -2.3f;
    private readonly float HORIZONTAL_BORDER_RIGHT = 2.3f;
    private readonly float VERTICAL_BORDER_LEFT = -4.3f;
    private readonly float VERTICAL_BORDER_RIGHT = 4.3f;

    private static readonly byte maxNumOfPoints = 5;
    private List<List<bool>> m;
    private bool[,] incidenceMatrix = new bool[maxNumOfPoints, maxNumOfPoints]; //5 сфер\

    private List<byte[]> LinesTied;



    private Sphere a;


    void Start() {
        StartCoroutine(InitGameplay());
        
    }

    void Update()
    {
        //CreateLine();
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x == 1)
        {
            Debug.Log("Pressed primary button.");
        }
        //Еще, как вариант, отдельный список, каждый элемент содержит: ссылку на сферу, список ссылок на связные сферы.
        //new WaitForSeconds(10f);
        //spheres.Remove(spheres[0].gameObject);
    }

    void MatrixFill()
    {

    }

    IEnumerator InitGameplay()
    {
        while (true)
        {
            CreateSphere();
            yield return new WaitForSeconds(2f);
        }
    }

    private void CreateSphere()
    {
        GameObject gameObject = Instantiate(spherePrefab, SampleCandidate(), new Quaternion());
        gameObject.name = spheres.Count.ToString();
        spheres.Add(gameObject);
        Debug.Log(gameObject.GetComponent<Transform>().position);
    }

    private void CreateLine()
    {
        //GameObject gameObject = new GameObject();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        if (spheres.Count == 2)
        {
            lineRenderer.SetPosition(0, spheres[0].GetComponent<Transform>().position);
            lineRenderer.SetPosition(1, spheres[1].GetComponent<Transform>().position);
            //lines.Add(new GameObject());
            GameObject gameObject = Instantiate(linePrefab, SampleCandidate(), new Quaternion());
            gameObject.name = lines.Count.ToString();
            lines.Add(gameObject);
            Debug.Log(gameObject.GetComponent<Transform>().position);
        }
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
        return Mathf.Max(a, c) <= Mathf.Min(b, d); //max(a, c) <= min(b, d);
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
    //public GameObject pr;
    //public Texture texture;
    //pr.GetComponent<SpriteRenderer>().sprite = Sprite.Create((Texture2D)texture,new Rect(448,448,32,32), new Vector2(0,0),100f);

    //Button but = test.GetComponent<Button>();
    //but.OnClick.AddListener("вашМетод");

    //https://unity3d.com/ru/how-to/highlights-from-end-to-end-2D-toolset
    //Заметка для опытных программистов: вы можете быть удивлены, что инициализация объекта выполняется не в функции-конструкторе. Это потому, что создание объектов обрабатывается 
    //редактором и происходит не в начале игрового процесса, как вы могли бы ожидать. Если вы попытаетесь определить конструктор для скриптового компонента, он будет мешать 
    //нормальной работе Unity и может вызвать серьезные проблемы с проектом.

    //public class Enemy : MonoBehaviour
    //{
    //    public GameObject player;

    //    void Start()
    //    {
    //        // Start the enemy ten units behind the player character.
    //        transform.position = player.transform.position - Vector3.forward * 10f;
    //    }
    //}

    //Кроме того, если объявить переменную с доступом public 
    //и заданным типом компонента в вашем скрипте, вы сможете перетащить любой объект, который содержит
    //присоединенный компонент такого типа.Это позволит обращаться к компоненту напрямую, а не через игровой объект.
    //public Transform playerTransform;

    //void Update()
    //{
    //    float distance = speed * Time.deltaTime * Input.GetAxis("Horizontal");
    //    transform.Translate(Vector3.right * distance);
    //}



    //public float distancePerSecond;

    //void Update()
    //{
    //    transform.Translate(0, 0, distancePerSecond * Time.deltaTime); перемещение объекта. умножать на дельтаТайм, чтобы при любом фпс все было ути-пути.
    //При применении расчётов передвижения внутри FixedUpdate, вам не нужно умножать ваши значения на Time.deltaTime.
    //Потому что FixedUpdate вызывается в соответствии с надёжным таймером, независящим от частоты кадров.
    //    https://docs.unity3d.com/ru/current/Manual/TimeFrameManagement.html
    //}

    //Основное правило заключается в том, чтобы не было ссылок на скрипты, компилирующиеся в фазе после.
    //Все, что компилируется в текущей или ранее выполненной фазе, должно быть полностью доступно.
}
