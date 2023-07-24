using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Glossary : MonoBehaviour
{
    public GlossaryBook GlossaryBook;

    public GameObject GlossaryTransform;
    public GameObject GlossaryButtonPrefab;


    public GameObject GlossaryPage;
    public Image GlossaryPageImage;
    public TextMeshProUGUI GlossaryPageTitle; 
    public TextMeshProUGUI GlossaryPageDesc;

    public Transform GlossaryButtonsHolder;

    public GameObject StatDisplayPrefab;
    public Transform StatsHolder;

    public Sprite SpawnerIcon;
    public Sprite MoverIcon;
    public Sprite LeaderIcon;
    public Sprite GrowerIcon;
    public Sprite FollowerIcon;
    public Sprite DefenderIcon;
    public Sprite BlockerIcon;
    public Sprite AttackerIcon;
    public Sprite EaterIcon;

    public Sprite MeatIcon;
    public Sprite DropOnDeathIcon;

    public void ToggleGlossary(){
        if(GlossaryTransform.activeSelf){
            HideGlossaryGrid();
        } else{
            ShowGlossaryGrid();
        }
    }


    public void ShowGlossaryGrid(){
        foreach (Transform child in GlossaryButtonsHolder) {
            GameObject.Destroy(child.gameObject);
        }

        GlossaryTransform.gameObject.SetActive(true);
        GlossaryPage.SetActive(false);
    
        foreach(GlossaryEntryText entry in GlossaryBook.RetrieveUnlockedEntries()){
            GlossaryButton glossaryButton = Instantiate(GlossaryButtonPrefab, GlossaryButtonsHolder).GetComponent<GlossaryButton>();
            glossaryButton.Initialize(this,entry);
            glossaryButton.GlossaryImage.sprite = glossaryButton.GlossaryObject.EntryPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    public void HideGlossaryGrid(){
        GlossaryTransform.gameObject.SetActive(false);
    }

    public void ShowGlossaryEntry(GlossaryButton glossaryButton){
        GlossaryPage.SetActive(true);
        GlossaryPageImage.sprite = glossaryButton.GlossaryImage.sprite;
        GlossaryPageTitle.text = glossaryButton.GlossaryObject.EntryName;
        GlossaryPageDesc.text = glossaryButton.GlossaryObject.EntryDesc;

        foreach (Transform child in StatsHolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Trait trait in glossaryButton.GlossaryObject.EntryPrefab.GetComponents<Trait>())
        {
          
            string sanitizedTraitName = trait.ToString().Substring(trait.ToString().IndexOf("(")+1, trait.ToString().IndexOf(")") - trait.ToString().IndexOf("(")-1);
            UnityEngine.Debug.Log(sanitizedTraitName);
            switch (sanitizedTraitName.Trim().ToLower()) //yes this is dumb but-
            {
               
                case "defender":
                    UnityEngine.Debug.Log("Hit!");
                    StatDisplay defenderHPDisplay = Instantiate(StatDisplayPrefab, StatsHolder).GetComponent<StatDisplay>();
                    Defender defender = (Defender)trait;
                    defenderHPDisplay.Initialize(DefenderIcon, defender.MaxHealth.ToString("D4"));

                    if (defender.meat > 0)
                    {
                        StatDisplay dropOnDeath = Instantiate(StatDisplayPrefab, StatsHolder).GetComponent<StatDisplay>();
                        dropOnDeath.Initialize(MeatIcon, defender.meat.ToString("D4"));
                    }

                    if (defender.Drop != null)
                    {
                        StatDisplay dropOnDeath = Instantiate(StatDisplayPrefab, StatsHolder).GetComponent<StatDisplay>();
                        dropOnDeath.Initialize(DropOnDeathIcon, defender.Drop.name.ToString());
                    }
                    break;
           
                case "spawner":
                    StatDisplay spawnerDisplay = Instantiate(StatDisplayPrefab, StatsHolder).GetComponent<StatDisplay>();
                    Spawner spawner = (Spawner)trait;
                    spawnerDisplay.Initialize(SpawnerIcon, spawner.spawn.name);
                    spawnerDisplay.icon.sprite = SpawnerIcon; break;

                default: break;
            }
        }
    }

    public void CloseGlossaryEntry(){
        GlossaryPage.SetActive(false);
    }
}
