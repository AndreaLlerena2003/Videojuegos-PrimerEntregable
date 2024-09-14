using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyType> enemyTypes; // Lista de tipos de enemigos
    public float spawnInterval = 2f; // Intervalo de aparici�n
    private Camera cam; // C�mara principal 
    public bool canSpawn=true;
    public float rotationSpeed;
    void Start()
    {
        cam = Camera.main; // Obt�n la c�mara principal
        StartCoroutine(SpawnEnemies()); // Inicia la corrutina de generaci�n de enemigos
    }

    IEnumerator SpawnEnemies()
    {
        while (canSpawn)
        {
            SpawnEnemy(); // Genera un enemigo
            yield return new WaitForSeconds(spawnInterval); // Espera antes de generar el siguiente enemigo
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyTypes.Count); // Selecciona un tipo de enemigo aleatorio
        EnemyType selectedEnemyType = enemyTypes[randomIndex];

        // Obtener el tama�o del Collider2D para validar el spawn
        float widthInViewport = 0;
        float heightInViewport = 0;
        Collider2D baseCollider = selectedEnemyType.enemyObject.GetComponent<Collider2D>();
        if (baseCollider != null)
        {
            widthInViewport = baseCollider.bounds.size.x / (cam.orthographicSize * 2f * cam.aspect);
            heightInViewport = baseCollider.bounds.size.y / (cam.orthographicSize * 2f);
        }

        Vector2 spawnPosition = GetRandomSpawnPosition(widthInViewport, heightInViewport);

        if (IsPositionOnScreen(spawnPosition, widthInViewport, heightInViewport))
        {

            GameObject newEnemy = CreateEnemy(selectedEnemyType.enemyObject, spawnPosition);
            newEnemy.GetComponent<SpriteRenderer>().color = GetRandomColor();

            EnemyMovement movement = newEnemy.AddComponent<EnemyMovement>();
            Debug.Log("Speed: " + selectedEnemyType.speed);
            movement.speed = selectedEnemyType.speed;
            movement.rotationSpeed = rotationSpeed;
     
        }
    }

    GameObject CreateEnemy(GameObject baseObject, Vector2 position)
    {
        GameObject enemy = new GameObject("Enemy");
        enemy.tag = "Enemigo";
        SpriteRenderer spriteRenderer = enemy.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = baseObject.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.sortingOrder = 1;
        enemy.transform.position = position;
        enemy.transform.localScale = baseObject.transform.localScale;
        Rigidbody2D rb = baseObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Rigidbody2D enemyRb = enemy.AddComponent<Rigidbody2D>();
            enemyRb.gravityScale = rb.gravityScale;
            enemyRb.mass = rb.mass;
        }

        Collider2D baseCollider = baseObject.GetComponent<Collider2D>();
        if (baseCollider != null)
        {
            if (baseCollider is CapsuleCollider2D)
            {
                CapsuleCollider2D capsuleCollider = enemy.AddComponent<CapsuleCollider2D>();
                CapsuleCollider2D baseCapsuleCollider = (CapsuleCollider2D)baseCollider;
                capsuleCollider.size = baseCapsuleCollider.size;
                capsuleCollider.offset = baseCapsuleCollider.offset;
            }
            else if (baseCollider is CircleCollider2D)
            {
                CircleCollider2D circleCollider = enemy.AddComponent<CircleCollider2D>();
                CircleCollider2D baseCircleCollider = (CircleCollider2D)baseCollider;
                circleCollider.radius = baseCircleCollider.radius;
                circleCollider.offset = baseCircleCollider.offset;
            }
            else if (baseCollider is PolygonCollider2D)
            {
                PolygonCollider2D polygonCollider = enemy.AddComponent<PolygonCollider2D>();
                PolygonCollider2D basePolygonCollider = (PolygonCollider2D)baseCollider;
                polygonCollider.points = basePolygonCollider.points;
                polygonCollider.offset = basePolygonCollider.offset;
            }

        
        }
        
        return enemy;
    }

    Vector2 GetRandomSpawnPosition(float widthInViewport, float heightInViewport)
    {
        // Ajustar el rango para que el enemigo se genere completamente dentro de la pantalla
        float minX = widthInViewport / 2f;
        float maxX = 1 - widthInViewport / 2f;
        float minY = heightInViewport / 2f;
        float maxY = 1 - heightInViewport / 2f;

        float screenX = UnityEngine.Random.Range(minX, maxX);
        float screenY = UnityEngine.Random.Range(minY, maxY);

        return cam.ViewportToWorldPoint(new Vector2(screenX, screenY));
    }

    bool IsPositionOnScreen(Vector2 position, float widthInViewport, float heightInViewport)
    {
        // Comprobar que el enemigo est� completamente dentro de los l�mites de la pantalla
        Vector3 viewportPoint = cam.WorldToViewportPoint(position);
        return viewportPoint.x >= widthInViewport / 2f && viewportPoint.x <= 1 - widthInViewport / 2f &&
               viewportPoint.y >= heightInViewport / 2f && viewportPoint.y <= 1 - heightInViewport / 2f;
    }

    Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }
    public void SetCanSpawn(bool spawn)
    {
        canSpawn = spawn;
    }

    public void UpdateAllEnemyWithTagConSpeed() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (GameObject enemy in enemies) {
            EnemyMovement enemyController = enemy.GetComponent<EnemyMovement>();
            if (enemyController != null) {
                enemyController.rotationSpeed = rotationSpeed;
            }
        }
    }


}