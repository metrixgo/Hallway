using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance { get; private set; }

    public int gameState { get; private set; } = -1;
    public int levelType { get; private set; } = 0;
    private float time = 0;
    public float speed { get; private set; } = 3.0f;
    private float score = 0;
    private float start = 0;
    private float end = 200.0f;
    public GameObject startScreen;
    public GameObject optionScreen;
    public GameObject endScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreResult;
    public TextMeshProUGUI deathResult;
    public AudioSource musicPlayer;
    public AudioSource effectPlayer;
    public AudioClip menu;
    public AudioClip bgm;
    public AudioClip collision;
    public AudioClip selection;
    public AudioClip error;
    public Image img;
    public GameObject[] normalObstacles;
    public GameObject[] redObstacles;
    public GameObject[] factoryObstacles;
    public GameObject[] errorObstacles;
    public GameObject[] brownObstacles;
    private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz" +
        "0123456789" +
        "!@#$%^&*()-_=+[]{};:,.<>?/";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicPlayer.clip = bgm;
        musicPlayer.Play();
        musicPlayer.clip = menu;
        musicPlayer.Play();
        effectPlayer.spatialBlend = 0;
        SetMusic(PlayerPrefs.GetFloat("Music", 50.0f));
        SetSoundEffects(PlayerPrefs.GetFloat("SoundEffects", 80.0f));
        startScreen.SetActive(true);
        optionScreen.SetActive(false);
        endScreen.SetActive(false);
        scoreText.text = "";
        StartCoroutine(Setup());
    }

    public void SetMusic(float m)
    {
        musicPlayer.volume = m / 100.0f;
    }

    public void SetSoundEffects(float s)
    {
        effectPlayer.volume = s / 100.0f;
    }

    public void ChangeScreen(Color a, Color b, float c)
    {
        StartCoroutine(ChangeScreenCoroutine(a, b, c));
    }

    public void StartGame()
    {
        if (gameState != 0) return ;
        gameState = 1;
        effectPlayer.clip = selection;
        effectPlayer.Play();
        StartCoroutine(StartGameCoroutine());
    }

    public void GameOver(string s)
    {
        musicPlayer.Stop();
        effectPlayer.clip = collision;
        effectPlayer.Play();
        gameState = 2;
        scoreText.text = "";
        scoreResult.text = "You've managed to struggle for " + (int) Mathf.Floor(score) + " meters......";
        if (s.Contains("Error")) StartCoroutine(ErrorDeath());
        else if (s.Contains("Red Bed")) s = "having a wonderful dream on a bloody bed";
        else if (s.Contains("Bed")) s = "having a wonderful dream on a bed";
        else if (s.Contains("Box")) s = "tripping over some random boxes";
        else if (s.Contains("Wooden Column")) s = "bumping into a wooden column";
        else if (s.Contains("Column")) s = "bumping into a concrete column";
        else if (s.Contains("Side Door")) s = "getting pierced by thousands of tentacles";
        else if (s.Contains("Sliding Door")) s = "a sliding door appeared out of nowhere";
        else if (s.Contains("Door")) s = "opening the door to hell";
        else if (s.Contains("Red Wall")) s = "realizing the bloody wall in front of you";
        else if (s.Contains("Wall")) s = "realizing the gray wall in front of you";
        else if (s.Contains("Iron Frame")) s = "iron frames penetrated into every part of your body";
        else if (s.Contains("Wood Ball")) s = "a giant ball crushed your soul";
        else if (s.Contains("Wood Log")) s = "tripping over an unexpected log";
        else StartCoroutine(ErrorDeath());
        deathResult.text = "......before " + s + "......";
    }

    public void ShowResults()
    {
        endScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        if (gameState != 2) return;
        gameState = -1;
        effectPlayer.spatialBlend = 0;
        effectPlayer.clip = selection;
        effectPlayer.Play();
        StartCoroutine(RestartGame());
    }

    public void ShowOptions(bool show)
    {
        if (gameState != 0) return ;
        gameState = -1;
        effectPlayer.clip = selection;
        effectPlayer.Play();
        if (show) StartCoroutine(Show());
        else StartCoroutine(Hide());
    }

    public void QuitGame()
    {
        effectPlayer.clip = selection;
        effectPlayer.Play();
        Application.Quit();
    }

    private IEnumerator ErrorDeath()
    {
        while (true)
        {
            string temp = "";
            for (int i = 1; i <= 20; i++) temp += chars[Random.Range(0, chars.Length)];
            deathResult.text = "......ERROR " + temp + "......";
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator Show()
    {
        yield return StartCoroutine(ShowCanvas(startScreen, 0.25f, false));
        startScreen.SetActive(false);
        optionScreen.SetActive(true);
        yield return StartCoroutine(ShowCanvas(optionScreen, 0.25f, true));
        gameState = 0;
    }

    private IEnumerator Hide()
    {
        yield return StartCoroutine(ShowCanvas(optionScreen, 0.25f, false));
        optionScreen.SetActive(false);
        startScreen.SetActive(true);
        yield return StartCoroutine(ShowCanvas(startScreen, 0.25f, true));
        gameState = 0;
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(ShowCanvas(startScreen, 0.25f, false));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startScreen.SetActive(false);
        StartCoroutine(GenerateObstacles());
        StartCoroutine(IncreaseDifficulty());
        musicPlayer.clip = bgm;
        musicPlayer.Play();
    }

    private IEnumerator RestartGame()
    {
        yield return StartCoroutine(ShowCanvas(endScreen, 0.25f, false));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Setup()
    {
        yield return StartCoroutine(ChangeScreenCoroutine(Color.black, Color.clear, 0.25f));
        yield return StartCoroutine(ShowCanvas(startScreen, 0.25f, true));
        gameState = 0;
    }

    private IEnumerator ShowCanvas(GameObject canvas, float dur, bool show)
    {
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        float t = 0;
        if (show) cg.alpha = 0;
        else cg.alpha = 1;
        while (t < dur)
        {
            yield return null;
            t += Time.deltaTime;
            if(show) cg.alpha = t / dur;
            else cg.alpha = 1 - t / dur;
        }
        if (show) cg.alpha = 1;
        else cg.alpha = 0;
    }

    private IEnumerator ChangeScreenCoroutine(Color s, Color e, float dur)
    {
        float t = 0;
        img.color = s;
        while (t < dur)
        {
            yield return null;
            t += Time.deltaTime;
            img.color = Color.Lerp(s, e, t / dur);
        }
        img.color = e;
    }

    private IEnumerator GenerateObstacles()
    {
        while (true)
        {
            if (gameState != 1) break;
            GameObject o;
            if(levelType == 0) o = normalObstacles[Random.Range(0, normalObstacles.Length)];
            else if (levelType == 1) o = redObstacles[Random.Range(0, redObstacles.Length)];
            else if (levelType == 2) o = factoryObstacles[Random.Range(0, factoryObstacles.Length)];
            else if (levelType == 3) o = errorObstacles[Random.Range(0, errorObstacles.Length)];
            else o = brownObstacles[Random.Range(0, brownObstacles.Length)];
            Instantiate(o);
            if (levelType == 0) yield return new WaitForSeconds(4.0f / speed);
            else if (levelType == 3) yield return new WaitForSeconds(0.7f / speed);
            else yield return new WaitForSeconds(3.0f / speed);
        }
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            if (gameState != 1) break;
            time += Time.deltaTime;
            speed = Mathf.Log(time + 1.0f) / 1.5f + 3.0f;
            score += Time.deltaTime * speed;
            start += Time.deltaTime * speed;
            if(start >= end)
            {
                start = 0;
                end = Random.Range(100.0f, 200.0f);
                int temp = levelType;
                levelType = Random.Range(0, 5);
                if (levelType == 3)
                {
                    effectPlayer.clip = error;
                    effectPlayer.Play();
                }
            }
            scoreText.text = Mathf.Floor(score) + "m";
            yield return null;
        }
    }
}
