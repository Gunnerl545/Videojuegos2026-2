using UnityEngine;

public class Plant : MonoBehaviour
{
    public float growTime = 5f;

    public Sprite smallSprite;
    public Sprite grownSprite;

    private float timer = 0f;
    private bool grown = false;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // aseguramos que empiece como planta pequeña
        sr.sprite = smallSprite;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= growTime && !grown)
        {
            Grow();
        }
    }

    void Grow()
    {
        grown = true;

        // cambiamos el sprite en lugar de escalar
        sr.sprite = grownSprite;
    }

    public bool IsGrown()
    {
        return grown;
    }
}