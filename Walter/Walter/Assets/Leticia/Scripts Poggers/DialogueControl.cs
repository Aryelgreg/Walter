using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{

    public bool CloseDialogue = false;
    

    [Header("Components")] 
    public GameObject dialogueObj;
    public GameObject GameObjectDinheiro;
    public GameObject ContadorMonetario;
    public TextMeshProUGUI actorNameText;
    public TextMeshProUGUI speechText;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            NextSentence();
        }
    }

    public void Speech(string[] txt, string actorName)
    {
        dialogueObj.SetActive(true);
        sentences = txt;
        actorNameText.text = actorName;
        StartCoroutine(TypeSentence());
        CloseDialogue = false;
    }

    IEnumerator TypeSentence ()
    {
        foreach (char letter in sentences[index].ToCharArray()) 
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        
        }
    }

    public void NextSentence ()
    {
        if (speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                StartCoroutine(TypeSentence());
               
                
                
            }
            else
            {
                speechText.text = "";
                index = 0;
                dialogueObj.SetActive (false);
                CloseDialogue = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (CloseDialogue == true)
        {
            GameObjectDinheiro.SetActive(true);
            ContadorMonetario.SetActive(true);
        }

        
    }



}
