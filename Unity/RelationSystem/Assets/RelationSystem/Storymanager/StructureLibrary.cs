using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StorySegment
{
    public StorySegment(float climaxticEffect)
    {
        ClimacticEffect = climaxticEffect;
        action = default(NRelationSystem.MAction);
        PreferenceStrength = 0.0f;
    }

    public float ClimacticEffect { get; private set; }
    public NRelationSystem.MAction action;
    public float PreferenceStrength;
};

public class StructureLibrary {
    public List<List<StorySegment>> StoryStructures = new List<List<StorySegment>>();
    

	// Use this for initialization
	public StructureLibrary() {

        //Freytag's model

        List<StorySegment> freytag = new List<StorySegment>();

        freytag.Add(new StorySegment(0.0f));
        freytag.Add(new StorySegment(0.2f));
        freytag.Add(new StorySegment(0.8f));
        freytag.Add(new StorySegment(1.0f));
        freytag.Add(new StorySegment(0.8f));
        freytag.Add(new StorySegment(0.2f));
        freytag.Add(new StorySegment(0.0f));

        StoryStructures.Add(freytag);


        //Tri-punct model
        List<StorySegment> triPunct = new List<StorySegment>();
        triPunct.Add(new StorySegment(0.0f));
        triPunct.Add(new StorySegment(0.2f));
        triPunct.Add(new StorySegment(0.5f));
        triPunct.Add(new StorySegment(0.4f));
        triPunct.Add(new StorySegment(0.5f));
        triPunct.Add(new StorySegment(0.7f));
        triPunct.Add(new StorySegment(0.6f));
        triPunct.Add(new StorySegment(0.8f));
        triPunct.Add(new StorySegment(1.0f));
        triPunct.Add(new StorySegment(0.7f));
        triPunct.Add(new StorySegment(0.3f));

        StoryStructures.Add(triPunct);

    }
}
