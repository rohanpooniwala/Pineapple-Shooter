using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	
	public const int maxHealth = 100;
	public bool destroyOnDeath=false;

	private NetworkStartPosition[] spawnPoints;

	void Start(){
		if (isLocalPlayer) {
			spawnPoints=FindObjectsOfType<NetworkStartPosition>();
			hp = maxHealth;
		}
	}


	public int hp = maxHealth;
	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	public void TakeDamage(int amount){
		if (!isServer)
			return;

		currentHealth -= amount;

		if (currentHealth <= 0) {
			if (!destroyOnDeath) {
				currentHealth = maxHealth;
				RpcRespawn ();
			} else
				Destroy (gameObject);
		}

	}

	public bool IsLocal(){
		return isLocalPlayer;
	}

	void OnChangeHealth(int currentHealth){
		healthBar.sizeDelta = new Vector2 (currentHealth, healthBar.sizeDelta.y);
		hp = currentHealth;
	}

	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			Vector3 spawnPoint = Vector3.zero;
			if (spawnPoints != null && spawnPoints.Length>0) {
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}
			transform.position = spawnPoint;
		}
	}

}
