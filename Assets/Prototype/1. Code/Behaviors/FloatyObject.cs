using UnityEngine;

public class FloatyObject : MonoBehaviour
{
    private Vector3 _rotationSpeed;
    private Vector3 _floatAmplitude;
    private Vector3 _floatFrequency;
    private Vector3 _startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetFloatyValue();
    }

    // Update is called once per frame
    void Update()
    {
        ObjFloating();
    }

    private void SetFloatyValue()
    {
        _startPosition = transform.position;

        _rotationSpeed = new Vector3(
            Random.Range(10f, 50f),
            Random.Range(10f, 50f),
            Random.Range(10f, 50f)
            );

        _floatAmplitude = new Vector3(
            Random.Range(0.1f, 0.5f),
            Random.Range(0.1f, 0.5f),
            Random.Range(0.1f, 0.5f)
            );

        _floatFrequency = new Vector3(
            Random.Range(0.5f, 1.5f),
            Random.Range(0.5f, 1.5f),
            Random.Range(0.5f, 1.5f)
            );
    }

    private void ObjFloating()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);

        Vector3 floatOffset = new Vector3(
            Mathf.Sin(Time.time * _floatFrequency.x) * _floatAmplitude.x,
            Mathf.Sin(Time.time * _floatFrequency.y) * _floatAmplitude.y,
            Mathf.Sin(Time.time * _floatFrequency.z) * _floatAmplitude.z
            );

        transform.position = _startPosition + floatOffset;
    }
}
