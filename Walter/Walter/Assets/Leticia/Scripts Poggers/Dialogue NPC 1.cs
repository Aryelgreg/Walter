using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] speechTxt;
    public string actorName;
    private Animator animator;

    private DialogueControl dc;
    public float radious;
    public LayerMask playerLayer;
    bool onRadious;
    public bool dialogueActive = false; // Vari�vel de controle para rastrear se o di�logo est� ativo

    



    private void Start()
    {
        dc = FindObjectOfType<DialogueControl>();
    }

    private void FixedUpdate()
    {
        Interact();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2") && onRadious && !dialogueActive)
        {
            dc.Speech(speechTxt, actorName);
            dialogueActive = true; // Marcar que o di�logo est� ativo
           
        }

        if (Input.GetKeyDown(KeyCode.F) && onRadious && !dialogueActive)
        {
            dc.Speech(speechTxt, actorName);
            dialogueActive = true; // Marcar que o di�logo est� ativo

        }

        if (dc.CloseDialogue == true)
        {
            dialogueActive = false;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Bot�o Fire1 pressionado e dentro da �rea de intera��o.");
        }

    }

    public void Interact()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer);

        if (hit != null)
        {
            onRadious = true;
        }
        else
        {
            onRadious = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radious);
    }

   

    // M�todo para fechar o di�logo e permitir que o personagem se mova novamente
    

}
