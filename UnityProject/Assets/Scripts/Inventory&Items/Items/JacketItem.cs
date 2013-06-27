using UnityEngine;
using System.Collections;

public class JacketItem : EquipmentItem {

    public JacketItem(int q, int p)
        : base("Jacket (+" + p.ToString() + ")", "Jackets (+" + p.ToString() + ")", 8, q) {
        addItemType(Item.equipment);
        usable = true;
        equipmentType = EquipmentType.jacket;
        useText = "(Equip) Increases your temperature by " + p.ToString() + ".";
        icon = (Texture2D)Resources.Load("jacket_icon");
        statPower = p;
	}

    public JacketItem(JacketItem other)
        : base(other) {
    }


    public override void useItem() {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        inventory.EquipItem(this);
    }
}