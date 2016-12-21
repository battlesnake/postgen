Typical bugs
============


Null lists
----------

Some lists default to null or do not get initailised by constructors in certain cases.
To fix, initialise all internal lists, then use AddRange in the constructor if a parameter is available.
