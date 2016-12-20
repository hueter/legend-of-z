using UnityEngine;
using System.Collections;

namespace RPG
{
    public class GanonLightningScript : MonoBehaviour
    {


        public Character Link;
        float startTime;

        // Use this for initialization
        void Start()
        {
            startTime = Time.time;
            Link = GameObject.Find("Player").GetComponent<Character>();
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time > startTime + .9f)
            {
                DestoryWall();
            }
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.SendMessage("TakeDamage", Link, SendMessageOptions.DontRequireReceiver);
            }
        }
        
        void DestoryWall()
        {
            Destroy(this.gameObject);
        }
    }
}