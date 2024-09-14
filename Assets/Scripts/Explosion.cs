using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private List<Transform> circles = new();
    public float ExplosionRadius = 2f;
    public float ExplosionSpeed = 2f;
    public float ExplosionDuration = 2f;

    void Awake()
    {
        foreach (Transform child in transform)
            circles.Add(child);
    }

    public void Explode(Color color)
    {
        foreach (Transform circle in circles)
        {
            PositionCircle(circle);
            circle.GetComponent<SpriteRenderer>().color = color;
        }

        foreach (Transform circle in circles)
            StartCoroutine(MoveCircle(circle));

        Destroy(gameObject, ExplosionDuration);
    }

    void PositionCircle(Transform circle)
    {
        Vector3 direction = circle.position - transform.position;
        direction.Normalize();
        Vector3 movement = new(
            direction.x * ExplosionRadius,
            direction.y * ExplosionRadius,
            0f
        );
        circle.position += movement;
    }

    IEnumerator MoveCircle(Transform circle)
    {
        Vector3 direction = circle.position - transform.position;
        direction.Normalize();
        Vector3 movement = new(
            direction.x * ExplosionSpeed,
            direction.y * ExplosionSpeed,
            0f
        );

        float elapsedTime = 0f;
        while (elapsedTime <= ExplosionDuration)
        {
            circle.position += movement * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
