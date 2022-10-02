# DotnetFetch

Simple .NET implementation of JavaScript fetch API. 

## Installation

```bash
Install-Package DotnetFetch
```

## Usage

```cs
using DotnetFetch;

var result = await GlobalFetch.Fetch("https://jsonplaceholder.typicode.com/todos/1");

Console.WriteLine(result.Text());
```

```cs
using DotnetFetch;

var result = await GlobalFetch.Fetch<string>("https://jsonplaceholder.typicode.com/todos/1");

Console.WriteLine(result);
```