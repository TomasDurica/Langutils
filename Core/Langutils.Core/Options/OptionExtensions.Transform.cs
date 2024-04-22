using Langutils.Core.Results;

namespace Langutils.Core.Options;

public static partial class OptionExtensions
{
    /// <summary>
    /// Filters the Option based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The Option to filter.</param>
    /// <param name="predicate">The predicate to use for filtering.</param>
    /// <returns>An Option that contains the value if it matches the predicate, otherwise None.</returns>
    public static Option<TValue> Filter<TValue>(this Option<TValue> self, Func<TValue, bool> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } when predicate(value) => self,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously filters the Option based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The Option to filter.</param>
    /// <param name="predicate">The predicate to use for filtering.</param>
    /// <returns>A Task of an Option that contains the value if it matches the predicate, otherwise None.</returns>
    public static async Task<Option<TValue>> Filter<TValue>(this Task<Option<TValue>> self, Func<TValue, bool> predicate)
        => (await self.ConfigureAwait(false)).Filter(predicate);

    /// <summary>
    /// Asynchronously filters the Option based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The Option to filter.</param>
    /// <param name="predicate">The predicate to use for filtering.</param>
    /// <returns>A Task of an Option that contains the value if it matches the predicate, otherwise None.</returns>
    public static async Task<Option<TValue>> FilterAsync<TValue>(this Option<TValue> self, Func<TValue, Task<bool>> predicate)
        => self switch
        {
            { IsSome: true, Value: var value } when await predicate(value).ConfigureAwait(false) => self,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously filters the Option based on a predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The Option to filter.</param>
    /// <param name="predicate">The predicate to use for filtering.</param>
    /// <returns>A Task of an Option that contains the value if it matches the predicate, otherwise None.</returns>
    public static async Task<Option<TValue>> FilterAsync<TValue>(this Task<Option<TValue>> self, Func<TValue, Task<bool>> predicate)
        => await (await self.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in the Option using a selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>An Option that contains the transformed value if the original Option is Some, otherwise None.</returns>
    public static Option<TOut> Map<TIn, TOut>(this Option<TIn> self, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> Map<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).Map(selector);

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Option<TIn> self, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise None.</returns>
    public static async Task<Option<TOut>> MapAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapAsync(selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in the Option using a selector function or returns a default value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>An Option that contains the transformed value if the original Option is Some, otherwise the default value.</returns>
    public static TOut MapOr<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns a default value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the default value.</returns>
    public static async Task<TOut> MapOr<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOr(defaultValue, selector);

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns a default value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the default value.</returns>
    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValue
        };

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns a default value.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValue">The default value to return if the Option is None.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the default value.</returns>
    public static async Task<TOut> MapOrAsync<TIn, TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrAsync(defaultValue, selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>An Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static TOut MapOrElse<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => defaultValueProvider()
        };

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElse<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, TOut> selector)
        => (await self.ConfigureAwait(false)).MapOrElse(defaultValueProvider, selector);

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => defaultValueProvider()
        };

    /// <summary>
    /// Transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Option<TIn> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => selector(value),
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    /// <summary>
    /// Transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, TOut> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Option<TIn> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => self switch
        {
            { IsSome: true, Value: var value } => await selector(value).ConfigureAwait(false),
            _ => await defaultValueProvider().ConfigureAwait(false)
        };

    /// <summary>
    /// Transforms the value in the Option using a selector function or returns the result of a default value provider.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value in the Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="defaultValueProvider">A function that provides the default value.</param>
    /// <param name="selector">The function to transform the value.</param>
    /// <returns>A Task of an Option that contains the transformed value if the original Option is Some, otherwise the result of the default value provider.</returns>
    public static async Task<TOut> MapOrElseAsync<TIn, TOut>(this Task<Option<TIn>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).MapOrElseAsync(defaultValueProvider, selector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the Option into a Result, using the provided error if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="error">The error to use if the Option is None.</param>
    /// <returns>A Result that contains the value if the Option is Some, otherwise the provided error.</returns>
    public static Result<TValue, TError> SomeOr<TValue, TError>(this Option<TValue> self, TError error)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => error
        };

