using UnityEngine;
using System.Collections;


public class StoneManager : ItemManager {
	
	// Use this for initialization
	void Start () {
		item = new Item("Sharp Stone", "Sharp Stones", 2,Random.Range(1,4));
		item.addItemType(Item.material);
	}


}