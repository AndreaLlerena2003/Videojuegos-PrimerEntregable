using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // referencia al prefab de la bala que será instanciada cuando se dispare
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f; // velocidad de bala

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //para q dispare solo con click izq del mouse
        {
            Shoot();
        }

    }
    void Shoot()
    {
        // creamos la bala desde punta de trangulo
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // le damos velocidad a la bala
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); //obtenemos el componente Rigidbody2D de la bala para aplicar física
        rb.velocity = transform.up * bulletSpeed; //aplicamos una velocidad a la bala en la dirección hacia la cual está apuntando el objeto -->  (osea la puntita del triangulo) 
    }
}
