using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreaturePartsCombiner : MonoBehaviour
{
    public Transform headPosition;
    public Transform torsoPosition;
    public Transform bottomPosition;

    public List<CreaturePart> allCreatureParts;

    List<CreaturePart> headCreatureParts;
    List<CreaturePart> torsoCreatureParts;
    List<CreaturePart> bottomCreatureParts;
    Dictionary<CreaturePartType, int> selectedPartIndices;

    CreaturePart selectedHeadPart;
    CreaturePart selectedTorsoPart;
    CreaturePart selectedBottomPart;

    private void Start()
    {
        MiniGameEvents.miniGameStartedEvent.AddListener(OnStart);
    }

    void OnStart()
    {
        InitCreaturePartsLists();
        InitSelectedParts();
    }

    void InitCreaturePartsLists()
    {
        headCreatureParts = new List<CreaturePart>();
        torsoCreatureParts = new List<CreaturePart>();
        bottomCreatureParts = new List<CreaturePart>();

        foreach (var part in allCreatureParts)
        {
            AddPartToAppropriateList(part);
        }
    }

    void AddPartToAppropriateList(CreaturePart part)
    {
        switch(part.type)
        {
            case CreaturePartType.Head:
                headCreatureParts.Add(part);
                break;
            case CreaturePartType.Torso:
                torsoCreatureParts.Add(part);
                break;
            case CreaturePartType.Bottom:
                bottomCreatureParts.Add(part);
                break;
        }
    }

    public void InitSelectedParts()
    {
        selectedPartIndices = new Dictionary<CreaturePartType, int> {{ CreaturePartType.Head, SelectRandomIndexFromList(headCreatureParts) },
            { CreaturePartType.Torso, SelectRandomIndexFromList(torsoCreatureParts) }, {CreaturePartType.Bottom, SelectRandomIndexFromList(bottomCreatureParts) } };

        ChangePart(headCreatureParts[selectedPartIndices[CreaturePartType.Head]]);
        ChangePart(torsoCreatureParts[selectedPartIndices[CreaturePartType.Torso]]);
        ChangePart(bottomCreatureParts[selectedPartIndices[CreaturePartType.Bottom]]);
    }

    int SelectRandomIndexFromList(List<CreaturePart> list)
    {
        return Random.Range(0, list.Count);
    }

    void ChangePart(CreaturePart newPart)
    {
        GameObject spawnedPart = null;

        CreaturePart relevantSelectedPart = null;

        switch(newPart.type)
        {
            case CreaturePartType.Head:
                spawnedPart = GameObject.Instantiate(newPart.gameObject, headPosition.position, headPosition.rotation, headPosition);
                relevantSelectedPart = selectedHeadPart;
                break;
            case CreaturePartType.Torso:
                spawnedPart = GameObject.Instantiate(newPart.gameObject, torsoPosition.position, torsoPosition.rotation, torsoPosition);
                relevantSelectedPart = selectedTorsoPart;
                break;
            case CreaturePartType.Bottom:
                spawnedPart = GameObject.Instantiate(newPart.gameObject, bottomPosition.position, bottomPosition.rotation, bottomPosition);
                relevantSelectedPart = selectedBottomPart;
                break;
        }
        

        if(relevantSelectedPart)
        {
            Destroy(relevantSelectedPart.gameObject);
        }

        switch(newPart.type)
        {
            case CreaturePartType.Head:
                selectedHeadPart = spawnedPart.GetComponent<CreaturePart>();
                break;
            case CreaturePartType.Torso:
                selectedTorsoPart = spawnedPart.GetComponent<CreaturePart>();
                break;
            case CreaturePartType.Bottom:
                selectedBottomPart = spawnedPart.GetComponent<CreaturePart>();
                break;
        }
    }

    //=============================

    public void ChangePartToNext(int typeInt)
    {
        CreaturePartType type = (CreaturePartType)typeInt;

        int currentlySelectedIndex = selectedPartIndices[type];

        switch(type)
        {
            case CreaturePartType.Head:
                selectedPartIndices[CreaturePartType.Head] = GetNextIndexInList(headCreatureParts, currentlySelectedIndex);

                ChangePart(headCreatureParts[selectedPartIndices[CreaturePartType.Head]]);
                break;
            case CreaturePartType.Torso:
                selectedPartIndices[CreaturePartType.Torso] = GetNextIndexInList(torsoCreatureParts, currentlySelectedIndex);

                ChangePart(torsoCreatureParts[selectedPartIndices[CreaturePartType.Torso]]);
                break;
            case CreaturePartType.Bottom:
                selectedPartIndices[CreaturePartType.Bottom] = GetNextIndexInList(bottomCreatureParts, currentlySelectedIndex);

                ChangePart(bottomCreatureParts[selectedPartIndices[CreaturePartType.Bottom]]);
                break;
        }
    }

    public void ChangePartToPrevious(int typeInt)
    {
        CreaturePartType type = (CreaturePartType)typeInt;

        int currentlySelectedIndex = selectedPartIndices[type];

        switch (type)
        {
            case CreaturePartType.Head:
                selectedPartIndices[CreaturePartType.Head] = GetPreviousIndexInList(headCreatureParts, currentlySelectedIndex);

                ChangePart(headCreatureParts[selectedPartIndices[CreaturePartType.Head]]);
                break;
            case CreaturePartType.Torso:
                selectedPartIndices[CreaturePartType.Torso] = GetPreviousIndexInList(torsoCreatureParts, currentlySelectedIndex);

                ChangePart(torsoCreatureParts[selectedPartIndices[CreaturePartType.Torso]]);
                break;
            case CreaturePartType.Bottom:
                selectedPartIndices[CreaturePartType.Bottom] = GetPreviousIndexInList(bottomCreatureParts, currentlySelectedIndex);

                ChangePart(bottomCreatureParts[selectedPartIndices[CreaturePartType.Bottom]]);
                break;
        }
    }

    private int GetNextIndexInList(List<CreaturePart> list, int currentIndex)
    {
        return ++currentIndex % list.Count;
    }

    private int GetPreviousIndexInList(List<CreaturePart> list, int currentIndex)
    {
        return (currentIndex + (list.Count - 1)) % list.Count;
    }


    public Creature GetCurrentCreature()
    {
        Creature creature = new Creature();
        creature.parts = new List<CreaturePart> { selectedHeadPart, selectedTorsoPart, selectedBottomPart };
        return creature;
    }
}
