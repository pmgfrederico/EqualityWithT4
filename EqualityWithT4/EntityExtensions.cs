using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualityWithT4
{
    public static class EntityExtensions
    {
        const string IdentifierArgumentExceptionMessageTemplate = "Please provide a valid {0}! Value: {1}";
        const string NullOrEmptyArgumentExceptionMessageTemplate = "Please provide a valid {0}! Value musn't be NULL or Empty.";

        public static void EnforceIdentifierValid<T>(this IEntity<T> source, T id, string nameOfArgument) where T: struct
        {
            if (id is Guid)
            {
                var guid = Guid.Parse(id.ToString());

                if (guid == Guid.Empty)
                {
                    throw new ArgumentException(string.Format(IdentifierArgumentExceptionMessageTemplate, nameOfArgument, guid));
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static void EnforceNotNullOrEmpty(this string source, string nameOfArgument)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException(string.Format(NullOrEmptyArgumentExceptionMessageTemplate, nameOfArgument));
            }
        }

        public static int GetEntityHashCode<T>(this IEntity<T> source)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("{0}: GetHashCode() for type {1} is {2}", System.Reflection.MethodInfo.GetCurrentMethod().Name, source.GetType().Name, source.Id.GetHashCode());
#endif
            return source.Id.GetHashCode();
        }

        public static bool EntityEquals<T>(this IEntity<T> @this, object that)
        {
            if (@this == null && that == null)
            {
                return true;
            }

            if (@this == null || that == null || ((@this.GetType() != that.GetType())))
            {
                return false;
            }

            return object.Equals(@this.Id, (that as IEntity<T>).Id);
        }
    }
}
