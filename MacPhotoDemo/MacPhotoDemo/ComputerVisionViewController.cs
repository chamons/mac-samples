using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using CoreGraphics;
using Foundation;
//using Microsoft.ProjectOxford.Face;
//using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace MacPhotoDemo
{
	public partial class ComputerVisionViewController : NSViewController
	{
		VisionServiceClient client;

		public ComputerVisionViewController (IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string api_key = "0dab8c7d8c4949b8ba48bfeeb2f511c6";
			if (string.IsNullOrEmpty(api_key))
			{
				NSAlert a = new NSAlert();
				a.MessageText = "Add an API key to ViewControl.cs.\n\nRegister at https://www.microsoft.com/cognitive-services";
				a.AlertStyle = NSAlertStyle.Critical;
				a.RunModal();
				NSApplication.SharedApplication.Terminate(this);
			}

			client = new VisionServiceClient(api_key);

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

					VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
					var analysis = await client.AnalyzeImageAsync(file, visualFeatures);

					DetailView.TextStorage.SetString(new NSAttributedString(""));

					DetailView.TextStorage.Append(new NSAttributedString("Descriptions: \n"));
					foreach (var caption in analysis.Description.Captions)
						DetailView.TextStorage.Append(new NSAttributedString(caption.Text + ", Confidence: " + caption.Confidence.ToString("n2") + "\n"));

					DetailView.TextStorage.Append(new NSAttributedString("Description Tags: " + string.Join(", ", analysis.Description.Tags)));

					DetailView.TextStorage.Append(new NSAttributedString("\n\nTags: \n"));
					foreach (var tag in analysis.Tags)
					{
						DetailView.TextStorage.Append(new NSAttributedString("Tag Name: " + tag.Name + ", Confidence: " + tag.Confidence.ToString("n2") + ", Hint: " + tag.Hint + "\n"));
					}
				}
			}
		}
	}
}