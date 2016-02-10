using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualityWithT4
{
    /// <summary>
    /// An example implementation of Equality using an inheritance scenario
    /// </summary>
    public abstract partial class Person : IEntity<Guid>
    {
        protected Person(Guid id) // This ctor supports entity objects represented solely by their identifiers
        {
            this.EnforceIdentifierValid(id, "id");

            this.Id = id;
        }

        protected Person(string name, Guid? id)
            :this(id ?? Guid.NewGuid()) // This ctor supports entity reconstitution (if the id is supplied) and entity creation if the id is null
        {
            name.EnforceNotNullOrEmpty("name");

            this.Name = name;
        }

        public Guid Id
        {
            get; private set;
        }

        public string Name { get; protected set; }

        object IEntity.GetId()
        {
            return this.Id;
        }
    }

    public abstract partial class Person // Equality implementation
    {
        public override int GetHashCode()
        {
            return this.GetEntityHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.EntityEquals(obj);
        }

        public static bool operator ==(Person @this, Person that)
        {
            return @this.EntityEquals(that);
        }

        public static bool operator !=(Person @this, Person that)
        {
            return !@this.EntityEquals(that);
        }
    }

    /// <summary>
    /// An example implementation of Equality using an inheritance scenario
    /// </summary>
    public partial class Citizen : Person
    {
        public Citizen(Guid id) : base(id)
        {            
        }

        public Citizen(string fiscalNumber, string name, Guid? id)
            : base(name, id)
        {
            fiscalNumber.EnforceNotNullOrEmpty("fiscalNumber");

            this.FiscalNumber = fiscalNumber;
        }

        public string FiscalNumber { get; private set; }
    }

    public partial class Citizen : IEquatable<Citizen>
    {
        public bool Equals(Citizen other)
        {
            return this.EntityEquals(other);
        }
    }
}
