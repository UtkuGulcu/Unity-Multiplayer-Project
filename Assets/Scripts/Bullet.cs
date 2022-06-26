using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float bulletSpeed;

    [HideInInspector] public Vector2 direction;
    Rigidbody2D rbBullet;

    void Start()
    {
        rbBullet.velocity = direction * bulletSpeed;
    }

    private void Awake()
    {
        rbBullet = GetComponent<Rigidbody2D>();
    }
}
