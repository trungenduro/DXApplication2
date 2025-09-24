using Android.Content;
using Android.Speech;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LanguageDetailsReceiver : BroadcastReceiver
{
    private TaskCompletionSource<IList<string>> _tcs;

    public LanguageDetailsReceiver(TaskCompletionSource<IList<string>> tcs)
    {
        _tcs = tcs;
    }

    public override void OnReceive(Context context, Intent intent)
    {
        var results = intent.GetStringArrayListExtra(RecognizerIntent.ExtraSupportedLanguages);
        _tcs.TrySetResult(results);
    }
}


