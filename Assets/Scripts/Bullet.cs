using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject bloodEffect;
	void OnCollisionEnter(Collision collision){

		var hit = collision.gameObject;
		var health = hit.GetComponent<Health> ();
		if (health != null) {
			Instantiate (bloodEffect, collision.contacts [0].point, transform.rotation);
			health.TakeDamage (10);
		}
		Destroy (gameObject);
	}
}
