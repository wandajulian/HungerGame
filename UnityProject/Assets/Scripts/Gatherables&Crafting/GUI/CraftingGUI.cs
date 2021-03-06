using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class CraftingGUI : MonoBehaviour {


    Inventory inventory;
	
	public GUISkin craftingSkin;
	public Rect craftingRect;
	Rect craftingRectNormalized;
	Rect craftDetailRectNormalized;
    Rect craftingRectNormalizedWithTooltipPadding;
    Rect craftDetailRectNormalizedWithTooltipPadding;

	Item selectedItem;
    bool craftAll;
    bool isCrafting;

    Texture2D selectedLabelTexture;
    Texture2D arrowTexture;
    Texture2D craftProgressBarTexture;

    public Texture2D tooltipBackground;

     float craftProgress = 0.0f;
     public float craftTime = 1.0f;



     public static string meatMessage = "You must be near a fire to cook meat";
	
	// Use this for initialization
	void Start () {
        

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

		selectedLabelTexture = (Texture2D)UnityEngine.Resources.Load("SelectedLabel");
        arrowTexture = (Texture2D)UnityEngine.Resources.Load("Arrow_2");
        craftProgressBarTexture = (Texture2D)UnityEngine.Resources.Load("CraftProgressBar");

        IEnumerator enumerator = inventory.craftingDictionary.Keys.GetEnumerator();
        enumerator.MoveNext();
        selectedItem = (Item)enumerator.Current;

        //Debug.Log(selectedItem);

		craftingRectNormalized = normalizeRect (craftingRect);
        craftingRectNormalizedWithTooltipPadding = new Rect(craftingRectNormalized.x, craftingRectNormalized.y, craftingRectNormalized.width + 300, craftingRectNormalized.height + 100);

        craftDetailRectNormalized = new Rect(craftingRect.width/4,0,craftingRect.width*3/4,craftingRect.height);
        craftDetailRectNormalizedWithTooltipPadding = new Rect(craftDetailRectNormalized.x, craftDetailRectNormalized.y, craftDetailRectNormalized.width + 300, craftDetailRectNormalized.height + 100);


        tooltipBackground = new Texture2D(1, 1, TextureFormat.RGB24, false);
        tooltipBackground.SetPixel(0, 0, Color.gray);
        tooltipBackground.Apply();
	}

    
	// Update is called once per frame
	void Update () {
        if (craftProgress > craftTime) {
            inventory.craftItem(selectedItem,craftAll);

            //if we're crafting all, reset the craft timer and go again
            if (craftAll && inventory.canCraft(selectedItem)) {
                craftProgress = Time.deltaTime;
            } else {
                //otherwise make it 0, crafting stops
                craftAll = false;
                isCrafting = false;
                craftProgress = 0.0f;
                inventory.resetMultiCraftCount();
            }
        } else if(craftProgress > 0.0f){
            craftProgress += Time.deltaTime;
        }
	}
	
          


	Vector2 scrollPosition = Vector2.zero;

    GUIStyle chooseLeftLabelStyle(Item key) {
        Color cannotCraftColor =  new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor =cannotCraftColor;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);


        GUIStyle selectedCannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        selectedCannotCraftLabelStyle.normal.background = selectedLabelTexture;
        selectedCannotCraftLabelStyle.normal.textColor = cannotCraftColor;

        GUIStyle selectedCanCraftLabelStyle = new GUIStyle(GUI.skin.label);
        selectedCanCraftLabelStyle.normal.background = selectedLabelTexture;

        bool craftable = inventory.canCraft(key);

        if (selectedItem == key && craftable) {
            return selectedCanCraftLabelStyle;
        } else if(selectedItem == key){
            return selectedCannotCraftLabelStyle;
        }else if(craftable){
            return canCraftLabelStyle;
        } else {
            return cannotCraftLabelStyle;
        }
    }

    GUIStyle chooseQuantityLabelStyle(Item ingredient) {
        Color cannotCraftColor = new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor = cannotCraftColor;
        cannotCraftLabelStyle.fontSize = 20;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);
        canCraftLabelStyle.fontSize = 20;

        //for quantity of result
        if (ingredient == null) {
            return canCraftLabelStyle;
        }

        return inventory.contains(ingredient.name, ingredient.quantity) ? canCraftLabelStyle : cannotCraftLabelStyle;

    }

    GUIStyle chooseMeatWarningLabelStyle() {
         Color cannotCraftColor = new Color(255.0f / 255, 40.0f / 255, 51.0f / 255, 1.0f);

        GUIStyle cannotCraftLabelStyle = new GUIStyle(GUI.skin.label);
        cannotCraftLabelStyle.normal.textColor = cannotCraftColor;

        GUIStyle canCraftLabelStyle = new GUIStyle(GUI.skin.label);

        return GameObject.FindWithTag("Player").GetComponent<PlayerVitals>().IsNearFire() ? canCraftLabelStyle : cannotCraftLabelStyle;
    }

    int findScrollHeight() {
        int scrollHeight = 0;
        foreach (KeyValuePair<Item, Item[]> entry in inventory.craftingDictionary) {
            scrollHeight += entry.Key.name.Length > 16 ? 36 : 22;
        }
        return scrollHeight + 20;
    }
	
	void OnGUI(){
		GUI.skin = craftingSkin;
		
		//set up custom styles
        
		
		
		//Crafting GUI
        GUI.BeginGroup(craftingRectNormalizedWithTooltipPadding);
		
		//Left scroll
        scrollPosition = GUI.BeginScrollView(new Rect(0, 10, craftingRect.width / 4, craftingRect.height-20), scrollPosition, new Rect(0, 0, craftingRect.width / 4 - 40, findScrollHeight()));
        
		int listYLoc = 20;
        foreach (KeyValuePair<Item, Item[]> entry in inventory.craftingDictionary)
		{

            int yHeight = entry.Key.name.Length > 16 ? 36 : 22;

            //Draw the left-side "buttons" to choose what you want to craft. Don't let the player switch selected items which "craft all" is occuring, becuase that doesn't make sense.
            if (GUI.Button(new Rect(10, listYLoc, craftingRect.width / 4 - 20, yHeight), entry.Key.name, chooseLeftLabelStyle(entry.Key)) && !craftAll && !isCrafting) {
				selectedItem = entry.Key;
			}


            listYLoc += yHeight;
		}
        // End the scroll view that we began above.
        GUI.EndScrollView ();
        
        //right display
        GUI.BeginGroup(craftDetailRectNormalizedWithTooltipPadding);
        GUI.Box(new Rect(0,0,craftDetailRectNormalized.width,craftDetailRectNormalized.height),selectedItem.name);


        Item[] ingredientsList = inventory.craftingDictionary[selectedItem];
        int numIcons = ingredientsList.Length + 2;
        float paddingPixels = .1f * craftDetailRectNormalized.width / 2;
        float pixelsPerIcon = .9f * craftDetailRectNormalized.width / numIcons;
        float paddingWithinIconSection = (pixelsPerIcon - 80) / 2;


        Vector2 mousePos = new Vector2(Input.mousePosition.x - (craftingRectNormalized.x + craftDetailRectNormalized.x), (Screen.height - Input.mousePosition.y) - craftingRectNormalized.y + craftDetailRectNormalized.y);
      
        //draw the ingredients
        int i = 0;
        for (; i < ingredientsList.Length; i++) {
               GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i*pixelsPerIcon, craftDetailRectNormalized.height/3, 80, 80), ingredientsList[i].icon);
               GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 78 - 8 * (int)Math.Floor(Math.Log10(ingredientsList[i].quantity) + 1), craftDetailRectNormalized.height / 3 + 58, 30, 30), ingredientsList[i].quantity.ToString(), chooseQuantityLabelStyle(ingredientsList[i]));
               Item itemInInventory = inventory.getItem(ingredientsList[i].name);
               GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i*(pixelsPerIcon-1), craftDetailRectNormalized.height / 3 + 80, 100, 20), "(You have " + ((itemInInventory != null) ? itemInInventory.quantity : 0) + ")");
       }

        //draw the arrow
        GUI.DrawTexture(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80), arrowTexture);
        i++;
        //draw the crafted result
        Rect resultTextureRect = new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80);
        GUI.DrawTexture(resultTextureRect, selectedItem.icon);
        GUI.Label(new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon + 78 - 8 * (int)Math.Floor(Math.Log10(selectedItem.quantity) + 1), craftDetailRectNormalized.height / 3 + 58, 30, 30), selectedItem.quantity.ToString(), chooseQuantityLabelStyle(null));


        //special case: the selected craft item is a cooked meat, add a label telling the player they need to be near fire
        if (selectedItem is SmallCookedMeatItem || selectedItem is LargeCookedMeatItem) {
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent(meatMessage), "label");
            GUI.Label(new Rect(craftDetailRectNormalized.width / 2 - labelRect.width / 2, craftDetailRectNormalized.height - 100, labelRect.width, labelRect.height), meatMessage, chooseMeatWarningLabelStyle());
        }


        bool canCraft = inventory.canCraft(selectedItem);
       //if you cannot craft the target item, or you are in the middle of "crafting all", the craft button should be disabled
        if (!canCraft || craftAll) {
            GUI.enabled = false;
        }
        //draw the craft button
        if (GUI.Button(new Rect(craftDetailRectNormalized.width - 127, craftDetailRectNormalized.height - 37, 120, 30), "Craft!")) {
            craftProgress += Time.deltaTime;
            isCrafting = true;
        }

        //enable the craft all button unless you can't craft the object
        if (canCraft) {
            GUI.enabled = true;
        }

        //draw craft all button
        if (GUI.Button(new Rect(craftDetailRectNormalized.width - 257, craftDetailRectNormalized.height - 37, 120, 30), craftAll? "Halt!":"Craft All!")) {
            if (craftAll) {
                inventory.resetMultiCraftCount();
                //already crafting all means this is the cancel button. So stop crafting
                craftAll = false;
                craftProgress = 0.0f;
                isCrafting = false;
            } else {
                //craft all
                craftProgress += Time.deltaTime;
                craftAll = true;
                isCrafting = true;
            }
        }

        //enable here down
        GUI.enabled = true;

        //draw the craft progress bar, if applicable
        GUI.DrawTexture(new Rect(craftDetailRectNormalized.width/2 - 120, craftDetailRectNormalized.height - 80, 200 * craftProgress/craftTime, 30), craftProgressBarTexture);
		


        //draw the tooltips, if applicable
        for (i = 0; i < ingredientsList.Length; i++) {
            if (new Rect(paddingPixels + paddingWithinIconSection + i * pixelsPerIcon, craftDetailRectNormalized.height / 3, 80, 80).Contains(mousePos)) {
                IGUI.drawTooltip(ingredientsList[i], mousePos.x, mousePos.y, tooltipBackground);
            }
        }
        if (resultTextureRect.Contains(mousePos)) {
            IGUI.drawTooltip(selectedItem, mousePos.x, mousePos.y, tooltipBackground);
        }

        GUI.EndGroup();
        


        GUI.EndGroup();
		
	}
	
	Rect normalizeRect(Rect screenRect){
		return new Rect(screenRect.x * Screen.width - (screenRect.width * 0.5f), screenRect.y * Screen.height - (screenRect.height * 0.5f), screenRect.width, screenRect.height);
	}
}
