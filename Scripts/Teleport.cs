using UnityEngine;
using UnityEngine.SceneManagement;  // Necess�rio para trocar de cena

public class Teleporte : MonoBehaviour
{
    public string nomeCena = "2.QuackshoreBay";  // Nome da cena para a qual o jogador ser� teleportado

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Verifica se o objeto que entrou no trigger � o jogador
        {
            SceneManager.LoadScene(nomeCena);  // Troca para a cena 2.QuackshoreBay
        }
    }
}
