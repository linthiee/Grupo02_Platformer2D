using UnityEngine;

public class Boar : Enemy
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform currentTarget;

    private void Start()
    {
        currentTarget = pointA;
    }

    private void FixedUpdate()
    {
        Patrol();
    }
    protected override void Attack()
    {
    }

    protected override void Run()
    {

    }
}