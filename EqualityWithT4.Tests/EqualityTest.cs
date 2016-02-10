using System;
using System.Linq;
using System.Collections;
using NUnit.Framework;
using System.Dynamic;

namespace EqualityWithT4.Tests
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/336aedhh(v=vs.100).aspx      
    /// </summary>
    [TestFixture]
    public class EqualityTest
    {
        public class EntityTypeProvider
        {
            static IEnumerable testCases;

            public static IEnumerable TestCases
            {
                get
                {
                    if (testCases == null)
                    {
                        testCases = typeof(Person)
                            .Assembly
                            .GetTypes()
                            .Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsAbstract)
                            .ToArray();
                    }

                    return testCases;
                }
            }
        }


        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void MustHaveDefaultEntityCtor(Type t)
        {
            if (typeof(IEntity<Guid>).IsAssignableFrom(t))
            {
                Assert.IsTrue(t.GetConstructors()
                    .Where(ctor => ctor.GetParameters()
                        .Any(p => typeof(IEntity<Guid>).GetGenericArguments().Contains(p.ParameterType))).Any());
            }
            else 
            {                
                Assert.Inconclusive();
            }
        }

        /// <summary>
        /// Override the GetHashCode method to allow a type to work correctly in a hash table.
        /// </summary>                
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void MustWorkCorrectlyOnHashTable(Type t)
        {
            var entities = Enumerable.Range(0, 10)
                .Select(i => Activator.CreateInstance(t, Guid.NewGuid()))
                .ToArray();

            var hT = new Hashtable(entities.ToDictionary(k => k, v => v));

            var index = new Random().Next(entities.Length);

            Assert.IsTrue(ReferenceEquals(hT[entities.GetValue(index)], entities.GetValue(index)));
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// x.Equals(x) returns true.
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void XMustBeEqualsToY(Type type)
        {
            var entity = Activator.CreateInstance(type, Guid.NewGuid());

            Assert.IsTrue(entity.Equals(entity));
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// x.Equals(y) returns the same value as y.Equals(x)
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void EqualityMustBeCommutative(Type t)
        {
            var entityX = Activator.CreateInstance(t, Guid.NewGuid());
            var entityY = Activator.CreateInstance(t, Guid.NewGuid());

            var XIsEquatToY = entityX.Equals(entityY);
            var YIsEqualToX = entityY.Equals(entityX);

            Assert.IsTrue(XIsEquatToY == YIsEqualToX);
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// (x.Equals(y) && y.Equals(z)) returns true if and only if x.Equals(z) returns true.
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void EqualityMustBeDistributive(Type t)
        {
            var id = Guid.NewGuid();

            var entityX = Activator.CreateInstance(t, id);
            var entityY = Activator.CreateInstance(t, id);
            var entityZ = Activator.CreateInstance(t, id);

            var XIsEqualToY = entityX.Equals(entityY);
            var YIsEqualToZ = entityY.Equals(entityZ);
            var XIsEqualToZ = entityX.Equals(entityY);

            Assert.IsTrue(XIsEqualToZ == (XIsEqualToY && YIsEqualToZ));
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void EqualityMustBeIdempotent(Type t)
        {
            var id = Guid.NewGuid();

            var entityX = Activator.CreateInstance(t, id);
            var entityY = Activator.CreateInstance(t, id);

            foreach (var i in Enumerable.Range(0, new Random().Next(2, DateTime.UtcNow.Second)))
            {
                Assert.IsTrue(entityX.Equals(entityY));
            }

            entityX = Activator.CreateInstance(t, Guid.NewGuid());
            entityY = Activator.CreateInstance(t, Guid.NewGuid());

            foreach (var i in Enumerable.Range(0, new Random().Next(2, DateTime.UtcNow.Second)))
            {
                Assert.IsFalse(entityX.Equals(entityY));
            }
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void EqualityWithNullMustBeFalse(Type t)
        {
            var id = Guid.NewGuid();

            var entityX = Activator.CreateInstance(t, id);

            Assert.IsFalse(entityX.Equals(default(Citizen)));
        }

        /// <summary>
        /// Follow the contract defined on the Object.Equals Method as follows:
        /// If you are programming in a language that supports operator overloading, and you choose to overload the equality operator (==) for a specified type, that type should override the Equals method. Such implementations of the Equals method should return the same results as the equality operator. Following this guideline will help ensure that class library code using Equals (such as ArrayList and Hashtable) works in a manner that is consistent with the way the equality operator is used by application code.
        /// </summary>
        [Test, TestCaseSource(typeof(EntityTypeProvider), "TestCases")]
        public void EqualityViaOperatorMustReturnTheSameValueAsViaObjectEquals(Type t)
        {
            var id = Guid.NewGuid();

            dynamic entityX = Activator.CreateInstance(t, id);
            dynamic entityY = Activator.CreateInstance(t, id);
            dynamic entityZ = Activator.CreateInstance(t, Guid.NewGuid());

            Assert.IsTrue(entityX.Equals(entityY) == (entityX == entityY));
            Assert.IsTrue(!entityX.Equals(entityZ) == (entityX != entityZ));
            Assert.IsTrue(!entityY.Equals(entityZ) == (entityY != entityZ));
        }
    }
}
