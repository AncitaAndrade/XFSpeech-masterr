using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XFSpeechDemo.Droid;
[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation))]
namespace XFSpeechDemo.Droid
{
    
    public class SpeechToTextImplementation : ISpeechToText
    {
        private readonly int VOICE = 10;
        private Activity _activity;
        SpeechRecognizer Recognizer { get; set; }
        Intent SpeechIntent { get; set; }
        public SpeechToTextImplementation()
        {
            _activity = CrossCurrentActivity.Current.Activity;

        }



        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            //        string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            //        if (rec == "android.hardware.microphone")
            //        {
            //try
            //{
            //	var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);


            //            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            //            _activity.StartActivityForResult(voiceIntent, VOICE);
            //}
            //catch(ActivityNotFoundException ex)
            //            {
            //                String appPackageName = "com.google.android.googlequicksearchbox";
            //                try
            //                {
            //                    Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("market://details?id=" + appPackageName));
            //                    _activity.StartActivityForResult(intent, VOICE);

            //                }
            //                catch (ActivityNotFoundException e)
            //                {
            //                    Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName));
            //                    _activity.StartActivityForResult(intent, VOICE);
            //                }
            //            }

            //        }
            //        else
            //        {
            //            throw new Exception("No mic found");
            //        }


            var recListener = new RecognitionListener();
            recListener.BeginSpeech += RecListener_BeginSpeech;
            recListener.EndSpeech += RecListener_EndSpeech;
            recListener.Error += RecListener_Error;
            recListener.Ready += RecListener_Ready;
            recListener.Recognized += RecListener_Recognized;

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(_activity.BaseContext);
            Recognizer.SetRecognitionListener(recListener);

            SpeechIntent = new Intent(RecognizerIntent.ActionVoiceSearchHandsFree);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage,_activity.PackageName);
            Recognizer.StartListening(SpeechIntent);

        }


        private void RecListener_Ready(object sender, Bundle e) => Log.Debug(nameof(MainActivity), nameof(RecListener_Ready));

        private void RecListener_BeginSpeech() => Log.Debug(nameof(MainActivity), nameof(RecListener_BeginSpeech));

        private void RecListener_EndSpeech() => Log.Debug(nameof(MainActivity), nameof(RecListener_EndSpeech));

        private void RecListener_Error(object sender, SpeechRecognizerError e) => Log.Debug(nameof(MainActivity), $"{nameof(RecListener_Error)}={e.ToString()}");

        private void RecListener_Recognized(object sender, string recognized) => MessagingCenter.Send<ISpeechToText, string>(this, "STT", recognized);

        public void StopSpeechToText()
        {

        }
    }
}