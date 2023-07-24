using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlossaryButton : MonoBehaviour
{
   public GlossaryEntryText GlossaryObject;
   public Image GlossaryImage;
   
   private Glossary glossary;
   
   public void Initialize(Glossary glossary, GlossaryEntryText glossaryEntry){
        this.glossary = glossary;
        this.name = glossaryEntry.EntryName + " - Glossary Button";
        GlossaryObject = glossaryEntry;
   }

   public void ShowPage(){
    glossary.ShowGlossaryEntry(this);
   }
}
