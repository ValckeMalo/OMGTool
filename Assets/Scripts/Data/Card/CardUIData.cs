namespace OMG.Data.Card
{
    using MVProduction.CustomAttributes;

    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    [CreateAssetMenu(menuName = "Card/UI Data", fileName = "CardUIData")]
    public class CardUIData : ScriptableObject
    {
        [System.Serializable]
        public class CardUIValue
        {
            public SpellType key = SpellType.Poison;
            public string title = string.Empty;
            public string description = string.Empty;
        }

        [Title("Card UI Data")]
        [SerializeField] private List<CardUIValue> cardUIData = new List<CardUIValue>();

        [SerializeField, TextArea(5, 10)] private string initiativeDescription = string.Empty;
        [SerializeField, TextArea(5, 10)] private string needSacrificeDescription = string.Empty;
        [SerializeField, TextArea(5, 10)] private string etheralDescription = string.Empty;

        public string InitiativeDesc => initiativeDescription;
        public string NeedSacrificeDesc => needSacrificeDescription;
        public string EtheralDesc => etheralDescription;

        public CardUIValue GetValueByKey(SpellType key)
        {
            return cardUIData.Where(current => current.key == key).FirstOrDefault();
        }

        #region Editor
        [Button("Check Unique Key")]
        public void CheckUniqueKey()
        {
            HashSet<SpellType> seenKeys = new HashSet<SpellType>();

            foreach (var cardUIData in cardUIData)
            {
                if (!seenKeys.Add(cardUIData.key))
                {
                    Debug.LogError($"Duplicate key found: {cardUIData.key}");
                }
            }

            seenKeys.Clear();
        }
        #endregion
    }
}