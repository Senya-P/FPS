using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public Transform rifle;
    public Animator enemyAnimator;
    public AudioSource shot;
    public AudioSource footsteps;
    public AudioSource dying;
    public float health = 50f;
    public float bulletSpeed = 500f;
    public int rounds = 10;
    private int curRoundsCount;
    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    public GameManager gameManager;
    private int minDistance = 10;
    private NavMeshAgent navMeshAgent;
    private float cooldown = 1f;
    private bool canShoot = true;
    private bool isWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Center");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = minDistance;
        curRoundsCount = rounds;
        footsteps.pitch = 1.2f;
    }

    // Update is called once per frame
    void Update()
    { 
        navMeshAgent.destination = player.transform.position;
        if (navMeshAgent.velocity.magnitude == 0)
        {
            if (isWalking)
            {
                footsteps.Pause();
                isWalking = false;
            }
            enemyAnimator.SetBool("isWalking", false);
            enemyAnimator.SetBool("isRunning", false);
            if (Vector3.Distance(player.transform.position, transform.position) < minDistance)
            {
                /*
                var lookPos = player.transform.position - transform.position;
                lookPos.y = 0;               
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
                */
                Quaternion relativeRot = Quaternion.Inverse(rifle.rotation) * bulletSpawner.transform.rotation;
                Vector3 desiredLookDir = player.transform.position - bulletSpawner.position;
                Quaternion desiredRotation = Quaternion.LookRotation(desiredLookDir, transform.up) ;
                Vector3 rotationEuler = desiredRotation.eulerAngles;
                rotationEuler.x = 0;
                rotationEuler.z = 0;
                desiredRotation = Quaternion.Euler(rotationEuler);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 5f);
                Quaternion rifleRotation = Quaternion.LookRotation(player.transform.position - rifle.position, rifle.up) * relativeRot;
                rifle.rotation = Quaternion.Slerp(rifle.rotation, rifleRotation, Time.deltaTime * 2f);

                if (canShoot)
                    Shoot();
            }
        }
        else
        {
            enemyAnimator.SetBool("isShooting", false);
            enemyAnimator.SetBool("isWalking", true);
            if (!isWalking)
            {
                isWalking = true;
                footsteps.Play();
            }
            
        }
        if(navMeshAgent.velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
            footsteps.pitch = 2f;
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
            footsteps.pitch = 1.2f;

        }
    }
    public void Hit(float damage)
    {
        health -= damage;
        Debug.Log(dying.name);
        dying.Play(0);
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        canShoot = false;
        
        gameManager.enemiesAlive--;
        gameManager.enemiesKilled++;
        enemyAnimator.SetBool("isDead", true);
        StartCoroutine(DyingCoroutine(2.5f));
        
    }

    IEnumerator DyingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    public void Shoot()
    {
        enemyAnimator.SetBool("isShooting", true);
        if (cooldown <= 0f)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
            bullet.GetComponent<BulletController>().shooter = "Enemy";
            Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
            bulletRig.velocity = bulletSpawner.transform.forward * bulletSpeed;
            shot.Play();
            cooldown = 1f;
            curRoundsCount--;
            if (curRoundsCount == 0)
            {
                canShoot = false;
                Reload();
                
            }
        }
        cooldown -= Time.deltaTime;
    }
    private void Reload()
    {
        enemyAnimator.SetBool("isReloading", true);
        StartCoroutine(ReloadingCoroutine(3.5f));
    }
    IEnumerator ReloadingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        enemyAnimator.SetBool("isReloading", false);
        canShoot = true;
        curRoundsCount = rounds;
    }
}
