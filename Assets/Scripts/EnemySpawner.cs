using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyType> enemyTypes; //objeto tipo lista de enemigos
    public float spawnInterval = 2f; //intervalo de spawn
    private Camera cam;//camara

    void Start()
    {
        cam = Camera.main;//camara principal
        StartCoroutine(SpawnEnemies()); //generacion de enemigos segun intervalos
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();//creacion de enemigo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyTypes.Count); //se seleciona un tipo de enemigo random de la lista
        EnemyType selectedEnemyType = enemyTypes[randomIndex];

        // Obtener el tamaño del Collider2D para validar el spawn -- para asegurar que el enemigo no se genere fuera de los límites visibles de la pantalla
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
            movement.speed = selectedEnemyType.speed;
        }
    }

    GameObject CreateEnemy(GameObject baseObject, Vector2 position)
    {
        GameObject enemy = new GameObject("Enemy");
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
        // Ajuste del rango para garantizar que el enemigo se genere completamente dentro de la pantalla
        float screenX = Random.Range(widthInViewport / 2f, 1 - widthInViewport / 2f);
        float screenY = Random.Range(heightInViewport / 2f, 1 - heightInViewport / 2f);
        return cam.ViewportToWorldPoint(new Vector2(screenX, screenY));
    }

    bool IsPositionOnScreen(Vector2 position, float widthInViewport, float heightInViewport)
    {
        // Comprobar que el enemigo esté completamente dentro de los límites de la pantalla
        Vector3 viewportPoint = cam.WorldToViewportPoint(position);
        return viewportPoint.x >= widthInViewport / 2f && viewportPoint.x <= 1 - widthInViewport / 2f &&
               viewportPoint.y >= heightInViewport / 2f && viewportPoint.y <= 1 - heightInViewport / 2f;
    }

    Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
