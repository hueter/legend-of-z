using UnityEngine;
using System.Collections;

public class BoltScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void DestroyBolt()
    {
        GameObject parentObject = this.GetComponentInParent<GameObject>();
        Destroy(parentObject);
    }
}
