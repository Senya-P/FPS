using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 25f;
    public string shooter;
    void Start()
    {
        //bulletRig = GetComponent<Rigidbody>();
        //bulletRig.velocity = transform.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    public void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.collider.gameObject.GetComponent<PlayerController>();
        EnemyController enemy = collision.collider.gameObject.GetComponent<EnemyController>();
        
        if (enemy != null && shooter != "Enemy")
        {
            enemy.Hit(damage);
        }
        if (player != null && shooter != "Player")
        {
            player.Hit(damage);
        }

        Destroy(gameObject);
    }
}
