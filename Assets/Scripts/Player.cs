using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    #region public references
    [Header("References")]
    [SerializeField] GameObject bullet;
    #endregion

    #region public variables
    [Header("Variables")]
    [SerializeField] [Range(0f,20f)] float movementSpeed;
    [SerializeField] [Range(0f, 20f)] float jumpForce;
    #endregion

    #region private references
    Rigidbody2D rbPlayer;
    Crosshair crosshairScript;
    GameObject gun;
    GameObject muzzle;
    #endregion

    #region private variables
    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 4f;
    #endregion

    private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        crosshairScript = FindObjectOfType<Crosshair>();
        gun = transform.GetChild(0).gameObject;
        muzzle = transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        HandleMovement();

        if (!isLocalPlayer) { return; }

        Vector2 gundDirection = (crosshairScript.mousePosition - transform.position).normalized;
        gun.transform.localPosition = gundDirection;

        float angle = Vector2.Angle(gun.transform.localPosition.normalized, transform.right);

        if (gun.transform.localPosition.y < 0)
        {
            gun.transform.rotation = Quaternion.Euler(0, 0, 360f - angle);
        }
        else
        {
            gun.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Input.GetMouseButtonDown(0))
        {
            CmdSpawnBullet(gundDirection, netId);
        }
    }

    [Command]
    private void CmdSpawnBullet(Vector2 gundDirection, uint shooterID)
    {
        SpawnBulletRPC(gundDirection, shooterID);
    }

    [ClientRpc]
    private void SpawnBulletRPC(Vector2 gundDirection, uint shooterID)
    {
        GameObject newBullet = Instantiate(bullet, muzzle.transform.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().direction = gundDirection;
        newBullet.GetComponent<Bullet>().shooterNetID = shooterID;
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rbPlayer.velocity = new Vector2(horizontalInput * movementSpeed, rbPlayer.velocity.y);

        if (Input.GetKeyDown(KeyCode.W))
        {
            rbPlayer.velocity += Vector2.up * jumpForce;
        }

        if (rbPlayer.velocity.y < 0) //Applying force to quickly fall from apex
        {
            rbPlayer.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbPlayer.velocity.y > 0 && !Input.GetKey(KeyCode.W)) // Applying force to quickly fall from a low jump
        {
            rbPlayer.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }   
    }
}
