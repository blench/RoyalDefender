using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public AudioClip[] audioClips;
    //已选择的敌人
    public GameObject enemy { set; get; }

    //箭的攻击力
    public float attackPower { set; get; }

    public float t { set; get; }

    private Vector3 pos1;
    private Vector3 pos2;
    private bool isDead = false;
    private void Start()
    {
        if (enemy == null)
        {
            Destroy(transform.gameObject);
            return;
        }
        pos1 = transform.position;
        pos2 = (enemy.transform.position - transform.position) / 3 * 2 + transform.position;

        if (enemy.transform.position.x > pos1.x)
        {
            if (enemy.transform.position.y > pos1.y)
            {
                pos2.x -= 1;
                pos2.y += 3;
            }
            else
            {
                pos2.x += 1;
                pos2.y += 3;
            }
        }
        else
        {
            if (enemy.transform.position.y > pos1.y)
            {
                pos2.y += 2;
            }
            else
            {
                pos2.x -= 1;
                pos2.y += 2;
            }
        }
        t = 0;
    }
    
    private void FixedUpdate()
    {
        if (isDead) return;
        if (enemy == null)
        {
            Destroy(transform.gameObject);
        }
        else
        {
            Vector3 pos = Mathf.Pow((1 - t), 2) * pos1 + 2 * t * (1 - t) * pos2 + Mathf.Pow(t, 2) * enemy.transform.position;
            transform.right = pos - transform.position;
            transform.position = pos;

            int a = Random.Range(0, 2);
            if (a == 0) transform.GetComponent<AudioSource>().clip = audioClips[0];
            else transform.GetComponent<AudioSource>().clip = audioClips[1];
            transform.GetComponent<AudioSource>().Play();
            if (t <= 1)
            {
                t += Time.fixedDeltaTime;
            }
            if (t>=1)
            {
                isDead = true;
                Destroy(transform.gameObject,1.0f);
                transform.GetComponent<SpriteRenderer>().enabled = false;
                enemy.GetComponent<Enemy>().Injured(attackPower);
            }
        }
    }
}
