namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using OMG.Game.Dungeon;
    using UnityEngine;

    [System.Serializable]
    public class FightCharacterEntity : FightEntity
    {
        [Title("Fight Character Entity")]
        [SerializeField] private CharacterDungeon characterDungeon;

        public void InitializeCharacter(CharacterDungeon characterDungeon)
        {
            this.characterDungeon = characterDungeon;
        }

        public override void EndTurn()
        {
            
        }
    }
}