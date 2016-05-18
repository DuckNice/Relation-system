using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NRelationSystem;

public class CentralStoryManager : MonoBehaviour {
    StructureLibrary strucLib = new StructureLibrary();
    EventLibrary eventLib = new EventLibrary();

    List<StorySegment> currentStory = new List<StorySegment>();
    WaitForSeconds selectionWaiter = new WaitForSeconds(5);
    public List<Being> beings;

    void Start () {
        
        StartCoroutine(StorySelector());
    }
    
    
    protected IEnumerator StorySelector()
    {
        while (true)
        {
            try
            {
                currentStory = StoryRecognizer.PredictClosestStructure(Program.relationSystem.GetAllPeople(), strucLib);

                Being.actionPreferenceModifiers.Clear();

                foreach (StorySegment segment in currentStory)
                {
                    if (!Being.actionPreferenceModifiers.ContainsKey(segment.action))
                        Being.actionPreferenceModifiers.Add(segment.action, segment.PreferenceStrength);
                    else
                        Being.actionPreferenceModifiers[segment.action] += segment.PreferenceStrength;
                }
            }
            catch { }

            yield return selectionWaiter;
        }
    }
}