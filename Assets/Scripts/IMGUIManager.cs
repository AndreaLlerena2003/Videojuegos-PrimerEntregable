using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUIManager : MonoBehaviour
{
    public bool isShown = false;

    public Player Player;
    public EnemySpawner EnemySpawner;
    public Explosion ExplosionTemplate;
    
    public string playerSpeed = "5";
    public string livesCounter = "5";
    public string spawnInterval = "2";
    public string rotationSpeed = "20";
    public string explosionSpeed = "5";
    public string explosionRadius = "0.5";
    public string explosionDuration = "2";


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) isShown = !isShown;
    }

    private void OnGUI()
    {
        if (isShown)
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 250));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Box("Game settings");
            
            // Player speed
            GUILayout.BeginHorizontal();
            GUILayout.Label("Player speed", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            playerSpeed = GUILayout.TextField(playerSpeed, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Lives counter
            GUILayout.BeginHorizontal();
            GUILayout.Label("Lives counter", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            livesCounter = GUILayout.TextField(livesCounter, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Spawn interval
            GUILayout.BeginHorizontal();
            GUILayout.Label("Spawn interval", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            spawnInterval = GUILayout.TextField(spawnInterval, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Rotation speed
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation speed", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            rotationSpeed = GUILayout.TextField(rotationSpeed, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();   
            GUILayout.EndArea();

            if (playerSpeed != "")
            {
                Player.moveSpeed = float.Parse(playerSpeed);
            }

            if (livesCounter != "")
            {
                Player.vidas = int.Parse(livesCounter);
            }

            if (spawnInterval != "")
            {
                EnemySpawner.spawnInterval = float.Parse(spawnInterval);
            }

            if (explosionSpeed != "")
            {
                ExplosionTemplate.ExplosionSpeed = float.Parse(explosionSpeed);
            }

            if (explosionDuration != "")
            {
                ExplosionTemplate.ExplosionDuration = float.Parse(explosionDuration);
            }

            if (explosionRadius != "")
            {
                ExplosionTemplate.ExplosionRadius = float.Parse(explosionRadius);
            }
        }

        if (isShown)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 210, 10, 200, 250));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Box("Explosion settings");

            // Explosion speed
            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            explosionSpeed = GUILayout.TextField(explosionSpeed, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Explosion radius
            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            explosionRadius = GUILayout.TextField(explosionRadius, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Explosion duration
            GUILayout.BeginHorizontal();
            GUILayout.Label("Duration", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            explosionDuration = GUILayout.TextField(explosionDuration, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
