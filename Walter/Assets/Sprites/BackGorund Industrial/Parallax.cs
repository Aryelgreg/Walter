using UnityEngine;
using UnityEngine.UI;

public class EfeitoParallax : MonoBehaviour
{
    [SerializeField] private Image fundo;
    [SerializeField] private float velocidade;
    [SerializeField] private float larguraDaImagem; // A largura da imagem de fundo
    private float limiteDireita; // A posição x onde a imagem atinge o limite direito

    private void Start()
    {
        limiteDireita = larguraDaImagem / 2; // Define o limite direito
    }

    private void Update()
    {
        MoveFundo();
    }

    public void MoveFundo()
    {
        // Move a imagem de fundo
        transform.position = new Vector3(transform.position.x - velocidade * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);

        // Verifica se a imagem de fundo se moveu completamente para a esquerda
        if (transform.position.x < -limiteDireita)
        {
            // Reposiciona a imagem de fundo para o limite direito
            float newX = limiteDireita + (transform.position.x + limiteDireita) % larguraDaImagem;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}