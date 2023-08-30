using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewind : MonoBehaviour
{

    [System.Serializable]
    public struct PositionRotation
    {
        public Vector3 position;
        public Quaternion rotation;

        public PositionRotation(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    private List<PositionRotation> _storedData = new List<PositionRotation>();
    private float _timeInterval = 1.0f; //Interval to store the posrot ( every second)
    private float _maxStoredTime = 3.0f; //Max time to store the posrot (max 3 sec) also is the amount of time you go back in time
    private float l_astStoredTime;

    private CharacterController _controller;

   // private GameObject _rewindVisual;
    //[SerializeField] private GameObject _rewindVisualPrefab;

    private void Start()
    {
        l_astStoredTime = Time.time;
        _controller = GetComponent<CharacterController>();
        //_rewindVisual = GameObject.Instantiate(_rewindVisualPrefab, _controller.transform);
    }

    private void Update()
    {
        //Store pos/rot every x time
        if (Time.time - l_astStoredTime >= _timeInterval)
        {
            l_astStoredTime = Time.time;
            StorePosRot();

           // _rewindVisual.transform.position = storedData[0].position;
          //  _rewindVisual.transform.rotation = storedData[0].rotation;
        }
    }

    private void StorePosRot()
    {
        PositionRotation posRot = new PositionRotation(transform.position, transform.rotation);
        _storedData.Add(posRot);



        //Remove oldest data(everything older than time that you woudl rewind)
        if (_storedData.Count > Mathf.FloorToInt(_maxStoredTime / _timeInterval))
        {
            _storedData.RemoveAt(0);
        }
    }

    public void Rewind()
    {
        if (_storedData.Count > 0)
        {
            PositionRotation posRot = _storedData[0];

            _controller.enabled = false;
            transform.position = posRot.position;
            transform.rotation = posRot.rotation;
            _controller.enabled = true;

            _storedData.RemoveAt(0);
        }
    }


}
