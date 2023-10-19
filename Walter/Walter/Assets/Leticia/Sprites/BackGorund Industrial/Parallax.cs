using UnityEngine;
using UnityEngine.UI;

public class EfeitoParallax : MonoBehaviour
{
    [SerializeField] private Image fundo;
    [SerializeField] private float velocidade;
    [SerializeField] private float larguraDaImagem; // A largura da imagem de fundo

    private void Update()
    {
        MoveFundo();
    }

    private void MoveFundo()
    {
        // Move a imagem de fundo
        transform.position = new Vector3(transform.position.x - velocidade * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);

        // Verifica se a imagem de fundo se moveu completamente para a esquerda
        if (transform.position.x < -larguraDaImagem / 2)
        {
            // Reposiciona a imagem de fundo para criar o efeito de scroll infinito
            float newX = larguraDaImagem / 2 + (transform.position.x + larguraDaImagem / 2) % larguraDaImagem;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        // Verifica se a imagem de fundo se moveu completamente para a direita
        else if (transform.position.x > larguraDaImagem / 2)
        {
            // Reposiciona a imagem de fundo para criar o efeito de scroll infinito
            float newX = -larguraDaImagem / 2 + (transform.position.x - larguraDaImagem / 2) % larguraDaImagem;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
