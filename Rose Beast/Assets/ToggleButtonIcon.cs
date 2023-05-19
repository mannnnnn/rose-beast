using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonIcon : MonoBehaviour
{
   public Image icon;

   public Sprite pressedIcon;
   private Sprite originalIcon;

    public void Start(){
        originalIcon = icon.sprite;
    }

    public void Toggle(){
        if(icon.sprite == originalIcon){
            icon.sprite = pressedIcon;
        } else {
            icon.sprite = originalIcon;
        }
    }
}
