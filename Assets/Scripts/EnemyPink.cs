using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPink : MonoBehaviour
{
    [SerializeField] private GameObject laserEnemyPrefab;
    [SerializeField] private float speed, delay;
    [SerializeField] private GameObject wayPointA, wayPointB, objective;
    private Animator animator;
    private Vector3 shootPosition;


    // Se activa cada vez que se activa el objeto
    void OnEnable()
    {
        // Empezamos la corrutina
        StartCoroutine(Shoot());
        // Selecciona un limite random de la lista del que spawnear
        ResetPosition();
        // Obtenemos el animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        // Nos movemos hacia el otro limite
        transform.position = Vector2.MoveTowards(transform.position, objective.transform.position, speed * Time.deltaTime);
        // Esto lo hacemos para evitar que choque con el limite objetivo
        if (Vector2.Distance(transform.position, objective.transform.position) < 0.2f)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        // Selecciona un limite random de la lista del que spawnear
        if (Random.Range(0, 2) == 0)
        {
            transform.position = wayPointA.transform.position;
            objective = wayPointB;
        }
        else
        {
            transform.position = wayPointB.transform.position;
            objective = wayPointA;
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            // Obtenemos la posicion del punto de disparo
            shootPosition = transform.GetChild(0).transform.position;
            Instantiate(laserEnemyPrefab, shootPosition, Quaternion.identity);
            animator.SetTrigger("Shooting");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser" || collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().ganarPuntos();
            gameObject.SetActive(false);
        }
    }
}
