using UnityEngine;

public class MoveRollingObstacles : MonoBehaviour
{
    public AudioSource ad;

    private void Start()
    {
        ad.volume = PlayerPrefs.GetFloat("SoundEffects", 80.0f);
        transform.position = new Vector3(35.0f, transform.position.y, Random.Range(-3f, 3f));
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1)
        {
            ad.Stop();
            return ;
        }
        transform.Translate(Vector3.left * MainManager.Instance.speed * 1.3f * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * MainManager.Instance.speed * 40.0f * Time.deltaTime, Space.World);
        if (transform.position.x < -9) Destroy(gameObject);
    }
}
