using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSmalltol : MonoBehaviour
{
    public GameObject player;

    Vector3 PlayerPos;

    float x_min;    //생성 x좌표 최소
    float x_max;    //생성 x좌표 최대

    float height;   //생성 높이''

    float t;     //타이머

    // Start is called before the first frame update
    void Start()
    {
        //초기생성위치 = 플레이어 위치 로 설정되있음

        x_min = transform.position.x - 12.5f; //플레이어 +-12.5 거리
        x_max = transform.position.x + 12.5f;
        height = 30;

        float x_rand = Random.Range(x_min, x_max);  //생성지점 결정
        transform.position = new Vector3(x_rand, transform.position.y+height, transform.position.z);

        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;   //현재 위치
        pos.y -= t;

        transform.position = pos;

        t += 0.01f;
    }
}
