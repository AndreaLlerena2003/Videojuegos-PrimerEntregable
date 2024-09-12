using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int points = 10;
    public GameManager gameManager;
    void Update()
    {
        //comprobamos si la bala ya no es visible en la pantalla
        if (!IsVisibleOnScreen())
        {
            Destroy(gameObject); //si no esta, la destruimos
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si la bala ha chocado con un enemigo
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // Llamar al método para destruir el enemigo
            Destroy(collision.gameObject);

            // Llamar al método para destruir la bala
            Destroy(gameObject);

            // Llamar al método para incrementar el contador en el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementEnemyCount();
            }
        }
        else
        {
            // Si la bala colisiona con algo que no sea un enemigo, solo destrúyela
            Destroy(gameObject);
        }
    }
    bool IsVisibleOnScreen()
    {
        //obtenemos la posición del objeto desde coordenadas del mundo a coordenadas de la pantalla
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        //vemos si la bala esta dentro de los límites de la pantalla en los ejes 
        // screenPosition.x e y deben estar entre 0 y 1 para estar en pantalla
        return screenPosition.x >= 0 && screenPosition.x <= 1 &&
               screenPosition.y >= 0 && screenPosition.y <= 1;
    }

}
