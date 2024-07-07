using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOfDeath : NetworkBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float _timeToChangeDir = 1;
    float _timer;

    int dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Player.LocalPlayer
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        _timer += Runner.DeltaTime;
        if (_timer > _timeToChangeDir)
        {
            _timer = 0;
            dir *= -1;
        }

        transform.position += (Vector3.up * dir) * speed * Runner.DeltaTime;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (Object == null) return;
        //if (!HasStateAuthority) return;

        if (collision.TryGetComponent(out Player player))
        {
            //Debug.Log("a");
            player.Die();

        }
    }*/
}
