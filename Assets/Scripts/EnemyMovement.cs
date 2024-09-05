using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed; //velocidad de movimiento del enemigo
    private Vector2 direction; //direcci�n
    private Collider2D collider2D; //collider q tiene el enemigo
    private float colliderWidth; //ancho y alto de collider para manejo de rebote en bordes
    private float colliderHeight;

    void Start()
    {
        // se inicializa la direcci�n con un vector aleatorio normalizado dentro de un c�rculo unitario
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
        //mover el enemigo en la direcci�n actual multiplicada por la velocidad y el tiempo transcurrido desde el �ltimo frame
        transform.Translate(direction * speed * Time.deltaTime);

        // ver si el enemigo ha alcanzado los l�mites de la pantalla
        CheckBounds();
    }

    void CheckBounds()
    {
        //  --> convertir la posici�n del enemigo de coordenadas de mundo a coordenadas de pantalla (Viewport)
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        // calcular el tama�o del collider en t�rminos de Viewport
        float widthInViewport = colliderWidth / Camera.main.orthographicSize / Camera.main.aspect;
        float heightInViewport = colliderHeight / Camera.main.orthographicSize;

        //  Verificar si el enemigo ha alcanzado los l�mites horizontales de la pantalla (incluyendo el tama�o del collider)
        if (pos.x <= widthInViewport / 2f || pos.x >= 1 - widthInViewport / 2f)
        {
            //invierte direccion para simular rebote
            direction.x = -direction.x;
        }

        //lo mismo pero con los limites verticales
        if (pos.y <= heightInViewport / 2f || pos.y >= 1 - heightInViewport / 2f)
        {
            direction.y = -direction.y;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // invertir la direcci�n al colisionar con otro objeto --> para esos casos q choque con otro enemigo y asi no le de lag
        direction = -direction; // -> rebote
    }
}
