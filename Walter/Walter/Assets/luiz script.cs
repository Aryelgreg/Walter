using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int vidaAtual;
    private bool estaNoAr = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        Vector2 direcaoMovimento = new Vector2(movimentoHorizontal, 0);

        if (!estaNoAr)
        {
            rb.velocity = new Vector2(rb.velocity.x * atritoNoChao, rb.velocity.y);
        }

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - skinWidth);
        RaycastHit2D hit = Physics2D.Raycast(origin, direcaoMovimento, velocidadeMovimento * Time.deltaTime + skinWidth);

        if (hit.collider != null && hit.collider.CompareTag("Chao"))
        {
            estaNoAr = false;
            float ajusteX = Mathf.Abs(hit.point.x - origin.x) - skinWidth;
            float ajusteY = Mathf.Abs(hit.point.y - origin.y) - skinWidth;

            if (Mathf.Abs(ajusteX) < Mathf.Abs(ajusteY))
            {
                transform.Translate(new Vector2(ajusteX * Mathf.Sign(direcaoMovimento.x), 0));
            }
            else
            {
                transform.Translate(new Vector2(0, ajusteY * Mathf.Sign(direcaoMovimento.y)));
            }
        }

        rb.velocity = new Vector2(movimentoHorizontal * velocidadeMovimento, rb.velocity.y);

        if (movimentoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimentoHorizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetButtonDown("Jump") && !estaNoAr)
        {
            rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
            estaNoAr = true;
        }

        animator.SetFloat("Velocidade", Mathf.Abs(movimentoHorizontal));
        animator.SetBool("NoAr", estaNoAr);

        // Comando para atacar (neste exemplo, é a tecla "Z").
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Atacar();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaNoAr = false;
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
        gameObject.SetActive(false);
    }

    void Atacar()
    {
        // Implemente o comportamento de ataque aqui.
        // Por exemplo, você pode usar raycasts para verificar se há inimigos na frente do personagem.
        // Se um inimigo estiver dentro do alcance, reduza a vida do inimigo.

        // Exemplo simplificado (use raycasts para detecção precisa):
        RaycastHit2D hit = Physics2D.Raycast(transform.position, spriteRenderer.flipX ? Vector2.left : Vector2.right, 1f);
        if (hit.collider != null && hit.collider.CompareTag("Inimigo"))
        {
            hit.collider.GetComponent<Inimigo>().SofrerDano(10); // Chame a função de dano do inimigo.
        }
    }

    internal void PerderVida()
    {
        throw new NotImplementedException();
    }
    private void FixedUpdate()
    {
        // Check if the attack button is pressed
        if (Input.GetButtonDown("Attack"))
        {
            // Set the animator state to the attack state
            animator.SetTrigger("Attack");
        }
    }


}
