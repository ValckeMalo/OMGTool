using OMG.Unit.Oropo;

namespace OMG.Game.Fight.Entities
{
    public class FightCharacterEntity : FightEntity
    {
        private Player/*Rename in CharacterData*/ characterData;

        public void InitializeCharacter(Player characterData)
        {
            this.characterData = characterData;
        }

        public override void EndTurn()
        {
            
        }
    }
}