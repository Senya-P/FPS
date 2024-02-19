using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    public bool canShoot { get; set; }
    public Image healthBar;
    public GameManager gameManager;
    public void Awake()
    {
        canShoot = true;
    }
    public void Hit(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100f;
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        gameManager.EndGame();
    }
}