    /// <summary>
    /// Asynchronously transforms the Option into a Result, using the provided error if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="error">The error to use if the Option is None.</param>
    /// <returns>A Task of a Result that contains the value if the Option is Some, otherwise the provided error.</returns>
    public static async Task<Result<TValue, TError>> SomeOr<TValue, TError>(this Task<Option<TValue>> self, TError error)
        => (await self.ConfigureAwait(false)).SomeOr(error);

    /// <summary>
    /// Transforms the Option into a Result, using the result of the errorProvider if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="errorProvider">A function that provides the error.</param>
    /// <returns>A Result that contains the value if the Option is Some, otherwise the result of the errorProvider.</returns>
    public static Result<TValue, TError> SomeOrElse<TValue, TError>(this Option<TValue> self, Func<TError> errorProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => errorProvider()
        };

    /// <summary>
    /// Asynchronously transforms the Option into a Result, using the result of the errorProvider if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="errorProvider">A function that provides the error.</param>
    /// <returns>A Task of a Result that contains the value if the Option is Some, otherwise the result of the errorProvider.</returns>
    public static async Task<Result<TValue, TError>> SomeOrElse<TValue, TError>(this Task<Option<TValue>> self, Func<TError> errorProvider)
        => (await self.ConfigureAwait(false)).SomeOrElse(errorProvider);

