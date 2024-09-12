using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject topLimit;
    public GameObject bottomLimit;
    public GameObject leftLimit;
    public GameObject rightLimit;

    public float margin = 0.1f; // Margen adicional para evitar que los objetos se vean en el borde

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Asigna la cámara principal si no se ha asignado
        }

        AdjustBounds();
    }

    private void Update()
    {
        AdjustBounds(); // Ajusta los límites en cada frame para adaptarse a cualquier cambio en el tamaño de la pantalla
    }

    private void AdjustBounds()
    {
        if (mainCamera == null || topLimit == null || bottomLimit == null || leftLimit == null || rightLimit == null)
        {
            Debug.LogError("Camera or bounds limits are not assigned.");
            return;
        }

        // Calcula el tamaño de la cámara en unidades del mundo
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Ajusta los límites con margen adicional
        SetLimit(topLimit, new Vector2(camWidth + margin * 2, margin), new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + camHeight / 2 + margin, 0));
        SetLimit(bottomLimit, new Vector2(camWidth + margin * 2, margin), new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - camHeight / 2 - margin, 0));
        SetLimit(leftLimit, new Vector2(margin, camHeight + margin * 2), new Vector3(mainCamera.transform.position.x - camWidth / 2 - margin, mainCamera.transform.position.y, 0));
        SetLimit(rightLimit, new Vector2(margin, camHeight + margin * 2), new Vector3(mainCamera.transform.position.x + camWidth / 2 + margin, mainCamera.transform.position.y, 0));
    }

    private void SetLimit(GameObject limit, Vector2 size, Vector3 position)
    {
        if (limit != null)
        {
            BoxCollider2D collider = limit.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.size = size;
                limit.transform.position = position;
            }
            else
            {
                Debug.LogError("The GameObject does not have a BoxCollider2D component.");
            }
        }
        else
        {
            Debug.LogError("The GameObject for the limit is not assigned.");
        }
    }
}
