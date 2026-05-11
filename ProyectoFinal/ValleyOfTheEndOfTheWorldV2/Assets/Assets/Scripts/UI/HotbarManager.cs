using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    // Lista de slots visuales
    public Image[] slots;

    // Slot actualmente seleccionado
    public int selectedSlot = 0;

    // Herramientas disponibles
    public ToolType[] tools;

    // Herramienta actual
    public ToolType CurrentTool => tools[selectedSlot];

    void Start()
    {
        UpdateHotbarUI();
    }

    void Update()
    {
        HandleScrollWheel();
    }

    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Scroll arriba
        if (scroll > 0f)
        {
            selectedSlot--;

            // Si baja de 0 → ir al último
            if (selectedSlot < 0)
            {
                selectedSlot = slots.Length - 1;
            }

            UpdateHotbarUI();
        }

        // Scroll abajo
        if (scroll < 0f)
        {
            selectedSlot++;

            // Si supera el último → volver al primero
            if (selectedSlot >= slots.Length)
            {
                selectedSlot = 0;
            }

            UpdateHotbarUI();
        }
    }

    void UpdateHotbarUI()
    {
        // Protección
        if (selectedSlot < 0 || selectedSlot >= slots.Length)
        {
            Debug.LogError("selectedSlot fuera de rango");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == selectedSlot)
            {
                slots[i].color = Color.yellow;
            }
            else
            {
                slots[i].color = Color.white;
            }
        }

        Debug.Log("Herramienta actual: " + CurrentTool);
    }
}