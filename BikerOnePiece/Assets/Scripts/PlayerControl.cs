using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;

	Animator _animator;

	void Start() {
		_animator = GetComponent<Animator> ();	
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			_animator.SetTrigger("Attack");
		}
	}

	void FixedUpdate() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Quaternion rot = Quaternion.LookRotation (transform.position - mousePos, Vector3.forward);
		transform.rotation = rot;
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		rigidbody2D.angularVelocity = 0;

		float input = Input.GetAxis("Vertical");
		if (input > 0) {
			rigidbody2D.AddForce (gameObject.transform.up * speed * input);
			_animator.Play (Animator.StringToHash ("skeletonWalk"));
		} else {
			_animator.Play(Animator.StringToHash("skeletonStand"));
		}
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if(col.name.IndexOf("Slope") >= 0)
			transform.Translate(Vector3.up * 100 * Time.deltaTime, Space.World);
	}

	void OnTriggerExit2D(Collider2D col) {
		if(col.name.IndexOf("Slope") >= 0)
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
	}
}
