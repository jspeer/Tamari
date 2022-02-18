using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float damage;
    public float Damage { get { return damage; } }
    [SerializeField] private float knockbackForce;
    public float KnockbackForce { get { return knockbackForce; } }

}
