using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFireball : MonoBehaviour
{
    int power;      //공격력

    float t;        //타이머
    float angle;    //생성 각도

    bool wall_col;  //벽 충돌 유무

    Vector3 Pos;
    Vector3 now;

    // Start is called before the first frame update
    void Start()
    {
        power = 10;
        t = 3.0f;
        wall_col = false;
        Pos = transform.position;   //생성 초기위치

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

        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += now * t;

        if (Mathf.Abs(transform.position.x) > 60 || transform.position.y > 69)  //벽에 맞은 경우
            now *= -1;  //방향 반대로
        if (transform.position.y <= 1.5f)  //바닥에 맞은 경우 
            now *= -1;  //방향 반대로
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
