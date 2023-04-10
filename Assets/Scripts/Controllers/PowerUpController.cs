using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public float bounceSpeed = 8;
    public float bounceAmplitude = 0.05f;
    public float rotationSpeed = 90;

    private float startHeight;
    private float timeOffset;

    // start is called before the first frame update
    void Start()
    {
        startHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
    }

    // update is called once per frame
    void Update()
    {
        animate();
        spin();
    }

    void animate()
    {
        float finalheight = startHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        Vector3 position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;
    }

    void spin()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}