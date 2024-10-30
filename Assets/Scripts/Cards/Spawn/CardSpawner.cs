using MaloProduction.CustomAttributes;
using System.Linq;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    #region Shader Class
    [System.Serializable]
    private class ShaderCards
    {
        public CardType cardType;
        public Material shaderMaterial;
    }
    #endregion

    public delegate void EventSpawnCard(CardData card);
    public static EventSpawnCard OnSpawnCard;

    [Header("Display")]
    [SerializeField] private CardOptions cardOptions;
    [SerializeField] private GameObject prefabCard;
    [SerializeField, Range(0.1f, 2.0f)] private float ratioCard = 0.6f;

    [Header("Shader")]
    [SerializeField] private ShaderCards[] shaderCards = new ShaderCards[3];

    [Header("Debug")]
    [SerializeField] private bool showCard = false;

    void Awake()
    {
        if (showCard)
        {
            CardsManager.onCardSpawn += SpawnCard;
        }

        OnSpawnCard += SpawnCard;
    }

    private void SpawnCard(CardData cardToSpawn)
    {
        GameObject newCard = Instantiate(prefabCard, transform);
        newCard.name = cardToSpawn.name;

        //keep a ratio to the screen
        RectTransform newCardRect = newCard.GetComponent<RectTransform>();
        Vector2 sizeDelta = newCardRect.sizeDelta;
        newCardRect.sizeDelta = new Vector2(sizeDelta.x * ratioCard, sizeDelta.y * ratioCard);

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