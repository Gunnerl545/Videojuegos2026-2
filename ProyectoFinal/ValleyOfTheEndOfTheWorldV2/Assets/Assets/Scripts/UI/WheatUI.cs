using UnityEngine;
using TMPro;

public class WheatUI : MonoBehaviour
{
    public TextMeshProUGUI wheatText;

    public PlayerFarming farming;

    void Update()
    {
        wheatText.text = "Trigo: " + farming.wheatCount;
    }
}