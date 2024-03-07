using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine;
using Unity.VisualScripting;

public class EnemiesPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyYellow, enemyPink, enemyGreen;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private List<GameObject> enemies;

    private static EnemiesPool instance;

    public static EnemiesPool Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddEnemiesToPool(poolSize);
        StartCoroutine(EnableEnemy());
    }
    //Agrega un número especificado de enemigos a la piscina.
    //Selecciona aleatoriamente entre los tipos de enemigos y los instancia.
    //Desactiva los enemigos recién instanciados y los agrega a la lista enemies.
    //Establece el objeto padre de los enemigos al transformador de este objeto (EnemiesPool).
    private void AddEnemiesToPool(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            int randomEnemy = UnityEngine.Random.Range(0, 3);
            GameObject enemy = null;
            switch (randomEnemy)
            {
                case 0:
                    enemy = Instantiate(enemyYellow);
                    break;
                case 1:
                    enemy = Instantiate(enemyPink);
                    break;
                case 2:
                    enemy = Instantiate(enemyGreen);
                    break;
            }
            enemy.SetActive(false);
            enemies.Add(enemy);
            enemy.transform.SetParent(transform);
        }
    }
    //Solicita un enemigo activo de la piscina.
    //Si encuentra un enemigo inactivo en la lista, lo activa y lo devuelve.

    public GameObject RequestEnemy()
    {
        int randomEnemy = UnityEngine.Random.Range(0, enemies.Count);
        for (int i = randomEnemy; i < enemies.Count; i++)
        {
            if (!enemies[i].activeSelf)
            {
                enemies[i].SetActive(true);
                return enemies[i];
            }
        }
        //Si no hay enemigos inactivos, agrega uno a la piscina y lo activa.
        AddEnemiesToPool(1);
        enemies[enemies.Count - 1].SetActive(true);
        return enemies[enemies.Count - 1];
    }
    //Corrutina que se ejecuta continuamente.
    //Cada 3 segundos, solicita un nuevo enemigo de la piscina.
        IEnumerator EnableEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            RequestEnemy();
        }
    }
}
