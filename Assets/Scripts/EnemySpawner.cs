using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {
	public GameObject EnemyPrefab;
	public int NoOfEnemy;

	public override void OnStartServer(){
		for (int i = 0; i < NoOfEnemy; i++) {
			var spawnPosition = new Vector3 (Random.Range (-4, 4), 1.132f, Random.Range (-4, 4));
			var spawnRotation = Quaternion.Euler (0, Random.Range (0, 180), 0);
			var enemy = (GameObject)Instantiate (EnemyPrefab, spawnPosition, spawnRotation);
			NetworkServer.Spawn (enemy);
		}
	}
}
