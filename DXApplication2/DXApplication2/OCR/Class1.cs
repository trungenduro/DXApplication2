using Android.Gms.Tasks;
using AObject = Java.Lang.Object;

// 名前空間はプロジェクトに合わせてください。
namespace SampleApp.Platforms.Android;

public class OnSuccessListener<T> : AObject, IOnSuccessListener
{
	private Action<T> _onSuccessAction;

	public OnSuccessListener(Action<T> onSuccessAction)
	{
		_onSuccessAction = onSuccessAction;
	}

	public void OnSuccess(AObject? result)
	{
		if (result is T tResult)
		{
			_onSuccessAction(tResult);
		}
	}
}