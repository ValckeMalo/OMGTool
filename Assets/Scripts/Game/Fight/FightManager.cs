namespace OMG.Game.Fight
{
    using MVProduction.CustomAttributes;
    using System.Collections;
    using UnityEngine;

    public class FightManager : MonoBehaviour
    {
        #region Singelton
        private static FightManager instance;
        public static FightManager Instance => instance;

        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }
        #endregion

        [Title("Fight Manager")]
        [SerializeField, ReadOnly] private FightData fightData;

        public FightData FightData => fightData;

        private void Start()
        {
            
        }

        private void InitializeFight()
        {
            //fightData.InitializeData();
        }

        //TODO: Possibli a StateMachine with each state for one coroutine
        //And we can stock the coroutine in a variable to stop it and start another one
        //Coroutine fightStateCoroutine;
        //A méditer

        private IEnumerator SpawnMobs()
        {
            yield return null;
        }
        private IEnumerator PrepareMobs()
        {
            yield return null;
        }
        private IEnumerator PrepareCharacter()
        {
            yield return null;
        }
        private IEnumerator NewMobsTurn()
        {
            yield return null;
        }
        private IEnumerator NewCharacterTurn()
        {
            yield return null;
        }
    }
}