using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] float stunTime = 2.0f;
    [SerializeField] float stunCooldown = 2.0f;
    [SerializeField] float curseTime = 5.0f;
    [SerializeField] float curseCooldown = 5.0f;
    [SerializeField] AudioSource curseAudioSource;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Main Enemy")
        {
            Debug.Log("����");
            if (player.isCursed || player.isCurseInvincible) return;   //�̹� ���ֻ����� ��� return
            StartCoroutine(Curse());
        }
        else if (other.tag == "Sub Enemy")
        {
            Debug.Log("����");
            if (player.isStunned || player.isStunInvincible) return;   //�̹� �������°ų� ���������� ��� return
            StartCoroutine(Stun());
        }
    }

    IEnumerator Curse()
    {
        player.Damage(35);
        if (player.currentHealth <= 0)
        {
            player.dieType = EDieType.Chased;
            yield break;
        }
        curseAudioSource.Play();
        player.isCursed = true;
        player.isCurseInvincible = true;
        yield return new WaitForSeconds(curseTime);
        player.isCursed = false;
        curseAudioSource.Pause();
        yield return new WaitForSeconds(curseCooldown);
        player.isCurseInvincible = false;
    }

    IEnumerator Stun()
    {
        player.Damage(10);
        if (player.currentHealth <= 0)
        {
            player.dieType = EDieType.Stunned;
            yield break;
        }
        player.isStunned = true;
        player.isStunInvincible = true;
        yield return new WaitForSeconds(stunTime);
        player.isStunned = false;
        yield return new WaitForSeconds(stunCooldown);
        player.isStunInvincible = false;
    }
}
