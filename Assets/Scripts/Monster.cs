using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AudioSource ad;
    public AudioClip scream;

    public void Kill()
    {
        StartCoroutine(MoveToPlayer());
    }

    private IEnumerator MoveToPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        float t = 0;
        bool flg = false;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            if (!flg && t > 0.7f)
            {
                flg = true;
                ad.spatialBlend = 0;
                ad.clip = scream;
                ad.Play();
            }
            transform.position = new Vector3(Mathf.Lerp(-6, 15, t), transform.position.y, transform.position.z);
            yield return null;
        }
        MainManager.Instance.ChangeScreen(Color.red, Color.red, 0f);
        yield return new WaitForSeconds(0.2f);
        MainManager.Instance.ChangeScreen(Color.red, Color.black, 0.5f);
        yield return new WaitForSeconds(0.6f);
        MainManager.Instance.ShowResults();
    }
}
