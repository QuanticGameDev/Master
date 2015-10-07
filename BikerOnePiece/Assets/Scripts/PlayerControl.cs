using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	
	public float maxSpeed;
	private float currentSpeed;
	private float rotateSpeed;
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

	public float marginX = 20f;
	public float marginY = 2f;
	public float rad = 9f;

	void Start() {
		isLeft = false;
		isRight = false;
		isRun = false;
		isFire = false;
		jumpStartVelocityY = -jumpDuration * Physics.gravity.y / 2;
		
		_animator = GetComponent<Animator> ();	
	}
	
	void Update() {
		//if (Input.GetMouseButtonDown (0)) {
		//_animator.SetTrigger("Attack");
		//}
	}
	
	void FixedUpdate() {
		if(isRun)
		{
			if(currentSpeed < maxSpeed)
				currentSpeed += 1.5f;
		}
		if (currentSpeed > 0) {
			currentSpeed --;
			GetComponent<Rigidbody2D> ().AddForce (gameObject.transform.up * currentSpeed * Time.deltaTime * 30);
			_animator.Play (Animator.StringToHash ("skeletonWalk"));
		} else {
			_animator.Play (Animator.StringToHash ("skeletonStand"));
		}
		if(currentSpeed == 0)
		{
			rotateSpeed = 1;
		}
		else
		{
			//rotateSpeed = currentSpeed;
		}
		if (isRight) {
			transform.Rotate ((Vector3.forward * -rotation * Mathf.Sqrt (20)) * Time.deltaTime);
		} else if (isLeft) {
			transform.Rotate ((Vector3.forward * rotation * Mathf.Sqrt (20)) * Time.deltaTime);
			
		}
	}
	
	public float jumpDuration = 0.5f;
	public float jumpDistance = 6f;
	
	private bool jumping = false;
	private float jumpStartVelocityY;

	void OnTriggerEnter2D(Collider2D col) {
		if (!isJumb) {
			if (col.name == "Slope01") {
				Vector3 forwardAndLeft = (transform.eulerAngles + transform.right) * jumpDistance * currentSpeed / marginX; 
				StartCoroutine(Jump(forwardAndLeft));
			} else if(col.name == "Slope02") {
				//DoJump (bound02, new Vector3(2f, 4f, 0));
			}
			
		}
		
	}

	void OnTriggerStay2D(Collider2D col) {
		if(col.name == "MaxSpeedBig") {
			currentSpeed += 2.2f;
			Debug.Log(currentSpeed);
		} else if(col.name == "MaxSpeedSmall") {
			currentSpeed += 1f;
			Debug.Log(currentSpeed);
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
			currentPos.z = 21.4f;
			transform.position = currentPos;
			
			//Wait until next frame.
			yield return null;
			
			height += velocityY * Time.deltaTime * (currentSpeed / marginY);
			velocityY += Time.deltaTime * Physics.gravity.y;
			time += Time.deltaTime;
		}
		targetPoint.z = 21.4f;
		float maxHeight = startPoint.y + (Mathf.Tan (Mathf.PI / rad) * (targetPoint.x - startPoint.x));
		if(targetPoint.y < maxHeight)
		//if(targetPoint.y < currentJumpHeight)
			targetPoint = new Vector3(targetPoint.x, maxHeight, targetPoint.z);
		//transform.position = targetPoint;
		StartCoroutine (setJumb (targetPoint));
		yield break;
	}
	
	IEnumerator setJumb(Vector3 targetPoint) {
		yield return new WaitForSeconds (0.016f);
		transform.position = targetPoint;
	}
}
