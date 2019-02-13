﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour {

	// Reference to object pooler. (Because this will be called at a high rate
	ObjectPooler objectPooler;
	public float fireRate = 0.2f;
	public GameObject flash;
    public AudioClip GunShot;
    public AudioSource GunShotSource;


    private float m_countDown = 0.0f;

	private void Start()
	{
		objectPooler = ObjectPooler.Instance;
        GunShotSource.clip = GunShot;
    }

    void Update() {
		m_countDown -= Time.deltaTime;

		if (Input.GetMouseButton (0) && m_countDown <= 0.0f)
        {
            m_countDown += fireRate;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(ray, out hit))
            {
				Vector3 dir = hit.point - transform.position;
				dir.y = 0;
                objectPooler.SpawnFromPool("HMG_Bullet", transform.position, Quaternion.LookRotation(dir));
                
                // Play thegunfire light
                flash.GetComponent<HMG_Flash>().StartLight();
                GunShotSource.volume = SFX.SFXvolchanger.audioSrc.volume;
                GunShotSource.Play();
            }
		}
		// More accurate firerate if more than 1 shot is fired in a row
		else if (m_countDown <= 0.0f) 
		{
			m_countDown = fireRate;
		}
	}

}