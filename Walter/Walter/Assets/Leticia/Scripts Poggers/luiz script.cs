using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonagemController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Dialogue dialogue; // Adicione uma refer�ncia ao script de di�logo
    private DialogueControl dc;
    private AudioController audioController;

    public float velocidadeMovimento = 5f;
    public float forcaPulo = 10f;
    public float atritoNoChao = 2f;
    public float skinWidth = 0.02f;
    public int vidaMaxima = 100;
    public int vidaAtual;
    private bool estaNoChao = false;
    private bool podePular = true;
    private bool podeDuploPulo = true;
    private bool estaGameOver = false;
    bool canMove = true;
    bool AtivarMetodo = true;




    public TextMeshProUGUI moedasText;
    private int moedasColetadas = 0;
    public int moedas;
    [SerializeField] private AudioClip somMoeda; // Adicione uma refer�ncia ao arquivo de �udio da moeda
    private AudioSource audioSource; // Crie uma refer�ncia para o componente de �udio

    [SerializeField] private GameObject GameOverLayer;

    


    private enum EstadoPersonagem
    {
        Parado,
        Andando,
        Pulando,
        Caindo,
        Pulo2
    }

    private EstadoPersonagem estado = EstadoPersonagem.Parado;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
        dialogue = FindObjectOfType<Dialogue>(); // Encontre o script de di�logo
        dc = FindObjectOfType<DialogueControl>();
        audioController = FindObjectOfType<AudioController>();

        // Obtenha o componente de �udio do GameObject
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // Verifique se o jogo est� no estado de "Game Over" antes de permitir movimento ou a��es do jogador
        if (!estaGameOver)
        {
            UpdateAnimacoes();

            if (AtivarMetodo == true)
            {
                HandleMovimento();
                HandlePulo();
            }
        }

        if (dc.CloseDialogue == true)
        {
            canMove = true;
        }

        if (dialogue.dialogueActive == true)
        {
            canMove = false;
        }
        

        

        // Outro c�digo de atualiza��o aqui...
    }

    void HandleMovimento()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        Vector2 direcaoMovimento = new Vector2(movimentoHorizontal, 0);

        rb.velocity = new Vector2(movimentoHorizontal * velocidadeMovimento, rb.velocity.y);

        if (movimentoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
            estado = EstadoPersonagem.Andando;
        }
        else if (movimentoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
            estado = EstadoPersonagem.Andando;
        }
        else
        {
            estado = EstadoPersonagem.Parado;
        }

        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            estaNoChao = true;
        }
        else
        {
            estaNoChao = false;
        }
    }

    void HandlePulo()
    {
        if (estaNoChao)
        {
            podePular = true;
            podeDuploPulo = true;
        }
        else
        {
            podePular = false; // N�o � poss�vel pular enquanto estiver no ar
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (podePular)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                estado = EstadoPersonagem.Pulando;
                podePular = false; // Desativa o pulo ap�s us�-lo
            }
            else if (podeDuploPulo)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                podeDuploPulo = false;
                estado = EstadoPersonagem.Pulo2;

               



            }
           
        }
    }

    void UpdateAnimacoes()
    {
        animator.SetFloat("Velocidade", Mathf.Abs(rb.velocity.x));
        animator.SetBool("NoChao", estaNoChao);
        
        switch (estado)
        {
            case EstadoPersonagem.Parado:
                animator.SetBool("Parado", true);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo", false);
                animator.SetBool("Pulo2", false);
                break;
            case EstadoPersonagem.Andando:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", true);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo", false);
                animator.SetBool("Pulo2", false);
                break;
            case EstadoPersonagem.Pulando:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", true);
                animator.SetBool("Caindo", false);
                animator.SetBool("Pulo2", false);
                break;
            case EstadoPersonagem.Caindo:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo", true);
                animator.SetBool("Pulo2",false);
                break;
            case EstadoPersonagem.Pulo2:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo",false);
                animator.SetBool("Pulo2", true);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estado = EstadoPersonagem.Parado;
            estaNoChao = true;
        }
    }

    public void PerderVida(int dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
            
        }
    }

    void Morrer()
    {
        animator.SetTrigger("Morrer");
        // Desativa o componente Rigidbody2D para parar o movimento
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Define estaGameOver como true para indicar o estado de "Game Over"
        estaGameOver = true;

        // Ativa o objeto "GameOverLayer"
        if (GameOverLayer != null)
        {
            GameOverLayer.SetActive(true);
            // Chame a fun��o para tocar o som de Game Over
            audioController.PlaySomGameOver();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("moeda"))
        {
            moedas++; // Incrementa o contador de moedas
            Destroy(other.gameObject); // Destroi a moeda coletada
            AtualizarTextoMoedas(); // Atualiza o texto na tela
            TocarSomMoeda(); // Toca o som da moeda
        }
        
    }

    void TocarSomMoeda()
    {
        if (somMoeda != null && audioSource != null)
        {
            audioSource.PlayOneShot(somMoeda);
        }
    }

    void AtualizarTextoMoedas()
    {
        moedasText.text = " " + moedas.ToString(); // Atualiza o texto na tela com o n�mero de moedas coletadas
    }

    private void FixedUpdate()
    {
        if (canMove == false)
        {
            AtivarMetodo = false;
            rb.velocity = Vector2.zero;

        }

        else
        {
            AtivarMetodo = true;
        }
    }


}