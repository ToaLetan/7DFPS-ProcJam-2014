using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScript : MonoBehaviour 
{
    private const float FADE_SPEED = 0.5f;

    private GameManager gameManager = null;
    private PlayerShip player = null;

    private GameObject[] killCount = new GameObject[3];
    private GameObject[] hullIntegrity = new GameObject[3];

    private GameObject threatText = null;
    private GameObject speedText = null;
    private GameObject damageIndicator = null;
    private GameObject pauseScreen = null;
    private GameObject gameOverScreen = null;

    private int prevNumOfEnemies = -1;
    private int prevKillCount = -1;
    private int prevHullIntegrity = -1;

    private float prevSpeed = -1;
    private float damageAlpha = 0.0f;
    private float prevDamageAlpha = -1;

    private bool prevPauseState = false;
    private bool prevGameOverState = false;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        threatText = gameObject.transform.FindChild("ThreatText").gameObject;

        killCount[0] = gameObject.transform.FindChild("KillCount_Ones").gameObject;
        killCount[1] = gameObject.transform.FindChild("KillCount_Tens").gameObject;
        killCount[2] = gameObject.transform.FindChild("KillCount_Hundreds").gameObject;

        hullIntegrity[0] = gameObject.transform.FindChild("Hull_Ones").gameObject;
        hullIntegrity[1] = gameObject.transform.FindChild("Hull_Tens").gameObject;
        hullIntegrity[2] = gameObject.transform.FindChild("Hull_Hundreds").gameObject;

        speedText = gameObject.transform.FindChild("Speed_Ones").gameObject;

        damageIndicator = gameObject.transform.FindChild("HUD_Damage").gameObject;
        damageIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, damageAlpha);

        pauseScreen = gameObject.transform.FindChild("Pause_Screen").gameObject;
        pauseScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        gameOverScreen = gameObject.transform.FindChild("GameOver_Screen").gameObject;
        gameOverScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        gameManager.PlayerTakeDamage += ShowHit;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (gameManager.IsGamePaused == false)
        {
            if (prevNumOfEnemies != gameManager.NumEnemiesInScene)
                UpdateThreatLevel();

            if (prevKillCount != gameManager.PlayerNumKills)
                UpdateKillCount();

            if (prevHullIntegrity != gameManager.PlayerHull)
                UpdateHull();

            if (prevSpeed != player.currentVelocity)
                UpdateSpeed();

            if (damageAlpha > 0)
            {
                damageAlpha -= FADE_SPEED * Time.deltaTime;

                if (damageAlpha < 0)
                    damageAlpha = 0;
            }

            if (prevDamageAlpha != damageAlpha)
                UpdateDamageIndicator();

            ShowHidePauseScreen(false);
        }
        if (gameManager.IsGamePaused == true && gameManager.IsGameOver == false)
            ShowHidePauseScreen(true);

        if (gameManager.IsGameOver == true)
        {
            UpdateHull(); //Make sure that the Hull displays as 0.
            ShowHideGameOverScreen(true);
        }
	}

    private void UpdateThreatLevel()
    {
        SpriteRenderer threatLevelText = threatText.GetComponent<SpriteRenderer>();

        if (threatLevelText != null)
        {
            if (gameManager.NumEnemiesInScene <= 0)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_None");
            else if (gameManager.NumEnemiesInScene >= 1 && gameManager.NumEnemiesInScene <= 5)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_Low");
            else if (gameManager.NumEnemiesInScene >= 6 && gameManager.NumEnemiesInScene <= 15)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_Med");
            else if (gameManager.NumEnemiesInScene >= 16 && gameManager.NumEnemiesInScene <= 30)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_High");
            else if (gameManager.NumEnemiesInScene >= 31 && gameManager.NumEnemiesInScene <= 50)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_Danger");
            else if (gameManager.NumEnemiesInScene >= 51 && gameManager.NumEnemiesInScene <= 99)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_Ultimate");
            else if (gameManager.NumEnemiesInScene >= 100)
                threatLevelText.sprite = Resources.Load<Sprite>("Sprites/UI/Threat_Levels/Threat_Fucked");
        }

        prevNumOfEnemies = gameManager.NumEnemiesInScene;
    }

    private void UpdateKillCount()
    {
        int oneValue = (gameManager.PlayerNumKills / 1) % 10;
        int tenValue = (gameManager.PlayerNumKills / 10) % 10;
        int hundredValue = (gameManager.PlayerNumKills / 100) % 10;

        SpriteRenderer onesText = killCount[0].GetComponent<SpriteRenderer>();
        SpriteRenderer tensText = killCount[1].GetComponent<SpriteRenderer>();
        SpriteRenderer hundredsText = killCount[2].GetComponent<SpriteRenderer>();

        onesText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Center/Center_" + oneValue);
        tensText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Center/Center_" + tenValue);
        hundredsText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Center/Center_" + hundredValue);

        prevKillCount = gameManager.PlayerNumKills;
    }

    private void UpdateHull()
    {
        if (gameManager.PlayerHull < 0)
            gameManager.PlayerHull = 0;

        int oneValue = (gameManager.PlayerHull / 1) % 10;
        int tenValue = (gameManager.PlayerHull / 10) % 10;
        int hundredValue = (gameManager.PlayerHull / 100) % 10;

        SpriteRenderer onesText = hullIntegrity[0].GetComponent<SpriteRenderer>();
        SpriteRenderer tensText = hullIntegrity[1].GetComponent<SpriteRenderer>();
        SpriteRenderer hundredsText = hullIntegrity[2].GetComponent<SpriteRenderer>();

        onesText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Right/Right_" + oneValue);
        tensText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Right/Right_" + tenValue);
        hundredsText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Right/Right_" + hundredValue);

        prevHullIntegrity = gameManager.PlayerHull;
    }

    private void UpdateSpeed()
    {
        float speedValue = Mathf.FloorToInt((player.currentVelocity * 10) % 10);

        if (speedValue < 0)
            speedValue = 0;

        SpriteRenderer onesText = speedText.GetComponent<SpriteRenderer>();
        onesText.sprite = Resources.Load<Sprite>("Sprites/UI/Numbers_Left/Left_" + speedValue);

        prevSpeed = player.currentVelocity;
    }

    private void ShowHit()
    {
        damageAlpha = 1;
        damageIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, damageAlpha);
    }

    private void UpdateDamageIndicator()
    {
        damageIndicator.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, damageAlpha);
        prevDamageAlpha = damageAlpha;
    }

    private void ShowHidePauseScreen(bool isPaused)
    {
        if (prevPauseState != gameManager.IsGamePaused)
        {
            if (isPaused == true)
                pauseScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            else
                pauseScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        prevPauseState = gameManager.IsGamePaused;
    }

    private void ShowHideGameOverScreen(bool isGameOver)
    {
        if (prevGameOverState != gameManager.IsGameOver)
        {
            if (isGameOver == true)
                gameOverScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            else
                gameOverScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        prevGameOverState = gameManager.IsGameOver;
    }
}
