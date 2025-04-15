using UnityEngine;

public class Heal : MonoBehaviour
{
    public int cureAdd = 1;  // Quantidade de cura que o item fornece

    // Referência ao script de saúde do personagem (assumindo que o personagem tem o script DuckHealth)
    public DuckHealth duckHealth;

    // Quando o personagem entra em contato com o item de cura
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifica se o objeto que tocou é o personagem
        {
            // Aplica a cura ao personagem
            duckHealth.Heal(cureAdd);

            // Destrói o item de cura após ser usado
            Destroy(gameObject);  // Destroi o objeto de cura
        }
    }
}
