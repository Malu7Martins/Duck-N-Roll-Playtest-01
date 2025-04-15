using System.Collections.Generic;
using UnityEngine;

public class EnemyCullingManager : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 30f; // Ativa at� 30 unidades de dist�ncia
    public List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(player.position, enemy.transform.position);

            bool shouldBeActive = distance <= activationDistance;
            if (enemy.activeSelf != shouldBeActive)
            {
                enemy.SetActive(shouldBeActive);
            }
        }
    }
}
