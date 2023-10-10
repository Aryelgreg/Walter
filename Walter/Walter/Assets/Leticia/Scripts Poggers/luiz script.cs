using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonagemController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

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



    public Text moedasText;
    private int moedasColetadas = 0;
    public int moedas;

    [SerializeField] private GameObject GameOverLayer;

    private enum EstadoPersonagem
    {
        Parado,
        Andando,
        Pulando,
        Caindo
    }

    private EstadoPersonagem estado = EstadoPersonagem.Parado;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        // Verifique se o jogo está no estado de "Game Over" antes de permitir movimento ou ações do jogador
        if (!estaGameOver)
        {
            HandleMovimento();
            HandlePulo();
            UpdateAnimacoes();
        }
        // Outro código de atualização aqui...
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
            podePular = false; // Não é possível pular enquanto estiver no ar
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (podePular)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                estado = EstadoPersonagem.Pulando;
                podePular = false; // Desativa o pulo após usá-lo
            }
            else if (podeDuploPulo)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
                podeDuploPulo = false;
                estado = EstadoPersonagem.Pulando;
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
                break;
            case EstadoPersonagem.Andando:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", true);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo", false);
                break;
            case EstadoPersonagem.Pulando:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", true);
                animator.SetBool("Caindo", false);
                break;
            case EstadoPersonagem.Caindo:
                animator.SetBool("Parado", false);
                animator.SetBool("Andando", false);
                animator.SetBool("Pulando", false);
                animator.SetBool("Caindo", true);
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
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("moeda"))
        {
            moedas++; // Incrementa o contador de moedas
            Destroy(other.gameObject); // Destroi a moeda coletada
            AtualizarTextoMoedas(); // Atualiza o texto na tela
        }
        
    }

    void AtualizarTextoMoedas()
    {
        moedasText.text = " " + moedas.ToString(); // Atualiza o texto na tela com o número de moedas coletadas
    }

}