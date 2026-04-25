using UnityEngine;

public class BossInNomAttack : BossBaseState
{
    private float timeT_c;
    private float timeF_c;
    public override void OnEnter(Boss boss)
    {
        currentBoss = boss;
        Debug.Log("执行进入NomAttack");
    }
    public override void LogicUpdate()
    {
        if (timeT_c >= currentBoss.bossThinkTime)
        {
            int ramT = 0;
            timeT_c = 0;
            ramT = Random.Range(0, 2);
            if (ramT == 1)
            {
                currentBoss.BossSwitchState(bossState.SkillA);
                Debug.Log("切换技能A");
            }
        }
        timeT_c += Time.deltaTime;
        if (timeT_c >= currentBoss.fireCoolDownTime)
        {
            timeT_c = 0;
            currentBoss.ShootByTimes();
        }
        timeT_c += Time.deltaTime;
        //Debug.Log("目标思考时间"+currentBoss.bossThinkTime+"已思考:" + timeC);
    }
    public override void PhysicsUpdate()
    {
        currentBoss.KeepWithPlayer();
        currentBoss.AutoTurn();
    }
    public override void OnExit()
    {

    }


}
