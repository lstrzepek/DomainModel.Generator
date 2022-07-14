# DomainModel.Generator
  
[![CI](https://github.com/lstrzepek/DomainModel.Generator/actions/workflows/ci.yml/badge.svg)](https://github.com/lstrzepek/DomainModel.Generator/actions/workflows/ci.yml)

Domain model is a tool to analyze your model (.Net dll) and generate up to date diagram which can be embedded into your documentation.

```
Domain model it a tool to analyze your model and generate upd to date diagram which can be embedded into your documentation.

Usage:
  model generate [--diagram=<dt>] (-m=<md> | --module=<md>) (-o=<out> | --output=<out>) [--format=<f>] [-n=<ns>... | --namespace=<ns>...] [--ignore-namespace=<ign>...] [-t=<type>... | --type=<type>...] [--ignore-type=<igt>...]
  model -h | --help
  model --version

Options:
  -h --help                         Show this screen.
  --version                         Show version.
  -m=<md> --module=<md>             **Required** Module where to look for types (eg: MyCompany.Domain.dll).
  -n=<ns>... --namespace=<ns>...    Only types in provided namespaces will be visible on diagram as objects.
  -t=<type>... --type=<type>...   Types to include even if not defined in provided namespaces.
  --ignore-namespace=<ign>...       Namespaces to ignore.
  --ignore-type=<igt>               Type to ignore.
  -o=<out> --output=<out>           **Required** File where to put markdown with diagram (will be overridden!).
  --format=<f>                      Markdown format. Supported values: mermaid, markdown [default: mermaid].
  --diagram=<dt>                    Type of diagram to generate [default: class].
```

  Example: 
  ```
  dotnet model generate -m ./MyCompany.AwesomeApp.Domain.dll -o ./AwesomeApp.mmd -n MyCompany.AwesomeApp.Domain
  ```
  
## Installation
  
  ```
  dotnet tool install dotnet-model --global
  ```
  
