Langutils
=========

Langutil - *noun* /læŋˈjuːtɪl/ - A mythical creature that helps you with your language needs.
And we pack some of them!

This is a collection of language utilities for C#, mainly inspired from other languages. Currently includes
- Result<TValue, TError>
- Option<TValue>
- Defer, DeferAsync
- DeferWith<TContext>, DeferWithAsync<TContext>
- Unit

## Result<TValue, TError>

A type that represents either a value or an error. It is similar to `Either` in functional languages.
Contains extensions to work with Result, Task<Result> and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Result` type, with some sprinkled in C#-isms.
To create a result, either use methods provided in the `Result` static class or use implicit operators

- Aggregate<TIn,TOut,TError>(this IEnumerable<Result<TIn,TError>> options, TOut seed, Func<TOut,TIn,TOut> selector):Result<TOut,TError>
- Aggregate<TValue,TError>(this IEnumerable<Result<TValue,TError>> options, Func<TValue,TValue,TValue> selector):Result<TValue,TError>
- And<TIn,TOut,TError>(this Result<TIn,TError> self, Result<TOut,TError> option):Result<TOut,TError>
- And<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Result<TOut,TError> option):Task<Result<TOut,TError>>
- AndThen<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TIn,Result<TOut,TError>> resultProvider):Result<TOut,TError>
- AndThen<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TIn,Result<TOut,TError>> optionProvider):Task<Result<TOut,TError>>
- AndThenAsync<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TIn,Task<Result<TOut,TError>>> optionProvider):Task<Result<TOut,TError>>
- AndThenAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TIn,Task<Result<TOut,TError>>> optionProvider):Task<Result<TOut,TError>>
- AsEnumerable<TValue,TError>(this Result<TValue,TError> self):IEnumerable<TValue>
- AsEnumerable<TValue,TError>(this Task<Result<TValue,TError>> self):Task<IEnumerable<TValue>>
- Collect<TValue,TError>(this IEnumerable<Result<TValue,TError>> options):Result<IEnumerable<TValue>,TError>
- Collect<TValue,TError>(this List<Result<TValue,TError>> options):Result<List<TValue>,TError>
- Collect<TValue,TError>(this Result<TValue,TError>[] options):Result<TValue[],TError>
- Collect<TValue,TError>(this Task<IEnumerable<Result<TValue,TError>>> results):Task<Result<IEnumerable<TValue>,TError>>
- Collect<TValue,TError>(this Task<List<Result<TValue,TError>>> results):Task<Result<List<TValue>,TError>>
- Collect<TValue,TError>(this Task<Result<TValue,TError>[]> results):Task<Result<TValue[],TError>>
- CompareTo<TValue,TError>(this Result<TValue,TError> self, Result<TValue,TError> option):int
- Error<TValue,TError>(this Result<TValue,TError> self):Option<TError>
- Error<TValue,TError>(this Task<Result<TValue,TError>> self):Task<Option<TError>>
- Expect<TValue,TError>(this Result<TValue,TError> self, string message):TValue
- Expect<TValue,TError>(this Task<Result<TValue,TError>> self, string message):Task<TValue>
- ExpectError<TValue,TError>(this Result<TValue,TError> self, string message):TError?
- ExpectError<TValue,TError>(this Task<Result<TValue,TError>> self, string message):Task<TError?>
- Flatten<TValue,TError>(this Result<Result<TValue,TError>,TError> self):Result<TValue,TError>
- Flatten<TValue,TError>(this Task<Result<Result<TValue,TError>,TError>> self):Task<Result<TValue,TError>>
- IsErrorAnd<TValue,TError>(this Result<TValue,TError> self, Func<TError?,bool> predicate):bool
- IsErrorAnd<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TError?,bool> predicate):Task<bool>
- IsErrorAndAsync<TValue,TError>(this Result<TValue,TError> self, Func<TError?,Task<bool>> predicate):Task<bool>
- IsErrorAndAsync<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TError?,Task<bool>> predicate):Task<bool>
- IsSuccessAnd<TValue,TError>(this Result<TValue,TError> self, Func<TValue,bool> predicate):bool
- IsSuccessAnd<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TValue,bool> predicate):Task<bool>
- IsSuccessAndAsync<TValue,TError>(this Result<TValue,TError> self, Func<TValue,Task<bool>> predicate):Task<bool>
- IsSuccessAndAsync<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TValue,Task<bool>> predicate):Task<bool>
- Map<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TIn,TOut> selector):Result<TOut,TError>
- Map<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TIn,TOut> selector):Task<Result<TOut,TError>>
- MapAsync<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TIn,Task<TOut>> selector):Task<Result<TOut,TError>>
- MapAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TIn,Task<TOut>> selector):Task<Result<TOut,TError>>
- MapError<TValue,TIn,TOut>(this Result<TValue,TIn> self, Func<TIn?,TOut> selector):Result<TValue,TOut>
- MapError<TValue,TIn,TOut>(this Task<Result<TValue,TIn>> self, Func<TIn?,TOut> selector):Task<Result<TValue,TOut>>
- MapErrorAsync<TValue,TIn,TOut>(this Result<TValue,TIn> self, Func<TIn?,Task<TOut>> selector):Task<Result<TValue,TOut>>
- MapErrorAsync<TValue,TIn,TOut>(this Task<Result<TValue,TIn>> self, Func<TIn?,Task<TOut>> selector):Task<Result<TValue,TOut>>
- MapOr<TIn,TOut,TError>(this Result<TIn,TError> self, TOut defaultValue, Func<TIn,TOut> selector):TOut
- MapOr<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, TOut defaultValue, Func<TIn,TOut> selector):Task<TOut>
- MapOrAsync<TIn,TOut,TError>(this Result<TIn,TError> self, TOut defaultValue, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, TOut defaultValue, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElse<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TOut> defaultValueProvider, Func<TIn,TOut> selector):TOut
- MapOrElse<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TOut> defaultValueProvider, Func<TIn,TOut> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Result<TIn,TError> self, Func<TOut> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Result<TIn,TError> self, Func<Task<TOut>> defaultValueProvider, Func<TIn,TOut> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Result<TIn,TError> self, Func<Task<TOut>> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<TOut> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn,TOut> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut,TError>(this Task<Result<TIn,TError>> self, Func<Task<TOut>> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- Or<TValue,TIn,TOut>(this Result<TValue,TIn> self, Result<TValue,TOut> option):Result<TValue,TOut>
- Or<TValue,TIn,TOut>(this Task<Result<TValue,TIn>> self, Result<TValue,TOut> option):Task<Result<TValue,TOut>>
- OrElse<TValue,TIn,TOut>(this Result<TValue,TIn> self, Func<TIn?,Result<TValue,TOut>> resultProvider):Result<TValue,TOut>
- OrElse<TValue,TIn,TOut>(this Task<Result<TValue,TIn>> self, Func<TIn?,Result<TValue,TOut>> optionProvider):Task<Result<TValue,TOut>>
- OrElseAsync<TValue,TIn,TOut>(this Result<TValue,TIn> self, Func<TIn?,Task<Result<TValue,TOut>>> optionProvider):Task<Result<TValue,TOut>>
- OrElseAsync<TValue,TIn,TOut>(this Task<Result<TValue,TIn>> self, Func<TIn?,Task<Result<TValue,TOut>>> optionProvider):Task<Result<TValue,TOut>>
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- Success<TValue,TError>(this Result<TValue,TError> self):Option<TValue>
- Success<TValue,TError>(this Task<Result<TValue,TError>> self):Task<Option<TValue>>
- Tap<TValue,TError>(this Result<TValue,TError> self, Action<TValue> onSuccess):Result<TValue,TError>
- Tap<TValue,TError>(this Task<Result<TValue,TError>> self, Action<TValue> onSuccess):Task<Result<TValue,TError>>
- TapAsync<TValue,TError>(this Result<TValue,TError> self, Func<TValue,Task> onSuccess):Task<Result<TValue,TError>>
- TapAsync<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TValue,Task> onSuccess):Task<Result<TValue,TError>>
- TapError<TValue,TError>(this Result<TValue,TError> self, Action<TError?> onError):Result<TValue,TError>
- TapError<TValue,TError>(this Task<Result<TValue,TError>> self, Action<TError?> onError):Task<Result<TValue,TError>>
- TapErrorAsync<TValue,TError>(this Result<TValue,TError> self, Func<TError?,Task> onError):Task<Result<TValue,TError>>
- TapErrorAsync<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TError?,Task> onError):Task<Result<TValue,TError>>
- ToArray<TValue,TError>(this Result<TValue,TError> self):TValue[]
- ToArray<TValue,TError>(this Task<Result<TValue,TError>> self):Task<TValue[]>
- ToList<TValue,TError>(this Result<TValue,TError> self):List<TValue>
- ToList<TValue,TError>(this Task<Result<TValue,TError>> self):Task<List<TValue>>
- Transpose<TValue,TError>(this Result<Option<TValue>,TError> self):Option<Result<TValue,TError>>
- Transpose<TValue,TError>(this Task<Result<Option<TValue>,TError>> self):Task<Option<Result<TValue,TError>>>
- TryUnwrap<TValue,TError>(this Result<TValue,TError> self, out TValue? value):bool
- TryUnwrapError<TValue,TError>(this Result<TValue,TError> self, out TError? error):bool
- Unwrap<TValue,TError>(this Result<TValue,TError> self):TValue
- Unwrap<TValue,TError>(this Task<Result<TValue,TError>> self):Task<TValue>
- UnwrapError<TValue,TError>(this Result<TValue,TError> self):TError?
- UnwrapError<TValue,TError>(this Task<Result<TValue,TError>> self):Task<TError?>
- UnwrapOr<TValue,TError>(this Result<TValue,TError> self, TValue defaultValue):TValue
- UnwrapOr<TValue,TError>(this Task<Result<TValue,TError>> self, TValue defaultValue):Task<TValue>
- UnwrapOrDefault<TValue,TError>(this Result<TValue,TError> self):TValue?
- UnwrapOrDefault<TValue,TError>(this Task<Result<TValue,TError>> self):Task<TValue?>
- UnwrapOrElse<TValue,TError>(this Result<TValue,TError> self, Func<TError?,TValue> defaultValueProvider):TValue
- UnwrapOrElse<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TError?,TValue> defaultValueProvider):Task<TValue>
- UnwrapOrElseAsync<TValue,TError>(this Result<TValue,TError> self, Func<TError?,Task<TValue>> defaultValueProvider):Task<TValue>
- UnwrapOrElseAsync<TValue,TError>(this Task<Result<TValue,TError>> self, Func<TError?,Task<TValue>> defaultValueProvider):Task<TValue>f, Func<TError?,Task<TValue>> defaultValueProvider):Task<TValue>


## Option<TValue>

A type that represents either a value or nothing. It is similar to `Option` in functional languages.
Contains extensions to work with Option, Task<Option> and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Option` type, with some sprinkled in C#-isms.
To create an option, either use methods provided in the `Option` static class, `None.Instancce` 
or use implicit operators to convert from nullable values (`null` is coerced into `None`)

