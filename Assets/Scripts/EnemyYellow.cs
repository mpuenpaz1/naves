using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYellow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject limitLeft, limitRight, limitTop, limitBottom, objective;
    
    void OnEnable()
    {
        // Set the limits of the enemy's movement
        limitLeft.transform.position = new Vector2(limitLeft.transform.position.x, Random.Range(-5.5f, 5.5f)); // Set the limitLeft position
        limitRight.transform.position = new Vector2(limitRight.transform.position.x, Random.Range(-5.5f, 5.5f)); // Set the limitRight position
        limitTop.transform.position = new Vector2(Random.Range(-3.5f, 3.5f), limitTop.transform.position.y); // Set the limitTop position
        limitBottom.transform.position = new Vector2(Random.Range(-3.5f, 3.5f), limitBottom.transform.position.y); // Set the limitBottom position
        ResetPosition();
    }


    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, objective.transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, objective.transform.position) < 0.2f)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        // Selects a random limit from the list to start form
        List<GameObject> limits = new List<GameObject> { limitLeft, limitRight, limitTop, limitBottom };
        int randomLimit = Random.Range(0, limits.Count);
        transform.position = limits[randomLimit].transform.position;
        // Selects a random limit from the list to move towards, the limit has to be different from the one the enemy started from
        limits.RemoveAt(randomLimit);
        randomLimit = Random.Range(0, limits.Count);
        objective = limits[randomLimit];
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
