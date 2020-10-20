using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBigbo : MonoBehaviour
{
    AudioSource Item_pick;
    AudioSource Item_drop;

    public AudioClip Item_drop_Sound;
    public AudioClip Item_Sound;

    bool isDDaeng = false;

    float t;    //타이머

    W_Bigbo w_Bigbo;
    // Start is called before the first frame update
    void Start()
    {
        w_Bigbo = GameObject.Find("W_Bigbo").GetComponent<W_Bigbo>();

        t = 0;

        Item_pick = gameObject.AddComponent<AudioSource>();
        Item_pick.clip = Item_Sound;
        Item_pick.volume = 0.6f;
        Item_pick.loop = false;

        Item_drop = gameObject.AddComponent<AudioSource>();
        Item_drop.clip = Item_drop_Sound;
        Item_drop.volume = 0.6f;
        Item_drop.loop = false;

        Item_drop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //일정시간 경과 후 아이템 소멸
        if (t >= 100.0f)
            Destroy(gameObject);

        t += 0.1f;
        if(isDDaeng&&Input.GetKeyDown(KeyCode.Z))
        {
            getBigbo();
            Item_pick.Play();
            Debug.Log("땡이가 먹음");
            Destroy(gameObject, 0.2f);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            isDDaeng = true;
            //Item_pick.Play();
            //Debug.Log("땡이와 충돌!");
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    Item_pick.Play();
            //    Debug.Log("땡이가 먹음");
            //    Destroy(gameObject);
            //}
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            isDDaeng = true;
            /*Debug.Log("땡이와 충돌!");
            if(Input.GetKeyDown(KeyCode.Z))
            {
                getBigbo();
                Item_pick.Play();
                Debug.Log("땡이가 먹음");
                Destroy(gameObject, 0.2f);
            }
            */
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)
        {
            isDDaeng = false;
        }
    }

    void getBigbo()
    {
        if (!w_Bigbo.isThere) //스킬 미습득한 상태
            w_Bigbo.isThere = true;  //스킬 습득
    }
}
