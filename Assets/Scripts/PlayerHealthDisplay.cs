using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerHealthDisplay : NetworkBehaviour {
	RectTransform playerHealth;
	Health hp;
	// Use this for initialization
	void Start () {
		playerHealth = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Health[] player = GameObject.FindObjectsOfType<Health>();
		print(player.Length);
		for (int i = 0; i < player.Length; i++) {
			if(player[i].IsLocal())
				hp = player [i];
		}
		if (hp != null) {
			playerHealth.sizeDelta = new Vector2 (hp.hp, playerHealth.sizeDelta.y);
			print (hp.hp);
		} else {
			playerHealth.sizeDelta = new Vector2 (100, playerHealth.sizeDelta.y);
		}
	}
}
