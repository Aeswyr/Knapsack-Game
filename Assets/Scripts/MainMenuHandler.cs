using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;
using TMPro;
public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private TextMeshProUGUI textBox; 
    public void OnHostPressed() {
        networkManager.StartHost();
        networkManager.ServerChangeScene("GameScene");
    }

    public void OnJoinPressed() {
        networkManager.networkAddress = textBox.text;
        networkManager.StartClient();
        
    }
}
