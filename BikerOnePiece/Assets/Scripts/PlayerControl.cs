using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;
	public Vector3 jumpVelocity;

	public static bool isLeft = false;
	public static bool isRight = false;

	public static bool isRun = false;
	public static bool isFire = false;
	public float rotation = 45f;

	public GameObject[] bound01;
	public GameObject[] bound02;

	Animator _animator;

	void Start() {
		isLeft = false;
		isRight = false;
		isRun = false;
		isFire = false;
		_animator = GetComponent<Animator> ();	
	}

	void Update() {
		//if (Input.GetMouseButtonDown (0)) {
			//_animator.SetTrigger("Attack");
		//}
	}

	void FixedUpdate() {
		if (isDrive) {
			//float inputV = Input.GetAxis("Vertical");
			//Debug.Log(inputV);
//			float input = Input.GetAxis("Vertical");
			if (isRun ) {
				GetComponent<Rigidbody2D>().AddForce (gameObject.transform.up * speed * Time.deltaTime * 30);
				_animator.Play (Animator.StringToHash ("skeletonWalk"));
			} else {
				_animator.Play(Animator.StringToHash("skeletonStand"));
			}
			if (isRight) {
				transform.Rotate ((Vector3.forward * -rotation * Mathf.Sqrt(speed/2)) * Time.deltaTime );
			} else if (isLeft) {
				transform.Rotate ((Vector3.forward * rotation  * Mathf.Sqrt(speed/2)) * Time.deltaTime );
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
	bool isBound = false;
	string currentSlope = "";
	void OnTriggerEnter2D(Collider2D col) {
		if (!isJumb) {

			if (col.name == "Slope01") {
				currentSlope = col.name;
				DoJump (bound01, new Vector3(4f, 2f, 0));
			} else if(col.name == "Slope02") {
				DoJump (bound02, new Vector3(2f, 4f, 0));
			}

		}

	}

	void DoJump (GameObject[] bound, Vector3 jumpVel) {
		isJumb = true;
		isDrive = false;
		setTriggerBound(bound, true);
		rigidbody2D.gravityScale = 4f;
		jumpVelocity = jumpVel;
	}

	void setTriggerBound(GameObject[] bound, bool status) {
		for (int i = 0; i < bound.Length; i++) {
			bound[i].GetComponent<BoxCollider2D>().isTrigger = status;
		}
	}
	IEnumerator Jumping() {
		jumpVelocity += new Vector3(3f, 10f, 0) * Time.deltaTime;
		transform.position += jumpVelocity * Time.deltaTime * speed;
		yield return new WaitForSeconds (Mathf.Sqrt(speed / 500f));
		rigidbody2D.gravityScale = 0;
		isDrive = true;
		switch (currentSlope) {
			case "Slope01" :

				setTriggerBound(bound01, false);
				break;
		}
	}
}
