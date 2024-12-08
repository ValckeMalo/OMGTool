namespace OMG.Card.Data
{
    using UnityEngine;
    using System.Collections.Generic;


    [CreateAssetMenu(fileName = "CardOptions", order = 2, menuName = "Card/Options")]
    public class CardOptions : ScriptableObject
    {
        public List<BackgroundSprite> backgroundSprite = new List<BackgroundSprite>();
        public List<IconSprite> iconSprite = new List<IconSprite>();

        public Sprite wakfu;

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