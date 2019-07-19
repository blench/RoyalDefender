using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    public Range2 range2;
    //进入我方小兵巡逻的范围的敌人
    //[HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
	
	void Update () {
        if (range2.soldiers.Length == 0) return;
        if (enemys.Count == 0)
        {
            foreach (GameObject game in range2.soldiers)
            {
                if (game != null)
                {
                    game.GetComponent<Soldier>().enemy = null;
                    game.GetComponent<Soldier>().isAttack = false;
                }
            }
        }
        else if (enemys.Count == 1)
        {
            foreach(GameObject game in range2.soldiers)
            {
                if (game != null)
                {
                    game.GetComponent<Soldier>().enemy = enemys[0];
                    game.GetComponent<Soldier>().isAttack = true;
                    enemys[0].GetComponent<Enemy>().soldier = game;
                    enemys[0].GetComponent<Enemy>().isAttack = true;
                }
            }
        }
        else if (enemys.Count == 2)
        {
            for(int i = 0; i < 3; i++)
            {
                if (range2.soldiers[i] != null)
                {
                    if (i < 2)
                    {
                        if (range2.soldiers[i].GetComponent<Soldier>().enemy != enemys[0])
                        {
                            range2.soldiers[i].GetComponent<Soldier>().enemy = enemys[0];
                            range2.soldiers[i].GetComponent<Soldier>().isAttack = true;
                            enemys[0].GetComponent<Enemy>().soldier = range2.soldiers[i];
                            enemys[0].GetComponent<Enemy>().isAttack = true;
                        }
                    }
                    else
                    {
                        if (range2.soldiers[i].GetComponent<Soldier>().enemy != enemys[1])
                        {
                            range2.soldiers[i].GetComponent<Soldier>().enemy = enemys[1];
                            range2.soldiers[i].GetComponent<Soldier>().isAttack = true;
                            enemys[1].GetComponent<Enemy>().soldier = range2.soldiers[i];
                            enemys[1].GetComponent<Enemy>().isAttack = true;
                        }
                    }
                }
            }
        }else if (enemys.Count > 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (range2.soldiers[i] != null)
                {
                    bool a = true;
                    foreach(GameObject game in enemys)
                    {
                        if (game.GetComponent<Enemy>().soldier != null)
                        {
                            if(game.GetComponent<Enemy>().soldier == range2.soldiers[i])
                            {
                                a = false;
                                break;
                            } 
                        }
                    }
                    if (a)
                    {
                        foreach (GameObject game in enemys)
                        {
                            if (game.GetComponent<Enemy>().soldier == null)
                            {
                                range2.soldiers[i].GetComponent<Soldier>().enemy = game;
                                range2.soldiers[i].GetComponent<Soldier>().isAttack = true;
                                game.GetComponent<Enemy>().soldier = range2.soldiers[i];
                                game.GetComponent<Enemy>().isAttack = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            enemys.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject game in enemys)
            {
                if (collision.gameObject == game)
                {
                    enemys.Remove(game);
                    break;
                }
            }
        }
    }
}
