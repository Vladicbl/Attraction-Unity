﻿using System;
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

        void Start()
        {
            particles = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
            

            //Debug.Log(this.gameObject.transform.position);
            IsTied = false;
            StartCoroutine(Move());
            //gameObject = GameObject.FindGameObjectWithTag("Sphere");
            //if (gameObject.tag == "Sphere")
            //{
            //    Debug.Log(gameObject);
            //    Destroy(gameObject);
            //}
        }

        private void Update()
        {
            //gameObjects = GameObject.FindGameObjectsWithTag("Sphere");

        }

        IEnumerator Move()
        {
            while (true) // массив сфер, чек связана ли , чек пересечение линии, движение
            {
                yield return new WaitForSeconds(0f);
                if (IsTied && gameObject != null)
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
            GameObject.Find("Initialization").GetComponent<GameInit>().GameOver();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Cut")
            {
                Debug.Log("hit sphere");
                if (gameObject != null)
                {
                    
                    GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount -= .3f;

                    if (GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetComponent<Image>().fillAmount == 0)
                    {
                        Debug.Log("Game Over");
                        GameOver();
                        StopCoroutine("Move");
                        GameObject.Find("Initialization").GetComponent<GameInit>().gameOverUI.SetActive(true);
                    }

                    GameObject.Find("Initialization").GetComponent<GameInit>().spheres.Remove(
                        GameObject.Find("Initialization").GetComponent<GameInit>().spheres.Find(_ => _ == gameObject));
                    GameObject.Find("Initialization").GetComponent<GameInit>().NumberOfSpheres -= 1;

                    Destroy(gameObject);
                    if (IsTied)
                    {
                        Destroy(Line);
                        GameObject.Find("Initialization").GetComponent<GameInit>().lines.Remove(
                            GameObject.Find("Initialization").GetComponent<GameInit>().lines.Find(_ => _ == gameObject));

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

                        GameObject.Find("Initialization").GetComponent<GameInit>().gameOverUI.SetActive(true);
                    }

                    GameObject.Find("Initialization").GetComponent<GameInit>().spheres.Remove(
                        GameObject.Find("Initialization").GetComponent<GameInit>().spheres.Find(_ => _ == gameObject));
                    GameObject.Find("Initialization").GetComponent<GameInit>().NumberOfSpheres -= 1;

                    GameObject.Find("Initialization").GetComponent<GameInit>().lines.Remove(
                            GameObject.Find("Initialization").GetComponent<GameInit>().lines.Find(_ => _ == gameObject));
                    
                    Destroy(gameObject);
                    Destroy(TiedWith);
                    Destroy(Line);

                    
                }
            }
        }

    }
}
