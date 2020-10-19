using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_trigger : MonoBehaviour
{
    Vector3 me;

    AudioSource Apa;
    public AudioClip ApaSound;

    private void Start()
    {
        me = transform.position;

        Apa = gameObject.AddComponent<AudioSource>();
        Apa.clip = ApaSound;
        Apa.loop = false;

        
    }
    void OnTriggerEnter(Collider other)
    {
        Move dd = GameObject.Find("DDaeng").GetComponent<Move>();

        if (other.gameObject.GetComponent<Move>() != null && !dd.isbig)//땡이가 맞을 경우 
        {
            Apa.Play();
            dd.TakeDamage(10);
            dd.hpMove(10);
        }
    }
}
