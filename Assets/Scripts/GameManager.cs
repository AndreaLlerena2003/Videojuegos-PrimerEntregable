using UnityEngine;
using UnityEngine.UI; // Asegúrate de importar esta librería si usas UI para mostrar el contador

public class GameManager : MonoBehaviour
{
    public GameManager gameManager;
    public Text enemyCountText; // Referencia al componente de texto en la UI para mostrar el conteo
    private int enemyCount = 0; // Contador de enemigos

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
