﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldClock : MonoBehaviour {

    public static float _worldTime;

	// Use this for initialization
	void Start () {
        _worldTime = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(_worldTime >= 24)
        {
            _worldTime = 0;
        }
        _worldTime += Time.deltaTime;
    }
}
