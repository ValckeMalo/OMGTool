using UnityEngine;

public class EnemiesBattleSystem : MonoBehaviour
{
    private Enemy[] enemies = new Enemy[3];

    private void Start()
    {
        BattleSystem.OnEnemiesTurn += EnemiesTurn;
        enemies[0] = new Bouftou();
        enemies[1] = new Arakne();
    }

    private void EnemiesTurn()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            PlayerBattleSystem.onPlayerDamage?.Invoke(enemy.Action());
        }

        BattleSystem.OnNextTurn?.Invoke();
    }
}

public abstract class Enemy
{
    public abstract int Action();
}

public class Bouftou : Enemy
{
    public override int Action()
    {
        return 5;
    }
}

public class Arakne : Enemy
{
    public override int Action()
    {
        return 10;
    }
}