using UnityEngine;
using System.Collections;

public class AxeItem : EquipmentItem {

    public AxeItem(int q) : base("Axe", "Axes", 5, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.equipable;
        useText = "(Equip) Allows you to attack with an axe.";
        descriptText = "An axe made of stone and wood. Can also chop down environmental trees.";
        icon = (Texture2D)UnityEngine.Resources.Load("axe_icon");
        statPower = 20f;
	}

    public AxeItem(AxeItem other)
        : base(other) {
    }


    public override bool useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        return inventory.EquipItem(this);
    }
}
