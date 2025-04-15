using UnityEngine;

public class RotatingCircle : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidade da rotação

    void Update()
    {
        // Rotaciona o objeto ao redor do eixo Z (vertical) a cada frame
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}