using UnityEngine;

public class WaterArea : MonoBehaviour
{
    // Quando o personagem entra na água
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou na água é o personagem (verificando a tag "Player")
        if (other.CompareTag("Player"))
        {
            Duck duck = other.GetComponent<Duck>();  // Obtém o componente Duck (que é o personagem)
            if (duck != null)
            {
                // Ativa a skin Water e permite o personagem respirar indefinidamente
                duck.EnterWater();
            }
        }
    }

    // Quando o personagem sai da água
    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se o objeto que saiu da água é o personagem
        if (other.CompareTag("Player"))
        {
            Duck duck = other.GetComponent<Duck>();
            if (duck != null)
            {
                // Retorna a skin para o normal (ou a skin anterior)
                duck.ExitWater();
            }
        }
    }
}