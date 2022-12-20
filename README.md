# DotnetFetch

- Simple .NET implementation of JavaScript fetch API.
- Package is on [Nuget](https://www.nuget.org/packages/DotnetFetch/).
- Following (partially) the [MDN](https://developer.mozilla.org/en-US/docs/Web/API/fetch) specifications.

## Installation

```bash
> Install-Package DotnetFetch
```

## Usage

```cs
using static DotnetFetch.GlobalFetch;

var result = await Fetch("https://jsonplaceholder.typicode.com/todos/1");

Console.WriteLine(result.Text());
```

```cs
using static DotnetFetch.GlobalFetch;

var result = await Fetch<string>("https://jsonplaceholder.typicode.com/todos/1");

Console.WriteLine(result);
```
