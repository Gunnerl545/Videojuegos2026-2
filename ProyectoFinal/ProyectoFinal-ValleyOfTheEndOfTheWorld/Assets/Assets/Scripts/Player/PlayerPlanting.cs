using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    public GameObject plantPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlant();
        }
    }


    void TryPlant()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Detecté: " + hit.name);

            if (hit.CompareTag("FarmPlot"))
            {
                Debug.Log("Plantando en FarmPlot");
                Instantiate(plantPrefab, hit.transform.position, Quaternion.identity);
                return; // importante: salir después de plantar
            }
        }
    }
}