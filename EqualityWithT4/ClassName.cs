using System;

namespace EqualityWithT4
{
    /// <summary>
    /// ClassName is a PoC while separating different competences using partial classes
    /// </summary>
    public partial class ClassName
    { 
    }

    public partial class ClassName : IEntity<Guid> // IEntity<Guid> Impl
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

    public partial class ClassName // Default .ctor Impl
    {
        public ClassName(Guid id) // This ctor supports entity objects represented solely by their identifiers
        {
            this.EnforceIdentifierValid(id, "id");

            this.Id = id;
        }
    }

    public partial class ClassName // Equals override
    {
        public override int GetHashCode()
        {
            return this.GetEntityHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.EntityEquals(obj);
        }

        public static bool operator ==(ClassName @this, ClassName that)
        {
            return @this.EntityEquals(that);
        }

        public static bool operator !=(ClassName @this, ClassName that)
        {
            return !@this.EntityEquals(that);
        }
    }

    public partial class ClassName : IEquatable<ClassName> // IEquality<ClassName> when required
    {
        public bool Equals(ClassName other)
        {
            return this.EntityEquals(other);
        }
    }
}
