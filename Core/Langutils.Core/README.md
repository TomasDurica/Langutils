Langutils.Core
==============

Langutil - *noun* /læŋˈjuːtɪl/ - A mythical creature that helps you with your language needs.
And we pack some of them!

This is a collection of language utilities for C#, mainly inspired from other languages. Currently includes
- Result&lt;TValue, TError&gt;
- Option&lt;TValue&gt;
- Defer, DeferAsync
- DeferWith&lt;TContext&gt;, DeferWithAsync&lt;TContext&gt;
- Let and Also extension methods
- Unit

## Result&lt;TValue, TError&gt;

A type that represents either a value or an error. It is similar to `Either` in functional languages.
Contains extensions to work with Result, Task&lt;Result&gt; and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Result` type, with some sprinkled in C#-isms.
To create a result, either use methods provided in the `Result` static class or use implicit operators

## Option&lt;TValue&gt;

A type that represents either a value or nothing. It is similar to `Option` in functional languages.
Contains extensions to work with Option, Task&lt;Option&gt; and Async overloads for higher order functions.

The set of available functions are directly inspired by Rust's `Option` type, with some sprinkled in C#-isms.
To create an option, either use methods provided in the `Option` static class, `None.Instancce` 
or use implicit operators to convert from nullable values (`null` is coerced into `None`)

## Defer, DeferAsync, DeferWith&lt;TContext&gt;, DeferWithAsync&lt;TContext&gt;

A type that represents a deferred action. It uses disposables to ensure the action gets reliably executed at specified times.
It is similar to `defer` in Go.
Contains extensions to work with Defer, Task&lt;Defer&gt; and Async overloads for higher order functions.
There is also a variant of defer that allows you to pass a context object to the deferred action for simplification.

## Let and Also extension methods

Extension methods that allow you to chain method calls in a more readable way.
Inspired by Kotlin's `let` and `also` functions.

## Unit

A type that represents a valueless type. It is similar to `void` in C#, but is a real type.
 