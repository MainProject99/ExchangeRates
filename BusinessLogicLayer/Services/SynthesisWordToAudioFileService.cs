using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace BusinessLogicLayer.Services
{
    public static class SynthesisWordToAudioFileService
    {
        /// <summary>
        ///This method convert words, to audio file.
        /// </summary>
        /// <param name="text">
        /// Should not contain  null or empty data because we won't have to convert.
        public static async Task SynthesisToAudioFileAsync(string text)
        {
            var config = SpeechConfig.FromSubscription("", "westeurope");

            var fileName = "InnerVoice.wav";
            using (var fileOutput = AudioConfig.FromWavFileOutput(fileName))
            {
                using (var synthesizer = new SpeechSynthesizer(config, fileOutput))
                {
                  
                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized to [{fileName}] for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        throw new AppException($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            throw new AppException($"CANCELED: ErrorCode {cancellation.ErrorCode} /n CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        }
                    }
                }
                }
            }

        /// <summary>
        ///This method convert audio file to words.
        /// </summary>    
        /// <returns>string</returns>
        public static async Task<string> RecognizeSpeechFromFileAsync()
             {
                var config = SpeechConfig.FromSubscription("", "westeurope");

                using (var audioInput = AudioConfig.FromWavFileInput("InnerVoice.wav"))
                    using (var recognizer = new SpeechRecognizer(config, audioInput))
                    {
                        Console.WriteLine("Recognizing first result...");
                        var result = await recognizer.RecognizeOnceAsync();

                        switch (result.Reason)
                        {
                            case ResultReason.RecognizedSpeech:
                                return result.Text;
                                break;  
                            case ResultReason.NoMatch:
                                 return $"NOMATCH: Speech could not be recognized {result.Text}.";
                                 break; 
                            case ResultReason.Canceled:
                                  var cancellation = CancellationDetails.FromResult(result);
                                  return $"CANCELED: Reason={cancellation.Reason} with {result.Text}";
                                  break;
                        }
                    }
            return "Just do It again!";       
             }
            /// <summary>
            ///This method convert current speech from microphone to words.
            /// </summary>    
            /// <returns>string</returns>
            public static  async Task<string> RecognizeSpeechAsync()
            {
                var config = SpeechConfig.FromSubscription("", "westeurope");

                using (var recognizer = new SpeechRecognizer(config))
                {
                    var result = await recognizer.RecognizeOnceAsync();

                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        return result.Text;
                    }
                    else if (result.Reason == ResultReason.NoMatch)
                    {
                        return $"NOMATCH: Speech could not be recognized.";
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(result);
                        throw new AppException($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            throw new AppException($"CANCELED: ErrorCode {cancellation.ErrorCode} /n CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        }
                    }
                }
                return $"Let's do it again";

        }
    }
}
