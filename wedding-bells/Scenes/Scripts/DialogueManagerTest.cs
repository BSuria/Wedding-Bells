using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YarnSpinnerGodot;

// Currently planning to try to have the position of the box change position based
// on which character is speaking, without having the writer make commands for
// it to switch place.
// I'm likely going to need to make my own lineview or copy and edit one.
// Look at DialogueRunner's OnComplete() or OnDialogueStart() Methods to find out how
// to send out signals when a line begins.
public partial class DialogueManagerTest : Node
{
	[Export] private NodePath _lineTextPath;
	[Export] private NodePath _dialogueRunnerPath;
	[Export] private NodePath _lineViewPath;
	[Export] private NodePath _lineBubblePath;
	[Export] private NodePath _optionViewPath;
	[Export] private NodePath _optionLastLinePath;
	
	[Export] private NodePath _actorPosOnePath;
	[Export] private NodePath _actorPosTwoPath;
	[Export] private ActorsInSceneData _actorsInSceneData;

	private RichTextLabel _lineText;
	private DialogueRunner _dialogueRunner;
	private LineView _lineView;
	private Panel _lineBubble;
	private OptionsListView _optionView;
	private RichTextLabel _optionsLastLine;

	private Sprite2D _actorSpriteOne;
	private Sprite2D _actorSpriteTwo;
	private Node2D _actorPosOne;
	private Node2D _actorPosTwo;
	private Godot.Collections.Dictionary<string, ActorData> Actors = new Godot.Collections.Dictionary<string, ActorData>();
	
	
	public override void _Ready()
	{
		Actors = _actorsInSceneData.Actors;

		_lineText = GetNode<RichTextLabel>(_lineTextPath);
		_dialogueRunner = GetNode<DialogueRunner>(_dialogueRunnerPath);
		_lineView = GetNode<LineView>(_lineViewPath);
		_lineBubble = GetNode<Panel>(_lineBubblePath);
		_optionView = GetNode<OptionsListView>(_optionViewPath);
		_optionsLastLine = GetNode<RichTextLabel>(_optionLastLinePath);

		_actorPosOne = GetNode<Node2D>(_actorPosOnePath);
		_actorPosTwo = GetNode<Node2D>(_actorPosTwoPath);
		_actorSpriteOne = GetNode<Sprite2D>(_actorPosOnePath);
		_actorSpriteTwo = GetNode<Sprite2D>(_actorPosTwoPath);
		
		_lineView.onLineStart += OnLineStart;
		_optionView.onOptionSelectBegin += OnOptionsSelectBegins;
		_dialogueRunner.AddCommandHandler<string, string>("actorPos", setPos);
		_dialogueRunner.AddCommandHandler<string, string>("setSprite", setSprite);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void OnLineStart()
	{
		GD.Print(_lineView.SpeakingCharacter);
		GD.Print("new line started");
		positionBox(_actorsPos[_lineView.SpeakingCharacter], _lineText);
	}
	
	public void BeginDialogue()
	{
		_dialogueRunner.StartDialogue("Introductions");
	}
	
	
	
	private Dictionary<string, string> _actorsPos = new Dictionary<string, string>();
	
	public void setPos(String actorName, String actorPos){
		_actorsPos[actorName] = actorPos;
		setSprite(actorName, "Neutral");
	}

	private void positionBox(string actorPos, RichTextLabel label)
	{
		//GD.Print(_lineText.GlobalPosition.ToString());
		//GD.Print(_lineText.Position.ToString());
		if (actorPos == "1")
		{
			// IDK why but setting the position twice makes this account for the box increasing in size from more lines of text
			// If I just set it once, it uses the dialogue bubble size from the previous line of dialogue.
			label.GlobalPosition = _actorPosOne.GlobalPosition - new Vector2(0, _actorSpriteOne.Texture.GetSize().Y/2) - new Vector2(label.GetSize().X/2, label.GetSize().Y + 50) - GetViewport().GetVisibleRect().Position;
			label.GlobalPosition = _actorPosOne.GlobalPosition - new Vector2(0, _actorSpriteOne.Texture.GetSize().Y/2) - new Vector2(label.GetSize().X/2, label.GetSize().Y + 50) - GetViewport().GetVisibleRect().Position;
			GD.Print(_actorPosOne.GlobalPosition - new Vector2(0, _lineBubble.Size.Y + _actorSpriteOne.Texture.GetSize().Y/2));
			GD.Print(_lineText.GlobalPosition);
		}
		if (actorPos == "2")
		{
			GD.Print("ACTORPOS: " + _actorPosOne.GlobalPosition);
			GD.Print("lINETEXT SIZE: " + _lineText.GetSize().X);
			
			label.Position = _actorPosTwo.GlobalPosition - new Vector2(0, _actorSpriteTwo.Texture.GetSize().Y/2) - new Vector2(label.GetSize().X/2, label.GetSize().Y + 50) - GetViewport().GetVisibleRect().Position;
			label.Position = _actorPosTwo.GlobalPosition - new Vector2(0, _actorSpriteTwo.Texture.GetSize().Y/2) - new Vector2(label.GetSize().X/2, label.GetSize().Y + 50) - GetViewport().GetVisibleRect().Position;
			GD.Print(_actorPosTwo.GlobalPosition - new Vector2(0, _lineBubble.Size.Y + _actorSpriteTwo.Texture.GetSize().Y/2));
			GD.Print(_lineText.GlobalPosition);
		}
		GD.Print(_actorSpriteOne.Texture.GetSize().ToString());
		GD.Print(_actorSpriteTwo.Texture.GetSize().ToString());
		//GD.Print(_lineText.GlobalPosition.ToString());
		//GD.Print(_lineText.Position.ToString());
	}

	private void setSprite(string actorName, string spriteName)
	{
		if (_actorsPos[actorName] == "1")
		{
			_actorSpriteOne.Texture = Actors[actorName].ActorSprites[spriteName];
		}
		if (_actorsPos[actorName] == "2")
		{
			_actorSpriteTwo.Texture = Actors[actorName].ActorSprites[spriteName];
		}
	}

	void OnOptionsSelectBegins()
	{
		//lineview.speakingcharacter does not work here as lineview returns an empty string because it is currently not running text
		
		positionBox(_actorsPos[_optionView.SpeakingCharacter], _optionsLastLine);
	}

}
