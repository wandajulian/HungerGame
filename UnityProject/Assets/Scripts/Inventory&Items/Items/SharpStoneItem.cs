using UnityEngine;
using System.Collections;

public class SharpStoneItem : Item {

    public SharpStoneItem(int q)
        : base("Sharp Stone", "Sharp Stones", 2, q) {
        addItemType(Item.material);
        descriptText = "A sharp stone.  Perhaps it could be used for a tool"; ;
        icon = (Texture2D)UnityEngine.Resources.Load("stone_icon");

    }

    public SharpStoneItem(SharpStoneItem other)
        : base(other) {
    }

    public override bool useItem() {
        return false;
    }
}
