using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Renderer _renderer = null;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time);

        if (Input.GetKeyDown(KeyCode.I) && _renderer.enabled)
        {
            _renderer.enabled = false;

            StartCoroutine(ShowCubeRoutine());
        }
    }

    IEnumerator ShowCubeRoutine()
    {
        yield return new WaitForSeconds(5);

        _renderer.enabled = true;
    }
}
