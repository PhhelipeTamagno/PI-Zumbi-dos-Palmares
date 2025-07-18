using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade = 2f;
    public float tempoMinimoEspera = 1f;
    public float tempoMaximoEspera = 3f;
    public float raioMovimento = 5f;

    [Header("Vida")]
    public int vidaMaxima = 10;
    private int vidaAtual;

    private Vector2 destino;
    private bool Andando = false;

    private Rigidbody2D rb;
    private Vector2 pontoInicial;
    private SpriteRenderer sprite;
    private Animator animator; // <- Novo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // <- Novo
        pontoInicial = transform.position;
        vidaAtual = vidaMaxima;

        StartCoroutine(AndarAleatoriamente());
    }

    void Update()
    {
        // Atualiza a animação
        animator.SetBool("Andando", Andando); // <- Novo

        if (Andando)
        {
            // Mover em direção ao destino
            rb.MovePosition(Vector2.MoveTowards(rb.position, destino, velocidade * Time.deltaTime));

            // Flip horizontal se necessário
            Vector2 direcao = destino - rb.position;
            if (direcao.x != 0)
                sprite.flipX = direcao.x < 0;

            // Chegou ao destino?
            if (Vector2.Distance(rb.position, destino) < 0.1f)
            {
                Andando = false;
                StartCoroutine(AndarAleatoriamente());
            }
        }
    }

    System.Collections.IEnumerator AndarAleatoriamente()
    {
        yield return new WaitForSeconds(Random.Range(tempoMinimoEspera, tempoMaximoEspera));

        Vector2 novaPosicao = pontoInicial + Random.insideUnitCircle * raioMovimento;
        destino = novaPosicao;
        Andando = true;
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Destroy(gameObject);
    }
}
