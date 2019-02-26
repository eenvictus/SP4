﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotation : MonoBehaviour
{
    Vector3 directionVector;
    Quaternion targetRotation;
	
	// Update is called once per frame
	void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, this.transform.position);
        float distToPlane;

        if (plane.Raycast(ray, out distToPlane))
        {
            Vector3 hitPos = ray.GetPoint(distToPlane);
            Vector3 dir = hitPos - transform.position;
            dir.y = 0;
            directionVector = hitPos - transform.position;
            targetRotation = Quaternion.LookRotation(directionVector);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1);
        }
    }
}