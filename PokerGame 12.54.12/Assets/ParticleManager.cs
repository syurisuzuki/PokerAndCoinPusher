using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

    [SerializeField]
    ParticleSystem tapEffect;
    [SerializeField]
    Camera _camera;
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            var pos = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward * -10);
            tapEffect.transform.position = pos;
            tapEffect.Emit(30);
        }
	
	}
}
