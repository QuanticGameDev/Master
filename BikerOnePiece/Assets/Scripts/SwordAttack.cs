using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {
		Destroy (collider.gameObject);
	}
}
