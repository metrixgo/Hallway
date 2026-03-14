using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(35.0f, transform.position.y, Random.Range(-4.0f, 4.0f));
        transform.Rotate(0, Random.Range(0, 360.0f), 0);
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1) return ;
        transform.Translate(Vector3.left * MainManager.Instance.speed * Time.deltaTime, Space.World);
        if (transform.position.x < -9) Destroy(gameObject);
    }
}
