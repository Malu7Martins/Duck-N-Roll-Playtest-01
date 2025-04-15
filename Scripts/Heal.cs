using UnityEngine;

public class Heal : MonoBehaviour
{
    public int cureAdd = 1;  // Quantidade de cura que o item fornece

    // Refer�ncia ao script de sa�de do personagem (assumindo que o personagem tem o script DuckHealth)
    public DuckHealth duckHealth;

    // Quando o personagem entra em contato com o item de cura
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifica se o objeto que tocou � o personagem
        {
            // Aplica a cura ao personagem
            duckHealth.Heal(cureAdd);

            // Destr�i o item de cura ap�s ser usado
            Destroy(gameObject);  // Destroi o objeto de cura
        }
    }
}
