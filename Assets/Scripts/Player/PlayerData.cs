using MaloProduction.CustomAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int life = 100;

    private PlayerData clone;
    [SerializeField] private bool ShowLog = false;

    [Button("Reset Data")]
    public void ResetData()
    {
        if (clone != null)
        {
            if (ShowLog)
            {
                Debug.LogWarning($"{this} reset to the clone");
            }
            maxLife = clone.maxLife;
            life = clone.life;
        }
        else
        {
            if (ShowLog)
            {
                Debug.LogWarning("no clone created,\n created one");
            }

            Clone();
        }
    }
    [Button("Clone Data")]
    public void Clone()
    {
        clone = ScriptableObject.CreateInstance<PlayerData>();

        clone.maxLife = maxLife;
        clone.life = life;

        if (ShowLog)
        {
            Debug.LogWarning("data cloned");
        }
    }

    public void InflictDamage(int DamageInflict)
    {
        life -= DamageInflict;
    }
}