using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class TestControl : Node
{
	[Export] private NodePath _dialogueManagerPath;
	
	private DialogueManagerTest _dialogueManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_dialogueManager = GetNode<DialogueManagerTest>(_dialogueManagerPath);
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("Press"))
		{
			_dialogueManager.BeginDialogue("Introductions");
			GD.Print("clicked");
		}
	}
}
