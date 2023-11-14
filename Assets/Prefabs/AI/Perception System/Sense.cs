using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sense : ScriptableObject
{
    public delegate void OnPerceptionUpdated(PerceptionStimuli stimuli, bool successfullySensed);
    public event OnPerceptionUpdated onPerceptionUpdated;

    HashSet<PerceptionStimuli> currentlyPerceivableStimuli = new HashSet<PerceptionStimuli>();
    static HashSet<PerceptionStimuli> registeredStims = new HashSet<PerceptionStimuli>();

    [SerializeField] float forgetTime = 2f;

    Dictionary<PerceptionStimuli, Coroutine> currentForgettingCoroutines = new Dictionary<PerceptionStimuli, Coroutine> { };

   
    public MonoBehaviour Owner
    {
        get;
        private set;
    }

    public virtual void Init(MonoBehaviour owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        foreach(PerceptionStimuli stimuli in registeredStims)
        {
            if(IsStimuliSensable(stimuli) && !IsStimuliSensed(stimuli)) //if currently sensable but was not previously sensed
            {
                currentlyPerceivableStimuli.Add(stimuli);
                if(currentForgettingCoroutines.ContainsKey(stimuli))
                {
                    StopForgettingStimuli(stimuli);
                }
                else
                {
                    Debug.Log($"I Sensed: {stimuli.gameObject.name}");
                    onPerceptionUpdated.Invoke(stimuli, true);
                }
            }

            if(!IsStimuliSensable(stimuli) && IsStimuliSensed(stimuli)) //if is not sensable but was previously sensed
            {
                currentlyPerceivableStimuli.Remove(stimuli);

                StartForgettingStimuli(stimuli);

            }
        }

    }

    private void StopForgettingStimuli(PerceptionStimuli stimuli)
    {
        Owner.StopCoroutine(currentForgettingCoroutines[stimuli]);
        currentForgettingCoroutines.Remove(stimuli);
    }

    private void StartForgettingStimuli(PerceptionStimuli stimuli)
    {
        Coroutine forgettingCoroutine = Owner.StartCoroutine(ForgettingCoroutine(stimuli));
        currentForgettingCoroutines.Add(stimuli, forgettingCoroutine);
    }

    private IEnumerator ForgettingCoroutine(PerceptionStimuli stimuli)
    {
        yield return new WaitForSeconds(forgetTime);
        currentForgettingCoroutines.Remove(stimuli); 
        onPerceptionUpdated.Invoke(stimuli, false);
        Debug.Log($"I Lost Track of: {stimuli.gameObject.name}");

    }

    private bool IsStimuliSensed(PerceptionStimuli stimuli)
    {
        return currentlyPerceivableStimuli.Contains(stimuli);
    }

    public abstract bool IsStimuliSensable(PerceptionStimuli stimuli);



    static public void RegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStims.Add(stimuli);
    }

    static public void UnRegisterStimuli(PerceptionStimuli stimuli)
    {
        registeredStims.Remove(stimuli);
    }

    public virtual void DrawDebug()
    {
        
    }

}
