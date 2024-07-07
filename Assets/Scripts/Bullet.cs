using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        transform.position += Vector3.down * 3 * Runner.DeltaTime;
    }

    //TODO que cuando colisione se destruya y llame a Die() del player
    
}
