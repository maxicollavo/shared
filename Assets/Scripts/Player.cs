using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer { get; private set; }

    public float speed;
    public float jumpForce = 5f;

    private float _moveX = 0f;

    private Rigidbody2D _rb;

    private bool hasToJump = false;

    #region Pasar al view
    [SerializeField] Renderer _renderer;
    [SerializeField] Animator _anim;

    [Networked, OnChangedRender(nameof(ColorChanged))]
    [HideInInspector] public Color NetworkedColor { get; set; }
    void ColorChanged() => _renderer.material.color = NetworkedColor;

    [Networked, OnChangedRender(nameof(MoveChanged))]
    [HideInInspector] public bool IsMoving { get; set; }
    void MoveChanged() => _anim.SetBool("isMoving", IsMoving);

    #endregion

    public override void Spawned()
    {
        //_renderer = GetComponent<Renderer>();
        //base.Spawned();

        if (!HasStateAuthority)
        {
            ResyncValuesRpc();
            return;
        }
        LocalPlayer = this;

        NetworkedColor = Color.white;
        _rb = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<CameraBehavior>().target = transform;

        //GameManager.instance.LocalPlayer = this;
    }

    //RpcSources.All => Quien lo puede ejecutar
    //RpcTargets.StateAuthority => Quien lo recibe
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ResyncValuesRpc()
    {
        StartCoroutine(SyncValues(NetworkedColor));
    }

    IEnumerator SyncValues(Color color)
    {
        NetworkedColor = Color.white;
        yield return new WaitForSeconds(0.03f);
        NetworkedColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasStateAuthority) return;
        _moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasToJump = true;
            //Jump();
            //Physics.Raycast()
            //Runner.GetPhysicsScene().Raycast
        }

        if (Input.GetKeyDown(KeyCode.R)) ChangeColor(Color.red);
        if (Input.GetKeyDown(KeyCode.G)) ChangeColor(Color.green);
        if (Input.GetKeyDown(KeyCode.B)) ChangeColor(Color.blue);
        if (Input.GetKeyDown(KeyCode.Y)) ChangeColor(Color.yellow);

    }

    void ChangeColor(Color color) => NetworkedColor = color;

    void Jump()
    {
        // _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _rb.velocity += Vector2.up * jumpForce;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        CheckCollisions();
        if (Object == null) return; //Si el objeto se destruyo en red
        
        IsMoving = _moveX != 0; //Animacion
        
        if (hasToJump)
        {
            hasToJump = false;
            Jump();
        }

        // transform.position += Vector3.right * _moveX * speed * Runner.DeltaTime;
        //_rb.MovePosition(transform.position + Vector3.right * _moveX * speed * Runner.DeltaTime);
        //_rb.AddForce(Vector3.right * _moveX * speed * 1000 * Runner.DeltaTime, ForceMode2D.Impulse);
        _rb.velocity += Vector2.right * _moveX * (speed * 1000) * Runner.DeltaTime;
        LimitSpeed();

    }

    private void LimitSpeed()
    {
        Vector3 vel = _rb.velocity;
        if (vel.x > speed)
        {
            vel.x = speed;
            _rb.velocity = vel;

        }
        else if (vel.x < -speed)
        {
            vel.x = -speed;
            _rb.velocity = vel;
        }
    }

    void CheckCollisions()
    {
        //Physics.Raycast solo local
        //(Vector2)transform.localScale
        //collider.bounds.size
        var collider = Runner.GetPhysicsScene2D().OverlapBox(transform.position, Vector2.one, 0, LayerMask.GetMask("Interact"));

        if (collider != null) Die();

    }


    public void Die()
    {
        //Destroy = Local
        //Despawn
        //hp -= dmg
        Runner.Despawn(Object);
    }

}
