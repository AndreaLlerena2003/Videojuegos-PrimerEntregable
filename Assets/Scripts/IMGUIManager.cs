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
    public string dashSpeed = "10";
    public string dashDuration ="0.2";
    public string livesCounter ="5";
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

            // Dash speed
            GUILayout.BeginHorizontal();
            GUILayout.Label("Dash speed", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            dashSpeed = GUILayout.TextField(dashSpeed, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            // Dash duration
            GUILayout.BeginHorizontal();
            GUILayout.Label("Dash duration", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            dashDuration = GUILayout.TextField(dashDuration, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();   
            GUILayout.EndArea();

            if (dashDuration != "")
            {
                if (float.TryParse(dashDuration, out float parsedDashDuration))
                {
                    Player.dashDuration = parsedDashDuration;
                }
            }

            if (dashSpeed != "")
            {
                if (float.TryParse(dashSpeed, out float parsedDashSpeed))
                {
                    Player.dashSpeed = parsedDashSpeed;
                }
            }

            if (playerSpeed != "")
            {
                if (float.TryParse(playerSpeed, out float parsedPlayerSpeed))
                {
                    Player.moveSpeed = parsedPlayerSpeed;
                }
                
            }

            if (livesCounter != "")
            {
                if (int.TryParse(livesCounter, out int parsedLivesCounter))
                {
                    Player.vidas = parsedLivesCounter;
                }

            }

            if (spawnInterval != "")
            {
                if (float.TryParse(spawnInterval, out float parsedSpawnInterval))
                {
                    EnemySpawner.spawnInterval = parsedSpawnInterval;
                }
                //EnemySpawner.spawnInterval = float.Parse(spawnInterval);
            }
            if (rotationSpeed != "")
            {
                if (float.TryParse(rotationSpeed, out float parsedRotationSpeed))
                {
                    EnemySpawner.rotationSpeed = parsedRotationSpeed;
                    EnemySpawner.UpdateAllEnemyWithTagConSpeed();
                }
                //EnemySpawner.rotationSpeed = float.Parse(rotationSpeed);
                //EnemySpawner.UpdateAllEnemyWithTagConSpeed();
            }

            if (explosionSpeed != "")
            {
                if (float.TryParse(explosionSpeed, out float parsedExplosionSpeed))
                {
                    ExplosionTemplate.ExplosionSpeed = parsedExplosionSpeed;

                }
            }

            if (explosionDuration != "")
            {
                if (float.TryParse(explosionDuration, out float parsedExplosionDuration))
                {
                    ExplosionTemplate.ExplosionDuration = parsedExplosionDuration;

                }

            }

            if (explosionRadius != "")
            {
                if (float.TryParse(explosionRadius, out float parsedExplosionRadius))
                {
                    ExplosionTemplate.ExplosionRadius = parsedExplosionRadius;

                }
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
