using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
// Resource class where all the sprites for one character are kept.
// This could possibly be used to store data on characters such as satisfaction,
// relationship strength, etc. For now it just holds a dictionary of
// sprites for dialogue use.
public partial class ActorData : Resource
{
	//I realized the </summary> comments don't appear in the inspector unless we use GDScript
	
	// The key here is supposed to be a string that describes
	// the expression of the sprite such as "Happy", "Sad", "Neutral", stuff like that.
	// Idk if this is actually how it's used, but I think this should give the
	// writer an easier time picking sprites when making the script.
	/// <summary>
	/// Dictionary containing a texture as the value and its associated name as the key.
	/// </summary>
	[Export] public Dictionary<string, Texture2D> ActorSprites = new Dictionary<string, Texture2D>();
	/// <summary>
	/// Name of the sprite to be added
	/// </summary>
	[Export] private string _spriteName;
	/// <summary>
	/// The sprite's texture
	/// </summary>
	[Export] private Texture2D _spriteTexture;
	// Checking this bool immediately adds the previously entered name and texture to the dictionary.
	// If there is another identical combination, it will throw an error.
	/// <summary>
	/// Pressing this button will immediately add the sprite and texture currently contained by the previous
	/// two parameters into the dictionary.
	/// The dictionary will not visually update until you move the change what you are currently inspecting
	/// and return to this resource, so don't worry if you don't see it in the dictionary immediately, it's
	/// there.
	/// </summary>
	[Export] public bool Add {get => false; set => AddEntry(_spriteName, _spriteTexture);}

	[Export] public string _actorName;

	public ActorData()
	{
		_actorName = "";
	}
	public void AddEntry(string name, Texture2D texture)
	{
		ActorSprites.Add(name, texture);
	}
}
