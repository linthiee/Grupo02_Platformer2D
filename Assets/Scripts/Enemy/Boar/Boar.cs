using UnityEngine;

public class Boar : Enemy
{
    protected override void Attack()
    {
    }
    
    protected override void Run()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Patrol();  
    }
}
