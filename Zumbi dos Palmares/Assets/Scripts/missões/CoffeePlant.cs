using UnityEngine;

public class CoffeePlant : MonoBehaviour
{
    public Sprite plantaComCafe;
    public Sprite plantaSemCafe;

    private SpriteRenderer spriteRenderer;
    private MissaoManager missaoManager;
    private bool coletado = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        missaoManager = FindObjectOfType<MissaoManager>();
        spriteRenderer.sprite = plantaComCafe;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !coletado)
        {
            coletado = true;
            spriteRenderer.sprite = plantaSemCafe; // troca o sprite para sem café
            missaoManager.CafeColetado();
        }
    }
}
