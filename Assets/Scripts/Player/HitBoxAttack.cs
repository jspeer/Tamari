using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAttack : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag) {
            case "Breakable":
                StartCoroutine(other.GetComponent<Breakable>().Break());
                break;
            case "Enemy":
                other.SendMessage("ReceiveKnockbackMessage", new object[2]{transform.position, player.KnockbackForce}, SendMessageOptions.DontRequireReceiver);

                // TODO: calculate damage applied?
                float damageApplied = 0f;
                other.SendMessage("ReceiveDamageMessage", new object[2]{gameObject, damageApplied}, SendMessageOptions.DontRequireReceiver);
                // TODO maybe use method below instaed of send message? -- see breakable
                // other.GetComponent<BaseEnemy>().ReceiveDamage(damageApplied);
                break;
        }
    }
}
