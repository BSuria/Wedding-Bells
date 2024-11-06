using Godot;
using System;

public partial class OptionsPip : TextureRect
{
	[Export] private Texture2D _filledTexture;

	private Texture2D _originalTexture;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_originalTexture = GetTexture();
	}
	
	public void OptionSelected(bool status = false)
	{
		if (status == true)
		{
			Texture = _filledTexture;
		}

		if (status == false)
		{
			Texture = _originalTexture;
		}
	}
}
