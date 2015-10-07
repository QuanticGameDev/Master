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
	
	private Vector3 startPos;
	private Vector3 targetPos;
	
	Animator _animator;
	
	bool isJumb = false;
	bool isDrive = true;
	bool isBound = false;
	string currentSlope = "";
	
	float currentJumpHeight = 0;
	void Start() {
		isLeft = false;
		isRight = false;
		isRun = false;
		isFire = false;
		currentJumpHeight = transform.position.y;
		jumpStartVelocityY = -jumpDuration * Physics.gravity.y / 2;
		
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
			if (isRun) {
				GetComponent<Rigidbody2D> ().AddForce (gameObject.transform.up * speed * Time.deltaTime * 60);
				_animator.Play (Animator.StringToHash ("skeletonWalk"));
			} else {
				_animator.Play (Animator.StringToHash ("skeletonStand"));
			}
			if (isRight) {
				transform.Rotate ((Vector3.forward * -rotation * Mathf.Sqrt (speed / 2)) * Time.deltaTime);
			} else if (isLeft) {
				transform.Rotate ((Vector3.forward * rotation * Mathf.Sqrt (speed / 2)) * Time.deltaTime);
				
			}
		} else {
			if (isJumb) {
				isJumb = false;
				
			}
			
		}
	}
	
	public float jumpDuration = 0.5f;
	public float jumpDistance = 6f;
	
	private bool jumping = false;
	private float jumpStartVelocityY;
	
	void OnTriggerEnter2D(Collider2D col) {
		if (!isJumb) {
			if (col.name == "Slope01") {
				//if(transform.eulerAngles.)
				currentJumpHeight = transform.position.y;
				Vector3 forwardAndLeft = (transform.eulerAngles + transform.right) * jumpDistance * speed / 20f; 
				StartCoroutine(Jump(forwardAndLeft));
			} else if(col.name == "Slope02") {
				//DoJump (bound02, new Vector3(2f, 4f, 0));
			}
			
		}
		
	}
	
	void DoJump (GameObject[] bound, Vector3 jumpVel) {
		isJumb = true;
		isDrive = false;
		setTriggerBound(bound, true);
		
	}
	
	void setTriggerBound(GameObject[] bound, bool status) {
		for (int i = 0; i < bound.Length; i++) {
			bound[i].GetComponent<BoxCollider2D>().isTrigger = status;
		}
	}
	
	private IEnumerator Jump(Vector3 direction)
	{
		jumping = true;
		Vector3 startPoint = transform.position;
		Vector3 targetPoint = startPoint + direction;
		float time = 0;
		float jumpProgress = 0;
		float velocityY = jumpStartVelocityY;
		float height = startPoint.y;


		while (jumping)
		{
			jumpProgress = time / jumpDuration;
			
			if (jumpProgress > 1)
			{
				jumping = false;
				jumpProgress = 1;
			}
			
			Vector3 currentPos = Vector3.Lerp(startPoint, targetPoint, jumpProgress);
			
			currentPos.y = height;
			//currentPos.x += speed * 10f * Time.deltaTime;
			currentPos.z = 21.4f;
			transform.position = currentPos;
			
			//Wait until next frame.
			yield return null;
			
			height += velocityY * Time.deltaTime * speed / 4f;
			velocityY += Time.deltaTime * Physics.gravity.y;
			time += Time.deltaTime;
		}
		targetPoint.z = 21.4f;

		if(targetPoint.y < currentJumpHeight) {
			targetPoint = new Vector3(targetPoint.x, currentJumpHeight, targetPoint.z);
		}
		transform.position = targetPoint;
		yield break;
	}
	
	//IEnumerator Jumping() {
	/*startPos = transform.position; //Set the start
		weight += Time.deltaTime * speed; //amount
		targetPos = new Vector3(startPos.x + 3f, startPos.y, startPos.z);
		transform.position = Vector3.Lerp(startPos, 
		                                  targetPos, weight);
		yield return new WaitForSeconds (Mathf.Sqrt(speed / 500f));
		//rigidbody2D.gravityScale = 0;
		isDrive = true;
		switch (currentSlope) {
			case "Slope01" :

				setTriggerBound(bound01, false);
				break;
		}*/
	//}
}
