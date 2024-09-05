using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// marcamos la clase como Serializable para que pueda ser editada en el Inspector de Unity
[System.Serializable]
public class EnemyType
{
    public string name;
    public float speed;
    public GameObject enemyObject; //aqui le mandaremos los objetos q tendra que duplicar el Spawner


    //constructor para inicializar los valores del tipo de enemigo
    public EnemyType(string name, float speed, GameObject enemyObject)
    {
        this.name = name;
        this.speed = speed;
        this.enemyObject = enemyObject;
    }
}
