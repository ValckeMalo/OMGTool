using MaloProduction.CustomAttributes;
using System.Linq;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [System.Serializable]
    private class ShaderCards
    {
        public CardType cardType;
        public Material shaderMaterial;
    }

    [SerializeField] private CardOptions cardOptions;
    [SerializeField] private GameObject prefabCard;

    [Header("Shader")]
    [SerializeField] private ShaderCards[] shaderCards = new ShaderCards[3];

    void Start()
    {
        CardsManager.onCardSpawn += SpawnCard;
    }

    private void SpawnCard(CardData cardToSpawn)
    {
        GameObject newCard = Instantiate(prefabCard, transform);
        newCard.name = cardToSpawn.name;

        UICard newCardUI = newCard.GetComponent<UICard>();
        newCardUI.Init(cardToSpawn, cardOptions);
        newCardUI.InitShader(shaderCards
                                    .Where(shader => shader.cardType == cardToSpawn.cardType)
                                    .Select(shader => shader.shaderMaterial)
                                    .FirstOrDefault());
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