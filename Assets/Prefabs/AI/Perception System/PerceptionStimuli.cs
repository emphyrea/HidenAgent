using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionStimuli : MonoBehaviour
{
    private void Awake()
    {
        Sense.RegisterStimuli(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Sense.UnRegisterStimuli(this);
    }
}
