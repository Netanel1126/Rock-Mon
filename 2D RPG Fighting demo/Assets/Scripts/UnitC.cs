using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitC : MonoBehaviour
{
	public string unitName;
	public int unitLevel;
	public int damage;
	public int maxHP;
	public int currentHP;
	public int healAmount;

	private Animator animator;
	private Vector3 orginalPostion;

    private void Start()
    {
		animator = GetComponent<Animator>();
		orginalPostion = GetComponent<Transform>().position;
	}

    public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;
        animator.SetTrigger("TakingDamage");
		return currentHP <= 0;
	}

	public void Heal()
	{
		animator.SetTrigger("Healing");
		currentHP += healAmount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}

    public void Attack()
    {
		Debug.Log("Attacking");
		animator.SetTrigger("Attacking");

	}

    public void Rewind()
    {
		GetComponent<Transform>().position = orginalPostion;
    }

}
