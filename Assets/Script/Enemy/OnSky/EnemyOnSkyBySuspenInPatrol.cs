using UnityEngine;

public class EnemyOnSkyBySuspenInPatrol : EnemyBaseState
{
    float timeSet = 0;
    bool isRamMove = false;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }
    public override void LogicUpdate()
    {

        if ((Vector3.Distance(currentEnemy.gameObject.transform.position, currentEnemy.player.transform.position) < currentEnemy.enemySeekRange) && currentEnemy.FindPlayer())
        {
            currentEnemy.SwichState(enemyStateOnSky.enemyAttack);
        }
        //Debug.Log(Vector3.Distance(currentEnemy.gameObject.transform.position, currentEnemy.player.transform.position));
        timeSet += Time.deltaTime;
        if (timeSet > currentEnemy.enemyThinkTime)
        {
            timeSet = 0;
            isRamMove = true;
            Debug.Log("Ö´ÐÐ");
        }

    }
    public override void PhysicsUpdate()
    {
        if (isRamMove)
        {
            currentEnemy.RamMove();
            isRamMove = false;
        }



    }
    public override void OnExit()
    {

    }
}
