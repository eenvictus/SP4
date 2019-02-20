﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking;

public class enemy_spawner : EnemySpawnerBehavior {
    
	public GameObject[] enemyPrefab;

	public static List<GameObject> enemyList;
    public static float spawnTimer;

    int waveCount;

    // Use this for initialization
    void Start ()
    {
        spawnTimer = 0.0f;
		enemyList = new List<GameObject>();
        Random.InitState((int)System.DateTime.Now.Ticks);

        waveCount = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (enemyList.Count == 0)
        {
            //TO DO: SHOW TIME LEFT TILL NEXT WAVE
            spawnTimer += Time.deltaTime;
        }

        //TO DO: SHOW ENEMIES LEFT
        if (spawnTimer >= 5)
        {
            if (NetworkManager.Instance.IsServer)
            {
                for (int i = 0; i <= (int)SpawnerCalc(waveCount, 3, 37, 40); ++i)
                {
                    //Rigidbody newEnemy;
                    Vector3 randPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
                    networkObject.SendRpc(RPC_START_INSTANTIATE, Receivers.All, randPos);
                    
                    spawnTimer = 0.0f;
                }
            }

            ++waveCount;
        }
    }

    float SpawnerCalc(float currentWave, float startMobCount, float MaxMinDiff, float MaxWave)
    {
        if ((currentWave /= MaxWave / 2) < 1)
            return MaxMinDiff / 2 * currentWave * currentWave * currentWave + startMobCount;

        return MaxMinDiff / 2 * ((currentWave -= 2) * currentWave * currentWave + 2) + startMobCount;
    }

    public override void StartInstantiate(RpcArgs args)
    {
        var newEnemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], args.GetNext<Vector3>(), transform.rotation);
        //var newEnemy = NetworkManager.Instance.InstantiateEnemy(Random.Range(0, enemyPrefab.Length), args.GetNext<Vector3>(), transform.rotation);

        enemyList.Add(newEnemy.gameObject);
    }

    public override void DestroyEnemy(RpcArgs args)
    {
        int count = args.GetNext<int>();

        if (enemyList[count] != null)
        {
            Destroy(enemyList[count].gameObject);
        }
    }

    public void DestroyEnemy(int _index)
    {
        networkObject.SendRpc(RPC_DESTROY_ENEMY, Receivers.All, _index);
    }

    public override void EnemyOnFire(RpcArgs args)
    {
        int count = args.GetNext<int>();
        bool status = args.GetNext<bool>();

        if (enemyList[count] != null)
        {
            enemyList[count].gameObject.GetComponent<EnemyBase>().m_burning = status;
        }
    }

    public void EnemyOnFire(int _index, bool _status)
    {
        networkObject.SendRpc(RPC_DESTROY_ENEMY, Receivers.All, _index, _status);
    }
}
