using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFireball : MonoBehaviour
{
    int power;      //공격력

    float t;        //타이머
    float angle;    //생성 각도

    //발사 위치
    float x;
    float y;

    int cnt;        //충돌 횟수

    Vector3 Pos;
    Vector3 now;

    // Start is called before the first frame update
    void Start()
    {
        power = 10;
        t = 0.85f;
        cnt = 1;
        Pos = transform.position;   //생성 초기위치

        setAngle(); //각도 설정

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Pos = transform.position;   //현재 위치
        transform.position += now.normalized * t;  //계속 발사 되게

        if (cnt >= 5)    //4번 이상 충돌할 경우 삭제
            Destroy(gameObject);

        //반시계방향으로 돌면서 발사
        if (transform.position.x > 60.0f)  //오른쪽 벽
        {   //위쪽 벽으로 발사
            x = Random.Range(-60.0f, 60.0f);
            y = 69.0f;
            now = new Vector3(x - Pos.x, y - Pos.y, 0);
            cnt++;
        }
        if (transform.position.y > 69.0f)   //위쪽 벽
        {   //왼쪽 벽으로 발사
            x = -60.0f;
            y = Random.Range(0, 69.0f);
            now = new Vector3(x - Pos.x, y - Pos.y, 0);
            cnt++;
        }
        if (transform.position.x < -60.0f)  //왼쪽 벽
        {   //바닥 으로 발사
            x = Random.Range(-60.0f, 60.0f);
            y = 0;
            now = new Vector3(x - Pos.x, y - Pos.y, 0);
            cnt++;
        }
        if (transform.position.y <= 1.5f)   //바닥
        {   //랜덤 방향 발사
            setAngle(); //발사 각도 재설정
            cnt++;
        }
    }
    void setAngle()
    {
        angle = Random.Range(0, 180.0f);   //바닥 윗 부분 각도에 대해서만 생성 각도 설정

        float x = Mathf.Cos(angle * Mathf.PI / 180.0f) * 10.0f + Pos.x;
        float y = Mathf.Sin(angle * Mathf.PI / 180.0f) * 10.0f + Pos.y;
        transform.position = new Vector3(x, y, Pos.z);

        if (angle < 90)
            now = new Vector3(0.1f, 0.1f, 0); //북동쪽
        if (angle == 90)
            now = new Vector3(0, 0.1f, 0); ; //북쪽
        if (angle > 90)
            now = new Vector3(-0.1f, 0.1f, 0);  //북서쪽
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //tag 에러 방지! -> 스크립트로 인식?
        { 
            Debug.Log("미니파이어볼 맞음");
            Move DD = GameObject.Find("DDaeng").GetComponent<Move>();

            DD.TakeDamage(power);//데미지 텍스트 뜨기 위함 
            DD.hpMove(power);
        }
    }
}
