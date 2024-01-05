using UnityEngine;
using UnityEngine.SceneManagement;

public class Go_Scene : MonoBehaviour
{
    public string SceneName;
    public void GoScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}