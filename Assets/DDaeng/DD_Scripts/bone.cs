using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour
{
    bool left; // 뼈다귀가 땡이 보다 왼쪽인지 판단
    bool arrival; // 던진 뼈다귀가 끝에 도착했는지 판단
    bool spin; // 던진 뼈다귀가 돌았는지 판단
    GameObject DD; // 뼈다귀를 발사한 주인공 ( 땡이 ) 오브젝트
    Vector3 position; // 오브젝트의 위치
    float check; 
    float rotate; // 뼈다귀 자체가 도는 rotation 값 ( 자전 ) 
    float velocity; // 속력
    float accelaration; // 가속력
    Vector3 trace; // 땡이 추적 벡터

    AudioSource Boomerang;
    public AudioClip BoomerangSound;

    // Start is called before the first frame update
    void Start()
    {
        accelaration = 0.1f; // 가속력 설정
        rotate = 0; // 회전각 초기화
        check = gameObject.transform.position.x; // 발사되고 끝에 도착할 때 까지 뼈다귀의 x 좌표를 담당
        position = gameObject.transform.position; // 뼈다귀 좌표
        arrival = false; // 초기화
        spin = false; // 초기화
        DD = GameObject.Find("DDaeng"); // 주인공 오브젝트 찾아서 초기화.

        Boomerang = gameObject.AddComponent<AudioSource>();
        Boomerang.clip = BoomerangSound;
        Boomerang.loop = false;

        Boomerang.Play();

        if (DD.transform.position.x > position.x) // 땡이보다 음파가 왼쪽일때 , 오른쪽일때 판단
            left = true;
        else
            left = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotate += 5f; // 뼈다귀는 계속해서 자전함
        if (!arrival) // 화면의 끝에 도착했는지
        {
            if (left && gameObject.transform.position.x > -60f) // 뼈다귀가 왼쪽으로 날라가야 하고 , 화면의 끝에 닿지 않았을 때
            {
                // check의 값을 x 좌표의 왼쪽방향으로 늘려줘서 x축은 왼쪽으로 전진
                // check값(x좌표) 값이 변화함에 따라 y축의 좌표는 던지는 기준에서 위로 포물선을 그리게 설정
                gameObject.transform.position = new Vector3(check, (position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (-60 - position.x))), 0);
                check -= 0.5f;
            }
            else if (!left && gameObject.transform.position.x < 60f) // 뼈다귀가 오른쪽으로 날라가야 하고 , 화면의 끝에 닿지 않았을 때
            {
                // check의 값을 x 좌표의 오른쪽방향으로 늘려줘서 x축은 왼쪽으로 전진
                // check값(x좌표) 값이 변화함에 따라 y축의 좌표는 던지는 기준에서 위로 포물선을 그리게 설정
                gameObject.transform.position = new Vector3(check, position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (60 - position.x)), 0);
                check += 0.5f;
            }
            else
            {
                // 뼈다귀가 화면의 끝에 도착했으면 도착한 좌표를 저장하고 ,check 0 초기화 , arrival 변수는 true 로 하여 다음 단계로 가게 만든다.
                position = gameObject.transform.position;
                arrival = true;
                check = 0;
            }
        }
        else // 화면의 양끝중 하나에 도착한 이후
        {
            if (check <= 20f&& !spin) // 아직 제자리에서 돌지 않은 경우
            {
                // 제자리에서 check가 증가하는 동안 오브젝트를 돌린다.
                gameObject.transform.position = new Vector3(position.x + 3 * Mathf.Cos(rotate), position.y + 3 * Mathf.Sin(rotate), 0);
                check += 0.5f;
            }
            else if (check > 20f &&!spin)
            {
                // check가 어느정도 증가하면 0으로 초기화 , 좌표 저장 , spin 변수를 바꿔주어 다음 단계로 진행
                gameObject.transform.position = new Vector3(position.x + 3 * Mathf.Cos(rotate), position.y + 3 * Mathf.Sin(rotate), 0);
                check = 0f;
                spin = true;
                position = gameObject.transform.position;
            }
            else if(spin)
            { 
                // 땡이와 뼈다귀의 위치를 빼서 위치벡터를 만들고 normalize 해준 벡터를 trace 저장
                trace = (DD.transform.position - transform.position).normalized;
                // 속도 = 속도 + 가속도 
                // check 는 속도 조절 
                velocity = velocity + accelaration * check;
                float distance = Vector3.Distance(DD.transform.position, transform.position); // 땡이와 뼈다귀의 거리
                if (distance <= 1f)
                {
                    // 거리가 1이하가 된다면 오브젝트를 없앤다.
                    Destroy(gameObject, 0);
                    Boomerang.Stop();
                }
                else if (distance > 0)
                {
                    // 거리가 멀다면 x,y 좌표를 trace 벡터*속도를 해서 오브젝트와의 거리를 좁힌다. z좌표는 그냥 땡이의 z좌표.
                    gameObject.transform.position = new Vector3(transform.position.x + (trace.x * velocity)
                        , transform.position.y + (trace.y * velocity),DD.transform.position.z);
                    check += 0.0001f;
                    Debug.Log(distance);
                }

            }
        }

        gameObject.transform.rotation = Quaternion.Euler(rotate, rotate, 0); // 자전 적용
    }

    void OnTriggerEnter(Collider other)//몬스터 때리기 
    {
        if (other.gameObject.GetComponent<small_toll>() != null)    //스테이지2 몬스터와 충돌한 경우
        {
            small_toll monster = other.GetComponent<small_toll>();
            monster.TakeDamage(10);//공격         
            monster.hpMove(10);
        }
        if (other.gameObject.GetComponent<small_toll_stage1>() != null)  //스테이지1 몬스터와 충돌한 경우
        {
            small_toll_stage1 monster = other.GetComponent<small_toll_stage1>();
            monster.TakeDamage(10);
            monster.hpMove(10);
        }
        if (other.gameObject.GetComponent<Bigtol>() != null)
        {
            Bigtol monster = other.GetComponent<Bigtol>();
            monster.TakeDamage(10);
            monster.hpMove(10);
        }
    }
}
