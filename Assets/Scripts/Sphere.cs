using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Sphere : MonoBehaviour
    {
        private enum Skin { Fire, Water, Ground };

        public bool IsTied { get; set; }
        public GameObject TiedWith { get; set; }    
        
        void Start()
        {
            Debug.Log(this.gameObject.transform.position);
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
                yield return new WaitForSeconds(1f);
                if (IsTied)
                {
                    transform.position = Vector3.MoveTowards(transform.position, TiedWith.transform.position, Time.deltaTime * 10);
                }
                
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
    }
}
