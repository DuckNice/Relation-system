using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StoryPredicter : MonoBehaviour {
    public static List<StructureContainer> Get5BestStructures(List<StorySegment> structure)
    {
        List<StructureContainer> bestStructures = new List<StructureContainer>();

        //TODO: this makes time static, make a dynamic way to do that.

        int timeForEachSegment = 120 / structure.Count;

        List<List<NRelationSystem.HistoryItem>> segmentedItems = new List<List<NRelationSystem.HistoryItem>>();

        int q = 0;

        for (int i = 0; i < structure.Count; i++)
        {
            segmentedItems.Add(new List<NRelationSystem.HistoryItem>());

            while(q < Program.relationSystem.historyBook.Count && Program.relationSystem.historyBook[q].GetTime() < timeForEachSegment * i)
            {
                segmentedItems[i].Add(Program.relationSystem.historyBook[q]);

                q++;
            }

            if (q >= Program.relationSystem.historyBook.Count)
                break;
        }

        List<float> storySoFar = new List<float>();

        for (int i = 0; i < segmentedItems.Count; i++)
        {
            float go = 0f;

            foreach(NRelationSystem.HistoryItem histItem in segmentedItems[i])
            {
                //TODO make fallback or fix actions with big/small letters
                if(EventLibrary.ActionDramas.ContainsKey(histItem.GetAction().name))
                go += EventLibrary.ActionDramas[histItem.GetAction().name];
            }

            storySoFar.Add(go);
        }



        for (int i = 0; i < 5; i++)
        {
            bestStructures.Add(StructureCreator(structure, storySoFar));
        }

        return bestStructures;
    }


    public static StructureContainer StructureCreator(List<StorySegment> structure, List<float> storySoFar)
    {
        float fitness = 0;

        List<StorySegment> go = new List<StorySegment>();

        int i = 0; 
        while (fitness < 0.8f && i < 10)
        {
            i++;

            int index = Random.Range(0, EventLibrary.ActionDramas.Count - 1);

            if(EventLibrary.ActionDramas.Values.ToList()[index] < structure[storySoFar.Count -1].ClimacticEffect - storySoFar[storySoFar.Count-1])
            {
                StorySegment newSegment = new StorySegment(structure[storySoFar.Count - 1].ClimacticEffect);
                newSegment.PreferenceStrength = 0.2f;

                //TODO make fallback or fix actions with big/small letters
                if (!Program.relationSystem.posActions.ContainsKey(EventLibrary.ActionDramas.Keys.ToList()[index]))
                    continue;

                newSegment.action = Program.relationSystem.posActions[EventLibrary.ActionDramas.Keys.ToList()[index]];
                fitness += EventLibrary.ActionDramas.Values.ToList()[index];
                go.Add(newSegment);
            }

        }

        
        return new StructureContainer(go, fitness);
    }


    public static List<StorySegment> RecursiveStorySegment(int maxRecursionsLeft, ref float fitness)
    {
        return RecursiveStorySegment(maxRecursionsLeft - 1, ref fitness);
    }
}