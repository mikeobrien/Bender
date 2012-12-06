Bender
=============

Bender is a simple xml de/serialization library for .NET. Unlike the `XmlSerializer` and `DataContractSerializer`, Bender gives you complete control over how values are de/serialized. It is ~%15 faster than the `XmlSerializer`.

Install
------------

Bender can be found on nuget:

    PM> Install-Package Bender

Usage
------------

The serializer and deserializer can be instantialted by passing in an options object:

```csharp
var serializer = new Serializer(new Options {...});

var deserializer = new Deserializer(new Options {...});
```

Or you can use the configuration dsl by calling the static factory method:

```csharp
var serializer = Serializer.Create(x => x.PrettyPrint().ExcludeNullValues());

var deserializer = Deserializer.Create(x => x.ExcludeType<Token>().ExcludeType<Password>());
```

To de/serialize call the respective methods:

```csharp
var model = deserializer.Deserialize<YadaModel>("<yada>...</yada>");

var model = deserializer.Deserialize(typeof(YadaModel), "<yada>...</yada>");

var xml = serializer.Serialize(new YadaModel {...});
```

To overried de/serialization add a reader or writer:

```csharp
var serializer = Serializer.Create(x => x.AddWriter<byte[]>((o, p, v) => Convert.ToBase64String(v)));

var deserializer = Deserializer.Create(x => x.AddReader<byte[]>((o, p, v) => Convert.FromBase64String(v)));
```

The first parameter is the `Options` object, the second paramater is the corresponding `PropertyInfo` and the last parameter is the raw value. Simply return the value you want de/serialized. Note: the `byte[]` reader/writer demonstrated above is automatically added by default so you get that behavior by default.

Some additional notes:

- Bender supports the `XmlTypeAttribute` and `XmlElementAttribute` to override element naming as the `XmlSerializer` does. 
- Bender will de/serialize nullable types and enumerations. 
- Bender de/serializes the following basic types out of the box: `IList<T>`, `String`, `Char`, `Boolean`, `SByte`, `Byte`, `Int16`, `UInt16`, `Int32`, `UInt32`, `Int64`, `UInt64`, `Single`, `Double`, `Decimal`, `DateTime`, `Guid`, `TimeSpan`, `byte[]` (As base64) and `Uri`.
    
Configuration
------------

The following are the common configuration options:

<table>
  <tr>
    <td><code>ExcludeTypes(Func&lt;Type, bool&gt; typeFilter)</code></td>
    <td>Allows you to exclude types based on the predicate passed in. This method is additive so it can be called multiple times.</td>
  </tr>
  <tr>
    <td><code>ExcludeType&lt;T&gt;()</code></td>
    <td>Exclude a particular type.</td>
  </tr>
  <tr>
    <td><code>WithDefaultGenericTypeNameFormat(string typeNameFormat)</code></td>
    <td>This is the format of generic type xml names that haven't been decorated with the `XmlTypeAttribute`. The default is the same as the `XmlSerializer` ([Type]Of[TypeArgs]).</td>
  </tr>
  <tr>
    <td><code>WithDefaultGenericListNameFormat(string listNameFormat)</code></td>
    <td>This is the format of generic list xml names. The default is the same as the `XmlSerializer` (ArrayOf[TypeArgs]).</td>
  </tr>
</table>

The following are the serialization configuration options:

<table>
  <tr>
    <td><code>PrettyPrint()</code></td>
    <td>Indent the xml and make it readable.</td>
  </tr>
  <tr>
    <td><code>ExcludeNullValues()</code></td>
    <td>Do not serialize the elements of properties that are null.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;Options, PropertyInfo, T, string&gt; writter)</code></td>
    <td>Allows you to override how a value is written.</td>
  </tr>
</table>

The following are the deserialization configuration options:

<table>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;Options, PropertyInfo, string, T&gt; reader)</code></td>
    <td>Allows you to override how a value is read.</td>
  </tr>
  <tr>
    <td><code>DefaultNonNullableTypesWhenEmpty()</code></td>
    <td>Set the property to the default value when the element is empty and the type is non nullable.</td>
  </tr>
</table>

Props
------------

Thanks to [JetBrains](http://www.jetbrains.com/) for providing OSS licenses! 