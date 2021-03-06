using UnityEngine;
using System.Collections;

public class TriggerZone : MonoBehaviour {
	
	public Light doorLight;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public AudioClip lockedSound;
	public GUIText textHints;
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			if(TestInventory.charge == 4){	
				transform.FindChild("door").SendMessage("DoorCheck");
				if(GameObject.Find("PowerGUI")){
					Destroy(GameObject.Find ("PowerGUI"));
					doorLight.color = Color.green;
				}
			}
			else if(TestInventory.charge > 0 && TestInventory.charge <4){
				textHints.SendMessage("ShowHint",
					"This door won't budge.. guess it needs fully charging - maybe more power cells will help...");
				transform.FindChild("door").audio.PlayOneShot(lockedSound);
			}
			else{
				textHints.SendMessage("ShowHint", "This door seems locked.. maybe that generator needs power...");
				transform.FindChild ("door").audio.PlayOneShot (lockedSound);
				col.gameObject.SendMessage("HUDon");
			}
		}
	}
}
