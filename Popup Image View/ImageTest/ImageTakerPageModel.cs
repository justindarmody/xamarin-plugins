using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;
using Plugin.Media.Abstractions;

namespace ImageTest
{
	[ImplementPropertyChanged]
	public class ImageTakerPageModel
	{
		private enum PictureMode
		{
			Camera,
			Gallery
		}

		private ICommand selectFromGalleryCommand;
		private ICommand takePhotoCommand;
		private ICommand clearCurrentImageCommand;

		private MediaFile imageFile;

		private IMedia mediaService;

		public bool CanSelectPhoto { get { return this.mediaService.IsPickPhotoSupported && this.imageFile == null; } }

		public bool CanTakePhoto { get { return this.mediaService.IsTakePhotoSupported && this.imageFile == null; } }

		public ICommand PickPhotoCommand
		{
			get { return this.selectFromGalleryCommand ?? (this.selectFromGalleryCommand = new Command(this.HandlePickPhotoCommand)); }
		}

		public ICommand TakePhotoCommand
		{
			get { return this.takePhotoCommand ?? (this.takePhotoCommand = new Command(this.HandleTakePhotoCommand)); }
		}

		public ICommand ClearCurrentImageCommand
		{
			get { return this.clearCurrentImageCommand ?? (this.clearCurrentImageCommand = new Command(this.HandleClearCurrentImageCommand)); }
		}

		public MediaFile ImageFile
		{
			get { return this.imageFile; }
			set
			{
				if (this.imageFile != null)
				{
					this.imageFile.Dispose();
				}

				if (this.ImageSource != null)
				{
					this.ImageSource = null;
				}

				this.imageFile = value;

				if (value != null)
				{
					this.ImageSource = Xamarin.Forms.ImageSource.FromStream(() =>
					{
						var stream = this.imageFile.GetStream();
						this.imageFile.Dispose();
						return stream;
					});
				}
			}
		}

		public ImageSource ImageSource
		{
			get; private set;
		}

		public ImageTakerPageModel()
		{
			this.mediaService = Plugin.Media.CrossMedia.Current;
		}

		private void HandleClearCurrentImageCommand(object obj)
		{
			this.ImageFile = null;
		}

		private async void HandlePickPhotoCommand()
		{
			await this.HandleSelectPhoto(PictureMode.Gallery);
		}

		private async void HandleTakePhotoCommand()
		{
			await this.HandleSelectPhoto(PictureMode.Camera);
		}

		private async Task HandleSelectPhoto(PictureMode mode)
		{
			bool notFound = false;

			MediaFile image = null;

			try
			{
				if (mode == PictureMode.Camera)
				{
					image = await this.GetImageFromCamera();
				}
				else if (mode == PictureMode.Gallery)
				{
					image = await this.GetImageFromGallery();
				}
			}
			catch
			{
				notFound = true;
			}

			if (notFound)
			{
				throw new Exception("Image Not Found");
			}

			this.ImageFile = image;
		}

		private async Task<MediaFile> GetImageFromCamera()
		{
			try
			{
				string fileName = string.Format("{0}.jpg", DateTime.Now.Ticks);

				var media = await this.mediaService.TakePhotoAsync(new StoreCameraMediaOptions
				{
					DefaultCamera = CameraDevice.Rear,
					Name = fileName,
					Directory = "Docs"
				});

				return media;
			}
			catch
			{
				// do nothing
			}

			return null;
		}

		private async Task<MediaFile> GetImageFromGallery()
		{
			try
			{
				var media = await this.mediaService.PickPhotoAsync();

				return media;
			}
			catch
			{
				// do nothing
			}

			return null;
		}
	}
}