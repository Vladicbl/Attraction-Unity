using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class GameInit : MonoBehaviour {
    
    public GameObject spherePrefab;
    public GameObject linePrefab;
    public GameObject waterSplashPrefab;
    public GameObject sphereDeathEffectPrefab;
    public GameObject gameOverUI;
    public GameObject FinalScore;

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
    public int Score { get; set; }

    void Start() {
        Time.timeScale = .8f;
        NumberOfSpheres = 0;
        StartCoroutine(InitGameplay());
        StartCoroutine(CountUntiedSpheres());
        StartCoroutine(TiedSpheres());
    }

    private void Update()
    {
        GameObject.Find("Canvas").transform.Find("Score").GetComponent<TextMesh>().text = Score.ToString();
        
        if (NumberOfSpheres > 200)
        {
            NumberOfSpheres = 0;
        }
        
    }

    public void GameOver()
    {
        FinalScore.GetComponent<Text>().text = Score.ToString();
        StopAllCoroutines();
        gameOverUI.SetActive(true);
    }

    IEnumerator CountUntiedSpheres()
    {
        while (true)
        {
            byte numberOfUntiedSpheres = 0;
            for (int i = 0; i < spheres.Count; i++)
            {
                if (spheres[i] != null && !spheres[i].GetComponent<Sphere>().IsTied)
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
            }
            yield return new WaitForSeconds(.7f);
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
                        if (spheres[i] != null && spheres[j] != null)
                            if (CanCreateLine(spheres[i].GetComponent<Sphere>(), spheres[j].GetComponent<Sphere>()) && spheres[i] != spheres[j])
                            {
                                CreateLine(spheres[i].GetComponent<Sphere>(), spheres[j].GetComponent<Sphere>());
                            }
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private bool CanCreateLine(Sphere firstSphere, Sphere secondSphere)
    {
        if (firstSphere != null && secondSphere != null)
        {
            if (firstSphere.IsTied == false && secondSphere.IsTied == false && !CheckForIntersect(firstSphere.transform, secondSphere.transform))
            {
                return true;
            }
        }
        
        return false;
    }

    private void CreateSphere()
    {
        GameObject gameObject = Instantiate(spherePrefab, SampleCandidate(), new Quaternion());
        gameObject.name = "Sphere " + currentSphereIndex.ToString();
        currentSphereIndex++;
        spheres.Add(gameObject);

        if (currentSphereIndex == 9)
            currentSphereIndex = 0;
    }

    private void CreateLine(Sphere firstSphere, Sphere secondSphere)
    {
        GameObject line = Instantiate(linePrefab, firstSphere.GetComponent<Transform>().position, new Quaternion());        

        line.name = "Line " + lines.Count;

        line.GetComponent<Line>().lineRenderer.SetPosition(0, firstSphere.GetComponent<Transform>().position);
        line.GetComponent<Line>().FirstSphere = firstSphere.gameObject;

        line.GetComponent<Line>().lineRenderer.SetPosition(1, secondSphere.GetComponent<Transform>().position);
        line.GetComponent<Line>().SecondSphere = secondSphere.gameObject;

        line.GetComponent<Line>().startInitAnimation(firstSphere, secondSphere);

        lines.Add(line);
    }
    
    private bool CheckForIntersect(Transform firstSphere, Transform secondSphere)
    {
        bool result = false;
        if (lines.Count >= 1)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] != null)
                {
                    if (firstSphere && secondSphere && lines[i].GetComponent<Line>().FirstSphere && lines[i].GetComponent<Line>().SecondSphere)
                    {
                        result = Intersect(firstSphere.position, secondSphere.position,
                                                lines[i].GetComponent<Line>().FirstSphere.transform.position,
                                                lines[i].GetComponent<Line>().SecondSphere.transform.position);
                    }
                }
                
                if (result == true)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private float Area(Vector3 firstPoint, Vector3 secondPoint, Vector3 thirdPoint)
    {
        return (secondPoint.x - firstPoint.x) * (thirdPoint.y - firstPoint.y) - 
            (secondPoint.y - firstPoint.y) * (thirdPoint.x - firstPoint.x);
    }

    private bool Intersect_1(float a, float b, float c, float d)
    {
        if (a > b) b = a - (a = b);
        if (c > d) c = d - (d = c);
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
        byte numCandidates = 10;

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
            if (spheres[i] != null)
                if (Vector3.Distance(sample, spheres[i].transform.position) < bestDistance)
                {
                    bestDistance = Vector3.Distance(sample, spheres[i].transform.position);
                    closest = spheres[i].transform.position;
                }
        }

        return closest;
    }
}
