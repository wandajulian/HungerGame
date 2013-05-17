using UnityEngine;
using System.Collections;

public class WoodItem : Item {

    public WoodItem(int q) : base("Piece of Wood", "Pieces of Wood", 1, q) {
        addItemType(Item.material);
        icon = (Texture2D)Resources.Load("wood_log_icon");
	}

    public WoodItem(WoodItem other)
        : base(other) {
    }

    public override void useItem() {

    }
}