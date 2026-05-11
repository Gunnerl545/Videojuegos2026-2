using UnityEngine;

 
public class ToolUser : MonoBehaviour
{
    public HotbarManager hotbar;
    public PlayerFarming farming;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseTool();
        }
    }

    void UseTool()
    {   
        switch (hotbar.CurrentTool)
        {
            case ToolType.Hoe:
                farming.Hoe();
                break;

            case ToolType.Seeds:
                farming.Plant();
                break;

            case ToolType.WateringCan:
                farming.Water();
                break;

            case ToolType.Sword:
                Debug.Log("Ataque");
                break;
            case ToolType.Harvest:
                farming.Harvest();
                Debug.Log("Cosechado ");
                break;
        }
    }
}