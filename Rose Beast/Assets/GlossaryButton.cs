using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlossaryButton : MonoBehaviour
{
   public GameObject GlossaryObject;
   public Image GlossaryImage;
   
   private Glossary glossary;
   
   public void Initialize(Glossary glossary, GameObject obj){
        this.glossary = glossary;
        this.name = obj.name + " - Glossary Button";
        GlossaryObject = obj;
   }

   public void ShowPage(){
    glossary.ShowGlossaryEntry(this);
   }
}
