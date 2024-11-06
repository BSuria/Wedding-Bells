using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YarnSpinnerGodot
{
	public partial class DialogueOptionsPreview : RichTextLabel
	{
		private enum direction
		{
			RIGHT,
			LEFT
		}

		/// <summary>
		/// Gets a value that indicates if options are currently selectable
		/// running.
		/// </summary>
		public bool AreOptionsActive { get; set; }
		public bool AreOptionsAdvancable { get; set; }

		private int _selectedOption = 0;

		// Holds the possible dialogue choices
		private DialogueOption[] _options;
		
		//Images for the arrows that show available dialogue
		[Export] private TextureRect _arrowLeft;
		[Export] private TextureRect _arrowRight;
		
		// The method we should call when an option has been selected.
		Action<DialogueOption> OnOptionSelected;
		
		private bool hasSubmittedOptionSelection = false;
		
		public MarkupPalette palette;
		
		[Export] PackedScene optionsPipPrefab;
		[Export] BoxContainer _pipBoxContainer;
		private List<OptionsPip> optionsPips = new List<OptionsPip>();

		private Timer _actionTimer = new Timer();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			AreOptionsAdvancable = false;
			AreOptionsActive = false;
			// timer to add a delay between transitioning from dialogue to choices
			// so players don't accidentally pick something when skipping through
			// dialogue
			AddChild(_actionTimer);
			_actionTimer.Autostart = false;
			_actionTimer.Timeout += OnTimerTimeout;
			_actionTimer.OneShot = true;
		}

		public void PrepareLine(DialogueOption[] dialogueOptions, Action<DialogueOption> onOptionSelected)
		{
			//Visible = true;
			_options = dialogueOptions;
			OnOptionSelected = onOptionSelected;
			hasSubmittedOptionSelection = false;
			AreOptionsActive = true;
			foreach (var optionsPip in optionsPips)
			{
				optionsPip.Visible = false;
			}
			
			while (dialogueOptions.Length > optionsPips.Count)
			{
				var optionsPip = CreateNewOptionPip();
				optionsPip.Visible = false;
			}
			int optionViewsCreated = 0;

			for (int i = 0; i < _options.Length; i++)
			{
				var optionsPip = optionsPips[i];
				var option = _options[i];

				if (option.IsAvailable == false)
				{
					// Don't show this option.
					continue;
				}

				optionsPip.Visible = true;

				// The first available option is selected by default
				//if (optionViewsCreated == 0)
				//{
				//	optionView.GrabFocus();
				//}

				optionViewsCreated += 1;
			}
			SetOptionText(0);
			_actionTimer.Start(0.6);
		}

		private void OnTimerTimeout()
		{
			GD.Print("Timed out");
			AreOptionsAdvancable = true;
		}
		
		public void Complete()
		{
			if (Visible)
			{
				OnOptionSelected = null;
				_options = null;
				AreOptionsActive = false;
				AreOptionsAdvancable = false;
				_selectedOption = 0;
				Visible = false;
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (AreOptionsActive)
			{
				GD.Print("Options ARE ACTIVE");
				if (@event.IsActionPressed("ShiftPreviewLeft"))
				{
					ShiftPreview(direction.LEFT);
					GD.Print("Pressed left button");
				}

				if (@event.IsActionPressed("ShiftPreviewRight"))
				{
					ShiftPreview(direction.RIGHT);
				}
				if (@event.IsActionPressed("AdvanceDialogue") && AreOptionsAdvancable)
				{
					InvokeOptionSelected();
				}
			}
		}

		void ShiftPreview(Enum Direction)
		{
			switch (Direction)
			{
				case direction.LEFT:
					SetOptionText(-1);
					break;
				case direction.RIGHT:
					SetOptionText(1);
					break;
			}
		}

		void SetOptionText(int direction)
		{
			/*
			// Following code is for if I want the options to loop instead of being clamped
			int selectedOption = (_selectedOption + direction);
			GD.Print("SelectedOption was: " + (selectedOption));
			if (selectedOption > _options.Length - 1)
			{
				selectedOption = 0;
			} else if (selectedOption < 0)
			{
				selectedOption = _options.Length - 1;
			}
			_selectedOption = selectedOption;
			GD.Print("SelectedOption was: " + (_options.Length - 1));
			*/
			_selectedOption = Math.Clamp(_selectedOption + direction, 0, _options.Length - 1);
			var line = _options[_selectedOption].Line.Text;
			_arrowLeft.Visible = true;
			_arrowRight.Visible = true;
			if (_selectedOption == 0)
			{
				_arrowLeft.Visible = false;
			}

			if (_selectedOption == _options.Length - 1)
			{
				_arrowRight.Visible = false;
			}

			for (int i = 0; i < _options.Length; i++)
			{
				optionsPips[i].OptionSelected(_selectedOption == i);
			}
			if (IsInstanceValid(palette))
			{
				Text =
					$"[center]{LineView.PaletteMarkedUpText(line, palette)}[/center]";
			}
			else
			{
				Text = $"[center]{line.Text}[/center]";
			}
		}
		
		public void InvokeOptionSelected()
		{
			// We only want to invoke this once, because it's an error to
			// submit an option when the Dialogue Runner isn't expecting it. To
			// prevent this, we'll only invoke this if the flag hasn't been cleared already.
			if (hasSubmittedOptionSelection)
			{
				return;
			}

			OnOptionSelected.Invoke(_options[_selectedOption]);
			AreOptionsActive = false;
			AreOptionsAdvancable = false;
			hasSubmittedOptionSelection = true;
			_selectedOption = 0;
			Visible = false;
		}
		
		OptionsPip CreateNewOptionPip()
		{
			var optionPip = optionsPipPrefab.Instantiate<OptionsPip>();
			_pipBoxContainer.AddChild(optionPip);
			
			optionsPips.Add(optionPip);

			return optionPip;
		}
	}
}
