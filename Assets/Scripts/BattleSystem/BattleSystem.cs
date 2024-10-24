using MaloProduction.CustomAttributes;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public delegate void EventNextTurn();
    public static EventNextTurn OnNextTurn;

    [SerializeField] private StateTurn stateTurn;

    private enum StateTurn
    {
        Player,
        Ennemi,
    }

    public void Start()
    {
        OnNextTurn += NextTurnCall;
    }

    [Button("Next turn Call")]
    private void NextTurnCall()
    {
        stateTurn = (StateTurn)(((int)stateTurn + 1) % 2);

        Debug.Log(stateTurn + " turn");
    }
}