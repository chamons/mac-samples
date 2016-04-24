using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using CoreGraphics;
using Foundation;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.Linq;

namespace MacPhotoDemo
{
	public partial class EmotionViewController : NSViewController
	{
		EmotionServiceClient client;

		public EmotionViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string api_key = "60914118050a44e9b4f79b67472ada14";
			if (string.IsNullOrEmpty(api_key))
			{
				NSAlert a = new NSAlert();
				a.MessageText = "Add an API key to ViewControl.cs.\n\nRegister at https://www.microsoft.com/cognitive-services";
				a.AlertStyle = NSAlertStyle.Critical;
				a.RunModal();
				NSApplication.SharedApplication.Terminate(this);
			}

			client = new EmotionServiceClient(api_key);

			PickPhotoButton.Activated += SelectPhoto;
		}

		async void SelectPhoto(Object o, EventArgs e)
		{
			NSOpenPanel openDialog = NSOpenPanel.OpenPanel;
			openDialog.CanChooseFiles = true;
			openDialog.CanChooseDirectories = false;
			openDialog.AllowsMultipleSelection = false;

			if (openDialog.RunModal(new string[] { "jpg", "png" }) == 1)
			{
				string fileName = openDialog.Filename;
				NSImage image = new NSImage(fileName);
				ThePhoto.Image = image;
				DetailView.TextStorage.SetString(new NSAttributedString("Processing..."));

				using (FileStream file = new FileStream(fileName, FileMode.Open))
				{
					var emotions = await client.RecognizeAsync(file);

					DetailView.TextStorage.SetString(new NSAttributedString(""));
					foreach (var emotion in emotions)
					{
						if (emotion.Scores.Anger > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Anger: " + emotion.Scores.Anger.ToString("n2") + "\n"));
						if (emotion.Scores.Contempt > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Contempt: " + emotion.Scores.Contempt.ToString("n2") + "\n"));
						if (emotion.Scores.Disgust > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Disgust: " + emotion.Scores.Disgust.ToString("n2") + "\n"));
						if (emotion.Scores.Fear > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Fear: " + emotion.Scores.Fear.ToString("n2") + "\n"));
						if (emotion.Scores.Happiness > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Happiness: " + emotion.Scores.Happiness.ToString("n2") + "\n"));
						if (emotion.Scores.Neutral > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Neutral: " + emotion.Scores.Neutral.ToString("n2") + "\n"));
						if (emotion.Scores.Sadness > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Sadness: " + emotion.Scores.Sadness.ToString("n2") + "\n"));
						if (emotion.Scores.Surprise > 0.1)
							DetailView.TextStorage.Append(new NSAttributedString("Surprise: " + emotion.Scores.Surprise.ToString("n2") + "\n"));
					}
					if (DetailView.TextStorage.Length == 0)
						DetailView.TextStorage.Append(new NSAttributedString("No emotions found"));
				}
			}
		}
	}
}