using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float speed; //velocidad de movimiento del enemigo
    [SerializeField]
    public float rotationSpeed = 1f;

    private Vector2 direction; //dirección

    private Explosion explosionPrefab;
    private SpriteRenderer spriteRenderer;
    private Color enemyColor;
    void Awake()
    {
        // Cargar el prefab de la explosión desde la carpeta Resources
        explosionPrefab = Resources.Load<Explosion>("Explosion Template"); // Asegúrate de que el prefab esté en la carpeta "Resources"

        if (explosionPrefab == null)
        {
            Debug.LogError("El prefab de explosión no se ha encontrado. Asegúrate de que esté en la carpeta Resources.");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Obtener el color actual del enemigo
            enemyColor = spriteRenderer.color;
            Debug.Log("Color del enemigo: " + enemyColor);
        }
        else
        {
            Debug.LogError("No se encontró el componente SpriteRenderer en el enemigo.");
        }
    }

    void Start()
    {
        // se inicializa la dirección con un vector aleatorio normalizado dentro de un círculo unitario
        direction = Random.insideUnitCircle.normalized;
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


        // Verificar si la bala ha chocado con un enemigo
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Explosion newExplosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
            newExplosion.Explode(enemyColor);
            // Destruye el enemigo
            Destroy(gameObject);
            // Destruye la bala
            Destroy(collision.gameObject);

            

            // Llamar al método para incrementar el contador en el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementEnemyCount();
            }


        }
        // invertir la dirección al colisionar con otro objeto --> para esos casos q choque con otro enemigo y asi no le de lag
        direction = -direction; // -> rebote
    }
}
