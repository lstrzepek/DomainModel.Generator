# DomainModel.Generator
  
[![CI](https://github.com/lstrzepek/DomainModel.Generator/actions/workflows/ci.yml/badge.svg)](https://github.com/lstrzepek/DomainModel.Generator/actions/workflows/ci.yml)

Domain model is a tool to analyze your model (.Net dll)

> You can run it as a part of your build or release pipeline to generate always up to date diagram and attach it to your documentation.
  
## Example 
First you need to build your model, or publish it to have all your external dependencies in same directory. Program will look for dotnet dependencies in `RuntimeEnvironment.GetRuntimeDirectory()`.
```sh
$ dotnet build
```
You can find my test model in [src/DomainModel.Generator.CLI.Tests/TestModel/Model.cs](https://github.com/lstrzepek/DomainModel.Generator/blob/main/src/DomainModel.Generator.CLI.Tests/TestModel/Model.cs)

My model is placed in `DomainModel.Generator.CLI.Tests.dll` but I do not want to have all types in my diagram so I will only choose `TestModel` namespace using -n param. I also do not want to have `Program` type from this namespace so I will exclude only this type using `--ignore-type`.

  ```sh
  $ dotnet model generate -m ./DomainModel.Generator.CLI.Tests.dll -n TestModel --ignore-type TestModel.Program -o ~/Projects/DomainModel.Generate/model.mmd
  ```
> Majority of remote repositories like GitHub and GitLab directly support display of marmaid markdown.

Output:
  
  ```mermaid
  %%{init: {'theme':'forest'}}%%
  classDiagram
	class Sale
	Sale : +List~Guid~ CustomerId

	class Customer
	Customer : +Guid Id
	Customer : +Int32 Age
	Customer : +List~ContactDetails~ Contacts
	Customer : +Address BusinessAddress
	Customer : +DateTime CreateDate
	Customer : +CustomerType Type

	class Address
	Address : +String City
	Address : +String Street

	class CustomerType
	<<enumeration>> CustomerType
	CustomerType : +Individual
	CustomerType : +Company

Sale --> Customer
Customer --> ContactDetails
Customer --> Address
Customer --> CustomerType
  ```

 ## Installation
  
  ```sh
  $ dotnet tool install DomainModel.Generator --global
  ```
  > Because marmaid format is a markdown you can easly find out what was changes using git diff
  
  ## Usage
```
  model generate [--diagram <dt>] (-m <md> | --module <md>) (-o <out> | --output <out>) [--format <f>] [-n <ns>... | --namespace <ns>...] [--ignore-namespace <ign>...] [-t <type>... | --type <type>...] [--ignore-type <igt>...]
  model -h | --help
  model --version

Options:
  -h --help                         Show this screen.
  --version                         Show version.
  -m <md> --module <md>             **Required** Module where to look for types (eg: MyCompany.Domain.dll).
  -n <ns>... --namespace <ns>...    Only types in provided namespaces will be visible on diagram as objects.
  -t <type>... --type <type>...   Types to include even if not defined in provided namespaces.
  --ignore-namespace <ign>...       Namespaces to ignore.
  --ignore-type <igt>               Type to ignore.
  -o <out> --output <out>           **Required** File where to put markdown with diagram (will be overridden!).
  --format <f>                      Markdown format. Supported values: mermaid, markdown [default: mermaid].
  --diagram <dt>                    Type of diagram to generate [default: class].
```
