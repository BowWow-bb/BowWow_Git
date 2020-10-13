using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    public int maxCount;
    public int enemyCount;//현재 수
    public float spawnTime = 3f;
    public float curTime;

    public Transform[] spawnPoints;
    public bool[] isSpawn;
    public GameObject smalltoll_stage1;

    private void Start()
    {
        isSpawn = new bool[spawnPoints.Length];//스폰 개수만큼 bool 할당
        for(int i=0; i<isSpawn.Length;i++)
        {
            isSpawn[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount < maxCount)//한 번 생성 된 후 다시 생성 안됨 
        {
            int x = Random.Range(0, spawnPoints.Length);
            if(!isSpawn[x])
            {
                SpawnSmalltoll_stage1(x);
            }
        }
        curTime += Time.deltaTime;
    }
    public void SpawnSmalltoll_stage1(int x)
    {
        enemyCount++;
        Instantiate(smalltoll_stage1, spawnPoints[x]);
        isSpawn[x] = true;//같은 자리에 생성 안되게 
    }
}
