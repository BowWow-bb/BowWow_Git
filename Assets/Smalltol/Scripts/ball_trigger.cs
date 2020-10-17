using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_trigger : MonoBehaviour
{
    Vector3 me;
    private void Start()
    {
        me = transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)//땡이가 맞을 경우 
        {
            Move dd = GameObject.Find("DDaeng").GetComponent<Move>();
            dd.TakeDamage(10);
            dd.hpMove(10);
        }
    }
}
