namespace OMG.Data.Card
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "CardOptions", order = 2, menuName = "Card/Options")]
    public class CardSettings : ScriptableObject
    {
        public List<BackgroundSprite> backgroundSprite = new List<BackgroundSprite>();
        public List<IconSprite> iconSprite = new List<IconSprite>();

        public Sprite energy;

        [System.Serializable]
        public class BackgroundSprite
        {
            public CardBackground type;
            public Sprite sprite;
        }

        [System.Serializable]
        public class IconSprite
        {
            public CardAction type;
            public Sprite sprite;
        }
    }
}