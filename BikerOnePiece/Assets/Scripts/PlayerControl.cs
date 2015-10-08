using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	
	public float maxSpeed;
	private float currentSpeed;
	private float rotateSpeed;
	public Vector3 jumpVelocity;
	public Vector3 jumpCenter;
	
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
	bool isFirst = true;
	bool isUp = false;
	bool isJump = false;
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
		isJump = false;
		jumpStartVelocityY = -jumpDuration * Physics.gravity.y / 2;
		_animator = GetComponent<Animator> ();
		
	}
	
	void Update() {
		//if (Input.GetMouseButtonDown (0)) {
		//_animator.SetTrigger("Attack");
		//}
	}
	float timecounter = 10.0f;
	void FixedUpdate() {
		if (isJump) {
			if(transform.position.y > startPos.y + currentSpeed/100)
			{
				isUp = true;
			}
			timecounter += Time.deltaTime * 2.0f;
			float x = Mathf.Cos (timecounter) * currentSpeed/15;
			float y = -Mathf.Sin (timecounter) * currentSpeed/10;
			if (isFirst == true) {
				jumpCenter = jumpCenter - new Vector3 (x, y, 0.0f);
				isFirst = false;
			}
			transform.position = jumpCenter + new Vector3 (x, y, 0.0f);
			float minY = 0.0f;
			if(isUp)
			{
				if(currentSpeed < maxSpeed/2)
				{
					minY = startPos.y - currentSpeed/100 + (transform.position.x - startPos.x)/5;
					Debug.Log (1);
				}
				else
				{
					minY = startPos.y + currentSpeed/100 + (transform.position.x - startPos.x)/5;
					Debug.Log (2);
				}
				
				if (transform.position.y < minY) {
					isJump = false;
					currentSpeed = 10;
					isUp = false;
					Debug.Log (3);
				}
			}
		} else 
			if (isRun) {
				if (currentSpeed < maxSpeed)
					currentSpeed += 1.5f;
			}
			else
			{
				if(currentSpeed > 0)
					currentSpeed -= 1;
			}
			if (currentSpeed > 0) {
				
				GetComponent<Rigidbody2D> ().AddForce (-transform.right * currentSpeed * Time.deltaTime * 30);
				//_animator.Play (Animator.StringToHash ("skeletonWalk"));
			} else {
				//_animator.Play (Animator.StringToHash ("skeletonStand"));
				isJump = false;
			}
			if (currentSpeed == 0) {
				rotateSpeed = 1;
			} else {
				//rotateSpeed = currentSpeed;
			}
			if (isRight) {
				Vector3 rot = (Vector3.back * -rotation * Mathf.Sqrt (20)) * Time.deltaTime;
				
				transform.Rotate (rot);
				
				//Vector3 playerRot = transform.localRotation.eulerAngles;
				//if(playerRot.z >= 60 && playerRot.z <= 90 ) {
					//if(playerRot.y >= 180.0) {
						//transform.localRotation = Quaternion.Euler(new Vector3(playerRot.x, 0, playerRot.z));
					//}
					//else transform.localRotation = Quaternion.Euler(new Vector3(playerRot.x, 180, playerRot.z));
				//}
			} else if (isLeft) {
				transform.Rotate ((Vector3.back * rotation * Mathf.Sqrt (20)) * Time.deltaTime);
				
			}
	}
	
	public float jumpDuration = 0.5f;
	public float jumpDistance = 6f;
	
	private bool jumping = false;
	private float jumpStartVelocityY;
	void OnTriggerEnter2D(Collider2D col) {
		if (!isJump) {
			if (col.name == "Slope01") {
				//Vector3 forwardAndLeft = (transform.up + transform.right); 
				//StartCoroutine(Jump(forwardAndLeft));
				jumpCenter = transform.position;
				startPos = transform.position;
				isJump = true;
				//Vector3 forwardAndLeft = (transform.eulerAngles + transform.right) * jumpDistance * currentSpeed / marginX; 
				//StartCoroutine(Jump(forwardAndLeft));
			} else if(col.name == "Slope02") {
				//DoJump (bound02, new Vector3(2f, 4f, 0));
			}
			
		}
		
	}

	void OnTriggerStay2D(Collider2D col) {
		if(col.name == "MaxSpeedBig") {
			currentSpeed += 2f;
			Debug.Log(currentSpeed);
		} else if(col.name == "MaxSpeedSmall") {
			currentSpeed += 2f;
			Debug.Log(currentSpeed);
		}
	}
	
	void DoJump (GameObject[] bound, Vector3 jumpVel) {
		isJump = true;
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