Langutils
=========

Langutil - *noun* /læŋˈjuːtɪl/ - A mythical creature that helps you with your language needs.
And we pack some of them!

This is a collection of language utilities for C#, mainly inspired from other languages. Currently includes
- Result&lt;TValue, TError&gt;
- Option&lt;TValue&gt;
- Defer, DeferAsync
- DeferWith&lt;TContext&gt;, DeferWithAsync&lt;TContext&gt;
- Unit

## Result&lt;TValue, TError&gt;

A type that represents either a value or an error. It is similar to `Either` in functional languages.
Contains extensions to work with Result, Task&lt;Result&gt; and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Result` type, with some sprinkled in C#-isms.
To create a result, either use methods provided in the `Result` static class or use implicit operators

- Aggregate&lt;TIn,TOut,TError&gt;(this IEnumerable&lt;Result&lt;TIn,TError&gt;&gt; options, TOut seed, Func&lt;TOut,TIn,TOut&gt; selector):Result&lt;TOut,TError&gt;
- Aggregate&lt;TValue,TError&gt;(this IEnumerable&lt;Result&lt;TValue,TError&gt;&gt; options, Func&lt;TValue,TValue,TValue&gt; selector):Result&lt;TValue,TError&gt;
- And&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Result&lt;TOut,TError&gt; option):Result&lt;TOut,TError&gt;
- And&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Result&lt;TOut,TError&gt; option):Task&lt;Result&lt;TOut,TError&gt;&gt;
- AndThen&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TIn,Result&lt;TOut,TError&gt;&gt; resultProvider):Result&lt;TOut,TError&gt;
- AndThen&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TIn,Result&lt;TOut,TError&gt;&gt; optionProvider):Task&lt;Result&lt;TOut,TError&gt;&gt;
- AndThenAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TIn,Task&lt;Result&lt;TOut,TError&gt;&gt;&gt; optionProvider):Task&lt;Result&lt;TOut,TError&gt;&gt;
- AndThenAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TIn,Task&lt;Result&lt;TOut,TError&gt;&gt;&gt; optionProvider):Task&lt;Result&lt;TOut,TError&gt;&gt;
- AsEnumerable&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):IEnumerable&lt;TValue&gt;
- AsEnumerable&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;IEnumerable&lt;TValue&gt;&gt;
- Collect&lt;TValue,TError&gt;(this IEnumerable&lt;Result&lt;TValue,TError&gt;&gt; options):Result&lt;IEnumerable&lt;TValue&gt;,TError&gt;
- Collect&lt;TValue,TError&gt;(this List&lt;Result&lt;TValue,TError&gt;&gt; options):Result&lt;List&lt;TValue&gt;,TError&gt;
- Collect&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt;[] options):Result&lt;TValue[],TError&gt;
- Collect&lt;TValue,TError&gt;(this Task&lt;IEnumerable&lt;Result&lt;TValue,TError&gt;&gt;&gt; results):Task&lt;Result&lt;IEnumerable&lt;TValue&gt;,TError&gt;&gt;
- Collect&lt;TValue,TError&gt;(this Task&lt;List&lt;Result&lt;TValue,TError&gt;&gt;&gt; results):Task&lt;Result&lt;List&lt;TValue&gt;,TError&gt;&gt;
- Collect&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;[]&gt; results):Task&lt;Result&lt;TValue[],TError&gt;&gt;
- CompareTo&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Result&lt;TValue,TError&gt; option):int
- Error&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):Option&lt;TError&gt;
- Error&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;Option&lt;TError&gt;&gt;
- Expect&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, string message):TValue
- Expect&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, string message):Task&lt;TValue&gt;
- ExpectError&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, string message):TError?
- ExpectError&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, string message):Task&lt;TError?&gt;
- Flatten&lt;TValue,TError&gt;(this Result&lt;Result&lt;TValue,TError&gt;,TError&gt; self):Result&lt;TValue,TError&gt;
- Flatten&lt;TValue,TError&gt;(this Task&lt;Result&lt;Result&lt;TValue,TError&gt;,TError&gt;&gt; self):Task&lt;Result&lt;TValue,TError&gt;&gt;
- IsErrorAnd&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TError?,bool&gt; predicate):bool
- IsErrorAnd&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TError?,bool&gt; predicate):Task&lt;bool&gt;
- IsErrorAndAsync&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TError?,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- IsErrorAndAsync&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TError?,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- IsSuccessAnd&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TValue,bool&gt; predicate):bool
- IsSuccessAnd&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TValue,bool&gt; predicate):Task&lt;bool&gt;
- IsSuccessAndAsync&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- IsSuccessAndAsync&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- Map&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TIn,TOut&gt; selector):Result&lt;TOut,TError&gt;
- Map&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TIn,TOut&gt; selector):Task&lt;Result&lt;TOut,TError&gt;&gt;
- MapAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;Result&lt;TOut,TError&gt;&gt;
- MapAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;Result&lt;TOut,TError&gt;&gt;
- MapError&lt;TValue,TIn,TOut&gt;(this Result&lt;TValue,TIn&gt; self, Func&lt;TIn?,TOut&gt; selector):Result&lt;TValue,TOut&gt;
- MapError&lt;TValue,TIn,TOut&gt;(this Task&lt;Result&lt;TValue,TIn&gt;&gt; self, Func&lt;TIn?,TOut&gt; selector):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- MapErrorAsync&lt;TValue,TIn,TOut&gt;(this Result&lt;TValue,TIn&gt; self, Func&lt;TIn?,Task&lt;TOut&gt;&gt; selector):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- MapErrorAsync&lt;TValue,TIn,TOut&gt;(this Task&lt;Result&lt;TValue,TIn&gt;&gt; self, Func&lt;TIn?,Task&lt;TOut&gt;&gt; selector):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- MapOr&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, TOut defaultValue, Func&lt;TIn,TOut&gt; selector):TOut
- MapOr&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, TOut defaultValue, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, TOut defaultValue, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, TOut defaultValue, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElse&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TError?, TOut&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):TOut
- MapOrElse&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TError?, TOut&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TError?, TOut&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TError?, Task&lt;TOut&gt;&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Result&lt;TIn,TError&gt; self, Func&lt;TError?, Task&lt;TOut&gt;&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TError?, TOut&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TError?, Task&lt;TOut&gt;&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut,TError&gt;(this Task&lt;Result&lt;TIn,TError&gt;&gt; self, Func&lt;TError?, Task&lt;TOut&gt;&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- Or&lt;TValue,TIn,TOut&gt;(this Result&lt;TValue,TIn&gt; self, Result&lt;TValue,TOut&gt; option):Result&lt;TValue,TOut&gt;
- Or&lt;TValue,TIn,TOut&gt;(this Task&lt;Result&lt;TValue,TIn&gt;&gt; self, Result&lt;TValue,TOut&gt; option):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- OrElse&lt;TValue,TIn,TOut&gt;(this Result&lt;TValue,TIn&gt; self, Func&lt;TIn?,Result&lt;TValue,TOut&gt;&gt; resultProvider):Result&lt;TValue,TOut&gt;
- OrElse&lt;TValue,TIn,TOut&gt;(this Task&lt;Result&lt;TValue,TIn&gt;&gt; self, Func&lt;TIn?,Result&lt;TValue,TOut&gt;&gt; optionProvider):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- OrElseAsync&lt;TValue,TIn,TOut&gt;(this Result&lt;TValue,TIn&gt; self, Func&lt;TIn?,Task&lt;Result&lt;TValue,TOut&gt;&gt;&gt; optionProvider):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- OrElseAsync&lt;TValue,TIn,TOut&gt;(this Task&lt;Result&lt;TValue,TIn&gt;&gt; self, Func&lt;TIn?,Task&lt;Result&lt;TValue,TOut&gt;&gt;&gt; optionProvider):Task&lt;Result&lt;TValue,TOut&gt;&gt;
- Success&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):Option&lt;TValue&gt;
- Success&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;Option&lt;TValue&gt;&gt;
- Tap&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Action&lt;TValue&gt; onSuccess):Result&lt;TValue,TError&gt;
- Tap&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Action&lt;TValue&gt; onSuccess):Task&lt;Result&lt;TValue,TError&gt;&gt;
- TapAsync&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TValue,Task&gt; onSuccess):Task&lt;Result&lt;TValue,TError&gt;&gt;
- TapAsync&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TValue,Task&gt; onSuccess):Task&lt;Result&lt;TValue,TError&gt;&gt;
- TapError&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Action&lt;TError?&gt; onError):Result&lt;TValue,TError&gt;
- TapError&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Action&lt;TError?&gt; onError):Task&lt;Result&lt;TValue,TError&gt;&gt;
- TapErrorAsync&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TError?,Task&gt; onError):Task&lt;Result&lt;TValue,TError&gt;&gt;
- TapErrorAsync&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TError?,Task&gt; onError):Task&lt;Result&lt;TValue,TError&gt;&gt;
- ToArray&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):TValue[]
- ToArray&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;TValue[]&gt;
- ToList&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):List&lt;TValue&gt;
- ToList&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;List&lt;TValue&gt;&gt;
- Transpose&lt;TValue,TError&gt;(this Result&lt;Option&lt;TValue&gt;,TError&gt; self):Option&lt;Result&lt;TValue,TError&gt;&gt;
- Transpose&lt;TValue,TError&gt;(this Task&lt;Result&lt;Option&lt;TValue&gt;,TError&gt;&gt; self):Task&lt;Option&lt;Result&lt;TValue,TError&gt;&gt;&gt;
- TryUnwrap&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, out TValue? value):bool
- TryUnwrapError&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, out TError? error):bool
- Unwrap&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):TValue
- Unwrap&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;TValue&gt;
- UnwrapError&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):TError?
- UnwrapError&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;TError?&gt;
- UnwrapOr&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, TValue defaultValue):TValue
- UnwrapOr&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, TValue defaultValue):Task&lt;TValue&gt;
- UnwrapOrDefault&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self):TValue?
- UnwrapOrDefault&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self):Task&lt;TValue?&gt;
- UnwrapOrElse&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TError?,TValue&gt; defaultValueProvider):TValue
- UnwrapOrElse&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TError?,TValue&gt; defaultValueProvider):Task&lt;TValue&gt;
- UnwrapOrElseAsync&lt;TValue,TError&gt;(this Result&lt;TValue,TError&gt; self, Func&lt;TError?,Task&lt;TValue&gt;&gt; defaultValueProvider):Task&lt;TValue&gt;
- UnwrapOrElseAsync&lt;TValue,TError&gt;(this Task&lt;Result&lt;TValue,TError&gt;&gt; self, Func&lt;TError?,Task&lt;TValue&gt;&gt; defaultValueProvider):Task&lt;TValue&gt;f, Func&lt;TError?,Task&lt;TValue&gt;&gt; defaultValueProvider):Task&lt;TValue&gt;


## Option&lt;TValue&gt;

A type that represents either a value or nothing. It is similar to `Option` in functional languages.
Contains extensions to work with Option, Task&lt;Option&gt; and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Option` type, with some sprinkled in C#-isms.
To create an option, either use methods provided in the `Option` static class, `None.Instancce` 
or use implicit operators to convert from nullable values (`null` is coerced into `None`)

