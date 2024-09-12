using TMPro;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de importar esta librería si usas UI para mostrar el contador

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton Instance

    public Text enemyCountText; // Referencia al componente de texto en la UI para mostrar el conteo
    public Text liveCountText; // Son las vidas del jugador
    private int enemyCount = 0; // Contador de enemigos
    public Player player;
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
        if (player != null)
        {
            // Inicializa el texto de las vidas con el valor inicial del jugador
            UpdateLivesCount(player.vidas);
        }
        UpdateEnemyCountText();
    }

    public void UpdateLivesCount(int lives)
    {
        if(lives> 0)
        {
            liveCountText.text = "Vidas: " + lives;
        }
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
            enemyCountText.text = "Enemigos Destruidos: " + enemyCount;
        }
    }
}
