Bender [![Build Status](https://travis-ci.org/mikeobrien/Bender.png?branch=master)](https://travis-ci.org/mikeobrien/Bender)
=============

<img src="https://raw.github.com/mikeobrien/Bender/master/misc/logo.png"/> 

Bender is a simple xml and json de/serialization library for .NET. Unlike the `JavaScriptSerializer`, `XmlSerializer` and `DataContractSerializer`, Bender gives you complete control over how values are de/serialized. Bender is ~%10 faster than the `XmlSerializer` and `JavaScriptSerializer`.

Install
------------

Bender can be found on nuget:

    PM> Install-Package Bender

Usage
------------

There are a number of ways to use Bender. The simplest is to use the convenience methods which can be optionally configured, for example:

```csharp
var model = Deserialize.Json<Model>("{ ... }");
var model = Deserialize.Xml(typeof(Model), "<Model>...</Model>");

var json = Serialize.Json(model);
var xml = Serialize.Xml(model, x => x.PrettyPrintXml());
```

The serializer and deserializer can also be created with static factory methods that can be optionally configured, for example:

```csharp
var serializer = Serializer.Create();
var serializer = Serializer.Create(x => x.PrettyPrintXml().ExcludeNullValues());

var deserializer = Deserializer.Create();
var deserializer = Deserializer.Create(x => x.ExcludeType<Token>().ExcludeType<Password>());
```

Finally a serializer and deserializer can also be instantiated directly and an options object can be passed in:

```csharp
var serializer = new Serializer(new Options {...});

var deserializer = new Deserializer(new Options {...});
```

#### Overriding de/serialization

To override de/serialization add a reader or writer:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<byte[]>(x => { if (x.Value != null) x.Node.Value = Convert.ToBase64String(x.Value); });

var deserializer = Deserializer.Create(x => x
    .AddReader<byte[]>(x => Convert.FromBase64String(x.Node.Value));
```

Both readers and writers are passed a context that contains the current `Options`, `PropertyInfo`, source/target value and `ValueNode`. Here you can fully control the reading and rendering of the node. The `Value` property gives you access to the source or target property value. The `Node` property gives you generic `Name` and `Value` access to xml attributes, xml elements and json fields. You can use the `NodeType` property to determine the exact type of node and then access it directly via the `XmlAttribute`, `XmlElement` and `JsonField` properties if you need work with node specific properties. In most cases though you will just return or set the `Value` of the node, as demonstrated above. 

Note: the `byte[]` reader/writer shown above is automatically added by default so you get that behavior out of the box.

Bender allows you to override nullable and non-nullable type de/serialization separately if you want to have fine grained control, for example:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<bool>(x => x.Node.Value = x.Value.ToString().ToLower())
    .AddWriter<bool?>(x => x.Node.Value = x.Value.HasValue ? x.Value.ToString().ToLower() ? ""));
```

But most of the time the functionality will be the same for nullable and non nullable readers and writers, save the boilerplate null checking logic. So Bender also allows you to set one reader or writer for both nullable and non-nullable types by passing `true` to the `handleNullable` parameter:

```csharp
var serializer = Serializer.Create(x => x
    .AddWriter<bool>(x => x.Node.Value = x.Value.ToString().ToLower(), true);
```

Note: the `bool` writer shown above is automatically added by default so you get that behavior out of the box.

Writers can also be used to further process the xml that is produced. Two overloads of `AddWriter`, without the generic type specification, allow you to operate on all elements and attributes. These writers are run after the value writers mentioned above and more than one can be specified. The following example demonstrates how to add a null attribute to elements that have a null value:

```csharp

var serializer = Bender.Serializer.Create(x => x
    .AddXmlNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance")
    .AddWriter(x => x.Node.NodeType == ValueNodeType.Element && x.Value == null,
               x => x.Node.XmlElement.Add(new XAttribute(x.Options.Namespaces["xsi"] + "nil", "true"))));
```

#### Deserialization errors

Errors during deserialization can result from either the source xml or from issues with your code and configuration. In a web service or application the former can likely be addressed by your end users. With that in mind, all Bender deserialization exceptions that are a result of the source xml or json inherit from `SourceException`. This exception has a property called `FriendlyMessage` which can be displayed to end users and give them information to help them resolve the problem. There are three exceptions that inherit from `SourceException`: `SourceParseException`, `UnmatchedNodeException` and `ValueParseException`. An `SourceParseException` occurs when the xml or json is malformed. An `UnmatchedNodeException` occurs when an element or attribute does not match a property in the target type (This behavior can be configured in the deserialization options). And finally a `ValueParseException` occurs when a simple type cannot be parsed because it is not formatted properly. Bender has default friendly messages for simple types and these can be overriden by calling the `WithFriendlyParseErrorMessage<T>(string message)` method in the deserialization options.

When creating your own custom readers you can make use of friendly error messages by specifying one for the type your reader handles:

```csharp
var deserializer = Deserializer.Create(x => x
  .WithFriendlyParseErrorMessage<IPAddress>("Not formatted correctly, must be formatted as '1.2.3.4'.")
  .AddReader<IPAddress>(x => IPAddress.Parse(n.Value));
```

Any errors resulting from the reader will be wrapped in a `ValueParseException` with the friendly error specified.

#### Miscellania

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
    <td><code>WithDefaultGenericTypeXmlNameFormat(string typeNameFormat)</code></td>
    <td>This is the format of generic xml type element names that haven't been decorated with the <code>XmlTypeAttribute</code>. The default is the same as the <code>XmlSerializer</code> (<code>&lt;[TypeName]Of[GenericTypeArgs]/&gt;</code>).</td>
  </tr>
  <tr>
    <td><code>WithDefaultGenericListXmlNameFormat(string listNameFormat)</code></td>
    <td>This is the format of generic xml list element names. The default is the same as the <code>XmlSerializer</code> (<code>&lt;ArrayOf[GenericTypeArgs]/&gt;</code>).</td>
  </tr>
</table>

The following are the **serialization** configuration options:

<table>
  <tr>
    <td><code>PrettyPrintXml()</code></td>
    <td>Indent the xml and make it readable.</td>
  </tr>
  <tr>
    <td><code>ExcludeNullValues()</code></td>
    <td>Do not serialize the nodes of properties that are null.</td>
  </tr>
  <tr>
    <td><code>XmlValuesAsAttributes()</code></td>
    <td>Specifies that values are serialized as xml attributes instead of elements.</td>
  </tr>
  <tr>
    <td><code>WithDefaultXmlNamespace(string namespace)</code></td>
    <td>Specifies the default xml namespace.</td>
  </tr>
  <tr>
    <td><code>AddXmlNamespace(string prefix, string namespace)</code></td>
    <td>Adds an xml namespace and prefix.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;WriterContext&lt;T&gt;&gt; writter)</code></td>
    <td>Allows you to override how a value of a specific type is serialized.</td>
  </tr>
  <tr>
    <td><code>AddWriter&lt;T&gt;(Func&lt;WriterContext&lt;T&gt;&gt; writter, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value of a specific type is serialized.</td>
  </tr>
  <tr>
    <td><code>AddWriter(Action&lt;WriterContext&gt; writter)</code></td>
    <td>Allows you to override xml elements, xml attributes and json fields.</td>
  </tr>
  <tr>
    <td><code>AddWriter(Func&lt;WriterContext, bool&gt; predicate, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Action&lt;WriterContext&gt; writter)</code></td>
    <td>Allows you to override xml elements, xml attributes and json fields that match the specified criteria.</td>
  </tr>
</table>

The following are the **deserialization** configuration options:

<table>
  <tr>
    <td><code>DefaultNonNullableTypesWhenEmpty()</code></td>
    <td>Set the property to the default value when the element is empty and the type is non nullable.</td>
  </tr>
  <tr>
    <td><code>IgnoreUnmatchedNodes()</code></td>
    <td>Ignore elements in the source xml or json that don't match properties in the target object. By default an exception is thrown if unmatched nodes exist.</td>
  </tr>
  <tr>
    <td><code>IgnoreTypeXmlElementNames()</code></td>
    <td>Ignore type xml element names in the source xml that don't match the type xml name. This applies specifically to the root element and list elements. In these two cases the element name is based on the type xml name. By default an exception is thrown if the element name does not match the type xml name.</td>
  </tr>
  <tr>
    <td><code>IgnoreCase()</code></td>
    <td>Ignore the case of the node name when deserializing.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;ReaderContext, T&gt; reader)</code></td>
    <td>Allows you to override how a value is deserialized.</td>
  </tr>
  <tr>
    <td><code>AddReader&lt;T&gt;(Func&lt;ReaderContext, T&gt; reader, <br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;bool handleNullable) where T : struct</code></td>
    <td>Allows you to override how both the nullable and non-nullable value is deserialized.</td>
  </tr>
  <tr>
    <td><code>WithFriendlyParseErrorMessage&lt;T&gt;(string message)</code></td>
    <td>Allows you to override friendly error messages returned when a value cannot be parsed.</td>
  </tr>
</table>

Props
------------

Thanks to [JetBrains](http://www.jetbrains.com/) for providing OSS licenses! 