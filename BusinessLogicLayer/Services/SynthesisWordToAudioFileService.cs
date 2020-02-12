using BusinessLogicLayer.Helpers;
using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;

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
            var config = SpeechConfig.FromSubscription("809556967f64412fa2e213b8b10e88d3", "westeurope");

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
    }
}
