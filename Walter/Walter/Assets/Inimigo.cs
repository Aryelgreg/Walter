using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float velocidadeMovimento = 2f;
    public float limiteEsquerdo = -5f;
    public float limiteDireito = 5f;
    public int vida = 3; // Adicione uma vari�vel de vida.
    public float taxaAtaque = 1f; // Adicione uma taxa de ataque (ataque por segundo).
    private bool indoDireita = true;
    private float tempoUltimoAtaque;

    void Update()
    {
        // Movimenta��o horizontal (mantida igual).

        // Verifica se atingiu os limites e inverte a dire��o (mantida igual).

        // Ataque ao personagem principal.
        if (Time.time - tempoUltimoAtaque > 1f / taxaAtaque)
        {
            Atacar();
            tempoUltimoAtaque = Time.time;
        }
    }

    void Atacar()
    {
        // Implemente o comportamento de ataque aqui.
        // Por exemplo, voc� pode usar raycasts para verificar se o jogador est� � frente do inimigo.
        // Se o jogador estiver dentro do alcance, reduza a sa�de do jogador.

        // Exemplo simplificado (use raycasts para detec��o precisa):
        PersonagemController personagem = FindObjectOfType<PersonagemController>();
        if (personagem != null && Vector2.Distance(transform.position, personagem.transform.position) < 1.5f)
        {
            personagem.PerderVida(); // Chame uma fun��o no script do personagem principal para reduzir a vida.
        }
    }

    public void SofrerDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            // O inimigo foi derrotado, voc� pode adicionar l�gica de morte aqui, como tocar uma anima��o de morte e destruir o GameObject do inimigo.
            Destroy(gameObject);
        }
    }
}
