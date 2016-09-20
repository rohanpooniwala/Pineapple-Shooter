using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : NetworkBehaviour{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public ParticleSystem fireParticle;
	public float fireRate=0;
	public float speed;
	public float boost;

	float timeToFire=0;
	Rigidbody rb;
	// Use this for initialization
//	public override void OnStartServer(){
//		GameObject cam = FindObjectOfType<Camera> ();
//		if (cam.CompareTag (""))
//			cam.SetActive (false);
//	}

	void Start () {
		Camera cw=null;
		if (!isLocalPlayer) {
			cw = GetComponentInChildren<Camera> ();
			cw.enabled = false;
		} else {
			cw = GetComponentInChildren<Camera> ();
			cw.enabled = true;
		}
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;
		rb.isKinematic = false;
		if (!isLocalPlayer) {
			return;
		}
		timeToFire += Time.deltaTime;

		if (CrossPlatformInputManager.GetButton("Shoot") && timeToFire>0.1) {
			timeToFire = 0;
			CmdFire ();
		}
		if (CrossPlatformInputManager.GetButtonDown ("Boost")) {
			boost = 2;
		}
		if (CrossPlatformInputManager.GetButtonUp ("Boost")) {
			boost = 1;
		}


		float h=CrossPlatformInputManager.GetAxis("Horizontal_Move");
		float v=CrossPlatformInputManager.GetAxis("Vertical_Move");

////		rb.velocity = movement;
		Vector3 t = transform.forward;
		t.Normalize ();
		Vector3 movement = t * v* speed * boost * Time.deltaTime;

		transform.Rotate (0, h * boost, 0);
		rb.MovePosition (transform.position+movement);
	}

	[Command]
	void CmdFire(){
		fireParticle.Play ();
		GameObject bullet=(GameObject) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
		bullet.GetComponent<Rigidbody>().velocity=bulletSpawn.forward*50;
		NetworkServer.Spawn (bullet);
		Destroy (bullet, 4.0f);
	}

}
