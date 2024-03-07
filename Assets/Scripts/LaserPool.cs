using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private List<GameObject> lasers;

    private static LaserPool instance;

    public static LaserPool Instance { get {  return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddLasersToPool(poolSize);
    }

    private void AddLasersToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject laser = Instantiate(laserPrefab); // Instantiate the laser prefab
            laser.SetActive(false); // Set the laser as inactive
            lasers.Add(laser); // Add the laser to the LaserPool
            laser.transform.SetParent(transform); // Set the laser as a child of the LaserPool
        }
    }

    public GameObject RequestLaser()
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            if (!lasers[i].activeSelf) // If the laser is not active
            {
                lasers[i].SetActive(true); // Set the laser as active
                return lasers[i]; // Return the laser
            }
        }
        AddLasersToPool(1); // If there are no inactive lasers, add one to the pool
        lasers[lasers.Count - 1].SetActive(true); // Set the last laser in the pool as active
        return lasers[lasers.Count - 1]; // Return the last laser in the pool
    }
}
