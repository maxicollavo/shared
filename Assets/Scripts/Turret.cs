using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : NetworkBehaviour
{
    [SerializeField] float _timeToShoot = 1;
    private float _timer = 0;

    [SerializeField] GameObject _bulletPrefab;
    
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        _timer += Runner.DeltaTime;
        if(_timer > _timeToShoot)
        {
            _timer = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        Runner.Spawn(_bulletPrefab, transform.position + Vector3.down * 1.5f);
    }
}
