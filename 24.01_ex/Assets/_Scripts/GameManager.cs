using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int coin = 0;
    public int life = 3;
    public float gameTime = 60.0f;
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textCoin;
    public TextMeshProUGUI textLife;
    public static GameManager instance;
    GameObject playerObject;
    PlayerMovement player;

    public int Coin
    {
        get { return coin; }
        set
        {
            if (value != coin)
            {
                coin = value;
            }
        }
    }
    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerMovement>();
        life = player.GetHp();

    }
    void Start()
    {
        instance = this;
        
        textTime.text = "Time: " + gameTime;
        textCoin.text = "Coin: " + coin;
        textLife.text = "Life: " + life;
    }
    void Update()
    {
        if(life != player.GetHp())
        {
            life = player.GetHp();
            textLife.text = "Life: " + life;

            if(life <= 0)
            {
                life = 0;
                textLife.text = "Life: " + life;
            }
        }
        gameTime -= Time.deltaTime;
        if (gameTime < 0)
        {
            SceneManager.LoadScene("Fail");
        }
        textTime.text = "Time: " + (int)gameTime;
        textCoin.text = "Coin: " + coin;
    }
}