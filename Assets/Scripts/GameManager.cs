using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int vidas = 3;
    [SerializeField] public int puntos = 10; // Puntos por alien
    [SerializeField] public float tiempo;
    [SerializeField] UIManager uiManager;
    public int puntosTotales;
    private void Awake()
    {
        // Patrón Singleton
        if (Instance != null && Instance != this) // Si ya existe una instancia de GameManager
        {
            Destroy(gameObject); // Destruimos el objeto
        }
        else // Si no existe una instancia de GameManager
        {
            Instance = this; // La creamos
            DontDestroyOnLoad(gameObject); // No se destruirá al cargar una nueva escena
        }
        
    }
    private void Update()
    {
        tiempo -= Time.deltaTime;
        if (tiempo >= 0)
        {
            uiManager.cambiarContador(tiempo);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }

    }
    public void perderVidas()
    {
        vidas -= 1;
        FindObjectOfType<UIManager>().LoseLife();
        if(vidas <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void ganarPuntos()
    {
        puntosTotales += puntos;
        FindObjectOfType<UIManager>().SetScore(puntosTotales);
    }
}
