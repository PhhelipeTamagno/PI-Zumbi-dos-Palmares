using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public GameObject facaSlotImage; // arraste a imagem da faca desativada aqui no Inspector

    void Awake()
    {
        instance = this;
    }

    public void Add(Item item)
    {
        if (item.itemName == "Faca")
        {
            facaSlotImage.SetActive(true);
        }

        // Se quiser adicionar outros itens no futuro, pode usar mais condições
    }
}
