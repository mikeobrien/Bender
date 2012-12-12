Bender
=============

Bender is a simple xml de/serialization library for .NET. Unlike the `XmlSerializer` and `DataContractSerializer`, Bender gives you complete control over how values are de/serialized. It is ~%15 faster than the `XmlSerializer`.

Install
------------

Bender can be found on nuget:

    PM> Install-Package Bender

Usage
------------

The serializer and deserializer can be instantiated by passing in an options object:

```csharp
var serializer = new Serializer(new Options {...});

var deserializer = new Deserializer(new Options {...});
```

Or you can use the configuration dsl by calling the static factory method:

```csharp
var serializer = Serializer.Create();
var serializer = Serializer.Create(x => x.PrettyPrint().ExcludeNullValues());

var deserializer = Deserializer.Create();
var deserializer = Deserializer.Create(x => x.ExcludeType<Token>().ExcludeType<Password>());
```

To de/serialize, call the respective methods:

```csharp
var model = deserializer.Deserialize<YadaModel>("<yada>...</yada>");
var model = deserializer.Deserialize<YadaModel>(stream);
var model = deserializer.Deserialize<YadaModel>(xdocument);
var model = deserializer.DeserializeFile<YadaModel>(@"d:\files\file.xml");

var model = deserializer.Deserialize(typeof(YadaModel), "<yada>...</yada>");
var model = deserializer.Deserialize(typeof(YadaModel), stream);
var model = deserializer.Deserialize(typeof(YadaModel), xdocument);
var model = deserializer.DeserializeFile(typeof(YadaModel), @"d:\files\file.xml");

string xml = serializer.Serialize(new YadaModel {...});
XDocument document = serializer.SerializeAsDocument(new YadaModel {...});
serializer.Serialize(new YadaModel {...}, stream);
serializer.Serialize(new YadaModel {...}, @"d:\files\file.xml");
```

To override de/serialization add a reader or writer:

```csharp
var serializer = Serializer.Create(x => x.AddWriter<byte[]>((o, p, v) => Convert.ToBase64String(v)));

var deserializer = Deserializer.Create(x => x.AddReader<byte[]>((o, p, v) => Convert.FromBase64String(v.Value)));
```

The first parameter is the `Options` object, the second parameter is the corresponding `PropertyInfo` and the last parameter is either the property value for writers or the `XElement` for readers. Simply return the de/serialized value. Note: the `byte[]` reader/writer shown above is automatically added by default so you get that behavior out of the box.

Bender allows you to override nullable and non-nullable type de/serialization separately if you want to have fine grained control, for example:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<bool>((o, p, v) => v.ToString().ToLower())
    .AddWriter<bool?>((o, p, v) => v.HasValue ? v.Value.ToString().ToLower() ? ""));
```

But most of the time the functionality will be the same for nullable and non nullable readers and writers, save the boilerplate null checking logic. So Bender also allows you to set one reader or writer for both nullable and non-nullable types by passing `true` to the `handleNullable` parameter:

```csharp
var serializer = Serializer.Create(x => x.AddWriter<bool>((o, p, v) => v.ToString().ToLower(), true);
```

Note: the `bool` writer shown above is automatically added by default so you get that behavior out of the box.

Some additional notes:

- Bender supports the `XmlTypeAttribute` and `XmlElementAttribute` to override element naming as the `XmlSerializer` does. 
- Bender supports the `XmlIgnoreAttribute` to ignore properties as the `XmlSerializer` does. 
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
    <td>This is the format of generic xml type element names that haven't been decorated with the <code>XmlTypeAttribute</code>. The default is the same as the <code>XmlSerializer</code> (<code>&lt;[TypeName]Of[GenericTypeArgs]/&gt;</code>).</td>
  </tr>
  <tr>
    <td><code>WithDefaultGenericListNameFormat(string listNameFormat)</code></td>
    <td>This is the format of generic xml list element names. The default is the same as the <code>XmlSerializer</code> (<code>&lt;ArrayOf[GenericTypeArgs]/&gt;</code>).</td>
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
    <td>Allows you to override how a value is serialized.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;Options, PropertyInfo, T, string&gt; writter, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value is serialized.</td>
  </tr>
</table>

The following are the deserialization configuration options:

<table>
  <tr>
    <td><code>DefaultNonNullableTypesWhenEmpty()</code></td>
    <td>Set the property to the default value when the element is empty and the type is non nullable.</td>
  </tr>
  <tr>
    <td><code>IgnoreUnmatchedElements()</code></td>
    <td>Ignore elements in the source xml that don't match properties in the target object. By default an exception is thrown if unmatched elements exist.</td>
  </tr>
  <tr>
    <td><code>IgnoreTypeElementNames()</code></td>
    <td>Ignore type element names in the source xml that don't match the type xml name. This applies specifically to the root element and list elements. In these two cases the element name is based on the type xml name. By default an exception is thrown if the element name does not match the type xml name.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;Options, PropertyInfo, XElement, T&gt; reader)</code></td>
    <td>Allows you to override how a value is deserialized.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;Options, PropertyInfo, XElement, T&gt; reader, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value is deserialized.</td>
  </tr>
</table>

Props
------------

Thanks to [JetBrains](http://www.jetbrains.com/) for providing OSS licenses! 