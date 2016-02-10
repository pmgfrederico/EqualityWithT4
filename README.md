# EqualityWithT4
DDD Entity Equality Implementation

DDD Entities are defined in nature by its Identity, typically represented by an Identifier. On most systems this means that proper Equality
implementation relies on equality by value.

This is a great opportunity for code reuse and single shot unit testing.

While at first the use of Remotion.Mixins (https://svn.re-motion.org/svn/Remotion | https://www.nuget.org/packages/Remotion.Mixins/) seemed the way to go, the override of op_Equality and op_Inequality had me considering using a T4 code generation for pseudo-mixin relying on partial classes.

This is a 1st shot at it. Following steps may explore using Visual Studio Item Templates for Domain Specific code bootstrap, or simply having multiple file generation based on type inference. 
