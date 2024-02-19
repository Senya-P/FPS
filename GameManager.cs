using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int enemiesAlive = 0;
    public int enemiesKilled = 0;
    public int round = 3;
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;
    public GameObject endScreen;
    public AudioSource background;
    public Text enemies;
    // Start is called before the first frame update
    void Start()
    {
        background.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive == 1)
        {
            round++;
            NextWave(round);
        }
        enemies.text = "Killed: " + enemiesKilled.ToString();
    }

    public void NextWave(int round)
    {
        for (int i = 0; i < round; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            enemySpawned.GetComponent<EnemyController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }

    public void EndGame()
    {
        endScreen.SetActive(true);
        background.Pause();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        background.Play();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
