using UnityEngine;

public class WoodenColumnScript : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(35.0f, transform.position.y, -8.0f * (Random.Range(0, 2) - 0.5f));
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1) return;
        transform.Translate(Vector3.left * MainManager.Instance.speed * Time.deltaTime, Space.World);
        if (transform.position.x < -9) Destroy(gameObject);
    }
}
