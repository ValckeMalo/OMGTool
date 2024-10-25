using UnityEngine;

public class PlayerBattleSystem : MonoBehaviour
{
    public delegate void EventPlayerTurn();
    public delegate void EventPlayerDamage(int damgeInflict);

    public static EventPlayerTurn OnPlayerTurn;
    public static EventPlayerDamage onPlayerDamage;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private int wakfuRemain = 3;
    [SerializeField] private int maxWakfu = 3;

    public void OnDestroy() => playerData.ResetData();

    public void Awake()
    {
        wakfuRemain = maxWakfu;
        playerData.Clone();

        onPlayerDamage += OnPlayerDamage;
        BattleSystem.OnPlayerTurn += PlayerTurn;
    }

    private void OnPlayerDamage(int damageInflict)
    {
        Debug.LogWarning("Damage Inflict to player " + damageInflict.ToString());
        playerData.InflictDamage(damageInflict);
    }

    private void PlayerTurn()
    {
        IncreaseWakfu();
        Debug.Log("Wakfu remain : " + wakfuRemain.ToString());
        OnPlayerTurn?.Invoke();
    }

    private void IncreaseWakfu()
    {
        wakfuRemain = maxWakfu;
        maxWakfu++;
        maxWakfu = Mathf.Min(maxWakfu, 6);
    }
}