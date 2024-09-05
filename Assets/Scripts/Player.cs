using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // velocidad de movimiento del jugador
    private Rigidbody2D rb; // referencia al Rigidbody2D
    private Vector2 moveInput; // entrada del movimiento
    private Camera cam; // camara principal
    private float halfWidth; // calculamos ancho y altura para manejar q no salga de la camara
    private float halfHeight; 

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
    }

    void Update()
    {
        //obtiene la entrada del jugador para los ejes horizontales y verticales
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        //se crea vector basado en la entrada del jugador --> y se normaliza (asi magnitud uno)
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        // movemoss el objeto en la dirección indicada multiplicando por la velocidad y el tiempo entre frames (para el tema de los frames siempre Time.deltaTime)
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // rotacion del triangulo para que siempre apunte la punta porque asi se ve mas bonito 0.0
        if (moveDirection != Vector2.zero)
        {
            //calcula el ángulo en radianes entre ejes
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            // aplicamos la rotación al objeto para que apunte en la dirección de movimiento
            // restamos 90 grados porque el sprite apunta con la puntita 
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        }

        // limitacion del jugador para que no salga de la pantalla
        Vector3 clampedPosition = transform.position;
        Vector3 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x + halfWidth, screenBounds.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y + halfHeight, screenBounds.y - halfHeight);

        transform.position = clampedPosition;
    }
}
