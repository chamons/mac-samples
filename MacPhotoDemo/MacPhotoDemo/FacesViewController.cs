using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using CoreGraphics;
using Foundation;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace MacPhotoDemo
{
	public class BorderView : NSView
	{
		public BorderView(CGRect rect) : base()
		{
			Frame = rect;
		}

		public override void ViewDidMoveToWindow()
		{
			base.ViewDidMoveToWindow();
			WantsLayer = true;
			Layer.BackgroundColor = NSColor.Clear.CGColor;
			Layer.BorderWidth = 3;
			Layer.BorderColor = NSColor.Red.CGColor;
		}
	}

	public partial class FacesViewController : NSViewController
	{
		FaceServiceClient client;
		List<BorderView> Borders = new List<BorderView> ();

		public FacesViewController (IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string api_key = "";
			if (string.IsNullOrEmpty(api_key))
			{
				NSAlert a = new NSAlert();
				a.MessageText = "Add an API key to ViewControl.cs.\n\nRegister at https://www.microsoft.com/cognitive-services";
				a.AlertStyle = NSAlertStyle.Critical;
				a.RunModal();
				NSApplication.SharedApplication.Terminate(this);
			}

			client = new FaceServiceClient(api_key);

			PickPhotoButton.Activated += SelectPhoto;
		}

		async void SelectPhoto(Object o, EventArgs e)
		{
			NSOpenPanel openDialog = NSOpenPanel.OpenPanel;
			openDialog.CanChooseFiles = true;
			openDialog.CanChooseDirectories = false;
			openDialog.AllowsMultipleSelection = false;

			if (openDialog.RunModal (new string[] { "jpg", "png" }) == 1)
			{
				string fileName = openDialog.Filename;
				NSImage image = new NSImage (fileName);
				ThePhoto.Image = image;

				ClearExistingBorders ();
				DetailView.TextStorage.SetString (new NSAttributedString("Processing..."));

				using (FileStream file = new FileStream (fileName, FileMode.Open))
				{
					var faces = await client.DetectAsync (file, true, true, new List<FaceAttributeType>() { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses });

					DetailView.TextStorage.SetString (new NSAttributedString(""));

					foreach (var face in faces)
					{
						FaceRectangle faceRect = face.FaceRectangle;
						DetailView.TextStorage.Append (FormatRect (faceRect));
						DetailView.TextStorage.Append (FormatDetails (face.FaceAttributes));

						AddFrameAroundFace (faceRect);
					}
				}
			}
		}

		NSAttributedString FormatRect(FaceRectangle rect)
		{
			return new NSAttributedString(string.Format("Position - ({0},{1},{2},{3})\n", rect.Width, rect.Height, rect.Left, rect.Top));
		}

		NSAttributedString FormatDetails(FaceAttributes attr)
		{
			return new NSAttributedString(string.Format("Gender - {0}\nAge - {1}\nSmile - {2}\nGlasses - {3}\n\n", attr.Gender, attr.Age, attr.Smile, attr.Glasses));
		}

		void ClearExistingBorders ()
		{
			foreach (var border in Borders)
				border.RemoveFromSuperview ();
			Borders.Clear ();
		}

		void AddFrameAroundFace(FaceRectangle faceRect)
		{
			NSImage image = ThePhoto.Image;
			// The API returns based on # of pixels, but NSImage scales, so we need to have scaled versions based on size;

			// The actual size of the image
			double imagePixelWidth = (double)image.Representations()[0].PixelsWide;
			double imagePixelHeight = (double)image.Representations()[0].PixelsHigh;

			CGRect photoFrame = ThePhoto.Frame;
			// The % scaling needed in each axis
			double percentageX = photoFrame.Width / imagePixelWidth;
			double percentageY = photoFrame.Height / imagePixelHeight;

			// Scaled position - API gives top left, but Cocoa wants bottom left.
			double faceRectTopConverted = imagePixelHeight - faceRect.Top;
			double picX = (int)Math.Round(faceRect.Left * percentageX);
			double picY = (int)Math.Round(faceRectTopConverted * percentageY);

			// Scaled size
			double picWidth = (photoFrame.Width / imagePixelWidth) * faceRect.Width;
			double picHeight = (photoFrame.Height / imagePixelHeight) * faceRect.Height;

			BorderView borderView = new BorderView (new CGRect (photoFrame.X + picX, photoFrame.Y + picY - picHeight, picWidth, picHeight));
			Borders.Add (borderView);
			View.AddSubview (borderView);
		}
	}
}