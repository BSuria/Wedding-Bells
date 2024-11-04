using Godot;
using System;
using System.Timers;
using YarnSpinnerGodot;

public partial class ExpandBubble : RichTextLabel
{
	[Export] private float maxWidth;
	[Export] private float maxHeight;
	[Export] private float minWidth;
	[Export] private float minHeight;
	
	[Export] private NodePath LineViewPath;
	[Export] private NodePath OptionViewPath;
	private LineView _lineView;
	private OptionsListView _optionView;
	
	private RichTextLabel label;

	private int charSize = 0;
	private int currentChar = 1;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (LineViewPath != null)
		{
			_lineView = GetNode<LineView>(LineViewPath);
			_lineView.onCharacterTyped += OnCharacterTyped;
			_lineView.onLineStart += OnLineStart;
			_lineView.onLineInterrupted += OnLineInterrupted;
		}
		//SetSize(new Vector2(0, 0));
		//SetFitContent(true);
		//Finished += expandBox;
		if (OptionViewPath != null)
		{
			_optionView = GetNode<OptionsListView>(OptionViewPath);
			_optionView.onOptionSelectBegin += OnOptionSelectBegin;
		}
	}

	void OnCharacterTyped()
	{
		Vector2 StringSize = GetThemeFont("normal_font")
			.GetStringSize(Text.Left(currentChar), HorizontalAlignment.Left, -1, GetThemeFontSize("normal_font_size"));
		GD.Print(StringSize);
		
		if (StringSize.X > 0)
		{
			//harSize = Text.
			var lineCount = maxWidth / StringSize.X;
			currentChar += 1;
		}
		GD.Print("First Clac: Size X = " + Size.X);
		GD.Print("Second Clac: maxWidth = " + maxWidth);
		GD.Print("Third Clac: CustomMinimumSize = " + GetCustomMinimumSize());
		GD.Print(Size.X <= maxWidth && Size.X > minWidth);
		
		if (Size.X < maxWidth && StringSize.X >= GetCustomMinimumSize().X)
		{
			SetPosition(new Vector2(Position.X - (Mathf.Clamp(StringSize.X + 40, 0, maxWidth) - Size.X)/2, Position.Y));
			SetSize(new Vector2(Mathf.Clamp(StringSize.X + 40, 0, maxWidth), Size.Y));
			GD.Print("BubbleSize.X is: " + Size.X);
			GD.Print("StringSizeTyped is: " + StringSize);
		}
		if (Size.X >= maxWidth)
		{
			SetAutowrapMode(TextServer.AutowrapMode.WordSmart);

			if (GetCharacterLine(currentChar) > 0)
			{
				var YSize = (GetCharacterLine(currentChar)) * 50;
				if (YSize + 60 != Size.Y)
				{
					SetPosition(new Vector2(Position.X, Position.Y - 50));
				}
				SetSize(new Vector2(Size.X, YSize + 60));
				GD.Print(Math.Floor(StringSize.X / maxWidth + 0.5));
				GD.Print("VisibleLineCount " + GetCharacterLine(currentChar));
			}

			//SetFitContent(true);
			//Before this, I must resize it so size is consistent in case it overshot
		}
	}
	
	void OnLineStart ()
	{
		SetPosition(Vector2.Zero);
		SetAutowrapMode(TextServer.AutowrapMode.Off);
		SetFitContent(false);
		SetSize(new Vector2(GetCustomMinimumSize().X, GetCustomMinimumSize().Y));
		currentChar = 1;
	}
	
	void OnLineInterrupted()
	{
		if (Text.Length == 0)
		{
			return;
		}
		GD.Print("LINE INTERRUPTED");
		Vector2 StringSize = GetThemeFont("normal_font")
			.GetStringSize(Text, HorizontalAlignment.Left, -1, GetThemeFontSize("normal_font_size"));
		GD.Print("Text is: " + Text);
		GD.Print("StringSize is: " + StringSize);
		if (StringSize.X > GetCustomMinimumSize().X)
		{
			SetPosition(new Vector2(Position.X - (Mathf.Clamp(StringSize.X + 40, 0, maxWidth) - Size.X) / 2, Position.Y));
			SetSize(new Vector2(Mathf.Clamp(StringSize.X + 40, 0, maxWidth), 0));
			GD.Print("BubbleSize.X is: " + Size.X);
		}
		if (Size.X >= maxWidth)
		{
			SetAutowrapMode(TextServer.AutowrapMode.WordSmart);
		}
		var lastYSize = ((GetCharacterLine(currentChar)) * 50);
		var YSize = ((GetCharacterLine(Text.Length - 1)) * 50);
		GD.Print("CharacterLine is: " + GetCharacterLine(Text.Length) + 1);
				SetPosition(new Vector2(Position.X, Position.Y - YSize + lastYSize));	
				SetSize(new Vector2(Size.X, YSize + 60));
	}
	
	//Now editing the bubble size for the option select lineview
	void OnOptionSelectBegin()
	{
		if (Text.Length == 0)
		{
			return;
		}
		GD.Print("LINE INTERRUPTED");
		Vector2 StringSize = GetThemeFont("normal_font")
			.GetStringSize(Text, HorizontalAlignment.Left, -1, GetThemeFontSize("normal_font_size"));
		GD.Print("Text is: " + Text);
		GD.Print("Options: StringSize is: " + StringSize);
		if (StringSize.X > GetCustomMinimumSize().X)
		{
			SetPosition(new Vector2(Position.X - (Mathf.Clamp(StringSize.X + 40, 0, maxWidth) - Size.X) / 2, Position.Y));
			SetSize(new Vector2(Mathf.Clamp(StringSize.X + 40, 0, maxWidth), 0));
		}
		if (Size.X >= maxWidth)
		{
			SetAutowrapMode(TextServer.AutowrapMode.WordSmart);
		}
		var YSize = ((GetCharacterLine(Text.Length - 1)) * 50);
		GD.Print("CharacterLine is: " + GetCharacterLine(Text.Length) + 1);
		SetPosition(new Vector2(Position.X, Position.Y - YSize));	
		SetSize(new Vector2(Size.X, YSize + 60));
	}
	
	/*
	public void expandBox()
	{
		var autoWrap = AutowrapMode;
		SetBlockSignals(true);
		
		SetAutowrapMode(TextServer.AutowrapMode.Off);
		
		Size = new Vector2(maxWidth, maxHeight);

		var width = Mathf.Clamp(Size.X, 0, maxWidth);
		var height = Size.Y;
		
		SetAutowrapMode(autoWrap);
		SetSize(new Vector2(width, Size.Y));

		if (Size.Y > height)
		{
			height = Size.Y;
			
			while(true)
			{
				SetSize(new Vector2(Size.X - 10, Size.Y));
				
				if (!(Size.Y > height - 0.00001 && Size.Y < height+ 0.00001))
				{
					SetSize(new Vector2(Size.X + 10, Size.Y));
					break;
				}
			}
		}
		SetSize(new Vector2(Size.X, height));
		
		SetBlockSignals(false);
	}
	*/
}
