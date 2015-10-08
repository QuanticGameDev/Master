using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	private GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void leftOn() {
		PlayerControl.isLeft = true;
	}

	public void leftOff() {
		PlayerControl.isLeft = false;
	}

	public void rightOn() {
		PlayerControl.isRight = true;
	}
	
	public void rightOff() {
		PlayerControl.isRight = false;
	}

	public void runOn() {
		PlayerControl.isRun = true;
	}
	
	public void runOff() {
		PlayerControl.isRun = false;
	}

	public void fireOn() {
		PlayerControl.isFire = true;
		Application.LoadLevel (0);
	}
	
	public void fireOff() {
		PlayerControl.isFire = false;
	}
}
