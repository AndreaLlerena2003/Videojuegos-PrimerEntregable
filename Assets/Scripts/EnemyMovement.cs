using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed; //velocidad de movimiento del enemigo
    [SerializeField]
    public float rotationSpeed = 1f;

    private Vector2 direction; //dirección
    private Collider2D collider2D; //collider q tiene el enemigo
    private float colliderWidth; //ancho y alto de collider para manejo de rebote en bordes
    private float colliderHeight;

    void Start()
    {
        // se inicializa la dirección con un vector aleatorio normalizado dentro de un círculo unitario
        direction = Random.insideUnitCircle.normalized;

        //obtenemos el collider del objeto
        collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            colliderWidth = collider2D.bounds.size.x;
            colliderHeight = collider2D.bounds.size.y;
        }
    }

    void Update()
    {
        //mover el enemigo en la dirección actual multiplicada por la velocidad y el tiempo transcurrido desde el último frame
        // Calcular el nuevo desplazamiento
        Vector3 movement = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;

        // Actualizar la posición sumando el vector de movimiento
        transform.position += movement;

        // Rota el enemigo
        transform.rotation *= Quaternion.Euler(0f, 0f, rotationSpeed);
 
    }


    // Rebota realista con todo
    /*void OnCollisionEnter2D(Collision2D collision)
    {        
        // Obtener la normal de la colisión
        Vector2 normal = collision.contacts[0].normal;

        // Reflejar la dirección actual del movimiento en base a la normal de la colisión
        direction = Vector2.Reflect(direction, normal);
        
        
    }*/

    // Rebota realista con las paredes, pero se invierte con lo demás
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto con el que se colisiona es una pared
        if (collision.gameObject.CompareTag("Limit"))
        {
            // Obtener la normal de la colisión
            Vector2 normal = collision.contacts[0].normal;

            // Reflejar la dirección actual del movimiento en base a la normal de la colisión
            direction = Vector2.Reflect(direction, normal);
        }
        else
        {
            // Invertir la dirección si se colisiona con otro objeto no etiquetado como "Limit"
            direction = -direction;
        }
    }*/

    // Invierte la dirección cuando rebota choca con alguien más
    void OnCollisionEnter2D(Collision2D collision)
    {
        // invertir la dirección al colisionar con otro objeto --> para esos casos q choque con otro enemigo y asi no le de lag
        direction = -direction; // -> rebote
    }
}
