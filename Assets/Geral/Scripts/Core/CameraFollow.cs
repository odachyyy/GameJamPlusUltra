using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [Header("Configurações do Alvo")]
    [SerializeField] private string playerTag = "Player";
    private Transform target;

    [Header("Configurações da Câmera")]
    [Tooltip("Quão suave a câmera segue. Menor = mais rápido.")]
    [SerializeField] private float smoothSpeed = 0.125f; 
    
    [Tooltip("O 'zoom' da câmera. Maior = mais longe.")]
    [SerializeField] private float orthographicSize = 8f; 
    
    [Tooltip("O deslocamento da câmera em relação ao Player.")]
    [SerializeField] private Vector3 offset;


    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = orthographicSize;
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    private void Start()
    {
        if (target == null)
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("CameraFollow: Não foi possível encontrar o Player na cena!");
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            FindPlayer(); 
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
