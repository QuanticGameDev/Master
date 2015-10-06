using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public float speed;
	public Vector3 jumpVelocity;
	public GameObject slope01Jump;

	public static bool isLeft = false;
	public static bool isRight = false;

	public static bool isRun = false;
	public static bool isFire = false;
	public float rotation = 45f;

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
