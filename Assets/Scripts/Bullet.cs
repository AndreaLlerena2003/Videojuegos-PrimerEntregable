using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        //comprobamos si la bala ya no es visible en la pantalla
        if (!IsVisibleOnScreen())
        {
            Destroy(gameObject); //si no esta, la destruimos
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
