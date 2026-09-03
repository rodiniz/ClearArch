public interface ILog
{
	void Verbose(string message, params object?[] args);

	void Debug(string message, params object?[] args);

	void Info(string message, params object?[] args);

	void Warning(string message, params object?[] args);

	void Warning(string message, Exception? e, params object?[] args);

	void Error(string message, Exception e, params object?[] args);

	void Error(string message, params object?[] args);

	void Critical(string message, Exception? e, params object?[] args);
}