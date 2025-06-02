namespace OMG.Game.Fight.Entities
{
    using MVProduction.CustomAttributes;
    using OMG.Game.Dungeon;
    using UnityEngine;

    [System.Serializable]
    public class FightCharacterEntity : FightEntity
    {
        [Title("Fight Character Entity")]
        [SerializeField] private DungeonCharacter dungeonCharacter;

        public void InitializeCharacter(DungeonCharacter dungeonCharacter, FightCharacterEntityUI characterUI , FightEntityWorld fightEntityWorld)
        {
            this.dungeonCharacter = dungeonCharacter;
            InitializeEntity(dungeonCharacter.Health, dungeonCharacter.MaxHealth, characterUI, fightEntityWorld, "Oropo");
        }
    }
}