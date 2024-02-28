using UnityEngine;

public class throwrope : MonoBehaviour {
    public GameObject hook;
     GameObject currenthook;
    public GameObject cursor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

            Vector2 destiny = cursor.transform.position;
           currenthook =cursor;
            currenthook.GetComponent<ropescript>().destiny = destiny;


     
        
	}
}
