using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace DXApplication2;

public interface ISpeechToText
{
	Task<bool> RequestPermissions();

	Task<string> Listen(CultureInfo culture,
		IProgress<string> recognitionResult,
		CancellationToken cancellationToken);
}