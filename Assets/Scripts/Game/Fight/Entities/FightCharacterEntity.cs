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

        public override void EndTurn()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeCharacter(CharacterDungeon characterDungeon)
        {
            this.characterDungeon = characterDungeon;
        }
    }
}