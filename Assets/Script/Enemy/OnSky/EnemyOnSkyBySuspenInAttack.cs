using UnityEngine;

public class EnemyOnSkyBySuspenInAttack : EnemyBaseState
{
    float timeSet = 0;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

    }
    public override void LogicUpdate()
    {
        currentEnemy.lockPlayer();
        timeSet += Time.deltaTime;
        if (timeSet > currentEnemy.enemyThinkTime)
        {
            timeSet = 0;
            currentEnemy.ShootByThrid();
            currentEnemy.enemyAnim.EnemyAttackAnim();
            Debug.Log("Ö´ÐÐ");
        }
    }
    public override void PhysicsUpdate()
    {
        currentEnemy?.ClosePlayer(currentEnemy.player.transform);
    }
    public override void OnExit()
    {

    }
}
