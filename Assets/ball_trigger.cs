using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_trigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)
        {
            Move dd = GameObject.Find("DDaeng").GetComponent<Move>();
            dd.TakeDamage(10);
            dd.hpMove(10);
        }
    }
}
