using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public delegate void EventNextTurn();
    public delegate void EventEnemiesTurn();
    public delegate void EventPlayerTurn();

    public static EventNextTurn OnNextTurn;
    public static EventEnemiesTurn OnEnemiesTurn;
    public static EventPlayerTurn OnPlayerTurn;

    [SerializeField] private StateTurn stateTurn = StateTurn.Ennemi;
    public static int TurnIndex { get; private set; }

    private enum StateTurn
    {
        Player,
        Ennemi,
    }

    private void Start()
    {
        TurnIndex = 0;
        OnNextTurn += NextTurnCall;
        OnNextTurn?.Invoke();
    }

    private void NextTurnCall()
    {
        stateTurn = (StateTurn)(((int)stateTurn + 1) % 2);

        Debug.Log(stateTurn + " turn");

        if (stateTurn == StateTurn.Player)
        {
            TurnIndex++;
            OnPlayerTurn?.Invoke();
        }
        else
        {
            OnEnemiesTurn?.Invoke();
        }
    }
}