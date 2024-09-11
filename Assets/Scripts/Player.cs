using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private TrailRenderer trailRenderer; // Referencia al TrailRenderer


    public float moveSpeed = 5f; // velocidad de movimiento del jugador
    public float dashSpeed = 10f; // Velocidad del dash
    public float dashDuration = 0.2f; // Duración del dash
    public float doubleTapTime = 0.3f; // Tiempo máximo entre dos pulsaciones para que se considere doble

    private Rigidbody2D rb; // referencia al Rigidbody2D
    private Vector2 moveInput; // entrada del movimiento
    private Camera cam; // camara principal
    private float halfWidth; // calculamos ancho y altura para manejar q no salga de la camara
    private float halfHeight;
    private float dashTimeLeft = 0f; // Tiempo restante del dash
    private Vector2 dashDirection; // Dirección del dash
    private float lastHorizontalInputTime = -1f; // Tiempo de la última pulsación horizontal
    private float lastVerticalInputTime = -1f; // Tiempo de la última pulsación vertical

    // Vidas del jugador
    public int vidas = 5; // Inicialmente tiene 3 vidas

    // Referencia al panel de Game Over
    public GameObject gameOverPanel;

    void Start()
    {
        cam = Camera.main; // se asigna la camara principal

        // obtenemos dimensiones del sprinte para ver el tamaño de nuestro objeto y manejar q no salfa de pantalla
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.extents.x;
            halfHeight = spriteRenderer.bounds.extents.y;
        }
        // lo colocamos como falso
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        //obtiene la entrada del jugador para los ejes horizontales y verticales
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Se crea vector basado en la entrada del jugador --> y se normaliza (asi magnitud uno)
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

   
        // Detectar doble pulsación para dash en el eje horizontal
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Time.time - lastHorizontalInputTime < doubleTapTime)
            {
                // Asegúrate de que el dash no sea en la dirección opuesta al movimiento actual
                // Se hace un producto vector con el vector unitario del movimiento del dash y el movimiento actual
                // Si es 1, son iguales. Misma direccion
                // Si es -1, direccion opuesta
                // Es 0.5 para que sean similares los inputs el dash
                if (Vector2.Dot(moveDirection, new Vector2(moveX, 0).normalized) > 0.5f)
                {
                    dashDirection = new Vector2(moveX, 0).normalized;
                    dashTimeLeft = dashDuration;
                }
            }
            lastHorizontalInputTime = Time.time;
        }

        // Detectar doble pulsación para dash en el eje vertical
        if (Input.GetButtonDown("Vertical"))
        {
            if (Time.time - lastVerticalInputTime < doubleTapTime)
            {
                // Se hace un producto vector con el vector unitario del movimiento del dash y el movimiento actual
                // Si es 1, son iguales. Misma direccion
                // Si es -1, direccion opuesta
                // Es 0.5 para que sean similares los inputs el dash
                if (Vector2.Dot(moveDirection, new Vector2(0, moveY).normalized) > 0.5f)
                {
                    dashDirection = new Vector2(0, moveY).normalized;
                    dashTimeLeft = dashDuration;
                }
            }
            lastVerticalInputTime = Time.time;
        }
         

        // Si hay tiempo de dash restante, mover al jugador en esa dirección
        if (dashTimeLeft > 0)
        {
            if (trailRenderer != null)
            {
                trailRenderer.enabled = true;
            }
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            dashTimeLeft -= Time.deltaTime;
        }
        else
        {
            if (trailRenderer != null)
            {
                trailRenderer.enabled = false;
            }

            // Movemos el objeto en la dirección indicada multiplicando por la velocidad y el tiempo entre frames (para el tema de los frames siempre Time.deltaTime)
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Rotacion del triangulo para que siempre apunte la punta porque asi se ve mas bonito 0.0
            if (moveDirection != Vector2.zero)
            {
                // Calcula el ángulo en radianes entre ejes
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                // aplicamos la rotación al objeto para que apunte en la dirección de movimiento
                // restamos 90 grados porque el sprite apunta con la puntita
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            }
        }

        // limitacion del jugador para que no salga de la pantalla
        Vector3 clampedPosition = transform.position;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x + halfWidth, screenBounds.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y + halfHeight, screenBounds.y - halfHeight);

        transform.position = clampedPosition;
    }

    // Método para detectar colisiones con las figuras (enemigos)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el objeto con el que colisiona tiene la etiqueta "Enemigo"
        if (collision.gameObject.tag == "Enemigo")
        {
            // Restar una vida
            vidas--;

            // Si las vidas llegan a 0, el jugador pierde
            if (vidas <= 0)
            {
                // Lógica de derrota (puedes agregar efectos, sonidos, etc.)
                Debug.Log("Has perdido todas las vidas.");

                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true); // Activar el panel de Game Over
                }
                /// PANTALLA DE MUERTE
                Destroy(gameObject);
                // Aquí podrías añadir la lógica de Game Over o reinicio del nivel
            }
            else
            {
                // EXPLOSION
                transform.position = Vector3.zero;
            }
        }
    }

    // Mostrar el contador de vidas en la pantalla con GUI
    void OnGUI()
    {
        // Mostrar las vidas restantes en la esquina superior izquierda
        GUI.Label(new Rect(10, 10, 100, 20), "Vidas: " + vidas);
    }
}
