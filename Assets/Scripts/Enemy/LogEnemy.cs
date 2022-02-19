using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEnemy : BaseEnemy
{
    [Header("Log Specific")]
    [SerializeField] float wakeUpRange;

    new private void Awake()
    {
        base.Awake();
    }

    new private void Start()
    {
        base.Start();
    }

    new private void Update()
    {
        base.Update();
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