- Aggregate&lt;TIn,TOut&gt;(this IEnumerable&lt;Option&lt;TIn&gt;&gt; options, TOut seed, Func&lt;TOut,TIn,TOut&gt; selector):Option&lt;TOut&gt;
- Aggregate&lt;TValue&gt;(this IEnumerable&lt;Option&lt;TValue&gt;&gt; options, Func&lt;TValue,TValue,TValue&gt; selector):Option&lt;TValue&gt;
- And&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Option&lt;TOut&gt; option):Option&lt;TOut&gt;
- And&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Option&lt;TOut&gt; option):Task&lt;Option&lt;TOut&gt;&gt;
- AndThen&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TIn,Option&lt;TOut&gt;&gt; optionProvider):Option&lt;TOut&gt;
- AndThen&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TIn,Option&lt;TOut&gt;&gt; optionProvider):Task&lt;Option&lt;TOut&gt;&gt;
- AndThenAsync&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TIn,Task&lt;Option&lt;TOut&gt;&gt;&gt; optionProvider):Task&lt;Option&lt;TOut&gt;&gt;
- AndThenAsync&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TIn,Task&lt;Option&lt;TOut&gt;&gt;&gt; optionProvider):Task&lt;Option&lt;TOut&gt;&gt;
- AsEnumerable&lt;TValue&gt;(this Option&lt;TValue&gt; self):IEnumerable&lt;TValue&gt;
- AsEnumerable&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self):Task&lt;IEnumerable&lt;TValue&gt;&gt;
- Collect&lt;TValue&gt;(this IEnumerable&lt;Option&lt;TValue&gt;&gt; options):Option&lt;IEnumerable&lt;TValue&gt;&gt;
- Collect&lt;TValue&gt;(this List&lt;Option&lt;TValue&gt;&gt; options):Option&lt;List&lt;TValue&gt;&gt;
- Collect&lt;TValue&gt;(this Option&lt;TValue&gt;[] options):Option&lt;TValue[]&gt;
- Collect&lt;TValue&gt;(this Task&lt;IEnumerable&lt;Option&lt;TValue&gt;&gt;&gt; options):Task&lt;Option&lt;IEnumerable&lt;TValue&gt;&gt;&gt;
- Collect&lt;TValue&gt;(this Task&lt;List&lt;Option&lt;TValue&gt;&gt;&gt; options):Task&lt;Option&lt;List&lt;TValue&gt;&gt;&gt;
- Collect&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;[]&gt; options):Task&lt;Option&lt;TValue[]&gt;&gt;
- CompareTo&lt;TValue&gt;(this Option&lt;TValue&gt; self, Option&lt;TValue&gt; option):int
- Expect&lt;TValue&gt;(this Option&lt;TValue&gt; self, string message):TValue
- Expect&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, string message):Task&lt;TValue&gt;
- Filter&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue,bool&gt; predicate):Option&lt;TValue&gt;
- Filter&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue,bool&gt; predicate):Task&lt;Option&lt;TValue&gt;&gt;
- FilterAsync&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;Option&lt;TValue&gt;&gt;
- FilterAsync&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;Option&lt;TValue&gt;&gt;
- Flatten&lt;TValue&gt;(this Option&lt;Option&lt;TValue&gt;&gt; self):Option&lt;TValue&gt;
- Flatten&lt;TValue&gt;(this Task&lt;Option&lt;Option&lt;TValue&gt;&gt;&gt; self):Task&lt;Option&lt;TValue&gt;&gt;
- IsSomeAnd&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue,bool&gt; predicate):bool
- IsSomeAnd&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue,bool&gt; predicate):Task&lt;bool&gt;
- IsSomeAndAsync&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- IsSomeAndAsync&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue,Task&lt;bool&gt;&gt; predicate):Task&lt;bool&gt;
- Map&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TIn,TOut&gt; selector):Option&lt;TOut&gt;
- Map&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TIn,TOut&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;
- MapAsync&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;
- MapAsync&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;
- MapOr&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, TOut defaultValue, Func&lt;TIn,TOut&gt; selector):TOut
- MapOr&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, TOut defaultValue, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrAsync&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, TOut defaultValue, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrAsync&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, TOut defaultValue, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElse&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TOut&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):TOut
- MapOrElse&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TOut&gt; defaultValueProvider, Func&lt;TIn,TOut&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut&gt;(this Option&lt;TIn&gt; self, Func&lt;TOut&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- MapOrElseAsync&lt;TIn,TOut&gt;(this Task&lt;Option&lt;TIn&gt;&gt; self, Func&lt;TOut&gt; defaultValueProvider, Func&lt;TIn,Task&lt;TOut&gt;&gt; selector):Task&lt;TOut&gt;
- Or&lt;TValue&gt;(this Option&lt;TValue&gt; self, Option&lt;TValue&gt; option):Option&lt;TValue&gt;
- Or&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Option&lt;TValue&gt; option):Task&lt;Option&lt;TValue&gt;&gt;
- OrElse&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;Option&lt;TValue&gt;&gt; optionProvider):Option&lt;TValue&gt;
- OrElse&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;Option&lt;TValue&gt;&gt; optionProvider):Task&lt;Option&lt;TValue&gt;&gt;
- OrElseAsync&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;Task&lt;Option&lt;TValue&gt;&gt;&gt; optionProvider):Task&lt;Option&lt;TValue&gt;&gt;
- OrElseAsync&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;Task&lt;Option&lt;TValue&gt;&gt;&gt; optionProvider):Task&lt;Option&lt;TValue&gt;&gt;
- Product&lt;TValue&gt;(this IEnumerable&lt;Option&lt;TValue&gt;&gt; options):Option&lt;TValue&gt;
- SomeOr&lt;TValue,TError&gt;(this Option&lt;TValue&gt; self, TError error):Result&lt;TValue,TError&gt;
- SomeOr&lt;TValue,TError&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, TError error):Task&lt;Result&lt;TValue,TError&gt;&gt;
- SomeOrElse&lt;TValue,TError&gt;(this Option&lt;TValue&gt; self, Func&lt;TError&gt; errorProvider):Result&lt;TValue,TError&gt;
- SomeOrElse&lt;TValue,TError&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TError&gt; errorProvider):Task&lt;Result&lt;TValue,TError&gt;&gt;
- SomeOrElseAsync&lt;TValue,TError&gt;(this Option&lt;TValue&gt; self, Func&lt;Task&lt;TError&gt;&gt; errorProvider):Task&lt;Result&lt;TValue,TError&gt;&gt;
- SomeOrElseAsync&lt;TValue,TError&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;Task&lt;TError&gt;&gt; errorProvider):Task&lt;Result&lt;TValue,TError&gt;&gt;
- Sum&lt;TValue&gt;(this IEnumerable&lt;Option&lt;TValue&gt;&gt; options):Option&lt;TValue&gt;
- Tap&lt;TValue&gt;(this Option&lt;TValue&gt; self, Action&lt;TValue&gt; onSome):Option&lt;TValue&gt;
- Tap&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Action&lt;TValue&gt; onSome):Task&lt;Option&lt;TValue&gt;&gt;
- TapAsync&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue,Task&gt; onSome):Task&lt;Option&lt;TValue&gt;&gt;
- TapAsync&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue,Task&gt; onSome):Task&lt;Option&lt;TValue&gt;&gt;
- ToArray&lt;TValue&gt;(this Option&lt;TValue&gt; self):TValue[]
- ToArray&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self):Task&lt;TValue[]&gt;
- ToList&lt;TValue&gt;(this Option&lt;TValue&gt; self):List&lt;TValue&gt;
- ToList&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self):Task&lt;List&lt;TValue&gt;&gt;
- Transpose&lt;TValue,TError&gt;(this Option&lt;Result&lt;TValue,TError&gt;&gt; self):Result&lt;Option&lt;TValue&gt;,TError&gt;
- Transpose&lt;TValue,TError&gt;(this Task&lt;Option&lt;Result&lt;TValue,TError&gt;&gt;&gt; self):Task&lt;Result&lt;Option&lt;TValue&gt;,TError&gt;&gt;
- TryUnwrap&lt;TValue&gt;(this Option&lt;TValue&gt; self, out TValue? value):bool
- Unwrap&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self):Task&lt;TValue&gt;
- UnwrapOr&lt;TValue&gt;(this Option&lt;TValue&gt; self, TValue defaultValue):TValue
- UnwrapOr&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, TValue defaultValue):Task&lt;TValue&gt;
- UnwrapOrDefault&lt;TValue&gt;(this Option&lt;TValue&gt; self):TValue?
- UnwrapOrDefault&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self):Task&lt;TValue?&gt;
- UnwrapOrElse&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;TValue&gt; defaultValueProvider):TValue
- UnwrapOrElse&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;TValue&gt; defaultValueProvider):Task&lt;TValue&gt;
- UnwrapOrElseAsync&lt;TValue&gt;(this Option&lt;TValue&gt; self, Func&lt;Task&lt;TValue&gt;&gt; defaultValueProvider):Task&lt;TValue&gt;
- UnwrapOrElseAsync&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Func&lt;Task&lt;TValue&gt;&gt; defaultValueProvider):Task&lt;TValue&gt;
- Unzip&lt;TLeft,TRight&gt;(this Task&lt;Option&lt;(TLeft Left, TRight Right)&gt;&gt; self):Task&lt;(Option&lt;TLeft&gt; Left, Option&lt;TRight&gt; Right)&gt;
- Unzip&lt;TValue1,TValue2&gt;(this Option&lt;(TValue1 Left, TValue2 Right)&gt; self):(Option&lt;TValue1&gt; Left, Option&lt;TValue2&gt; Right)
- Xor&lt;TValue&gt;(this Option&lt;TValue&gt; self, Option&lt;TValue&gt; option):Option&lt;TValue&gt;
- Xor&lt;TValue&gt;(this Task&lt;Option&lt;TValue&gt;&gt; self, Option&lt;TValue&gt; option):Task&lt;Option&lt;TValue&gt;&gt;
- Zip&lt;TLeft,TRight&gt;(this Task&lt;Option&lt;TLeft&gt;&gt; self, Option&lt;TRight&gt; option):Task&lt;Option&lt;(TLeft Left, TRight Right)&gt;&gt;
- Zip&lt;TValue1,TValue2&gt;(this Option&lt;TValue1&gt; self, Option&lt;TValue2&gt; option):Option&lt;(TValue1 Left, TValue2 Right)&gt;
- ZipWith&lt;TIn1,TIn2,TOut&gt;(this Option&lt;TIn1&gt; self, Option&lt;TIn2&gt; option, Func&lt;TIn1,TIn2,TOut&gt; selector):Option&lt;TOut&gt;
- ZipWith&lt;TLeft,TRight,TOut&gt;(this Task&lt;Option&lt;TLeft&gt;&gt; self, Option&lt;TRight&gt; option, Func&lt;TLeft,TRight,TOut&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;
- ZipWithAsync&lt;TIn1,TIn2,TOut&gt;(this Option&lt;TIn1&gt; self, Option&lt;TIn2&gt; option, Func&lt;TIn1,TIn2,Task&lt;TOut&gt;&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;
- ZipWithAsync&lt;TIn1,TIn2,TOut&gt;(this Task&lt;Option&lt;TIn1&gt;&gt; self, Option&lt;TIn2&gt; option, Func&lt;TIn1,TIn2,Task&lt;TOut&gt;&gt; selector):Task&lt;Option&lt;TOut&gt;&gt;

