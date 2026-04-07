using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    private Vector3 _randomRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetRandomRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetRandomRotation()
    {
        //Set Random Rotation for the GameObject On X,Y,Z 
        _randomRotation = new Vector3(
            Random.Range(0, 360f),
            Random.Range(0, 360f),
            Random.Range(0, 360f)
            );

        transform.eulerAngles = _randomRotation;
    }
}
