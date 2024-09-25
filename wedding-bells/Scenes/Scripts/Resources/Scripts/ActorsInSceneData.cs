using Godot;
using System;
using Godot.Collections;

[Tool]
[GlobalClass]
// This class is supposed to hold the data for all the characters in the game,
// it could also be split into the different scenes or types of characters, like couples,
// vendors, random people.
// I made this so we don't need an object in every scene holding the data for the characters
// and so neither the writers and programmers need to update the dialoguemanager with the paths
// of the different sprite images

// I originally just wanted to have the dictionary directly in the dialoguemanager
// but that ended up causing a lot of issues, so I just created this resource that
// holds the data for all the characters in the game, then the dialoguemanager takes
// this script and steals its dictionary
public partial class ActorsInSceneData : Resource
{
    //The following is a dictionary for all the actors and their sprites in this scene
    //Dictionary can't be exported in regular nodes for some reason
    [Export] public Dictionary<string, ActorData> Actors = new Dictionary<string, ActorData>();
    [Export] private ActorData _actorData;
    [Export] public bool Add {get => false; set => AddEntry(_actorData);}
    [Export] private string _actorToRemove;
    [Export] public bool Remove {get => false; set => RemoveEntry(_actorToRemove);}
    [Export] public bool Check {get => false; set => CheckEntries();}
    
    public void AddEntry(ActorData data)
    {
        Actors.Add(data._actorName, data);
    }
	
    public void RemoveEntry(string name)
    {
        Actors.Remove(name);
    }

    public void CheckEntries()
    {
        GD.Print("Current Actors in Dictionary are:");
        foreach (string name in Actors.Keys)
        {
            GD.Print(name);
            //GD.Print(Actors[name]._actorName); was just testing using dictionaries here
        }
    }
}
