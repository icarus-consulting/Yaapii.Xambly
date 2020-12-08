[![Build status](https://ci.appveyor.com/api/projects/status/ujp18c6psl8um73a/branch/master?svg=true)](https://ci.appveyor.com/project/icarus-consulting/yaapii-xambly/branch/master)
[![codecov](https://codecov.io/gh/icarus-consulting/Yaapii.Xambly/branch/master/graph/badge.svg?token=0Ni6VpsfMN)](https://codecov.io/gh/icarus-consulting/Yaapii.Xambly)

# Yaapii.Xambly

Port of [Xembly Library](https://github.com/yegor256/Xembly) from Yegor Bugayenko.

The following usage guide is taken from the original repository.

# Usage

**Xambly** is an [Assembly](http://en.wikipedia.org/wiki/Assembly_language)-like
[imperative](http://en.wikipedia.org/wiki/Imperative_programming) programming language
for data manipulation in XML documents.
It is a much simplier alternative to
[XSLT](http://www.w3.org/TR/xslt) and [XQuery](http://www.w3.org/TR/xquery).
Read this blog post
for a more detailed explanation: [Xembly, an Assembly for XML](http://www.yegor256.com/2014/04/09/Xembly-intro.html).

Here is a command line implementation (as Ruby gem): [Xembly-gem](https://github.com/yegor256/Xembly-gem)

For example, you have an XML document:

```xml
<orders>
  <order id="553">
    <amount>$45.00</amount>
  </order>
</orders>
```

And you want to change the amount of the order #553
from `$45.00` to `$140.00`. Xambly script would look like:

```
XPATH "orders/order[@id=553]";
SET "$140.00";
```

It is much simpler and compact than
[XSLT](http://www.w3.org/TR/xslt) or [XQuery](http://www.w3.org/TR/xquery).


## Directives

Full list of supported directives in the current version:

  * `ADD`: adds new node to all current nodes
  * `ADDIF`: adds new node, if it's absent
  * `SET`: sets text value of current node
  * `XSET`: sets text value, calculating it with XPath
  * `CDATA`: same as `SET`, but makes `CDATA`
  * `UP`: moves cursor one node up
  * `XPATH`: moves cursor to the nodes found by XPath
  * `REMOVE`: removes all current nodes
  * `STRICT`: throws an exception if cursor is missing nodes
  * `PI`: adds processing instruction
  * `PUSH`: saves cursor in stack
  * `POP`: retrieves cursor from stack
  * `NS`: sets namespace of all current nodes

"Cursor" or "current nodes" is where we're currently located
in the XML document. When Xambly script starts, the cursor is
empty and simply points to the highest level in the XML hierarchy.
Pay attention, it doesn't point to the root node. It points to one
level above the root. Remember, when document is empty, there is no root.

Then, we start executing directives one by one. After each directive
cursor is moving somewhere. There may be many nodes under the cursor,
or just one, or none. For example, let's assume we're starting
with this simple document `<car/>`:

```assembly
ADD 'hello';        // nothing happens, since cursor is empty
XPATH '/car';       // there is one node <car> under the cursor
ADD 'make';         // the result is "<car><make/></car>",
                    // cursor has one node "<make/>"
ATTR 'name', 'BMW'; // the result is "<car><make name='BMW'/></car>"
                    // cursor still has one node "<make/>"
UP;                 // cursor has one node "<car>"
ADD 'mileage';      // the result is "<car><make name='BMW'/><mileage/></car>"
                    // cursor still has one node "<car>"
XPATH '*';          // cursor has two nodes "<make name='BMW'/>"
                    // and "<mileage/>"
REMOVE;             // the result is "<car/>", since all nodes under
                    // the cursor are removed
```

You can create a collection of directives either from text or
via supplementary methods, one per each directive. In both cases,
you need to use class `Directives`:

```java
import org.Xambly.Directives;
new Directives("XPATH '//car'; REMOVE;");
new Directives().xpath("//car").remove();
```

The second option is preferable, because it is faster - there is
no parsing involved.

### ADD

`ADD` directive adds a new node to every node in the current node set.
`ADD` expects exactly one mandatory argument, which is the name of
a new node to be added (case sensitive):

```assembly
ADD 'orders';
ADD 'order';
```

Even if the node with the same name already exists, a new node
will be added. Use `ADDIF` if you need to add only if the same-name node
is absent.

After execution, `ADD` directive moves the cursor to the nodes just added.

### ADDIF

`ADDIF` directive adds a new node to every node of the current set,
only if it's absent. `ADDIF` expects exactly one argument, which
is the name of the node to be added (case sensitive):

```assembly
ADD 'orders';
ADDIF 'order';
```

After execution, `ADDIF` directive moves the cursor to the nodes just added.

### SET

`SET` changes text content of all current nodes, and expects
exactly one argument, which is the text content to set:

```assembly
ADD "employee";
SET "John Smith";
```

`SET` doesn't move the cursor anywhere.

### XSET

`XSET` changes text content of all current nodes to a value
calculated with XPath expression:

```assembly
ADD "product-1";
ADD "price";
XSET "sum(/products/price) div count(/products)";
```

`XSET` doesn't move the cursor anywhere.

### UP

`UP` moves all current nodes to their parents.

### XPATH

`XPATH` changes current nodes to the all found by XPath expression:

```assembly
XPATH "//employee[@id='234' and name='John Smith']/name";
SET "John R. Smith";
```

### REMOVE

`REMOVE` removes current nodes under the cursor and
moves the cursor to their parents:

```assembly
ADD "employee";
REMOVE;
```

### STRICT

`STRICT` checks that there is certain number of current nodes:

```assembly
XPATH "//employee[name='John Doe']";  // move cursor to the employee
STRICT "1";                           // throw an exception if there
                                      // is not exactly one node under
                                      // the cursor
```

This is a very effective mechanism of validation of your script,
in production mode. It is similar to `assert`  statement in Java.
It is recommended to use `STRICT` regularly, to make sure your
cursor has correct amount of nodes, to avoid unexpected modifications.

`STRICT` doesn't move the cursor anywhere.

### PI

`PI` directive add a new processing directive to the XML:

```assembly
PI "xsl-stylesheet" "href='http://example.com'";
```

`PI` doesn't move the cursor anywhere.

### PUSH and POP

`PUSH` and `POP` directives saves current DOM position to stack
and restores it from there.

Let's say you start your Xambly manipulations from a place in DOM,
which location is not determined for you. After your manipulations are
done, you want to get back to exactly the same place. You should
use `PUSH` to save your current location and `POP` to restore it
back, when manipulations are finished, for example:

```assemlby
PUSH;                        // doesn't matter where we are
                             // we just save the location to stack
XPATH '//user[@id="123"]';   // move the cursor to a completely
                             // different location in the XML
ADD 'name';                  // add "<name/>" to all nodes under the cursor
SET 'Jeff';                  // set text value to the nodes
POP;                         // get back to where we were before the PUSH
```

`PUSH` basically saves the cursor into stack and `POP` restores it from there.
This is a very similar technique to `PUSH`/`POP` directives in Assembly. The
stack has no limits, you can push multiple times and pop them back. It is
a stack, that's why it is First-In-Last-Out (FILO).

This operation is fast and it is highly recommended to use it everywhere,
to be sure you're not making unexpected changes to the XML document. Every time
you're not sure where your

### NS

`NS` adds a namespace attribute to a node:

```assembly
XPATH '/garage/car';                // move cursor to "<car/>" node(s)
NS "http://www.w3.org/TR/html4/";   // set namespace there
```

If original document was like this:

```xml
<garage>
  <car>BMW</car>
  <car>Toyota</car>
</garage>
```

After applying that two directives it will look like this:

```xml
<garage xmlns:a="http://www.w3.org/TR/html4/">
  <a:car>BMW</a:car>
  <a:car>Toyota</a:car>
</garage>
```

The namspace prefix may no necessarily be `a:`, but it doesn't
really matter.

`NS` doesn't move the cursor anywhere.

## XML Collections

Let's say you want to build an XML document with a collection
of names:

```java
package org.Xambly.example;
import org.Xambly.Directives;
import org.Xambly.Xambler;
public class XamblyExample {
  public static void main(String[] args) throws Exception {
    String[] names = new String[] {
      "Jeffrey Lebowski",
      "Walter Sobchak",
      "Theodore Donald 'Donny' Kerabatsos",
    };
    Directives directives = new Directives().add("actors");
    for (String name : names) {
      directives.add("actor").set(name).up();
    }
    System.out.println(new Xambler(directives).xml());
  }
}
```

Standard output will contain this text:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<actors>
  <actor>Jeffrey Lebowski</actor>
  <actor>Walter Sobchak</actor>
  <actor>Theodore Donald &apos;Donny&apos; Kerabatsos</actor>
</actors>
```

## Merging Documents

When you need to add an entire XML document, you can convert
it first into Xambly directives and then add them all together:

```java
Iterable<Iterable> dirs = new Directives()
  .add("garage")
  .append(Directives.copyOf(node))
  .add("something-else");
```

This static utility method `copyOf()` converts an instance of class
`org.w3c.dom.Node` into a collection of Xambly directives. Then,
method `append()` adds them all together to the main list.

Unfortunately, not every valid XML document can be parsed by `copyOf()`. For
example, this one will lead to a runtime exception:
`<car>2015<name>BMW</name></car>`. Read more about Xambly limitations,
a few paragraphs below.

## Escaping Invalid XML Text

XML, as standard, doesn't allow certain characters in its body. For example,
this code will throw an exception:

```java
String xml = new Xambler(
  new Directives().add("car").set("\u00")
).xml();
```

Character `\u00` is not allowed in XML. Actually, these ranges
are not allowed: `\u00..\u08`, `\u0B..\u0C`, `\u0E..\u1F`,
`\u7F..\u84`, and `\u86..u9F`.

This means that you should validate everything and make sure you're
setting only "valid" text values to XML nodes. Sometimes, it's not feasible
to always check them. Sometimes you may simply need to save whatever
is possible and call it a day. There a utility static method `Xambler.escape()`, to help
you do that:

```java
String xml = new Xambler(
  new Directives().add("car").set(Xambler.escape("\u00"))
).xml();
```

This code won't throw an exception. Method `Xambler.escape()` will
conver "\u00" to "\\u0000". It is recommended to use this method
everywhere, if you are not sure about the quality of the content.

## Shaded Xambly JAR With Dependencies

Usually, you're supposed to use this dependency in your `pom.xml`:

```xml
<dependency>
  <groupId>com.jcabi.incubator</groupId>
  <artifactId>Xambly</artifactId>
</dependency>
```

However, if you have conflicts between dependencies, you can
use our "shaded" JAR, that includes all dependencies:

```xml
<dependency>
  <groupId>com.jcabi.incubator</groupId>
  <artifactId>Xambly</artifactId>
  <classifier>jar-with-dependencies</classifier>
</dependency>
```

## Known Limitations

Xambly is not intended to be a replacement of XSL or XQuery. It is
a lightweight (!) instrument for XML manipulations. There are a few things
that can't be done by means of Xambly:

  * You can't add, remove, or modify XML comments
    (but you can find them with XPath)

  * DTD section can't be modified

  * Elements and text content can't be mixed, e.g.
    this structure is not supported: `<test>hello <b>friend</a></test>`

Some of these limitations may be removed in the next versions.

## How To Contribute

Fork repository, make changes, send us a pull request. We will review
your changes and apply them to the `master` branch shortly, provided
they don't violate our quality standards. To avoid frustration, before
sending us your pull request, please run full build from powershell:

```
.\build.ps1
```
