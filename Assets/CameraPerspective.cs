using UnityEngine;

public class CameraPerspective : MonoBehaviour
{
    public static CameraPerspective Instance;
    public float focalLength = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetPerspective(float zPos)
    {
        return focalLength / (focalLength + zPos);
    }
}
