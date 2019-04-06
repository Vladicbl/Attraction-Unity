using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Sphere : MonoBehaviour
    {
        //private enum Skin { Fire, Water, Ground };

        public bool IsTied { get; set; }
        public GameObject TiedWith { get; set; }
        public GameObject Line { get; set; }
        
        private AudioSource audioSource;
        private ParticleSystem particles;

        private GameInit gameInit;

        void Start()
        {
            gameInit = GameObject.Find("Initialization").GetComponent<GameInit>();

            particles = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();

            IsTied = false;
            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (true)
            {
                yield return new WaitForSeconds(0f);
                if (IsTied && gameObject != null && TiedWith != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, TiedWith.transform.position, Time.deltaTime * 2);


                    if (Vector3.Distance(transform.position, TiedWith.transform.position) < 1)
                    {
                        //GameObject.Find("Initialization").GetComponent<GameInit>().NumberOfSpheres -= 2;

                        //if (gameObject != null)
                        //{
                        //    Destroy(gameObject);
                        //}

                        //if (TiedWith != null)
                        //{
                        //    Destroy(TiedWith);
                        //}

                        //if (Line != null)
                        //{
                        //    Destroy(Line);
                        //}
                    }

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
                //gameObject.GetComponent<Animator>().SetTrigger("SphereDeath");
                gameObject.GetComponent<Animator>().SetBool("IsTouched" , true);
                //gameObject.GetComponent<Animator>().;


                Debug.Log("hit sphere");
                if (gameObject != null)
                {
                    
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount -= .3f;

                    if (GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        GameOver();
                        StopCoroutine("Move");
                        gameInit.gameOverUI.SetActive(true);
                    }

                    gameInit.spheres.Remove(gameInit.spheres.Find(_ => _ == gameObject));
                    gameInit.NumberOfSpheres -= 1;

                    Destroy(gameObject);
                    if (IsTied)
                    {
                        Destroy(Line);
                        gameInit.lines.Remove(gameInit.lines.Find(_ => _ == gameObject));

                        TiedWith.GetComponent<Sphere>().IsTied = false;
                    }

                    

                }
            }

            if (collision.tag == "Sphere")
            {
                if (gameObject != null)
                {
                    //audioSource.Play();

                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount -= .2f;

                    if (GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        Debug.Log("Game Over");

                        gameInit.gameOverUI.SetActive(true);
                    }

                    gameInit.spheres.Remove(gameInit.spheres.Find(_ => _ == gameObject));
                    gameInit.NumberOfSpheres -= 1;

                    gameInit.lines.Remove(gameInit.lines.Find(_ => _ == gameObject));
                    
                    Destroy(gameObject);
                    Destroy(TiedWith);
                    Destroy(Line);

                    
                }
            }
        }

    }
}
