using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Monster monster;
    public float sensitivity = 10.0f;

    public void SetSensitivity(float s)
    {
        sensitivity = s;
    }

    private void Start()
    {
        SetSensitivity(PlayerPrefs.GetFloat("Sensitivity", 10.0f));
    }

    private void Update()
    {
        if (MainManager.Instance.gameState != 1) return ;
        transform.Translate(Vector3.right * Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -4, 4));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && MainManager.Instance.gameState == 1)
        {
            MainManager.Instance.GameOver(other.gameObject.name);
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        float t = 0f;
        Color eRed = new Color(1.0f, 0, 0, 0.6f);
        Color sRed = new Color(1.0f, 0, 0, 0);
        MainManager.Instance.ChangeScreen(sRed, eRed, 0.2f);
        while(t < 0.2f)
        {
            t += Time.deltaTime;
            transform.Rotate(20.0f * Time.deltaTime, 0, 0);
            transform.Translate(-1.0f * Time.deltaTime, 0, 0, Space.World);
            yield return null;
        }
        MainManager.Instance.ChangeScreen(eRed, sRed, 0.2f);
        while (t > 0)
        {
            t -= Time.deltaTime;
            transform.Rotate(20.0f * Time.deltaTime, 0, 0);
            transform.Translate(-1.0f * Time.deltaTime, 0, 0, Space.World);
            yield return null;
        }

        t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            transform.Rotate(20.0f * Time.deltaTime, 0, 0);
            transform.Translate(-1.0f * Time.deltaTime, 0, 0, Space.World);
            yield return null;
        }

        t = 0;
        monster.Kill();
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.Rotate(-20.0f * Time.deltaTime, 0, 0);
            transform.Rotate(0, 180.0f * Time.deltaTime, 0, Space.World);
            transform.Translate(-1.0f * Time.deltaTime, 0, 0, Space.World);
            yield return null;
        }

        t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            transform.Rotate(-80.0f * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

}
