using System.Collections;
using UnityEngine;

public class SideDoorScript : MonoBehaviour
{
    private bool b;
    public bool isLeft;
    public GameObject door;
    public GameObject lamps;
    public AudioSource ad;

    private void Start()
    {
        ad.volume = PlayerPrefs.GetFloat("SoundEffects", 80.0f);
        if (isLeft) transform.position = new Vector3(35.0f, 0, 4.45f);
        else transform.position = new Vector3(35.0f, 0, -4.45f);
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1) return;
        transform.Translate(Vector3.left * MainManager.Instance.speed * Time.deltaTime, Space.World);
        if(transform.position.x < 22 && !b)
        {
            b = true;
            StartCoroutine(OpenDoor(MainManager.Instance.speed));
        }
        if (transform.position.x < -9) Destroy(gameObject);
    }

    private IEnumerator OpenDoor(float speed)
    {
        ad.Play();
        float t = 0, l = 1.5f / speed;
        while (t < l)
        {
            t += Time.deltaTime;
            if (t < 0.5f * l)
            {
                if (isLeft) door.transform.Rotate(0, 180.0f / l * Time.deltaTime, 0);
                else door.transform.Rotate(0, -180.0f / l * Time.deltaTime, 0);
            }
            lamps.transform.Translate(0, 0, -2.0f / l * Time.deltaTime);
            yield return null;
        }
    }
}
