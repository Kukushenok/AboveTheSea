using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gg_rotate : MonoBehaviour
{
    public float y_min, y_max, cameraspeed_X, cameraspeed_Y;
    InputSkript input;
    float x, y;
    Transform targetT;
    // Start is called before the first frame update
    void Start()
    {
        targetT = transform.GetChild(0).GetComponent<Transform>();
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (input.mouseX >= 0.05f || input.mouseX <= -0.05f)
            x += input.mouseX * cameraspeed_X;
        //   else if (input.joystickX >= 0.05f || input.joystickX <= -0.05f)
        //       x += input.joystickX * cameraspeed_X * Time.deltaTime * 250;
        if (x > 360)
            x -= 360;
        if (x < -360)
            x += 360;
        if (input.mouseY >= 0.05f || input.mouseY <= -0.05f)
            y += input.mouseY * -cameraspeed_Y;
        //   else if (input.joystickY >= 0.05f || input.joystickY <= -0.05f)
        //      y += input.joystickY * -cameraspeed_Y * Time.deltaTime * 250;
        y = Mathf.Clamp(y, y_min, y_max);
        targetT.eulerAngles = new Vector3(0, transform.rotation.ToEulerAngles().y * Mathf.Rad2Deg, 0);
        transform.eulerAngles = new Vector3(y, x, 0);
    }
}
