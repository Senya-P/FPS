using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public PlayerController shooter;
    public Animator shooterAnimator;
    public Camera playerCamera;
    public AudioSource shot;
    public AudioSource reload;
    public AudioSource empty;
    public int rounds = 12;
    private int curRoundsCount;
    private float cooldown = 0.1f;
    public float bulletSpeed = 500f;
    private Vector3 destination;
    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    public Text roundsText;

    // Start is called before the first frame update
    void Start()
    {
        curRoundsCount = rounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooterAnimator.GetBool("isShooting"))
        {
            shooterAnimator.SetBool("isShooting", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            PlayerShoot();
        }
        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
        roundsText.text = curRoundsCount.ToString();
        if (curRoundsCount == 0)
        {
            roundsText.color = Color.red;
        }
        else
            roundsText.color = Color.white;
    }
    private void PlayerShoot()
    {
        
        if (cooldown <= 0f)
        {
            if (shooter.canShoot)
            {
                shooterAnimator.SetBool("isShooting", true);
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    destination = hit.point;
                }
                else
                {
                    destination = ray.GetPoint(1000);
                }

                var bullet = Instantiate(bulletPrefab, bulletSpawner.position, transform.rotation);
                bullet.GetComponent<BulletController>().shooter = "Player";
                bullet.GetComponent<Rigidbody>().velocity = (destination - bulletSpawner.position).normalized * bulletSpeed;
                shot.Play(0);
                cooldown = 0.1f;
                curRoundsCount--;
                if (curRoundsCount <= 0)
                {
                    shooter.canShoot = false;

                }
            }
            else
            {
                empty.Play(0);
            }
        }
        cooldown -= Time.deltaTime;
        
    }
    private void Reload()
    {
        shooterAnimator.SetBool("isReloading", true);
        reload.PlayDelayed(2f);
        shooter.canShoot = false;
        StartCoroutine(ReloadingCoroutine(2.8f));
    }
    IEnumerator ReloadingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        shooterAnimator.SetBool("isReloading", false);
        curRoundsCount = rounds;
        shooter.canShoot = true;
    }
}
