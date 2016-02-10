

using System;

namespace EqualityWithT4
{
	public partial class PostalCode : IEntity<Guid> // IEntity<TIdentifier> Impl
{
	public Guid Id
	{
		get;
		private set;
	}

	object IEntity.GetId()
	{
		return this.Id;
	}
}

	public partial class PostalCode // Default .ctor Impl
{
	public PostalCode(Guid id) // This ctor supports entity objects represented solely by their identifiers
	{
		this.EnforceIdentifierValid(id, "id");

		this.Id = id;
	}
}
	
	public partial class PostalCode // Equals override
{
	public override int GetHashCode()
	{
		return this.GetEntityHashCode();
	}

	public override bool Equals(object obj)
	{
		return this.EntityEquals(obj);
	}

	public static bool operator ==(PostalCode @this, PostalCode that)
	{
		return @this.EntityEquals(that);
	}

	public static bool operator !=(PostalCode @this, PostalCode that)
	{
		return !@this.EntityEquals(that);
	}
}
	
	public partial class PostalCode : IEquatable<PostalCode> // IEquality<PostalCode> when required
{
	public bool Equals(PostalCode other)
	{
		return this.EntityEquals(other);
	}
}
}