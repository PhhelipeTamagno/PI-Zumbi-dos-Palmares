using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;  // Referência ao painel do inventário
    public Button openInventoryButton; // Referência ao botão de abrir inventário
    public GameObject inventorySlotPrefab; // Prefab do slot do inventário
    public Transform inventoryContent; // Conteúdo onde os slots serão gerados

    private bool isInventoryOpen = false;
    private List<Item> inventory = new List<Item>(); // Lista para armazenar os itens

    void Start()
    {
        // Inicialmente, o painel de inventário está oculto
        inventoryPanel.SetActive(false);

        // Atribuindo a função de abrir inventário ao botão
        openInventoryButton.onClick.AddListener(ToggleInventory);
    }

    void ToggleInventory()
    {
        // Alterna a visibilidade do painel de inventário
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        // Atualiza o inventário (para mostrar os itens)
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        // Limpa o conteúdo do inventário antes de atualizar
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Cria slots no inventário para cada item coletado
        foreach (Item item in inventory)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContent);
            Image slotImage = slot.GetComponent<Image>();
            Text slotText = slot.GetComponentInChildren<Text>();

            if (item.itemIcon != null)
            {
                slotImage.sprite = item.itemIcon;
            }
            if (slotText != null)
            {
                slotText.text = item.itemName;
            }
        }
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
        if (isInventoryOpen)
        {
            UpdateInventoryUI();
        }
    }
}
