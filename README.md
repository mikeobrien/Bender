[![Nuget](http://img.shields.io/nuget/v/Bender.svg?style=flat)](http://www.nuget.org/packages/Bender/) [![Nuget downloads](http://img.shields.io/nuget/dt/Bender.svg?style=flat)](http://www.nuget.org/packages/Bender/) [![Travis CI Build Status](http://img.shields.io/travis/mikeobrien/Bender.svg?style=flat&label=Travis%20CI)](https://travis-ci.org/mikeobrien/Bender) [![TeamCity Build Status](https://img.shields.io/teamcity/http/build.mikeobrien.net/s/bender.svg?style=flat&label=TeamCity)](http://build.mikeobrien.net/viewType.html?buildTypeId=bender&guest=1)

<img src="https://raw.github.com/mikeobrien/Bender/master/misc/logo.png"/> 

Bender is a highly configurable xml and json serialization library for .NET. Unlike other serializers, Bender gives you complete control over the serialization process though a simple API.

- Not strongly named.
- One serializer for both json and XML.
- [Readers](#readers), [writers](#writers) and [visitors](#visitors) give you full control of serialization and deserialization.
- [Naming conventions](#naming-conventions) give you full control over naming.
- [Friendly deserialization error messages](#friendly-deserialization-error-messages) allow you to give valuable feedback to consumers of your API.
- Supports enums, nullable types, `String`, `Char`, `Boolean`, `SByte`, `Byte`, `Int16`, `UInt16`, `Int32`, `UInt32`, `Int64`, `UInt64`, `Single`, `Double`, `Decimal`, `DateTime`, `Guid`, `TimeSpan`, `Uri`, `IntPtr`, `UIntPtr`, `Array`, `ArrayList` (Serialization only), `IEnumerable` (Serialization only), `IEnumerable<T>`,  `IDictionary` (Serialization only), `IDictionary<TKey, TValue>`, `byte[]` (As Base64), `MailAddress`, `Version`, `IPAddress` out of the box.
- Supports `XmlRootAttribute`, `XmlTypeAttribute`, `XmlElementAttribute`, `XmlAttributeAttribute`, `XmlArrayAttribute`, `XmlArrayItemAttribute` and `XmlIgnoreAttribute` attributes.
- Extensibility point to override object creation during deserialization with your own implementation.
- Filtering options for types, members and visibility.
- Options for name comparison and ignoring names of certain elements altogether.
- Options for failing on unmatched members or elements.
- Ability to serialize and deserialize members of enumerable and dictionary implementations.
- Supports form url encoded.

## Install

Bender can be found on nuget:

    PM> Install-Package Bender

## Basic Usage

The `Serialize` and `Deserialize` convenience methods allow you to easily serialize or deserialize:

```csharp
public class Model
{
    public string Property { get; set; } 
}

var model = Deserialize.Json<Model>("{\"Property\":\"value\"}");
var model = Deserialize.Xml<Model>("<Model><Property>value</Property></Model>");

var json = Serialize.Json(new Model { Property = "value" });
var xml = Serialize.Xml(new Model { Property = "value" });
```

Additional convenience methods take or return a `byte[]`, `Stream`, file path, `XDocument`, `XElement` and `INode`. 

## Configuration

Bender can be configured in a number of different ways depending on your needs.  First, the configuration DSL is available on all convenience and factory methods:

```csharp
var model = Deserialize.Json<Model>("{ ... }", x => { ... });
var serializer = Serializer.Create(x => { ... });
var deserializer = Deserializer.Create(x => { ... });
```

Second, you can create options with the options factory method and pass those in later:

```csharp
var options = Options.Create(x => { ... });

var model = Deserialize.Json<Model>("{ ... }", options);
var serializer = new Serializer(options);
var deserializer = new Deserializer(options);
```

### Common Configuration

| Method | Description |
| ------------- | ------------- |
| `WithGenericTypeNameFormat` | Naming convention for generic types. The default is `{0}Of{1}`. The first parameter is the type name and the second is the type parameter(s). |
| `WithEnumerableTypeNameFormat` | Naming convention for enumerable types. The default is `ArrayOf{0}`. The parameter is the item type name. |
| `WithDictionaryTypeNameFormat` | Naming convention for dictionaries. The default is ``DictionaryOf{0}`. The parameter is the value type name.` |
| `WithDefaultItemTypeName` | Naming convention for non generic enumerable and dictionary type items. The default is `AnyType`. |
| `TreatEnumerableImplsAsObjects` | Indicates whether types implementing `IEnumerable` are treated as objects, where members are traversed, or enumerables where items are traversed. |
| `TreatDictionaryImplsAsObjects` | Indicates whether types implementing `IDictionary` or `IDictionary<TKey, TValue>` are treated as objects, where members are traversed, or dictionaries where key/value pairs are traversed. |
| `IncludeNonPublicProperties` | Indicates that non public properties should also be traversed. By default, non public properties are ignored. |
| `IncludePublicFields` | Indicates that public fields should also be traversed. By default, fields are ignored. |
| `IncludeNonPublicFields` | Indicates that non public fields should also be traversed. By default, fields are ignored. |
| `IncludeTypesWhen` | Predicate that determines what types should be included. Multiple predicates can be specified and are additive. By default all types are included. |
| `ExcludeTypesWhen` | Predicate that determines what types should be excluded. Multiple predicates can be specified and are additive.  |
| `ExcludeType<T>` | Specifies a type that should be excluded. Multiple types can be specified and are additive.  |
| `IncludeMembersWhen` | Predicate that determines what properties and fields should be included. Multiple predicates can be specified and are additive. By default all public properties are included. |
| `ExcludeMembersWhen` | Predicate that determines what properties and fields should be excluded. Multiple predicates can be specified and are additive. |
| `UseSnakeCaseNaming` | Deserializes from and serializes to `snake_case`. |
| `UseCamelCaseNaming` | Deserializes from and serializes to `camelCase`. |
| `UseJsonCamelCaseNaming` | Deserializes from and serializes to `camelCase` only for json. |
| `UseXmlSpinalCaseNaming` | Deserializes from and serializes to `spinal-case` only for xml. |
| `UseXmlTrainCaseNaming` | Deserializes from and serializes to `Train-Case` only for xml. |
| `WithMemberNamingConvention` | Function that returns the name of a field or property. Multiple conventions can be added to form a pipeline. |
| `WithFieldNamingConvention` | Function that returns the name of a field. Multiple conventions can be added to form a pipeline. |
| `WithPropertyNamingConvention` | Function that returns the name of a property. Multiple conventions can be added to form a pipeline. |
| `WithArrayItemNamingConvention` | Function that returns the name of an array item. Multiple conventions can be added to form a pipeline. |
| `WithTypeNamingConvention` | Function that returns the name of a type. Multiple conventions can be added to form a pipeline. |
| `WithNamingConvention` | Function that returns the name of fields, properties, array items and types. Multiple conventions can be added to form a pipeline. |

### Serialization Configuration

| Method | Description |
| ------------- | ------------- |
| `PrettyPrint` | Includes whitespace in the output. |
| `UseActualType` | Indicates whether to serialize the actual type as opposed to the cast type. The cast type is used by default. |
| `WriteDateTimeAsUtcIso8601` | Serialize `DateTime`'s as [ISO8601](http://en.wikipedia.org/wiki/ISO_8601). |
| `WriteMicrosoftJsonDateTime` | Serialize `DateTime`'s as the Microsoft date format for json only (e.g. `/Date(1330848000000)/`). |
| `XmlValuesAsAttributes` | Serialize values as XML attributes instead of elements. |
| `WithDefaultXmlNamespace` | Allows you to specify the default XML namespace. |
| `AddXmlNamespace` | Allows you to add an XML namespace. |
| `OmitXmlDeclaration` | Indicates that you want to omit the XML declaration. |
| `AsSimpleType<T>` | Treat an object as a simple type, calling `ToString()` for the value. |
| `AddVisitor` | Adds node visitor. |
| `AddJsonVisitor` | Adds node visitor for json only. |
| `AddXmlVisitor` | Adds node visitor for xml only. |
| `AddVisitor<T>` | Adds node visitor for the specified type. |
| `AddJsonVisitor<T>` | Adds node visitor for the specified type for json only. |
| `AddXmlVisitor<T>` | Adds node visitor for the specified type for xml only. |
| `AddWriter` | Adds a writer. |
| `AddWriter<T>` | Adds a writer for the specified type. |

### Deserialization Configuration

| Method | Description |
| ------------- | ------------- |
| `WithObjectFactory` | Allows you to specify a factory for creating objects. Useful if you would like to hydrate objects with an IoC container.  |
| `IgnoreNameCase` | Ignores name casing. |
| `WithNameComparison` | Allows you to set the specific `StringComparison` for name matching. |
| `IgnoreEnumNameCase` | Ignores `enum` value casing. |
| `WithEnumNameComparison` | Allows you to set the specific `StringComparison` for `enum` value matching. |
| `IgnoreXmlAttributes` | Ignores XML attributes. |
| `IgnoreRootName` | Does not attempt to match the root element name. |
| `IgnoreArrayItemNames` | Does not attempt to match array item element names. |
| `FailOnUnmatchedElements` | Fail if an element exists but not its corresponding member. |
| `FailOnUnmatchedMembers` | Fail if a member exists but not its corresponding element. |
| `TreatDatesAsUtcAndConvertToLocal` | Treat dates as local but serialize as UTC. |
| `WithFriendlyParseErrorMessage<T>` | Allows you to specify friendly parse error messages for specific types. |
| `AddVisitor` | Adds node visitor. |
| `AddJsonVisitor` | Adds node visitor for json only. |
| `AddXmlVisitor` | Adds node visitor for xml only. |
| `AddVisitor<T>` | Adds node visitor for the specified type. |
| `AddJsonVisitor<T>` | Adds node visitor for the specified type for json only. |
| `AddXmlVisitor<T>` | Adds node visitor for the specified type for xml only. |
| `AddReader` | Adds a reader. |
| `AddReader<T>` | Adds a reader for the specified type. |

## Overriding Serialization and Deserialization

Bender has a number of extensibility points. The following covers these in more detail.

### Naming Conventions

Bender ships with some common naming conventions out of the box: `camelCase`,  `snake_case`, `spinal-case` and `Train-Case`. You can add your own naming conventions with the `With*NamingConvention()` options. There are options to override naming for fields, properties, members (fields and properties), array items, types or all of the above. Each option method has an overload that applies to all and one that takes a predicate to limit its application. Naming conventions form a pipeline where a name is modified by one convention and then passed to the next convention to be modified further. This allows you to define multiple conventions that work together. The order that the conventions are defined is the order they will execute.

Lets take a look at an arbitrary example where we create a couple of naming conventions.

```csharp
Options.Create(x => x
    .WithPropertyNamingConvention(
        (name, member) => name + "EmailAddress",
        (name, member) => member.Type == typeof(MailAddress))
    .WithNamingConvention(name => "_" + name));
```

In the first convention we append `EmailAddress` to the name when the property is of type `MailAddress`. Next we define a convention that prepends an `_` to every name. If we had a property named `Support` of type `MailAddress` the name would end up being `_SupportEmailAddress`.

### Visitors

Visitors enable you to operate on each node in the target graph. Both serialization and deserialization options contain methods for adding visitors. Each option method has an overload that applies to all and one that takes a predicate to limit its application. There are also convenience methods that apply visitors to either xml or json or specific types.

Lets take a look at an xml visitor for serialization that adds a `nil` attribute when a value is null.

```csharp
Options.Create(x => x
    .Serialization(s => s
        .AddXmlNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance")
        .AddXmlVisitor(
            (source, target, options) => 
                target.Element.Add(new XAttribute("xsi:nil", "true")),
            (source, target, options) => source.NodeType.IsValue() && 
                         source.Value == null)));
```

### Readers

Readers read values from a source like xml or json during deserialization. Each option method has an overload that applies to all and one that takes a predicate to limit its application. There are also convenience methods that apply readers to specific types.

Lets look at an example where we read an IP address.

```csharp
Options.Create(x => x
    .Deserialization(s => s
        .AddReader((value, source, target, options) => 
            IPAddress.Parse(value.ToString()))));
```

You can also define readers for nullable and non-nullable types separately if you want to have fine grained control, for example if you wanted to override how `bool`'s are parsed:

```csharp
Options.Create(x => x
    .Deserialization(s => s
        .AddReader<bool>(
            (value, source, target, options) => bool.Parse(value.ToString()))
        .AddReader<bool?>(
            (value, source, target, options) => 
                !string.IsNullOrEmpty(value.ToString()) ? 
                    (bool?)bool.Parse(value.ToString()) : null)));
```
    
But most of the time the functionality will be the same for nullable and non nullable readers and writers, save the boilerplate null checking logic. So you can define one writer for both nullable and non-nullable types by passing true to the `handleNullable` parameter:

```csharp
Options.Create(x => x
    .Deserialization(s => s
        .AddReader<bool>(
            (value, source, target, options) => 
                bool.Parse(value.ToString()), true)));
```

### Writers

Writers write values to a target like xml or json during serialization. Each option method has an overload that applies to all and one that takes a predicate to limit its application. There are also convenience methods that apply readers to specific types and indicate if nullable types are included.

Lets look at an example where we write an IP address.

```csharp
Options.Create(x => x
    .Serialization(s => s
        .AddWriter<IPAddress>((value, source, target, options) => 
            value.ToString())));
```

## Friendly Deserialization Error Messages

Errors during deserialization can result from issues with the source or with your code and configuration. In an API consumed by others, the former can likely be addressed by your end users if they have good feedback. With that in mind, all Bender deserialization exceptions that are caused by a problem with the source inherit from `FriendlyBenderException`. This exception has a property named `FriendlyMessage` that can be displayed to end users and give them information to help them resolve the problem. Unlike the raw error messages, no internal information is included. 

Bender also has default friendly messages for parsing simple types and these can be overridden by calling the `WithFriendlyParseErrorMessage<T>(string message)` method in the deserialization options. Also, when creating readers you can make use of friendly parse error messages by specifying one for the type your reader handles:

```csharp
var deserializer = Deserializer.Create(x => x
    .Deserialization(d => d
        .WithFriendlyParseErrorMessage<IPAddress>(
            "IP addresss not formatted correctly, must be formatted as '1.2.3.4'.")
        .AddReader((v, s, t, o) => IPAddress.Parse(v.ToString()))));
```
  
Any errors resulting from the reader will be wrapped in a `FriendlyBenderException` with the friendly error specified.

## Performance

Bender has traded some performance for flexibility so does not match the speed of other serializers. It averages ~.05 ms/KB (2.8 GHz i7) so would be more than enough for most applications. But if raw speed is a requirement you might want to look at the [ServiceStack serializers](https://github.com/ServiceStackV3/mythz_blog/blob/master/pages/344.md) or [fastJSON](https://fastjson.codeplex.com/).

## License

[MIT License](https://raw.githubusercontent.com/mikeobrien/Bender/master/LICENSE).

