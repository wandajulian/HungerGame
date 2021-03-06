using UnityEngine;
using System.Collections;

public class EquippedAxe : EquippedItem {
	
	
	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
    protected override void Update() {

		if(Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI){
			StartCoroutine(COAction());
			//audio.PlayOneShot(testSound);
		}
//		if(Input.GetButtonDown("Undo") && !isInAction)
//		{
//			parentScript.EquipItem((int)EquippedItem.Equippable.Axe);
//		}
	}
	
	public AudioClip swing2Sound;
	IEnumerator COAction() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f); // trigger delay
	    // create bullet and flash effect...
		gameObject.animation.Play("axeSwing1");
	    //yield return 0; // wait 1 frame
	    // stop flash
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(swing2Sound);
		yield return new WaitForSeconds(0.35f);
        this.gameObject.GetComponentInChildren<BoxCollider>().enabled = true; // activate damage collider
		yield return new WaitForSeconds(0.25f);
        this.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
	    yield return new WaitForSeconds(0.15f); // extra delay before you can shoot again
	    isInAction = false;
	}
	public AudioClip woodSound;
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Axeable" && this.gameObject.GetComponentInChildren<BoxCollider>().enabled)
		{
			GameObject.Destroy(col.gameObject);
			audio.PlayOneShot(woodSound);

            float r = Random.Range(0, 3);
            if (r < 1) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().addItem(new WoodItem(1));
            }
		}
	}
	
}
