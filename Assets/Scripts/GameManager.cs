using TMPro;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de importar esta librería si usas UI para mostrar el contador

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton Instance

    public TextMeshProUGUI enemyCountText; // Referencia al componente de texto en la UI para mostrar el conteo
    private int enemyCount = 0; // Contador de enemigos

    void Awake()
    {
        // Implementar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruir instancias duplicadas
        }
    }

    void Start()
    {
        UpdateEnemyCountText();
    }

    public void IncrementEnemyCount()
    {
        enemyCount++;
        UpdateEnemyCountText();
    }

    void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Enemies Destroyed: " + enemyCount;
        }
    }
}
