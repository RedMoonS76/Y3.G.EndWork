using UnityEngine;

public class BossInSkillA : BossBaseState
{
    public override void OnEnter(Boss boss)
    {
        currentBoss = boss;
        currentBoss.bossStartSkillA();
        Debug.Log("进入技能A");
    }
    public override void LogicUpdate()
    {
        if (currentBoss.SkillAOnEnd)
        {
            currentBoss.BossSwitchState(bossState.NomAttack);
        }
    }
    public override void PhysicsUpdate()
    {
        currentBoss.bossSprint();
    }
    public override void OnExit()
    {

    }
}