- Aggregate<TIn,TOut>(this IEnumerable<Option<TIn>> options, TOut seed, Func<TOut,TIn,TOut> selector):Option<TOut>
- Aggregate<TValue>(this IEnumerable<Option<TValue>> options, Func<TValue,TValue,TValue> selector):Option<TValue>
- And<TIn,TOut>(this Option<TIn> self, Option<TOut> option):Option<TOut>
- And<TIn,TOut>(this Task<Option<TIn>> self, Option<TOut> option):Task<Option<TOut>>
- AndThen<TIn,TOut>(this Option<TIn> self, Func<TIn,Option<TOut>> optionProvider):Option<TOut>
- AndThen<TIn,TOut>(this Task<Option<TIn>> self, Func<TIn,Option<TOut>> optionProvider):Task<Option<TOut>>
- AndThenAsync<TIn,TOut>(this Option<TIn> self, Func<TIn,Task<Option<TOut>>> optionProvider):Task<Option<TOut>>
- AndThenAsync<TIn,TOut>(this Task<Option<TIn>> self, Func<TIn,Task<Option<TOut>>> optionProvider):Task<Option<TOut>>
- AsEnumerable<TValue>(this Option<TValue> self):IEnumerable<TValue>
- AsEnumerable<TValue>(this Task<Option<TValue>> self):Task<IEnumerable<TValue>>
- Collect<TValue>(this IEnumerable<Option<TValue>> options):Option<IEnumerable<TValue>>
- Collect<TValue>(this List<Option<TValue>> options):Option<List<TValue>>
- Collect<TValue>(this Option<TValue>[] options):Option<TValue[]>
- Collect<TValue>(this Task<IEnumerable<Option<TValue>>> options):Task<Option<IEnumerable<TValue>>>
- Collect<TValue>(this Task<List<Option<TValue>>> options):Task<Option<List<TValue>>>
- Collect<TValue>(this Task<Option<TValue>[]> options):Task<Option<TValue[]>>
- CompareTo<TValue>(this Option<TValue> self, Option<TValue> option):int
- Expect<TValue>(this Option<TValue> self, string message):TValue
- Expect<TValue>(this Task<Option<TValue>> self, string message):Task<TValue>
- Filter<TValue>(this Option<TValue> self, Func<TValue,bool> predicate):Option<TValue>
- Filter<TValue>(this Task<Option<TValue>> self, Func<TValue,bool> predicate):Task<Option<TValue>>
- FilterAsync<TValue>(this Option<TValue> self, Func<TValue,Task<bool>> predicate):Task<Option<TValue>>
- FilterAsync<TValue>(this Task<Option<TValue>> self, Func<TValue,Task<bool>> predicate):Task<Option<TValue>>
- Flatten<TValue>(this Option<Option<TValue>> self):Option<TValue>
- Flatten<TValue>(this Task<Option<Option<TValue>>> self):Task<Option<TValue>>
- IsSomeAnd<TValue>(this Option<TValue> self, Func<TValue,bool> predicate):bool
- IsSomeAnd<TValue>(this Task<Option<TValue>> self, Func<TValue,bool> predicate):Task<bool>
- IsSomeAndAsync<TValue>(this Option<TValue> self, Func<TValue,Task<bool>> predicate):Task<bool>
- IsSomeAndAsync<TValue>(this Task<Option<TValue>> self, Func<TValue,Task<bool>> predicate):Task<bool>
- Map<TIn,TOut>(this Option<TIn> self, Func<TIn,TOut> selector):Option<TOut>
- Map<TIn,TOut>(this Task<Option<TIn>> self, Func<TIn,TOut> selector):Task<Option<TOut>>
- MapAsync<TIn,TOut>(this Option<TIn> self, Func<TIn,Task<TOut>> selector):Task<Option<TOut>>
- MapAsync<TIn,TOut>(this Task<Option<TIn>> self, Func<TIn,Task<TOut>> selector):Task<Option<TOut>>
- MapOr<TIn,TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn,TOut> selector):TOut
- MapOr<TIn,TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn,TOut> selector):Task<TOut>
- MapOrAsync<TIn,TOut>(this Option<TIn> self, TOut defaultValue, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrAsync<TIn,TOut>(this Task<Option<TIn>> self, TOut defaultValue, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElse<TIn,TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn,TOut> selector):TOut
- MapOrElse<TIn,TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn,TOut> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut>(this Option<TIn> self, Func<TOut> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MapOrElseAsync<TIn,TOut>(this Task<Option<TIn>> self, Func<TOut> defaultValueProvider, Func<TIn,Task<TOut>> selector):Task<TOut>
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- MemberwiseClone():object (in Object)
- Or<TValue>(this Option<TValue> self, Option<TValue> option):Option<TValue>
- Or<TValue>(this Task<Option<TValue>> self, Option<TValue> option):Task<Option<TValue>>
- OrElse<TValue>(this Option<TValue> self, Func<Option<TValue>> optionProvider):Option<TValue>
- OrElse<TValue>(this Task<Option<TValue>> self, Func<Option<TValue>> optionProvider):Task<Option<TValue>>
- OrElseAsync<TValue>(this Option<TValue> self, Func<Task<Option<TValue>>> optionProvider):Task<Option<TValue>>
- OrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<Option<TValue>>> optionProvider):Task<Option<TValue>>
- Product<TValue>(this IEnumerable<Option<TValue>> options):Option<TValue>
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- ReferenceEquals(object?, object?):bool (in Object)
- SomeOr<TValue,TError>(this Option<TValue> self, TError error):Result<TValue,TError>
- SomeOr<TValue,TError>(this Task<Option<TValue>> self, TError error):Task<Result<TValue,TError>>
- SomeOrElse<TValue,TError>(this Option<TValue> self, Func<TError> errorProvider):Result<TValue,TError>
- SomeOrElse<TValue,TError>(this Task<Option<TValue>> self, Func<TError> errorProvider):Task<Result<TValue,TError>>
- SomeOrElseAsync<TValue,TError>(this Option<TValue> self, Func<Task<TError>> errorProvider):Task<Result<TValue,TError>>
- SomeOrElseAsync<TValue,TError>(this Task<Option<TValue>> self, Func<Task<TError>> errorProvider):Task<Result<TValue,TError>>
- Sum<TValue>(this IEnumerable<Option<TValue>> options):Option<TValue>
- Tap<TValue>(this Option<TValue> self, Action<TValue> onSome):Option<TValue>
- Tap<TValue>(this Task<Option<TValue>> self, Action<TValue> onSome):Task<Option<TValue>>
- TapAsync<TValue>(this Option<TValue> self, Func<TValue,Task> onSome):Task<Option<TValue>>
- TapAsync<TValue>(this Task<Option<TValue>> self, Func<TValue,Task> onSome):Task<Option<TValue>>
- ToArray<TValue>(this Option<TValue> self):TValue[]
- ToArray<TValue>(this Task<Option<TValue>> self):Task<TValue[]>
- ToList<TValue>(this Option<TValue> self):List<TValue>
- ToList<TValue>(this Task<Option<TValue>> self):Task<List<TValue>>
- Transpose<TValue,TError>(this Option<Result<TValue,TError>> self):Result<Option<TValue>,TError>
- Transpose<TValue,TError>(this Task<Option<Result<TValue,TError>>> self):Task<Result<Option<TValue>,TError>>
- TryUnwrap<TValue>(this Option<TValue> self, out TValue? value):bool
- Unwrap<TValue>(this Task<Option<TValue>> self):Task<TValue>
- UnwrapOr<TValue>(this Option<TValue> self, TValue defaultValue):TValue
- UnwrapOr<TValue>(this Task<Option<TValue>> self, TValue defaultValue):Task<TValue>
- UnwrapOrDefault<TValue>(this Option<TValue> self):TValue?
- UnwrapOrDefault<TValue>(this Task<Option<TValue>> self):Task<TValue?>
- UnwrapOrElse<TValue>(this Option<TValue> self, Func<TValue> defaultValueProvider):TValue
- UnwrapOrElse<TValue>(this Task<Option<TValue>> self, Func<TValue> defaultValueProvider):Task<TValue>
- UnwrapOrElseAsync<TValue>(this Option<TValue> self, Func<Task<TValue>> defaultValueProvider):Task<TValue>
- UnwrapOrElseAsync<TValue>(this Task<Option<TValue>> self, Func<Task<TValue>> defaultValueProvider):Task<TValue>
- Unzip<TLeft,TRight>(this Task<Option<(TLeft Left, TRight Right)>> self):Task<(Option<TLeft> Left, Option<TRight> Right)>
- Unzip<TValue1,TValue2>(this Option<(TValue1 Left, TValue2 Right)> self):(Option<TValue1> Left, Option<TValue2> Right)
- Xor<TValue>(this Option<TValue> self, Option<TValue> option):Option<TValue>
- Xor<TValue>(this Task<Option<TValue>> self, Option<TValue> option):Task<Option<TValue>>
- Zip<TLeft,TRight>(this Task<Option<TLeft>> self, Option<TRight> option):Task<Option<(TLeft Left, TRight Right)>>
- Zip<TValue1,TValue2>(this Option<TValue1> self, Option<TValue2> option):Option<(TValue1 Left, TValue2 Right)>
- ZipWith<TIn1,TIn2,TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1,TIn2,TOut> selector):Option<TOut>
- ZipWith<TLeft,TRight,TOut>(this Task<Option<TLeft>> self, Option<TRight> option, Func<TLeft,TRight,TOut> selector):Task<Option<TOut>>
- ZipWithAsync<TIn1,TIn2,TOut>(this Option<TIn1> self, Option<TIn2> option, Func<TIn1,TIn2,Task<TOut>> selector):Task<Option<TOut>>
- ZipWithAsync<TIn1,TIn2,TOut>(this Task<Option<TIn1>> self, Option<TIn2> option, Func<TIn1,TIn2,Task<TOut>> selector):Task<Option<TOut>>

