namespace Langutils.Core.Results;

public static partial class Result
{
    /// <summary>
    /// Executes the given function in a try/catch block and returns the result.
    /// </summary>
    /// <param name="func">Function to run</param>
    /// <typeparam name="TValue">Type of value the function returns</typeparam>
    /// <returns>
    /// A Success result with the value returned by the function if the function succeeds.
    /// An Error result with the exception thrown by the function if the function fails.
    /// </returns>
    public static Result<TValue, Exception> Try<TValue>(Func<TValue> func)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Asynchronously executes the given function in a try/catch block and returns the result.
    /// </summary>
    /// <param name="func">Asynchronous function to run</param>
    /// <typeparam name="TValue">Type of value the function returns</typeparam>
    /// <returns>
    /// A Success result with the value returned by the function if the function succeeds.
    /// An Error result with the exception thrown by the function if the function fails.
    /// </returns>
    public static async Task<Result<TValue, Exception>> TryAsync<TValue>(Func<Task<TValue>> func)
    {
        try
        {
            return await func().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Executes the given function in a try/catch block and returns the result.
    /// </summary>
    /// <param name="func">Function to run</param>
    /// <typeparam name="TValue">Type of value the function returns</typeparam>
    /// <typeparam name="TException">Type constraints of the exception</typeparam>
    /// <returns>
    /// A Success result with the value returned by the function if the function succeeds.
    /// An Error result with the exception thrown by the function if the function fails and the exception match the type constraints.
    /// </returns>
    /// <exception>Propagates exception if the type does not match the constraints</exception>
    public static Result<TValue, TException> Try<TValue, TException>(Func<TValue> func)
        where TException: Exception
    {
        try
        {
            return func();
        }
        catch (Exception e) when (e.GetType().IsAssignableTo(typeof(TException)))
        {
            return (TException)e;
        }
    }

    /// <summary>
    /// Asynchronously executes the given function in a try/catch block and returns the result.
    /// </summary>
    /// <param name="func">Asynchronous function to run</param>
    /// <typeparam name="TValue">Type of value the function returns</typeparam>
    /// <typeparam name="TException">Type constraints of the exception</typeparam>
    /// <returns>
    /// A Success result with the value returned by the function if the function succeeds.
    /// An Error result with the exception thrown by the function if the function fails and the exception match the type constraints.
    /// </returns>
    /// <exception>Propagates exception if the type does not match the constraints</exception>
    public static async Task<Result<TValue, TException>> TryAsync<TValue, TException>(Func<Task<TValue>> func)
        where TException: Exception
    {
        try
        {
            return await func().ConfigureAwait(false);
        }
        catch (Exception e) when (e.GetType().IsAssignableTo(typeof(TException)))
        {
            return (TException)e;
        }
    }

    /// <summary>
    /// Executes the given action in a try/catch block and returns the result.
    /// </summary>
    /// <param name="action">Action to run</param>
    /// <returns>
    /// A Success result with the Unit value if the action succeeds.
    /// An Error result with the exception thrown by the action if the action fails.
    /// </returns>
    public static Result<Unit, Exception> Try(Action action)
    {
        try
        {
            action();
            return Unit.Instance;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Asynchronously executes the given action in a try/catch block and returns the result.
    /// </summary>
    /// <param name="action">Asynchronous action to run</param>
    /// <returns>
    /// A Success result with the Unit value if the action succeeds.
    /// An Error result with the exception thrown by the action if the action fails.
    /// </returns>
    public static async Task<Result<Unit, Exception>> TryAsync(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
            return Unit.Instance;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    /// <summary>
    /// Executes the given action in a try/catch block and returns the result.
    /// </summary>
    /// <param name="action">Action to run</param>
    /// <typeparam name="TException">Type constraints of the exception</typeparam>
    /// <returns>
    /// A Success result with the Unit value if the action succeeds.
    /// An Error result with the exception thrown by the action if the action fails and the exception match the type constraints.
    /// </returns>
    /// <exception>Propagates exception if the type does not match the constraints</exception>
    public static Result<Unit, TException> Try<TException>(Action action)
        where TException: Exception
    {
        try
        {
            action();
            return Unit.Instance;
        }
        catch (Exception e) when (e.GetType().IsAssignableTo(typeof(TException)))
        {
            return (TException)e;
        }
    }

    /// <summary>
    /// Asynchronously executes the given action in a try/catch block and returns the result.
    /// </summary>
    /// <param name="action">Asynchronous action to run</param>
    /// <typeparam name="TException">Type constraints of the exception</typeparam>
    /// <returns>
    /// A Success result with the Unit value if the action succeeds.
    /// An Error result with the exception thrown by the action if the action fails and the exception match the type constraints.
    /// </returns>
    /// <exception>Propagates exception if the type does not match the constraints</exception>
    public static async Task<Result<Unit, TException>> TryAsync<TException>(Func<Task> action)
        where TException: Exception
    {
        try
        {
            await action().ConfigureAwait(false);
            return Unit.Instance;
        }
        catch (Exception e) when (e.GetType().IsAssignableTo(typeof(TException)))
        {
            return (TException)e;
        }
    }
}