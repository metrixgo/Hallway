using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class SlidingDoorScript : MonoBehaviour
{
    public AudioSource ad;
    private bool b;

    private void Start()
    {
        ad.volume = PlayerPrefs.GetFloat("SoundEffects", 80.0f);
        transform.position = new Vector3(35.0f, 10.0f, Random.Range(-3.5f, 3.5f));
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1) return;
        transform.Translate(Vector3.left * MainManager.Instance.speed * Time.deltaTime, Space.World);
        if (transform.position.x < 23 && !b)
        {
            b = true;
            transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z);
            StartCoroutine(ShutDoor(MainManager.Instance.speed));
        }
        if (transform.position.x < -9) Destroy(gameObject);
    }

    private IEnumerator ShutDoor(float speed)
    {
        float t = 0, l = 1.5f / speed;
        while (t < l)
        {
            t += Time.deltaTime;
            transform.Translate(0, -3.0f / l * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        ad.Play();
    }
}
