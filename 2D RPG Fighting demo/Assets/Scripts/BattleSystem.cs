using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefub;
    public GameObject enemyPrefub;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Button attackButton, healButton;

    public ButtleHUD playerHUD, enemyHUD;
    public Text dialogueText;

    private UnitC playerUnit;
    private UnitC enemyUnit;

    void Start()
    {
        State = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        GameObject playerGameObject = Instantiate(playerPrefub, playerBattleStation);
        GameObject enemyGameObject = Instantiate(enemyPrefub, enemyBattleStation);

        playerUnit = playerGameObject.GetComponent<UnitC>();
        enemyUnit = enemyGameObject.GetComponent<UnitC>();

        dialogueText.text = "A wild \"" + enemyUnit.unitName + "\" approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        State = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void OnAttackButton()
    {
        if (State != BattleState.PLAYER_TURN)
        {
            return;
        }

        ShowButtons(false);
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (State != BattleState.PLAYER_TURN)
            return;

        ShowButtons(false);
        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerAttack()
    {
        playerUnit.Attack();
        yield return new WaitForSeconds(1f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            State = BattleState.WON;
            EndBattle();
        }
        else
        {
            State = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal();
        playerHUD.SetHP(playerUnit.currentHP);

        dialogueText.text = "You've healed by " + playerUnit.healAmount + " points!";
        yield return new WaitForSeconds(2f);

        State = BattleState.ENEMY_TURN;
        StartCoroutine(EnemyTurn());

    }

    private void EndBattle()
    {
        if(State == BattleState.WON)
        {
            dialogueText.text = "You won the battle";
        }
        else if (State == BattleState.LOST)
        {
            dialogueText.text = "You were defeated";
        }
    }

    private IEnumerator EnemyTurn()
    {

        dialogueText.text = "\"" +enemyUnit.unitName + "\" attacks!" ;

        enemyUnit.Attack();
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            State = BattleState.LOST;
            EndBattle();
        }
        else
        {
            State = BattleState.PLAYER_TURN;
            PlayerTurn();

        }
    }

    private void ShowButtons(bool sholdShowButtons)
    {
        attackButton.gameObject.SetActive(sholdShowButtons);
        healButton.gameObject.SetActive(sholdShowButtons);
    }

    private BattleState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            ShowButtons(value == BattleState.PLAYER_TURN);
        }
    }
}
