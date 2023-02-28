using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    public bool IsPlayerInEnemyArea { get; private set; }

    private void Start()
    {
        IsPlayerInEnemyArea = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            IsPlayerInEnemyArea = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
            IsPlayerInEnemyArea = false;
    }
}