using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPage : Page
    {
        public CameraPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>    

        private MediaCapture captureManager;
        private bool isPreviewing = false;
        private WriteableBitmap bitmap;

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            CleanCapture();
            Frame.GoBack();
        }

        private void Capture_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (isPreviewing == false)
            {
                InitializePreview();
                takenImage.Source = null;
                takenImage.Visibility = Visibility.Collapsed;
                Capture_Btn.Label = "Camera On";
            }
            else if (isPreviewing == true)
            {
                CleanCapture();
            }
        }
        
        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (isPreviewing == true)
            {
                previewElement.Visibility = Visibility.Collapsed;

                if (GetDisplayAspectRatio() == DisplayAspectRatio.FifteenByNine)
                {
                    CaptureFifteenByNineImage();
                }

                if (GetDisplayAspectRatio() == DisplayAspectRatio.SixteenByNine)
                {
                    CaptureSixteenByNineImage();
                }
            }
        }

        
        #region preview helpers
        /// <summary>
        /// initializes the preview with our desired settings
        /// </summary>
        private async void InitializePreview()
        {
            captureManager = new MediaCapture();

            var cameraID = await GetCameraID(Windows.Devices.Enumeration.Panel.Back);

            await captureManager.InitializeAsync(new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.Photo,
                AudioDeviceId = string.Empty,
                VideoDeviceId = cameraID.Id,
            });

            await captureManager.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, maxResolution());

            captureManager.SetPreviewRotation(VideoRotation.Clockwise90Degrees);

            StartPreview();
        }

        /// <summary>
        /// starts the preview and effectifely shows the finalPhotoAreaBorder for 15:9 devices
        /// </summary>
        private async void StartPreview()
        {


            previewElement.Visibility = Visibility.Visible;
            previewElement.Source = captureManager;
            await captureManager.StartPreviewAsync();

            isPreviewing = true;

            if (GetDisplayAspectRatio() == DisplayAspectRatio.FifteenByNine)
            {
                GetFifteenByNineBounds();
            }

        }


        /// <summary>
        /// stops preview and frees all system resources, and converts the app back to the default values.
        /// </summary>
        private async void CleanCapture()
        {

            if (captureManager != null)
            {
                if (isPreviewing == true)
                {
                    await captureManager.StopPreviewAsync();
                    isPreviewing = false;
                }
                captureManager.Dispose();

                previewElement.Source = null;
                previewElement.Visibility = Visibility.Collapsed;
                takenImage.Source = null;
                takenImage.Visibility = Visibility.Collapsed;
                Capture_Btn.Label = "capture";
            }

        }
        #endregion

        #region camera resoultion helpers

        /// <summary>
        /// get highest possible resolution
        /// </summary>
        /// <returns></returns>
        private VideoEncodingProperties maxResolution()
        {
            VideoEncodingProperties resolutionMax = null;

            //get all photo properties
            var resolutions = captureManager.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo);

            //generate new list to work with
            List<VideoEncodingProperties> vidProps = new List<VideoEncodingProperties>();

            //add only those properties that are 16:9 to our own list
            for (var i = 0; i < resolutions.Count; i++)
            {
                VideoEncodingProperties res = (VideoEncodingProperties)resolutions[i];

                if (MatchScreenFormat(new Size(res.Width, res.Height)) != CameraResolutionFormat.FourByThree)
                {
                    vidProps.Add(res);
                }
            }

            //order the list, and select the highest resolution that fits our limit
            if (vidProps.Count != 0)
            {
                vidProps = vidProps.OrderByDescending(r => r.Width).ToList();

                resolutionMax = vidProps.Where(r => r.Width < 2600).First();
            }

            return resolutionMax;
        }

        /// <summary>
        /// get device panel to interact with the correct camera
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel camera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == camera);

            return deviceID;
        }

        /// <summary>
        /// the camera resolution format (aspect ratio)
        /// </summary>
        public enum CameraResolutionFormat
        {
            Unknown = -1,

            FourByThree = 0,

            SixteenByNine = 1
        }

        /// <summary>
        /// Helper to detect the correct camera resolution
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        private CameraResolutionFormat MatchScreenFormat(Size resolution)
        {
            CameraResolutionFormat result = CameraResolutionFormat.Unknown;

            double relation = Math.Max(resolution.Width, resolution.Height) / Math.Min(resolution.Width, resolution.Height);
            if (Math.Abs(relation - (4.0 / 3.0)) < 0.01)
            {
                result = CameraResolutionFormat.FourByThree;
            }
            else if (Math.Abs(relation - (16.0 / 9.0)) < 0.01)
            {
                result = CameraResolutionFormat.SixteenByNine;
            }

            return result;
        }
        #endregion




        #region capture and save helpers
        /// <summary>
        /// captures and saves the 15:9 image with rotation and cropping applied
        /// </summary>
        private async void CaptureFifteenByNineImage()
        {
            //declare string for filename
            string captureFileName = string.Empty;
            //declare image format
            ImageEncodingProperties format = ImageEncodingProperties.CreateJpeg();

            using (var imageStream = new InMemoryRandomAccessStream())
            {
                //generate stream from MediaCapture
                await captureManager.CapturePhotoToStreamAsync(format, imageStream);

                //create decoder and transform
                BitmapDecoder dec = await BitmapDecoder.CreateAsync(imageStream);
                BitmapTransform transform = new BitmapTransform();

                //roate the image
                transform.Rotation = BitmapRotation.Clockwise90Degrees;
                transform.Bounds = GetFifteenByNineBounds();

                //get the conversion data that we need to save the cropped and rotated image
                BitmapPixelFormat pixelFormat = dec.BitmapPixelFormat;
                BitmapAlphaMode alpha = dec.BitmapAlphaMode;

                //read the PixelData
                PixelDataProvider pixelProvider = await dec.GetPixelDataAsync(
                    pixelFormat,
                    alpha,
                    transform,
                    ExifOrientationMode.RespectExifOrientation,
                    ColorManagementMode.ColorManageToSRgb
                    );
                byte[] pixels = pixelProvider.DetachPixelData();

                //generate the file
                StorageFolder folder = KnownFolders.SavedPictures;
                StorageFile capturefile = await folder.CreateFileAsync("photo_" + DateTime.Now.Ticks.ToString() + ".jpg", CreationCollisionOption.ReplaceExisting);
                captureFileName = capturefile.Name;

                //writing directly into the file stream
                using (IRandomAccessStream convertedImageStream = await capturefile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    //write changes to the BitmapEncoder
                    BitmapEncoder enc = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, convertedImageStream);
                    enc.SetPixelData(
                        pixelFormat,
                        alpha,
                        transform.Bounds.Width,
                        transform.Bounds.Height,
                        dec.DpiX,
                        dec.DpiY,
                        pixels
                        );

                    await enc.FlushAsync();
                }
            }
            CleanCapture();

            //load saved image
            LoadCapturedphoto(captureFileName);
        }

        /// <summary>
        /// captures and saves the 16:9 image with only rotation applied, as no cropping is needed
        /// </summary>
        private async void CaptureSixteenByNineImage()
        {
            //declare string for filename
            string captureFileName = string.Empty;
            //declare image format
            ImageEncodingProperties format = ImageEncodingProperties.CreateJpeg();

            //rotate and save the image
            using (var imageStream = new InMemoryRandomAccessStream())
            {
                //generate stream from MediaCapture
                await captureManager.CapturePhotoToStreamAsync(format, imageStream);

                //create decoder and encoder
                BitmapDecoder dec = await BitmapDecoder.CreateAsync(imageStream);
                BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(imageStream, dec);

                //roate the image
                enc.BitmapTransform.Rotation = BitmapRotation.Clockwise90Degrees;

                //write changes to the image stream
                await enc.FlushAsync();

                //save the image
                StorageFolder folder = KnownFolders.SavedPictures;
                StorageFile capturefile = await folder.CreateFileAsync("photo_" + DateTime.Now.Ticks.ToString() + ".jpg", CreationCollisionOption.ReplaceExisting);
                captureFileName = capturefile.Name;

                //store stream in file
                using (var fileStream = await capturefile.OpenStreamForWriteAsync())
                {
                    try
                    {
                        //because of using statement stream will be closed automatically after copying finished
                        await RandomAccessStream.CopyAsync(imageStream, fileStream.AsOutputStream());
                    }
                    catch
                    {

                    }
                }
            }
            CleanCapture();

            //load saved image
            LoadCapturedphoto(captureFileName);
        }

        /// <summary>
        /// easiest way to capture a photo, but does not respect orientation/flipping etc! (only shown for reference)
        /// </summary>
        private async void CapturePreviewWithoutMondifications()
        {
            //declare image format
            ImageEncodingProperties format = ImageEncodingProperties.CreateJpeg();

            //generate file in local folder:
            StorageFile capturefile = await ApplicationData.Current.LocalFolder.CreateFileAsync("photo_" + DateTime.Now.Ticks.ToString(), CreationCollisionOption.ReplaceExisting);

            ////take & save photo
            await captureManager.CapturePhotoToStorageFileAsync(format, capturefile);

            //show captured photo
            BitmapImage img = new BitmapImage(new Uri(capturefile.Path));
            takenImage.Source = img;
            takenImage.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// asynchronusly opens the saved picture and displays it
        /// </summary>
        /// <param name="filename">the file name string</param>
        private async void LoadCapturedphoto(string filename)
        {
            //load saved image
            StorageFolder pictureLibrary = KnownFolders.SavedPictures;
            StorageFile savedPicture = await pictureLibrary.GetFileAsync(filename);
            ImageProperties imgProp = await savedPicture.Properties.GetImagePropertiesAsync();
            var savedPictureStream = await savedPicture.OpenAsync(FileAccessMode.Read);

            //set image properties and show the taken photo
            bitmap = new WriteableBitmap((int)imgProp.Width, (int)imgProp.Height);
            await bitmap.SetSourceAsync(savedPictureStream);
            takenImage.Source = bitmap;
            takenImage.Visibility = Visibility.Visible;
        }
        #endregion


        #region display helpers
        /// <summary>
        /// the display resolution format
        /// </summary>
        public enum DisplayAspectRatio
        {
            Unknown = -1,

            FifteenByNine = 0,

            SixteenByNine = 1
        }

        /// <summary>
        /// method to detect DisplayResolutionFormat
        /// </summary>
        /// <returns></returns>
        private DisplayAspectRatio GetDisplayAspectRatio()
        {
            DisplayAspectRatio result = DisplayAspectRatio.Unknown;

            //WP8.1 uses logical pixel dimensions, we need to convert this to raw pixel dimensions
            double logicalPixelWidth = Windows.UI.Xaml.Window.Current.Bounds.Width;
            double logicalPixelHeight = Windows.UI.Xaml.Window.Current.Bounds.Height;

            double rawPerViewPixels = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            double rawPixelHeight = logicalPixelHeight * rawPerViewPixels;
            double rawPixelWidth = logicalPixelWidth * rawPerViewPixels;

            //calculate and return screen format
            double relation = Math.Max(rawPixelWidth, rawPixelHeight) / Math.Min(rawPixelWidth, rawPixelHeight);
            if (Math.Abs(relation - (15.0 / 9.0)) < 0.01)
            {
                result = DisplayAspectRatio.FifteenByNine;
            }
            else if (Math.Abs(relation - (16.0 / 9.0)) < 0.01)
            {
                result = DisplayAspectRatio.SixteenByNine;
            }

            return result;
        }

        /// <summary>
        /// Helper to get the correct Bounds for 15:9 screens and to set finalPhotoAreaBorder values
        /// </summary>
        /// <returns></returns>
        private BitmapBounds GetFifteenByNineBounds()
        {
            BitmapBounds bounds = new BitmapBounds();

            //image size is raw pixels, so we need also here raw pixels
            double logicalPixelWidth = Windows.UI.Xaml.Window.Current.Bounds.Width;
            double logicalPixelHeight = Windows.UI.Xaml.Window.Current.Bounds.Height;

            double rawPerViewPixels = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            double rawPixelHeight = logicalPixelHeight * rawPerViewPixels;
            double rawPixelWidth = logicalPixelWidth * rawPerViewPixels;

            //calculate scale factor of UniformToFill Height (remember, we rotated the preview)
            double scaleFactorVisualHeight = maxResolution().Width / rawPixelHeight;

            //calculate the visual Width 
            //(because UniFormToFill scaled the previewElement Width down to match the previewElement Height)
            double visualWidth = maxResolution().Height / scaleFactorVisualHeight;

            //calculate cropping area for 15:9
            uint scaledBoundsWidth = maxResolution().Height;
            uint scaledBoundsHeight = (scaledBoundsWidth / 9) * 15;

            //we are starting at the top of the image
            bounds.Y = 0;
            //cropping the image width
            bounds.X = 0;
            bounds.Height = scaledBoundsHeight;
            bounds.Width = scaledBoundsWidth;

            //set finalPhotoAreaBorder values that shows the user the area that is captured
            finalPhotoAreaBorder.Width = (scaledBoundsWidth / scaleFactorVisualHeight) / rawPerViewPixels;
            finalPhotoAreaBorder.Height = (scaledBoundsHeight / scaleFactorVisualHeight) / rawPerViewPixels;
            finalPhotoAreaBorder.Margin = new Thickness(Math.Floor(((rawPixelWidth - visualWidth) / 2) / rawPerViewPixels), 0, Math.Floor(((rawPixelWidth - visualWidth) / 2) / rawPerViewPixels), 0);
            finalPhotoAreaBorder.Visibility = Visibility.Visible;

            return bounds;
        }
        #endregion

        private void Save_Pic_Btn_Click(object sender, RoutedEventArgs e)
        {
            CleanCapture();
            Frame.Navigate(typeof(InsertClothes));
        }

        

       

        
    }
}
