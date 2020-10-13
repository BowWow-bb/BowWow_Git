using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour
{
    bool left;
    bool arrival;
    bool spin;
    GameObject DD;
    Vector3 position;
    float check;
    float rotate;
    float velocity;
    float accelaration;
    Vector3 trace;

    // Start is called before the first frame update
    void Start()
    {
        //-45 45

        accelaration = 0.1f;
        rotate = 0;
        check = gameObject.transform.position.x;
        position = gameObject.transform.position;
        arrival = false;
        spin = false;
        DD = GameObject.Find("DDaeng");


        if (DD.transform.position.x > position.x) // 음파가 왼쪽일때
            left = true;
        else
            left = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotate += 5f;
        if (!arrival)
        {
            if (left && gameObject.transform.position.x > -45f)
            {
                gameObject.transform.position = new Vector3(check, (position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (-46 - position.x))), 0);
                check -= 0.1f;
            }
            else if (!left && gameObject.transform.position.x < 45f)
            {
                gameObject.transform.position = new Vector3(check, position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (46 - position.x)), 0);
                check += 0.1f;
            }
            else
            {
                position = gameObject.transform.position;
                arrival = true;
                check = 0;
            }
        }
        else
        {
            if (check <= 20f&& !spin)
            {
                gameObject.transform.position = new Vector3(position.x + 3 * Mathf.Cos(rotate), position.y + 3 * Mathf.Sin(rotate), 0);
                check += 0.1f;
            }
            else if (check > 20f &&!spin)
            {
                gameObject.transform.position = new Vector3(position.x + 3 * Mathf.Cos(rotate), position.y + 3 * Mathf.Sin(rotate), 0);
                check = 0f;
                spin = true;
                position = gameObject.transform.position;
            }
            else if(spin)
            {
             //  DD = GameObject.Find("DDaeng");
                //trace.x = (DD.transform.position.x - gameObject.transform.position.x) / Mathf.Sqrt(Mathf.Pow((DD.transform.position.x - gameObject.transform.position.x), 2) + Mathf.Pow((DD.transform.position.y - gameObject.transform.position.y), 2));
                //trace.y = (DD.transform.position.y - gameObject.transform.position.y) / Mathf.Sqrt(Mathf.Pow((DD.transform.position.x - gameObject.transform.position.x), 2) + Mathf.Pow((DD.transform.position.y - gameObject.transform.position.y), 2));
                //trace.z = 0;
                trace = (DD.transform.position - transform.position).normalized;
                velocity = velocity + accelaration * check;
                float distance = Vector3.Distance(DD.transform.position, transform.position);
                if (distance <= 1f)
                {
                    Debug.Log('x');
                    Destroy(gameObject, 0);
                }
                else if (distance > 0)
                {
                  //  Debug.Log('x');
                    gameObject.transform.position = new Vector3(transform.position.x + (trace.x * velocity)
                        , transform.position.y + (trace.y * velocity),DD.transform.position.z);
                    check += 0.00001f;
                    Debug.Log(distance);
                }

                //if (position.x+check <= DD.transform.position.x && DD.transform.position.x>0) {
                //    gameObject.transform.position = trace*check;
                //    check += 0.1f;
                //}
            }
        }

        gameObject.transform.rotation = Quaternion.Euler(rotate, rotate, 0);
    }

    void OnTriggerEnter(Collider other)//몬스터 때리기 
    {
        if (other.gameObject.GetComponent<small_toll>() != null)    //스테이지2 몬스터와 충돌한 경우
        {
            small_toll monster = other.GetComponent<small_toll>();
            monster.TakeDamage(10);//공격         
            monster.hpMove(10.0f);

            if (monster.HP <= 0)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.GetComponent<small_toll_stage1>() != null)  //스테이지1 몬스터와 충돌한 경우
        {
            small_toll_stage1 monster = other.GetComponent<small_toll_stage1>();
            monster.TakeDamage(10);
            monster.hpMove(10.0f);

            if (monster.HP <= 0)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.GetComponent<Bigtol>() != null)
        {
            Bigtol monster = other.GetComponent<Bigtol>();
            monster.TakeDamage(10);
            monster.hpMove(10.0f);

            if (monster.HP <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
