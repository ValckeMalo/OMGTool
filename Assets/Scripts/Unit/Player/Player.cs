namespace OMG.Unit.Player
{
    using OMG.Card.Data;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Player", menuName = "Unit/Player", order = 0)]
    public class Player : Unit
    {
        [SerializeField] private CardDeck deck;
        public CardDeck Deck { get => deck; }

        public override string GetName()
        {
            return "Player";
        }
    }
}