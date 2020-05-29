using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFSpeechDemo
{
    public partial class MainPage : ContentPage
    {
        private ISpeechToText _speechRecongnitionInstance;
        public MainPage()
        {
            InitializeComponent();
            try
            {
                _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch(Exception ex)
            {
                recon.Text = ex.Message;
            }
            

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                start.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
        }

        private void SpeechToTextFinalResultRecieved(string args)
        {
            recon.Text = args;
        }

        private async void Start_Clicked(object sender, EventArgs e)
        {
            try
            {
                var canProceed =await CheckPermissionAsync();
                
                if(canProceed)
                    _speechRecongnitionInstance.StartSpeechToText();
                else
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                    {
                        await DisplayAlert("Need Mic", "Need MicrophoneAccess", "OK");
                    }

                    var status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                    if (status == PermissionStatus.Granted)
                        Start_Clicked(sender,e);
                }
            }
            catch(Exception ex)
            {
                recon.Text = ex.Message;
            }
            
            if (Device.RuntimePlatform == Device.iOS)
            {
                start.IsEnabled = false;
            }

            

        }

        private async Task<bool> CheckPermissionAsync()
        {
            System.Diagnostics.Debug.WriteLine("here");
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if(PermissionStatus.Granted==permissionStatus)
            {
                return true;
            }
            return false;
        }
    }
}
