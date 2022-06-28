using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [Header("Variables")]
    [SerializeField] float bulletSpeed;

    [HideInInspector] public Vector2 direction;
    Rigidbody2D rbBullet;
    public uint shooterNetID;

    void Start()
    {
        Debug.Log(direction);
        rbBullet.velocity = direction * bulletSpeed;
    }

    private void Awake()
    {
        rbBullet = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Player>().netId != shooterNetID)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.x < -17 || transform.position.x > 17 || transform.position.y < -10 || transform.position.y > 10)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
