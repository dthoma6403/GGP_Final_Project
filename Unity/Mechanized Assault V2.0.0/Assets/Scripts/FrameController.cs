﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameController : MonoBehaviour {

	//Camera
	public GameObject PlayerCamera;
	private float MouseSensitivity = 175.0f;
	private float CameraLimitSensitivity = 2.0f;
	[SerializeField] public float CameraRotationLimit = 0f;

	//Speed and movement
	[SerializeField]public float Gspeed = 4500;
	public Rigidbody RB;
	float tempY = 0f;

	//Spawns
	public GameObject LeftWeapon_Spawn;
	public GameObject RightWeapon_Spawn;

	//LeftWeapons
	public GameObject L_Assault_Rifle;
	public GameObject L_SMG;
	public GameObject L_Sniper_Rifle;

	//RightWeapons
	public GameObject R_Assault_Rifle;
	public GameObject R_SMG;
	public GameObject R_Sniper_Rifle;

	// MISC.
	public bool LockedOn = false;
	[SerializeField]public bool OnGround=true;
	[SerializeField] public bool canBoost = true;
	public GameObject enemy;
	public GameObject TestSpawn;

	void Start () {
		LeftWeapon_Spawn = GameObject.FindGameObjectWithTag ("LWS");
		RightWeapon_Spawn = GameObject.FindGameObjectWithTag ("RWS");
		enemy = GameObject.FindGameObjectWithTag ("Enemy");
		TestSpawn = GameObject.FindGameObjectWithTag ("TestSpawn");
		SpawnLeftWeapon ();
		SpawnRightWeapon ();
	}

	void Update () {

		OnGroundCheck ();

		float moveX = Input.GetAxis ("Horizontal") * Gspeed * Time.deltaTime;
		float moveZ = Input.GetAxis ("Vertical") * Gspeed * Time.deltaTime;
		float moveY = Input.GetAxis ("Flight") * Gspeed * Time.deltaTime;
		//float boost = Input.GetAxis ("Jump") * Gspeed + 2000 * Time.deltaTime;

		if (Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) {
			Debug.Log ("Bout to call input");
			Move(moveX, moveZ);
			Debug.Log ("Just called input");
		}

		if (Input.GetButton ("Flight")) {
			Fly (moveY);
		}

		if (Input.GetButtonDown ("Jump")) {
			StartCoroutine(Boost ());
		}
		CameraRotationLimit -= Input.GetAxis ("Mouse Y") * CameraLimitSensitivity;
		CameraRotationLimit = Mathf.Clamp (CameraRotationLimit, -10, 17);

		this.gameObject.transform.rotation *= Quaternion.Euler (0.0f, Input.GetAxis ("Mouse X") * Time.deltaTime * MouseSensitivity, 0.0f);
		PlayerCamera.gameObject.transform.localEulerAngles = new Vector3 (CameraRotationLimit, 0.0f, 0.0f);

		if (Input.GetButtonDown ("LockOn")) {
			tempY = transform.rotation.eulerAngles.y;
			Debug.Log("the y rotation is" + tempY);
			RaycastHit hit = new RaycastHit();
			//Physics.Raycast (transform.position, transform.forward, out hit);
			//Vector3 fwd = transform.TransformDirection(Vector3.forward);
			//if(Physics.Raycast(transform.forward, fwd, 500)){
			if (Physics.Raycast (transform.position, transform.forward, out hit) && LockedOn == false) {
				if (hit.collider.gameObject.tag == "Enemy") {
					Debug.Log ("I hit the enemy, I think...");
					//this.transform.localRotation.y = enemy.transform.position;
					//transform.rotation *= Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
					LockedOn = true;
				}
			} else {

				ResetRotations ();
				//PlayerCamera.transform.rotation = Quaternion.Euler (0.0f, this.gameObject.transform.rotation.y, 0.0f);
			}
		}

		if (LockedOn == true) {
			Debug.Log ("LockOn is true");
			transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
		}
	}

	void Move(float movex, float movez){
		//Vector3 movement = new Vector3 (movex, 0.0f, movez);
		transform.position += transform.forward * Time.deltaTime * movez;
		transform.position += transform.right * Time.deltaTime * movex;
	}

	void Fly(float movey){
		transform.position += transform.up * Time.deltaTime * movey;
	}

	void OnGroundCheck(){

	}

	void OnTriggerStay (Collider other){
		if (other.tag == "Terrain") {
			OnGround = true;
		} else {
			OnGround = false;
		}
	}

	public IEnumerator Boost(){
		if (canBoost == true) {
			canBoost = false;
			Gspeed += 9000;
			yield return new WaitForSeconds (0.5f);
			Gspeed -= 9000;
			StartCoroutine (BoostBuffer ());
		}

	}

	public void ResetRotations(){
		Debug.Log ("Lockon is false");
		LockedOn = false;
		Debug.Log("the y rotation for reset is" + tempY);
		transform.rotation = Quaternion.Euler (0.0f, tempY, 0.0f);
	}

	public IEnumerator BoostBuffer(){
		yield return new WaitForSeconds (0.70f);
		canBoost = true;
	}

	public void SpawnLeftWeapon(){
		switch(PlayerPrefs.GetInt("LeftWeaponChoice")){

		case 0:
			GameObject L_SMG_I = Instantiate (L_SMG);
			Debug.Log ("L Weapon Spawned");
			L_SMG_I.transform.position = LeftWeapon_Spawn.transform.position;
			Debug.Log ("L Weapon positon Changed");
			L_SMG_I.transform.parent = transform;
			break;

		case 1:
			GameObject L_AssaultRifle_I = Instantiate (L_Assault_Rifle);
			Debug.Log ("L Weapon Spawned");
			L_AssaultRifle_I.transform.position = LeftWeapon_Spawn.transform.position;
			Debug.Log ("L Weapon positon Changed");
			L_AssaultRifle_I.transform.parent = transform;
			break;

		case 2:
			GameObject L_SniperRifle_I = Instantiate (L_Sniper_Rifle);
			Debug.Log ("L Weapon Spawned");
			L_SniperRifle_I.transform.position = LeftWeapon_Spawn.transform.position;
			Debug.Log ("L Weapon positon Changed");
			L_SniperRifle_I.transform.parent = transform;
			break;

		}
	}

	public void SpawnRightWeapon(){
		switch(PlayerPrefs.GetInt("RightWeaponChoice")){

		case 0:
			GameObject R_SMG_I = Instantiate (R_SMG);
			Debug.Log ("R Weapon Spawned");
			R_SMG_I.gameObject.transform.position = RightWeapon_Spawn.gameObject.transform.position;
			Debug.Log ("R Weapon positon Changed");
			R_SMG_I.transform.parent = transform;
			break;

		case 1:
			GameObject R_AssaultRifle_I = Instantiate (R_Assault_Rifle);
			Debug.Log ("R Weapon Spawned");
			R_AssaultRifle_I.gameObject.transform.position = RightWeapon_Spawn.gameObject.transform.position;
			Debug.Log ("R Weapon positon Changed");
			R_AssaultRifle_I.transform.parent = transform;
			break;

		case 2:
			GameObject R_SniperRifle_I = Instantiate (R_Sniper_Rifle);
			Debug.Log ("R Weapon Spawned");
			R_SniperRifle_I.gameObject.transform.position = RightWeapon_Spawn.gameObject.transform.position;
			Debug.Log ("R Weapon positon Changed");
			R_SniperRifle_I.transform.parent = transform;
			break;

		}
	}
}