using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range2 : MonoBehaviour {
    //我方小兵
    public GameObject soldier;
    //集结点
    public Vector3 point { set; get; }
    private Vector3[] points = { new Vector3(0, 0.2f, 0), new Vector3(-0.4f, -0.1f, 0), new Vector3(0.4f, -0.1f, 0) };
    //父物体
    public Tower2 tower2;
    //一个小队
    [HideInInspector]
    public GameObject[] soldiers = new GameObject[3];
    //是否出兵，第二次
    public float sendTroopsTime { set; get; }
    //第一次出兵
    public int isFirst { set; get; }

    void Start () {
        point = transform.position + new Vector3(0, -1, 0);
        sendTroopsTime = 0;
        isFirst = 1;
    }
	
	void Update () {
        if (isFirst==1)
        {
            tower2.gameObject.transform.Find("Tower").GetComponent<Animator>().SetBool("Switch", true);
            isFirst = 2;
            StartCoroutine(FirstSendTroops());
        }
        else if(isFirst==3)
        {
            tower2.gameObject.transform.Find("Tower").GetComponent<Animator>().SetBool("Switch", false);
            StartCoroutine(SendTroops());
        }
	}
    private IEnumerator SendTroops()
    {
        if (sendTroopsTime < Time.time)
        {
            sendTroopsTime = Time.time + 5.0f;
            for (int i = 0; i < soldiers.Length; i++)
            {
                if (soldiers[i] == null)
                {
                    tower2.gameObject.transform.Find("Tower").GetComponent<Animator>().SetBool("Switch", true);
                    yield return new WaitForSeconds(0.3f);
                    soldiers[i] = Instantiate(soldier, transform.position, Quaternion.identity);
                    soldiers[i].GetComponent<Soldier>().point = point + points[i];
                    soldiers[i].GetComponent<Soldier>().attackPower = tower2.attackPower;
                    soldiers[i].GetComponent<Soldier>().initHp = tower2.hp;
                    if (tower2.grade == 2|| tower2.grade == 3)
                    {
                        soldiers[i].transform.Find("Image").GetComponent<Animator>().SetLayerWeight(1, 1);
                    }
                }
            }
        }
        for (int i = 0; i < soldiers.Length; i++)
        {
            if (soldiers[i] != null)
            {
                if (soldiers[i].GetComponent<Soldier>().isDead)
                {
                    Destroy(soldiers[i], 2.0f);
                    soldiers[i] = null;
                }
            }
        }
    }

    private IEnumerator FirstSendTroops()
    {
        yield return new WaitForSeconds(0.35f);
        for (int i = 0; i < soldiers.Length; i++)
        {
            GameObject game = Instantiate(soldier, transform.position, Quaternion.identity);
            game.GetComponent<Soldier>().point = point + points[i];
            game.GetComponent<Soldier>().attackPower = tower2.attackPower;
            game.GetComponent<Soldier>().initHp = tower2.hp;
            soldiers[i] = game;
        }
        isFirst = 3;
    }
    public void MovePoint(Vector3 pos)
    {
        point = pos;
        for (int i = 0; i < soldiers.Length; i++)
        {
            if (soldiers[i] != null)
            {
                soldiers[i].GetComponent<Soldier>().point= point + points[i];
                soldiers[i].GetComponent<Soldier>().isAttack = false;
            }
        }
    }

}
