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

    public void MoveFundo()
    {
        // Move a imagem de fundo
        transform.position = new Vector3(transform.position.x - velocidade * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);
    }
}
