using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanInput : MonoBehaviour
{
    public class InputData
    {
        public float Horizontal;
        public float Vertical;
    }

    private InputData _input;

    private readonly string Horizontal = "Horizontal";
    private readonly string Vertical = "Vertical";

    public void Initialize()
    {
        _input = new InputData();
    }

    public void GetInput()
    {
        float horizontal = Input.GetAxisRaw(Horizontal);
        float vertical = Input.GetAxisRaw(Vertical);

        if(horizontal != 0)
        {
            _input.Vertical = 0.0f;
            _input.Horizontal = horizontal;
        }
        else
        {
            _input.Horizontal = horizontal;
        }

        if (vertical != 0)
        {
            _input.Horizontal = 0.0f;
            _input.Vertical = vertical;
        }
        else
        {
            _input.Vertical = vertical;
        }
    }

    public InputData GetInputData()
    {
        return _input;
    }
}
