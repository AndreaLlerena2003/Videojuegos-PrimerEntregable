using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// marcamos la clase como Serializable para que pueda ser editada en el Inspector de Unity
[System.Serializable]
public class EnemyType
{
    public string name;
    public float speed;
    public Explosion explotion;
    public GameObject enemyObject; //aqui le mandaremos los objetos q tendra que duplicar el Spawner

    //constructor para inicializar los valores del tipo de enemigo
    public EnemyType(string name, float speed, GameObject enemyObject, Explosion ExplosionTemplate)
    {
        this.name = name;
        this.speed = speed;
        this.enemyObject = enemyObject;
        this.explotion = ExplosionTemplate;
   
    }

    /*void explotionAnimation()
    {
        GameObject explotion = Instantiate(explotion, transform.position, Quaternion.identity);
        Destroy(explotion, 5f);
        Destroy(gameObject);
    }*/
}
