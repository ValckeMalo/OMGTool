namespace OMG.Card.UI
{
    using System.Linq;
    using UnityEngine;

    using OMG.Card.Data;

    public class CardSpawner : MonoBehaviour
    {
        #region Shader Class
        [System.Serializable]
        private class ShaderCards
        {
            public CardBackground background = CardBackground.Attack;
            public Material shaderMaterial = null;
        }
        #endregion

        public delegate PlayableCard EventSpawnCard(CardData card);
        public static EventSpawnCard OnSpawnCard;

        [Header("Display")]
        [SerializeField] private CardOptions cardOptions;
        [SerializeField] private GameObject prefabCard;
        [SerializeField, Range(0.1f, 2.0f)] private float ratioCard = 0.6f;

        [Header("Shader")]
        [SerializeField] private ShaderCards[] shaderCards = new ShaderCards[3];

        void Awake()
        {
            OnSpawnCard += SpawnCard;
        }

        private PlayableCard SpawnCard(CardData cardToSpawn)
        {
            GameObject newCard = Instantiate(prefabCard, transform);
            newCard.name = cardToSpawn.name;

            //keep a ratio to the screen
            RectTransform newCardRect = newCard.GetComponent<RectTransform>();
            Vector2 sizeDelta = newCardRect.sizeDelta;
            newCardRect.sizeDelta = new Vector2(sizeDelta.x * ratioCard, sizeDelta.y * ratioCard);

            PlayableCard newCardUI = newCard.GetComponent<PlayableCard>();
            newCardUI.Init(cardToSpawn, cardOptions);
            newCardUI.InitShader(shaderCards
                                        .Where(shader => shader.background == cardToSpawn.background)
                                        .Select(shader => shader.shaderMaterial)
                                        .FirstOrDefault());

            return newCardUI;
        }
    }
}