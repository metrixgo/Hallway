using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    private int currentType = 0;
    public GameObject normal;
    public GameObject red;
    public GameObject factory;
    public GameObject error;
    public GameObject brown;

    private void Update()
    {
        if (MainManager.Instance.gameState == 2) return;
        transform.Translate(Vector3.left * MainManager.Instance.speed * Time.deltaTime);
        if (transform.position.x <= -3)
        {
            transform.Translate(36.0f, 0, 0);
            if (MainManager.Instance.levelType != currentType) switchType(MainManager.Instance.levelType);
        }
    }

    private void switchType(int type)
    {
        currentType = type;
        normal.SetActive(false);
        red.SetActive(false);
        factory.SetActive(false);
        error.SetActive(false);
        brown.SetActive(false);
        if (type == 0) normal.SetActive(true);
        else if (type == 1) red.SetActive(true);
        else if (type == 2) factory.SetActive(true);
        else if (type == 3) error.SetActive(true);
        else if (type == 4) brown.SetActive(true);
    }

}
