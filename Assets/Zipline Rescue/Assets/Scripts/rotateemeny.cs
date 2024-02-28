﻿using UnityEngine;

public class rotateemeny : MonoBehaviour {

    [SerializeField]
    Transform rotationcenter;
   // [SerializeField]
   public float rotationRadius = 2f, angularspeed = 2f;
    float posx, posy, angle = 0f;
	
	// Update is called once per frame
	void Update () {
        posx = rotationcenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posy = rotationcenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posx, posy);
        angle = angle + Time.deltaTime * angularspeed;
       
	}
}
