Domain Layer Entities and an Identity that is represented by an Identifier. In our case the contract that stands for an entity typically is IEntity<Guid>.

An Entity typically has 3 states:

-Identified
Instead of working with primitive types we have defined that our API would always use Domain Layer Entities. This means that an entity can be represented by its identifier
from where we can derive the need for a public .ctor(Guid id) to be always present. This enables the use of Generic Factories to create an Identified Entity

-Reconstituted
Entities are in this state immediately after they are retrieved (and therefore already exist) from a data store, typically used specialized Repositories. Ctors must be supplied
to enforce the data contract of the entity.

-Created
The entity is a prototype that is yet to be persisted

The last two states, Reconstituted and Create typically can benefit from a second .ctor that uses a Nullable<Guid> to differentiate Reconstitution from Creation.
e.g. public Genre(string desigantion, Guid? id)


The contract that we are setting with IEntity<Guid> exposes a lot of boiler plate code if we want to consistently implement Equality across our Entities.

While at first we were inclined to use Remotion.Mixins to reuse Equality implementation, this strategy wouldn't allows us to override op_Equality and op_Inequality.

This led to trying T4 code generation together with partial classes, thus bootstrapping code on our entities for a series of competences, namely the presence of the public .ctor(Guid id),
overriding Equals and GetHashCode for consistent use across IDictionary<,> as well as IEquatable<> interface. A side effect that comes as a benefit is that we can provide a single shot
unit tests for Equality that spwans the entire set of Entity types.