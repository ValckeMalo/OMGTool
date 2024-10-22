using MaloProduction.CustomAttributes;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private CardOptions cardOptions;
    [SerializeField] private GameObject prefabCard;

    void Start()
    {
        CardsManager.onCardSpawn += SpawnCard;
    }

    private void SpawnCard(CardData cardToSpawn)
    {
        GameObject newCard = prefabCard;

        newCard.name = cardToSpawn.name;
        newCard.GetComponent<UICard>().Init(cardToSpawn, cardOptions);

        Instantiate(newCard, transform);
    }

    [Button("Clear Children")]
    private void ResetChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}