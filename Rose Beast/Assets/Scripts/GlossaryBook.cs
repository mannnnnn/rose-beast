using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct GlossaryEntryText{
    public string EntryName;
    public GameObject EntryPrefab;
    public string EntryDesc;
}

[CreateAssetMenu(fileName = "GlossaryBook", menuName = "Data/GlossaryBook", order = 1)]
public class GlossaryBook : ScriptableObject 
{
    public List<GlossaryEntryText> AllGlossaryEntryInfo = new List<GlossaryEntryText>();
    private List<GlossaryEntryText> UnlockedGlossaryEntries = new List<GlossaryEntryText>();


    public List<GlossaryEntryText> RetrieveUnlockedEntries(){
        //TryUnlockEntriesOnMap();
        return AllGlossaryEntryInfo; //for testing, don't lock
    }

    public void TryUnlockEntriesOnMap(){
        //Any tiles on the map will be unlocked in the glossary if they are not unlocked already
        foreach(TileBound obj in FindObjectsOfType<TileBound>()){
            //check if there's a glossary entry. Can't check against the prefab because PrefabUtility is an editor script :/
            GlossaryEntryText foundEntry = new GlossaryEntryText();
            
            foreach(GlossaryEntryText entry in AllGlossaryEntryInfo){
                 if(obj.gameObject == entry.EntryPrefab){
                    foundEntry = entry;
                 }
            }

            if(foundEntry.EntryPrefab != null && !UnlockedGlossaryEntries.Contains(foundEntry)){
                UnlockedGlossaryEntries.Add(foundEntry);
            }
        }
    }

    public List<string> SerializeUnlockedEntries(){
        //just store the name of the entry (and pray it never changes?)
        //the alternatives are not great for a lazy person like me, so just don't change the names of things Savvy >:/
        List<string> UnlockedEntriesNames = new List<string>();
        foreach(GlossaryEntryText entry in UnlockedGlossaryEntries){
            UnlockedEntriesNames.Add(entry.EntryName);
        }
        return UnlockedEntriesNames;
    }


    public void LoadUnlockedEntries(List<string> EntryNames){
        foreach(string entryName in EntryNames){
            GlossaryEntryText foundEntry = new GlossaryEntryText();
            foreach(GlossaryEntryText entry in AllGlossaryEntryInfo){
                if(entryName == entry.EntryName){
                foundEntry = entry;
                }
            }
            if(foundEntry.EntryPrefab != null && !UnlockedGlossaryEntries.Contains(foundEntry)){
                UnlockedGlossaryEntries.Add(foundEntry);
            }
        }
    }


}
