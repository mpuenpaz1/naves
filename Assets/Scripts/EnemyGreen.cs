using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    [SerializeField] private GameObject limitLeft, limitRight, limitTop, limitBottom;

    // Se activa cada vez que se activa el objeto
    private void OnEnable()
    {
        // Asignamos los limites del movimiento del enemigo
        limitLeft.transform.position = new Vector2(limitLeft.transform.position.x, Random.Range(-5.5f, 5.5f)); // Limite izquierdo
        limitRight.transform.position = new Vector2(limitRight.transform.position.x, Random.Range(-5.5f, 5.5f)); // Limite derecho
        limitTop.transform.position = new Vector2(Random.Range(-3.5f, 3.5f), limitTop.transform.position.y); // Limite superior
        limitBottom.transform.position = new Vector2(Random.Range(-3.5f, 3.5f), limitBottom.transform.position.y); // Limite inferior
        // Selecciona un limite random de la lista del que spawnear
        ResetPosition();
    }

    public void ResetPosition()
    {
        // Selecciona un limite random de la lista del que spawnear
        List<Transform> limits = new List<Transform> { limitLeft.transform, limitRight.transform, limitTop.transform, limitBottom.transform };
        int randomLimit = Random.Range(0, limits.Count);
        transform.position = limits[randomLimit].position;
    }

    void Update()
    {
        player = FindObjectOfType<Player>().transform;
        // Seguimos al jugador a donde se mueva
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Laser" || collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().ganarPuntos();
            gameObject.SetActive(false);
        }
    }
}