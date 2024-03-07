using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 0f;
    [SerializeField] private float maxSpeed, aceleration, curbing, deceleration; // La velocidad máxima del jugador se establece en el inspector
    [SerializeField] private float rotationGrades; // La velocidad de rotación del jugador se establece en el inspector

    private Animator animator, fireAnimator;
    //private bool isAlive = true;
    public int municion;
    [SerializeField] private Laser laserPrefab; // El prefab del láser se establece en el inspector
    [SerializeField] private float laserOffset; // El desplazamiento del láser se establece en el inspector
    private Vector3 shootPosition;

    // Start se llama antes del primer frame update
    void Start()
    {
        FindObjectOfType<UIManager>().quitarMunicion(municion);
        //isAlive = true;
        animator = GetComponent<Animator>(); // Obtener el animador
        fireAnimator = transform.GetChild(0).GetComponent<Animator>(); // Obtener el animador del fuego
    }

    // Update se llama una vez por frame
    void Update()
    {
        //if (isAlive)
        //{
            Move();
        //}
    }

    private void Move() // Movimiento y rotación del jugador
    {
        float rotationMovement = Input.GetAxis("Horizontal");
        float movement = Input.GetAxis("Vertical");
        if (rotationMovement < 0) // Si el jugador está rotando a la izquierda
        {
            transform.Rotate(0.0f, 0.0f, rotationGrades * Time.deltaTime, Space.Self); // Rotar al jugador
            animator.SetTrigger("left");
        }
        else if (rotationMovement > 0) // Si el jugador está rotando a la derecha
        {
            transform.Rotate(0.0f, 0.0f, -rotationGrades * Time.deltaTime, Space.Self); // Rotar al jugador
            animator.SetTrigger("right");
        }
        else
        {
            animator.SetTrigger("idle"); // Si el jugador no está rotando, establecer la animación de inactividad
        }

        if (movement > 0 && speed < maxSpeed) // Si el jugador se mueve hacia adelante y la velocidad es menor que la velocidad máxima
        {
            speed += aceleration * Time.deltaTime;
            fireAnimator.SetBool("going", true);
        }
        else if (movement < 0 && speed > 0) // Si el jugador se mueve hacia atrás y la velocidad es mayor que 0
        {
            speed -= curbing * Time.deltaTime;
            fireAnimator.SetBool("going", false);
        }
        else if (movement == 0 && speed > 0) // Si el jugador no se está moviendo y la velocidad es mayor que 0
        {
            speed -= deceleration * Time.deltaTime;
            fireAnimator.SetBool("going", false);
        }
        else if (movement <= 0 && speed <= 0) // Si el jugador se está moviendo hacia atrás o no se está moviendo y la velocidad es menor o igual a 0
        {
            speed = 0f;
            fireAnimator.SetBool("going", false);
        }
        transform.Translate(Vector3.up * speed * Time.deltaTime); // Mover al jugador hacia adelante o hacia atrás dependiendo de la velocidad
        
        if (Input.GetKeyDown(KeyCode.Space)) // Si el jugador presiona la tecla de espacio para disparar
        {
            if (municion > 0)
            {
                municion--;
                FindObjectOfType<UIManager>().quitarMunicion(municion);
                shootPosition = transform.GetChild(1).transform.position; // Obtener la posición del punto de disparo
                /* Utilizamos el pool de láseres para solicitar un láser en lugar de instanciar uno nuevo
                Laser laser = Instantiate(laserPrefab, shootPosition, transform.rotation);
                laser.Shoot(transform.up); // Disparar el láser
                */
                GameObject laser = LaserPool.Instance.RequestLaser(); // Solicitar un láser del pool
                laser.transform.position = shootPosition; // Establecer la posición del láser
                laser.transform.rotation = transform.rotation; // Establecer la rotación del láser
                laser.GetComponent<Laser>().Shoot(transform.up); // Disparar el láser
            }
        }
        //si pulsas r llama al metodo recargar que recarga cada 3 segundos
        if (Input.GetKeyDown(KeyCode.R))
        {
            RecargarMunicion();
        }

    }
    private void RecargarMunicion()
    {
        StartCoroutine(Recargar());
    }

    private IEnumerator Recargar()
    {
        yield return new WaitForSeconds(3f); // Esperar 5 segundos

        municion = 10; // Recargar la munición a 10 balas
        FindObjectOfType<UIManager>().quitarMunicion(municion); // Actualizar el UI de la munición
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // Si el jugador colisiona con un enemigo
        {
            FindObjectOfType<GameManager>().perderVidas();
            //esto esta comentado ya que tiene vidas y el codigo de abajo es por si muere con 1 golpe

            //isAlive = false; // El jugador no está vivo
            //transform.GetChild(0).gameObject.SetActive(false); // Desactivar el fuego
            //GetComponent<CircleCollider2D>().enabled = false; // Desactivar el collider
            //animator.SetTrigger("die"); // Establecer la animación de muerte
            //Destroy(gameObject, 1f); // Destruir al jugador después de 1 segundo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            FindObjectOfType<GameManager>().perderVidas();
        }
            if (collision.gameObject.tag == "limit") // Si el jugador golpea el límite, será teletransportado al otro lado
        {
            if (collision.transform.position.x != 0) // Si el límite es vertical
            {
                if (collision.transform.position.x > 0) // Si el límite está a la derecha
                {
                    transform.position = new Vector3(transform.position.x * -1 + 0.1f, transform.position.y, 0); // Teleportarse a la izquierda, añadiendo 0.1f para evitar la colisión
                }
                else
                {
                    transform.position = new Vector3(transform.position.x * -1 - 0.1f, transform.position.y, 0); // Teleportarse a la derecha, restando 0.1f para evitar la colisión
                }
            }
            else
            {
                if (collision.transform.position.y > 0) // Si el límite está arriba
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 + 0.1f, 0); // Teleportarse abajo, añadiendo 0.1f para evitar la colisión
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 - 0.1f, 0); // Teleportarse arriba, restando 0.1f para evitar la colisión
                }
            }
        }
    }
}