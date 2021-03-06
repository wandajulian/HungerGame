using UnityEngine;
using System.Collections;

public class Gatherable : MonoBehaviour {
	
	bool inZone;
	float buttonDownTime;
	public float gatherTime = 2.0f;
	
	// Use this for initialization
	void Start () {
		inZone = false;
		buttonDownTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (inZone && Input.GetButton ("Fire1")) {
			buttonDownTime += Time.deltaTime;
			if (buttonDownTime > gatherTime) {
				GameObject player  = GameObject.FindWithTag("Player");
				player.SendMessage ("Gather");
				Destroy (gameObject);
			}	
		}
		else {buttonDownTime = 0.0f;}
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = true;
		}
	}
	
	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player") {
			inZone = false;
		}
	}
}
