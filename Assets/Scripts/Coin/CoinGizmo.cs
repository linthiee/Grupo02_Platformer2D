using UnityEngine;

public class CoinGizmo : MonoBehaviour
{
    // Este script se adjunta
    // a un objeto vacío que representa el punto de spawn del jugador.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    //el Gizmo se dibuja en la escena cuando el objeto está seleccionado, lo que permite

    //al desarrollador ver la posición del spawn point de manera más clara.
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 0.5f);
    //}
}