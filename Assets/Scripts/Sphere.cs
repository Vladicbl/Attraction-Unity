using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Sphere : MonoBehaviour
    {
        public bool IsTied { get; set; }
        public GameObject TiedWith { get; set; }
        public GameObject Line { get; set; }
        
        private AudioSource audioSource;

        private GameInit gameInit;

        void Start()
        {
            gameInit = GameObject.Find("Initialization").GetComponent<GameInit>();
            
            audioSource = GetComponent<AudioSource>();

            IsTied = false;
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.001f);
                if (IsTied && gameObject != null && TiedWith != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, TiedWith.transform.position, Time.deltaTime * 2);
                }                
            }
        }

        private void GameOver()
        {
            StopAllCoroutines();
            gameInit.GameOver();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Cut")
            {
                transform.GetComponent<CircleCollider2D>().enabled = false;
                audioSource.Play();
                GameObject waterSplash = Instantiate(gameInit.waterSplashPrefab, collision.transform.position, new Quaternion());
                Destroy(waterSplash, 1f);

                if (gameObject != null)
                {                    
                    GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount -= .3f;

                    if (GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount <= 0)
                    {
                        GameOver();
                        StopCoroutine(Move());
                        gameInit.gameOverUI.SetActive(true);
                    }

                    gameInit.spheres.Remove(gameInit.spheres.Find(_ => _ == gameObject));
                    gameInit.NumberOfSpheres -= 1;


                    if (IsTied)
                    {
                        StopCoroutine(Move());
                        Destroy(Line);
                        gameInit.lines.Remove(gameInit.lines.Find(_ => _ == gameObject));

                        TiedWith.GetComponent<Sphere>().IsTied = false;
                    }

                    gameObject.GetComponent<Animator>().SetBool("IsTouched", true);
                }
            }

            if (collision.tag == "Sphere")
            {
                if (gameObject != null)
                {
                    audioSource.Play();

                    GameObject sphereTouchEffect = Instantiate(gameInit.sphereDeathEffectPrefab, collision.transform.position, new Quaternion());
                    Destroy(sphereTouchEffect, 1f);
                    GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount -= .2f;

                    if (GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        GameOver();
                        StopCoroutine(Move());
                        gameInit.gameOverUI.SetActive(true);
                    }

                    gameInit.spheres.Remove(gameInit.spheres.Find(_ => _ == gameObject));
                    gameInit.NumberOfSpheres -= 1;

                    gameInit.lines.Remove(gameInit.lines.Find(_ => _ == gameObject));

                    DoubleSphereDestroy();
                    Destroy(Line);                    
                }
            }
        }

        public void DoubleSphereDestroy()
        {
            TiedWith.GetComponent<CircleCollider2D>().enabled = false;
            this.Line.GetComponent<EdgeCollider2D>().enabled = false;
            transform.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(DoubleSphereDeath());
        }

        public void DestroySphere()
        {
            StartCoroutine(SphereDeath());
        }

        private IEnumerator DoubleSphereDeath()
        {
            yield return new WaitForSeconds(.3f);
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
            if (TiedWith != null)
            {
                Destroy(TiedWith);
            }
            yield return null;
        }

        private IEnumerator SphereDeath()
        {

            //yield return new WaitWhile(() => audioSource.isPlaying);

            //gameObject.GetComponent<Animator>().cli
            //yield return new WaitForSeconds(.3f);
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
            yield return null;
        }

    }
}
