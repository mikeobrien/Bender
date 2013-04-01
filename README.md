Bender
=============

Bender is a simple xml de/serialization library for .NET. Unlike the `XmlSerializer` and `DataContractSerializer`, Bender gives you complete control over how values are de/serialized. Bender is ~%15 faster than the `XmlSerializer`.

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
var model = deserializer.Deserialize<YadaModel>(xelement);
var model = deserializer.DeserializeFile<YadaModel>(@"d:\files\file.xml");

var model = deserializer.Deserialize(typeof(YadaModel), "<yada>...</yada>");
var model = deserializer.Deserialize(typeof(YadaModel), stream);
var model = deserializer.Deserialize(typeof(YadaModel), xdocument);
var model = deserializer.Deserialize(typeof(YadaModel), xelement);
var model = deserializer.DeserializeFile(typeof(YadaModel), @"d:\files\file.xml");

string xml = serializer.Serialize(new YadaModel {...});
XDocument document = serializer.SerializeAsDocument(new YadaModel {...});
serializer.Serialize(new YadaModel {...}, stream);
serializer.Serialize(new YadaModel {...}, @"d:\files\file.xml");
```

To override de/serialization add a reader or writer:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<byte[]>((options, property, value, node) => node.Value = Convert.ToBase64String(value)));

var deserializer = Deserializer.Create(x => x
    .AddReader<byte[]>((options, property, node) => Convert.FromBase64String(node.Value)));
```

For both readers and writers, the first parameter is the Bender `Options` object and the second parameter is the corresponding `PropertyInfo`. 

For **writers**, the last two parameters are the source property value and the target node which references a `XElement` or `XAttribute` (depending on the target node type set in the config). Here you can fully control the final xml by modifying the target `XElement` or `XAttribute` directly via the `Element` and `Attribute` properties of the `Node`. The node type is indicated by the `NodeType` property. In most cases though you will probably just set the value of the target node to the value of the source property, as demonstrated above, via the convenience `Value` property. 

For **readers** the last parameter is the source node which references a `XElement` or `XAttribute` (depending on the source node type) and the deserialized value is returned. At this point you can fully control the deserialization by reading the source `XElement` or `XAttribute` directly via the `Element` and `Attribute` properties of the `Node`. The node type is indicated by the `NodeType` property. In most cases though you will probably just return the value of the source node, as demonstrated above, from the convenience `Value` property`. 

Note: the `byte[]` reader/writer shown above is automatically added by default so you get that behavior out of the box.

Bender allows you to override nullable and non-nullable type de/serialization separately if you want to have fine grained control, for example:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<bool>((options, property, value, node) => node.Value = value.ToString().ToLower())
    .AddWriter<bool?>((options, property, value, node) => node.Value = value.HasValue ? value.Value.ToString().ToLower() ? ""));
```

But most of the time the functionality will be the same for nullable and non nullable readers and writers, save the boilerplate null checking logic. So Bender also allows you to set one reader or writer for both nullable and non-nullable types by passing `true` to the `handleNullable` parameter:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<bool>((options, property, value, node) => node.Value = value.ToString().ToLower(), true);
```

Note: the `bool` writer shown above is automatically added by default so you get that behavior out of the box.

Some additional notes:

- Bender supports the `XmlRootAttribute`, `XmlTypeAttribute`, `XmlElementAttribute`, `XmlAttributeAttribute`, `XmlArrayAttribute` and `XmlArrayItemAttribute` to override element naming as the `XmlSerializer` does. 
- Bender supports the `XmlIgnoreAttribute` to ignore properties as the `XmlSerializer` does. 
- Bender will de/serialize nullable types and enumerations. 
- Bender will pass the parent object to into the constructor of the child object on deserialization if a constructor is defined with a single argument of the parent type.
- Bender de/serializes the following basic types out of the box: `String`, `Char`, `Boolean`, `SByte`, `Byte`, `Int16`, `UInt16`, `Int32`, `UInt32`, `Int64`, `UInt64`, `Single`, `Double`, `Decimal`, `DateTime`, `Guid`, `TimeSpan`, `IEnumerable<T>`, `List<T>`, `IList<T>`, `Array`, `ArrayList` (Serialization only), `IEnumerable` (Serialization only), `byte[]` (As base64), `MailAddress`, `Version`, `IPAddress` and `Uri`.
- Bender will automatically deserialize values in either attributes or elements. By default values are serialized as elements but this can be changed to attributes in configuration.

Bender also includes a helper for building out object graphs. This can be usefull for creating larger object graphs for tests:

```csharp
var model = Expander.Expand<Model>();
```
    
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

The following are the **serialization** configuration options:

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
    <td><code>ValuesAsAttributes()</code></td>
    <td>Specifies that values are serialized as attributes instead of elements.</td>
  </tr>
  <tr>
    <td><code>WithDefaultNamespace(string namespace)</code></td>
    <td>Specifies the default namespace.</td>
  </tr>
  <tr>
    <td><code>AddNamespace(string prefix, string namespace)</code></td>
    <td>Adds a namespace and prefix.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;Options, PropertyInfo, T, Node&gt; writter)</code></td>
    <td>Allows you to override how a value of a specific type is serialized.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;Options, PropertyInfo, T, Node&gt; writter, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value of a specific type is serialized.</td>
  </tr>
  <tr>
    <td><code>AddWriter(Action&lt;Options, PropertyInfo, object, ValueNode&gt; writter)</code></td>
    <td>Allows you to override elements and attributes.</td>
  </tr>
  <tr>
    <td><code>AddWriter(Func&lt;Options, PropertyInfo, object, ValueNode, bool&gt; predicate, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Action&lt;Options, PropertyInfo, object, ValueNode&gt; writter)</code></td>
    <td>Allows you to override elements and attributes that match the specified criteria.</td>
  </tr>
</table>

The following are the **deserialization** configuration options:

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
    <td><code>IgnoreCase()</code></td>
    <td>Ignore the case of the element name when deserializing.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;Options, PropertyInfo, Node, T&gt; reader)</code></td>
    <td>Allows you to override how a value is deserialized.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;Options, PropertyInfo, Node, T&gt; reader, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value is deserialized.</td>
  </tr>
</table>

Props
------------

Thanks to [JetBrains](http://www.jetbrains.com/) for providing OSS licenses! 