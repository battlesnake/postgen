# PostGen

> MongoDB is like /dev/null except it occasionally stores data

## Language

Classes mapping to SQL language for defining tables and indexes.
Includes some Postgres extensions.


## CodeGenerator

Classes which transform Language objects into runnable SQL code.
Different generators may target different database engines.
Personally, I only care about implementing for Postgres.


## Demo

Demonstrations of various generators and other functionality, used for manual testing during development.


## Paradigm

Classes I can use for designing my data structures, in a way which intuitively maps to how I want to use them.
These classes can then genrerate Language classes, representing the way SQL/Postgres likes to think about data structures.

Example:

 * Log/Latest tables, supporting "write", "read_range", "read_latest".

 * Proxy functions, useful for adding DEFINER wrappers around restricted functions, to verify access rights before calling.

 * Views for JOIN tables and INSERT/UPDATE helper functions, giving appearance of OO-style inheritance and type-polymorphism over conditional joins.


# Why?

A big difference between this and other ORMs is that this will never actually become an ORM.
It is focussed on improving the quality and consistency of, and reducing the development/testing time of procedural programming in the database.
The fact that functions defined using Language.Function can be used to generate strictly-typed SQL-calling wrappers (e.g. for prepared statements) in other programming languages is a very nice bonus.


 — Mark K Cowan
