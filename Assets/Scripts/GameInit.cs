using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour {

    public GameObject sphere;
    public List<GameObject> sphere1;

    void Start () {
        StartCoroutine(InitGameplay());
    }

    void Update()
    {

    }

    IEnumerator InitGameplay()
    {
        
        for (int i = 0; i < 10; i++)
        {            
            sphere1.Add(Instantiate(sphere, SampleCandidate(), new Quaternion()));
            sphere1[i].name = "Sphere " + i.ToString();
            yield return new WaitForSeconds(1f);
        }
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
            candidate.Set(Random.Range(-2.3f, 2.3f), Random.Range(-4.3f, 4.3f), 0);
            distance = Vector3.Distance(candidate, FindClosest(candidate));
            if (distance > bestDistance)
            {
                bestCandidate.Set(candidate.x, candidate.y, 0);
                bestDistance = distance;
            }
        }
        return bestCandidate;
    }

    private Vector3 FindClosest(Vector3 sample)
    {
        float bestDistance = 10f;
        Vector3 closest = new Vector3();
        for (byte i = 0; i < sphere1.Count; i++)
        {
            if (Vector3.Distance(sample, sphere1[i].transform.position) < bestDistance)
            {     
                bestDistance = Vector3.Distance(sample, sphere1[i].transform.position);
                closest = sphere1[i].transform.position;
            }
        }
        return closest;
    }


    // Организация взаимодействия между скриптами https://habrahabr.ru/post/212055/
    // блюр который сейчас используется https://www.youtube.com/watch?v=YKTjVACAfqE

    //boolean npotSupported = Gdx.graphics.supportsExtension("GL_OES_texture_npot")
    //		|| Gdx.graphics.supportsExtension("GL_ARB_texture_non_power_of_two"); // chek support of NPOT images																	//создание сфер
    //NOTE use 1024x1024 textures or lesser.

    // swipe https://github.com/mattdesl/lwjgl-basics/wiki/LibGDX-Finger-Swipe
    // https://vk.com/gdevs  ПАБЛИК ПО ГЕЙМДЕВУ.

}
