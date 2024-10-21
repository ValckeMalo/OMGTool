using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    void Start()
    {
        CardsManager.onCardSpawn += SpawnCard;
    }

    private void SpawnCard()
    {
        print("Hello World");
    }
}