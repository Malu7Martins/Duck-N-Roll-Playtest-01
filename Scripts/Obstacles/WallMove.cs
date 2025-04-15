using UnityEngine;

public class WallMove : MonoBehaviour
{
    public float velocidade = 4f;
    public float alturaMax = 3f;
    public float alturaMin = -3f;

    private int direcao = 1; // 1 para subir, -1 para descer

    void Update()
    {
        transform.Translate(Vector2.up * velocidade * direcao * Time.deltaTime);

        if (transform.position.y >= alturaMax)
        {
            direcao = -1; // Começa a descer
        }
        else if (transform.position.y <= alturaMin)
        {
            direcao = 1; // Começa a subir
        }
    }
}