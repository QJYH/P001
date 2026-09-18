using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class BattleAreaDirector : MonoBehaviour
{
	private int currentPhase = 0;
	private int maxPhase = 0;
	private int enemyCountInCurrentPhase = 0;

	private void Awake()
	{
		SetAreaEdge();
		SetPhaseList();
	}

	private void SetPhaseList()
	{
		Transform phaseList = transform.Find("PhaseList");
		maxPhase = phaseList.childCount;
		for (int i = 1; i <= phaseList.childCount; i++)
		{
			string phaseName = string.Format("Phase{0}", i);
			Transform phase = phaseList.transform.Find(phaseName);
			foreach (Transform enemy in phase.Find("EnemyList"))
			{

				enemy.gameObject.SetActive(false);
			}
			foreach (Transform trap in phase.Find("TrapList"))
			{
				trap.gameObject.SetActive(false);
			}
		}
	}

	private void SetAreaEdge()
	{
		GameObject areaEdge = transform.Find("AreaEdge").gameObject;
		GameObject areaEdgeTriger = transform.Find("AreaEdgeTrigger").gameObject;
		areaEdge.SetActive(false);
		areaEdgeTriger.GetComponent<HeroTriggerRange>().OnHeroEnter.AddListener(() =>
		{
			areaEdge.SetActive(true);
			EnterNewPhase();
			areaEdgeTriger.SetActive(false);
		});
	}

	private void EnterNewPhase()
	{
		currentPhase++;

		string phaseName = string.Format("PhaseList/Phase{0}", currentPhase);
		Transform phase = transform.Find(phaseName);

		enemyCountInCurrentPhase = phase.Find("EnemyList").childCount;
		foreach (Transform enemy in phase.Find("EnemyList"))
		{
			GameObject dust = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Dust/Dust"));
			dust.transform.position = enemy.position;

			enemy.gameObject.SetActive(true);
			enemy.GetComponent<Enemy>().enabled = false;
			CommonFunc.FadeSprite(enemy.GetComponent<SpriteRenderer>(), 0, 1, 1, () =>
			{
				enemy.GetComponent<Enemy>().enabled = true;
				enemy.GetComponent<Enemy>().OnDie.AddListener(OnEnemyDie);
				enemy.GetComponent<Enemy>().SeeHero(LevelDirector.LevelInstance.CurrentHero.GetComponent<Hero>());
			});
		}

		foreach (Transform trap in phase.Find("TrapList"))
		{
			GameObject dust = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Dust/Dust"));
			dust.transform.position = trap.position;

			trap.gameObject.SetActive(true);
			trap.GetComponent<Trap>().enabled = false;
			CommonFunc.FadeSprite(trap.GetComponent<SpriteRenderer>(), 0, 1, 1, () =>
			{
				trap.GetComponent<Trap>().enabled = true;
			});
		}
	}

	private void OnEnemyDie()
	{
		enemyCountInCurrentPhase--;
		if (enemyCountInCurrentPhase == 0)
		{
			TryEnterNewPhase();
			
		}
	}

	private void TryEnterNewPhase()
	{
		if (currentPhase == maxPhase)
		{
			transform.Find("AreaEdge").gameObject.SetActive(false);
		}
		else
		{
			DOVirtual.DelayedCall(1, () => { EnterNewPhase(); });
		}
	}
}
