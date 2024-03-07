using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text; // Texto para mostrar alg�n mensaje
    [SerializeField] TMPro.TextMeshProUGUI scores; // Texto para mostrar la puntuaci�n
    [SerializeField] private GameObject[] lifes; // Array de objetos de vida
    [SerializeField] GameManager gameManager; // Referencia al GameManager
    [SerializeField] TMPro.TextMeshProUGUI muni; // Texto para mostrar la cantidad de munici�n

    void Awake()
    {
        // Busca y guarda todos los objetos con el tag "vida"
        lifes = GameObject.FindGameObjectsWithTag("vida");

        // Obtiene una referencia al GameManager
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        // Configura la puntuaci�n y las vidas iniciales al inicio del juego
        SetScore(gameManager.puntos);
        SetLifes(gameManager.vidas);
    }

    // Establece el n�mero de vidas en la interfaz de usuario
    public void SetLifes(int vidas)
    {
        foreach (var life in lifes)
        {
            life.SetActive(false); // Desactiva todas las vidas
        }
        for (int i = 0; i < vidas; i++)
        {
            lifes[i].SetActive(true); // Activa las vidas necesarias seg�n la cantidad de vidas del jugador
        }
    }

    // Actualiza el texto que muestra la cantidad de munici�n
    public void quitarMunicion(int numero)
    {
        muni.text = numero.ToString();
    }

    // Establece el texto que muestra la puntuaci�n
    public void SetScore(int score)
    {
        scores.text = score.ToString();
    }

    // Actualiza el contador de tiempo mostrado en la interfaz de usuario
    public void cambiarContador(float tiempo)
    {
        // Actualiza el texto con el tiempo redondeado a dos decimales
        text.SetText(tiempo.ToString("0.00"));
    }

    // M�todo llamado cuando el jugador pierde una vida
    public void LoseLife()
    {
        // Itera sobre las vidas en orden inverso
        for (int i = lifes.Length - 1; i > 0; i--)
        {
            if (lifes[i].activeSelf) // Encuentra la primera vida activa
            {
                lifes[i].SetActive(false); // Desactiva la vida
                break; // Sale del bucle
            }
        }
    }

    // M�todo llamado cuando el jugador gana una vida
    public void AddLife()
    {
        // Itera sobre todas las vidas
        for (int i = 0; i < lifes.Length; i++)
        {
            if (!lifes[i].activeSelf) // Encuentra la primera vida inactiva
            {
                lifes[i].SetActive(true); // Activa la vida
                break; // Sale del bucle
            }
        }
    }
}