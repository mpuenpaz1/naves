using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 0f;
    [SerializeField] private float maxSpeed, aceleration, curbing, deceleration; // La velocidad m�xima del jugador se establece en el inspector
    [SerializeField] private float rotationGrades; // La velocidad de rotaci�n del jugador se establece en el inspector

    private Animator animator, fireAnimator;
    //private bool isAlive = true;
    public int municion;
    [SerializeField] private Laser laserPrefab; // El prefab del l�ser se establece en el inspector
    [SerializeField] private float laserOffset; // El desplazamiento del l�ser se establece en el inspector
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

    private void Move() // Movimiento y rotaci�n del jugador
    {
        float rotationMovement = Input.GetAxis("Horizontal");
        float movement = Input.GetAxis("Vertical");
        if (rotationMovement < 0) // Si el jugador est� rotando a la izquierda
        {
            transform.Rotate(0.0f, 0.0f, rotationGrades * Time.deltaTime, Space.Self); // Rotar al jugador
            animator.SetTrigger("left");
        }
        else if (rotationMovement > 0) // Si el jugador est� rotando a la derecha
        {
            transform.Rotate(0.0f, 0.0f, -rotationGrades * Time.deltaTime, Space.Self); // Rotar al jugador
            animator.SetTrigger("right");
        }
        else
        {
            animator.SetTrigger("idle"); // Si el jugador no est� rotando, establecer la animaci�n de inactividad
        }

        if (movement > 0 && speed < maxSpeed) // Si el jugador se mueve hacia adelante y la velocidad es menor que la velocidad m�xima
        {
            speed += aceleration * Time.deltaTime;
            fireAnimator.SetBool("going", true);
        }
        else if (movement < 0 && speed > 0) // Si el jugador se mueve hacia atr�s y la velocidad es mayor que 0
        {
            speed -= curbing * Time.deltaTime;
            fireAnimator.SetBool("going", false);
        }
        else if (movement == 0 && speed > 0) // Si el jugador no se est� moviendo y la velocidad es mayor que 0
        {
            speed -= deceleration * Time.deltaTime;
            fireAnimator.SetBool("going", false);
        }
        else if (movement <= 0 && speed <= 0) // Si el jugador se est� moviendo hacia atr�s o no se est� moviendo y la velocidad es menor o igual a 0
        {
            speed = 0f;
            fireAnimator.SetBool("going", false);
        }
        transform.Translate(Vector3.up * speed * Time.deltaTime); // Mover al jugador hacia adelante o hacia atr�s dependiendo de la velocidad
        
        if (Input.GetKeyDown(KeyCode.Space)) // Si el jugador presiona la tecla de espacio para disparar
        {
            if (municion > 0)
            {
                municion--;
                FindObjectOfType<UIManager>().quitarMunicion(municion);
                shootPosition = transform.GetChild(1).transform.position; // Obtener la posici�n del punto de disparo
                /* Utilizamos el pool de l�seres para solicitar un l�ser en lugar de instanciar uno nuevo
                Laser laser = Instantiate(laserPrefab, shootPosition, transform.rotation);
                laser.Shoot(transform.up); // Disparar el l�ser
                */
                GameObject laser = LaserPool.Instance.RequestLaser(); // Solicitar un l�ser del pool
                laser.transform.position = shootPosition; // Establecer la posici�n del l�ser
                laser.transform.rotation = transform.rotation; // Establecer la rotaci�n del l�ser
                laser.GetComponent<Laser>().Shoot(transform.up); // Disparar el l�ser
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

        municion = 10; // Recargar la munici�n a 10 balas
        FindObjectOfType<UIManager>().quitarMunicion(municion); // Actualizar el UI de la munici�n
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // Si el jugador colisiona con un enemigo
        {
            FindObjectOfType<GameManager>().perderVidas();
            //esto esta comentado ya que tiene vidas y el codigo de abajo es por si muere con 1 golpe

            //isAlive = false; // El jugador no est� vivo
            //transform.GetChild(0).gameObject.SetActive(false); // Desactivar el fuego
            //GetComponent<CircleCollider2D>().enabled = false; // Desactivar el collider
            //animator.SetTrigger("die"); // Establecer la animaci�n de muerte
            //Destroy(gameObject, 1f); // Destruir al jugador despu�s de 1 segundo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            FindObjectOfType<GameManager>().perderVidas();
        }
            if (collision.gameObject.tag == "limit") // Si el jugador golpea el l�mite, ser� teletransportado al otro lado
        {
            if (collision.transform.position.x != 0) // Si el l�mite es vertical
            {
                if (collision.transform.position.x > 0) // Si el l�mite est� a la derecha
                {
                    transform.position = new Vector3(transform.position.x * -1 + 0.1f, transform.position.y, 0); // Teleportarse a la izquierda, a�adiendo 0.1f para evitar la colisi�n
                }
                else
                {
                    transform.position = new Vector3(transform.position.x * -1 - 0.1f, transform.position.y, 0); // Teleportarse a la derecha, restando 0.1f para evitar la colisi�n
                }
            }
            else
            {
                if (collision.transform.position.y > 0) // Si el l�mite est� arriba
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 + 0.1f, 0); // Teleportarse abajo, a�adiendo 0.1f para evitar la colisi�n
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y * -1 - 0.1f, 0); // Teleportarse arriba, restando 0.1f para evitar la colisi�n
                }
            }
        }
    }
}