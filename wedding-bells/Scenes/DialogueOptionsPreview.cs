using Godot;
using System;

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

		private int _selectedOption = 0;

		// Holds the possible dialogue choices
		private DialogueOption[] _options;
		
		// The method we should call when an option has been selected.
		Action<DialogueOption> OnOptionSelected;
		
		private bool hasSubmittedOptionSelection = false;
		
		public MarkupPalette palette;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public void PrepareLine(DialogueOption[] dialogueOptions, Action<DialogueOption> onOptionSelected)
		{
			//Visible = true;
			_options = dialogueOptions;
			OnOptionSelected = onOptionSelected;
			AreOptionsActive = true;
			hasSubmittedOptionSelection = false;
			SetOptionText(0);
		}

		public void Complete()
		{
			if (Visible)
			{
				OnOptionSelected = null;
				_options = null;
				AreOptionsActive = false;
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
				if (@event.IsActionPressed("AdvanceDialogue"))
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
			var line = _options[selectedOption].Line.Text;
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
			hasSubmittedOptionSelection = true;
			_selectedOption = 0;
			Visible = false;
		}
	}
}
