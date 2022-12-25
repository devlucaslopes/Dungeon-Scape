using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    class WaveSettings
    {
        public Enemy[] Enemies;
        public int TotalEnemies;
        public float Duration;
    }

    [SerializeField] private WaveSettings[] Waves;
    [SerializeField] private Transform[] SpawnPoints;

    [Header("UI")]
    [SerializeField] private GameObject CountdownUI;
    [SerializeField] private Text CountdownText;
    [SerializeField] private GameObject LevelCompletedUI;

    [Header("FX")]
    [SerializeField] private GameObject ParticleEffects;

    private int _currentWaveIndex = 0;
    private bool _finishSpawning;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (_finishSpawning && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            _finishSpawning = false;
            _currentWaveIndex++;

            if (_currentWaveIndex < Waves.Length)
            {
                StartCoroutine(StartWave());
            } else
            {
                Instantiate(ParticleEffects, transform.position, Quaternion.identity);
                LevelCompletedUI.SetActive(true);
            }
        }
    }

    IEnumerator StartWave()
    {
        Debug.Log($"WAVE #{_currentWaveIndex} - CREATING WAVE!");

        CountdownUI.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        CountdownUI.SetActive(false);

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log($"WAVE #{_currentWaveIndex} - SPAWNING ENEMIES!");

        WaveSettings currentWave = Waves[_currentWaveIndex];

        for (int i = 0; i < currentWave.TotalEnemies; i++)
        {
            Enemy randomEnemy = currentWave.Enemies[Random.Range(0, currentWave.Enemies.Length)];
            Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

            Instantiate(randomEnemy, spawnPoint.position, Quaternion.identity);

            _finishSpawning = i == currentWave.TotalEnemies - 1;

            yield return new WaitForSeconds(1);
        }
    }
}
