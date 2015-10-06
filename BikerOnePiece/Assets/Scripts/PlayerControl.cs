using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;
	public Vector3 jumpVelocity;
	public GameObject slope01Jump;

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
//		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		Quaternion rot = Quaternion.LookRotation (transform.position - mousePos, Vector3.forward);
//		transform.rotation = rot;
//		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
//		rigidbody2D.angularVelocity = 0;

		if (isDrive) {
			float inputV = Input.GetAxis("Vertical");
			float inputH = Input.GetAxis("Horizontal");
//			float input = Input.GetAxis("Vertical");
			if (inputV > 0) {
				GetComponent<Rigidbody2D>().AddForce (gameObject.transform.up * speed * inputV);
				_animator.Play (Animator.StringToHash ("skeletonWalk"));
			} else {
				_animator.Play(Animator.StringToHash("skeletonStand"));
			}
			if (inputH > 0) {
				transform.Rotate ((Vector3.forward * -45) * Time.deltaTime );
			} else if (inputH < 0) {
				transform.Rotate ((Vector3.forward * 45) * Time.deltaTime );
			}
		} else {
			if(isJumb) {
				isJumb = false;
				StartCoroutine(Jumping());
			}

		}

	}

	bool isJumb = false;
	bool isDrive = true;
	void OnTriggerEnter2D(Collider2D col) {
		if (col.name == slope01Jump.name && !isJumb) {
			isJumb = true;
			isDrive = false;
			rigidbody2D.gravityScale = 4f;
			jumpVelocity = new Vector3(4f, 2f, 0f);
		}
	}

	IEnumerator Jumping() {
		jumpVelocity += new Vector3(3f, 10f, 0) * Time.deltaTime;
		transform.position += jumpVelocity * Time.deltaTime * speed;
		yield return new WaitForSeconds (Mathf.Sqrt(speed / 50));
		rigidbody2D.gravityScale = 0;
		isDrive = true;
	}
}
