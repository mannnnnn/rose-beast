using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Glossary : MonoBehaviour
{
    public GameObject GlossaryTransform;
    public GameObject GlossaryButtonPrefab;


    public GameObject GlossaryPage;
    public Image GlossaryPageImage;
    public TextMeshProUGUI GlossaryPageTitle; 
    public TextMeshProUGUI GlossaryPageDesc;

    public Transform GlossaryButtonsHolder;

    public List<GameObject> glossaryItems;

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
    
        foreach(GameObject item in glossaryItems){
            GlossaryButton glossaryButton = Instantiate(GlossaryButtonPrefab, GlossaryButtonsHolder).GetComponent<GlossaryButton>();
            glossaryButton.Initialize(this,item);
            glossaryButton.GlossaryImage.sprite = glossaryButton.GlossaryObject.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    public void HideGlossaryGrid(){
        GlossaryTransform.gameObject.SetActive(false);
    }

    public void ShowGlossaryEntry(GlossaryButton glossaryButton){
        GlossaryPage.SetActive(true);
        GlossaryPageImage.sprite = glossaryButton.GlossaryImage.sprite;
        GlossaryPageTitle.text = glossaryButton.GlossaryObject.name;
    }

    public void CloseGlossaryEntry(){
        GlossaryPage.SetActive(false);
    }
}
