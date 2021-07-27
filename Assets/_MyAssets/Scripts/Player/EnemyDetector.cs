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
            
            if (player.isCursed || player.isCurseInvincible) return;   //이미 저주상태일 경우 return
            StartCoroutine(Curse());
        }
        else if (other.tag == "Sub Enemy")
        {
            if (player.isStunned || player.isStunInvincible) return;   //이미 기절상태거나 무적상태일 경우 return
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
        float t1 = Time.time;
        Debug.Log(Time.timeScale);

        yield return new WaitForSeconds(curseTime);
        Debug.Log(Time.time - t1);
        player.isCursed = false;
        curseAudioSource.Pause();
        yield return new WaitForSeconds(curseCooldown);
        player.isCurseInvincible = false;
    }

    IEnumerator Stun()
    {
        Debug.Log("기절");
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
