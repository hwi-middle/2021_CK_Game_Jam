using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] float stunTime = 2.0f;
    [SerializeField] float stunCooldown = 2.0f;

    private bool isInvincible = false;
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
            player.isDead = false;
        }
        else if (other.tag == "Sub Enemy")
        {
            if (player.isStunned || isInvincible) return;   //이미 기절상태거나 무적상태일 경우 return
            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun()
    {
        player.isStunned = true;
        isInvincible = true;
        yield return new WaitForSeconds(stunTime);
        player.isStunned = false;
        yield return new WaitForSeconds(stunCooldown);
        isInvincible = false;
    }
}
