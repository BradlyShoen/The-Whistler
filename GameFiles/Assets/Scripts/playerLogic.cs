﻿//This script is used to control the player stats like health, stamina, battery power, etc. It is also used to contol the UI elements of the game.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class playerLogic : MonoBehaviour {
	
	public GameObject mainCanvas;
	public GameObject pausePanel;
	
	private GameObject playerObject;
	private playerController playerMover;
	private bool currentlyPaused = false;
	
	public float playerHealth = 100f;
	public float playerStamina = 100f;
	public float staminaIncreaseRate = 50.0f;
	public float runningDecreaseRate = 10.0f;
	
	private const float PLAYERMAXHEALTH = 100f;
	private const float PLAYERMAXSTAMINA = 100f;
	
	Vector2 healthBarPos = new Vector2(20,Screen.height - 40);
	Vector2 healthBarSize = new Vector2(250,20);
	public Texture2D healthBarEmpty;
	public Texture2D healthBarFull;
	
	Vector2 staminaBarPos = new Vector2(20,Screen.height - 20);
	Vector2 staminaBarSize = new Vector2(50,10);
	public Texture2D staminaBarEmpty;
	public Texture2D staminaBarFull;
	

	void Start () {
		playerObject = GameObject.FindWithTag("Player");
		playerMover = playerObject.GetComponent<playerController>();
		mainCanvas = GameObject.Find("MainCanvas");

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		if(Input.GetKeyDown("`")){
			PauseUnpause();
		}
		
		if(currentlyPaused){
			pausePanel.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;

			playerMover.enabled = false;
			Time.timeScale = 0.0f;
		}else{
			pausePanel.SetActive(false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			playerMover.enabled = true;
			Time.timeScale = 1.0f;
			
		}
		
		if(playerHealth > PLAYERMAXHEALTH){
			playerHealth = PLAYERMAXHEALTH;
		}else if(playerHealth < 0){
			playerHealth = 0;
		}
		
		if(playerStamina > PLAYERMAXSTAMINA){
			playerStamina = PLAYERMAXSTAMINA;
		}else if(playerStamina < 0){
			playerStamina = 0;
		}
	}
	
	public void RestartCurrentScene(){
		Scene loadedLevel = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (loadedLevel.buildIndex);
	}
	
	public void ExitGame(){
		Application.Quit();
	}
	
	public void PauseUnpause(){
		if(currentlyPaused){
			currentlyPaused = false;
		}else{
			currentlyPaused = true;
		}
	}

	public void TakeDamage(float dmg){
		playerHealth = playerHealth - dmg;
	}
	
	public IEnumerator decreaseStamina(){
		playerStamina -=  runningDecreaseRate * Time.deltaTime;
		yield return 0;
	}
	
	public IEnumerator increaseStamina(){
		playerStamina +=  staminaIncreaseRate * Time.deltaTime;
		yield return 0;
	}
	
	public void OnGUI(){
		//Healthbar
		GUI.BeginGroup (new Rect (healthBarPos.x, healthBarPos.y, healthBarSize.x, healthBarSize.y));
			GUI.Box (new Rect (0,0, healthBarSize.x, healthBarSize.y),healthBarEmpty);
			GUI.DrawTexture(new Rect (0,0, healthBarSize.x, healthBarSize.y), healthBarEmpty, ScaleMode.StretchToFill, true, 10.0F);

			// draw the filled-in part:
			GUI.BeginGroup (new Rect (0, 0, healthBarSize.x * (playerHealth/PLAYERMAXHEALTH), healthBarSize.y));
			GUI.DrawTexture(new Rect (0,0, healthBarSize.x, healthBarSize.y), healthBarFull, ScaleMode.StretchToFill, true, 10.0F);
			GUI.EndGroup ();

		GUI.EndGroup ();
		
		//Staminabar
		GUI.BeginGroup (new Rect (staminaBarPos.x, staminaBarPos.y, staminaBarSize.x, staminaBarSize.y));
			GUI.Box (new Rect (0,0, staminaBarSize.x, staminaBarSize.y),healthBarEmpty);
			GUI.DrawTexture(new Rect (0,0, staminaBarSize.x, staminaBarSize.y), staminaBarEmpty, ScaleMode.StretchToFill, true, 10.0F);

			// draw the filled-in part:
			GUI.BeginGroup (new Rect (0, 0, staminaBarSize.x * (playerStamina/PLAYERMAXSTAMINA), staminaBarSize.y));
			GUI.DrawTexture(new Rect (0,0, staminaBarSize.x, staminaBarSize.y), staminaBarFull, ScaleMode.StretchToFill, true, 10.0F);
			GUI.EndGroup ();

		GUI.EndGroup ();
	}
}