    /// <summary>
    /// Asynchronously transforms the Option into a Result, using the result of the errorProvider if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="errorProvider">A function that provides the error.</param>
    /// <returns>A Task of a Result that contains the value if the Option is Some, otherwise the result of the errorProvider.</returns>
    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Option<TValue> self, Func<Task<TError>> errorProvider)
        => self switch
        {
            { IsSome: true, Value: var value } => value,
            _ => await errorProvider().ConfigureAwait(false)
        };

    /// <summary>
    /// Asynchronously transforms the Option into a Result, using the result of the errorProvider if the Option is None.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option to transform.</param>
    /// <param name="errorProvider">A function that provides the error.</param>
    /// <returns>A Task of a Result that contains the value if the Option is Some, otherwise the result of the errorProvider.</returns>
    public static async Task<Result<TValue, TError>> SomeOrElseAsync<TValue, TError>(this Task<Option<TValue>> self, Func<Task<TError>> errorProvider)
        => await (await self.ConfigureAwait(false)).SomeOrElseAsync(errorProvider).ConfigureAwait(false);

    /// <summary>
    /// Transposes an Option of a Result into a Result of an Option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option of a Result to transpose.</param>
    /// <returns>A Result of an Option that contains the Option of the value if the original Result is successful, otherwise the error.</returns>
    public static Result<Option<TValue>, TError> Transpose<TValue, TError>(this Option<Result<TValue, TError>> self)
        => self switch
        {
            { IsSome: true, Value: { IsSuccess: true, Value: var value }} => Option.Some(value),
            { IsSome: true, Value: { IsError: true, Error: var error }} => error!,
            _ => Option.None<TValue>()
        };

    /// <summary>
    /// Asynchronously transposes an Option of a Result into a Result of an Option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Result.</typeparam>
    /// <typeparam name="TError">The type of the error in the Result.</typeparam>
    /// <param name="self">The Option of a Result to transpose.</param>
    /// <returns>A Task of a Result of an Option that contains the Option of the value if the original Result is successful, otherwise the error.</returns>
    public static async Task<Result<Option<TValue>, TError>> Transpose<TValue, TError>(this Task<Option<Result<TValue, TError>>> self)
        => (await self.ConfigureAwait(false)).Transpose();

    /// <summary>
    /// Flattens a nested Option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The nested Option to flatten.</param>
    /// <returns>An Option that contains the value if the original Option is Some, otherwise None.</returns>
    public static Option<TValue> Flatten<TValue>(this Option<Option<TValue>> self)
        => self switch
        {
            { IsSome: true, Value: var option} => option,
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously flattens a nested Option.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the Option.</typeparam>
    /// <param name="self">The nested Option to flatten.</param>
    /// <returns>A Task of an Option that contains the value if the original Option is Some, otherwise None.</returns>
    public static async Task<Option<TValue>> Flatten<TValue>(this Task<Option<Option<TValue>>> self)
        => (await self.ConfigureAwait(false)).Flatten();

    /// <summary>
    /// Combines two Options into a single Option of a tuple.
    /// </summary>
    /// <typeparam name="TValue1">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TValue2">The type of the value in the second Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>An Option of a tuple that contains the values of the original Options if both are Some, otherwise None.</returns>
    public static Option<(TValue1 Left, TValue2 Right)> Zip<TValue1, TValue2>(this Option<TValue1> self, Option<TValue2> option)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => (left!, right!),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously combines two Options into a single Option of a tuple.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <returns>A Task of an Option of a tuple that contains the values of the original Options if both are Some, otherwise None.</returns>
    public static async Task<Option<(TLeft Left, TRight Right)>> Zip<TLeft, TRight>(this Task<Option<TLeft>> self, Option<TRight> option)
        => (await self.ConfigureAwait(false)).Zip(option);

    /// <summary>
    /// Combines two Options into a single Option using a selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <param name="selector">The function to combine the values.</param>
    /// <returns>An Option that contains the result of the selector function if both original Options are Some, otherwise None.</returns>
    public static Option<TOut> ZipWith<TLeft, TRight, TOut>(this Option<TLeft> self, Option<TRight> option, Func<TLeft, TRight, TOut> selector)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => selector(left!, right!),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously combines two Options into a single Option using a selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <param name="selector">The function to combine the values.</param>
    /// <returns>A Task of an Option that contains the result of the selector function if both original Options are Some, otherwise None.</returns>
    public static async Task<Option<TOut>> ZipWith<TLeft, TRight, TOut>(this Task<Option<TLeft>> self, Option<TRight> option, Func<TLeft, TRight, TOut> selector)
        => (await self.ConfigureAwait(false)).ZipWith(option, selector);

    /// <summary>
    /// Asynchronously combines two Options into a single Option using a selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <param name="selector">The function to combine the values.</param>
    /// <returns>A Task of an Option that contains the result of the selector function if both original Options are Some, otherwise None.</returns>
    public static async Task<Option<TOut>> ZipWithAsync<TLeft, TRight, TOut>(this Option<TLeft> self, Option<TRight> option, Func<TLeft, TRight, Task<TOut>> selector)
        => (self, option) switch
        {
            ({ IsSome: true, Value: var left }, { IsSome: true, Value: var right}) => await selector(left!, right!).ConfigureAwait(false),
            _ => None.Instance
        };

    /// <summary>
    /// Asynchronously combines two Options into a single Option using a selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <typeparam name="TOut">The type of the output value in the Option.</typeparam>
    /// <param name="self">The first Option.</param>
    /// <param name="option">The second Option.</param>
    /// <param name="selector">The function to combine the values.</param>
    /// <returns>A Task of an Option that contains the result of the selector function if both original Options are Some, otherwise None.</returns>
    public static async Task<Option<TOut>> ZipWithAsync<TLeft, TRight, TOut>(this Task<Option<TLeft>> self, Option<TRight> option, Func<TLeft, TRight, Task<TOut>> selector)
        => await (await self.ConfigureAwait(false)).ZipWithAsync(option, selector).ConfigureAwait(false);

    /// <summary>
    /// Splits an Option of a tuple into a tuple of two Options.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <param name="self">The Option of a tuple to split.</param>
    /// <returns>A tuple of two Options that contain the values of the original tuple if the Option is Some, otherwise None.</returns>
    public static (Option<TLeft> Left, Option<TRight> Right) Unzip<TLeft, TRight>(this Option<(TLeft Left, TRight Right)> self)
        => self switch
        {
            { IsSome: true, Value: var (left, right) } => (left, right),
            _ => (None.Instance, None.Instance)
        };

    /// <summary>
    /// Asynchronously splits an Option of a tuple into a tuple of two Options.
    /// </summary>
    /// <typeparam name="TLeft">The type of the value in the first Option.</typeparam>
    /// <typeparam name="TRight">The type of the value in the second Option.</typeparam>
    /// <param name="self">The Option of a tuple to split.</param>
    /// <returns>A Task of a tuple of two Options that contain the values of the original tuple if the Option is Some, otherwise None.</returns>
    public static async Task<(Option<TLeft> Left, Option<TRight> Right)> Unzip<TLeft, TRight>(this Task<Option<(TLeft Left, TRight Right)>> self)
        => (await self.ConfigureAwait(false)).Unzip();
}