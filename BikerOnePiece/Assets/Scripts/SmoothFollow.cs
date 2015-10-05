using UnityEngine;
using System.Collections;



public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
	public static bool isBounded = false;

	public float xPosDefault;
	public float yPosDefault;
	
	public float maxCarmeraY = -3.6f;
	
	void Start()
	{
		transform = gameObject.transform;
		_playerController = target.GetComponent<CharacterController2D>();
	}

	void LateUpdate()
	{
		if (!useFixedUpdate)
			updateCameraPosition ();
	}


	void FixedUpdate()
	{
		if (useFixedUpdate)
				updateCameraPosition ();
	}


	void updateCameraPosition()
	{
		if (isBounded) {
			//transform.position = Vector3.SmoothDamp( transform.position, defaulCameraPos, ref _smoothDampVelocity, smoothDampTime );
		} else {
			if(target != null) {
				if( _playerController == null)
				{
					Vector3 targetPosition = target.position - cameraOffset;
					//targetPosition.y = maxCarmeraY;
					transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref _smoothDampVelocity, smoothDampTime );
					return;
				}

				if( _playerController.velocity.x > 0 )
				{
					Vector3 targetPosition = target.position - cameraOffset;
					//targetPosition.y = maxCarmeraY;
					transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref _smoothDampVelocity, smoothDampTime );
				}
				else
				{
					var leftOffset = cameraOffset;
					leftOffset.x *= -1;
					Vector3 targetPosition = target.position - leftOffset;
					//targetPosition.y = maxCarmeraY;
					transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref _smoothDampVelocity, smoothDampTime );
				}
			}

		}

	}
	
}
