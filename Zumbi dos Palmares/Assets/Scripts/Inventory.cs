using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("Referências dos ícones")]
    public GameObject facaSlotImage;
    public GameObject cenouraSlotImage;
    public GameObject trigoSlotImage;

    void Awake()
    {
        instance = this;
    }

    public void Add(Item item)
    {
        switch (item.itemName)
        {
            case "Faca":
                facaSlotImage.SetActive(true);
                break;
            case "Cenoura":
                cenouraSlotImage.SetActive(true);
                break;
            case "Trigo":
                trigoSlotImage.SetActive(true);
                break;
            default:
                Debug.LogWarning("Item não reconhecido: " + item.itemName);
                break;
        }
    }
}
