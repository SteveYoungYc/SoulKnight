using UnityEngine;
using UnityEngine.UI;

public class PrepareState : State
{
    private float interval;
    
    public override void Enter()
    {
        GameManager.mode = GameMode.Prepare;
        interval = GameManager.levelManager.levelInterval;
    }

    public override void Update()
    {
        interval -= Time.deltaTime;
        if (interval <= 0)
        {
            GameManager.levelManager.fsm.ChangeState(GameManager.levelManager.fightState);
        }
    }
}

public class FightState : State
{
    public override void Enter()
    {
        GameManager.mode = GameMode.Fight;
        Debug.Log("Level " + GameManager.levelManager.currentLevel + " has started!");
        GameManager.enemySpawner.StartSpawnEnemy();
    }

    public override void Exit()
    {
        Debug.Log("Level " + GameManager.levelManager.currentLevel + " has ended!");
        GameManager.levelManager.currentLevel++;
    }
    
    public override void Update()
    {
        if (GameManager.enemySpawner.isFinished())
        {
            GameManager.levelManager.fsm.ChangeState(GameManager.levelManager.prepareState);
        }
    }
}

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1;
    public readonly float levelInterval = 5f;
    
    public Text countdownText;
    public StateMachine fsm;
    public PrepareState prepareState;
    public FightState fightState;
    
    public void Start()
    {
        prepareState = new PrepareState();
        fightState = new FightState();
        fsm = new StateMachine();
        fsm.ChangeState(prepareState);
    }

    public void Update()
    {
        fsm.Update();
    }
}
