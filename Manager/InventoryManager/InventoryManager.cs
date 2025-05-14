using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject inventoryUI;
    public InventorySlot[] inventorySlots;
    [Header("���� ������ & �θ� ������Ʈ")]
    public GameObject slotPref;
    public Transform slotParent;
    [Header("���� ���� ����")]
    public int slotCount = 30;

    public bool isOpenInven = false;
    private void Awake()
    {
        Instance = this;
        GenerateSlot();
    }
    private void Start()
    {
        inventoryUI.SetActive(false);
    }
    public void UIActiveTrue()
    {
        isOpenInven = true;
    }
    private void GenerateSlot()
    {
        inventorySlots = new InventorySlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPref, slotParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            inventorySlots[i] = slot;
        }
    }
    public bool AddItem(ItemData item,int amount)
    {
        // ���� ������ ���
        if (item.itemType != ItemType.Equipment)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.itemData != null && slot.itemData.id == item.id)
                {
                    if (slot.itemCount < item.maxStack)
                    {
                        slot.AddCount(amount);
                        return true;
                    }
                }
            }
        }

        // ����ִ� ���� ã��
        foreach (var slot in inventorySlots)
        {
            if (slot.itemData == null)
            {
                slot.SetSlot(item,amount);
                return true;
            }
        }

        // �κ��丮 �� á�� ��
        Debug.Log("Inventory Full");
        return false;
    }
    public void InvenTouchOpen()
    {
        isOpenInven = !isOpenInven;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpenInven =!isOpenInven;
        }
        inventoryUI.SetActive(isOpenInven);
        
    }
}