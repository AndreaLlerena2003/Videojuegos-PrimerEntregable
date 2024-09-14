using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    private TrailRenderer trailRenderer; // Referencia al TrailRenderer

    public Camera camera;
    public float moveSpeed = 5f; // velocidad de movimiento del jugador
    public float dashSpeed = 10f; // Velocidad del dash
    public float dashDuration = 0.2f; // Duración del dash
    public float doubleTapTime = 0.3f; // Tiempo máximo entre dos pulsaciones para que se considere doble

    private Rigidbody2D rb; // referencia al Rigidbody2D
    private Vector2 moveDirection; // entrada del movimiento
    private Camera cam; // camara principal
    private float halfWidth; // calculamos ancho y altura para manejar q no salga de la camara
    private float halfHeight;
    private float dashTimeLeft = 0f; // Tiempo restante del dash
    public float dashCooldown = 5f; // Tiempo de espera entre dashes
    public float currentDashCooldown = 0f; // Tiempo actual de cooldown
    private Vector2 dashDirection; // Dirección del dash
    private float lastHorizontalInputTime = -1f; // Tiempo de la última pulsación horizontal
    private float lastVerticalInputTime = -1f; // Tiempo de la última pulsación vertical
    public Explosion ExplosionTemplate;
    public EnemySpawner enemySpawner;
    // Vidas del jugador
    public int vidas = 5; // Inicialmente tiene 3 vidas

    // Disparo 
    private Shooting shootingScript;

    // Referencia al panel de Game Over
    public GameObject gameOverPanel;
    public UnityEngine.Quaternion originalRotation;

    void Start()
    {
        cam = Camera.main; // se asigna la camara principal
        originalRotation = transform.rotation;
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
        // Obtiene el componente de disparo
        shootingScript = GetComponent<Shooting>();

        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // Actualizar el cooldown del dash
        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
            if (currentDashCooldown < 0) currentDashCooldown = 0;

            // Actualizar el texto del cooldown en el GameManager
            GameManager.Instance.UpdateDashCooldownText(currentDashCooldown);
        }

        //obtiene la entrada del jugador para los ejes horizontales y verticales
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Se crea vector basado en la entrada del jugador --> y se normaliza (asi magnitud uno)
        moveDirection = new Vector2(moveX, moveY).normalized;

        
        // Detectar doble pulsación para dash en el eje horizontal
        if (Input.GetButtonDown("Horizontal"))
        {
            if ((Time.time - lastHorizontalInputTime < doubleTapTime) && currentDashCooldown <= 0)
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
                    currentDashCooldown = dashCooldown;
                }
            }
            lastHorizontalInputTime = Time.time;
        }

        // Detectar doble pulsación para dash en el eje vertical
        if (Input.GetButtonDown("Vertical"))
        {
            if ((Time.time - lastVerticalInputTime < doubleTapTime) && currentDashCooldown <= 0)
            {
                // Se hace un producto vector con el vector unitario del movimiento del dash y el movimiento actual
                // Si es 1, son iguales. Misma direccion
                // Si es -1, direccion opuesta
                // Es 0.5 para que sean similares los inputs el dash
                if (Vector2.Dot(moveDirection, new Vector2(0, moveY).normalized) > 0.5f)
                {
                    dashDirection = new Vector2(0, moveY).normalized;
                    dashTimeLeft = dashDuration;
                    currentDashCooldown = dashCooldown;
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
            // Actualiza el contador de vidas
            GameManager.Instance.UpdateLivesCount(vidas);
            //agregar congelamiento de enemigos
            // Llamar al método para destruir el enemigo
            Explosion newExplosion = Instantiate(
                ExplosionTemplate,
                transform.position,
                Quaternion.identity
            );
            newExplosion.Explode(Color.white);
            Destroy(collision.gameObject);
            //enemy movement y enemu spawner
            
            StartCoroutine(CongelarEnemigosYJugador(5f));
            

            // Si las vidas llegan a 0, el jugador pierde
            if (vidas <= 0)
            {
                GameManager.Instance.UpdateLivesCount(0);
                // Lógica de derrota (puedes agregar efectos, sonidos, etc.)
                Debug.Log("Has perdido todas las vidas.");

                if (gameOverPanel != null)
                {
                    enemySpawner.SetCanSpawn(false);
                    // Consigo todos los enemigos
                    List<GameObject> enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemigo"));
                    // Consigo los movimientos
                    foreach (GameObject enemigo in enemigos)
                    {
                        // Intenta de si el enemigo existe traer su movimiento para detenerlo
                        if (enemigo != null)
                        {
                            Destroy(enemigo);
                        }
                    }
                    GameManager.Instance.SetGameOverPanelStatus(true);
                    camera.backgroundColor = Color.black;
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
                transform.rotation = originalRotation; 
            }
        }
    }
    private IEnumerator CongelarEnemigosYJugador(float duracion)
    {
        // Consigo todos los enemigos
        List<GameObject> enemigos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemigo"));
        // Consigo los movimientos
        List<EnemyMovement> movimientosEnemigos = new List<EnemyMovement>();
       
        // Desactivar movimiento de enemigos
        foreach (GameObject enemigo in enemigos)
        {
            // Intenta de si el enemigo existe traer su movimiento para detenerlo
            if (enemigo != null && enemigo.TryGetComponent<EnemyMovement>(out var movimiento))
            {
                movimiento.enabled = false;
                movimientosEnemigos.Add(movimiento);
            }
        }
        /*
        // Desactivar movimiento y disparo del jugador
        float originalMoveSpeed = moveSpeed;
        float originalDashSpeed = dashSpeed;
        moveSpeed = 0f;
        dashSpeed = 0f;
        shootingScript.SetCanShoot(false);*/

        yield return new WaitForSeconds(duracion);

        // Reactivar movimiento de enemigos que aún existen
        foreach (EnemyMovement movimiento in movimientosEnemigos)
        {
            if (movimiento != null && movimiento.gameObject != null)
            {
                movimiento.enabled = true;
            }
        }
       
        /*
        // Reactivar movimiento y disparo del jugador
        moveSpeed = originalMoveSpeed;
        dashSpeed = originalDashSpeed;
        shootingScript.SetCanShoot(true);*/
    }
}
