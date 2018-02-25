using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour {

    public GameObject sphere;
    public List<GameObject> sphere1;

    void Start () {
        StartCoroutine(Lol());
    }

    IEnumerator Lol()
    {
        float x, y;
        for (int i = 0; i < 5; i++)
        {
            x = Random.Range(-2.3f, 2.3f);
            y = Random.Range(-4.3f, 4.3f);
            sphere1.Add(Instantiate(sphere, new Vector3(x, y, 0), new Quaternion()));
            sphere1[i].name = "Sphere"/* + i.ToString*/;
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
//	Random a = new Random ();

//    random = new RandomXS128();
//    actors = new ArrayList<SphereActor>();
//	actors.add(new SphereActor(new Vector2((float) random.nextInt(11) + 10, (float) random.nextInt(11) + 10), 40));     //ПЕРВАЯ ТОЧКА
//	stage.addActor(actors.get(0));
//	for (byte i = 1; i< 10; i++) {
//		actors.add(new SphereActor(sampleCandidate(), 40));
//		stage.addActor(actors.get(i));
//	}
//}

//private Vector2 sampleCandidate() {  //МЕТОД ПОДБОРА КАНДИДАТОВ.
//	byte numCandidates = 100;
//	Vector2 candidate = new Vector2();
//	Vector2 bestCandidate = new Vector2();
//	float bestDistance = 0f;
//	float distance;
//	for (byte i = 0; i < numCandidates; i++) {
//		candidate.set((float) random.nextInt(WIDTH) + 10, (float) random.nextInt(HEIGHT) + 10);
//		distance = candidate.dst(findClosest(candidate));
//		if (distance > bestDistance) {
//			bestCandidate.set(candidate.x, candidate.y);
//			bestDistance = distance;
//		}
//	}
//	return bestCandidate;
//}

//private Vector2 findClosest(Vector2 sample) {    // МЕТОД НАХОЖДЕНИЯ БЛИЖАЙШЕГО КАНДИДАТА К ТЕКУЩЕМУ.
//	float bestDistance = 1000f;
//	Vector2 closest = new Vector2();
//	for (byte i = 0; i < actors.size(); i++) {
//		if (sample.dst(actors.get(i).getSphereVector()) < bestDistance) {
//			bestDistance = sample.dst(actors.get(i).getSphereVector());
//			closest = actors.get(i).getSphereVector();
//		}
//	}
//	return closest;
//}


// BLURE - https://github.com/mattdesl/lwjgl-basics/wiki/ShaderLesson5
//boolean npotSupported = Gdx.graphics.supportsExtension("GL_OES_texture_npot")
//		|| Gdx.graphics.supportsExtension("GL_ARB_texture_non_power_of_two"); // chek support of NPOT images																	//создание сфер
//NOTE use 1024x1024 textures or lesser.
//abs((x2-x1)*(x2-x1) + (y2-y1)*(y2-y1))  расстояние между 2 точками.
// swipe https://github.com/mattdesl/lwjgl-basics/wiki/LibGDX-Finger-Swipe
// signing your apk https://developer.android.com/studio/publish/app-signing.html
// https://vk.com/gdevs  ПАБЛИК ПО ГЕЙМДЕВУ.

}
