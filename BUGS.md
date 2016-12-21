Typical bugs
============


Untyped formatter
-----------------

Since our formatter/templater is untyped, we sometimes pass containers and arrays/lists by mistake.
To fix, create generic subclasses of Template which take N type parameters, and which strictly-type the index operator's arguments.


Null lists
----------

Some lists default to null or do not get initailised by constructors in certain cases.
To fix, initialise all internal lists, then use AddRange in the constructor if a parameter is available.
