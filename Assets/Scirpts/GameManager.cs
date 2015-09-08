using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public int baseMaxHealth =  100;
    //public int defCapturableConstructionMaxHealth = 10;
    public int pinballLauncherMaxHealth = 5;
    public int bumperMaxHealth = 5;
	public int minionMaxHealth = 5;

	public List<KingdomBase> kingdoms;

    private bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; } }

    public bool pinballInMap = false;

    public Canvas canvasHUD;
    private Animator gameOverAnim;

    public float restartInterval = 3.0f;
    private float restartTimer;
	private void Start()
	{
		Debug.Assert(baseMaxHealth > 0);
        Debug.Assert(kingdoms != null);
        Debug.Assert(canvasHUD != null);
        gameOverAnim = canvasHUD.GetComponent<Animator>();
        Debug.Assert(gameOverAnim != null);

        restartInterval = Mathf.Clamp(restartInterval, 1f, 10f);
        restartTimer = 0f;

        pinballInMap = false;
    }


	public void OnBaseDestroy()
	{
        //Debug.Log("base destroyed");
        
        if (kingdoms.FindAll((KingdomBase k) => k.currentHealth > 0).Count == 1)
        {
            //game over
            isGameOver = true;

            //game over animation
            gameOverAnim.SetTrigger("GameOverTrigger");
        }
	}

    

    private void Update()
    {
        if (IsGameOver)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartInterval)
                Application.LoadLevel("start");
        }
    }
}
